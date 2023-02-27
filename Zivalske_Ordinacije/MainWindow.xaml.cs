using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zivalske_Ordinacije
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string host = "ep-dawn-rain-830060.eu-central-1.aws.neon.tech";
        public string db = "neondb";
        public string user = "gasperflorjan5";
        public string pass = "l7fQcR5kvPZM";
        NpgsqlConnection con = new NpgsqlConnection("Host=ep-dawn-rain-830060.eu-central-1.aws.neon.tech;" +
            " Port=5432; User Id=gasperflorjan5;Password=l7fQcR5kvPZM;Database=neondb");
        public MainWindow()
        {
            InitializeComponent();
        }

        public void RefreshList()
        {
            ListView listView1 = new ListView();
            listView1.Items.Clear();
            using (var cmd = new NpgsqlCommand("SELECT * FROM IzpisOrdinacije()", con))
            {
                var reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                GridOrdinacije.DataContext = dataTable;
            }
        }

        private void Ordinacije_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            GridOrdinacije.Visibility = Visibility.Visible;
            con.Open();
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM kraji()", con))
            {
                using (NpgsqlDataReader read = command.ExecuteReader())
                {
                    while (read.Read())
                    {
                        string name = read.GetString(0);
                        Kraji.Items.Add(name);
                    }
                }
            }
            using (var cmd = new NpgsqlCommand("SELECT * FROM IzpisOrdinacije()", con))
            {
                var reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                GridOrdinacije.DataContext = dataTable; 
            }
            con.Close();
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (ord_ime.Text == "" || ord_naslov.Text == "" || ord_mail.Text == "" || Kraji.SelectedItem == null)
            {
                MessageBox.Show("Can't add ordinacija without all information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                con.Open();

                string insert = "SELECT VnosOrdinacij('" + ord_ime.Text + "','" + ord_naslov.Text + "','" + ord_mail.Text + "','" + Kraji.SelectedItem.ToString() + "')";
                NpgsqlCommand cmmd = new NpgsqlCommand(insert, con);
                cmmd.ExecuteNonQuery();

                RefreshList();

                con.Close();
                ord_ime.Text = "";
                ord_naslov.Text = "";
                ord_mail.Text = "";
                Kraji.SelectedItem = null;
            }
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                try
                {
                    var selectedItem = listView.SelectedItems[0] as ListViewItem;
                    if (selectedItem != null && selectedItem.Content is DataRowView rowView)
                    {
                        var subItem = rowView.Row[1].ToString();
                        ord_ime.Text = subItem;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)listView.SelectedItem;
            int o_id = Convert.ToInt32(row[0]);

            con.Open();
            string delete = "SELECT DeleteOrdinacije('" + o_id + "')";
            NpgsqlCommand cmd = new NpgsqlCommand(delete, con);
            cmd.ExecuteNonQuery();
            using (var cmmd = new NpgsqlCommand("SELECT * FROM IzpisOrdinacije()", con))
            {
                var reader = cmmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                GridOrdinacije.DataContext = dataTable;
            }
            con.Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            GridOrdinacije.Visibility = Visibility.Hidden;
            MainGrid.Visibility = Visibility.Visible;
        }

        private void Veterinarji_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            GridVet.Visibility = Visibility.Visible;
            con.Open();
            using (var cmd = new NpgsqlCommand("SELECT * FROM IzpisVeterinarjev()", con))
            {
                var reader = cmd.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                GridVet.DataContext = dataTable;
            }
            con.Close();
        }
    }
}

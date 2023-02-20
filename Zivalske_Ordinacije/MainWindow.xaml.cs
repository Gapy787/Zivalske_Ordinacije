using Npgsql;
using System;
using System.Collections.Generic;
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

        private void Ordinacije_Click(object sender, RoutedEventArgs e)
        {
            MainGrid.Visibility = Visibility.Hidden;
            GridOrdinacije.Visibility = Visibility.Visible;
            con.Open();

            using (var cmd = new NpgsqlCommand("SELECT * FROM IzpisOrdinacije()", con))
            {
                var reader = cmd.ExecuteReader();
                GridOrdinacije.DataContext = reader;
            }

            con.Close();
        }

        
       
    }
}

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
using System.Windows.Shapes;
using Npgsql;

namespace Zivalske_Ordinacije
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        public string host = "ep-dawn-rain-830060.eu-central-1.aws.neon.tech";
        public string db = "neondb";
        public string user = "gasperflorjan5";
        public string pass = "l7fQcR5kvPZM";
        NpgsqlConnection con = new NpgsqlConnection("Host=ep-dawn-rain-830060.eu-central-1.aws.neon.tech; Port=5432; User Id=gasperflorjan5;Password=l7fQcR5kvPZM;Database=neondb");
        private void Login1_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            string statement = "SELECT Login('" + username.Text + "' , '" + password.Text + "')";

            NpgsqlCommand cmd = new NpgsqlCommand(statement,con);
            string preveri_log = cmd.ExecuteScalar().ToString();
            if (preveri_log.Equals("False"))
            {
                MessageBox.Show("Neuspešen Login");
            }
            else
            {
                MessageBox.Show("Uspešen Login");
            }
            con.Close();
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridLogin.Visibility= Visibility.Hidden;
            GridRegistracija.Visibility= Visibility.Visible;
        }

        private void Registracija_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            string statement = "SELECT Register('" + r_username.Text + "' , '" + r_password.Text + "')";

            NpgsqlCommand cmd = new NpgsqlCommand(statement, con);
            string preveri_reg = cmd.ExecuteScalar().ToString();
            if (preveri_reg.Equals("False"))
            {
                MessageBox.Show("Neuspešna registracija poskusite znova");
            }
            else
            {
                MessageBox.Show("Uspešen Registracija");
                GridRegistracija.Visibility= Visibility.Hidden;
                GridLogin.Visibility= Visibility.Visible;
            }
            con.Close();
        }
    }
}

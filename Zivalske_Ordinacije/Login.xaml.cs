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

            string statement = "SELECT Login('" + l_username.Text + "' , '" + l_password.Password.ToString() + "')";

            NpgsqlCommand cmd = new NpgsqlCommand(statement,con);
            string preveri_log = cmd.ExecuteScalar().ToString();
            if (preveri_log.Equals("False"))
            {
                MessageBox.Show("Login failed");
                l_username.Text = "";
                l_password.Password = null;
            }
            else
            {
                MessageBox.Show("Successful login");
                
                MainWindow newWindow = new MainWindow();
                newWindow.Show();
                this.Close();
            }


            con.Close();
        }

        private void label1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridLogin.Visibility= Visibility.Hidden;
            GridRegistracija.Visibility= Visibility.Visible;
            r_username.Text = "";
            r_password.Password = null;
            r_confirmpassword.Password = null;
        }

        private void Registracija_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            string statement = "SELECT RegisterPreveri('" + r_username.Text + "' , '" + r_password.Password.ToString() + "')";

            NpgsqlCommand cmd = new NpgsqlCommand(statement, con);
            string preveri_reg = cmd.ExecuteScalar().ToString();

            string state = "SELECT UsernameExists('" + r_username.Text + "')";
            NpgsqlCommand cmmd = new NpgsqlCommand(state, con);
            string preveri_username = cmmd.ExecuteScalar().ToString();
            if (r_confirmpassword.Password == r_password.Password)
            {
                if (preveri_username.Equals("False"))
                {
                    if (preveri_reg.Equals("True"))
                    {
                        MessageBox.Show("Registration failed");
                        r_username.Text = "";
                        r_password.Password = null;
                        r_confirmpassword= null;
                    }
                    else
                    {
                        string insert = "SELECT VnesiUporabnika('" + r_username.Text + "','" + r_password.Password.ToString() + "')";
                        NpgsqlCommand command = new NpgsqlCommand(insert, con);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Successful registration");
                        GridRegistracija.Visibility = Visibility.Hidden;
                        GridLogin.Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    MessageBox.Show("Username already exists");
                    r_username.Text = "";
                    r_password.Password = null;
                    r_confirmpassword.Password = null;
                }
            }
            else
            {
                MessageBox.Show("Passwords don't match.");
               
                r_password.Password = null;
                r_confirmpassword.Password = null;
            }

            con.Close();

        }

        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            r_password.PasswordChar = '\0';
            r_confirmpassword.PasswordChar = '\0';
        }

        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            r_password.PasswordChar = '*';
            r_confirmpassword.PasswordChar = '*';
        }

        private void HaveAcc_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridLogin.Visibility = Visibility.Visible;
            GridRegistracija.Visibility = Visibility.Hidden;    
            l_username.Text = "";
            l_password.Password = null;
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GridLogin.Visibility = Visibility.Hidden;
            SpremeniPasswordGrid.Visibility = Visibility.Visible;
        }

        private void User_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            string state = "SELECT UsernameExists('" + cp_username.Text + "')";
            NpgsqlCommand cmmd = new NpgsqlCommand(state, con);
            string preveri_username = cmmd.ExecuteScalar().ToString();

            if (preveri_username.Equals("True"))
            {
                SpremeniPasswordGrid.Visibility = Visibility.Hidden;
                ChangePasswordGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("This user doesn't exist");
                cp_username.Text = "";
            }
            con.Close();
        }


        private void cp_username_GotFocus(object sender, RoutedEventArgs e)
        {
            if (cp_username.Text == "Enter your username")
            {
                cp_username.Text = "";
            }
        }

        private void cp_username_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cp_username.Text))
            {
                cp_username.Text = "Enter your username";
            }
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            con.Open();

            string preveri = "SELECT PreveriPass('" + cp_username.Text + "','" + old_password.Password.ToString() + "')";
            NpgsqlCommand command = new NpgsqlCommand(preveri, con);
            string preveri_pass = command.ExecuteScalar().ToString();
            if (preveri_pass.Equals("True"))
            {
                string state = "SELECT ChangePasword('" + cp_username.Text + "','" + old_password.Password.ToString() + "','" + new_password.Password.ToString() + "')";
                NpgsqlCommand cmd = new NpgsqlCommand(state, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("You have successfully changed your password");
                ChangePasswordGrid.Visibility= Visibility.Collapsed;
                GridLogin.Visibility= Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Wrong password entered for this user.");
                old_password.Password = null;
                new_password.Password = null;
            }
            con.Close();

            
        }
    }
}

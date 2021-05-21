using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace LoginRegiser
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con = new SqlConnection(); 
        SqlCommand cmd = new SqlCommand();
        SqlDataReader dr;

        public MainWindow()
        {
            InitializeComponent();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["LoginRegiser.Properties.Settings.Setting"].ConnectionString.ToString();
        }

        private void tbUsername_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text == "Username")  tbUsername.Text = "";
        }

        private void tbPassword_GotFocus(object sender, RoutedEventArgs e)
        {   
            if (tbPassword.Password == "********") tbPassword.Password = "";

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private bool VerifyUser(string username, string password)
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Users where username='" + username + "' and password='" + password + "'";
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                if (Convert.ToBoolean(dr["Status"]))
                {
                    con.Close();
                    return true;

                }
                else
                {
                    con.Close();
                    return false;
                }
            }
            else
            {
                con.Close();
                return false;
            }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (con.State == System.Data.ConnectionState.Open)
            {
                con.Close();
            }

            if (VerifyUser(tbUsername.Text, tbPassword.Password))
            {
                MessageBox.Show("Success", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                tbUsername.Text = "";
                tbPassword.Password = "";
                MessageBox.Show("Fail", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
        }

        private bool UserExisted(string username)
        {
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "select * from Users where username='" + username + "'";
            dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            if (UserExisted(tbUsername.Text))
            {
                con.Close();
                tbUsername.Text = "";
                tbPassword.Password = "";
                MessageBox.Show("User already exist", "Register", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                con.Close();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into Users values('"+tbUsername.Text+"', '"+tbPassword.Password+"', 2, 1)";
                dr = cmd.ExecuteReader();
                con.Close();

                tbUsername.Text = "";
                tbPassword.Password = "";

                MessageBox.Show("Success", "Register", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }

                if (VerifyUser(tbUsername.Text, tbPassword.Password))
                {
                    MessageBox.Show("Success", "Login", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    tbUsername.Text = "";
                    tbPassword.Password = "";
                    MessageBox.Show("Fail", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);

                }
            }
        }
    }
}

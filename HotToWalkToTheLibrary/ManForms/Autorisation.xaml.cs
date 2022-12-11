using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
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

namespace HotToWalkToTheLibrary.ManForms
{
    public partial class Autorisation : Window
    {
        public Autorisation()
        {
            InitializeComponent();
        }
        private void Login_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Login.Text.Length > 0)
            {
                LogText.Visibility = Visibility.Hidden;
            }
            else
            {
                LogText.Visibility = Visibility.Visible;
            }
        }
        private void Войти_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserEnter();  
        }

        private void UserEnter()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string query = "select count(1) from Users where Username=@Username and Pass=@Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", Login.Text);
                cmd.Parameters.AddWithValue("@Password", Password.Password);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 1)
                {

                    MainWindow next = new MainWindow();
                    next.Show();
                    this.Close();
                }
                else
                {
                    Error.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            CloseButton.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"pack://application:,,,/Resources/Закрыть.png"));
        }

        private void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            CloseButton.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"pack://application:,,,/Resources/Закрыть нажат.png"));
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Password.Focus();
            }
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (Password.Password.Length > 0)
            {
                PassText.Visibility = Visibility.Hidden;
            }
            else
            {
                PassText.Visibility = Visibility.Visible;
            }    
        }
        private void PassTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PassTextBox.Text.Length > 0)
            {
                PassText.Visibility = Visibility.Hidden;
            }
            else
            {
                PassText.Visibility = Visibility.Visible;
            }
            

        }
        private void HidePass_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ImageSourceConverter imgs = new ImageSourceConverter();
            HidePass.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"pack://application:,,,/Resources/Hide.png"));
            Password.Visibility = Visibility.Visible;
            PassTextBox.Visibility = Visibility.Hidden;
            //Password.Focus();
        }
        private void HidePass_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            ImageSourceConverter imgs = new ImageSourceConverter();
            HidePass.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"pack://application:,,,/Resources/Unhide.png"));
            PassTextBox.Text = Password.Password;
            Password.Visibility = Visibility.Hidden;
            PassTextBox.Visibility = Visibility.Visible;
        }

        private void Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UserEnter();
            }       
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary.ManForms
{
    /// <summary>
    /// Логика взаимодействия для BooksPage.xaml
    /// </summary>
    public partial class BooksPage : Page
    {
        public BooksPage()
        {
            InitializeComponent();
            ControlID.SqlB = @"select * from Books";
            UpdateTable(ControlID.SqlB);
        }
        public void UpdateTable(string SqlB)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SqlB;
                cmd.Connection = con;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Books");
                MainDataGrid.DataContext = ds;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search.Text.Length > 0)
            {
                SearchText.Visibility = Visibility.Hidden;
            }
            else
            {
                SearchText.Visibility = Visibility.Visible;
            }
            string sql = @"select * from Books where Title like '" + "%" + Search.Text + "%' or Author like '" + "%" + Search.Text + "%' or MakeYears like '" + "%" + Search.Text + "%'";
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sql;
                cmd.Connection = con;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Books");
                MainDataGrid.DataContext = ds;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatB = true;
            AddMainShowForm();
        }

        private void AddMainShowForm()
        {
            BooksChanger next = new BooksChanger();
            next.Show();
        }

        public void Edit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatB = false;
            AddMainShowForm();
        }

        private void Delete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ControlID.Books_ID != 0)
            {
                try
                {
                    string sql = "delete from Books where Books_ID like @Books_ID";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Books_ID", ControlID.Books_ID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    UpdateTable(ControlID.SqlB);
                }
                catch
                {
                    MessageBox.Show("Удаление невозможно! Есть упоминание в таблице Абонементы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Error.Visibility = Visibility.Hidden;
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }

        }

        public void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;

                DataRowView selectedRow = dg.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    ControlID.Books_ID = (int)selectedRow["Books_ID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateTable(ControlID.SqlB);
        }

        private void MainDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateTable(ControlID.SqlB);
        }
    }
}

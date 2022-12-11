using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;


namespace HotToWalkToTheLibrary.ManForms
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    /// 
    public partial class MainPage : Page
    {
        public static class ControlID
        {
            public static int Main_ID { get; set; }
            public static int Books_ID { get; set; }
            public static int Readers_ID { get; set; }
            public static bool ChangeWhat { get; set; }
            public static bool ChangeWhatB { get; set; }
            public static bool ChangeWhatR { get; set; }
            public static string Sql { get; set; }
            public static string SqlB { get; set; }
            public static string SqlR { get; set; }
            //public static bool UpdateInitialize { get; set; }
            //public static bool UpdateInitializeB { get; set; }
            //public static bool UpdateInitializeR { get; set; }
        }
        public MainPage()
        {
            InitializeComponent();

            ControlID.Sql = @"select * from Main";
            UpdateTable(ControlID.Sql);

            //System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            //dispatcherTimer.Start();

        }
        public void UpdateTable(string Sql)
        {
            try
            {

                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = Sql;
                cmd.Connection = con;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Main");
                MainDataGrid.DataContext = ds;
                con.Close();

                //MainDataGrid.RowStyle = convert.ToDateTime("Дата выдачи").Date;
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
            string sql = @"select * from Main where FIO like '%" + Search.Text + "%' or M_Book like '%" + Search.Text + "%'";
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
                da.Fill(ds, "Main");
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
            ControlID.ChangeWhat = true;
            AddMainShowForm();
        }

        private void AddMainShowForm()
        {
            MainChanger next = new MainChanger();
            next.Show();
        }

        public void Edit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhat = false;
            AddMainShowForm();
        }

        private void Delete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ControlID.Main_ID != 0)
            {
                try
                {
                    string sql = "delete from Main where Main_ID like @Main_ID";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Main_ID", ControlID.Main_ID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    UpdateTable(ControlID.Sql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
                    ControlID.Main_ID = (int)selectedRow["Main_ID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateTable(ControlID.Sql);
        }

        private void MainDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateTable(ControlID.Sql);
        }
    }
}

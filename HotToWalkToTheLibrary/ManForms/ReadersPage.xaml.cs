using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary.ManForms
{
    public partial class ReadersPage : Page
    {
        //DispatcherTimer _dispTimer;
        public ReadersPage()
        {
            InitializeComponent();
            ControlID.SqlR = @"select * from Readers";
            UpdateTable(ControlID.SqlR);
        }
        public void UpdateTable(string SqlR)
        {
            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = SqlR;
                cmd.Connection = con;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds, "Readers");
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
            string sql = @"select * from Readers where Sname like '%" + Search.Text + "%' or Fname like '%" + Search.Text + "%' or Lname like '%" + Search.Text + "%' or LivingPlace like '%" + Search.Text + "%'";
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
                da.Fill(ds, "Readers");
                MainDataGrid.DataContext = ds;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При фильтрации");
            }
        }
        public void Add_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatR = true;
            AddMainShowForm();
        }

        private void AddMainShowForm()
        {
            ReadersChanger next = new ReadersChanger();
            next.Show();
        }

        public void Edit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatR = false;
            AddMainShowForm();
        }

        private void Delete_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ControlID.Readers_ID != 0)
            {
                try
                {
                    string sql = "delete from Readers where Readers_ID like @Readers_ID";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Readers_ID", ControlID.Readers_ID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    UpdateTable(ControlID.SqlR);
                }
                catch
                {
                    MessageBox.Show("Удаление невозможно! Есть упоминание в таблице Абонементы.","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Error.Visibility = Visibility.Hidden;
            }
            else
            {
                Error.Visibility = Visibility.Visible;
            }

        }
        public static int Readers_ID;
        public void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;

                DataRowView selectedRow = dg.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    ControlID.Readers_ID = (int)selectedRow["Readers_ID"];
                    Readers_ID = (int)selectedRow["Readers_ID"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateTable(ControlID.SqlR);
        }
        private void Bilet_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string Sname;
            string Fname;
            string Lname;
            if(Readers_ID != 0)
            {
                try
                {
                    string Sql = "select * from Readers where Readers_ID like " + Readers_ID;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    SqlDataReader DR = cmd.ExecuteReader();

                    while (DR.Read())
                    {
                        Sname = DR["Sname"].ToString();
                        Fname = DR["Fname"].ToString();
                        Lname = DR["Lname"].ToString();
                        if(Sname != null)
                        {
                            Replacer(Readers_ID.ToString(), Sname, Fname, Lname);
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При загрузке данных в отчет");
                }
            }
            else
            {

            }
        }
        
        private void Replacer(string Readers_ID, string Sname, string Fname, string Lname)
        {
            //File.Copy(@"/Resources/ReaderTicket.docx", @"c:\Users\%username%\Desktop\ReaderTicket.docx");

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            string fileLoc = System.AppDomain.CurrentDomain.BaseDirectory;
            var wDoc = app.Documents.Open(fileLoc + @"ReaderTicket.docx");

            GenerateDoc(Readers_ID, "{Readers_ID}", wDoc);
            GenerateDoc(Sname, "{Sname}", wDoc);
            GenerateDoc(Fname, "{Fname}", wDoc);
            GenerateDoc(Lname, "{Lname}", wDoc);
            app.Visible = true;
        }


        private void GenerateDoc(string toPrint, string toFind, Microsoft.Office.Interop.Word.Document wDoc)
        {
            var range = wDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: toFind, ReplaceWith: toPrint);
        }

        private void MainDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateTable(ControlID.SqlR);
        }
    }
}

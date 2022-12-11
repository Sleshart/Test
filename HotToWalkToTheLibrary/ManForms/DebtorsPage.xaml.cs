using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static HotToWalkToTheLibrary.ManForms.MainPage;
using System.IO;

namespace HotToWalkToTheLibrary.ManForms
{
    public partial class DebtorsPage : Page
    {
        public DebtorsPage()
        {
            InitializeComponent();

            ControlID.Sql = @"select * from Main where WhereIsBook like 'У читателя'";
            UpdateTable(ControlID.Sql);
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
            string  sql = @"select * from Main where WhereIsBook like 'У читателя' and FIO like '%" + Search.Text + "%' or WhereIsBook like 'У читателя' and M_Book like '%" + Search.Text + "%'";
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
        private void Reload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UpdateTable(ControlID.Sql);
        }
        public string FIO;
        public string Reader_Num;
        public string M_Book;
        public string Books_Num;
        public string DateGet;
        public string LastDate;
        private void MainDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dg = (DataGrid)sender;

                DataRowView selectedRow = dg.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    FIO = selectedRow["FIO"].ToString();
                    Reader_Num = selectedRow["Reader_Num"].ToString();
                    M_Book = selectedRow["M_Book"].ToString();
                    Books_Num = selectedRow["Books_Num"].ToString();
                    DateGet = selectedRow["DateGet"].ToString();
                    LastDate = selectedRow["LastDate"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Debtor_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //CopyFilesRecursively();

            Replacer(FIO, Reader_Num, M_Book, Books_Num, DateGet, LastDate);
        }
        private void Replacer(string FIO, string Reader_Num, string M_Book, string Books_Num, string DateGet, string LastDate)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
                string fileLoc = AppDomain.CurrentDomain.BaseDirectory;
                var wDoc = app.Documents.Open(fileLoc + @"Debtor.docx");

                GenerateDoc(FIO, "{FIO}", wDoc);
                GenerateDoc(Reader_Num, "{Reader_Num}", wDoc);
                GenerateDoc(M_Book, "{M_Book}", wDoc);
                GenerateDoc(Books_Num, "{Books_Num}", wDoc);
                GenerateDoc(DateGet, "{DateGet}", wDoc);
                GenerateDoc(LastDate, "{LastDate}", wDoc);
                app.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void GenerateDoc(string toPrint, string toFind, Microsoft.Office.Interop.Word.Document wDoc)
        {
            var range = wDoc.Content;
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: toFind, ReplaceWith: toPrint);
        }

        private void MainDataGrid_MouseMove(object sender, MouseEventArgs e)
        {
            UpdateTable(ControlID.Sql);
        }
    }
}

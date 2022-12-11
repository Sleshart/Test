using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary.ManForms
{
    /// <summary>
    /// Логика взаимодействия для MainChanger.xaml
    /// </summary>
    public partial class MainChanger : Window
    {
        public int Main_ID = ControlID.Main_ID;
        public bool ChangeWhat = ControlID.ChangeWhat;
        public MainChanger()
        {
            InitializeComponent();

            InputOnChange();
        }

        private void InputOnChange()
        {
            if (ChangeWhat == true)
            {
                Atext.Text = "Добавить";
            }
            else
            {
                try
                {
                    string Sql = "select * from Main where Main_ID like " + Main_ID;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    SqlDataReader DR = cmd.ExecuteReader();

                    while (DR.Read())
                    {
                        CombText.Text = DR["FIO"].ToString();
                        CombText2.Text = DR["M_Book"].ToString();
                        FIOComboBox.Text = DR["FIO"].ToString();
                        BookComboBox.Text = DR["M_Book"].ToString();
                        DateGet.Text = DR["DateGet"].ToString();
                        DateGive.Text = DR["DateGive"].ToString();
                        LastDate.Text = DR["LastDate"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При загрузке данных на изменение");
                }
                Atext.Text = "Изменить";

            }
        }

        private void BookComboBoxItemsFill()
        {
            BookComboBox.Items.Clear();
            try
            {
                string Sql = "select (cast(Books_ID as varchar(5)) + ': \"' + cast(Title as varchar(50)) + '\" ' + cast(Author as varchar(50))) as Books from Books";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand(Sql, conn);
                SqlDataReader DR = cmd.ExecuteReader();

                while (DR.Read())
                {
                    BookComboBox.Items.Add(DR[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При загрузке данных в комбо бокс книги");
            }

        }

        private void FIOComboBoxItemsFill()
        {
            FIOComboBox.Items.Clear();
            try
            {
                string Sql = "select (cast(Readers_ID as varchar(5)) + ': ' + cast(Sname as varchar(50)) + ' ' + cast(Fname as varchar(50)) + ' ' + cast(Lname as varchar(50))) as FIO from Readers";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                conn.Open();
                SqlCommand cmd = new SqlCommand(Sql, conn);
                SqlDataReader DR = cmd.ExecuteReader();

                while (DR.Read())
                {
                    FIOComboBox.Items.Add(DR[0]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При загрузке данных в комбо бокс фио");
            }
        }

        private void FIOComboBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            CombText.Text = FIOComboBox.Text;
        }
        private void BookComboBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            CombText2.Text = BookComboBox.Text;
        }
        private void Action_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            string RnumT = CombText.Text.Substring(0, 3);
            int Rnum;
            int.TryParse(string.Join("", RnumT.Where(c => char.IsDigit(c))), out Rnum);

            string BnumT = CombText2.Text.Substring(0, 3);
            int Bnum;
            int.TryParse(string.Join("", BnumT.Where(r => char.IsDigit(r))), out Bnum);
            if (ChangeWhat == true)
            {
                AddNote(Rnum, Bnum);
            }
            else
            {
                ChangeNote(Rnum, Bnum);
            }
        }

        private void ChangeNote(int Rnum, int Bnum)
        {
            try
            {
                string sql = "update Main set FIO = @FIO, Reader_Num = @Reader_Num, M_Book = @Book, Books_Num = @Books_Num, DateGet = @DateGet, DateGive = @DateGive, LastDate = @LastDate where Main_ID like " + Main_ID + " update Main set WhereIsBook = CASE when DateGive = '1900-01-01' THEN 'У читателя' ELSE 'Возвращена' END";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FIO", CombText.Text);
                cmd.Parameters.AddWithValue("@Reader_Num", Rnum.ToString());
                cmd.Parameters.AddWithValue("@Book", CombText2.Text);
                cmd.Parameters.AddWithValue("@Books_Num", Bnum.ToString());
                cmd.Parameters.AddWithValue("@DateGet", DateGet.Text);
                if (DateGive.SelectedDate.Value != null)
                {
                    cmd.Parameters.AddWithValue("@DateGive", DateGive.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateGive", "1900-01-01");
                }
                cmd.Parameters.AddWithValue("@LastDate", LastDate.Text);
                conn.Open();
                cmd.ExecuteNonQuery();

                MainPage update = new MainPage();
                update.UpdateTable(ControlID.Sql);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При изменении записи");
            }
        }

        private void AddNote(int Rnum, int Bnum)
        {
            try
            {
                string sql = "insert into Main(FIO,Reader_Num,M_Book,Books_Num,DateGet,DateGive,LastDate) values(@FIO, @Reader_Num, @Book, @Books_Num1, @DateGet, @DateGive, @LastDate) update Main set WhereIsBook = CASE when DateGive = '1900-01-01' THEN 'У читателя' ELSE 'Возвращена' END";
                SqlConnection conn = new SqlConnection();
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@FIO", CombText.Text);
                cmd.Parameters.AddWithValue("@Reader_Num", Rnum.ToString());
                cmd.Parameters.AddWithValue("@Book", CombText2.Text);
                cmd.Parameters.AddWithValue("@Books_Num1", Bnum.ToString());
                cmd.Parameters.AddWithValue("@DateGet", DateGet.Text);
                if (DateGive.Text != "Выберите дату")
                {
                    cmd.Parameters.AddWithValue("@DateGive", DateGive.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@DateGive", "1900-01-01");
                }
                cmd.Parameters.AddWithValue("@LastDate", LastDate.Text);
                conn.Open();
                cmd.ExecuteNonQuery();

                MainPage update = new MainPage();
                update.UpdateTable(ControlID.Sql);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При добавлении записи");
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
        private void AddBook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatR = true;
            BooksChanger next = new BooksChanger();
            next.Show();
        }

        private void AddReader_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ControlID.ChangeWhatB = true;
            ReadersChanger next = new ReadersChanger();
            next.Show();
        }
        private void FIOComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            FIOComboBoxItemsFill();
        }

        private void BookComboBox_MouseEnter(object sender, MouseEventArgs e)
        {
            BookComboBoxItemsFill();
        }
    }
}

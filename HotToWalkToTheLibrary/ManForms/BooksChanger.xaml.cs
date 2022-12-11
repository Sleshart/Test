using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary.ManForms
{
    public partial class BooksChanger : Window
    {
        public int Books_ID = ControlID.Books_ID;
        public bool ChangeWhatB = ControlID.ChangeWhatB;
        public BooksChanger()
        {
            InitializeComponent();
            AddNoteToChange();
        }

        private void AddNoteToChange()
        {
            if (ChangeWhatB == true)
            {
                Atext.Text = "Добавить";
            }
            else
            {
                try
                {
                    string Sql = "select * from Books where Books_ID like " + Books_ID;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(Sql, conn);
                    SqlDataReader DR = cmd.ExecuteReader();

                    while (DR.Read())
                    {
                        Title.Text = DR["Title"].ToString();
                        Author.Text = DR["Author"].ToString();
                        MakeYears.Text = DR["MakeYears"].ToString();
                        HowMany.Text = DR["HowMany"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При загрузке данных на изменение");
                }
                Atext.Text = "Изменить";

            }
        }

        private void Action_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ChangeWhatB == true)
            {
                try
                {
                    string sql = "insert into Books(Title,Author,MakeYears,HowMany) values(@Title, @Author, @MakeYears, @HowMany)";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Title", Title.Text);
                    cmd.Parameters.AddWithValue("@Author", Author.Text);
                    cmd.Parameters.AddWithValue("@MakeYears", MakeYears.Text);
                    cmd.Parameters.AddWithValue("@HowMany", HowMany.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    //BooksPage update = new BooksPage();
                    //update.UpdateTable(ControlID.SqlB);
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При добавлении записи");
                }
            }
            else
            {
                try
                {
                    string sql = "update Books set Title = @Title, Author = @Author, MakeYears = @MakeYears, HowMany = @HowMany where Books_ID like " + Books_ID;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Title", Title.Text);
                    cmd.Parameters.AddWithValue("@Author", Author.Text);
                    cmd.Parameters.AddWithValue("@MakeYears", MakeYears.Text);
                    cmd.Parameters.AddWithValue("@HowMany", HowMany.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    BooksPage update = new BooksPage();
                    update.UpdateTable(ControlID.SqlB);
                    this.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При изменении записи");
                }
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
    }
}

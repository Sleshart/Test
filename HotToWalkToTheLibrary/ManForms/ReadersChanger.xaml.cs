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
using System.Windows.Shapes;
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary.ManForms
{
    public partial class ReadersChanger : Window
    {
        public int Readers_ID = ControlID.Readers_ID;
        public bool ChangeWhatR = ControlID.ChangeWhatR;
        public ReadersChanger()
        {
            InitializeComponent();
            InputOnChange();
        }

        private void InputOnChange()
        {
            if (ChangeWhatR == true)
            {
                Atext.Text = "Добавить";
            }
            else
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
                        Sname.Text = DR["Sname"].ToString();
                        Fname.Text = DR["Fname"].ToString();
                        Lname.Text = DR["Lname"].ToString();
                        YearsOld.Text = DR["YearsOld"].ToString();
                        LivingPlace.Text = DR["LivingPlace"].ToString();
                        PhoneNumber.Text = DR["PhoneNumber"].ToString();
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
            if (ChangeWhatR == true)
            {
                try
                {
                    string sql = "insert into Readers(Sname,Fname,Lname,YearsOld,LivingPlace,PhoneNumber) values(@Sname, @Fname, @Lname, @YearsOld, @LivingPlace, @PhoneNumber)";
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Sname", Sname.Text);
                    cmd.Parameters.AddWithValue("@Fname", Fname.Text);
                    cmd.Parameters.AddWithValue("@Lname", Lname.Text);
                    cmd.Parameters.AddWithValue("@YearsOld", YearsOld.Text);
                    cmd.Parameters.AddWithValue("@LivingPlace", LivingPlace.Text);
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    UpdateTable();
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
                    string sql = "update Readers set Sname = @Sname, Fname = @Fname, Lname = @Lname, YearsOld = @YearsOld, LivingPlace = @LivingPlace, PhoneNumber = @PhoneNumber where Readers_ID like " + Readers_ID;
                    SqlConnection conn = new SqlConnection();
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["connsetting"].ToString();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@Sname", Sname.Text);
                    cmd.Parameters.AddWithValue("@Fname", Fname.Text);
                    cmd.Parameters.AddWithValue("@Lname", Lname.Text);
                    cmd.Parameters.AddWithValue("@YearsOld", YearsOld.Text);
                    cmd.Parameters.AddWithValue("@LivingPlace", LivingPlace.Text);
                    cmd.Parameters.AddWithValue("@PhoneNumber", PhoneNumber.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    UpdateTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "При изменении записи");
                }
            }
        }

        private void UpdateTable()
        {
            ReadersPage update = new ReadersPage();
            update.UpdateTable(ControlID.SqlR);
            this.Close();
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

using HotToWalkToTheLibrary.ManForms;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static HotToWalkToTheLibrary.ManForms.MainPage;

namespace HotToWalkToTheLibrary
{
    /// Влажный код
    /// 
    /// var bc = new BrushConverter();
    /// Должники.Background = (Brush) bc.ConvertFrom("#8B93A7");
    /// Дтекст.Foreground = (Brush) bc.ConvertFrom("#1B1D23");

    public partial class MainWindow : Window
    {
        int i = 0;
        public MainWindow()
        {
            InitializeComponent();

            MainFrame.Content = new MainPage();
            Абонемент.Margin = new Thickness(-70, 0, 0, 0);

            //var bc = new BrushConverter();
            //Абонемент.Background = (Brush) bc.ConvertFrom("#959AA7");
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            //CloseButton.Source = new Uri(@"Resources\Закрыть нажат.png");

            ImageSourceConverter imgs = new ImageSourceConverter();
            CloseButton.SetValue(Image.SourceProperty, imgs.ConvertFromString(@"pack://application:,,,/Resources/Закрыть нажат.png"));

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
        public string sql;
        public bool Ab = false;
        public bool Do = false;
        public bool Ch = false;
        public bool Kn = false;

        private void Абонемент_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ab = true;
            Do = false;
            Ch = false;
            Kn = false;
            ////Абонемент.Margin = new Thickness(-80, 0, 0, 0);
            //Должники.Margin = new Thickness(-100, 0, 0, 0);
            //Читатели.Margin = new Thickness(-100, 0, 0, 0);
            //Книги.Margin = new Thickness(-100, 0, 0, 0);
            ControlID.Sql = "select * from Main";
            MainFrame.Content = new MainPage();
        }
        private void Должники_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ab = false;
            Do = true;
            Ch = false;
            Kn = false;
            //Абонемент.Margin = new Thickness(-100, 0, 0, 0);
            ////Должники.Margin = new Thickness(-80, 0, 0, 0);
            //Читатели.Margin = new Thickness(-100, 0, 0, 0);
            //Книги.Margin = new Thickness(-100, 0, 0, 0);
            MainFrame.Content = new DebtorsPage();
        }
        private void Читатели_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ab = false;
            Do = false;
            Ch = true;
            Kn = false;
            Абонемент.Margin = new Thickness(-100, 0, 0, 0);
            Должники.Margin = new Thickness(-100, 0, 0, 0);
            //Читатели.Margin = new Thickness(-80, 0, 0, 0);
            Книги.Margin = new Thickness(-100, 0, 0, 0);
            MainFrame.Content = new ReadersPage();
        }
        private void Книги_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ab = false;
            Do = false;
            Ch = false;
            Kn = true;
            Абонемент.Margin = new Thickness(-100, 0, 0, 0);
            Должники.Margin = new Thickness(-100, 0, 0, 0);
            Читатели.Margin = new Thickness(-100, 0, 0, 0);
            //Книги.Margin = new Thickness(-80, 0, 0, 0);
            MainFrame.Content = new BooksPage();
        }  
    }
}

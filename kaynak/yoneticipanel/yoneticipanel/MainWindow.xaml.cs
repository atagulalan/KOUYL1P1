using System;
using System.Collections.Generic;
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
using MySql.Data;
using MySql.Data.MySqlClient;

namespace yoneticipanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
 
    public partial class MainWindow : Window
    {

        string connectionString = "SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;";
        



        public MainWindow()
        {
            InitializeComponent();
            /*
            string connectionString = "SERVER=localhost;DATABASE=yazlab1;UID=root;PASSWORD=;SslMode=none;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("select * from books", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;*/

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("select * from books", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("select * from users", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("SELECT isbn, COUNT(*) as cnt FROM ratings group by isbn ORDER BY cnt DESC LIMIT 15;", connection);
            connection.Open();  
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            kitap_ekle pencere = new kitap_ekle();
            pencere.Show();
           
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            kullanıcı_ekle pencere = new kullanıcı_ekle();
            pencere.Show();
            
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("SELECT * FROM books ORDER BY tarih DESC LIMIT 9;", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            kitap_sil pencere = new kitap_sil();
            pencere.Show();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            kullanici_sil pencere = new kullanici_sil();
            pencere.Show();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand("SELECT isbn, AVG(book_rating) as ortalama FROM ratings GROUP BY isbn ORDER BY ortalama DESC LIMIT 30;", connection);
            connection.Open();
            DataTable dt = new DataTable();
            dt.Load(command.ExecuteReader());
            connection.Close();
            dtGrid.DataContext = dt;
        }
    }

     

    }


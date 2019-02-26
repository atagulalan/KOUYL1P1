using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Bookmark {
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window {
        public static int oy;
        public MySqlConnection connection;
        public MainWindow reelMain;
        public void hataVer(String s) {
            textBlock.Text = s;
        }

        public Login() {
            InitializeComponent();
        }

        public Login(MainWindow main) {
            InitializeComponent();
            reelMain = main;

        }

        public void login() {
            connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");
            String k = girisKullaniciAdiBox.Text;
            String s = girisSifreBox.Password;

            Console.WriteLine("Giriş yapılıyor...");
            if (Regex.IsMatch(k, "^[a-zA-Z0-9]+$") == true) {
                hataVer("");
                if (s.Length >= 3) {
                    hataVer("");
                    MySqlDataReader dr;
                    Console.WriteLine("Giriş işlemi başladı...");
                    connection.Open();
                    string sorgu = "SELECT * FROM (SELECT * FROM users WHERE nick=@k AND pw=@s) AS realUserTable INNER JOIN (SELECT COUNT(*) as books_read FROM ratings WHERE user_id IN(SELECT user_id FROM users WHERE nick = @k)) AS readBooksTable";
                    MySqlCommand command = new MySqlCommand(sorgu, connection);
                    command.Parameters.AddWithValue("@k", k);
                    command.Parameters.AddWithValue("@s", s);
                    dr = command.ExecuteReader();

                    if (dr.Read()) {
                        Properties.Settings.Default.userID = dr.GetString("user_id");
                        Properties.Settings.Default.userNick = dr.GetString("nick");
                        oy = dr.GetInt32("books_read");
                        Properties.Settings.Default.userOy = oy.ToString();
                        Console.WriteLine(oy);
                        if (oy >= 10) {
                            Read kitaplar = new Read();
                            kitaplar.Show();
                            this.Hide();
                        } else {
                            Books kitaplar = new Books();
                            kitaplar.Show();
                            this.Hide();
                        }
                    } else {
                        hataVer("Kullanıcı adı veya şifre hatalı.");
                    }
                } else {
                    hataVer("Kullanıcı adı veya şifre hatalı.");
                }
            } else {
                hataVer("Kullanıcı adı veya şifre hatalı.");
            }
            connection.Close();
            
        }

        private void girisIslemi(object sender, RoutedEventArgs e) {
            login();
        }
        private void geriGel2(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                reelMain.Show();
            } catch (Exception) {}
        }
    }
}

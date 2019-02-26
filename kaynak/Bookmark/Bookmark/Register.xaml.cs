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
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window {
        public MySqlConnection connection;
        public MainWindow reelMain;


        public void hataVer(String s) {
            textBlock.Text = s;
        }

        public void hataVer(String s, TextBox k) {
            textBlock.Text = s;
            k.Focus();
        }

        public void hataVer(String s, PasswordBox k) {
            textBlock.Text = s;
            k.Focus();
        }

        public void hataSil() {
            textBlock.Text = "";
        }


        public Register() {
            InitializeComponent();
        }

        public Register(MainWindow main) {
            InitializeComponent();
            reelMain = main;
        }

        private void kayitIslemi(object sender, RoutedEventArgs e) {
            connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");
            String k = kullaniciAdiBox.Text;
            String y = yasBox.Text;
            String b = konumBox.Text;
            String s = sifreBox.Password;
            Console.WriteLine("Kayıt butonuna tıklandı, kontroller yapılıyor...");
            //Kullanıcı adı uygun mu?
            if (Regex.IsMatch(k, "^[a-zA-Z0-9]+$") == true) {
                hataSil();
                //Konum uygun mu?
                if (b.Length > 0) {
                    hataSil();
                    //Yaş uygun mu?
                    int n;
                    var isNumeric = int.TryParse(y, out n);
                    if (isNumeric && n >= 0 && n < 120) {
                        hataSil();
                        //Şifre uygun mu?
                        if (s.Length >= 3) {
                            Console.WriteLine("Kayıt işlemi başladı...");

                            connection.Open();
                            string sorgu = "SELECT count(*)>0 as userExists FROM users WHERE nick=@k";
                            MySqlCommand command = new MySqlCommand(sorgu, connection);
                            MySqlDataReader dr;
                            command.Parameters.AddWithValue("@k", k);
                            dr = command.ExecuteReader();
                            if (dr.Read()) {
                                if (dr.GetInt32("userExists") == 0) {
                                    connection.Close();
                                    connection.Open();
                                    string sorgu2 = "INSERT INTO users (nick, pw, location, age) Values (@k, @s, @b, @y)";
                                    MySqlCommand command2 = new MySqlCommand(sorgu2, connection);
                                    command2.Parameters.AddWithValue("@k", k);
                                    command2.Parameters.AddWithValue("@s", s);
                                    command2.Parameters.AddWithValue("@b", b);
                                    command2.Parameters.AddWithValue("@y", y);
                                    command2.ExecuteNonQuery();
                                    var lg = new Login(reelMain);
                                    lg.girisKullaniciAdiBox.Text = k;
                                    lg.girisSifreBox.Password = s;
                                    lg.login();
                                    this.Hide();
                                } else {
                                    MessageBox.Show("Bu kullanıcı adıyla kayıtlı bir kullanıcı var. Lütfen farklı bir kullanıcı adı seçin.");
                                }
                            } else {
                                MessageBox.Show("Bir hata meydana geldi");
                            }
                            connection.Close();


                        } else {
                            hataVer("Hata: Şifreniz en az 3 karakter olmalıdır.", sifreBox);
                        }
                    } else {
                        hataVer("Hata: Lütfen yaşınızı doğru girin.", yasBox);
                    }

                } else {
                    hataVer("Hata: Lütfen konumunuzu doğru girin.", konumBox);
                }
            } else {
                hataVer("Hata: Kullanıcı adı geçersiz karakterler içeriyor.", kullaniciAdiBox);
            }
        }

        private void geriGel(object sender, MouseButtonEventArgs e) {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            try {
                reelMain.Show();
            } catch (Exception) { }
        }
    }
}

using System;
using System.Collections.Generic;
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
using MySql.Data.MySqlClient;
using System.Threading;
using System.ComponentModel;

namespace Bookmark {
    /// <summary>
    /// Interaction logic for BookInner.xaml
    /// </summary>
    public partial class BookInner : Window {
        public MySqlConnection connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");
        public string pIsbn;
        public int userVote;
        Books cagiran;
        public bool cagirildi = false;

        public void getImage(string url) {
            if (url != null) {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                bgimage.Source = bitmap;
            }

        }

        public BookInner() {
            InitializeComponent();
        }

        public void disableButtons(int selected) {
            Button[] buts = new Button[] { oyla1, oyla2, oyla3, oyla4, oyla5, oyla6, oyla7, oyla8, oyla9, oyla10 };
            for (int i = 0; i < 10; i++) {
                buts[i].IsEnabled = false;
            }
            buts[selected - 1].BorderBrush = new SolidColorBrush(Color.FromArgb(255, 248, 110, 143));
        }

        public void isle(string[] values) {
            /*
             * 0 - Isbn
             * 1 - BookName
             * 2 - Stars
             * 3 - VoteCount
             * 4 - Image
             * 5 - Author
             * 6 - Publisher
             * 7 - Year Of Publication
             * 8 - Date
             * 9 - Ready State
             */
            pIsbn = values[0];
            isbn.Content = "ISBN: " + values[0];
            bookname.Text = values[1];
            kitapadi.Content = "Kitap Adı: " + values[1];
            oy.Content = "Oy: " + values[2] + " / 10";
            oysayisi.Content = "Oy Sayısı: " + values[3];
            getImage(values[4]);
            yazar.Content = "Yazar: " + values[5];
            yayimci.Content = "Yayımcı: " + values[6];
            yayinyili.Content = "Yayım Yılı: " + values[7];
            eklenmetarihi.Content = "Eklenme Tarihi: " + values[8];
            if (values[9] == "notReady") {
                kitabiOku.IsEnabled = false;
                hataSebebi.Visibility = Visibility.Visible;
                kitabiOku.Background = new SolidColorBrush(Color.FromArgb(255, 200, 200, 200));
            } else {
                kitabiOku.IsEnabled = true;
                hataSebebi.Visibility = Visibility.Hidden;
                kitabiOku.Background = new SolidColorBrush(Color.FromArgb(255, 248, 110, 143));
            }
            connection.Open();
            string sorgu = "SELECT book_rating FROM ratings WHERE user_id=@k AND isbn=@i LIMIT 1";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            MySqlDataReader dr;
            command.Parameters.AddWithValue("@k", Properties.Settings.Default.userID);
            command.Parameters.AddWithValue("@i", values[0]);
            dr = command.ExecuteReader();
            if (dr.Read()) {
                userVote = dr.GetInt32("book_rating");
                Console.WriteLine(userVote);
                disableButtons(userVote);
            } else {
                userVote = -1;
            }
            connection.Close();
        }

        public BookInner(string[] values) {
            InitializeComponent();
            isle(values);
        }

        public BookInner(Books cgr, string[] values) {
            InitializeComponent();
            cagirildi = true;
            cagiran = cgr;
            isle(values);
        }

        private void oyla(object sender, RoutedEventArgs e) {
            string s = (sender as Button).Content.ToString();
            connection.Open();
            string sorgu = "INSERT INTO ratings (user_id, isbn, book_rating) Values (@userid, @isbn, @book_rating)";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@userid", Properties.Settings.Default.userID);
            command.Parameters.AddWithValue("@isbn", pIsbn);
            command.Parameters.AddWithValue("@book_rating", s);
            command.ExecuteNonQuery();
            connection.Close();
            Console.WriteLine(s);
            userVote = Int32.Parse(s);
            disableButtons(userVote);
            Properties.Settings.Default.userOy = (Int32.Parse(Properties.Settings.Default.userOy) + 1).ToString();

            //MessageBox.Show("Oylanan Toplam Kitap Sayısı : " + Properties.Settings.Default.userOy);
            if (cagirildi) {
                cagiran.oyArttir();
            }
        }

        private void kitabiOku_Click(object sender, RoutedEventArgs e) {
            PDFRead pf = new PDFRead(pIsbn);
            pf.Show();
        }
    }
}

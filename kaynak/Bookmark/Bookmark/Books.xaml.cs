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
using System.ComponentModel;
using System.Data;

namespace Bookmark
{
    /// <summary>
    /// Interaction logic for Books.xaml
    /// </summary>
    public partial class Books : Window
    {
        public MySqlConnection connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");
        public BackgroundWorker bw = new BackgroundWorker();
        public string aramaParamteresi = "";
        public int nOy = 0;

        public void paginationChanged()
        {
            if (bw.IsBusy)
            {
                Console.WriteLine("was busy, now not");
                //bw.CancelAsync();
            }else
            {
                bw.RunWorkerAsync();
            }
            
        }

        private bool OpenConnection()
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        public int getPageCount()
        {
            var where = aramaParamteresi != "" ? "book_title LIKE '%" + (aramaParamteresi) + "%'" : "1";
            string query = ("SELECT CEIL(COUNT(*) / 21) as page FROM books WHERE "+where+";");
            string kk = "1";
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                kk = dataReader["page"] + "";
            }
            dataReader.Close();
            connection.Close();
            int sayi = 0;
            int.TryParse(kk, out sayi);
            return sayi;
        }

        public void getBooks(BookCover[] books, int sayfa)
        {
            Console.WriteLine("getbooks cagirildi");
            disableClick();
            var where = aramaParamteresi != "" ? "book_title LIKE '%" + (aramaParamteresi) + "%'" : "1";
            string query = ("SELECT p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi FROM (SELECT * FROM `books` WHERE "+ where + " LIMIT " + (sayfa - 1) * 21 + ",21) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn;");
            List<string>[] list = new List<string>[9];
            list[0] = new List<string>();
            list[1] = new List<string>();
            list[2] = new List<string>();
            list[3] = new List<string>();
            list[4] = new List<string>();
            list[5] = new List<string>();
            list[6] = new List<string>();
            list[7] = new List<string>();
            list[8] = new List<string>();
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                list[0].Add(dataReader["isbn"] + "");
                list[1].Add(dataReader["book_title"] + "");
                list[2].Add(dataReader["image_url_l"] + "");
                list[3].Add(dataReader["yildiz"] + "");
                list[4].Add(dataReader["oysayisi"] + "");
                list[5].Add(dataReader["book_author"] + "");
                list[6].Add(dataReader["year_of_publication"] + "");
                list[7].Add(dataReader["publisher"] + "");
                list[8].Add(dataReader["tarih"] + "");
            }
            dataReader.Close();
            connection.Close();
            for (int i = 0; i < 21; i++)
            {
                books[i].Isbn = "";
                books[i].BookName = "";
                books[i].Image = "";
                books[i].Stars = "";
                books[i].VoteCount = "";
                books[i].Author = "";
                books[i].Publisher = "";
                books[i].YearOfPublication = "";
                books[i].Date = "";
                books[i].cek();
            }
            for (int i = 0; i < list[0].Count; i++)
            {
                books[i].Isbn = list[0][i];
                books[i].BookName = list[1][i];
                books[i].Image = list[2][i];
                books[i].Stars = list[3][i] != "" ? list[3][i].Split('.')[0] : "0";
                books[i].VoteCount = list[4][i];
                books[i].Author = list[5][i];
                books[i].YearOfPublication = list[6][i];
                books[i].Publisher = list[7][i];
                books[i].Date = list[8][i];
                books[i].cek();
            }
        }

        public Books()
        {
            InitializeComponent();
            bw.DoWork += delegate {
                BookCover[] books = new BookCover[] { book1, book2, book3, book4, book5, book6, book7, book8, book9, book10, book11, book12, book13, book14, book15, book16, book17, book18, book19, book20, book21 };
                getBooks(books, pg.n);
            };
            bw.RunWorkerCompleted += delegate {
                Console.WriteLine("BİTTİ");
                enableClick();
            };
            label.Content = "Hoş geldin, " + Properties.Settings.Default.userNick;
            nOy = Int32.Parse(Properties.Settings.Default.userOy);
            ileriButonu.Content = nOy+"/10";
            pg.nmax = getPageCount();
            pg.bul();
        }


        public void disableClick()
        {
            this.Dispatcher.Invoke(() =>
            {
                yukleniyor.Visibility = Visibility.Visible;
                Tumu.IsEnabled = false;
            });
        }

        public void enableClick()
        {
            this.Dispatcher.Invoke(() =>
            {
                yukleniyor.Visibility = Visibility.Hidden;
                Tumu.IsEnabled = true;
            });
        }

        private void araButonu_Click(object sender, RoutedEventArgs e)
        {
            aramaParamteresi = kitapAraInput.Text;
            pg.n = 1;
            pg.nmax = getPageCount();
            pg.bul();
        }

        public void oyArttir() {
            nOy++;
            ileriButonu.Content = nOy + "/10";
            if (nOy >= 10) {
                ileriButonu.Content = "İlerle";
            }
        }

        private void book_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BookCover selectedBook = (BookCover)sender;
            Console.WriteLine(selectedBook.Isbn);
            string[] values = { selectedBook.Isbn, selectedBook.BookName, selectedBook.Stars, selectedBook.VoteCount, selectedBook.Image, selectedBook.Author, selectedBook.Publisher, selectedBook.YearOfPublication, selectedBook.Date, "notReady" };
            BookInner book = new BookInner(this,values);
            book.Show();
        }

        private void ileriButonu_Click(object sender, RoutedEventArgs e) {
            if (nOy < 10) {
                MessageBox.Show("Lütfen devam etmeden önce 10 adet kitap oylayın.");
            } else {
                Read kitaplar = new Read();
                kitaplar.Show();
                this.Hide();
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

        private void kitapAraInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                aramaParamteresi = kitapAraInput.Text;
                pg.n = 1;
                pg.nmax = getPageCount();
                pg.bul();
            }
        }
    }
}

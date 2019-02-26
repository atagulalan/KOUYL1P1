using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bookmark {
    /// <summary>
    /// Interaction logic for Read.xaml
    /// </summary>
    public partial class Read : Window {
        public int kitap_sayim = 0;
        public MySqlConnection connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");
        public BackgroundWorker bw = new BackgroundWorker();
        public BackgroundWorker popibw = new BackgroundWorker();
        public BackgroundWorker iyibw = new BackgroundWorker();
        public BackgroundWorker yenibw = new BackgroundWorker();
        public BackgroundWorker oneribw = new BackgroundWorker();
        bool zatenCekildi = false;
        public string aramaParamteresi = "";

        public void paginationChanged() {
            if (bw.IsBusy || popibw.IsBusy || iyibw.IsBusy || oneribw.IsBusy) {
                Console.WriteLine("BW IS BUSY");
                //bw.CancelAsync();
            } else {
                Console.WriteLine("PG CHANGED!");
                bw.RunWorkerAsync();
            }
        }

        private bool OpenConnection() {
            try {
                if (connection != null && connection.State == ConnectionState.Closed) {
                    connection.Open();
                }
                return true;
            } catch (MySqlException ex) {
                switch (ex.Number) {
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

        public int getPageCount() {
            var where = aramaParamteresi != "" ? "book_title LIKE '%" + (aramaParamteresi) + "%'" : "1";
            string query = ("SELECT CEIL(COUNT(*) / 21) as page FROM books WHERE " + where + ";");
            string kk = "1";
            OpenConnection();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
                kk = dataReader["page"] + "";
            }
            dataReader.Close();
            connection.Close();
            int sayi = 0;
            int.TryParse(kk, out sayi);
            return sayi;
        }


        public double cosine_similarity(int eslesme) {
            //Console.WriteLine("cosine_similarity cagirildi");
            /* Kosinüs Benzerliði 
             * kullanýcý 1 = k1
             * kullanýcý 2 = k2
             * 
             * Kosinüs benzerliði: Ýki matrisin skalar çarpýmý bölüm iki matrisin elemanlarýnýn karelerinin toplamýnýn kare kökü
             * http://prntscr.com/ldxf4w
             * 
             * Aranan etiketler kullanýcýda bulunuyorsa o indexe 1 bulunmuyorsa 0 yazýyoruz.
             * Tüm aranan kitaplar k1 kullanýcýnsa bulunduðundan dolayý k1 matrisi
             * [1 1 1 1 ...] þeklinde olur. k1 matrisiyle diðer matrislerin skalar çarpýmý da
             * direkt diðer matriste bulunan 1 sayýsýna eþittir " user_id2.Sum() "
             * Paydada ise k1 ve k2 matrislerinin elemanlarýnýn karelerinin 1/2'nci kuvveti vardýr.
             * Matrislerin elemanlarý 0 ve 1 den oluþtuðundan dolayý karelerini almamýza gerek yoktur.
             * Diðer toplamlarýn karekökünü almamýz yeterlidir.
             * 
            */
            double benzerlik = eslesme / (Math.Sqrt(kitap_sayim) * Math.Sqrt(eslesme));
            //Console.WriteLine("cosine_similarity bitti");
            return benzerlik;

        }


        public String find_my_books() {
            //Console.WriteLine("Find My Books cagirildi");
            int userid = Int32.Parse(Properties.Settings.Default.userID);
            List<String> kitaplar = new List<String>();
            OpenConnection();
            MySqlDataReader dr;
            string sorgu = " Select isbn from ratings where user_id = @userid";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@userid", userid.ToString());
            dr = command.ExecuteReader();

            while (dr.Read()) {
                kitaplar.Add(dr[0].ToString());
            }
            dr.Close();
            dr.Dispose();
            CloseConnection();
            kitap_sayim = kitaplar.Count;

            String arama = "";

            for (int i = 0; i < kitaplar.Count; i++) {
                if (i == kitaplar.Count - 1) {
                    arama = arama + "'" + kitaplar[i] + "'";
                } else {
                    arama = arama + "'" + kitaplar[i] + "',";
                }



            }
            //Console.WriteLine("Find My Books bitti");
            return arama;

        }

        private void CloseConnection() {
            try {
                connection.Close();
            } catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public List<String> find_matches() {
            //Console.WriteLine("Find Matches cagirildi");
            int userid = Int32.Parse(Properties.Settings.Default.userID);
            String arama = find_my_books();

            OpenConnection();
            List<String> eslesenler = new List<String>(); ;
            MySqlDataReader dr;

            string sorgu = " Select distinct user_id from ratings where isbn in (" + arama + ") and  not user_id = @userid";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@userid", userid.ToString());
            dr = command.ExecuteReader();

            while (dr.Read()) {
                eslesenler.Add(dr[0].ToString());
            }
            dr.Close();
            dr.Dispose();
            CloseConnection();
            //Console.WriteLine("Find Matches bitti");
            return eslesenler;

        }

        public int find_matches_value(int userid2) {
            //Console.WriteLine("find_matches_value cagirildi");
            int userid = Int32.Parse(Properties.Settings.Default.userID);
            int ortak_kitap_sayisi = 0;
            List<String> eslesenler = new List<String>(); ;
            MySqlDataReader dr;
            OpenConnection();
            string sorgu = "SELECT count(*) as eslesme from ratings WHERE user_id=@id1 and user_id=@id2";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            command.Parameters.AddWithValue("@id1", userid.ToString());
            command.Parameters.AddWithValue("@id2", userid2.ToString());
            dr = command.ExecuteReader();
            while (dr.Read()) {
                ortak_kitap_sayisi = dr.GetInt32(dr.GetOrdinal("eslesme"));
            }
            dr.Close();
            dr.Dispose();
            CloseConnection();
            //Console.WriteLine("find_matches_value bitti");
            return ortak_kitap_sayisi;
        }


        public List<string> find_rates() {
            //Console.WriteLine("find_rates cagirildi");
            List<String> idler = find_matches();
            String ids = "";
            String arama = find_my_books();
            int userid = Int32.Parse(Properties.Settings.Default.userID);

            for (int i = 0; i < idler.Count; i++) {
                if (i == idler.Count - 1) {
                    ids = ids + "'" + idler[i] + "'";
                } else {
                    ids = ids + "'" + idler[i] + "',";
                }

            }

            List<String> id = new List<String>();
            List<String> kitap = new List<String>();
            List<String> oy = new List<String>();
            List<double> benzerlik = new List<double>();
            int eslesme_sayisi;

            MySqlDataReader dr;
            OpenConnection();
            string sorgu = "SELECT user_id , GROUP_CONCAT(isbn SEPARATOR ', ') as kitap ,GROUP_CONCAT(book_rating SEPARATOR ', ') as oy from ratings WHERE user_id in (" + ids + ") and not isbn in (" + arama + ") GROUP BY user_id";
            MySqlCommand command = new MySqlCommand(sorgu, connection);
            dr = command.ExecuteReader();
            while (dr.Read()) {
                id.Add(dr["user_id"].ToString());
                kitap.Add(dr["kitap"].ToString());
                oy.Add(dr["oy"].ToString());
            }
            dr.Close();
            dr.Dispose();
            CloseConnection();


            for (int i = 0; i < id.Count; i++) {
                try {
                    eslesme_sayisi = find_matches_value(Int32.Parse(id[i]));
                    benzerlik.Add(cosine_similarity(eslesme_sayisi));
                } catch (Exception) {
                }

            }


            List<string> oneri_kitaplar = oneri(id, kitap, oy, benzerlik);

            //Console.WriteLine("find_rates bitti");
            return oneri_kitaplar;
        }

        public int Compare(List<string> x, List<string> y) {
            return (Int32.Parse(x[2]) > Int32.Parse(y[2])) ? 1 : (Int32.Parse(x[2]) == Int32.Parse(y[2])) ? 0 : -1;
        }

        public List<string> oneri(List<String> idler, List<String> kitaplar, List<String> oylar, List<double> oranlar) {
            //Console.WriteLine("oneri cagirildi");
            List<List<string>> kitap_benzerligi = new List<List<string>>();

            for (int i = 0; i < idler.Count - 1; i++) {
                List<string> tum_kitaplar = new List<string>();
                string[] ayrik_kitap = kitaplar[i].Split(',');
                string[] ayrik_benzerlik = oylar[i].Split(',');
                //Console.WriteLine(ayrik_benzerlik.Length + " , " + ayrik_kitap.Length);
                for (int j = 0; j < ayrik_benzerlik.Length; j++) {
                    Double n;
                    var isNumeric = Double.TryParse(ayrik_benzerlik[j], out n);
                    if (isNumeric) {
                        ayrik_benzerlik[j] = (n * oranlar[i]).ToString();
                        tum_kitaplar.Add(ayrik_kitap[j]);
                    }
                }

                for (int j = 0; j < tum_kitaplar.Count; j++) {
                    int zaten_var = -1;
                    for (int k = 0; k < kitap_benzerligi.Count; k++) {
                        if (kitap_benzerligi[k][0] == tum_kitaplar[j]) {
                            zaten_var = k;
                        }

                    }

                    if (zaten_var > -1) {
                        kitap_benzerligi[zaten_var][1] = (Int32.Parse(kitap_benzerligi[zaten_var][1]) + 1).ToString();
                        //Console.WriteLine(ayrik_benzerlik[j]);
                        //Console.WriteLine(Double.Parse(kitap_benzerligi[zaten_var][2].ToString()));
                        kitap_benzerligi[zaten_var][2] = (Double.Parse(kitap_benzerligi[zaten_var][2].ToString()) + Double.Parse(ayrik_benzerlik[j].ToString())).ToString();
                    } else {
                        List<string> inner = new List<string>();
                        inner.Add(tum_kitaplar[j]);
                        inner.Add("1");
                        inner.Add(ayrik_benzerlik[j].ToString());
                        kitap_benzerligi.Add(inner);
                    }

                }

            }

            List<string> isbnler = new List<string>();
            for (int i = 0; i < kitap_benzerligi.Count; i++) {
                isbnler.Add(kitap_benzerligi[i][0]);
            }

            List<double> benzerlikler = new List<double>();
            for (int i = 0; i < kitap_benzerligi.Count; i++) {
                benzerlikler.Add(Double.Parse(kitap_benzerligi[i][2]) / Double.Parse(kitap_benzerligi[i][1]));
            }

            double tempN = 0;
            string tempIsbn;
            for (int i = 0; i < benzerlikler.Count - 1; i++) {
                for (int j = i + 1; j < benzerlikler.Count; j++) {
                    if (benzerlikler[i] > benzerlikler[j]) {
                        tempN = benzerlikler[j];
                        benzerlikler[j] = benzerlikler[i];
                        benzerlikler[i] = tempN;
                        tempIsbn = isbnler[j];
                        isbnler[j] = isbnler[i];
                        isbnler[i] = tempIsbn;
                    }
                }
            }

            //Console.WriteLine("oneri bitti");
            return isbnler;
        }

        public void getYeni(BookCover[] books) {
            Console.WriteLine("getYeni cagirildi");
            string wrapperBefore = "select p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi from (SELECT * FROM books WHERE isbn IN(";
            string yeniquery = ("SELECT isbn FROM (SELECT * FROM books ORDER BY tarih DESC LIMIT 10");
            string wrapperAfter = ") as b)) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn ORDER BY tarih DESC ";
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
            MySqlCommand cmd = new MySqlCommand(wrapperBefore + yeniquery + wrapperAfter, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
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
            dataReader.Dispose();
            connection.Close();
            for (int i = 0; i < 10; i++) {
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
            for (int i = 0; i < 10; i++) {
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


        public void getPopi(BookCover[] books) {
            Console.WriteLine("getPopi cagirildi");
            string wrapperBefore = "select p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi from (SELECT * FROM books WHERE isbn IN(";
            string popiquery = ("SELECT isbn FROM (SELECT isbn, COUNT(*) as cnt FROM ratings group by isbn ORDER BY cnt DESC LIMIT 15");
            string wrapperAfter = ") as b)) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn ORDER BY oysayisi DESC";
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
            MySqlCommand cmd = new MySqlCommand(wrapperBefore + popiquery + wrapperAfter, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
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
            dataReader.Dispose();
            connection.Close();
            for (int i = 0; i < 10; i++) {
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
            for (int i = 0; i < 10; i++) {
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

        public void getIyi(BookCover[] books) {
            Console.WriteLine("getİyi cagirildi");
            string wrapperBefore = "select p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi from (SELECT * FROM books WHERE isbn IN(";
            string iyiquery = ("SELECT isbn FROM (SELECT isbn, AVG(book_rating) as ortalama FROM ratings GROUP BY isbn ORDER BY ortalama DESC LIMIT 30");
            string wrapperAfter = ") as b)) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn ORDER BY oysayisi DESC";
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
            MySqlCommand cmd = new MySqlCommand(wrapperBefore + iyiquery + wrapperAfter, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
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
            dataReader.Dispose();
            connection.Close();
            for (int i = 0; i < 10; i++) {
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
            for (int i = 0; i < 10; i++) {
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

        public void getOneri(BookCover[] books, string isbnler) {
            Console.WriteLine("getOneri cagirildi");
            string wrapperBefore = "select p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi from (SELECT * FROM books WHERE isbn IN(";
            string wrapperAfter = ")) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn ORDER BY oysayisi DESC";
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
            MySqlCommand cmd = new MySqlCommand(wrapperBefore + isbnler + wrapperAfter, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
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
            dataReader.Dispose();
            connection.Close();
            for (int i = 0; i < 10; i++) {
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
            for (int i = 0; i < 10; i++) {
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

        public void getBooks(BookCover[] books, int sayfa) {
            Console.WriteLine("getbooks cagirildi");
            disableClick();
            var where = aramaParamteresi != "" ? "book_title LIKE '%" + (aramaParamteresi) + "%'" : "1";
            string query = ("SELECT p1.isbn, year_of_publication, publisher, tarih, book_title, book_author, image_url_l, sum(book_rating)/count(book_rating) as yildiz, count(book_rating) as oysayisi FROM (SELECT * FROM books WHERE " + where + " LIMIT " + (sayfa - 1) * 21 + ",21) as p1 LEFT JOIN (SELECT * FROM ratings) as p2 on p1.isbn=p2.isbn GROUP BY p1.isbn;");
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
            while (dataReader.Read()) {
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
            for (int i = 0; i < 21; i++) {
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
            for (int i = 0; i < list[0].Count; i++) {
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

        public Read() {
            InitializeComponent();

            popibw.DoWork += delegate {
                BookCover[] popis = new BookCover[] { popi1, popi2, popi3, popi4, popi5, popi6, popi7, popi8, popi9, popi10 };
                getPopi(popis);
            };
            iyibw.DoWork += delegate {
                BookCover[] iyis = new BookCover[] { iyi1, iyi2, iyi3, iyi4, iyi5, iyi6, iyi7, iyi8, iyi9, iyi10 };
                getIyi(iyis);
            };
            yenibw.DoWork += delegate {
                BookCover[] yenis = new BookCover[] { yeni1, yeni2, yeni3, yeni4, yeni5, yeni6, yeni7, yeni8, yeni9, yeni10 };
                getYeni(yenis);
            };
            oneribw.DoWork += delegate {
                BookCover[] oneris = new BookCover[] { oneri1, oneri2, oneri3, oneri4, oneri5, oneri6, oneri7, oneri8, oneri9, oneri10 };
                List<string> oneriler = find_rates();
                var maxC = oneriler.Count > 20 ? 20 : oneriler.Count;
                string kayit = "";
                for (int i = 0; i < maxC; i++) {
                    kayit += "'"+(oneriler[oneriler.Count() - 1 - i].ToString()).Trim()+"',";
                }
                kayit = kayit.Remove(kayit.Length - 1);
                getOneri(oneris, kayit);
            };

            popibw.RunWorkerCompleted += delegate {
                Console.WriteLine("POPİ BİTTİ");
                this.Dispatcher.Invoke(() => {
                    popiyukleniyor.Visibility = Visibility.Hidden;
                });
                iyibw.RunWorkerAsync();
            };

            iyibw.RunWorkerCompleted += delegate {
                Console.WriteLine("İYİ BİTTİ");
                this.Dispatcher.Invoke(() => {
                    iyiyukleniyor.Visibility = Visibility.Hidden;
                });
                yenibw.RunWorkerAsync();
            };

            yenibw.RunWorkerCompleted += delegate {
                Console.WriteLine("YENİ BİTTİ");
                this.Dispatcher.Invoke(() => {
                    yeniyukleniyor.Visibility = Visibility.Hidden;
                });
                oneribw.RunWorkerAsync();
            };

            oneribw.RunWorkerCompleted += delegate {
                Console.WriteLine("ÖNERİ BİTTİ");
                this.Dispatcher.Invoke(() => {
                    oneriyukleniyor.Visibility = Visibility.Hidden;
                    pg.IsEnabled = true;
                    aramakismi.IsEnabled = true;
                });
            };


            bw.DoWork += delegate {
                BookCover[] books = new BookCover[] { book1, book2, book3, book4, book5, book6, book7, book8, book9, book10, book11, book12, book13, book14, book15, book16, book17, book18, book19, book20, book21 };
                getBooks(books, pg.n);
            };
            bw.RunWorkerCompleted += delegate {
                Console.WriteLine("BİTTİ");
                enableClick();
                if (!zatenCekildi) {
                    popibw.RunWorkerAsync();
                    zatenCekildi = true;
                }
            };
            label.Content = "Hoş geldin, " + Properties.Settings.Default.userNick;
            pg.nmax = getPageCount();
            pg.bul();

        }


        public void disableClick() {
            this.Dispatcher.Invoke(() => {
                yukleniyor.Visibility = Visibility.Visible;
                Tumu.IsEnabled = false;
            });
        }

        public void enableClick() {
            this.Dispatcher.Invoke(() => {
                yukleniyor.Visibility = Visibility.Hidden;
                Tumu.IsEnabled = true;
            });
        }

        private void araButonu_Click(object sender, RoutedEventArgs e) {
            if (kitapAraInput.Text == "") {
                aramaLabel.Content = "Tüm Kitaplar";
                notSearch.Visibility = Visibility.Visible;
                kucult.Height = 3000;
            } else {
                aramaLabel.Content = "Aranan: " + kitapAraInput.Text;
                notSearch.Visibility = Visibility.Collapsed;
                kucult.Height = 780;
            }

            aramaParamteresi = kitapAraInput.Text;
            pg.n = 1;
            pg.nmax = getPageCount();
            pg.bul();
        }

        private void book_MouseDown(object sender, MouseButtonEventArgs e) {
            BookCover selectedBook = (BookCover)sender;
            Console.WriteLine(selectedBook.Isbn);
            string[] values = { selectedBook.Isbn, selectedBook.BookName, selectedBook.Stars, selectedBook.VoteCount, selectedBook.Image, selectedBook.Author, selectedBook.Publisher, selectedBook.YearOfPublication, selectedBook.Date, "ready" };
            BookInner book = new BookInner(values);
            book.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e) {
            System.Windows.Application.Current.Shutdown();
        }

        private void AYRAC_MouseDown(object sender, MouseButtonEventArgs e) {
            kitapAraInput.Text = "";
            aramaLabel.Content = "Tüm Kitaplar";
            notSearch.Visibility = Visibility.Visible;
            kucult.Height = 3000;
            aramaParamteresi = "";
            pg.n = 1;
            pg.nmax = getPageCount();
            pg.bul();
        }

        private void kitapAraInput_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter || e.Key == Key.Return) {
                aramaLabel.Content = "Aranan: " + kitapAraInput.Text;
                notSearch.Visibility = Visibility.Collapsed;
                kucult.Height = 780;
                aramaParamteresi = kitapAraInput.Text;
                pg.n = 1;
                pg.nmax = getPageCount();
                pg.bul();
            }
        }
    }
}

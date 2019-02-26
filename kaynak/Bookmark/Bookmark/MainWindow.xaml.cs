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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using System.Windows.Threading;

namespace Bookmark {
    //https://www.codeproject.com/Articles/43438/Connect-C-to-MySQL
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {



        public MySqlConnection connection;

        public bool sql(String query) {
            try {
                connection.Open();
            } catch (MySqlException) {
                return false;
            }

            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQuery();

            try {
                connection.Close();
            } catch (MySqlException ex) {
                return false;
            }

            return true;
        }


        public List<string>[] getUserPoints(string[] selectedBooks) {
            string cumulativeUnion = "";
            foreach (var book in selectedBooks) {
                cumulativeUnion += " union SELECT * FROM `ratings` WHERE isbn=" + book;
            }
            cumulativeUnion = cumulativeUnion.Substring(7, cumulativeUnion.Length - 7);
            string query = ("SELECT * FROM ( SELECT isbn,SUM(rating*count) as point FROM (SELECT user_id, COUNT(user_id) as count FROM (" + cumulativeUnion + ") as userPoints GROUP BY user_id) as userPoints LEFT JOIN (SELECT user_id,book_rating-5 as rating,isbn FROM ratings WHERE user_id IN ( SELECT user_id FROM (" + cumulativeUnion + ") AS userIDs GROUP BY user_id)) as keke ON userPoints.user_id = keke.user_id GROUP BY isbn ) AS allPoints ORDER BY point DESC LIMIT 25");
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
                list[0].Add(dataReader["isbn"] + "");
                list[1].Add(dataReader["point"] + "");
            }
            dataReader.Close();
            connection.Close();
            return list;
        }

        public List<string>[] getAllBooks(int page) {
            string query = ("");
            List<string>[] list = new List<string>[2];
            list[0] = new List<string>();
            list[1] = new List<string>();
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read()) {
                list[0].Add(dataReader["isbn"] + "");
                list[1].Add(dataReader["point"] + "");
            }
            dataReader.Close();
            connection.Close();
            return list;
        }



        DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        private void dispatcherTimer_Tick(object sender, EventArgs e) {
            if (Properties.Settings.Default.userID != "") {
                Read kitaplar = new Read();
                kitaplar.Show();
                this.Hide();
            }
            dispatcherTimer.Stop();
        }


        public MainWindow() {
            InitializeComponent();

            connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");

            string[] selectedBooks = new string[] { "0374157065", "0679425608", "3257227329", "3442353866", "0767906918", "0679865691", "0060938412", "9783423114578", "0060930365", "0553062042" };
            //sql("INSERT INTO users VALUES(NULL, 'yes', '4')");
            /*
            List <string>[] keke = getUserPoints(selectedBooks);
            for(int i = 0; i < keke[0].Count; i++)
            {
                Console.WriteLine(keke[0][i] + " " + keke[1][i]);
                //Console.WriteLine("'"+keke[0][i]+"' ");
            }
            */

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer.Start();

        }

        // Ana form kapatıldığında tüm formları kapat
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private void openLoginForm(object sender, RoutedEventArgs e) {

            Login girisFormu = new Login(this);
            girisFormu.Show();
            this.Hide();
        }

        private void openRegisterForm(object sender, RoutedEventArgs e) {
            Register kayitFormu = new Register(this);
            kayitFormu.Show();
            this.Hide();
        }
    }
}

using MySql.Data.MySqlClient;
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

namespace yoneticipanel
{
    /// <summary>
    /// Interaction logic for kitap_ekle.xaml
    /// </summary>
    public partial class kitap_ekle : Window
    {
        string connectionString = "SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;";
        string query;
        public kitap_ekle()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string isbn1 = "'" + isbn.Text + "',";
            string title1 = "'" + title.Text + "',";
            string author1 = "'" + author.Text + "',";
            string year1 = "'" + year.Text + "',";
            string publisher1 = "'" + publisher.Text + "',";
            string imgs1 = "'" + imgs.Text + "',";
            string imgm1 = "'" + imgm.Text + "',";
            string imgl1 = "'" + imgl.Text + "'";

            string tum = isbn1 + title1 + author1 + year1 + publisher1 + imgs1 + imgm1 + imgl1;
            query = ("INSERT INTO books (isbn, book_title, book_author,year_of_publication,publisher,image_url_s,image_url_m,image_url_l)" +
    " VALUES(" + tum + "); ");

            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlDataReader dr;
            MySqlCommand command = new MySqlCommand(query, connection);
            connection.Open();
            dr = command.ExecuteReader();
            while (dr.Read())
            {

            }
            connection.Close();
            this.Close();
        }
    }
}

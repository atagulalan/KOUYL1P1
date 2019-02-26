using MySql.Data.MySqlClient;
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
using System.Windows.Shapes;

namespace yoneticipanel
{
    /// <summary>
    /// Interaction logic for kitap_sil.xaml
    /// </summary>
    public partial class kitap_sil : Window
    {

        string connectionString = "SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;";
        public kitap_sil()
        {
            InitializeComponent();


            ara.Items.Add("isbn");
            ara.Items.Add("book_title");
            ara.Items.Add("book_author");
            ara.Items.Add("year_of_publication");
            ara.Items.Add("image_url_s");
            ara.Items.Add("image_url_m");
            ara.Items.Add("image_url_l");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string selected = ara.SelectedValue.ToString();

            string Query = "DELETE FROM books WHERE " + selected + " = '" + metin.Text + "' ;";
            MessageBox.Show(Query);
            MySqlConnection connection = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Query, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            this.Close();
        }

    }
}

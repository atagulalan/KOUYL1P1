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
    /// Interaction logic for kullanici_sil.xaml
    /// </summary>
    public partial class kullanici_sil : Window
    {
        string connectionString = "SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;";

        public kullanici_sil()
        {
            InitializeComponent();

            ara.Items.Add("user id");
            ara.Items.Add("nick");
            ara.Items.Add("pw");
            ara.Items.Add("location");
            ara.Items.Add("age");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
               string selected = ara.SelectedValue.ToString();

                string Query = "DELETE FROM users WHERE "+selected+" = '"+metin.Text+"' ;";
                MySqlConnection connection = new MySqlConnection(connectionString);
                MySqlCommand command = new MySqlCommand(Query, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            this.Close();
         
        }
    }
}

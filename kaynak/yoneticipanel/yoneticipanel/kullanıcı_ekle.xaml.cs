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
    /// Interaction logic for kullanıcı_ekle.xaml
    /// </summary>
    public partial class kullanıcı_ekle : Window
    {
        string connectionString = "SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;";
        string query;
        public kullanıcı_ekle()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string nick ="'"+ t1.Text + "',";
            string pw = "'" + t2.Password.ToString() + "',";
            string location = "'" + t3.Text +","+ t4.Text + "',";
            string age = "'" + t5.Text + "'";
            string tum =nick+pw+location+age;
            query = ("INSERT INTO users (nick, pw, location,age)" +
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

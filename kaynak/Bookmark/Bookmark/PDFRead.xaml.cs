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
using System.Security.Cryptography;

namespace Bookmark
{
    /// <summary>
    /// Interaction logic for BookInner.xaml
    /// </summary>
    public partial class PDFRead : Window
    {
        public MySqlConnection connection = new MySqlConnection("SERVER=localhost;DATABASE=bookmark;UID=root;PASSWORD=;SslMode=none;");

        public PDFRead()
        {
            InitializeComponent();
        }

        public PDFRead(string isbn)
        {
            InitializeComponent();
            //https://stackoverflow.com/questions/26870267/generate-integer-based-on-any-given-string-without-gethashcode
            MD5 md5Hasher = MD5.Create();
            var hashed = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(isbn));
            var ivalue = (Math.Abs(BitConverter.ToInt32(hashed, 0)) % 5) + 1;
            wb.Source = new Uri("pack://siteoforigin:,,,/books/" + ivalue + ".pdf");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookmark
{
    /// <summary>
    /// Interaction logic for BookCover.xaml
    /// </summary>
    public partial class BookCover : UserControl
    {
        public string isbn;
        public string bookname;
        public string stars;
        public string votecount;
        public string image;
        public string author;
        public string publisher;
        public string date;
        public string yearofpublication;

        public string Publisher
        {
            get { return publisher; }
            set { publisher = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }
    
        public string YearOfPublication
        {
            get { return yearofpublication; }
            set { yearofpublication = value; }
        }

        public string Isbn
        {
            get { return isbn; }
            set { isbn = value; }
        }

        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        public string BookName
        {
            get { return bookname; }
            set { bookname = value; }
        }

        public string Stars
        {
            get { return stars; }
            set { stars = value; }
        }

        public string VoteCount
        {
            get { return votecount; }
            set { votecount = value; }
        }

        public string Image
        {
            get { return image; }
            set { image = value; }
        }


        public void getImage(string url)
        {
            if (url!=null)
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(url, UriKind.Absolute);
                bitmap.EndInit();
                bgimage.Source = bitmap;
            }
            
        }

        public BookCover()
        {
            InitializeComponent();
        }

        public void cek()
        {
            this.Dispatcher.Invoke(() =>
            { 
                if (isbn!="")
                {
                    allCons.Visibility = Visibility.Visible;
                    getImage(image);
                    label.Text = bookname;
                    label1.Content = votecount + " Oy";
                    if (stars != "")
                    {
                        int intStars = (int)(Convert.ToDouble(stars)/2);
                        if (intStars > 0)
                        {
                            star1.Opacity = 1;
                            if (intStars > 1)
                            {
                                star2.Opacity = 1;
                                if (intStars > 2)
                                {
                                    star3.Opacity = 1;
                                    if (intStars > 3)
                                    {
                                        star4.Opacity = 1;
                                        if (intStars > 4)
                                        {
                                            star5.Opacity = 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    allCons.Visibility = Visibility.Hidden;
                }
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cek();
        }
    }
}

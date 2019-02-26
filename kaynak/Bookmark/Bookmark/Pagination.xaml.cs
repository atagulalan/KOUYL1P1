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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookmark
{
    /// <summary>
    /// Interaction logic for Pagination.xaml
    /// </summary>
    public partial class Pagination : UserControl
    {

        public Pagination()
        {
            InitializeComponent();
        }

        public int nmax = 1;
        public int n = 1;

        public void removeClasses(Button[] ks)
        {
            for (int i = 0; i < 21; i++)
            {
                ks[i].IsEnabled = true;
            }
        }

        public void removeContents(Button[] ks)
        {
            for (int i = 0; i < 21; i++)
            {
                ks[i].Content = "";
            }
        }

        public void sonlar()
        {
            if (nmax > 21)
            {
                if (n + 8 <= nmax - 4)
                {
                    k18.Content = "...";
                    k19.Content = nmax - 2;
                    k20.Content = nmax - 1;
                    k21.Content = nmax;
                }
                else
                {
                    if (n == nmax - 10) { k11.IsEnabled = false; }
                    if (n == nmax - 9) { k12.IsEnabled = false; }
                    if (n == nmax - 8) { k13.IsEnabled = false; }
                    if (n == nmax - 7) { k14.IsEnabled = false; }
                    if (n == nmax - 6) { k15.IsEnabled = false; }
                    if (n == nmax - 5) { k16.IsEnabled = false; }
                    if (n == nmax - 4) { k17.IsEnabled = false; }
                    if (n == nmax - 3) { k18.IsEnabled = false; }
                    if (n == nmax - 2) { k19.IsEnabled = false; }
                    if (n == nmax - 1) { k20.IsEnabled = false; }
                    if (n == nmax) { k21.IsEnabled = false; }
                    k5.Content = nmax - 16;
                    k6.Content = nmax - 15;
                    k7.Content = nmax - 14;
                    k8.Content = nmax - 13;
                    k9.Content = nmax - 12;
                    k10.Content = nmax - 11;
                    k11.Content = nmax - 10;
                    k12.Content = nmax - 9;
                    k13.Content = nmax - 8;
                    k14.Content = nmax - 7;
                    k15.Content = nmax - 6;
                    k16.Content = nmax - 5;
                    k17.Content = nmax - 4;
                    k18.Content = nmax - 3;
                    k19.Content = nmax - 2;
                    k20.Content = nmax - 1;
                    k21.Content = nmax;
                }
            }
        }

        public void normalSirala()
        {
            if (nmax >= 1) { k1.Content = "1"; }
            if (nmax >= 2) { k2.Content = "2"; }
            if (nmax >= 3) { k3.Content = "3"; }
            if (nmax >= 4) { k4.Content = "4"; }
            if (nmax >= 5) { k5.Content = "5"; }
            if (nmax >= 6) { k6.Content = "6"; }
            if (nmax >= 7) { k7.Content = "7"; }
            if (nmax >= 8) { k8.Content = "8"; }
            if (nmax >= 9) { k9.Content = "9"; }
            if (nmax >= 10) { k10.Content = "10"; }
            if (nmax >= 11) { k11.Content = "11"; }
            if (nmax >= 12) { k12.Content = "12"; }
            if (nmax >= 13) { k13.Content = "13"; }
            if (nmax >= 14) { k14.Content = "14"; }
            if (nmax >= 15) { k15.Content = "15"; }
            if (nmax >= 16) { k16.Content = "16"; }
            if (nmax >= 17) { k17.Content = "17"; }
            if (nmax < 22)
            {
                if (nmax >= 18) { k18.Content = "18"; }
                if (nmax >= 19) { k19.Content = "19"; }
                if (nmax >= 20) { k20.Content = "20"; }
                if (nmax >= 21) { k21.Content = "21"; }
            }
            else
            {
                sonlar();
            }
        }

        public void onceNoktali()
        {
            if (n < nmax - 10)
            {
                if (n >= 11)
                {
                    k10.IsEnabled = false;
                }
                k5.Content = (n - 5);
                k6.Content = (n - 4);
                k7.Content = (n - 3);
                k8.Content = (n - 2);
                k9.Content = (n - 1);
                k10.Content = (n);
                k11.Content = (n + 1);
                k12.Content = (n + 2);
                k13.Content = (n + 3);
                k14.Content = (n + 4);
                k15.Content = (n + 5);
                k16.Content = (n + 6);
                k17.Content = (n + 7);
            }
            k1.Content = "1";
            k2.Content = "2";
            k3.Content = "3";
            k4.Content = "...";
            sonlar();
        }

        public void bul()
        {
            Button[] ks = new Button[] { k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15, k16, k17, k18, k19, k20, k21 };
            removeContents(ks);
            removeClasses(ks);
            if (nmax >= 22)
            {
                if (n <= 10)
                {
                    if (n == 1) { k1.IsEnabled = false;}
                    if (n == 2) { k2.IsEnabled = false;}
                    if (n == 3) { k3.IsEnabled = false;}
                    if (n == 4) { k4.IsEnabled = false;}
                    if (n == 5) { k5.IsEnabled = false;}
                    if (n == 6) { k6.IsEnabled = false;}
                    if (n == 7) { k7.IsEnabled = false;}
                    if (n == 8) { k8.IsEnabled = false;}
                    if (n == 9) { k9.IsEnabled = false;}
                    if (n == 10) { k10.IsEnabled = false;}
                    normalSirala();
                }

                if (n > 10)
                {
                    onceNoktali();
                }
            }
            else
            {
                if (n == 1) { k1.IsEnabled = false;}
                if (n == 2) { k2.IsEnabled = false;}
                if (n == 3) { k3.IsEnabled = false;}
                if (n == 4) { k4.IsEnabled = false;}
                if (n == 5) { k5.IsEnabled = false;}
                if (n == 6) { k6.IsEnabled = false;}
                if (n == 7) { k7.IsEnabled = false;}
                if (n == 8) { k8.IsEnabled = false;}
                if (n == 9) { k9.IsEnabled = false;}
                if (n == 10) { k10.IsEnabled = false;}
                if (n == 11) { k11.IsEnabled = false;}
                if (n == 12) { k12.IsEnabled = false;}
                if (n == 13) { k13.IsEnabled = false;}
                if (n == 14) { k14.IsEnabled = false;}
                if (n == 15) { k15.IsEnabled = false;}
                if (n == 16) { k16.IsEnabled = false;}
                if (n == 17) { k17.IsEnabled = false;}
                if (n == 18) { k18.IsEnabled = false;}
                if (n == 19) { k19.IsEnabled = false;}
                if (n == 20) { k20.IsEnabled = false;}
                if (n == 21) { k21.IsEnabled = false;}
                normalSirala();
            }

            var win = Window.GetWindow(this) as Books;
            if (win != null)
            {
                win.paginationChanged();
            }

            var win2 = Window.GetWindow(this) as Read;
            if (win2 != null) {
                win2.paginationChanged();
            }
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            bul();
        }

        private void kClick(object sender, RoutedEventArgs e)
        {
            if (nmax != 0) {
                bool bulma = false;
                string s = (sender as Button).Content.ToString();
                if (s == "<<") {
                    n = 1;
                } else if (s == "<") {
                    if (n > 1) {
                        n--;
                    }

                } else if (s == ">") {
                    if (n < nmax) {
                        n++;
                    }
                } else if (s == ">>") {
                    n = nmax;
                } else if (s == "..." || s == "") {
                    Console.WriteLine("Boşluğa tıklandı");
                    bulma = true;
                } else {
                    int.TryParse(s, out n);
                }
                if (!bulma) {
                    bul();
                }
            }
        }
    }
}

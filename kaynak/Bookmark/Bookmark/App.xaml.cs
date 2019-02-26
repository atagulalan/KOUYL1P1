using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bookmark
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // ŞİFRELERE DİREK OLARAK ERİŞEMEDİĞİMİZ İÇİN ŞİFRE OLUP OLMADIĞINI
        // KONTROL EDİP (ÜST ÜSTE ELEMAN KOYMADIĞIMIZDAN GEREKSİZ GÖRDÜĞÜMÜZ) zINDEX
        // PROPERTYSİNİ 1 VEYA 2 İLE DEĞİŞTİRİYORUZ. XAML DOSYASINA DA 2 İSE 
        // YUKARI ÇIKMASI, AKSİ HALDE AŞAĞI İNMESİ GEREKTİĞİNİ YAZIYORUZ.
        // BİRAZ HİLE GİBİ GÖZÜKEBİLİR, ANCAK PASSWORD PROPERTY'SİNE ERİŞMENİN
        // TEK YOLU BU.
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            Panel.SetZIndex(pb, (!string.IsNullOrEmpty(pb.Password)) ? 1 : 2);
        }
    }

    
}

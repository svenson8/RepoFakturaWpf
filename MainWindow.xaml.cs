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
using FakturaWpf.Documents;
using FakturaWpf.Users;
using FakturaWpf.Tools;
using WPF.MDI;
using System.ComponentModel;
using FakturaWpf.Customer;
using FakturaWpf.Dictionary;

namespace FakturaWpf
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Environment.Exit(-1);
            ///Z domu zmiana
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DocumentList), null, "Lista dokumentów sprzedaży", "ImgFaktura", 450, 1000);
          
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(UserList), null, "Lista uzytkowników", "ImgFaktura", 450, 765);

        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ServerConf.ShowServerConf(false);

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(CustomerList), null, "Lista kontrahentów", "ImgCustomers", 450, 700);
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzGrupK }, "Grupy kontrahentów", "ImgGroup", 450, 675);
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzProv }, "Lista województw", "ImgPoland", 450, 675);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzCountry }, "Lista Państw", "ImgEarth", 450, 675);
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzDokDef }, "Lista definicji dokumentów", "ImgDocuments", 450, 675);
        }
    }
}

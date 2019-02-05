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
            MdiControl.FindMainContainer();
        }


        private void MainWindow1_Closed(object sender, EventArgs e)
        {
            Environment.Exit(-1);
            ///Z domu zmiana
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DocumentList), null, "Lista dokumentów sprzedaży", "faktura sprzedazy16.ico", 450, 800);
          
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(UserList), null, "Lista uzytkowników", "faktura sprzedazy16.ico", 450, 765);

        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);

        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ServerConf.ShowServerConf(false);

        }
    }
}

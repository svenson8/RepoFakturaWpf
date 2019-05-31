﻿using System;
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
using FakturaWpf.Assortment;

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


        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            ServerConf.ShowServerConf(false);

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(CustomerList), new object[] { false }, "Lista kontrahentów", "ImgCustomers", 450, 700);
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzGrupK, false }, "Grupy kontrahentów", "ImgGroup", 450, 675);
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzProv, false }, "Lista województw", "ImgPoland", 450, 675);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzCountry, false }, "Lista Państw", "ImgEarth", 450, 675);
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzDokDef, false }, "Lista definicji dokumentów", "ImgDocuments", 500, 675);
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzPay, false }, "Lista sposobów płątności", "ImgPay", 500, 675);
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DocumentNumber), null, "Numeracja dokumentów", "ImgDocNum", 300, 390);
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(AssortmentList), new object[] { false }, "Lista asortymentów", "ImgStack", 500, 800);
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzAsGroup, false }, "Lista grup asortymentowych", "ImgGroupas", 500, 675);
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzMeasure, false }, "Lista grup asortymentowych", "ImgMeasure", 500, 675);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}

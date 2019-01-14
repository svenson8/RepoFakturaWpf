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
using WPF.MDI;

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
            MdiMain.Children.Add(new MdiChild()
            {
                Title = " Lista dokumentów sprzedaży",
                Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/faktura sprzedazy16.ico")),
                Height = 450,
                Width = 800,
                Content = new DocumentList()
            });


        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MdiMain.Children.Add(new MdiChild()
            {
                Title = " Dane użytkownika uprzywilejowanego",
                Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/faktura sprzedazy16.ico")),
                 Height = 420,
                 Width = 580,
                MaximizeBox = false,
                Resizable = false,
                Content = new UserEdit()
            });

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}

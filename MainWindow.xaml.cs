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
using System.ServiceModel;

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
            lblCursorPosition.Text = "Zalogowano: " + UserClass.UserACTLogin;
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
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzMeasure, false }, "Lista jednostek miar", "ImgMeasure", 500, 675);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*  Uri adres = new Uri("http://localhost:2222/Hello");
              using (var c = new ChannelFactory<IMyWcf>(
                new BasicHttpBinding(),
                new EndpointAddress(adres))) */
            /*   Uri adres = new Uri("net.tcp://localhost:2222/Hello");
               using (var c = new ChannelFactory<IMyWcf>(
                 new NetTcpBinding(),
                 new EndpointAddress(adres))) */
            /*    using (var c = new ChannelFactory<IMyWcf>(""))
                {
                    var s = c.CreateChannel();
                    Various.InfoOk(s.Hello());
                    Various.InfoOk(s.AddNumbers(5, 8).ToString());
                    Various.InfoOk(s.GetCustomerById(5).FirstName);

                    Various.InfoOk(s.GetValue(2000.0M).ToString());
                    List<decimal> valuesList = new List<decimal>(){
                    2000.0M, 2200.0M, 2400.0M,
                    2600.0M, 2800.0M, 3000.0M
                    };
                    List<decimal> returnValues = s.GetValues(valuesList);
                    foreach (var value in returnValues)
                        Various.InfoOk(value.ToString());


                } */

            /*  KnowType
             *  var binding = new BasicHttpBinding();
              ChannelFactory<IContainerSvc> factory = new ChannelFactory<IContainerSvc>(binding);
              EndpointAddress address = new EndpointAddress("http://localhost:8000/ContainerSvc");
              IContainerSvc container = factory.CreateChannel(address);
              var list = container.GetSomeData();
              foreach (var item in list.Items)
                  Console.WriteLine(item.Name) */

            //Klasa nasłuchowa
            var listener = new Listener();

            //Tworzymy fabrykę kanałów i sam kanał
            using (var factory = new DuplexChannelFactory<IRegister>(listener, ""))
            using (var baseChannel = (IClientChannel)factory.CreateChannel())
            {
                IRegister channel = (IRegister)baseChannel;
                //Uruchamiamy zadanie na serwerze,
                //zostaniemy poinformowani o zakończeniu
                channel.RunTask();

                //Wykonujemy inne operacje
                Various.InfoOk("Do other stuff...");

                //Czekamy na odpowiedź
                listener.FinishedEvent.WaitOne();
                //Otrzmyalimy odpowiedź, możemy spokojnie zakończyć pracę
                Various.InfoOk("Exit...");
            }
           // Console.ReadLine();

        }
    }
}

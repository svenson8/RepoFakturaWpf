using FakturaWpf.Tools;
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

namespace FakturaWpf.Customer
{
    /// <summary>
    /// Logika interakcji dla klasy CustomerList.xaml
    /// </summary>
    public partial class CustomerList : UserControl, IMdiControl
    {
        public CustomerList()
        {
            InitializeComponent();
            Prepare();
            
        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_Customer, new int[] { 0, 3 });
            Various.FillWithFiltrItems(CB_Choice.comboBox);
            CB_Choice.comboBox.SelectedIndex = 0;
        }

        private void ComboSearchChange()
        {
            var cbLeft = CB_ChoiceGr.Margin.Left;
            var txLeft = TX_Search.Margin.Left;

            var nearLeft = Math.Min(cbLeft, txLeft);
            var farLeft =  Math.Max(cbLeft, txLeft);

            switch (CB_Choice.comboBox.SelectedIndex)
            {
                case 5 :
                    CB_ChoiceGr.Margin = new Thickness(nearLeft, CB_ChoiceGr.Margin.Top, CB_ChoiceGr.Margin.Right, CB_ChoiceGr.Margin.Bottom);
                    CB_ChoiceGr.Visibility = Visibility.Visible;
                    TX_Search.Margin =  new Thickness(farLeft, TX_Search.Margin.Top, TX_Search.Margin.Right, TX_Search.Margin.Bottom);
                    TX_Search.Visibility = Visibility.Hidden;
                    break;
                case 7:
                    CB_ChoiceGr.Visibility = Visibility.Hidden;
                    TX_Search.Visibility = Visibility.Hidden;
                    break;
                default:
                    CB_ChoiceGr.Margin = new Thickness(farLeft, CB_ChoiceGr.Margin.Top, CB_ChoiceGr.Margin.Right, CB_ChoiceGr.Margin.Bottom);
                    CB_ChoiceGr.Visibility = Visibility.Hidden;
                    TX_Search.Margin = new Thickness(nearLeft, TX_Search.Margin.Top, TX_Search.Margin.Right, TX_Search.Margin.Bottom);
                    TX_Search.Visibility = Visibility.Visible;
                    break;
            }
        }



        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(CustomerList), TreeName());
        }

        public void OnRefresh()
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Lista klientów";
        }

        private void CB_Choice_mySelect(object sender, SelectionChangedEventArgs e)
        {
            ComboSearchChange();
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            CustomerClass cl = new CustomerClass();
            cl.ThisTableCheck();
        }
    }
}

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
        }


        private void SelectChoice(object sender, SelectionChangedEventArgs e)
        {
           // Various.InfoOk(CB_Choice.comboBox.SelectedItem.ToString());
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
    }
}

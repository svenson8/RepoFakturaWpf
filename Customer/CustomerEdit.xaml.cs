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
    /// Logika interakcji dla klasy CustomerEdit.xaml
    /// </summary>
    public partial class CustomerEdit : UserControl, IMdiControl
    {
        private CustomerClass customer;

        public CustomerEdit(int id)
        {
            InitializeComponent();

            InitBinding(id);
        }

        void InitBinding(int id)
        {
            customer = new CustomerClass(id);
            customer.ACTIVE = "T";

            GR_Cus.DataContext = customer;

        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(CustomerEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Edycja kontrahenta";
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            if (customer.SaveCustomer())
            {
                Various.InfoOk("Klient zapisany", "Informacja");
                MdiControl.RefreshMdi(typeof(CustomerList));
            }
            else
                Various.Warning("Błąd zapisu danych", "");

            Close(sender, e);
        }
    }
}

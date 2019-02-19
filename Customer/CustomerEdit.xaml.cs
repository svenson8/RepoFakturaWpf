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
        public CustomerEdit(int id)
        {
            InitializeComponent();
        }




        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(CustomerEdit), TreeName());
        }

        public void OnRefresh()
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Edycja kontrahenta";
        }
    }
}

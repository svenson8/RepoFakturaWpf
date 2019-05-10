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

namespace FakturaWpf.Documents
{
    /// <summary>
    /// Logika interakcji dla klasy DocumentNumber.xaml
    /// </summary>
    public partial class DocumentNumber : UserControl, IMdiControl
    {
        public DocumentNumber()
        {
            InitializeComponent();
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentNumber), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Tworzenie numeru";
        }
    }
}

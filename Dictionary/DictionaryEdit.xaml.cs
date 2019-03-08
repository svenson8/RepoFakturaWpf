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

namespace FakturaWpf.Dictionary
{
    /// <summary>
    /// Logika interakcji dla klasy DictionaryEdit.xaml
    /// </summary>
    public partial class DictionaryEdit : UserControl, IMdiControl
    {
        private DictionaryClass mydict;

        public DictionaryEdit(int id=0)
        {
            InitializeComponent();

            mydict = new DictionaryClass(id);
            InitBinding();
        }

        private void InitBinding()
        {
            SP_SLow.DataContext = mydict;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DictionaryEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Grupy kontr.";   
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            if (mydict.ThisSaveData())
            {
                Various.InfoOk("Słownik zapisany", "Informacja");
                MdiControl.RefreshMdi(typeof(DictionaryList), mydict);
            }
            else
                Various.Warning("Błąd zapisu danych", "");

            Close(sender, e);
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }
    }
}

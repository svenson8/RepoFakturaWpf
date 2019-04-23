using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
  /*  enum Operationa
    {
        none = 0, // brak operacji
    addition, // dodawanie
    subtraction, // odejmowanie
    multiplication, // mnożenie
    division, // dzielenie
    result // wynik
    }*/
    /// <summary>
    /// Logika interakcji dla klasy UserControl1.xaml
    /// </summary>
    public partial class DocumentList : UserControl, IMdiControl
    {
        private class ItemDok
        {
            public string desc { get; set; }
            public int id { get; set; }
            public bool IsChecked { get; set; }

            public ItemDok(string d, int i, bool v)
            {
                this.desc = d;
                this.id = i;
                this.IsChecked = v;
            }
        }

        List<ItemDok> ListData;

        public DocumentList()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {

            InitDokDef();
           
        }

        void InitDokDef()
        {
            ListData = new List<ItemDok>();
            NQueryReader nq = new NQueryReader("select *, SLKOMUN2 +' '+SLKOMUN1 as concat from TSlownik " +
                                               "where slrodz =" + Various.QuotedStr(DictionaryClass.slRodzDokDef));
            while (nq.NReader.Read())
                ListData.Add(new ItemDok((string)nq.NReader["concat"], (int)nq.NReader["ID"], false ));

            LB_Dok.ItemsSource = ListData;


        }





        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Lita dokum.";
        }


        private void Btn_Refresh_myClick(object sender, RoutedEventArgs e)
        {
            var selecteds = ListData.Where(ps => ps.IsChecked);
        }
    }
}

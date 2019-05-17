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
        List<DocumentClass> listD = null;

        public DocumentList()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_DocList, new int[] { 3, 4 });
            InitDokDef();
            LoadData();
           
        }

        private void LoadData()
        {
            if (listD == null)
            {
                DocumentClass dl = new DocumentClass();
                listD = dl.ThisReadListData().OfType<DocumentClass>().ToList();

                foreach (DocumentClass d in listD)
                    d.CHECK = false;
            }

            List<DocumentClass> lpom = listD.Select(x => x).ToList();

            DG_DocList.ItemsSource = lpom;
            DG_DocList.SelectedIndex = 0;

        }

        void InitDokDef()
        {
            ListData = new List<ItemDok>();
            NQueryReader nq = new NQueryReader("select *, SLKOMUN2 +' '+SLKOMUN1 as concat from TSlownik " +
                                               "where slrodz =" + Various.QuotedStr(DictionaryClass.slRodzDokDef));
            while (nq.NReader.Read())
                ListData.Add(new ItemDok((string)nq.NReader["concat"], (int)nq.NReader["ID"], true ));

            nq.NReader.Close();

            LB_Dok.ItemsSource = ListData;


        }


        private void EditPosition(Boolean mod)
        {
            DocumentClass document = (DocumentClass)DG_DocList.SelectedItem;

            int id = 0;
            var title = "Redagowanie danych ";

            if (mod)
            {
                id = document.ID;
                title += document.MDNRDOK;
            }

            if (id >= 0)
            {
                MdiControl.AddChild(typeof(DocumentEdit), new object[] { id }, title, "ImgFakt", 600, 930, TreeName());
            }
        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            DocumentClass csMod = ((DocumentClass)obj);

            var index = listD.FindIndex(x => x.ID == csMod.ID);

            if (index > -1)
                listD[index] = csMod;
            else
                listD.Add(csMod);

            LoadData();
        }

        public string TreeName()
        {
            return "Lita dokum.";
        }


        private void Btn_Refresh_myClick(object sender, RoutedEventArgs e)
        {
            var selecteds = ListData.Where(ps => ps.IsChecked);
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            DataPanel.GetSqlDates("dat1");
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition(false);
        }

        private void btnMod_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition(true);
        }

        private void DG_DocList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPosition(true);
        }
    }
}

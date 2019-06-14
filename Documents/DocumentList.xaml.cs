using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

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
            this.Focusable = true;
        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_DocList, new int[] { 3, 4 });
            InitDokDef();
            DataPanel.CB_Period.comboBox.Text = "Bieżący rok";
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

            lpom = lpom.Where(x => x.ACTIVE == (CH_archiwe.IsChecked == false ? "T" : "N")).ToList();

            if (CH_Cancel.IsChecked == true)
                lpom = lpom.Where(x => x.MDSTATUS == "A").ToList();

            List<ItemDok> seldok = ListData.Where(ps => ps.IsChecked).ToList();

            lpom = lpom.Where(x => seldok.Select(o => o.id).ToArray().Contains(x.MDDOKDEFID)).ToList();

            DataPanel.GetDates();
            lpom = lpom.Where(x => (x.MDDATAWYST >= DataPanel.dateFrom && x.MDDATAWYST <= DataPanel.dateTo)).ToList();

            col_zam.Visibility = (Ch_zam.IsChecked == true ? Visibility.Visible : Visibility.Collapsed);
            col_wg.Visibility  = (Ch_wgdok.IsChecked == true ? Visibility.Visible : Visibility.Collapsed);

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

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            LoadData();
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DocumentClass document = (DocumentClass)DG_DocList.SelectedItem;
            document.MDSTATUS = "A";
            NQuery n = new NQuery("update " + document.TableName() + " set " + nameof(document.MDSTATUS) + "='A' where ID=" + document.ID);
            LoadData();
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            if (Various.Question("Czy na pewno usunąć dokument ?"))
            {
                DocumentClass doc = (DocumentClass)DG_DocList.SelectedItem;

                if (doc.DeletPosition(doc.ID, doc.TableName()))
                {
                    var index = listD.FindIndex(x => x.ID == doc.ID);
                    if (index > -1)
                        listD.RemoveAt(index);

                    Various.InfoOk("Dokument usunięty pomyślnie");

                    LoadData();
                }
            }
        }


        private void US_DocumentList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close(sender, e);

            if (e.Key == Key.Insert)
                btIns_myClick(sender, e);

            if (e.Key == Key.Delete)
                MyButton_myClick_1(sender, e);

            if (e.Key == Key.F6)
                btnMod_myClick(sender, e);

            if (e.Key == Key.F3)
                MyButton_myClick(sender, e);
        }

        private void MyButton_myClick_2(object sender, RoutedEventArgs e)
        {
              
              PrintDialog dlgPrint = new PrintDialog();
              WpfPrinting p = new WpfPrinting();
              p.PrintDataGrid(null, DG_DocList, null, dlgPrint, true, false, false);

                 


        }






    }
}

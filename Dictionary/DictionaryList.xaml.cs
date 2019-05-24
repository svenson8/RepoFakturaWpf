using FakturaWpf.Assortment;
using FakturaWpf.MyControls;
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
    /// Logika interakcji dla klasy DictionaryList.xaml
    /// </summary>
    public partial class DictionaryList : UserControl, IMdiControl
    {
        List<DictionaryClass> listD = null;
        string slowkind;
        private Boolean choice;

        public DictionaryList(string slowkind, Boolean achoice = false)
        {
            InitializeComponent();

            this.slowkind = slowkind;
            choice = achoice;
            Prepare();
        }


        void Prepare()
        {
            Various.FillWithFiltrItems(CB_Choice.comboBox, 2);
            CB_Choice.comboBox.SelectedIndex = 0;
            DGC_Code.Visibility = (new[] { DictionaryClass.slRodzCountry, DictionaryClass.slRodzAsGroup }.Contains(slowkind)) ? 
                                  Visibility.Visible : 
                                  Visibility.Hidden;
            if (!choice)
                btCho.Visibility = Visibility.Collapsed;

            LoadData();
        }

        public void LoadData()
        {
            if (listD == null)
            {
                DictionaryClass dl = new DictionaryClass(0, slowkind);
                listD = dl.ThisReadListData().OfType<DictionaryClass>().ToList();
            }

            List<DictionaryClass> lpom = listD.Select(x => x).ToList();

            if (TX_Search.Text.Length > 0)
            {
                switch (CB_Choice.comboBox.SelectedIndex)
                {
                    case 0:
                        lpom = listD.Where(x => x.SLKOMUN1.ToUpper().StartsWith(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 1:
                        lpom = listD.Where(x => x.SLKOMUN1.ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                }
            }

            DG_Dict.ItemsSource = lpom;
            DG_Dict.SelectedIndex = 0;

        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DictionaryList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            DictionaryClass dcMod = ((DictionaryClass)obj);

            var index = listD.FindIndex(x => x.ID == dcMod.ID);

            if (index > -1)
                listD[index] = dcMod;
            else
                listD.Add(dcMod);

            LoadData();
        }

        public string TreeName()
        {
            return "Grupy kontrah.";
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void EditPosition(Boolean mod)
        {
            DictionaryClass dc = (DictionaryClass)DG_Dict.SelectedItem;

            int id = 0;

            if (mod)
            {
                id = dc.ID;
            }

            if (id >= 0)
            {
                MdiControl.AddChild(typeof(DictionaryEdit), new object[] { id, slowkind }, "Eydcja ...", "ImgFakt", 180, 550, TreeName());
            }
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition((((MyButton)sender).Name.Equals("btnMod")));
        }

        private void DG_Dict_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPosition(true);
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            if (Various.Question("Czy na pewno usunąć pozycję ?"))
            {
                DictionaryClass dc = (DictionaryClass)DG_Dict.SelectedItem;

                if (dc.DeletPosition(dc.ID, dc.TableName()))
                {
                    var index = listD.FindIndex(x => x.ID == dc.ID);
                    if (index > -1)
                        listD.RemoveAt(index);

                    Various.InfoOk("Pozycja usunięta pomyślnie");
                    LoadData();
                }
            }
        }

        private void btCho_myClick(object sender, RoutedEventArgs e)
        {
            DictionaryClass dict = (DictionaryClass)DG_Dict.SelectedItem;

            if (dict != null)
                MdiControl.RefreshMdi(typeof(AssortmentEdit), dict);

            Close(sender, e);
        }
    }
}

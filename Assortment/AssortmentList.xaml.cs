using FakturaWpf.Dictionary;
using FakturaWpf.Documents;
using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FakturaWpf.Assortment
{
    /// <summary>
    /// Logika interakcji dla klasy AssortmentList.xaml
    /// </summary>
    public partial class AssortmentList : UserControl, IMdiControl
    {
        private List<AssortmentClass> listA;
        private int idGroup = 0;
        private bool choice;

        public AssortmentList(Boolean achoice =false)
        {
            InitializeComponent();
            choice = achoice;
            Prepare();

        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_AsList, new int[] { 0 });
            InitCbChoice();
            CbChoiceChange();
            FillgroupTree();

            btCho.Visibility = (choice == true ? Visibility.Visible : Visibility.Collapsed);

            LoadData();
        }

        private void FillgroupTree()
        {
            TreeViewItem itemM = new TreeViewItem();
            itemM.Header = "Grupy asortymentowe";
            itemM.FontWeight = FontWeights.Bold;

            NQueryReader nq = new NQueryReader("select *, SLKOMUN2 +' ('+SLKOMUN1+')' as concat from TSlownik " +
                                               "where slrodz =" + Various.QuotedStr(DictionaryClass.slRodzAsGroup));
            while (nq.NReader.Read())
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = (string)nq.NReader["concat"];
                item.Tag = nq.NReader["ID"];
                item.FontWeight = FontWeights.Normal;
                itemM.Items.Add(item);
            }
            nq.NReader.Close();

            groupTV.Items.Add(itemM);
            TreeControl.ExpandRecursively(groupTV.Items, true);
        }

        private void LoadData()
        {
            if (listA == null)
            {
                AssortmentClass ac = new AssortmentClass();
                listA = ac.ThisReadListData().OfType<AssortmentClass>().ToList();
            }

            List<AssortmentClass> lpom = listA.Select(x => x).ToList();

            lpom = lpom.Where(x => x.ACTIVE == (CH_archiwe.IsChecked == false ? "T" : "N")).ToList();

            if (idGroup > 0)
                lpom = lpom.Where(x => x.ASGRUPAID == idGroup).ToList();

            if ((TX_Search.Text.Length > 0) || (CB_Choice.comboBox.SelectedIndex == 2))
            {
                switch (CB_Choice.comboBox.SelectedIndex)
                {
                    case 0:
                        lpom = lpom.Where(x => x.ASNAZWA.ToUpper().StartsWith(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 1:
                        lpom = lpom.Where(x => x.ASNAZWA.ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 2:
                        lpom = lpom.Where(x => x.ASTYP == CB_ChoiceGr.comboBox.SelectedIndex).ToList();
                        break;
                }  
            }; 


            DG_AsList.ItemsSource = lpom;
            DG_AsList.SelectedIndex = 0;
        }

        private void CbChoiceChange()
        {
            CB_ChoiceGr.Visibility = Visibility.Collapsed;
            TX_Search.Visibility = Visibility.Collapsed;

            switch (CB_Choice.comboBox.SelectedIndex)
            {
                case 0:
                    TX_Search.Visibility = Visibility.Visible;
                    break;
                case 2:
                    CB_ChoiceGr.Visibility = Visibility.Visible;
                    break;
                default:
                    TX_Search.Visibility = Visibility.Visible;
                    break;
            }

        }

        private void InitCbChoice()
        {
            CB_Choice.comboBox.Items.Clear();
            CB_Choice.comboBox.Items.Add("Wg początku nazwy");
            CB_Choice.comboBox.Items.Add("Wg fragmentu nazwy");
            CB_Choice.comboBox.Items.Add("Wg typu asortymentu");
            CB_Choice.comboBox.Items.Add("Wg kodu kreskowego");

            CB_Choice.comboBox.SelectedIndex = 0;

            Various.InitCbAssortType(CB_ChoiceGr.comboBox);

        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(AssortmentList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            AssortmentClass csMod = ((AssortmentClass)obj);

            var index = listA.FindIndex(x => x.ID == csMod.ID);

            if (index > -1)
                listA[index] = csMod;
            else
                listA.Add(csMod);

            LoadData();
        }

        public string TreeName()
        {
            return "Lista asortymentów";
        }

        private void EditPosition(Boolean mod)
        {       
            int id = 0;
          
            if (mod)
            {
                AssortmentClass assort = (AssortmentClass)DG_AsList.SelectedItem;
                id = assort.ID;                
            }  

            if (id >= 0)
            {
                MdiControl.AddChild(typeof(AssortmentEdit), new object[] { id }, "Edycja asortymentu", "ImgStack", 480, 880, TreeName());
            }
        }

        private void CB_Choice_mySelect(object sender, SelectionChangedEventArgs e)
        {
            CbChoiceChange();
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition(false);
        }

        private void btnMod_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition(true);
        }

        private void DG_AsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AssortmentClass assort = (AssortmentClass)DG_AsList.SelectedItem;
            if (assort != null)
                TB_uwagi.Text = assort.ASUWAGI;
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            if (Various.Question("Czy na pewno usunąć asortyment ?"))
            {
                AssortmentClass asso = (AssortmentClass)DG_AsList.SelectedItem;

                if (asso.DeletPosition(asso.ID, asso.TableName()))
                {
                    var index = listA.FindIndex(x => x.ID == asso.ID);
                    if (index > -1)
                        listA.RemoveAt(index);

                    Various.InfoOk("Asortyment usunięty pomyślnie");

                    LoadData();
                }
            }
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void groupTV_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (((TreeViewItem)e.NewValue).Tag != null)
                idGroup = (int)((TreeViewItem)e.NewValue).Tag;
            else
                idGroup = 0;
            LoadData();
        }

        private void btCho_myClick(object sender, RoutedEventArgs e)
        {
           AssortmentClass asso = (AssortmentClass)DG_AsList.SelectedItem;

            if (asso != null)
                MdiControl.RefreshMdi(typeof(DocumentEdit), asso);

            Close(sender, e);
        }
    }


    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.ToString().Equals("-1"))
                    return "ZW";
                else
                    return value;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}

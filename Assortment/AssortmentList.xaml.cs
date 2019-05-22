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

namespace FakturaWpf.Assortment
{
    /// <summary>
    /// Logika interakcji dla klasy AssortmentList.xaml
    /// </summary>
    public partial class AssortmentList : UserControl, IMdiControl
    {
        public AssortmentList()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_AsList, new int[] { 0 });
            InitCbChoice();
            CbChoiceChange();
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

            CB_ChoiceGr.comboBox.Items.Clear();
            CB_ChoiceGr.comboBox.Items.Add("towary");
            CB_ChoiceGr.comboBox.Items.Add("usługi");
            CB_ChoiceGr.comboBox.Items.Add("materiały");
            CB_ChoiceGr.comboBox.Items.Add("wyroby");
            CB_ChoiceGr.comboBox.Items.Add("zestawy");

            CB_ChoiceGr.comboBox.SelectedIndex = 0;

        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(AssortmentList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Lista asortymentów";
        }

        private void CB_Choice_mySelect(object sender, SelectionChangedEventArgs e)
        {
            CbChoiceChange();
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(AssortmentEdit), null, "Edycja asortymentu", "ImgStack", 480, 880, TreeName());
        }
    }
}

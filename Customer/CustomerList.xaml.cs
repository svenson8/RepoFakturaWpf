using FakturaWpf.Documents;
using FakturaWpf.MyControls;
using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logika interakcji dla klasy CustomerList.xaml
    /// </summary>
    public partial class CustomerList : UserControl, IMdiControl
    {

        private List<CustomerClass> listU = null;
        private Boolean choice;

        public CustomerList(Boolean achoice = false)
        {
            InitializeComponent();
            choice = achoice;
            Prepare();
            
        }

        private void Prepare()
        {
            Various.SetAutoColumnWidth(DG_Customer, new int[] { 0, 3 });
            Various.FillWithFiltrItems(CB_Choice.comboBox, 8);
            CB_Choice.comboBox.SelectedIndex = 0;

            if (!choice)
              btCho.Visibility = Visibility.Collapsed;

            LoadData();
        }

        private void ComboSearchChange()
        {
            var cbLeft = CB_ChoiceGr.Margin.Left;
            var txLeft = TX_Search.Margin.Left;

            var nearLeft = Math.Min(cbLeft, txLeft);
            var farLeft =  Math.Max(cbLeft, txLeft);

            switch (CB_Choice.comboBox.SelectedIndex)
            {
                case 5 :
                    CB_ChoiceGr.Margin = new Thickness(nearLeft, CB_ChoiceGr.Margin.Top, CB_ChoiceGr.Margin.Right, CB_ChoiceGr.Margin.Bottom);
                    CB_ChoiceGr.Visibility = Visibility.Visible;
                    TX_Search.Margin =  new Thickness(farLeft, TX_Search.Margin.Top, TX_Search.Margin.Right, TX_Search.Margin.Bottom);
                    TX_Search.Visibility = Visibility.Hidden;
                    break;
                case 7:
                    CB_ChoiceGr.Visibility = Visibility.Hidden;
                    TX_Search.Visibility = Visibility.Hidden;
                    break;
                default:
                    CB_ChoiceGr.Margin = new Thickness(farLeft, CB_ChoiceGr.Margin.Top, CB_ChoiceGr.Margin.Right, CB_ChoiceGr.Margin.Bottom);
                    CB_ChoiceGr.Visibility = Visibility.Hidden;
                    TX_Search.Margin = new Thickness(nearLeft, TX_Search.Margin.Top, TX_Search.Margin.Right, TX_Search.Margin.Bottom);
                    TX_Search.Visibility = Visibility.Visible;
                    break;
            }
        }


        private void LoadData()
        {
            if (listU == null)
            {
                CustomerClass cl = new CustomerClass(); 
                listU = cl.ThisReadListData().OfType<CustomerClass>().ToList(); 
            }

            List<CustomerClass> lpom = listU.Select(x => x).ToList();

            if ((TX_Search.Text.Length > 0) || (CB_Choice.comboBox.SelectedIndex == 7))
            {
                switch (CB_Choice.comboBox.SelectedIndex)
                {
                    case 0:
                        lpom = listU.Where(x => x.KLINAZ.ToUpper().StartsWith(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 1:
                        lpom = listU.Where(x => x.KLINAZ.ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 2:
                        lpom = listU.Where(x => (x.KLIPESEL ?? "").ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 3:
                        lpom = listU.Where(x => (x.KLINIP ?? "").ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                    case 4:
                        lpom = listU.Where(x => (x.KLIMIEJSC ?? "").ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                        break;
                    /*  case 5:
                          lpom = listU.Where(x => (x.KLIPESEL ?? "").ToUpper().Contains(TX_Search.Text.ToUpper())).ToList();
                          break;  */  // Grupa
                    case 6:
                        lpom = listU.Where(x => (x.KLIULICA ?? "").ToUpper().Contains(TX_Search.Text.ToUpper()) ||
                                                (x.KLINRDOMU ?? "").ToUpper().Contains(TX_Search.Text.ToUpper()) ||
                                                (x.KLINRLOK ?? "").ToUpper().Contains(TX_Search.Text.ToUpper()) ||
                                                (x.KLIKOD ?? "").ToUpper().Contains(TX_Search.Text.ToUpper()) ||
                                                (x.KLIMIEJSC ?? "").ToUpper().Contains(TX_Search.Text.ToUpper()) ).ToList();
                        break;
                    case 7:
                        lpom = listU.OrderBy(x => x.ID).ToList();
                        break;
                }
            }


            DG_Customer.ItemsSource = lpom;
            DG_Customer.SelectedIndex = 0;

        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(CustomerList), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            CustomerClass csMod = ((CustomerClass)obj);

            var index = listU.FindIndex(x => x.ID == csMod.ID);

            if (index > -1)
                listU[index] = csMod;
            else
                listU.Add(csMod);

            LoadData();
        }

        public string TreeName()
        {
            return "Lista klientów";
        }

        private void CB_Choice_mySelect(object sender, SelectionChangedEventArgs e)
        {
            ComboSearchChange();
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            EditPosition((((MyButton)sender).Name.Equals("btnMod")));
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            if (Various.Question("Czy na pewno usunąć klienta ?"))
            {
                CustomerClass customer = (CustomerClass)DG_Customer.SelectedItem;

                if (customer.DeletPosition(customer.ID, customer.TableName()))
                {
                    var index = listU.FindIndex(x => x.ID == customer.ID);
                    if (index > -1)
                        listU.RemoveAt(index);

                    Various.InfoOk("Klient usunięty pomyślnie");

                    LoadData();
                }
            }
        }

        private void DG_Customer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditPosition(true);
        }

        private void EditPosition(Boolean mod)
        {
            CustomerClass customer = (CustomerClass)DG_Customer.SelectedItem;

            int id = 0;
            var title = "Redagowanie danych ";

            if (mod)
            {
                id = customer.ID;
                title += customer.KLINAZ;
            }

            if (id >= 0)
            {
                MdiControl.AddChild(typeof(CustomerEdit), new object[] { id }, title, "ImgFakt", 600, 700, TreeName());
            }
        }

        private void btCho_myClick(object sender, RoutedEventArgs e)
        {
            CustomerClass customer = (CustomerClass)DG_Customer.SelectedItem;

            if (customer != null)
              MdiControl.RefreshMdi(typeof(DocumentEdit), customer);

            Close(sender, e);
        }
    }

    public class NameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string adress;

            adress = (string)values[0] + " "+ values[1] + "/" + values[2] + ", " + values[3] +" "+ values[4];
            if (adress.Length <= 5)
                adress = "";

            return adress;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            string[] splitValues = ((string)value).Split(' ');
            return splitValues;
        }
    }

}

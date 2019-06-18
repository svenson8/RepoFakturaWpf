using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
using FakturaWpf.WebApi;
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
        private CustomerClass customer;
        private delegate void LoadCombos();

        public CustomerEdit(int id)
        {
            InitializeComponent();

            customer = new CustomerClass(id);
            customer.ACTIVE = "T";

            InitBinding();
            this.Focusable = true;
        }

        void InitBinding()
        {
            GR_Cus.DataContext = null;
            GR_Cus.DataContext = customer;
            InitCombos();
        }

        void InitCombos()
        {
            LoadCombos ld = new LoadCombos(InitCBProvinces);
            ld += InitCBCountries;
            ld.Invoke();
            

        }

        void InitCBProvinces()
        {
            List<DictionaryClass> ListData = new List<DictionaryClass>();
            ListData = new DictionaryClass(0, DictionaryClass.slRodzProv).ThisReadListData().OfType<DictionaryClass>().ToList();

            CB_Province.comboBox.ItemsSource = ListData;
            CB_Province.comboBox.DisplayMemberPath = "SLKOMUN1";
            CB_Province.comboBox.SelectedValuePath = "ID";

            if (customer.ID > 0)
                CB_Province.comboBox.SelectedValue = customer.KLIWOJID;
            else
                CB_Province.comboBox.SelectedIndex = 0;
            
        }

        void InitCBCountries()
        {
            List<DictionaryClass> ListData = new List<DictionaryClass>();
            ListData = new DictionaryClass(0, DictionaryClass.slRodzCountry).ThisReadListData().OfType<DictionaryClass>().ToList();

            CB_Country.comboBox.ItemsSource = ListData;
            CB_Country.comboBox.DisplayMemberPath = "SLKOMUN1";
            CB_Country.comboBox.SelectedValuePath = "ID";

            if (customer.ID > 0)
                CB_Country.comboBox.SelectedValue = customer.KLIKRAJID;
            else
                CB_Country.comboBox.SelectedIndex = 0;

        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(CustomerEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Edycja kontrahenta";
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TX_Name.Text.Length == 0)
                {
                    Various.Warning("Nie nadano nazwy kontrahentowi", "Ostrzeżenie");
                    return;
                }
                customer.KLIWOJID  = (int)(CB_Province.comboBox.SelectedValue);
                customer.KLIKRAJID = (int)(CB_Country.comboBox.SelectedValue);
                if (customer.ThisSaveData())
                {
                    Various.InfoOk("Klient zapisany", "Informacja");
                    MdiControl.RefreshMdi(typeof(CustomerList), customer);
                }
                else
                    Various.Warning("Błąd zapisu danych", "");

                Close(sender, e);
            }
            catch (Exception ex)
            {
                Various.Error("Błąd "+nameof(CustomerEdit)+ " MyButton_myClick :" + ex.Message, "Błąd");
            }
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }

        private async void Btn_Gus_myClick(object sender, RoutedEventArgs e)
        {

            GusSearch gs = new GusSearch();
            CustomerClass cs = await gs.StartGusSearching(customer);

            if ((customer.KLIUWAGI != "") && (customer.KLIUWAGI != null))
            {
                Various.Warning(customer.KLIUWAGI, "Ostrzeżenie");
                customer.KLIUWAGI = "";
            } else
            {
                InitBinding();
                CB_Country.comboBox.Text = "Polska";
                CB_Province.comboBox.Text = customer.WOJTXT.ToLower();
            }

 

        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                MyButton_myClick(sender, e);

            if (e.Key == Key.Escape)
                MyButton_myClick_1(sender, e);

        }
    }
}

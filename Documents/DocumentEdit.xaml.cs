using FakturaWpf.Customer;
using FakturaWpf.Dictionary;
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
    /// Logika interakcji dla klasy DocumentEdit.xaml
    /// </summary>
    public partial class DocumentEdit : UserControl, IMdiControl
    {
        public DocumentEdit(int id)
        {
            InitializeComponent();
            InitBinding();

        }

        void InitBinding()
        {
            InitCbDoks();
            InitCbSellKind();
            InitCbTransaction();
            Various.SetTodayDates(GR_up);
        }

        private void InitCbTransaction()
        {
            CB_Trans.comboBox.Items.Clear();
            CB_Trans.comboBox.Items.Add("Sprzedaż krajowa");
            CB_Trans.comboBox.Items.Add("Eksport towarów poza UE");
            CB_Trans.comboBox.Items.Add("Wewnątrzwspólnotowa dostawa towarów");
            CB_Trans.comboBox.Items.Add("Eksport usług");
            CB_Trans.comboBox.Items.Add("Odwrotne obciążenie w krajowej sprzedaży towarów");
            CB_Trans.comboBox.Items.Add("Odwrotne obciążenie, sprzedaż usług podwykonawców budowlanych");

            CB_Trans.comboBox.SelectedIndex = 0;
        }

        private void InitCbSellKind()
        {
            CB_SellKind.comboBox.Items.Clear();
            CB_SellKind.comboBox.Items.Add("Data sprzedaży");
            CB_SellKind.comboBox.Items.Add("Miesiąc sprzedaży");
            CB_SellKind.comboBox.Items.Add("Data dokonania dostawy towarów");
            CB_SellKind.comboBox.Items.Add("Data zakończenia dostawy towarów");
            CB_SellKind.comboBox.Items.Add("Data wykonania usługi");
            CB_SellKind.comboBox.Items.Add("Data zakończenia usługi");
            CB_SellKind.comboBox.Items.Add("Data otrzymania należności");

            CB_SellKind.comboBox.SelectedIndex = 3;
        }

        void InitCbDoks()
        {
            List<object> ListData = new List<object>();
            NQueryReader nq = new NQueryReader("select *, SLKOMUN2 +' '+SLKOMUN1 as concat from TSlownik " +
                                               "where slrodz =" + Various.QuotedStr(DictionaryClass.slRodzDokDef));
            while (nq.NReader.Read())
                ListData.Add(new { Desc = (string)nq.NReader["concat"], Id = (int)nq.NReader["ID"] });

            nq.NReader.Close();

            CB_Docs.comboBox.ItemsSource = ListData;
            CB_Docs.comboBox.DisplayMemberPath = "Desc";
            CB_Docs.comboBox.SelectedValuePath = "Id"; 

            /* if (customer.ID > 0)
                 CB_Province.comboBox.SelectedValue = customer.KLIWOJID;
             else*/
                 CB_Docs.comboBox.SelectedIndex = 0;  
        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Nowy dokument";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(CustomerList), null, "Lista kontrahentów", "ImgCustomers", 450, 700, TreeName());
        }
    }
}

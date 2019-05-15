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
        DocumentClass document; 
        List<DictionaryClass> ListNumber;
        DictionaryClass chosenNumber;

        public DocumentEdit(int id)
        {
            InitializeComponent();

            document = new DocumentClass(id);

            InitBinding();

        }

        void InitBinding()
        {
            DP_Main.DataContext = document;
            InitCbDoks();
            InitCbSellKind();
            InitCbTransaction();
            InitCbPayment();
            SetCustomer((document.ID > 0 ? new CustomerClass(document.MDKLIID) : null));
            Various.SetTodayDates(GR_up);
            SetDocNumber();
        }

        private void SetDocNumber()
        {
            if (document.ID <= 0)
            {
                if (ListNumber == null)
                    ListNumber = new DictionaryClass(0, DictionaryClass.slDocNumber).ThisReadListData().OfType<DictionaryClass>().ToList();

                if (CB_Docs.comboBox.SelectedValue != null)
                {
                    chosenNumber = ListNumber.Where(x => x.SLKOMUN11 == (int)CB_Docs.comboBox.SelectedValue).SingleOrDefault();

                    if (chosenNumber != null)
                        L_Nrdok.Content = Various.SetActualNumber(chosenNumber);
                }
            }
            else
            {
                L_Nrdok.Content = document.MDNRDOK;
                CB_Docs.comboBox.IsEnabled = false;
            }

        }

        private void InitCbPayment()
        {
            List<DictionaryClass> ListData = new List<DictionaryClass>();
            ListData = new DictionaryClass(0, DictionaryClass.slRodzPay).ThisReadListData().OfType<DictionaryClass>().ToList();

            CB_Payment.comboBox.ItemsSource = ListData;
            CB_Payment.comboBox.DisplayMemberPath = "SLKOMUN1";
            CB_Payment.comboBox.SelectedValuePath = "ID";

            if (document.ID > 0)
                CB_Payment.comboBox.SelectedValue = document.MDRODZPLATID;
            else
                CB_Payment.comboBox.SelectedIndex = 0;
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

            if (document.ID > 0)
                CB_Trans.comboBox.SelectedIndex = document.MDRODZTRANS;
            else
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

            if (document.ID > 0)
                CB_SellKind.comboBox.SelectedIndex = document.MDDATASPRZRODZ;
            else
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

             if (document.ID > 0)
                 CB_Docs.comboBox.SelectedValue = document.MDDOKDEFID;
             else
                 CB_Docs.comboBox.SelectedIndex = 0;  
        }

        void SetCustomer(CustomerClass cs = null)
        {
            TB_Nip.Text  = (cs != null ? cs.KLINIP : "");
            TB_Name.Text = (cs != null ? cs.KLINAZ : "");

            document.MDKLIID = (cs != null ? cs.ID : 0);
        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            SetCustomer((CustomerClass)obj);
        }

        public string TreeName()
        {
            return "Nowy dokument";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(CustomerList), new object[] { true }, "Lista kontrahentów", "ImgCustomers", 450, 700, TreeName());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SetCustomer();
        }

        private void btSave_myClick(object sender, RoutedEventArgs e)
        {
            try
            {
                document.MDDOKDEFID     = (int)CB_Docs.comboBox.SelectedValue;
                document.MDDATASPRZRODZ = CB_SellKind.comboBox.SelectedIndex;
                document.MDRODZTRANS    = CB_Trans.comboBox.SelectedIndex;
                document.MDRODZPLATID   = (int)CB_Payment.comboBox.SelectedValue;
                document.MDNRDOK        = L_Nrdok.Content.ToString();

                if (document.ID <= 0)
                {
                    chosenNumber.SLKOMUN12 = +1;
                    chosenNumber.ThisSaveData();
                }

                if (document.ThisSaveData())
                {
                    Various.InfoOk("Dokument zapisany", "Informacja");
                    MdiControl.RefreshMdi(typeof(DocumentList), document);
                }
                else
                    Various.Warning("Błąd zapisu danych", "");

                Close(sender, e);
             }
            catch (Exception ex)
            {
                Various.Error("Błąd "+nameof(DocumentEdit)+ " btSave_myClick:" + ex.Message, "Błąd");
            }
        }

        private void CB_Docs_mySelect(object sender, SelectionChangedEventArgs e)
        {
            SetDocNumber();
        }
    }
}

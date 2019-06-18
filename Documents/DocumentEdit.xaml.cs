using FakturaWpf.Assortment;
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
        List<DocPositionClass> ListPosition;

        public DocumentEdit(int id)
        {
            InitializeComponent();

            document = new DocumentClass(id);

            InitBinding();
            this.Focusable = true;

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
            Various.SetAutoColumnWidth(DG_Position, new[] { 1 });
            SetDocNumber();
            LoadPosition();
        }

        private void LoadPosition()
        {
            DocPositionClass dc = new DocPositionClass();
            dc.DOKID = document.ID;
            ListPosition =  dc.ThisReadListData().OfType<DocPositionClass>().ToList();

            RefreshGrid();
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
            if (obj is CustomerClass)
              SetCustomer((CustomerClass)obj);

            if (obj is List<AssortmentClass>)
                AddPosition((List<AssortmentClass>)obj);

            if (obj is DocPositionClass)
                EditPosition((DocPositionClass)obj);
        }

        private void EditPosition(DocPositionClass dp)
        {
            if (dp != null)
            {
                var index = ListPosition.FindIndex(x => x.MPLP == dp.MPLP);
                if (index > -1)
                { 
                    ListPosition[index].MPILOSC = dp.MPILOSC;
                    ListPosition[index].MPCENA = dp.MPCENA;
                    ListPosition[index].MPWARNET = dp.MPILOSC * dp.MPCENA;
                    ListPosition[index].MPWARVAT = ((Decimal)ListPosition[index].MPSTAWKAVAT / (Decimal)100) * (Decimal)ListPosition[index].MPWARNET;
                    ListPosition[index].MPWARBR = ListPosition[index].MPWARNET + ListPosition[index].MPWARVAT;

                    RefreshGrid();
                }

            }
        }

        private void RefreshGrid()
        {
            DG_Position.ItemsSource = ListPosition;
            DG_Position.Items.Refresh();
            DG_Position.SelectedIndex = 0;

            // Summary
            Decimal sumNet = (Decimal)0;
            Decimal sumVat = (Decimal)0;
            Decimal sumBru = (Decimal)0;
            foreach (DocPositionClass dc in ListPosition)
            {
                sumNet += (Decimal)dc.MPWARNET;
                sumVat += (Decimal)dc.MPWARVAT;
                sumBru += (Decimal)dc.MPWARBR;
            }

            l_netto.Content = string.Format("{0:0.00}", sumNet);
            l_vat.Content = string.Format("{0:0.00}", sumVat);
            l_brutto.Content = string.Format("{0:0.00}", sumBru);
        }

        private void AddPosition(List<AssortmentClass> list)
        {
            if ((list != null) && (list.Count > 0))
            {
                var lp = 1;
                if (ListPosition == null)
                    ListPosition = new List<DocPositionClass>();     
                else
                    lp = ListPosition.Count + 1;

                foreach (AssortmentClass a in list)
                {
                    DocPositionClass dc = new DocPositionClass();
                    dc.MPILOSC = a.ASILOSCWYB;
                    dc.MPLP = lp;
                    dc.MPNAZWA = a.ASNAZWA;
                    if (a.ASJM > 0)
                        dc.MPJM = new DictionaryClass(a.ASJM).SLKOMUN1;
                    dc.MPCENA = a.ASNETTOWYB;
                    dc.MPWARNET = dc.MPILOSC * dc.MPCENA;
                    dc.MPWARVAT = ((Decimal)a.ASVAT / (Decimal)100) * (Decimal)dc.MPWARNET;
                    dc.MPWARBR = dc.MPWARNET + dc.MPWARVAT;
                    dc.ASSORTID = a.ID;
                    dc.MPSTAWKAVAT = a.ASVAT;

                    ListPosition.Add(dc);
                    lp++;
                }


                RefreshGrid();
            }
            
        }

        private void DeletePosition()
        {
            DocPositionClass docp = (DocPositionClass)DG_Position.SelectedItem;

            if (docp != null)
            {
                var index = ListPosition.FindIndex(x => x.MPLP == docp.MPLP);
                if (index > -1)
                    ListPosition.RemoveAt(index);

                RefreshGrid();
            }
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
                if (document.MDKLIID <=0)
                {
                    Various.Warning("Nie wybrano kontrahenta", "Ostrzeżenie");
                        return;
                }

                if ((ListPosition != null) && (ListPosition.Count > 0))
                {

                    document.MDDOKDEFID = (int)CB_Docs.comboBox.SelectedValue;
                    document.MDDATASPRZRODZ = CB_SellKind.comboBox.SelectedIndex;
                    document.MDRODZTRANS = CB_Trans.comboBox.SelectedIndex;
                    document.MDRODZPLATID = (int)CB_Payment.comboBox.SelectedValue;
                    document.MDNRDOK = L_Nrdok.Content.ToString();

                    if (document.ID <= 0)
                    {
                        chosenNumber.SLKOMUN12 += 1;
                        chosenNumber.ThisSaveData();
                    }

                    document.MDWARNET = ListPosition.Sum(x => x.MPWARNET);
                    document.MDWARVAT = ListPosition.Sum(x => x.MPWARVAT);
                    document.MDWARBR = ListPosition.Sum(x => x.MPWARBR);

                    if (document.ThisSaveData() && SavePositions())
                    {
                        Various.InfoOk("Dokument zapisany", "Informacja");
                        MdiControl.RefreshMdi(typeof(DocumentList), document);
                    }
                    else
                        Various.Warning("Błąd zapisu danych", "Błąd");

                    Close(sender, e);
                }
                else
                    Various.Warning("Nie dodano żadnych pozycji sprzedażowych", "Ostrzeżenie");
             }
            catch (Exception ex)
            {
                Various.Error("Błąd "+nameof(DocumentEdit)+ " btSave_myClick:" + ex.Message, "Błąd");
            }
        }

        private bool SavePositions()
        {
            bool result = true;
            foreach (DocPositionClass dc in ListPosition)
            {
                dc.DOKID = document.ID;
                result = dc.ThisSaveData();
            }

            if (result)
                return true;
            else
                return false;
        }

        private void CB_Docs_mySelect(object sender, SelectionChangedEventArgs e)
        {
            SetDocNumber();
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(AssortmentList), new object[] { true }, "Lista asortymentów", "ImgStack", 500, 800, TreeName());
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            DeletePosition();
        }

        private void btnMod_myClick(object sender, RoutedEventArgs e)
        {
            DocPositionClass docp = (DocPositionClass)DG_Position.SelectedItem;

            if (docp != null)
            {
                MdiControl.AddChild(typeof(DocPositionEdit), new object[] { docp }, "Edycja pozycji", "ImgStack", 190, 550, TreeName());
            }
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                btSave_myClick(sender, e);

            if (e.Key == Key.Escape)
                MyButton_myClick_1(sender, e);

            if (e.Key == Key.F2)
                Button_Click(sender, e);
        }
    }
}

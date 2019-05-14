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
    /// Logika interakcji dla klasy DocumentNumber.xaml
    /// </summary>
    public partial class DocumentNumber : UserControl, IMdiControl
    {
        DictionaryClass mydict;
        List<DictionaryClass> ListNumber;

        public DocumentNumber()
        {
            InitializeComponent();
            Prepare();
        }

        private void Prepare()
        {
            InitCbDoks();
            LoadtData();
        }

        private void LoadtData()
        {
            ListNumber = new DictionaryClass(0, DictionaryClass.slDocNumber).ThisReadListData().OfType<DictionaryClass>().ToList();
            FindDoc();
        }

        private void FindDoc()
        {
            if (ListNumber != null)
            {
                int iddok = (int)CB_Docs.comboBox.SelectedValue;
                mydict = ListNumber.Where(x => x.SLKOMUN11 == iddok).SingleOrDefault();

                if (mydict == null)
                {
                    mydict = new DictionaryClass(0, DictionaryClass.slDocNumber);
                    mydict.SLKOMUN11 = (int)CB_Docs.comboBox.SelectedValue;
                }

                GR_Main.DataContext = null;
                GR_Main.DataContext = mydict;

                L_AktNum.Content = Various.SetActualNumber(mydict);
            }
        }

        void SaveToList()
        {
            if (ListNumber != null)
            {
                mydict.SLKOMUN1 = TB_number.Text;

                var index = ListNumber.FindIndex(x => x.SLKOMUN11 == mydict.SLKOMUN11);

                if (index > -1)
                    ListNumber[index] = mydict;
                else
                    ListNumber.Add(mydict);
            }
            
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

            CB_Docs.comboBox.SelectedIndex = 0;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocumentNumber), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Tworzenie numeru";
        }

        private void CB_Docs_mySelect(object sender, SelectionChangedEventArgs e)
        {
            SaveToList();
            FindDoc();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            L_AktNum.Content = Various.SetActualNumber(mydict);
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            bool error = false;
            foreach (DictionaryClass dc in ListNumber)
            {
                if (!dc.ThisSaveData())
                    error = true;
            }

            if (!error)
                Various.InfoOk("Numeracja zapisana pomyślnie");
            else
                Various.Warning("Błąd zapisu danych");

            Close(sender, e);
        }
    }
}

using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
using Microsoft.Win32;
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
    /// Logika interakcji dla klasy AssortmentEdit.xaml
    /// </summary>
    public partial class AssortmentEdit : UserControl, IMdiControl
    {
        private AssortmentClass assort;

        public AssortmentEdit(int id)
        {
            InitializeComponent();

            assort = new AssortmentClass(id);
            SetGroup((assort.ID > 0 ? new DictionaryClass(assort.ASGRUPAID, DictionaryClass.slRodzAsGroup) : null));
            GR_Main.DataContext = assort;
            InitCbType();
            InitCbVat();
            InitCbMeasure();

        }

        private void InitCbMeasure()
        {
            List<DictionaryClass> ListData = new List<DictionaryClass>();
            ListData = new DictionaryClass(0, DictionaryClass.slRodzMeasure).ThisReadListData().OfType<DictionaryClass>().ToList();

            CB_jm.comboBox.ItemsSource = ListData;
            CB_jm.comboBox.DisplayMemberPath = "SLKOMUN1";
            CB_jm.comboBox.SelectedValuePath = "ID";

            if (assort.ID > 0)
                CB_jm.comboBox.SelectedValue = assort.ASJM;
            else
                CB_jm.comboBox.SelectedIndex = 0;
        }

        private void InitCbVat()
        {
            CB_vat.comboBox.Items.Clear();
            CB_vat.comboBox.Items.Add("23");
            CB_vat.comboBox.Items.Add("8");
            CB_vat.comboBox.Items.Add("5");
            CB_vat.comboBox.Items.Add("0");
            CB_vat.comboBox.Items.Add("ZW");

            if (assort.ID > 0)
                CB_vat.comboBox.Text = (assort.ASVAT == -1 ? "ZW" : assort.ASVAT.ToString());
            else
                CB_vat.comboBox.SelectedIndex = 0;
        }

        private void InitCbType()
        {
            CB_typ.comboBox.Items.Clear();
            CB_typ.comboBox.Items.Add("Towar");
            CB_typ.comboBox.Items.Add("Usługa");

            if (assort.ID > 0)
                CB_typ.comboBox.SelectedIndex = assort.ASTYP;
            else
                CB_typ.comboBox.SelectedIndex = 0;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(AssortmentEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            SetGroup((DictionaryClass)obj);
        }

        private void SetGroup(DictionaryClass dc = null)
        {
            TB_Group.Text = (dc != null ? dc.SLKOMUN1 : "");

            assort.ASGRUPAID = (dc != null ? dc.ID : 0);
        }

        public string TreeName()
        {
            return "Edycja asortymentu";
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                string selectedFileName = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(selectedFileName);
                bitmap.EndInit();
                image.Source = bitmap;
            }
        }



        private void button_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.AddChild(typeof(DictionaryList), new object[] { DictionaryClass.slRodzAsGroup, true}, "Lista grup asortymentowych", "ImgGroupas", 500, 675);
        }
    }
}

using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        private DictionaryClass dcgr;

        public AssortmentEdit(int id)
        {
            InitializeComponent();

            assort = new AssortmentClass(id);
            SetGroup((assort.ID > 0 ? new DictionaryClass(assort.ASGRUPAID, DictionaryClass.slRodzAsGroup) : null));
            DP_Main.DataContext = assort;
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
            CB_vat.comboBox.Items.Add(23);
            CB_vat.comboBox.Items.Add(8);
            CB_vat.comboBox.Items.Add(5);
            CB_vat.comboBox.Items.Add("0");
            CB_vat.comboBox.Items.Add("ZW");

            if (assort.ID > 0)
                CB_vat.comboBox.Text = (assort.ASVAT == -1 ? "ZW" : assort.ASVAT.ToString());
            else
                CB_vat.comboBox.SelectedIndex = 0;
        }

        private void InitCbType()
        {
            Various.InitCbAssortType(CB_typ.comboBox);

            if (assort.ID > 0)
                CB_typ.comboBox.SelectedIndex = assort.ASTYP;

        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(AssortmentEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            SetGroup((DictionaryClass)obj, true);
        }

        private void SetGroup(DictionaryClass dc = null, bool getnumber = false)
        {
            TB_Group.Text = (dc != null ? dc.SLKOMUN1 : "");
            assort.ASGRUPAID = (dc != null ? dc.ID : 0);
            dcgr = dc;
            
            if (getnumber)
            {
                NQueryReader nq = new NQueryReader("select max(ASNUMER) + 1 from " + assort.TableName());
                nq.NReader.Read();
                var nb = nq.NReader.GetInt32(0);
                nq.NReader.Close();

                TB_Numer.Text = nb.ToString();

                if (dc != null)
                  TB_Kod.Text = string.Format(dc.SLKOMUN2+"{0:d6}", nb);

            }
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

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }

        private void btSave_myClick(object sender, RoutedEventArgs e)
        {
            assort.ASTYP = CB_typ.comboBox.SelectedIndex;
            assort.ASVAT =  (CB_vat.comboBox.SelectedValue.ToString() != "ZW" ? (int)CB_vat.comboBox.SelectedValue : -1);
            assort.ASJM = (int)CB_jm.comboBox.SelectedValue;

            if (assort.ASGRUPAID <=0)
            {
                Various.Warning("Nie wybrano grupy asortymentowej", "Uwaga !");
                return;
            }

            if (assort.ThisSaveData())
            {
                Various.InfoOk("Assortyment zapisany", "Informacja");
                MdiControl.RefreshMdi(typeof(AssortmentList), assort);
                Close(sender, e);
            }
            else
                Various.Warning("Błąd zapisu danych", "");
        }

        private void TB_Numer_LostFocus(object sender, RoutedEventArgs e)
        {
            if (dcgr != null)
            {
                var nb = int.Parse(TB_Numer.Text);
                TB_Kod.Text = string.Format(dcgr.SLKOMUN2 + "{0:d6}", nb);
            }
        }
    }

    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                using (var ms = new System.IO.MemoryStream((byte[])value))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad; // here
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapImage)value));
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }
    }


}

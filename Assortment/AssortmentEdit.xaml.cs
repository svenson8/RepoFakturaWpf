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
        public AssortmentEdit()
        {
            InitializeComponent();
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(AssortmentEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
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
    }
}

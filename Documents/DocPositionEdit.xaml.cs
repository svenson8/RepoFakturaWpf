using FakturaWpf.Assortment;
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
    /// Logika interakcji dla klasy DocPositionEdit.xaml
    /// </summary>
    public partial class DocPositionEdit : UserControl, IMdiControl    
    {
        private DocPositionClass docp;

        public DocPositionEdit(DocPositionClass dc)
        {
            InitializeComponent();
            docp = dc;
            Prepare();
        }

        private void Prepare()
        {
            if (docp != null)
            {
                GR_asort.DataContext = docp;

                AssortmentClass aso = new AssortmentClass(docp.ASSORTID);
                TextBlock tb = new TextBlock();
                tb.Inlines.Add(new Run("Kod: "));
                tb.Inlines.Add(new Bold(new Run(aso.ASKOD)));
                l_group.Content = tb;

                TextBlock tb1 = new TextBlock();
                tb1.Inlines.Add(new Run("Jednostka: "));
                tb1.Inlines.Add(new Bold(new Run(docp.MPJM)));
                l_measure.Content = tb1;

                TextBlock tb2 = new TextBlock();
                tb2.Inlines.Add(new Run("Typ: "));
                tb2.Inlines.Add(new Bold(new Run(Various.asorttypes[aso.ASTYP])));
                l_type.Content = tb2;

                L_Name.Content = aso.ASNAZWA;
                TB_Count.Text = docp.MPILOSC.ToString();
                TB_Price.Text = docp.MPCENA.ToString();
            }
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DocPositionEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Edycja pozycji";
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }

        private void btSave_myClick(object sender, RoutedEventArgs e)
        {
            MdiControl.RefreshMdi(typeof(DocumentEdit), docp);
            Close(sender, e);
        }
    }
}

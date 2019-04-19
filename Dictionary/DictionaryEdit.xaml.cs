﻿using FakturaWpf.Tools;
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

namespace FakturaWpf.Dictionary
{
    /// <summary>
    /// Logika interakcji dla klasy DictionaryEdit.xaml
    /// </summary>
    public partial class DictionaryEdit : UserControl, IMdiControl
    {
        private DictionaryClass mydict;

        public DictionaryEdit(int id=0, string slowkind = null)
        {
            InitializeComponent();
            Various.FillPayForms(CB_PayF.comboBox);

            mydict = new DictionaryClass(id, slowkind);
            if ((slowkind != DictionaryClass.slRodzCountry) && (slowkind != DictionaryClass.slRodzDokDef))
                GR_SLow.RowDefinitions[2].Height = new GridLength(0);

            if (slowkind != DictionaryClass.slRodzDokDef)
                GR_SLow.RowDefinitions[3].Height = new GridLength(0);
            else
            {
                if ((mydict.SLKOMUN3 != null) && (mydict.SLKOMUN3.Equals("G")))
                    CB_PayF.comboBox.SelectedIndex = 1;
            }

            InitBinding();

        }

        private void InitBinding()
        {
            GR_SLow.DataContext = mydict;
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(DictionaryEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Grupy kontr.";   
        }

        private void btIns_myClick(object sender, RoutedEventArgs e)
        {
            if (mydict.SLRODZ == DictionaryClass.slRodzDokDef)
            {
                if (CB_PayF.comboBox.SelectedIndex == 1)
                    mydict.SLKOMUN3 = "G";
                else
                    mydict.SLKOMUN3 = "P";
            }


            if (mydict.ThisSaveData())
            {
                Various.InfoOk("Słownik zapisany", "Informacja");
                MdiControl.RefreshMdi(typeof(DictionaryList), mydict);
            }
            else
                Various.Warning("Błąd zapisu danych", "");

            Close(sender, e);
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }
    }
}

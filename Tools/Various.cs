using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using WPF.MDI;
using System.Windows.Media.Imaging;
using FakturaWpf.Tools;
using System.Windows.Data;

namespace FakturaWpf
{
    class Various
    {
       
        public static string QuotedStr(string val)
        {
            return "'" + val + "'";
        }

        public static void Warning(String comm, String title = "")
        {
            MessageBox.Show(comm, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void Error(String comm, String title = "")
        {
            MessageBox.Show(comm, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void InfoOk(String comm, String title = "")
        {
            MessageBox.Show(comm, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static Boolean Question(String comm, String title = "")
        {
            if (MessageBox.Show(comm, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        } 

        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                        new Action(delegate { }));
        }

        public static void SetAutoColumnWidth(DataGrid myGrid, int[] arr)
        {
            foreach (int i in arr)
            {
                myGrid.Columns[i].Width =
                new DataGridLength(1, DataGridLengthUnitType.Star);
            }            
        }

        public static List<object> GetPeriodList()
        {
            List<object> list = new List<object>();

            list.Add(new { desc = "Dziś", field = "TD" });
            list.Add(new { desc = "Wczoraj", field = "YD" });
            list.Add(new { desc = "Ostanie N dni", field = "LND" });
            list.Add(new { desc = "Bieżący tydzień", field = "TW" });
            list.Add(new { desc = "Poprzedni tydzień", field = "LW" });
            list.Add(new { desc = "Bieżący miesiąc", field = "TM" });
            list.Add(new { desc = "Poprzedni miesiąc", field = "LM" });
            list.Add(new { desc = "Ostatnie N miesięcy", field = "LNM" });
            list.Add(new { desc = "Miesiąc", field = "M" });
            list.Add(new { desc = "Od m-ca do m-ca", field = "FMTM" });
            list.Add(new { desc = "Bieżący rok", field = "TY" });
            list.Add(new { desc = "Poprzedni rok", field = "LY" });
            list.Add(new { desc = "Rok", field = "Y" });
            list.Add(new { desc = "Od roku do roku", field = "FYTY" });
            list.Add(new { desc = "Okres", field = "PR" });

            return list;

        }

        public static void FillWithMonths(ComboBox cb)
        {
            cb.Items.Clear();
            cb.Items.Add("Styczeń");
            cb.Items.Add("Luty");
            cb.Items.Add("Marzec");
            cb.Items.Add("Kwiecień");
            cb.Items.Add("Maj");
            cb.Items.Add("Czerwiec");
            cb.Items.Add("Lipiec");
            cb.Items.Add("Sierpień");
            cb.Items.Add("Wrzesień");
            cb.Items.Add("Październik");
            cb.Items.Add("Lstopad");
            cb.Items.Add("Grudzień");

            cb.SelectedIndex = 0;
        }

        public static void FillWithYears(ComboBox cb)
        {
            int ystart = DateTime.Now.Year - 5;
            int yend   = DateTime.Now.Year + 5;

            for (int i = ystart; i <= yend; i++)
                cb.Items.Add(i);

            cb.SelectedValue = DateTime.Now.Year;

        }


        public static void FillWithFiltrItems(ComboBox cb, int typ)
        {
            cb.Items.Clear();
            if (typ >= 1)
                cb.Items.Add("Wg początku nazwy");
            if (typ >= 2)
                cb.Items.Add("Wg fragmentu nazwy");
            if (typ >= 3)
                cb.Items.Add("Wg pesel");
            if (typ >= 4)
                cb.Items.Add("Wg NIPu");
            if (typ >= 5)
                cb.Items.Add("Wg miejscowości");
            if (typ >= 6)
                cb.Items.Add("Wg grupy");
            if (typ >= 7)
                cb.Items.Add("Wg fragmentu adresu");
            if (typ >= 8)
                cb.Items.Add("W kolejności wpisania");
        }

        public static void RestartApp()
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
        }

        public static BitmapImage GetImageFromResources(string resname)
        {
            return Application.Current.TryFindResource(resname) as BitmapImage;
        }

        public static string StringBeetween(string main, string first, string second)
        {
            string FinalString;
            int Pos1 = main.IndexOf(first) + (first).Length;
            int Pos2 = main.IndexOf(second);
            FinalString = main.Substring(Pos1, Pos2 - Pos1);

            return FinalString;
        }

    

      }

    public class YesNoToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value.ToString().ToUpper())
            {
                case "T":
                    return true;
                case "N":
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "T";
                else
                    return "N";
            }
            return "N";
        }
    }



    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);

        public IniFile(string Path)
        {
            path = Path;
        }

        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            255, this.path);
            return temp.ToString();

        }

    }
}

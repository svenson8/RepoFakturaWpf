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

    }

    public class MdiControl
    {
        public static MdiContainer mdParent = null;

        public static void FindMainContainer()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    mdParent = (window as MainWindow).MdiMain;
                }
            }
        }

        public static void CloseMdi(string name)
        {
            FindMainContainer();
            foreach (MdiChild mdc in mdParent.Children)
            {
                if (mdc.Name.Equals(name))
                {
                    mdParent.Children.Remove(mdc);
                    break;
                }
            }
        }

        public static void AddChild(Type myClass, object[] args ,string title ,string iconame, int height, int width)
        {
            Object theObject = Activator.CreateInstance(myClass, args?.ToArray()); 

            MdiControl.mdParent.Children.Add(new MdiChild()
            {
                Title = title,
                Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/"+iconame)),
                Height = height,
                Width = width,
                Name = ((IMdiControl)theObject).ChildName(),
                Content = (UserControl)theObject,
            });
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

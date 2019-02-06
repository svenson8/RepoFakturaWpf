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

        public static void RestartApp()
        {
            Application.Current.Shutdown();
            System.Diagnostics.Process.Start(Environment.GetCommandLineArgs()[0]);
        }

        public static BitmapImage GetImageFromResources(string resname)
        {
            return Application.Current.TryFindResource(resname) as BitmapImage;
        }

    }

    public class MdiControl
    {
        public static MdiContainer mdParent = null;
        public static TreeView mainTree = null;

        public static void FindMainContainer()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                {
                    mdParent = (window as MainWindow).MdiMain;
                    mainTree = (window as MainWindow).mainTreeView;
                }
            }
        }

        public static MdiChild FindChild(string childname)
        {
            if (mdParent == null)
                FindMainContainer();

            foreach (MdiChild mdc in mdParent.Children)
            {
                if (mdc.Name.Equals(childname))
                {
                    return mdc;
                }
            }

            return null;
        }

        public static MdiChild FindChild(Type myClass)
        {
            if (mdParent == null)
                FindMainContainer();

            foreach (MdiChild mdc in mdParent.Children)
            {
                if (mdc.Content.GetType() == myClass)
                {
                    return mdc;
                }
            }

            return null;
        }


        public static void CloseMdi(Type myClass, string treename)
        {
            mdParent.Children.Remove(FindChild(myClass));
            RemoveTreeItem(treename);
        }

        public static void AddChild(Type myClass, object[] args, string title, string iconame, int height, int width, string parent = "")
        {
            Object theObject = Activator.CreateInstance(myClass, args?.ToArray());

            MdiChild md = new MdiChild()
            {
                Title = title,
                Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/" + iconame)),
                Height = height,
                Width = width,
                Content = (UserControl)theObject,
            };

            MdiControl.mdParent.Children.Add(md);
            md.Closing += ((IMdiControl)theObject).Close;

            AddToTree(((IMdiControl)theObject).TreeName(), parent);
            ExpandRecursively(mainTree.Items, true);
        }

        public static void RefreshMdi(Type myClass)
        {
            MdiChild md = FindChild(myClass);

            if (md != null)
                ((IMdiControl)md.Content).OnRefresh();
        }

        private static void ExpandRecursively(ItemCollection itemsControl, bool expand)
        {
            foreach (TreeViewItem it in itemsControl)
            {
                it.IsExpanded = expand;

                if (it.HasItems)
                {
                    ItemsControl ic = (ItemsControl)it;
                    ExpandRecursively(it.Items, expand);
                }
            }
        }

        private static TreeViewItem FindTreeItem(ItemCollection items, string name)
        {

            TreeViewItem foundItem = null;

            foreach (TreeViewItem it in items)
            {
                if (it.Header.Equals(name))
                {
                    foundItem = it;
                    return foundItem;
                }

                if (it.HasItems)
                {
                    ItemsControl ic = (ItemsControl)it;
                    foundItem = FindTreeItem(ic.Items, name); //Recursive call
                }
            }

            return foundItem;
        }

        private static void AddToTree(string name, string parent)
        {
            if (mainTree == null)
                FindMainContainer();

            if (parent.Equals(""))
                parent = "AKTYWNE OKNA";

            TreeViewItem newChild = new TreeViewItem();
            newChild.Header = name;
            FindTreeItem(mainTree.Items, parent).Items.Add(newChild);
        }

        private static void RemoveTreeItem(string name)
        {
            TreeViewItem it = FindTreeItem(mainTree.Items, name);

            if ((it!= null) && (it.Parent != null))
            {
                var parent = it.Parent as TreeViewItem;
                if (parent != null)
                {
                    parent.Items.Remove(it);
                }
            }
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

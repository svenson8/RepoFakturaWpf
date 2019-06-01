using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPF.MDI;

namespace FakturaWpf.Tools
{
    class MdiControl
    {
        public static MdiContainer mdParent = null;

        static MdiControl()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                    mdParent = (window as MainWindow).MdiMain;
            }
        }

        public static MdiChild FindChild(string childname)
        {
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
            TreeControl.RemoveTreeItem(treename);
        }

        public static void AddChild(Type myClass, object[] args, string title, string iconame, int height, int width, string parentree = "")
        {
            Object theObject = Activator.CreateInstance(myClass, args?.ToArray());

            MdiChild md = new MdiChild()
            {
                Title = title,
                Icon = Various.GetImageFromResources(iconame),
                Height = height,
                Width = width,
                Content = (UserControl)theObject,               
            };

            MdiControl.mdParent.Children.Add(md);
            md.Closing += ((IMdiControl)theObject).Close;
         
            TreeControl.AddToTree(((IMdiControl)theObject).TreeName(), parentree);
            TreeControl.ExpandRecursively(TreeControl.mainTree.Items, true);

            SetPosition(((IMdiControl)theObject).TreeName(), parentree, md);
        }

        private static void SetPosition(string treename, string parentree, MdiChild md)
        {
            if (parentree.Equals(""))
            {
                md.Position = new Point(0, 0);
                TreeControl.FindTreeItem(TreeControl.mainTree.Items, treename).Tag = 0;
            }
            else
            {
                int tagp = (int)TreeControl.FindTreeItem(TreeControl.mainTree.Items, parentree).Tag;
                md.Position = new Point(tagp + 30, tagp + 30);
                TreeControl.FindTreeItem(TreeControl.mainTree.Items, treename).Tag = tagp + 30;
            }

        }

        public static void RefreshMdi(Type myClass, object obj = null)
        {
            MdiChild md = FindChild(myClass);

            if (md != null)
                ((IMdiControl)md.Content).OnRefresh(obj);
        }

     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FakturaWpf.Tools
{
     class TreeControl
    {
        public static TreeView mainTree = null;

        static TreeControl()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                    mainTree = (window as MainWindow).mainTreeView;
            }

        }

        public static void ExpandRecursively(ItemCollection itemsControl, bool expand)
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

        public static TreeViewItem FindTreeItem(ItemCollection items, string name)
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

        public static void AddToTree(string name, string parent)
        {
            if (parent.Equals(""))
                parent = "AKTYWNE OKNA";

            TreeViewItem newChild = new TreeViewItem();
            newChild.Header = name;
            FindTreeItem(mainTree.Items, parent).Items.Add(newChild);
        }

        public static void RemoveTreeItem(string name)
        {
            TreeViewItem it = FindTreeItem(mainTree.Items, name);

            if ((it != null) && (it.Parent != null))
            {
                var parent = it.Parent as TreeViewItem;
                if (parent != null)
                {
                    parent.Items.Remove(it);
                }
            }
        }
    }
}

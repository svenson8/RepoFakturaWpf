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

namespace FakturaWpf.MyControls
{
    /// <summary>
    /// Logika interakcji dla klasy MyCombobox.xaml
    /// </summary>
    public partial class MyCombobox : UserControl
    {
        public MyCombobox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ComboWidthProperty =
            DependencyProperty.Register("myComboWidth", typeof(int),
            typeof(MyCombobox), new FrameworkPropertyMetadata(190));

        public int myComboWidth
        {
            get { return Convert.ToInt32(GetValue(ComboWidthProperty)); }
            set { SetValue(ComboWidthProperty, value); }

        }

        public static readonly RoutedEvent SelectEvent =
            EventManager.RegisterRoutedEvent("mySelect", RoutingStrategy.Bubble,
            typeof(SelectionChangedEventHandler), typeof(MyCombobox));

        public event SelectionChangedEventHandler mySelect
        {
            add { AddHandler(SelectEvent, value); }
            remove { RemoveHandler(SelectEvent, value); }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaiseEvent(new SelectionChangedEventArgs(SelectEvent, e.RemovedItems, e.AddedItems));
        }
    }
}

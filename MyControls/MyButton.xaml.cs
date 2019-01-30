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
    /// Logika interakcji dla klasy MyButton.xaml
    /// </summary>
    public partial class MyButton : UserControl
    {
        public MyButton()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
                DependencyProperty.Register("myText", typeof(String),
                typeof(MyButton), new FrameworkPropertyMetadata(string.Empty));

        public String myText
        {
            get { return GetValue(TextProperty).ToString(); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("myImageSource", typeof(ImageSource),
            typeof(MyButton), new FrameworkPropertyMetadata(null));

        public ImageSource myImageSource
        {
            get { return GetValue(ImageSourceProperty) as ImageSource; }
            set { SetValue(ImageSourceProperty, value); }

        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("myImageWidth", typeof(int),
            typeof(MyButton), new FrameworkPropertyMetadata(24));

        public int myImageWidth
        {
            get { return Convert.ToInt32(GetValue(ImageWidthProperty)); }
            set { SetValue(ImageWidthProperty, value); }

        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("myImageHeight", typeof(int),
            typeof(MyButton), new FrameworkPropertyMetadata(24));

        public int myImageHeight
        {
            get { return Convert.ToInt32(GetValue(ImageHeightProperty)); }
            set { SetValue(ImageHeightProperty, value); }

        }


        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("myClick", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(MyButton));

        public event RoutedEventHandler myClick
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        } 

    }

}

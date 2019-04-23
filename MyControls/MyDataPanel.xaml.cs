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
    /// Logika interakcji dla klasy MyDataPanel.xaml
    /// </summary>
    public partial class MyDataPanel : UserControl
    {
        public string res;

        public MyDataPanel()
        {
            InitializeComponent();

            CB_Period.comboBox.ItemsSource = Various.GetPeriodList();
            CB_Period.comboBox.DisplayMemberPath = "desc";
            CB_Period.comboBox.SelectedValuePath = "field";
            CB_Period.comboBox.SelectedIndex = 0;

            Various.FillWithMonths(CB_Month.comboBox);
            Various.FillWithMonths(CB_Month2.comboBox);
            Various.FillWithYears(CB_Year.comboBox);
            Various.FillWithYears(CB_Year2.comboBox);

            ChangePeriodItem();
        }

        private void ChangeVisibility(DependencyObject parent, Visibility vis, Control[] exceptlist)
        {

            for (int x = 0; x < VisualTreeHelper.GetChildrenCount(parent); x++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, x);
                var instance = child as Control;

                if (null != instance)
                    instance.Visibility = vis;

                if (exceptlist != null)
                {
                    for (int i = 0; i < exceptlist.Length; i++)
                    {
                        if (instance.Equals(exceptlist[i]))
                            instance.Visibility = (vis == Visibility.Hidden ? Visibility.Visible : Visibility.Hidden);
                    }
                }
            }

        }

        private void ChangePeriodItem()
        {
            ChangeVisibility(UpperDock,
                             Visibility.Hidden,
                             new Control[] { label, CB_Period, Btn_Refresh });

            switch (CB_Period.comboBox.SelectedValue)
            {
                case "LND":
                    L_days.Content = "Liczba dni:";
                    L_days.Visibility = Visibility.Visible;
                    TB_days.Visibility = Visibility.Visible;
                    break;
                case "LNM":
                    L_days.Content = "Liczba m-cy:";
                    L_days.Visibility = Visibility.Visible;
                    TB_days.Visibility = Visibility.Visible;
                    break;
                case "M":
                    CB_Month.Visibility = Visibility.Visible;
                    CB_Month.Margin = new Thickness(-100, CB_Month.Margin.Top, CB_Month.Margin.Right, CB_Month.Margin.Bottom);
                    CB_Year.Visibility = Visibility.Visible;
                    CB_Year.Margin = new Thickness(-30, CB_Year.Margin.Top, CB_Year.Margin.Right, CB_Year.Margin.Bottom);
                    break;
                case "FMTM":
                    CB_Month2.Visibility = Visibility.Visible;
                    CB_Year2.Visibility = Visibility.Visible;
                    goto case "M";
                case "Y":
                    TB_days.Visibility = Visibility.Visible;
                    break;
                case "FYTY":
                    TB_days.Visibility = Visibility.Visible;
                    TB_days2.Visibility = Visibility.Visible;
                    TB_days2.Margin = new Thickness(-480, TB_days2.Margin.Top, TB_days2.Margin.Right, TB_days.Margin.Bottom);
                    break;
                case "PR":
                    DT_from.Visibility = Visibility.Visible;
                    DT_from.Margin = new Thickness(-800, DT_from.Margin.Top, DT_from.Margin.Right, DT_from.Margin.Bottom);
                    DT_to.Visibility = Visibility.Visible;
                    DT_to.Margin = new Thickness(-570, DT_to.Margin.Top, DT_to.Margin.Right, DT_to.Margin.Bottom);
                    break;
            }

        }

        private void CB_Period_mySelect(object sender, SelectionChangedEventArgs e)
        {
            ChangePeriodItem();
        }

        public static readonly RoutedEvent ClickEvent =
            EventManager.RegisterRoutedEvent("myClickDate", RoutingStrategy.Bubble,
            typeof(RoutedEventHandler), typeof(MyDataPanel));

        public event RoutedEventHandler myClickDate
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }

        private void Btn_Refresh_myClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ClickEvent));
        }



        /*
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
                } */
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public DateTime dateFrom;
        public DateTime dateTo;

        public MyDataPanel()
        {
            InitializeComponent();

            CB_Period.comboBox.ItemsSource = GetPeriodList();
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
                {
                    instance.Visibility = vis;

                    if (exceptlist != null)
                    {
                        for (int i = 0; i < exceptlist.Length; i++)
                        {
                            if (instance.Equals(exceptlist[i]))
                                instance.Visibility = (vis == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
                        }
                    }
                }
            }

        }

        private void ChangePeriodItem()
        {
            ChangeVisibility(Stack_date,
                             Visibility.Collapsed,
                             new Control[] { label, CB_Period});

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
                    CB_Year.Visibility = Visibility.Visible;
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
                    break;
                case "PR":
                     DT_from.Visibility = Visibility.Visible;
                     DT_to.Visibility = Visibility.Visible;
                    break;
            }

        }

        public List<object> GetPeriodList()
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

        public string GetDates(string field = null)
        {
            try
            {
                switch (CB_Period.comboBox.SelectedValue)
                {
                    case "TD":
                        dateFrom = DateTime.Now;
                        dateTo = DateTime.Now;
                        break;
                    case "YD":
                        dateFrom = DateTime.Now.AddDays(-1);
                        dateTo = DateTime.Now.AddDays(-1);
                        break;
                    case "LND":
                        dateFrom = DateTime.Now.AddDays(-Convert.ToDouble(TB_days.Text));
                        dateTo = DateTime.Now;
                        break;
                    case "TW":
                        dateFrom = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
                        dateTo = dateFrom.AddDays(6);
                        break;
                    case "LW":
                        dateFrom = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek - 1);
                        dateTo = dateFrom.AddDays(-6);
                        break;
                    case "TM":
                        dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                        dateTo = dateFrom.AddMonths(1).AddDays(-1);
                        break;
                    case "LM":
                        dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month - 1, 1);
                        dateTo = dateFrom.AddMonths(1).AddDays(-1);
                        break;
                    case "LNM":
                        dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month - Convert.ToInt32(TB_days.Text), 1);
                        dateTo = dateFrom.AddMonths(1).AddDays(-1);
                        break;
                    case "M":
                        dateFrom = new DateTime(Convert.ToInt32(CB_Year.comboBox.Text), CB_Month.comboBox.SelectedIndex + 1, 1);
                        dateTo = dateFrom.AddMonths(1).AddDays(-1);
                        break;
                    case "FMTM":
                        dateFrom = new DateTime(Convert.ToInt32(CB_Year.comboBox.Text), CB_Month.comboBox.SelectedIndex + 1, 1);
                        dateTo = new DateTime(Convert.ToInt32(CB_Year2.comboBox.Text), CB_Month2.comboBox.SelectedIndex + 1, 1).AddMonths(1).AddDays(-1);
                        break;
                    case "TY":
                        dateFrom = new DateTime(DateTime.Now.Year, 1, 1);
                        dateTo = dateFrom.AddMonths(12).AddDays(-1);
                        break;
                    case "LY":
                        dateFrom = new DateTime(DateTime.Now.Year - 1, 1, 1);
                        dateTo = dateFrom.AddMonths(12).AddDays(-1);
                        break;
                    case "Y":
                        dateFrom = new DateTime(Convert.ToInt32(TB_days.Text), 1, 1);
                        dateTo = dateFrom.AddMonths(12).AddDays(-1);
                        break;
                    case "FYTY":
                        dateFrom = new DateTime(Convert.ToInt32(TB_days.Text), 1, 1);
                        dateTo = new DateTime(Convert.ToInt32(TB_days2.Text), 1, 1).AddMonths(12).AddDays(-1);
                        break;
                    case "PR":
                        dateFrom = DT_from.SelectedDate.Value;
                        dateTo = DT_to.SelectedDate.Value;
                        break;
                }

            }
            catch
            {
                dateFrom = DateTime.MinValue;
                dateTo = DateTime.MinValue;
                return null;
            }


            if (field != null)
            {
                return " " + field + " >=" + Various.QuotedStr(dateFrom.ToShortDateString()) +
                       " and " + field + " <=" + Various.QuotedStr(dateTo.ToShortDateString());
            } else
                return "";
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

        private void TB_days_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void TB_days_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text == "") textBox.Text = "0";
        }


    }
}

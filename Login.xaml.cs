using FakturaWpf.Customer;
using FakturaWpf.Dictionary;
using FakturaWpf.Tools;
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
using System.Windows.Shapes;

namespace FakturaWpf
{
    /// <summary>
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            if (!DatabaseConnect.Connect())
            {
                Environment.Exit(-1);
            }

            if (!CheckDbStructure())
            {
                Environment.Exit(-1);
            }

            InitializeComponent();
        }

        private Boolean CheckDbStructure()
        {
            Boolean result;
            Scripts sc = new Scripts();

            result  = UserClass.ThisTableCheck();
            result = sc.Triggers(UserClass.TABLENAME);

            CustomerClass cs = new CustomerClass();
            result = cs.ThisTableCheck();
            result = sc.Triggers(cs.TableName());

            DictionaryClass dc = new DictionaryClass();
            result = dc.ThisTableCheck();
            result = sc.Triggers(dc.TableName());
            dc.InsertProvincesFromXML();

            return result;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (UserClass.LogToSystem(TB_LOGIN.Text, TB_HASLO.Password))
            {
                 MainWindow MW = new MainWindow();
                  MW.Show();

                this.Hide();
            }
            else
                Various.Warning("Niepoprawny użytkownik lub hasło", "Błąd logowania");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(-1);
        }
    }
}

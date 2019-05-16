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
using FakturaWpf.Properties;
using FakturaWpf.Documents;
using FakturaWpf.Assortment;

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

            XmlClass xl = new XmlClass();
            result = xl.ThisTableCheck();

            result = xl.InsertXmlCountries(nameof(Properties.Resources.Countries));

            DictionaryClass dc = new DictionaryClass();
            result = dc.ThisTableCheck();
            result = sc.Triggers(dc.TableName());
            dc.InsertProvincesFromXML();
            result = sc.AddCountryPorcedure();    // dodanie procedury wpisujacej kraje z pliku (bulkowanie) 
            result = dc.InsertCountriesFromProc();  // wykonanie skryptu wczytujacego kraje z xml w bazie

            DocumentClass ds = new DocumentClass();
            result = ds.ThisTableCheck();
            result = sc.Triggers(ds.TableName());

            AssortmentClass ac = new AssortmentClass();
            result = ac.ThisTableCheck();
            result = sc.Triggers(ac.TableName());

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

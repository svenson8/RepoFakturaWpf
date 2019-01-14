using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
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

namespace FakturaWpf
{
    /// <summary>
    /// Logika interakcji dla klasy ServerConf.xaml
    /// </summary>
    public partial class ServerConf : UserControl
    {
        private string pathFile;

        public ServerConf()
        {
            

            InitializeComponent();

            DataTable table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow server in table.Rows)
            {
                CB_Srever.Items.Add(server[table.Columns["ServerName"]].ToString());
            }
        }

        public static Boolean ShowAsDialog()
        {
            Window window = new Window
            {
                Title = "Konfiguracja połączenia",
                Width = 520,
                Height = 280,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new ServerConf()
            };
            
            if (window.ShowDialog() == true)
                return true;
            else
                return false;        
        }
    }
}

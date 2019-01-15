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
        public static string pathFile;
        private ServerClass server = null;
        public static Boolean start;


        public ServerConf()
        {

            pathFile = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            InitializeComponent();
            InitBinding();

            DataTable table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow server in table.Rows)
            {
                CB_Srever.Items.Add(server[table.Columns["ServerName"]].ToString());
            }
        }

        private void InitBinding()
        {
            server = new ServerClass();
            gridServer.DataContext = server;

        }

        public static Boolean ShowServerConf(Boolean atStart)
        {
            Window window = new Window
            {
                Title = "Konfiguracja połączenia",
                Width = 520,
                Height = 325,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new ServerConf()
            };

            ServerConf.start = atStart;

            if (atStart)
            {
                if (window.ShowDialog() == true)
                    return true;
                else
                    return false;
            } else
            {
                window.Show();
                return true;
            }
        }

        private void CB_Srever_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CB_Srever.SelectedIndex != -1)
            {
                TX_Server.Text = CB_Srever.SelectedItem.ToString();
                CB_Srever.SelectedIndex = -1;
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Various.InfoOk(server.ID);
        }
    }

    public class ServerClass : DatabaseConnect
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public string SERVER { get; set; }
        public string DATABASE { get; set; }

        private void Mys()
        {
          //  ServerConf.pathFile
        }

    }
}

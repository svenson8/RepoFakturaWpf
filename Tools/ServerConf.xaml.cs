using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.IO;
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
        public static Boolean OnStart;
        public static Window window;


        public ServerConf()
        {
            pathFile = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            InitializeComponent();
            InitBinding();

            FindServers(); 
        }

        private void FindServers()
        {
            MyProgressBar pg = new MyProgressBar("Wyszukiwanie serwerów ...", 100);
            pg.Show();
            pg.Step(50);
            DataTable table = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow server in table.Rows)
            {
                CB_Srever.Items.Add(server[table.Columns["ServerName"]].ToString());
            }
            pg.Step(new int[] { 90, 100 });
        }

        private void InitBinding()
        {
            server = new ServerClass();

            if (!ServerConf.OnStart)
            {
                server.ReadFromToFile();
                SetPassword(false);
            }

            gridServer.DataContext = server;

        }

        private void SetPassword(Boolean toObject)
        {
            if (toObject)
                server.Password = TX_Passsword.Password;
            else
                TX_Passsword.Password = server.Password;
        }

        public static Boolean ShowServerConf(Boolean onstart)
        {
            ServerConf.OnStart = onstart;

            window = new Window
            {
                Title = "Konfiguracja połączenia",
                Width = 520,
                Height = 365,
                Icon = Various.GetImageFromResources("ImgConfig"),
                 WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Content = new ServerConf(),
             };


            if (window.ShowDialog() == true)
            {
                if (onstart)
                  return true;
                else
                {
                    if (Various.Question("Czy uruchomić aplikację ponownie ?", "Pytanie"))
                    {
                        Various.RestartApp();
                        return true;
                    }
                    else
                        return false;
                }
            }
            else
                return false;

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
            SetPassword(true);
            server.SaveToFile();            
            Various.InfoOk("Konfiguracja połączenia do serwera zapisana pomyślnie");
            window.DialogResult = true;
        }

        private void buttonTest_Click(object sender, RoutedEventArgs e)
        {
            SetPassword(true);
            if (server.TestConnect())
                Various.InfoOk("Połączono pomyślnie", "Informacja");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            window.DialogResult = false;
        }
    }

    public class ServerClass : DatabaseConnect
    {
        public string ID { get; set; }
        public string Password { get; set; }
        public string SERVER { get; set; }
        public string DATABASE { get; set; }
       
        public void SaveToFile()
        {
            IniFile ini = new IniFile(ServerConf.pathFile);
            ini.IniWriteValue("Settings", "Id",       this.ID);
            ini.IniWriteValue("Settings", "Password", this.Password);
            ini.IniWriteValue("Settings", "Server",   this.SERVER);
            ini.IniWriteValue("Settings", "Database", this.DATABASE);
        }

        public void ReadFromToFile()
        {
            IniFile ini = new IniFile(ServerConf.pathFile);
            this.ID = ini.IniReadValue("Settings", "Id");
            this.Password = ini.IniReadValue("Settings", "Password");
            this.SERVER = ini.IniReadValue("Settings", "Server");
            this.DATABASE = ini.IniReadValue("Settings", "Database");
        }

        public Boolean TestConnect()
        {
            BuildConnectionstring(this.ID,
                                  this.Password,
                                  this.SERVER);

            if (NewConnect(this.DATABASE))
                return true;
            else
                return false;
                                 
        }

        public override int GetLengthOfStringField(string name)
        {
            throw new NotImplementedException();
        }
    }
}

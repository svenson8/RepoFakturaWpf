using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using WPF.MDI;

namespace FakturaWpf.Users
{
    /// <summary>
    /// Logika interakcji dla klasy UserList.xaml
    /// </summary>
    /// 

    public partial class UserList : UserControl
    {

        private DataTable dt;
        private const string ChildName = "UC_UserList";

        public UserList()
        {
            InitializeComponent();

            LoadData();


        }

        void LoadData()
        {
            string sques = "Select* from TLogin where ID > 0";

            if (TX_Login.Text != String.Empty)
                sques += " and NAZWA like '%" + TX_Login.Text + "%'";

            if (TX_Name.Text != String.Empty)
                sques += " and IMIE like '%" + TX_Name.Text + "%'";

            if (TX_Surname.Text != String.Empty)
                sques += " and NAZWISKO like '%" + TX_Surname.Text + "%'";

            NQueryReader nq = new NQueryReader(sques);

            dt = new DataTable();
            dt.Load(nq.NReader);
            DG_User.ItemsSource = dt.AsDataView();

            Various.SetAutoColumnWidth(DG_User, new int[] { 1, 2 });

            DG_User.SelectedIndex = 0;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //UserClass customer = (UserClass)DG_User.SelectedItem;  

            int id = 0;

            if (((Button)sender).Name.Equals("btnMod"))
            {
                try
                {
                    id = Convert.ToInt32(((DataRowView)DG_User.SelectedItem)[0]);
                }
                catch
                {
                    id = -1;
                }
            }

            if (id >= 0)
            {
                MdiControl.mdParent.Children.Add(new MdiChild()
                {
                    Title = "Dane użytkownika",
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/faktura.ico")),
                    Height = 395,
                    Width = 565,
                    Name = UserEdit.ChildName,
                    Content = new UserEdit(id)
                });
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}

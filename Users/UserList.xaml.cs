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
        private MdiContainer mdiParent;

        public UserList(MdiContainer mdi)
        {
            InitializeComponent();
            this.mdiParent = mdi;

         //   Point NewLoc = Screen.FromControl(this).WorkingArea.Location,
            LoadData();



             /*      < DataGridTextColumn.Header >
                        < TextBlock Text = "Login" FontWeight = "Bold" TextAlignment = "Center" />
     
                         </ DataGridTextColumn.Header > */
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

            DataTable dt = new DataTable();
            dt.Load(nq.NReader);
            DG_User.ItemsSource = dt.AsDataView();

            Various.SetAutoColumnWidth(DG_User, new int[] { 1, 2 });



        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
             mdiParent.Children.Add(new MdiChild()
            {
                Title = "Dane użytkownika",
                Icon = new BitmapImage(new Uri("pack://application:,,,/Resources/faktura sprzedazy16.ico")),
                Height = 395,
                Width = 565,
                Content = new UserEdit()
            });
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}

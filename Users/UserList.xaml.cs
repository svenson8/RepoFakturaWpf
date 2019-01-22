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

namespace FakturaWpf.Users
{
    /// <summary>
    /// Logika interakcji dla klasy UserList.xaml
    /// </summary>
    public partial class UserList : UserControl
    {
        public UserList()
        {
            InitializeComponent();

         //   Point NewLoc = Screen.FromControl(this).WorkingArea.Location,
            LoadData();



             /*      < DataGridTextColumn.Header >
                        < TextBlock Text = "Login" FontWeight = "Bold" TextAlignment = "Center" />
     
                         </ DataGridTextColumn.Header > */
        }

        void LoadData()
        {
            NQueryReader nq = new NQueryReader("Select * from TLogin");

            DataTable dt = new DataTable();
            dt.Load(nq.NReader);
            DG_User.ItemsSource = dt.AsDataView();

            Various.SetAutoColumnWidth(DG_User, new int[] { 1, 2 });



        }
    }
}

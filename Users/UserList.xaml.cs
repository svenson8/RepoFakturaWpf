using FakturaWpf.MyControls;
using FakturaWpf.Tools;
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

    public partial class UserList : UserControl, IMdiControl
    {

        private DataTable dt;


        public UserList()
        {
            try
            {
                InitializeComponent();

                LoadData();
                this.Focusable = true;
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.UserList: " + ex.Message, "Błąd");
            }
        }

        void LoadData()
        {
            try
            {

                string sques = "Select* from TLogin where ID > 0";

                if (TX_Login.Text != String.Empty)
                    sques += " and NAZWA like '%" + TX_Login.Text + "%'";

                if (TX_Name.Text != String.Empty)
                    sques += " and IMIE like '%" + TX_Name.Text + "%'";

                if (TX_Surname.Text != String.Empty)
                    sques += " and NAZWISKO like '%" + TX_Surname.Text + "%'";

                if (CH_Active.IsChecked == true)
                    sques += " and ACTIVE =" + Various.QuotedStr("N");

                NQueryReader nq = new NQueryReader(sques);

                dt = new DataTable();
                dt.Load(nq.NReader);
                DG_User.ItemsSource = dt.AsDataView();

                Various.SetAutoColumnWidth(DG_User, new int[] { 1, 2 });

                DG_User.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.LoadData: "+ex.Message, "Błąd");
            }

        }

        private void GetId(out int id)
        {
            try
            {
                id = Convert.ToInt32(((DataRowView)DG_User.SelectedItem)["ID"]);
            }
            catch
            {
                id = -1;
            }
          
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            EditPosition(((MyButton)sender).Name.Equals("btnMod"));
        }

        private void EditPosition(bool mod)
        {
            try
            {
                int id = 0;

                if (mod)
                {
                    GetId(out id);
                }

                if (id >= 0)
                {
                    MdiControl.AddChild(typeof(UserEdit), new object[] { id }, "Dane użytkownika", "ImgFakt", 395, 575, TreeName());
                }
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.EditPosition: " + ex.Message, "Błąd");
            }
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void Close(object sender, RoutedEventArgs e)
        {
            try
            {
                MdiControl.CloseMdi(typeof(UserList), TreeName());
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.Close: " + ex.Message, "Błąd");
            }
        }

        public void OnRefresh(object obj = null)
        {
            LoadData();
        }

        private void MyButton_myClick(object sender, RoutedEventArgs e)
        {
            try
            {
                GetId(out int id);

                if (id > 0)
                {
                    if (Various.Question("Czy na pewno usunąć użytkownika ?" + ((DataRowView)DG_User.SelectedItem)["NAZWA"], "Pytanie"))
                    {
                        if (UserClass.DeleteUser(id))
                        {
                            Various.InfoOk("Użytkownik usunięty");
                            LoadData();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.MyButton_myCilck: " + ex.Message, "Błąd");
            }
        }

        public string TreeName()
        {
            return "Lista użytkow.";
        }

        private void UC_UserList_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Escape)
                    Close(sender, e);

                if (e.Key == Key.Insert)
                    EditPosition(false);

                if (e.Key == Key.F6)
                    EditPosition(true);

                if (e.Key == Key.Delete)
                    MyButton_myClick(sender, e);

                if (e.Key == Key.F3)
                    button5_Click(sender, e);
            }
            catch (Exception ex)
            {
                Various.Warning("UserList.UC_UserList_PreviewKeyDown: " + ex.Message, "Błąd");
            }

        }

        private void MyButton_myClick_1(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog dlgPrint = new PrintDialog();
                WpfPrinting p = new WpfPrinting();

                p.PrintDataGrid(Various.GetHeader("Lista użytkowników"), DG_User, null, dlgPrint, false, false, false);
            }
            catch
            {
                Various.Error("UserList.MyButton_myClick_1");
            }
        }
    }
}

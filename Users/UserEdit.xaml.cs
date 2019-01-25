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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.MDI;

namespace FakturaWpf.Users
{
    /// <summary>
    /// Logika interakcji dla klasy UserControl1.xaml
    /// </summary>
    public partial class UserEdit : UserControl
    {
        private UserClass user;
        public const string ChildName = "UC_UserEdit";

        public UserEdit(int id)
            
        {
            InitializeComponent();

            InitBinding(id);
        }

        private void InitBinding(int id)
        {
            user = new UserClass(id);
            if (id > 0)
                user.ReadUser(id);

            gridField.DataContext = user;

            CH_Active.IsChecked = (user.ACTIVE.Equals("T")) ? true : false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            user.ACTIVE = (CH_Active.IsChecked == true) ? "T" : "F";

            if (user.SaveUser())
                Various.InfoOk("Użytkownik zapisany", "Informacja");
            else
                Various.Warning("Błąd zapisu danych", "");

            MdiControl.CloseMdi(UserEdit.ChildName);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(UserEdit.ChildName);
        }
    }
}

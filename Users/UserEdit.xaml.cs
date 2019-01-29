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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.MDI;

namespace FakturaWpf.Users
{
    /// <summary>
    /// Logika interakcji dla klasy UserControl1.xaml
    /// </summary>
    public partial class UserEdit : UserControl, IMdiControl
    {
        private UserClass user;

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

            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public string ChildName()
        {
            return this.Name;
        }

        public void Close()
        {
            MdiControl.CloseMdi(ChildName());
        }

        public void OnRefresh()
        {
            throw new NotImplementedException();
        }

    }
}

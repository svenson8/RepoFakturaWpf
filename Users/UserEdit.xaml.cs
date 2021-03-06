﻿using FakturaWpf.Tools;
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
            this.Focusable = true;
        }

        private void InitBinding(int id)
        {
            user = new UserClass(id);
            if (id > 0)
            {
                user.ReadUser(id);
                TX_Passsword.Password     = user.HASLO;
                TX_Passsword_rep.Password = user.HASLO;
            }

            gridField.DataContext = user;

        }

        private Boolean CheckPassword()
        {
             if (! TX_Passsword.Password.Equals(TX_Passsword_rep.Password))
            {
                Various.Warning("Błędnie powtórzone hasło", "Ostrzeżenie");
                return false;
            }
            user.HASLO = TX_Passsword.Password;
            return true;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (TX_Name.Text.Length == 0)
            {
                Various.Warning("Nie podano nazwy użytkownika", "Ostrzeżenie");
                return;
            }

            if (CheckPassword())
            {

                if (user.SaveUser())
                {
                    Various.InfoOk("Użytkownik zapisany", "Informacja");
                    MdiControl.RefreshMdi(typeof(UserList));
                }
                else
                    Various.Warning("Błąd zapisu danych", "");

                Close(sender , e);
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            Close(sender, e);
        }


        public void Close(object sender, RoutedEventArgs e)
        {
            MdiControl.CloseMdi(typeof(UserEdit), TreeName());
        }

        public void OnRefresh(object obj = null)
        {
            throw new NotImplementedException();
        }

        public string TreeName()
        {
            return "Dane użytkow.";
        }

        private void UC_UserEdit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
                button_Click(sender, e);

            if (e.Key == Key.Escape)
                button2_Click(sender, e);

        }
    }
}

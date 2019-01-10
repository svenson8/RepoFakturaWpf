using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FakturaWpf
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    /// 


    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)         // Przeciązona metoda OnStartup, w której możemy zmienić okno startu (obliagoryjne)
        {
            base.OnStartup(e);
            this.StartupUri = new Uri("Login.xaml", UriKind.Relative);
        }
    }
}

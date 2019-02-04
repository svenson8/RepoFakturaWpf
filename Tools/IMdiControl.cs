using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FakturaWpf.Tools
{

    public interface IMdiControl
    {
        
        void Close(object sender, RoutedEventArgs e);
        void OnRefresh();
        string TreeName();

    }
}

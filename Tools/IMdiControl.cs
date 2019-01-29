using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Tools
{
    interface IMdiControl
    {
        string ChildName();
        void Close();
        void OnRefresh();
                   
    }
}

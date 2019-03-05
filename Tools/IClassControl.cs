using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Tools
{
    interface IClassControl
    {
        int GetLengthOfStringField(string name);
        List<object> ThisReadListData();
        Boolean ThisSaveData();
        void ThisReadData();
        bool ThisTableCheck();
    }
}

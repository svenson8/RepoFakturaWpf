using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


    [ServiceContract]
    public interface IMyWcf
    {
        [OperationContract]
        string Hello();

        [OperationContract]
        int AddNumbers(int a, int b);
    }


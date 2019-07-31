using System;
using System.Collections.Generic;
using System.ComponentModel; // Container
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


    [ServiceContract]
    public interface IContainerSvc
    {
        [OperationContract]
        Container GetSomeData();
    }


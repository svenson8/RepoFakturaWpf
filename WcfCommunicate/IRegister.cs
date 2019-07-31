using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

//Kontrakt serwera
[ServiceContract(CallbackContract = typeof(INotify))]
public interface IRegister
{
    [OperationContract(IsOneWay = true)]
    void RunTask();
}
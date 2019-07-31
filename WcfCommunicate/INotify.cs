using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

[ServiceContract]
public interface INotify
{
    [OperationContract(IsOneWay = true)]
    void Notify();
}
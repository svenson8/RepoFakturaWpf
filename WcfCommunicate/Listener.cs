using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Listener : INotify
{
    //Obiekt synchronizacji, ustawiany po otrzymaniu odpowiedzi z serwera
    private AutoResetEvent notifyEvent = new AutoResetEvent(false);
    public AutoResetEvent FinishedEvent
    {
        get
        {
            return notifyEvent;
        }
    }

    public void Notify()
    {
        FakturaWpf.Various.InfoOk("Notified...");
        notifyEvent.Set();
    }
}

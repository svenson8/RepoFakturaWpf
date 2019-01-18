using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Tools
{
    class Scripts :DatabaseConnect
    {

        public static Boolean UserInsertTrigger()
        {
            NQuery nQ = new NQuery("CREATE  OR ALTER TRIGGER TloginAFterInsert ON TLogin "+
                                   "FOR INSERT "+
                                   "AS "+
                                   "UPDATE TLogin "+
                                   "SET DATAW = GETDATE(), "+
                                   "DMODDATE = GETDATE() "+
                                   "FROM Inserted i "+
                                   "WHERE TLogin.ID = i.ID ");
            if (nQ.WellDone)
                return true;
            else
                return false;
        }
    }
}

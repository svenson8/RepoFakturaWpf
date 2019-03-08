using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Tools
{
    

    class Scripts :DatabaseConnect
    {
        public delegate Boolean AddTrigers(string table);

        private AddTrigers _addtrig;

        public Boolean Triggers(string table)
        {
            if (_addtrig == null)
            {
                _addtrig += InsertTrigger;
                _addtrig += UpdateTrigger;
            }
            return _addtrig(table);
        }

        public Boolean InsertTrigger(string table)
        {
            NQuery nQ = new NQuery("CREATE  OR ALTER TRIGGER "+table+"AFterInsert ON "+table+" "+
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

        public Boolean UpdateTrigger(string table)
        {
            NQuery nQ = new NQuery("CREATE  OR ALTER TRIGGER "+table+"AFterUpdate ON "+table+ " "+
                                   "AFTER UPDATE " +
                                   "AS " +
                                   "UPDATE Tlogin " +
                                   "SET DMODDATE = GETDATE() " +
                                   "WHERE ID IN(SELECT DISTINCT ID FROM Inserted) ");

            if (nQ.WellDone)
                return true;
            else
                return false;
        }

    }
}

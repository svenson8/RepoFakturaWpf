using FakturaWpf.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        public Boolean AddCountryPorcedure()
        {
            string script = Resources.XML_COUNT_SCR.ToString();
            NQuery nq = new NQuery(script);
            return nq.WellDone;
        }

        public Boolean AddSumProcedure()
        {
            string script = "CREATE OR ALTER PROCEDURE GET_SUM " + 
                            "@ID_DOK varchar(100) " +
                            "AS " +
                            "BEGIN " +
                            "  DECLARE @sumnet decimal(18, 4); " +
                            "            DECLARE @sumvat decimal(18, 4); " +
                            "            DECLARE @sumbr  decimal(18, 4); " +
                            "            DECLARE @strSQL varchar(2000) " +

                            "SET @strSQL = 'select sum(MDWARNET), sum(MDWARVAT), sum(MDWARBR) from TDOKumnety where ID IN (' + @ID_DOK + ')' " +
                            "EXECUTE(@strSQL) " +
                            "END ";
            NQuery nq = new NQuery(script);
            return nq.WellDone;
        }

    }
}

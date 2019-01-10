using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf
{
    class UserClass : DatabaseConnect
    {
        public static string UserACTLogin;
        public static int UserACTId;

        public int      ID       { get; set; }
        public string   NAZWA    { get; set; }
        public string   HASLO    { get; set; }
        public string   IMIE     { get; set; }
        public string   NAZWISKO { get; set; }
        public DateTime DATAW    { get; set; }
        public string   TELEFON  { get; set; }

        public const string TABLENAME = "Tlogin";

        public static Boolean ThisTableCheck()
        {
            List<Params> list = new List<Params>();
            list.Add(new Params("ID",       SqlDbType.Int, 0));
            list.Add(new Params("NAZWA",    SqlDbType.VarChar, 50));
            list.Add(new Params("HASLO",    SqlDbType.VarChar, 50));
            list.Add(new Params("IMIE",     SqlDbType.VarChar, 50));
            list.Add(new Params("NAZWISKO", SqlDbType.VarChar, 50));
            list.Add(new Params("DATAW",    SqlDbType.DateTime, 0));
            list.Add(new Params("TELEFON",  SqlDbType.VarChar, 50));

            return TableCheck(TABLENAME, list);
        }


        public static Boolean LogToSystem(string log, string pass)
        {
            List<Params> list = new List<Params>();
            list.Add(new Params("@NZ", SqlDbType.VarChar, log));
            list.Add(new Params("@HS", SqlDbType.VarChar, pass));
            NQueryReader nQ = new NQueryReader("select ID from "+TABLENAME+" where NAZWA = @NZ and HASLO = @HS", list);

            while (nQ.NReader.Read())
            {
                UserACTLogin = log;
                UserACTId = nQ.NReader.GetInt32(0);
                nQ.NReader.Close();
                return true;
            }



            nQ.NReader.Close();
            return false;
        }

        public Boolean SaveUser()
        {
             Boolean New = false;
             if (this.ID.Equals(0))
             {
                New = true;
             }

             List<Params> list = new List<Params>();
             list.Add(new Params("@ID", SqlDbType.Int,       this.ID));
             list.Add(new Params("@NW", SqlDbType.VarChar,   this.NAZWA));
             list.Add(new Params("@IM", SqlDbType.VarChar,   this.IMIE));
             list.Add(new Params("@NZ", SqlDbType.VarChar,   this.NAZWISKO));
             list.Add(new Params("@DW", SqlDbType.DateTime,  this.DATAW));
             list.Add(new Params("@HS", SqlDbType.VarChar,   this.HASLO));
             list.Add(new Params("@TL", SqlDbType.VarChar,   this.TELEFON));

            string sqlpyt = "";

             if (New)
             {
                sqlpyt = "insert into "+TABLENAME+" (NAZWA, IMIE, NAZWISKO, DATAW, HASLO, TELEFON) values (@NW, @IM, @NZ, @DW, @HS, @TL)"; 
             }
             else
             {
                 sqlpyt = "update "+TABLENAME+" set NAZWA =@NW, IMIE = @IM, NAZWISKO = @NZ, HASLO = @HS, DATAW = @DW, TELEFON = @TL where ID = @ID";
             }

             NQuery nQ = new NQuery(sqlpyt, list); 
             return (nQ.WellDone);  

        }

        public void ReadUser(int Id)
        {
            List<Params> list = new List<Params>();
            list.Add(new Params("@ID", SqlDbType.Int, Id));
            NQueryReader nQ = new NQueryReader("select ID,NAZWA,IMIE,NAZWISKO,HASLO,DATAW,TELEFON from "+TABLENAME+" where ID = @ID", list);

            while (nQ.NReader.Read())
            {
                this.ID       = nQ.NReader.GetInt32(0); 
                this.NAZWA    = nQ.NReader.GetString(1);
                this.IMIE     = nQ.NReader.GetString(2);
                this.NAZWISKO = nQ.NReader.GetString(3);
                this.HASLO    = nQ.NReader.GetString(4);
                this.DATAW    = nQ.NReader.GetDateTime(5);
                this.TELEFON = nQ.NReader.GetString(6);
                nQ.NReader.Close();
                return;
            }

        }

        public static Boolean DeleteUser(int Id)
        {
              List<Params> list = new List<Params>();
              list.Add(new Params("@ID", SqlDbType.Int, Id));

              NQuery nQ = new NQuery("delete from "+TABLENAME+" where ID = @ID", list); 
            return (nQ.WellDone); 
        }
    }
}

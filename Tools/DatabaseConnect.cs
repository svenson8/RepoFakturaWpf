using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace FakturaWpf
{
    public class DatabaseConnect
    {
        public static string ConnectionString = "";

        public static SqlConnection conn;

        public static Boolean Connect()
        {

            string pathf = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            if (!File.Exists(pathf))
            {
                Various.Error("Brak pliku config.ini", "Błąd");
                return false;
            }

            IniFile ini = new IniFile(pathf);
            ConnectionString = @"user id =" + ini.IniReadValue("Settings", "Id") +
                                ";password=" + ini.IniReadValue("Settings", "Password") +
                                ";server=" + ini.IniReadValue("Settings", "Server") +
                                ";Trusted_Connection=" + ini.IniReadValue("Settings", "Trusted") +
                                ";connection timeout=" + ini.IniReadValue("Settings", "Timeout");
                                //MultipleActiveResultSets = True

          
            if (NewConnect()) {
                CheckOrCreateDB(ini.IniReadValue("Settings", "Database"));
            } else
                return false;

            return true;
        }

        private static Boolean NewConnect(string database = "")
        {
            if (database != String.Empty)
              ConnectionString += ";database=" + database;

            if (conn != null)
            {
                conn.Dispose();                
            }

            conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();
                return true;
            }
            catch (Exception e)
            {
                Various.Error(e.Message, "Błąd");
                return false;
            }


        }

        private static void CheckOrCreateDB(string database)
        {
            NQueryReader nq = new NQueryReader("SELECT * FROM master.dbo.sysdatabases where name = '" + database + "'");
            Boolean exist = false;

            while (nq.NReader.Read())
            {
                exist = true;
            }
            nq.NReader.Close();

            if (!exist)
            {
                if (Various.Question("Baza danych: " + database + " nie istnieje. Czy utworzyć ?", "Pytanie"))
                {
                    NQuery nQ = new NQuery("CREATE DATABASE " + database);
                    if (nQ.WellDone)
                    {
                        Various.InfoOk("Baza " + database + " utworozna pomyślnie", "Potwierdzenie");

                        if (NewConnect(database))
                        {
                            UserClass.ThisTableCheck();
                            UserClass us = new UserClass(0, "admin", "a");
                            us.SaveUser();
                        }
                        else
                            Environment.Exit(-1);
                    }
                    else
                        Environment.Exit(-1);
                }
                else
                    Environment.Exit(-1);
            }
            else
                NewConnect(database);
        }


        public static Boolean TableCheck(string tableName, List<Params> list)
        {
            NQueryReader nq = new NQueryReader("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N" + "'" + tableName + "'");
            Boolean exist = false;
            Boolean result = true;

            while (nq.NReader.Read()){
                exist = true;                
            }
            nq.NReader.Close();

            if (!exist)
            {
                NQuery nQ = new NQuery("create table " + tableName + "( " +
                                       "[ID][int] IDENTITY(1, 1) NOT NULL, " +
                                       "CONSTRAINT[PK_" + tableName + "] PRIMARY KEY CLUSTERED " +
                                       "( [ID] ASC ) " +
                                       ") ON [PRIMARY] ");
                if (nQ.WellDone)
                    exist = true;
            }

            if (exist)
            {
                foreach (Params p in list)
                {
                    string dbtype = p.type.ToString();
                    if (p.type is SqlDbType.VarChar){
                        dbtype = dbtype+"("+p.value.ToString()+")";                      
                    }
                    NQuery n = new NQuery("IF NOT EXISTS ( SELECT  * FROM    syscolumns WHERE   id = OBJECT_ID('" + tableName + "') AND name = '" + p.name + "') " +
                                          "ALTER TABLE " + tableName + " ADD " + p.name + " "+dbtype+" NULL");
                }
            } else
            {
                Various.Warning("Błąd struktury bazy danych ! Tabela " + tableName + " nie istnieje");
                result = false;
            }

            return result;
        }
    }



    public class NQuery : DatabaseConnect
    {
        public Boolean WellDone = true;

        public NQuery(string sqlquet, List<Params> list = null)
        {            
            try
            {
                SqlCommand SqlComm = new SqlCommand(sqlquet, conn);
                Params.AddParameters(ref SqlComm, list);
                SqlComm.ExecuteNonQuery();                
            }
            catch (Exception ex)
            {
                WellDone = false;
                Various.Warning("Błąd zapisu danych: " +ex.Message, "Błąd Zapisu");
            }
        }
    }

    public class Params
    {
        public string name { get; set; }
        public SqlDbType type { get; set; }
        public object value { get; set; }

        public Params(string name, SqlDbType type, object value)
        {
            this.name  = name;
            this.type  = type;
            this.value = value;
        }
        
        public static void AddParameters(ref SqlCommand Sqlc, List<Params> list)
        {
            if (list != null)
            {
                foreach (Params p in list)
                {
                      Sqlc.Parameters.Add(p.name, p.type);
                      Sqlc.Parameters[p.name].Value = p.value;
                }
            }
        }
        
    }

    public class NQueryReader : DatabaseConnect
    {
        public SqlDataReader NReader;

        public NQueryReader(string sqlquet, List<Params> list= null)
        {
            try
            {
                SqlCommand command = new SqlCommand(sqlquet, conn);
                Params.AddParameters(ref command, list);
                NReader = command.ExecuteReader();                
            }
            catch (Exception ex)
            {
                Various.Warning("Błąd odczytu danych: "+ex.Message, "Błąd odczytu");
            }

        } 
    }


}

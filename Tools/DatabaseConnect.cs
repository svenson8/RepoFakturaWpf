using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows;
using System.Reflection;
using System.Xml;
using System.Data.SqlTypes;

namespace FakturaWpf
{
    public abstract class DatabaseConnect
    {
        public static string ConnectionString = "";
        public static SqlConnection conn;
        public const string Trusted = "false";
        public const string Timeout = "30";


        public static Boolean Connect()
        {

            string pathf = AppDomain.CurrentDomain.BaseDirectory + "config.ini";
            if (!File.Exists(pathf))
            {
                Various.Error("Brak pliku config.ini", "Błąd");
                if (!ServerConf.ShowServerConf(true))          
                return false;  
            }

            IniFile ini = new IniFile(pathf);
            BuildConnectionstring(ini.IniReadValue("Settings", "Id"),
                                  ini.IniReadValue("Settings", "Password"),
                                  ini.IniReadValue("Settings", "Server"));

          
            if (NewConnect()) {
                CheckOrCreateDB(ini.IniReadValue("Settings", "Database"));
            } else
                return false;

            return true;
        }

        public static void BuildConnectionstring(string id,string pass, string ser)
        {

            ConnectionString = @"user id =" + id +
                               ";password=" + pass +
                               ";server=" + ser +
                               ";Trusted_Connection=" +Trusted +
                               ";connection timeout="+ Timeout;
        }                      //MultipleActiveResultSets = True


        public static Boolean NewConnect(string database = "")
        {
            if ((database != String.Empty) && (database != null))
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

        public Boolean DeletPosition(int id, string table)
        {
            if (id > 0)
            {
                NQuery n = new NQuery("delete from " + table + " where ID=" + id.ToString());
                return n.WellDone;
            }
            return true;
        }


        public virtual Boolean TableCheck(string tableName, Type typ, Func<string, int> met)//, List<Params> list)
        {

            List<Params> list = new List<Params>();
            PropertyInfo[] property = typ.GetProperties();

            foreach (PropertyInfo prop in property.Where(p => p.MemberType == MemberTypes.Property))
            {

                list.Add(new Params(prop.Name,
                                    GetSqlTypeFromVariable(prop.PropertyType),
                                    met(prop.Name)));
            }

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
                    if (p.type is SqlDbType.VarChar)
                        dbtype = dbtype+"("+p.value.ToString()+")";

                    if (p.type is SqlDbType.Decimal)
                        dbtype = dbtype + "(18," + p.value.ToString() + ")";

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

        public SqlDbType GetSqlTypeFromVariable(Type type)
        {            
            if (type == typeof(string))
              return SqlDbType.VarChar;

            if (type == typeof(int))
                return SqlDbType.Int;

            if (type == typeof(DateTime))
                return SqlDbType.DateTime;

            if (type == typeof(Decimal))
                return SqlDbType.Decimal;

            if (type == typeof(XmlDocument))
                return SqlDbType.Xml;

            return SqlDbType.Int;
        }

        public void ReadData(object obj, string table, int id)
        {
            NQueryReader nQ = new NQueryReader("select * from " + table + " where ID=" + id.ToString());

            while (nQ.NReader.Read())
            {
                foreach (PropertyInfo propf in obj.GetType().GetProperties())
                {
                    if (nQ.NReader[propf.Name].GetType() != typeof(DBNull))
                    {
                        Convert.ChangeType(nQ.NReader[propf.Name], propf.PropertyType);  // prawdopodobnie niepotrzebne
                        propf.SetValue(obj, nQ.NReader[propf.Name]);
                    }
                }
            }
            nQ.NReader.Close();
        }

        public virtual List<object> ReadListData(object obj, string table, object[] args)
        {
            NQueryReader nQ = new NQueryReader("select * from " + table);
            List<object> listU = new List<object>();

            while (nQ.NReader.Read())
            {
                Object u = Activator.CreateInstance(obj.GetType(), args?.ToArray());
                foreach (PropertyInfo propf in u.GetType().GetProperties())
                {
                    if (nQ.NReader[propf.Name].GetType() != typeof(DBNull))
                    {
                        Convert.ChangeType(nQ.NReader[propf.Name], propf.PropertyType);  // prawdopodobnie niepotrzebne
                        propf.SetValue(u, nQ.NReader[propf.Name]);
                    }
                }
                listU.Add(u);
            }
            nQ.NReader.Close();
            return listU;
        }

        public virtual int SaveData(int ID, string table, Type typ, object obj)
        {

            BuildSaveString bfs = new BuildSaveString(ID.Equals(0), table);

            List<Params> list = new List<Params>();
            PropertyInfo[] property = typ.GetProperties();

            foreach (PropertyInfo prop in property.Where(p => p.MemberType == MemberTypes.Property))
            {
                if (prop.Name != nameof(ID))
                {
                    list.Add(new Params("@" + prop.Name,
                                        GetSqlTypeFromVariable(prop.PropertyType),
                                        CheckValue(prop.GetValue(obj, null))));

                    bfs.AddString(prop.Name);
                }

            }

            NQuery nQ = new NQuery(bfs.GetResult(ID), list);
            if ((ID <= 0) && (nQ.WellDone))
            {
                NQueryReader nQ1 = new NQueryReader("select MAX(ID) from " + table);
                nQ1.NReader.Read();
                ID = nQ1.NReader.GetInt32(0);
                nQ1.NReader.Close();
            }

            if (!nQ.WellDone)
                ID = 0;

            return (ID);
        }

        private object CheckValue(object ob)
        {
            if (ob is DateTime)
            {
                if ((DateTime)ob == DateTime.MinValue)
                    return null;
            }

            if (ob is XmlDocument)
                return new SqlXml(new XmlTextReader((ob as XmlDocument).InnerXml, XmlNodeType.Document, null));

            return ob;
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
                      Sqlc.Parameters[p.name].Value = p.value ?? (object)DBNull.Value;
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

        public Boolean NCheckCount()
        {
            while (NReader.Read())
            {
                if (NReader.GetInt32(0) > 0)
                {
                    NReader.Close();
                    return true;
                }
            }        

            NReader.Close();
            return false;
        }



    }

    public class BuildSaveString
    {
        private string outstring;
        private string tmps1;
        private string tmps2;
        private Boolean insert;

        public BuildSaveString(Boolean ainsert, string table)
        {
            insert = ainsert;
            if (insert)
                outstring = "insert into " + table;
            else
                outstring = "update " + table + " set ";          
        }

        public void AddString(string add1)
        {
            if (insert)
            {
                if (tmps1 == null)
                    tmps1 += " (";

                if (tmps2 == null)
                    tmps2 += "(";

                tmps1 += add1 + ",";
                tmps2 += "@"+add1 + ",";
            } else
            {
                tmps1 += add1 + "=" + "@"+add1 + ",";
            }

        }

        public string GetResult(int id=0)
        {
            tmps1 = tmps1.TrimEnd(',');

            if (insert)
            {
                tmps2 = tmps2.TrimEnd(',');
                return outstring + tmps1 + ") values " + tmps2 + ")";
            } else
            {
                return outstring + tmps1 + " where ID = "+id.ToString();
            }
        }

    }

}

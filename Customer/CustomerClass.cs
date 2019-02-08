using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Customer
{
    class CustomerClass :DatabaseConnect
    {
        /*  public int ID { get; set; }
          public string KLINAZ { get; set; }
          public string KLINAZSKROT { get; set; }
          public string KLINIP { get; set; }
          public string KLIREGON { get; set; }
          public string KLITEL1 { get; set; }
          public string KLITEL2 { get; set; }
          public string KLIFAX { get; set; }
          public string KLIWWW { get; set; }
          public string KLIMAIL { get; set; }
          public string KLIBANK { get; set; }
          public string KLIULICA { get; set; }
          public string KLINRDOMU { get; set; }
          public string KLINRLOK { get; set; }
          public string KLIKOD { get; set; }
          public string KLIMIEJSC { get; set; }
          public string KLIWOJID { get; set; }
          public string KLIKRAJID { get; set; }
          public string KLIUWAGI { get; set; }
          public string KLIPESEL { get; set; }
          public string KLIIMIE { get; set; }
          public string KLINAZWISKO { get; set; }
          public string KLIDOWOD { get; set; }
          public string KLDOWODWYD { get; set; }
          public string KLIDOWODDATA { get; set; }
          public DateTime DATAW { get; set; }
          public DateTime DMODDATE { get; set; }
          public string ACTIVE { get; set; } */

        public string ACTIVE { get; set; }

        public MyField ID = new MyField(null, SqlDbType.Int, 0);
        public MyField KLINAZ = new MyField(null, SqlDbType.VarChar, 200);

        public const string TABLENAME = "TKlient";

        public Boolean ThisTableCheck()
        {
            /*  List<Params> list = new List<Params>();
              list.Add(new Params(nameof(ID), SqlDbType.Int, 0));
              list.Add(new Params(nameof(KLINAZ), SqlDbType.VarChar, 50));
              list.Add(new Params(nameof(KLINAZSKROT), SqlDbType.VarChar, 50));
              list.Add(new Params(nameof(KLINIP), SqlDbType.VarChar, 50));
              list.Add(new Params("NAZWISKO", SqlDbType.VarChar, 50));
              list.Add(new Params("DATAW", SqlDbType.DateTime, 0));
              list.Add(new Params("TELEFON", SqlDbType.VarChar, 50));
              list.Add(new Params("DMODDATE", SqlDbType.DateTime, 0));
              list.Add(new Params("ACTIVE", SqlDbType.VarChar, 1));  */

            List<Params> list = new List<Params>();

            FieldInfo[] fields = typeof(CustomerClass).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
    // do something

                Various.InfoOk(nameof(field));
            }

            return TableCheck(TABLENAME, list);
        }

    }

    public class MyField
    {
        public object value;
        public SqlDbType type;
        public int lenght;

        public MyField(object val, SqlDbType sqlt, int len)
        {
            this.value = val;
            this.type = sqlt;
            this.lenght = len;

        }


    }

}

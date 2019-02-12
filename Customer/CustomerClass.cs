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
          public int ID { get; set; }
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
          public int KLIWOJID { get; set; }
          public int KLIKRAJID { get; set; }
          public string KLIUWAGI { get; set; }
          public string KLIPESEL { get; set; }
          public string KLIIMIE { get; set; }
          public string KLINAZWISKO { get; set; }
          public string KLIDOWOD { get; set; }
          public string KLDOWODWYD { get; set; }
          public DateTime KLIDOWODDATA { get; set; }
          public DateTime DATAW { get; set; }
          public DateTime DMODDATE { get; set; }
          public string ACTIVE { get; set; }  


        public const string TABLENAME = "TKlient";

        public Boolean ThisTableCheck()
        {

            List<Params> list = new List<Params>();
            PropertyInfo[] property = typeof(CustomerClass).GetProperties();

            foreach (PropertyInfo prop in property.Where(p => p.MemberType == MemberTypes.Property))
            {

                list.Add(new Params(prop.Name,
                                    GetSqlTypeFromVariable(prop.PropertyType),
                                    GetLengthOfStringField(prop.Name)));               
            }
                  


            return TableCheck(TABLENAME, list);
            
        }
        private int GetLengthOfStringField(string name)
        {
            switch (name)
            {
                case nameof(KLINAZ): return 200;
                case nameof(KLINAZSKROT): return 100;
                case nameof(KLINIP): return 11;
                case nameof(KLIREGON): return 12;
                case nameof(KLITEL1): return 20;
                case nameof(KLITEL2): return 20;
                case nameof(KLIFAX): return 20;
                case nameof(KLIWWW): return 40;
                case nameof(KLIMAIL): return 40;
                case nameof(KLIBANK): return 100;
                case nameof(KLIULICA): return 100;
                case nameof(KLINRDOMU): return 10;
                case nameof(KLINRLOK): return 10;
                case nameof(KLIKOD): return 7;
                case nameof(KLIMIEJSC): return 100;
                case nameof(KLIUWAGI): return 1000;
                case nameof(KLIPESEL): return 11;
                case nameof(KLIIMIE): return 50;
                case nameof(KLINAZWISKO): return 100;
                case nameof(KLIDOWOD): return 10;
                case nameof(KLDOWODWYD): return 100;
                default: return 100;
            }
        }

        /*    private List<CustomerClass> ReadUsers()
            {

                NQueryReader nQ = new NQueryReader("select * from " + TABLENAME);

                List<CustomerClass> U = new List<CustomerClass>();

                while (nQ.NReader.Read())
                {
                    CustomerClass u = new CustomerClass();
                    u.KLINAZ.value = "df";

                    U.Add(u);
                }
            }  */

    }



}

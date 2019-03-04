using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Dictionary
{
    class DictionaryClass :DatabaseConnect
    {
        public int ID { get; set; } 
        public int IUSERID { get; set; }
        public string ACTIVE { get; set; }
        public string SLRODZ { get; set; }
        public string SLKOMUN1 { get; set; }
        public string SLKOMUN2 { get; set; }
        public string SLKOMUN3 { get; set; }
        public string SLKOMUN4 { get; set; }
        public string SLKOMUN5 { get; set; }
        public string SLKOMUN6 { get; set; }
        public string SLKOMUN7 { get; set; }
        public double SLKOMUN8 { get; set; }
        public double SLKOMUN9 { get; set; }
        public int SLKOMUN11 { get; set; }
        public int SLKOMUN12 { get; set; }
        public DateTime SLKOMUN13 { get; set; }
        public DateTime SLKOMUN14 { get; set; }
        public DateTime SLKOMUN15 { get; set; }
        public string SLKOMUN20 { get; set; }
        public string SLKOMUN21 { get; set; }
        public string SLKOMUN22 { get; set; }

        public const string TABLENAME = "TSlownik";

        public override int GetLengthOfStringField(string name)
        {
            throw new NotImplementedException();
        }
    }



}

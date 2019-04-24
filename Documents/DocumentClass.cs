using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Documents
{
    class DocumentClass: DatabaseConnect, IClassControl
    {

        public int ID { get; set; }
        public int IUSERID { get; set; }
        public string ACTIVE { get; set; }
        public int MDKLIID { get; set; }
        public int MDDOKDEFID { get; set; }
        public DateTime MDDATAWYST { get; set; }
        public DateTime MDDATASPRZ { get; set; }
        public int MDDATASPRZRODZ { get; set; }
        public string MDWYSTAWWG { get; set; }
        public DateTime MDWYSTAWWGDAT { get; set; }
        public string MDZAM { get; set; }
        public DateTime MDZAMDAT { get; set; }
        public int MDRODZTRANS { get; set; }
        public int MDRODZPLATID { get; set; }
        public int MDTERMIN { get; set; }
        public DateTime MDTERMINDAT { get; set; }
        public Decimal MDGOTOW { get; set; }
        public string MDWPLACAJACY { get; set; }
        public string MDTYTUL { get; set; }
        public string MDUWAGI { get; set; }
        public string MDODEBRAL { get; set; }
        public string MDSTATUS { get; set; }
        public DateTime DATAW { get; set; }
        public DateTime DMODDATE { get; set; }

        public int GetLengthOfStringField(string name)
        {
            switch (name)
            {
                case nameof(ACTIVE): return 1;
                case nameof(MDGOTOW): return 4;
                case nameof(MDWYSTAWWG): return 100;
                case nameof(MDZAM): return 100;
                case nameof(MDWPLACAJACY): return 100;
                case nameof(MDTYTUL): return 150;
                case nameof(MDUWAGI): return 1000;
                case nameof(MDODEBRAL): return 100;
                case nameof(MDSTATUS): return 1;
                default: return 100;
            }
        }

        public string TableName()
        {
            return "TDokumnety";
        }

        public void ThisReadData()
        {
            throw new NotImplementedException();
        }

        public List<object> ThisReadListData()
        {
            throw new NotImplementedException();
        }

        public bool ThisSaveData()
        {
            throw new NotImplementedException();
        }

        public bool ThisTableCheck()
        {
            return TableCheck(TableName(), typeof(DocumentClass), GetLengthOfStringField);
        }
    }
}

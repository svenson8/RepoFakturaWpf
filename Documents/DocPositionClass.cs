using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Documents
{
    class DocPositionClass : DatabaseConnect, IClassControl
    {
        public int ID { get; set; }
        public int IUSERID { get; set; }
        public string ACTIVE { get; set; }
        public int DOKID { get; set; }
        public int ASSORTID { get; set; }
        public int MPILOSC { get; set; }
        public Decimal MPCENA { get; set; }
        public Decimal MPWARNET { get; set; }
        public Decimal MPWARBR { get; set; }
        public Decimal MPWARVAT { get; set; }
        public Decimal MPWARTOSC { get; set; }
        public int MPSTAWKAVAT { get; set; }
        public DateTime DATAW { get; set; }
        public DateTime DMODDATE { get; set; }

        public int GetLengthOfStringField(string name)
        {
            throw new NotImplementedException();
        }

        public string TableName()
        {
            return "TDokPozycje";
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
            throw new NotImplementedException();
        }
    }
}

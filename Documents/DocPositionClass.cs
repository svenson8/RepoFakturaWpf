using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Documents
{
    public class DocPositionClass : DatabaseConnect, IClassControl
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

        public string MPJM { get; set; }
        public string MPNAZWA { get; set; }
        public int MPLP { get; set; }

        public DocPositionClass(int id = 0)
        {
            this.ID = id;
            if (this.ID > 0)
                ThisReadData();
            else
            {
                this.DATAW = DateTime.Now;
                this.DMODDATE = DateTime.Now;
                this.ACTIVE = "T";
            }
        }

        public int GetLengthOfStringField(string name)
        {
            switch (name)
            {
                case nameof(ACTIVE):    return 1;
                case nameof(MPCENA):    return 4;
                case nameof(MPWARNET):  return 4;
                case nameof(MPWARVAT):  return 4;
                case nameof(MPWARBR):   return 4;
                case nameof(MPWARTOSC): return 4;

                case nameof(MPJM):    return -1;
                case nameof(MPNAZWA): return -1;
                case nameof(MPLP):    return -1;
                default: return 100;
            }
        }

        public string TableName()
        {
            return "TDokPozycje";
        }

        public void ThisReadData()
        {
            ReadData(this, TableName(), this.ID);
        }

        public List<object> ThisReadListData()
        {
            return ReadListData(this, null, new object[] { 0 }, "select d.*, t.ASNAZWA as MPNAZWA , cast(ROW_NUMBER() OVER( ORDER BY d.ID ) AS int) AS MPLP " +
                                                                "from TDokPozycje d left outer join TTowary t on t.ID = d.ASSORTID  where d.DOKID ="+this.DOKID+" order by ID");
        }

        public bool ThisSaveData()
        {
            this.ID = SaveData(this.ID, TableName(), typeof(DocPositionClass), this);
            return (this.ID > 0);
        }

        public bool ThisTableCheck()
        {
            return TableCheck(TableName(), typeof(DocPositionClass), GetLengthOfStringField);
        }
    }
}

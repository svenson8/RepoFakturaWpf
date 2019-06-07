using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakturaWpf.Assortment
{
    class AssortmentClass : DatabaseConnect, IClassControl
    {
        public int ID { get; set; }
        public int IUSERID { get; set; }
        public string ACTIVE { get; set; }
        public string ASKOD { get; set; }
        public int ASNUMER { get; set; }
        public string ASNAZWA { get; set; }
        public int ASTYP { get; set; }
        public string ASINDEX { get; set; }
        public int ASJM { get; set; }
        public string ASPKWIU { get; set; }
        public int ASILOSC { get; set; }
        public Decimal ASNETTO { get; set; }
        public Decimal ASBRUTTO { get; set; }
        public string ASUWAGI { get; set; }
        public byte[] ASIMAGE { get; set; }
        public int ASGRUPAID { get; set; }
        public int ASVAT { get; set; }
        public DateTime DATAW { get; set; }
        public DateTime DMODDATE { get; set; }
        public int ASILOSCWYB { get; set; }
        public Decimal ASNETTOWYB { get; set; }

        public AssortmentClass(int id = 0)
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
                case nameof(ACTIVE): return 1;
                case nameof(ASKOD): return 10;
                case nameof(ASNAZWA): return 300;
                case nameof(ASINDEX): return 30;
                case nameof(ASPKWIU): return 20;
                case nameof(ASUWAGI): return 1000;
                case nameof(ASNETTO): return 4;
                case nameof(ASBRUTTO): return 4;
                case nameof(ASILOSCWYB): return -1;
                case nameof(ASNETTOWYB): return -1;

                default: return 100;
            }
        }

        public string TableName()
        {
            return "TTowary";
        }

        public void ThisReadData()
        {
            ReadData(this, TableName(), this.ID);
        }

        public List<object> ThisReadListData()
        {
            return ReadListData(this, TableName(), new object[] { 0});
        }

        public bool ThisSaveData()
        {
            this.ID = SaveData(this.ID, TableName(), typeof(AssortmentClass), this);
            return (this.ID > 0);
        }

        public bool ThisTableCheck()
        {
            return TableCheck(TableName(), typeof(AssortmentClass), GetLengthOfStringField);
        }
    }
}

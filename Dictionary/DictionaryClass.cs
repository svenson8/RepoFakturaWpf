﻿using FakturaWpf.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakturaWpf.Dictionary
{
    class DictionaryClass : DatabaseConnect, IClassControl
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
        public Decimal SLKOMUN8 { get; set; }
        public Decimal SLKOMUN9 { get; set; }
        public int SLKOMUN11 { get; set; }
        public int SLKOMUN12 { get; set; }
        public DateTime SLKOMUN13 { get; set; }
        public DateTime SLKOMUN14 { get; set; }
        public DateTime SLKOMUN15 { get; set; }
        public string SLKOMUN20 { get; set; }
        public string SLKOMUN21 { get; set; }
        public string SLKOMUN22 { get; set; }
        public DateTime DATAW { get; set; }
        public DateTime DMODDATE { get; set; }

        public const string slRodzGrupK   = "GRUPKON";
        public const string slRodzProv    =  "WOJ";
        public const string slRodzCountry = "KRAJE";

        public DictionaryClass(int id =0, string rodz=null)
        {
            this.SLRODZ = rodz;
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
            if (name.Equals(nameof(ACTIVE)))
                return 1;

            if ((name.Equals("SLKOMUN8") || name.Equals("SLKOMUN9")))
                return 4;

            return 80;
        }

        public List<object> ThisReadListData()
        {
            return ReadListData(this, TableName() + " where SLRODZ =" + Various.QuotedStr(this.SLRODZ), new object[] { 0, "" });
        }

        public bool ThisSaveData()
        {
            this.ID = SaveData(this.ID, TableName() , typeof(DictionaryClass), this);
            return (this.ID > 0);
        }

        public void ThisReadData()
        {
            ReadData(this, TableName(), this.ID);
        }

        public bool ThisTableCheck()
        {
           return TableCheck(TableName(), typeof(DictionaryClass), GetLengthOfStringField);
        }

        public string TableName()
        {
           return "TSlownik";
        }

        public void InsertProvincesFromXML()
        {
            NQueryReader nq = new NQueryReader("select count(ID) from " + TableName() + " where " + nameof(SLRODZ) + " = "+Various.QuotedStr(slRodzProv));

            if (!nq.NCheckCount())
            {
                XmlDocument myxml = new XmlDocument();
                myxml.Load("pack://application:,,,/Provinces.xml");
               


                if (myxml.ChildNodes.Count == 0)
                {
                    Various.InfoOk("nima");
                }
                else
                    Various.InfoOk("jest");
            }
            
        }
    }



}

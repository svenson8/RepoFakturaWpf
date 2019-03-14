using FakturaWpf.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FakturaWpf.Tools
{
    class XmlClass: DatabaseConnect, IClassControl
    {
        public int ID { get; set; }
        public string NAME { get; set; }
        public XmlDocument XML { get; set; }

        public XmlClass()
        {
            this.XML = new XmlDocument();
        }


        public string TableName()
        {
            return "XML_STORE";
        }

        public int GetLengthOfStringField(string name)
        {
            if (name.Equals(nameof(NAME)))
                return 50;

            return 0;
        }

        public List<object> ThisReadListData()
        {
            throw new NotImplementedException();
        }

        public bool ThisSaveData()
        {
            this.ID = SaveData(this.ID, TableName(), typeof(XmlClass), this);
            return (this.ID > 0);
        }

        public void ThisReadData()
        {
            throw new NotImplementedException();
        }

        public bool ThisTableCheck()
        {
            return TableCheck(TableName(), typeof(XmlClass), GetLengthOfStringField);
        }

        public Boolean InsertXmlCountries(string resname)
        {
            NQueryReader nq = new NQueryReader("select count(ID) from " + TableName() + " where " + nameof(NAME) + " = " + Various.QuotedStr(resname));

            if (!nq.NCheckCount())
            {
                this.NAME = resname;               
                this.XML.LoadXml(FakturaWpf.Properties.Resources.ResourceManager.GetString(resname));

                return ThisSaveData();
            }

            return true;
        }
    }
}

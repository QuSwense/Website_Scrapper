using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    [XmlRoot("AppConfig")]
    public class ApplicationConfig
    {
        [XmlElement("Key")]
        public ApplicationConfigKey[] Keys { get; set; }

        public string Key(string name)
        {
            return Keys.Where(item => item.Name == "db").First().Value;
        }

        public string Db()
        {
            return Key("db");
        }

        public bool DoCreateDb()
        {
            return Convert.ToBoolean(Key("dodbcreate"));
        }
    }
}

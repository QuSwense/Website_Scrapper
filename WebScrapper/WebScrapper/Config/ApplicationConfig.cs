using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebScrapper.Config
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
    }
}

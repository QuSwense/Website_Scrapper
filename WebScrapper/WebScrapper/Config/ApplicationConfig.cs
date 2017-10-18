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
        private ApplicationConfigKey[] Keys { get; set; }
    }
}

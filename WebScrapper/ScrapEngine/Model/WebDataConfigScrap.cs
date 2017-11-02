using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class WebDataConfigScrap
    {
        [XmlElement("Scrap")]
        public List<WebDataConfigScrap> Scraps { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("dbtbl")]
        public string DbTable { get; set; }

        [XmlAttribute("xpath")]
        public string XPath { get; set; }

        [XmlIgnore]
        public EWebDataConfigType Type { get; set; }

        [XmlElement("Column")]
        public WebDataConfigColumn[] Columns { get; set; }

        public WebDataConfigScrapState State { get; set; }

        public WebDataConfigScrap Parent { get; set; }
    }
}

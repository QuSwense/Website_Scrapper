using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebScrapper.Web.Config
{
    [Serializable]
    public class WebDataConfigScrap
    {
        [XmlElement("Scrap")]
        public WebDataConfigScrap[] Scraps { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("xpath")]
        public string XPath { get; set; }

        [XmlAttribute("type")]
        public string TypeString
        {
            get
            {
                return Type.ToString();
            }
            set
            {
                Type = (EWebDataConfigType)Enum.Parse(typeof(EWebDataConfigType), value);
            }
        }

        [XmlIgnore]
        public EWebDataConfigType Type { get; set; }

        [XmlElement("Column")]
        public WebDataConfigColumn[] Columns { get; set; }
    }
}

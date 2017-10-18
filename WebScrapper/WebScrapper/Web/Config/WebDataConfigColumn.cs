using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebScrapper.Web.Config
{
    [Serializable]
    public class WebDataConfigColumn
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        [XmlAttribute("ispk")]
        public bool IsPk { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlElement("Column")]
        public WebDataConfigMetadata[] Metadatas { get; set; }
    }
}

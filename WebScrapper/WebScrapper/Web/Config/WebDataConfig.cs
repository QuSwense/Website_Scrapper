using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebScrapper.Web.Config
{
    [Serializable]
    [XmlRoot("WebData")]
    public class WebDataConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Scrap")]
        public WebDataConfigScrap[] Scraps { get; set; }
    }
}

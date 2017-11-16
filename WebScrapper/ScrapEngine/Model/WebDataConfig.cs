using System;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    [XmlRoot("WebData")]
    public class WebDataConfig
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("Scrap")]
        public WebDataConfigScrapHtmlTable[] Scraps { get; set; }
    }
}

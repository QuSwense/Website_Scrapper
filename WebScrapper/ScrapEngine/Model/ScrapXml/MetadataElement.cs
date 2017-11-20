using System;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class WebDataConfigMetadata
    {
        [XmlElement("Split")]
        public SplitElement[] Splits { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class WebDataConfigMetadata
    {
        [XmlElement("Split")]
        public WebDataConfigSplit[] Splits { get; set; }
    }
}

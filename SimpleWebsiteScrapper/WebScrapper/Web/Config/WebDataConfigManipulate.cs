using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebScrapper.Web.Config
{
    [Serializable]
    public class WebDataConfigManipulate
    {
        [XmlElement("Split")]
        public WebDataConfigSplit[] Splits { get; set; }

        [XmlElement("Trim")]
        public WebDataConfigTrim Trim { get; set; }
    }
}

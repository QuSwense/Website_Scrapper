using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class WebDataConfigSplit
    {
        [XmlAttribute("data")]
        public string Data { get; set; }

        [XmlAttribute("index")]
        public int Index { get; set; }
    }
}

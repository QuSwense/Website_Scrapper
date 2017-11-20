using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Model
{
    public class WebScrapParserColumnCsvStateModel
    {
        public int NodeIndex { get; set; }

        public ScrapElement ConfigScrap { get; set; }

        public XmlNode CurrentXmlNode { get; set; }

        public string FileLine { get; set; }
    }
}

using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapManipulateChildConfigParser : AppTopicConfigParser
    {
        public ScrapManipulateChildConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public virtual ManipulateChildElement Process(XmlNode manipulateNode, ManipulateElement webConfigObj) { return null; }
        public virtual void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild) { }
    }
}

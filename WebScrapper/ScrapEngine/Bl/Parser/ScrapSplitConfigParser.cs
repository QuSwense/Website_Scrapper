using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ScrapEngine.Model.ScrapXml;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapSplitConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapSplitConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public override ManipulateChildElement Process(XmlNode childManipulateNode, ManipulateElement webConfigObj)
        {
            return configParser.XmlConfigReader.ReadElement<SplitElement>(childManipulateNode);
        }

        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            SplitElement splitElement = (SplitElement)manipulateChild;
            string[] split = result.Value.Split(splitElement.Data.ToArray());
            result.Value = split[splitElement.Index];
        }
    }
}

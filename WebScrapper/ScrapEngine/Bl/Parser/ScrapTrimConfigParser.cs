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
    public class ScrapTrimConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapTrimConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public override ManipulateChildElement Process(XmlNode trimNode, ManipulateElement webConfigObj)
        {
            TrimElement configTrimObj = configParser.XmlConfigReader.ReadElement<TrimElement>(trimNode);
            configTrimObj.Data = Normalize(configTrimObj.Data);
            return configTrimObj;
        }

        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            TrimElement trimElement = (TrimElement)manipulateChild;
            
            result.Value = result.Value.Trim(trimElement.Data.ToCharArray());
        }
    }
}

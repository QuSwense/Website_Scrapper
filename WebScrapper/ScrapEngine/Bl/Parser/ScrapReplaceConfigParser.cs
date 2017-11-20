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
    public class ScrapReplaceConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapReplaceConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public override ManipulateChildElement Process(XmlNode replaceNode, ManipulateElement webConfigObj)
        {
            ReplaceElement configTrimObj = configParser.XmlConfigReader.ReadElement<ReplaceElement>(replaceNode);
            configTrimObj.InString = Normalize(configTrimObj.InString);
            configTrimObj.OutString = Normalize(configTrimObj.OutString);

            return configTrimObj;
        }
        
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            ReplaceElement replaceConfig = (ReplaceElement)manipulateChild;
            result.Value = result.Value.Replace(replaceConfig.InString, replaceConfig.OutString);
        }
    }
}

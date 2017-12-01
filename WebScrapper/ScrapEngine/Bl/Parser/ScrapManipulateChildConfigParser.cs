using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System.Xml;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapManipulateChildConfigParser : AppTopicConfigParser
    {
        public ScrapManipulateChildConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        public virtual void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild) { }
    }
}

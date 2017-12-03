using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapDbChangeConfigParser : ScrapManipulateChildConfigParser
    {
        private ScrapDbChangeExistsConfigParser scrapDbChangeExistsConfigParser;

        public ScrapDbChangeConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapDbChangeExistsConfigParser = new ScrapDbChangeExistsConfigParser(configParser);
        }

        public override void Process(ManipulateHtmlData manipulateHtml, ManipulateChildElement manipulateChild)
        {
            scrapDbChangeExistsConfigParser.Process(manipulateHtml,
                ((DbchangeElement)manipulateChild).Exists);
        }
    }
}

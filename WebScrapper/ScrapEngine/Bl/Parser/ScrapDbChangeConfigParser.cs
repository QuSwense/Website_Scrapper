using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The parser to process Dbchange elements
    /// </summary>
    public class ScrapDbChangeConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// The Dbchange exists parser
        /// </summary>
        private ScrapDbChangeExistsConfigParser scrapDbChangeExistsConfigParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapDbChangeConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapDbChangeExistsConfigParser = new ScrapDbChangeExistsConfigParser(configParser);
        }

        /// <summary>
        /// Main method to process
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData manipulateHtml, ManipulateChildElement manipulateChild)
        {
            scrapDbChangeExistsConfigParser.Process(manipulateHtml,
                ((DbchangeElement)manipulateChild).Exists);
        }
    }
}

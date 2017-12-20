using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// A parser to parse
    /// </summary>
    public class ScrapXmlConfigParser : ScrapHtmlTableConfigParser
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapXmlConfigParser(WebScrapConfigParser configParser) 
            : base(configParser) { }
    }
}

using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The base class for the scrap xml app topic engine
    /// </summary>
    public class AppTopicConfigParser : IInnerBaseParser
    {
        /// <summary>
        /// The config parser
        /// </summary>
        protected WebScrapConfigParser configParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public AppTopicConfigParser(WebScrapConfigParser configParser)
        {
            this.configParser = configParser;
        }
    }
}

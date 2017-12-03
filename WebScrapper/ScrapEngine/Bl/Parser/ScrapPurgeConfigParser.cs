using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapPurgeConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapPurgeConfigParser(WebScrapConfigParser configParser) 
            : base(configParser) { }

        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public void Process(ManipulateHtmlData manipulateHtml, PurgeElement purgeElement)
        {
            List<string> finalList = new List<string>();

            foreach (string value in manipulateHtml.Results)
            {
                if (purgeElement.IsEmptyOrNull && string.IsNullOrEmpty(value)) ;
                else finalList.Add(value);
            }

            manipulateHtml.Results = new List<string>(finalList);
        }
    }
}

using ScrapEngine.Model;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// A parser class to purge the data as per the Purge element rules
    /// </summary>
    public class ScrapPurgeConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
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
                if (purgeElement.IsEmptyOrNull && string.IsNullOrEmpty(value)) ;//skip
                if (purgeElement.IsWhitespace && string.IsNullOrWhiteSpace(value)) ;//skip
                else finalList.Add(value);
            }

            manipulateHtml.Results = new List<string>(finalList);
        }
    }
}

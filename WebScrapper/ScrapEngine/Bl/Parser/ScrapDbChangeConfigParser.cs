using ScrapEngine.Model;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;

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
        private ScrapDbChangeSelectConfigParser scrapDbChangesSelectConfigParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapDbChangeConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapDbChangesSelectConfigParser = new ScrapDbChangeSelectConfigParser(configParser);
        }

        /// <summary>
        /// Main method to process
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData manipulateHtml, ManipulateChildElement manipulateChild)
        {
            DbchangeElement dbChangeElement = (DbchangeElement)manipulateChild;

            if(string.IsNullOrEmpty(dbChangeElement.IsExistsQuery))
                scrapDbChangesSelectConfigParser.Process(manipulateHtml,
                    ((DbchangeElement)manipulateChild).Select);
            else
            {
                List<string> finalResults = new List<string>();

                for (int i = 0; i < manipulateHtml.Results.Count; i++)
                {
                    string result = manipulateHtml.Results[i];
                    string validatedata = configParser.WebDbContext.ValidateExists(dbChangeElement.IsExistsQuery, result);

                    result = scrapDbChangesSelectConfigParser.Process(validatedata, result, ((DbchangeElement)manipulateChild).Select);

                    finalResults.Add(result);
                }

                manipulateHtml.Results = new List<string>(finalResults);
            }
        }
    }
}

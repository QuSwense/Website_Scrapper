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
    /// The manipulate config parse which processes the "Exists" node of Dbchange.
    /// </summary>
    public class ScrapDbChangeExistsConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// The Select parser
        /// </summary>
        private ScrapDbChangeSelectConfigParser scrapDbChangeSelectConfigParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapDbChangeExistsConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapDbChangeSelectConfigParser = new ScrapDbChangeSelectConfigParser(configParser);
        }

        /// <summary>
        /// Method to process the Exists node
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="existsElement"></param>
        public void Process(ManipulateHtmlData manipulateHtml, DbchangeExistsElement existsElement)
        {
            List<string> finalResults = new List<string>();

            foreach (var result in manipulateHtml.Results)
            {
                int count = configParser.WebDbContext.ValidateExists(existsElement.Table,
                        existsElement.Column, result);

                if (count > 0)
                {
                    string value = scrapDbChangeSelectConfigParser.Process(result, existsElement.Select);
                    finalResults.Add(value);
                }
                else
                    finalResults.Add(result);
            }
        }
    }
}

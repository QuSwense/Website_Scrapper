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
    /// The manipulate child parser which processes the Select node
    /// </summary>
    public class ScrapDbChangeSelectConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapDbChangeSelectConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Process the individual result to modify the data from the database
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="selectElement"></param>
        /// <returns></returns>
        public void Process(ManipulateHtmlData manipulateHtml, DbchangeSelectElement selectElement)
        {
            List<string> finalResults = new List<string>();

            for (int i = 0; i < manipulateHtml.Results.Count; i++)
            {
                string result = manipulateHtml.Results[i];
                result = configParser.WebDbContext.SelectSingle(selectElement.Query, result);
                finalResults.Add(result);
            }

            manipulateHtml.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// Process the individual result to modify the data from the database
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="selectElement"></param>
        /// <returns></returns>
        public string Process(string validatedata, string result, DbchangeSelectElement selectElement)
        {
            if(!string.IsNullOrEmpty(selectElement.Query))
                return configParser.WebDbContext.SelectSingle(selectElement.Query, result);
            else
            {
                if (!selectElement.IsEmptyOrNull && !string.IsNullOrEmpty(result))
                    return validatedata;
            }

            return result;
        }
    }
}

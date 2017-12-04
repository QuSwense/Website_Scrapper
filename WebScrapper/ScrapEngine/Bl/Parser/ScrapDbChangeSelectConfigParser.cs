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
        /// <param name="result"></param>
        /// <param name="selectElement"></param>
        /// <returns></returns>
        public string Process(string result, DbchangeSelectElement selectElement)
        {
            return configParser.WebDbContext.SelectSingle(selectElement.Parent.Table,
                selectElement.Parent.Column, selectElement.Table, selectElement.InnerJoinCriteria,
                        selectElement.Column, result);
        }
    }
}

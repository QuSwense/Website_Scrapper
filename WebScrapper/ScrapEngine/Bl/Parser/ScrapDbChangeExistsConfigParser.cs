using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapDbChangeExistsConfigParser : AppTopicConfigParser
    {
        private ScrapDbChangeSelectConfigParser scrapDbChangeSelectConfigParser;

        public ScrapDbChangeExistsConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapDbChangeSelectConfigParser = new ScrapDbChangeSelectConfigParser(configParser);
        }

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

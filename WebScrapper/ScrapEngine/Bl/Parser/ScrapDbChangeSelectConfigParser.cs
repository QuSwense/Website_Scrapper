using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapDbChangeSelectConfigParser : AppTopicConfigParser
    {
        public ScrapDbChangeSelectConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public string Process(string result, DbchangeSelectElement selectElement)
        {
            return configParser.WebDbContext.SelectSingle(selectElement.Parent.Table,
                selectElement.Parent.Column, selectElement.Table, selectElement.InnerJoinCriteria,
                        selectElement.Column, result);
        }
    }
}

using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main business logic to parse the Xml config file and alongside extract the 
    /// website data as per the attributes.
    /// This extracts data from the Html table
    /// </summary>
    public class ScrapHtmlTableConfigParser : ScrapConfigParser
    {
        /// <summary>
        /// Constructor (no default constructor)
        /// </summary>
        /// <param name="configParser"></param>
        /// <param name="startState"></param>
        public ScrapHtmlTableConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public void Process(XmlNode scrapNode, WebDataConfigScrap parentConfig, HtmlNodeNavigator htmlNode)
        {
            var webScrapConfigObj = 
                ParseScrapElementAttributes<WebDataConfigScrapHtmlTable>(scrapNode, parentConfig, htmlNode);

            // This finally scraps the html webpage data
            var webNodeNavigatorList = FetchHtmlTable(webScrapConfigObj);

            // Process
            int nodeIndex = 0;
            foreach (var webNodeNavigator in webNodeNavigatorList)
            {
                // Read the child Scraps nodes which are the individual reader config nodes
                configParser.ParseChildScrapNodes(scrapNode, parentConfig, webNodeNavigator);

                // Check the constraints on the Scrap nodes
                // 1. Only maximum 4 levels is allowed
                // 2. Only one "name" tag should be present from the top level to bottom Scrap
                //    If multiple "name" tag is present throw error
                AssertLevelConstraint(webScrapConfigObj);
                AssertScrapNameAttribute(webScrapConfigObj);

                // Read the Column nodes which are the individual reader config nodes
                new ScrapColumnConfigParser(configParser).Process(nodeIndex, scrapNode, parentConfig, webNodeNavigator);

                nodeIndex++;
            }
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private List<HtmlNodeNavigator> FetchHtmlTable(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            HtmlNode htmlDoc = configParser.ScrapperCommand.Load(webScrapConfigObj.Url);
            return configParser.ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);
        }
    }
}

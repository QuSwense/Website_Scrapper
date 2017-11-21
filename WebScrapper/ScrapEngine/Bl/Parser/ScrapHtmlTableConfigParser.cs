using HtmlAgilityPack;
using log4net;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The main business logic to parse the Xml config file and alongside extract the 
    /// website data as per the attributes.
    /// This extracts data from the Html table
    /// </summary>
    public class ScrapHtmlTableConfigParser : ScrapConfigParser
    {
        public static ILog logger = LogManager.GetLogger(typeof(ScrapHtmlTableConfigParser));

        /// <summary>
        /// Scrap column config parser
        /// </summary>
        private ScrapColumnConfigParser scrapColumnConfigParser;

        /// <summary>
        /// Constructor (no default constructor)
        /// </summary>
        /// <param name="configParser"></param>
        /// <param name="startState"></param>
        public ScrapHtmlTableConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapColumnConfigParser = new ScrapColumnConfigParser(configParser);
        }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public void Process(XmlNode scrapNode, ScrapElement parentConfig, HtmlNodeNavigator htmlNode)
        {
            logger.Info("Parsing Config ScrapHtmlTable node");

            var webScrapConfigObj = 
                ParseScrapElementAttributes<ScrapHtmlTableElement>(scrapNode, parentConfig, htmlNode);

            // This finally scraps the html webpage data
            var webNodeNavigatorList = FetchHtmlTable(webScrapConfigObj);

            if (webNodeNavigatorList != null && webNodeNavigatorList.Count > 0)
            {
                logger.DebugFormat("{0} html Nodes found which will be parsed in loop.", webNodeNavigatorList.Count);

                // Process
                int nodeIndex = 0;
                foreach (var webNodeNavigator in webNodeNavigatorList)
                {
                    logger.DebugFormat("Parsing Config ScrapHtmlTable {0}th node with data '{1}'", 
                        nodeIndex, webNodeNavigator.Value);

                    // Read the child Scraps nodes which are the individual reader config nodes
                    configParser.ParseChildScrapNodes(scrapNode, webScrapConfigObj, webNodeNavigator);

                    // Check the constraints on the Scrap nodes
                    // 1. Only maximum 4 levels is allowed
                    // 2. Only one "name" tag should be present from the top level to bottom Scrap
                    //    If multiple "name" tag is present throw error
                    AssertLevelConstraint(webScrapConfigObj);
                    AssertScrapNameAttribute(webScrapConfigObj);

                    // Read the Column nodes which are the individual reader config nodes
                    scrapColumnConfigParser.Process(nodeIndex, scrapNode, webScrapConfigObj, webNodeNavigator);

                    nodeIndex++;
                }
            }
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private List<HtmlNodeNavigator> FetchHtmlTable(ScrapHtmlTableElement webScrapConfigObj)
        {
            logger.DebugFormat("Fetch '{0}' from Url '{1}'", webScrapConfigObj.XPath, webScrapConfigObj.Url);

            HtmlNode htmlDoc = configParser.ScrapperCommand.Load(webScrapConfigObj.Url);
            return configParser.ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);
        }
    }
}

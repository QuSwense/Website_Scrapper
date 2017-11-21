using HtmlAgilityPack;
using log4net;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ScrapCsvConfigParser : ScrapConfigParser
    {
        public static ILog logger = LogManager.GetLogger(typeof(ScrapCsvConfigParser));

        /// <summary>
        /// Scrap column config parser
        /// </summary>
        private ScrapColumnConfigParser scrapColumnConfigParser;

        /// <summary>
        /// Constructor (no default constructor)
        /// </summary>
        /// <param name="configParser"></param>
        /// <param name="startState"></param>
        public ScrapCsvConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapColumnConfigParser = new ScrapColumnConfigParser(configParser);
        }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public void Process(XmlNode scrapNode, ScrapElement parentConfig, HtmlNodeNavigator htmlNode)
        {
            logger.Info("Parsing Config ScrapCsv node");

            var webScrapConfigObj = 
                ParseScrapElementAttributes<WebDataConfigScrapCsv>(scrapNode, parentConfig, htmlNode);
            webScrapConfigObj.Delimiter = Normalize(webScrapConfigObj.Delimiter);

            // This finally scraps the html webpage data
            using (StringReader reader = FetchFileReader(webScrapConfigObj))
            {
                int nodeIndex = -1;
                string fileLine = "";
                while ((fileLine = reader.ReadLine()) != null)
                {
                    nodeIndex++;
                    if (webScrapConfigObj.SkipFirstLines > nodeIndex) continue;

                    logger.DebugFormat("Parsing Config ScrapCsv {0}th node with data '{1}'",
                        nodeIndex, fileLine);

                    // Check the constraints on the Scrap nodes
                    // 1. Only maximum 4 levels is allowed
                    // 2. Only one "name" tag should be present from the top level to bottom Scrap
                    //    If multiple "name" tag is present throw error
                    AssertLevelConstraint(webScrapConfigObj);
                    AssertScrapNameAttribute(webScrapConfigObj);

                    // Read the Column nodes which are the individual reader config nodes
                    scrapColumnConfigParser.Process(nodeIndex, 
                        scrapNode, webScrapConfigObj, htmlNode, fileLine);
                }
            }
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private StringReader FetchFileReader(WebDataConfigScrapCsv webScrapConfigObj)
        {
            logger.DebugFormat("Fetch File from Url '{0}'", webScrapConfigObj.Url);

            return new StringReader(configParser.ScrapperCommand.LoadFile(webScrapConfigObj.Url));
        }
    }
}

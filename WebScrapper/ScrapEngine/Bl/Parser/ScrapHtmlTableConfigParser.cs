using HtmlAgilityPack;
using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.Collections.Generic;
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
        /// Stores the current state which is getting processed. Save the State before
        /// sending to process child node
        /// </summary>
        private ScrapIteratorHtmlArgs currentscrapIteratorHtmlArgs;

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
        public override void Process(ScrapIteratorArgs scrapIteratorArgs)
        {
            logger.Info("Parsing Config ScrapHtmlTable node");

            currentscrapIteratorHtmlArgs = scrapIteratorArgs as ScrapIteratorHtmlArgs;

            // This finally scraps the html webpage data
            var webNodeNavigatorList = FetchHtmlTable(currentscrapIteratorHtmlArgs.ScrapConfigObj
                as ScrapHtmlTableElement);

            // Loop through the html nodes
            if (webNodeNavigatorList != null && webNodeNavigatorList.Count > 0)
            {
                logger.DebugFormat("{0} html Nodes found which will be parsed in loop.", webNodeNavigatorList.Count);

                // Process
                //int nodeIndex = 0;
                foreach (var webNodeNavigator in webNodeNavigatorList)
                {
                    ScrapIteratorHtmlArgs childScrapIteratorHtmlArgs = new ScrapIteratorHtmlArgs();
                    childScrapIteratorHtmlArgs.CloneData(scrapIteratorArgs);

                    childScrapIteratorHtmlArgs.NodeIndex++;
                    childScrapIteratorHtmlArgs.WebHtmlNode = webNodeNavigator;
                    childScrapIteratorHtmlArgs.Parent = scrapIteratorArgs;

                    logger.DebugFormat("Parsing Config ScrapHtmlTable {0}th node with data",
                        currentscrapIteratorHtmlArgs.NodeIndex);

                    childScrapIteratorHtmlArgs.ScrapConfigObj.Url = ParseUrlValue(childScrapIteratorHtmlArgs);

                    configParser.StateModel.AddNewScrap(childScrapIteratorHtmlArgs);

                    // Read the child Scraps nodes which are the individual reader config nodes
                    configParser.ParseChildScrapNodes(childScrapIteratorHtmlArgs);

                    // Process column values for this scrapped node
                    configParser.StateModel.AddNewColumn(new ColumnScrapIteratorHtmlArgs()
                    {
                        NodeIndexId = childScrapIteratorHtmlArgs.NodeIndexId,
                        Parent = childScrapIteratorHtmlArgs.ScrapConfigObj,
                        WebHtmlNode = childScrapIteratorHtmlArgs.WebHtmlNode
                    });
                    ProcessColumnParser(configParser.StateModel.CurrentColumnScrapIteratorArgs);
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
            configParser.Performance.NewHtmlLoad(webScrapConfigObj);

            logger.DebugFormat("Fetch '{0}' from Url '{1}'", webScrapConfigObj.XPath, webScrapConfigObj.Url);

            HtmlNode htmlDoc = configParser.ScrapperCommand.Load(webScrapConfigObj.Url);
            List<HtmlNodeNavigator> navigators =
                configParser.ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);

            configParser.Performance.FinalHtmlLoad(webScrapConfigObj.Url);

            return navigators;
        }
        
        public override void Reset()
        {
            currentscrapIteratorHtmlArgs = null;
        }
    }
}

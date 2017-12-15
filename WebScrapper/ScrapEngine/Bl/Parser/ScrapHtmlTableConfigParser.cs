using HtmlAgilityPack;
using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.Collections.Generic;
using System.Web;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The main business logic to parse the Xml config file and alongside extract the 
    /// website data as per the attributes.
    /// This extracts data from the Html table
    /// </summary>
    public class ScrapHtmlTableConfigParser : ScrapConfigParser
    {
        /// <summary>
        /// Logger
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapHtmlTableConfigParser));
        
        /// <summary>
        /// Stores the current state which is getting processed. Save the State before
        /// sending to process child node
        /// </summary>
        private ScrapIteratorHtmlArgs currentscrapIteratorHtmlArgs
        {
            get
            {
                return (ScrapIteratorHtmlArgs)(configParser.StateModel.CurrentScrapIteratorArgs);
            }
        }

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
        public override void Process()
        {
            logger.Info("Parsing Config ScrapHtmlTable node");

            ParseUrlValue(currentscrapIteratorHtmlArgs);

            logger.DebugFormat("The Calculated Url {0}", currentscrapIteratorHtmlArgs.ScrapConfigObj.UrlCalculated);

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
                    currentscrapIteratorHtmlArgs.NodeIndex++;
                    currentscrapIteratorHtmlArgs.WebHtmlNode = webNodeNavigator;

                    logger.DebugFormat("Parsing Config ScrapHtmlTable {0}th node with data",
                        currentscrapIteratorHtmlArgs.NodeIndex);
                    
                    // Read the child Scraps nodes which are the individual reader config nodes
                    configParser.ParseChildScrapNodes();

                    // Push the stacked column iterator
                    configParser.StateModel.AddNewColumn(new ColumnScrapIteratorHtmlArgs()
                    {
                        NodeIndexId = currentscrapIteratorHtmlArgs.NodeIndexId,
                        Parent = currentscrapIteratorHtmlArgs,
                        WebHtmlNode = currentscrapIteratorHtmlArgs.WebHtmlNode
                    });

                    // Process column values for this scrapped node
                    ProcessColumnParser();

                    // Pop the Stacked column iterator
                    configParser.StateModel.RestoreColumn();
                }
            }

            currentscrapIteratorHtmlArgs.ScrapConfigObj.UrlCalculated = null;
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private List<HtmlNodeNavigator> FetchHtmlTable(ScrapHtmlTableElement webScrapConfigObj)
        {
            configParser.Performance.NewHtmlLoad(webScrapConfigObj);

            logger.DebugFormat("Fetch '{0}' from Url '{1}'", webScrapConfigObj.XPath, 
                webScrapConfigObj.Url);

            var htmlDoc = configParser.ScrapperCommand.Load(webScrapConfigObj.Url);
            var navigators =
                configParser.ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);

            configParser.Performance.FinalHtmlLoad(webScrapConfigObj.Url);

            return navigators;
        }
    }
}

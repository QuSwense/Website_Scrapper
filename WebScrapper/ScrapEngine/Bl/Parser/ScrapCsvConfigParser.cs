using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using ScrapEngine.Model.Scrap;
using System.IO;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The main business logic to parse the Xml config file and alongside extract the 
    /// website data as per the attributes.
    /// This extracts data from the Html table
    /// </summary>
    public class ScrapCsvConfigParser : ScrapConfigParser
    {
        /// <summary>
        /// The logging context
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapCsvConfigParser));

        /// <summary>
        /// Current Csv iterator scrap
        /// </summary>
        private ScrapIteratorArgs currentscrapIteratorCsvArgs
        {
            get
            {
                return configParser.StateModel.CurrentScrapIteratorArgs;
            }
        }

        public ScrapCsvConfigParser() { }

        /// <summary>
        /// Constructor (no default constructor)
        /// </summary>
        /// <param name="configParser"></param>
        /// <param name="startState"></param>
        public ScrapCsvConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public override void Process()
        {
            logger.Info("Parsing Config ScrapCsv node");
            
            ParseUrlValue(currentscrapIteratorCsvArgs);

            // This finally scraps the html webpage data
            using (StringReader reader = FetchFileReader(currentscrapIteratorCsvArgs))
            {
                //int nodeIndex = -1;
                string fileLine = "";
                while ((fileLine = reader.ReadLine()) != null)
                {
                    currentscrapIteratorCsvArgs.NodeIndex++;
                    if (((ScrapCsvElement)currentscrapIteratorCsvArgs.ScrapConfigObj).SkipFirstLines >
                        currentscrapIteratorCsvArgs.NodeIndex) continue;
                    
                    logger.DebugFormat("Parsing Config ScrapCsv {0}th node with data '{1}'",
                        currentscrapIteratorCsvArgs.NodeIndex, fileLine);
                    
                    // Read the child Scraps nodes which are the individual reader config nodes
                    configParser.ParseChildScrapNodes();

                    // Process column values for this scrapped node
                    configParser.StateModel.AddNewColumn(new ColumnScrapIteratorFileArgs()
                    {
                        NodeIndexId = currentscrapIteratorCsvArgs.NodeIndexId,
                        FileLine = fileLine,
                        Parent = currentscrapIteratorCsvArgs
                    });
                    ProcessColumnParser();
                    configParser.StateModel.RestoreColumn();
                }
            }

            currentscrapIteratorCsvArgs.ScrapConfigObj.UrlCalculated = null;
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private StringReader FetchFileReader(ScrapIteratorArgs scrapIteratorCsvArgs)
        {
            configParser.Performance.NewHtmlLoad(scrapIteratorCsvArgs.ScrapConfigObj);

            logger.DebugFormat("Fetch File from Url '{0}'", scrapIteratorCsvArgs.ScrapConfigObj.Url);

            StringReader stringReader = new StringReader(
                configParser.ScrapperCommand.LoadFile(scrapIteratorCsvArgs.ScrapConfigObj.Url));

            configParser.Performance.FinalHtmlLoad(scrapIteratorCsvArgs.ScrapConfigObj.Url);

            return stringReader;
        }
    }
}

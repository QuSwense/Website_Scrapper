using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System.IO;
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
        /// <summary>
        /// The logging context
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapCsvConfigParser));

        /// <summary>
        /// Current Csv iterator scrap
        /// </summary>
        private ScrapIteratorCsvArgs currentscrapIteratorCsvArgs;

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
        public override void Process(ScrapIteratorArgs scrapIteratorArgs)
        {
            logger.Info("Parsing Config ScrapCsv node");

            currentscrapIteratorCsvArgs = scrapIteratorArgs as ScrapIteratorCsvArgs;

            // This finally scraps the html webpage data
            using (StringReader reader = FetchFileReader(currentscrapIteratorCsvArgs))
            {
                //int nodeIndex = -1;
                string fileLine = "";
                while ((fileLine = reader.ReadLine()) != null)
                {
                    ScrapIteratorCsvArgs childScrapIteratorCsvArgs = new ScrapIteratorCsvArgs();
                    childScrapIteratorCsvArgs.CloneData(scrapIteratorArgs);

                    childScrapIteratorCsvArgs.NodeIndex++;
                    if (((ScrapCsvElement)childScrapIteratorCsvArgs.ScrapConfigObj).SkipFirstLines >
                        childScrapIteratorCsvArgs.NodeIndex) continue;

                    childScrapIteratorCsvArgs.Parent = scrapIteratorArgs;

                    childScrapIteratorCsvArgs.ScrapConfigObj.Url = ParseUrlValue(childScrapIteratorCsvArgs);

                    logger.DebugFormat("Parsing Config ScrapCsv {0}th node with data '{1}'",
                        childScrapIteratorCsvArgs.NodeIndex, fileLine);

                    // Set current state
                    configParser.StateModel.AddNewScrap(childScrapIteratorCsvArgs);

                    // Read the child Scraps nodes which are the individual reader config nodes
                    configParser.ParseChildScrapNodes(childScrapIteratorCsvArgs);

                    // Process column values for this scrapped node
                    configParser.StateModel.AddNewColumn(new ColumnScrapIteratorFileArgs()
                    {
                        NodeIndexId = childScrapIteratorCsvArgs.NodeIndexId,
                        FileLine = fileLine,
                        Parent = childScrapIteratorCsvArgs.ScrapConfigObj
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
        private StringReader FetchFileReader(ScrapIteratorCsvArgs scrapIteratorCsvArgs)
        {
            configParser.Performance.NewHtmlLoad(scrapIteratorCsvArgs.ScrapConfigObj);

            logger.DebugFormat("Fetch File from Url '{0}'", scrapIteratorCsvArgs.ScrapConfigObj.Url);

            StringReader stringReader = new StringReader(configParser.ScrapperCommand.LoadFile(scrapIteratorCsvArgs.ScrapConfigObj.Url));

            configParser.Performance.FinalHtmlLoad(scrapIteratorCsvArgs.ScrapConfigObj.Url);

            return stringReader;
        }

        public override void Reset()
        {
            currentscrapIteratorCsvArgs = null;
        }
    }
}

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
        private ScrapIteratorCsvArgs currentScrapIteratorCsv;

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
        public override void Process(ScrapIteratorArgs args)
        {
            logger.Info("Parsing Config ScrapCsv node");

            var webScrapConfigObj = 
                ParseScrapElementAttributes<ScrapCsvElement>(args);
            webScrapConfigObj.Delimiter = Normalize(webScrapConfigObj.Delimiter);

            // This finally scraps the html webpage data
            using (StringReader reader = FetchFileReader(webScrapConfigObj))
            {
                //int nodeIndex = -1;
                string fileLine = "";
                while ((fileLine = reader.ReadLine()) != null)
                {
                    args.NodeIndex++;
                    if (webScrapConfigObj.SkipFirstLines > args.NodeIndex) continue;

                    logger.DebugFormat("Parsing Config ScrapCsv {0}th node with data '{1}'",
                        args.NodeIndex, fileLine);

                    // Set current state
                    SetCurrentState(args, new ScrapIteratorCsvArgs()
                    {
                        NodeIndex = args.NodeIndex,
                        ScrapConfigNode = args.ScrapConfigNode,
                        ScrapConfigObj = webScrapConfigObj,
                        WebHtmlNode = args.WebHtmlNode
                    }, ref currentScrapIteratorCsv);

                    // Process column values
                    ProcessColumnParser(new ColumnScrapIteratorFileArgs()
                    {
                        FileLine = fileLine,
                        NodeIndexId = currentScrapIteratorCsv.NodeIndexId,
                        ScrapConfig = currentScrapIteratorCsv.ScrapConfigObj,
                        ScrapNode = args.ScrapConfigNode
                    });
                }
            }
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private StringReader FetchFileReader(ScrapCsvElement webScrapConfigObj)
        {
            configParser.Performance.NewHtmlLoad(webScrapConfigObj);

            logger.DebugFormat("Fetch File from Url '{0}'", webScrapConfigObj.Url);

            StringReader stringReader = new StringReader(configParser.ScrapperCommand.LoadFile(webScrapConfigObj.Url));

            configParser.Performance.FinalHtmlLoad(webScrapConfigObj.Url);

            return stringReader;
        }

        /// <summary>
        /// Create new args to pass to Process method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override ScrapIteratorArgs CreateArgs(ScrapIteratorArgs args, XmlNode nextChildNode)
        {
            return new ScrapIteratorCsvArgs()
            {
                ScrapConfigNode = nextChildNode,
                ScrapConfigObj = args.ScrapConfigObj,
                WebHtmlNode = args.WebHtmlNode
            };
        }
    }
}

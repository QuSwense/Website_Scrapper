using log4net;
using Newtonsoft.Json;
using SimpleWebsiteScrapper.ParseTree;
using System.Linq;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// The engine class to scrap data for FIFA code details for Countries of the world from 
    /// Wikipedia
    /// </summary>
    public class WikipediaFIFACodeEngine : WebScrapperEngine
    {
        /// <summary>
        /// The logger for the class
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(WikipediaFIFACodeEngine));

        /// <summary>
        /// Default constructor
        /// </summary>
        public WikipediaFIFACodeEngine() : base("FIFAListOfCodes")
        {
            logger.Info("Initializing Wikipedia FIFA Code scrapping engine");

            RootNode.References.AddItem().AddXPath(EHtmlNodeType.Single, "//div[@class='mw-parser-output']/p[position() = 1]/text()[1]");
            RootNode.AddUrl("https://en.wikipedia.org/wiki/List_of_FIFA_country_codes")
                .AddXPath(EHtmlNodeType.Collection, "//div[@id='mw-content-text']/div[@class='mw-parser-output']");

            AddChildNodes();
        }

        private void AddChildNodes()
        {
            logger.Debug("Adding Child nodes for Wikipedia FIFA Code");

            AddChildNode("MemberCodes", 1, 4, false);
            AddChildNode("NonMemberCodes", 2, 5);
            AddChildNode("IrregularCodes", 3, 6);
            
            if(logger.IsDebugEnabled)
            {
                logger.Debug("WikipediaFIFACodeEngine object node is as follows:");
                logger.Debug(JsonConvert.SerializeObject(RootNode));
            }
        }

        /// <summary>
        /// A template to create common child nodes
        /// </summary>
        /// <param name="name">The name of the node</param>
        /// <param name="xpathIndx">The XPath Fetching index id of table</param>
        /// <param name="refIndx">The reference Index</param>
        /// <param name="addConfederation">Add the child node confederation node</param>
        /// <returns></returns>
        private ScrapWebpageProcessorNode AddChildNode(string name, int xpathIndx, int refIndx, bool addConfederation = true)
        {
            ScrapWebpageProcessorNode fifaCode = RootNode.Nodes.AddItem(name);

            fifaCode.Nodes.AddItem("Name").AddXPath(ENodeType.Single, "td[position() = 1]/span/a");
            fifaCode.Nodes.AddItem("NationalTeamUrl").AddXPath(ENodeType.Single, "td[position() = 1]/span/a", "href");
            fifaCode.Nodes.AddItem("Code").AddXPath(ENodeType.Single, "td[position() = 1]");
            if(addConfederation) fifaCode.Nodes.AddItem("Code").AddXPath(ENodeType.Single, "td[position() = 3]");

            fifaCode.AddXPath(ENodeType.Collection, 
                string.Format(".//table[position() = {0}]//table[@class='wikitable']//tr[position() > 1]", xpathIndx));
            fifaCode.References.AddItem().AddXPath(ENodeType.Single,
                string.Format("//div[@class='mw-parser-output']/p[position() = {0}]/text()[1]", refIndx));

            return fifaCode;
        }
    }
}

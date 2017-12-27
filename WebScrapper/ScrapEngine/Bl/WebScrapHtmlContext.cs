using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapException;
using System.Collections.Generic;
using WebReader.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main web site scrapper context class
    /// </summary>
    public class WebScrapHtmlContext : IScrapHtmlContext
    {
        #region Properties

        /// <summary>
        /// Reference to the Scrapper Engine context class
        /// </summary>
        public IScrapEngineContext EngineContext { get; protected set; }

        /// <summary>
        /// The Html helper command class
        /// </summary>
        private HtmlScrapperCommand ScrapperCommand;

        /// <summary>
        /// The main parser
        /// </summary>
        private WebScrapConfigParser WebScrapParser;

        /// <summary>
        /// Parse scrap config xml file
        /// </summary>
        private DXmlReader<WebDataElement> configXmlParser;

        #endregion Properties

        #region Properties Helper

        /// <summary>
        /// The main web database conetxt for website scrapping
        /// </summary>
        public IScrapDbContext WebDbContext
        {
            get { return EngineContext.WebDbContext; }
        }

        /// <summary>
        /// The main web database conetxt for website scrapping
        /// </summary>
        public ApplicationConfig AppConfig
        {
            get { return EngineContext.AppConfig; }
        }

        /// <summary>
        /// The core database context
        /// </summary>
        public SqliteDatabase.DatabaseContext WebScrapDb
        {
            get { return WebDbContext.WebScrapDb; }
        }

        public List<ScrapElement> RootScrapNodes
        {
            get { return configXmlParser.Root.Scraps; }
        }

        #endregion Properties Helper

        /// <summary>
        /// Constructor
        /// </summary>
        public WebScrapHtmlContext() { }

        /// <summary>
        /// Constructor initializes with parent engine
        /// </summary>
        /// <param name="engineContext"></param>
        public void Initialize(IScrapEngineContext engineContext)
        {
            EngineContext = engineContext;

            ScrapperCommand = new HtmlScrapperCommand();

            // Load the scrap xml config
            EngineContext.AppTopicPath.AppTopicScrap.AssertExists();
            configXmlParser = new DXmlReader<WebDataElement>();
            configXmlParser.Read(EngineContext.AppTopicPath.AppTopicScrap.FullPath);
            AssertScrapXml();

            WebScrapParser = new WebScrapConfigParser();
            WebScrapParser.Initialize(this);
        }

        /// <summary>
        /// Assert config
        /// </summary>
        public void AssertScrapXml()
        {
            foreach (var scrapNode in RootScrapNodes)
            {
                AssertLevelConstraint(scrapNode);
                AssertScrapNameAttribute(scrapNode);
            }
        }

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        protected void AssertLevelConstraint(ScrapElement scrapObj)
        {
            ScrapElement tmpObj = scrapObj;
            int level = 0;
            for (; tmpObj != null && level <= AppConfig.ScrapMaxLevel();
                level++, tmpObj = tmpObj.Parent) ;

            if (level > AppConfig.ScrapMaxLevel() || level <= 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
                    level.ToString());
        }

        /// <summary>
        /// The Scrap element tag (and its child Scrap tags) should contain one and only one name 
        /// attribute
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        protected void AssertScrapNameAttribute(ScrapElement scrapObj)
        {
            bool isTableNameFound = false;
            string NameValue = null;
            ScrapElement tmpObj = scrapObj;

            while (tmpObj != null)
            {
                if (!string.IsNullOrEmpty(tmpObj.Name))
                {
                    if (isTableNameFound)
                        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_MULTIPLE);
                    isTableNameFound = true;
                    NameValue = tmpObj.Name;
                }

                tmpObj = tmpObj.Parent;
            }

            if (!isTableNameFound || string.IsNullOrEmpty(NameValue))
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_EMPTY);
        }

        /// <summary>
        /// Execute
        /// </summary>
        public void Run()
        {
            WebScrapDb.Open();

            try
            {
                // Loop through the instances of table to be modified
                WebScrapParser.Run();
            }
            finally
            {
                WebScrapDb.Close();
            }
        }
    }
}

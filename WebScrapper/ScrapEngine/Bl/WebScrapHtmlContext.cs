using ScrapEngine.Bl.Parser;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System.Collections.Generic;

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
        private WebDataScrapConfigXmlParser configXmlParser;

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
        /// The core database context
        /// </summary>
        public SqliteDatabase.DatabaseContext WebScrapDb
        {
            get { return WebDbContext.WebScrapDb; }
        }

        public List<ScrapElement> RootScrapNodes
        {
            get { return configXmlParser.RootScrapNodes; }
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
            configXmlParser = new WebDataScrapConfigXmlParser();
            configXmlParser.Parse(EngineContext.AppTopicPath.AppTopicScrap.FullPath);

            WebScrapParser = new WebScrapConfigParser();
            WebScrapParser.Initialize(this);
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

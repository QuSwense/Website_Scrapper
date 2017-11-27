using ScrapEngine.Interfaces;

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

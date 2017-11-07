using ScrapEngine.Interfaces;
using ScrapEngine.Model;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main web site scrapper context class
    /// </summary>
    public class WebScrapHtmlContext
    {
        #region Properties

        /// <summary>
        /// Reference to the Scrapper Engine context class
        /// </summary>
        public IScrapEngineContext EngineContext { get; protected set; }

        /// <summary>
        /// The web data rules configuration
        /// </summary>
        public WebDataConfig ScrapperRulesConfig { get; protected set; }

        /// <summary>
        /// The Html helper command class
        /// </summary>
        private HtmlScrapperCommand ScrapperCommand;

        /// <summary>
        /// The main parser
        /// </summary>
        private WebScrapConfigParser WebScrapParser;

        #endregion Properties

        #region Constructor

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
        /// Static initialize
        /// </summary>
        /// <param name="engineContext"></param>
        /// <returns></returns>
        public static WebScrapHtmlContext Init(IScrapEngineContext engineContext)
        {
            WebScrapHtmlContext webScrapHtmlContext = new WebScrapHtmlContext();
            webScrapHtmlContext.Initialize(engineContext);
            return webScrapHtmlContext;
        }

        #endregion Constructor

        /// <summary>
        /// Execute
        /// </summary>
        public void Run()
        {
            EngineContext.WebDbContext.WebScrapDb.Open();

            try
            {
                // Loop through the instances of table to be modified
                WebScrapParser.Run();
            }
            finally
            {
                EngineContext.WebDbContext.WebScrapDb.Close();
            }
        }
    }
}

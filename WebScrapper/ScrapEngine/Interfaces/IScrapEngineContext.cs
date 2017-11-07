using ScrapEngine.Bl;
using ScrapEngine.Db;
using ScrapEngine.Model;
using WebCommon.PathHelp;

namespace ScrapEngine.Interfaces
{
    public interface IScrapEngineContext
    {
        #region Properties

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        AppTopicConfigPathHelper AppTopicPath { get; }

        /// <summary>
        /// Application config
        /// </summary>
        ApplicationConfig AppConfig { get; }

        /// <summary>
        /// The Database class layer
        /// </summary>
        IScrapDbContext WebDbContext { get; }

        /// <summary>
        /// Web scrapper html context
        /// </summary>
        WebScrapHtmlContext WebScrapHtml { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initialize the engine context
        /// </summary>
        /// <param name="appTopic"></param>
        /// <param name="sqldb"></param>
        void Initialize(AppTopicConfigPathHelper appTopicPath, ApplicationConfig appGenericConfig);

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        void Run();

        #endregion Methods
    }
}

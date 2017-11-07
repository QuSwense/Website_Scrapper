using DynamicDatabase.Interfaces;
using ScrapEngine.Db;

namespace ScrapEngine.Interfaces
{
    /// <summary>
    /// An interface to define application topic specific database context
    /// </summary>
    public interface IScrapDbContext
    {
        #region Properties

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        IScrapEngineContext ParentEngine { get; }

        /// <summary>
        /// Read the Database configuration
        /// </summary>
        DynamicAppDbConfig MetaDbConfig { get; }

        /// <summary>
        /// The configuration for web scrapping
        /// </summary>
        IDbContext WebScrapDb { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize(IScrapEngineContext parent);

        #endregion Constructor
    }
}

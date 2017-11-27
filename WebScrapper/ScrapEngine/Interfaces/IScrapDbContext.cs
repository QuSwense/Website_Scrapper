using ScrapEngine.Db;
using ScrapEngine.Model;
using SqliteDatabase;
using SqliteDatabase.Model;
using System.Collections.Generic;
using ScrapEngine.Bl;

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
        DatabaseContext WebScrapDb { get; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initialize
        /// </summary>
        void Initialize(IScrapEngineContext parent);

        #endregion Constructor

        /// <summary>
        /// Add or update the data scrapped from the webpages including the metadata information
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="row"></param>
        void AddOrUpdate(ScrapElement scrapConfig, List<DynamicTableDataInsertModel> row);

        /// <summary>
        /// To Add all combination of column values
        /// </summary>
        /// <param name="scrapConfig"></param>
        /// <param name="rows"></param>
        void AddOrUpdate(ScrapElement scrapConfig, List<List<DynamicTableDataInsertModel>> rows);

        /// <summary>
        /// Load partial
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        void AddMetadata(ScrapElement webScrapConfigObj);

        /// <summary>
        /// Add performance metadata
        /// </summary>
        /// <param name="performance"></param>
        //void Add(PerformanceMeasure performance);

        void AddMetadata(string id, PerformanceMeasure performanceMeasure);
    }
}

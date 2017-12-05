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

        /// <summary>
        /// Validate if the data exists in the table column
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int ValidateExists(string table, string column, string value);

        /// <summary>
        /// Validate if the data exists using the custom database query
        /// </summary>
        /// <param name="queryFormat"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string ValidateExists(string queryFormat, string value);

        /// <summary>
        /// Select a single value from the database using the query format
        /// </summary>
        /// <param name="queryFormat"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        string SelectSingle(string queryFormat, string result);
    }
}

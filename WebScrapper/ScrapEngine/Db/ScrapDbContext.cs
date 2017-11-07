using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using ScrapEngine.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ScrapEngine.Db
{
    /// <summary>
    /// A class used to help in forming a wrapper between the actual Database activity and the 
    /// </summary>
    public class ScrapDbContext : IScrapDbContext
    {
        #region Properties

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public IScrapEngineContext ParentEngine { get; protected set; }

        /// <summary>
        /// Read the Database configuration
        /// </summary>
        public DynamicAppDbConfig MetaDbConfig { get; protected set; }

        /// <summary>
        /// The configuration for web scrapping
        /// </summary>
        public IDbContext WebScrapDb { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapDbContext() { }

        /// <summary>
        /// A static initialize method
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static IScrapDbContext Init(IScrapEngineContext parent)
        {
            IScrapDbContext dbContext = new ScrapDbContext();
            dbContext.Initialize(parent);
            return dbContext;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize(IScrapEngineContext parent)
        {
            ParentEngine = parent;

            // Read aplication specific database config
            MetaDbConfig = DynamicAppDbConfig.Init(this);
            MetaDbConfig.Read();

            // Create database context
            InitializeDbContext();
        }

        /// <summary>
        /// Initialize database context
        /// </summary>
        private void InitializeDbContext()
        {
            ArgsContextInitialize dbContextArgs = new ArgsContextInitialize();
            dbContextArgs.DbFilePath = ParentEngine.AppTopicPath.AppTopicMain.FullPath;
            dbContextArgs.Name = ParentEngine.AppTopicPath.AppTopic;

            WebScrapDb = DynamicDbContext.Init(dbContextArgs, ParentEngine.AppConfig.Db());

            if (ParentEngine.AppConfig.DoCreateDb())
                WebScrapDb.CreateDatabase();

            // Create database
            CreateDbContextTables();
        }

        #endregion Constructor

        #region Create

        /// <summary>
        /// Create all the application specific tables
        /// </summary>
        private void CreateDbContextTables()
        {
            WebScrapDb.Open();

            try
            {
                // Create metadata table
                CreateTableMetadata();

                // Create each table column metadata
                CreateTableColumnMetadata();

                // Create each table column rows metadata
                CreateTableColumnRowsMetadata();
            }
            finally
            {
                // Commit all the data in one go
                WebScrapDb.Commit();
                WebScrapDb.Close();
            }
        }

        /// <summary>
        /// Create table which contains metadata information about all tables
        /// </summary>
        private void CreateTableMetadata()
        {
            // Call the dynamic database context to create the table
            WebScrapDb.CreateTable(ParentEngine.GenericDbConfig.TableMetadataConfigs);

            // Add data
            WebScrapDb.AddOrUpdate(ParentEngine.GenericDbConfig.MetadataTableName,
                MetaDbConfig.TableMetadatas);

            // Create all the data specific tables
            WebScrapDb.CreateTable(MetaDbConfig.TableColumnConfigs);
        }

        /// <summary>
        /// Create and update table column metadata
        /// </summary>
        private void CreateTableColumnMetadata()
        {
            // Create tables
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                string tableName = string.Format(
                    ParentEngine.GenericDbConfig.ColumnMetadataTableName, item.Key);

                WebScrapDb.CreateTable(tableName,
                    ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Values.First());
            }

            // Add column data that is available
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                string tableName = string.Format(
                    ParentEngine.GenericDbConfig.ColumnMetadataTableName, item.Key);

                foreach (var coldata in item.Value)
                {
                    WebScrapDb.AddOrUpdate(tableName, new string[] { coldata.Key, coldata.Value.Display });
                }
            }
        }

        /// <summary>
        /// Create table rows metadata
        /// </summary>
        private void CreateTableColumnRowsMetadata()
        {
            foreach (var tableconfig in MetaDbConfig.TableColumnConfigs)
            {
                foreach (var tablecolconfig in tableconfig.Value)
                {
                    string tableName = string.Format(
                        ParentEngine.GenericDbConfig.RowMetadataTableName,
                        tableconfig.Key, tablecolconfig.Key);

                    WebScrapDb.CreateTable(tableName, ParentEngine.GenericDbConfig.TableColumnRowMetadataConfigs.Values.First());
                }
            }
        }

        /// <summary>
        /// Add or update the data scrapped from the webpages including the metadata information
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="row"></param>
        public void AddOrUpdate(string tableName, List<TableDataColumnModel> row)
        {
            string colMetadatatableName = string.Format(
                    ParentEngine.GenericDbConfig.ColumnMetadataTableName, tableName);

            List<string> ukeys = new List<string>();
            Dictionary<string, string> dataList = new Dictionary<string, string>();

            foreach (var col in row)
            {
                if (col.IsPk)
                    ukeys.Add(col.Name);
                else
                    dataList.Add(col.Name, col.Value);
            }

            // Add data
            string pk = WebScrapDb.AddOrUpdate(tableName, ukeys, dataList);

            foreach (var colDataKv in row)
            {
                // Add metadata
                WebScrapDb.AddOrUpdate(colMetadatatableName, new string[] { pk, null, null, colDataKv.Url, colDataKv.XPath });
            }
        }

        #endregion Create
    }
}

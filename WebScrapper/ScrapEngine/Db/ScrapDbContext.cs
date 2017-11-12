using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using ScrapEngine.Interfaces;
using System.Linq;
using System;
using System.Collections.Generic;
using ScrapEngine.Model;

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
        /// Default Constructor
        /// </summary>
        public ScrapDbContext() { }

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
            WebScrapDb = new DynamicDbContext();

            ArgsContextInitialize dbContextArgs = new ArgsContextInitialize();
            dbContextArgs.DbFilePath = ParentEngine.AppTopicPath.AppTopicMain.FullPath;
            dbContextArgs.Name = ParentEngine.AppTopicPath.AppTopic;
            dbContextArgs.DbType = ParentEngine.AppConfig.Db();

            WebScrapDb.Initialize(dbContextArgs);

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
                WebScrapDb.Clear();
                WebScrapDb.Close();
            }
        }

        /// <summary>
        /// Create table which contains metadata information about all tables
        /// </summary>
        private void CreateTableMetadata()
        {
            // Call the dynamic database context to create the table
            WebScrapDb.CreateTable(DynamicGenericDbConfig.I.TableMetadataConfigs);

            // Add data
            WebScrapDb.AddOrUpdate(DynamicGenericDbConfig.I.MetadataTableName,
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
                    DynamicGenericDbConfig.I.ColumnMetadataTableName, item.Key);

                WebScrapDb.CreateTable(tableName,
                    DynamicGenericDbConfig.I.TableColumnMetadataConfigs.Values.First());
            }

            // Add column data that is available
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                string tableName = string.Format(
                    DynamicGenericDbConfig.I.ColumnMetadataTableName, item.Key);

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
                        DynamicGenericDbConfig.I.RowMetadataTableName,
                        tableconfig.Key, tablecolconfig.Key);

                    WebScrapDb.CreateTable(tableName, DynamicGenericDbConfig.I.TableColumnRowMetadataConfigs.Values.First());
                }
            }
        }

        /// <summary>
        /// Add or update the data scrapped from the webpages including the metadata information
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="row"></param>
        public void AddOrUpdate(string tableName, List<TableDataColumnModel> row, EWebDataConfigType scrapType)
        {
            // Add data
            string pk = WebScrapDb.AddOrUpdate(tableName, row.Select(p => p.Name).ToArray());   

            foreach (var colDataKv in row)
            {
                string rowMetadatatableName = string.Format(
                    DynamicGenericDbConfig.I.RowMetadataTableName, tableName, colDataKv.Name);
                // Add metadata
                WebScrapDb.AddOrUpdate(rowMetadatatableName, new string[] { pk, null, scrapType.ToString(), colDataKv.Url, colDataKv.XPath });
            }
        }

        /// <summary>
        /// LOad table with partial columns
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        public void LoadPartial(WebDataConfigScrap webScrapConfigObj)
        {
            var columns = new Dictionary<string, ColumnLoadDataModel>();

            foreach (var colObj in webScrapConfigObj.Columns)
            {
                ColumnLoadDataModel loadModelObj = new ColumnLoadDataModel();
                loadModelObj.Name = colObj.Name;
                loadModelObj.IsUnique = colObj.IsPk;
                columns.Add(colObj.Name, loadModelObj);
            }

            string tableName = "";
            while(string.IsNullOrEmpty(webScrapConfigObj.Name))
            {
                webScrapConfigObj = webScrapConfigObj.Parent;
            }

            tableName = webScrapConfigObj.Name;
            WebScrapDb.LoadPartial(tableName, columns);
        }

        #endregion Create
    }
}

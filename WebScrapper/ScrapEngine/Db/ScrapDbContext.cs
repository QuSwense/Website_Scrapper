using DynamicDatabase;
using DynamicDatabase.Config;
using DynamicDatabase.Interfaces;
using DynamicDatabase.Model;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCommon.Config;

namespace ScrapEngine.Db
{
    /// <summary>
    /// A class used to help in forming a wrapper between the actual Database activity and the 
    /// </summary>
    public class ScrapDbContext
    {
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

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapDbContext(IScrapEngineContext parent)
        {
            ParentEngine = parent;
        }

        public void Initialize()
        {
            // Read aplication specific database config
            MetaDbConfig = new DynamicAppDbConfig(ParentEngine.AppTopic);
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
            WebScrapDb.DbFactory.Initialize(ParentEngine.AppConfig.Db());

            ArgsContextInitialize dbContextArgs = new ArgsContextInitialize();
            dbContextArgs.DbFilePath = ConfigPathHelper.GetDbFilePath(ParentEngine.AppTopic, ParentEngine.ScrapperFolderPath);
            dbContextArgs.Name = ParentEngine.AppTopic;

            WebScrapDb.Initialize(dbContextArgs);

            if (ParentEngine.AppConfig.DoCreateDb())
                WebScrapDb.CreateDatabase();

            // Open connection to the database
            CreateDbContextTables();
        }

        /// <summary>
        /// Create all the application specific tables
        /// </summary>
        private void CreateDbContextTables()
        {
            WebScrapDb.Open();

            try
            {
                // Global Table metadata should contain only one type of table which is the main metdata table
                if (ParentEngine.GenericDbConfig.TableMetadataConfigs.Keys.Count > 1)
                    throw new Exception("Multiple Table metadata tables are not supported.");
                WebScrapDb.CreateTable(ParentEngine.GenericDbConfig.TableMetadataConfigs);
                // Add table metadata data
                WebScrapDb.AddOrUpdate(ParentEngine.GenericDbConfig.TableMetadataConfigs.Keys.First(),
                    MetaDbConfig.TableMetadatas);

                // Create all the data specific tables
                WebScrapDb.CreateTable(MetaDbConfig.TableColumnConfigs);

                if (ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Keys.Count > 1)
                    throw new Exception("Multiple Table column metadata are not supported.");

                // Create each table column metadata
                CreateTableColumnMetadata();

                if (ParentEngine.GenericDbConfig.TableColumnRowMetadataConfigs.Keys.Count > 1)
                    throw new Exception("Multiple Table column rows metadata are not supported.");

                // Create each table column rows metadata
                CreateTableColumnRowsMetadata();
            }
            finally
            {
                WebScrapDb.Close();
            }
        }

        private void CreateTableColumnRowsMetadata()
        {
            foreach (var tableconfig in MetaDbConfig.TableColumnConfigs)
            {
                foreach (var tablecolconfig in tableconfig.Value)
                {
                    string tableName = string.Format(
                        ParentEngine.GenericDbConfig.TableColumnRowMetadataConfigs.Keys.First(),
                        tableconfig.Key, tablecolconfig.Key);

                    WebScrapDb.CreateTable(tableName, ParentEngine.GenericDbConfig.TableColumnRowMetadataConfigs.Values);
                }
            }
        }

        private void CreateTableColumnMetadata()
        {
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                var metaTableConfig = new Dictionary<string, ConfigDbColumn>();
                string tableName = string.Format(
                    ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Keys.First(), item.Key);

                WebScrapDb.CreateTable(tableName, ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Values);
            }
        }
    }
}

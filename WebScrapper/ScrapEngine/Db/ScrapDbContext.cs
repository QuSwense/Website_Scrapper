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

        /// <summary>
        /// Initialize
        /// </summary>
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

            // Create database
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

                CreateTableMetadata();

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

        private void CreateTableMetadata()
        {
            WebScrapDb.CreateTable(ParentEngine.GenericDbConfig.TableMetadataConfigs);
            // Add table metadata data
            WebScrapDb.AddOrUpdate(ParentEngine.GenericDbConfig.TableMetadataConfigs.Keys.First(),
                MetaDbConfig.TableMetadatas);
        }

        private void CreateTableColumnMetadata()
        {
            // Create tables
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                string tableName = string.Format(
                    ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Keys.First(), item.Key);

                WebScrapDb.CreateTable(tableName, ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Values);
            }

            // Add column data that is available
            foreach (var item in MetaDbConfig.TableColumnConfigs)
            {
                string tableName = string.Format(
                    ParentEngine.GenericDbConfig.TableColumnMetadataConfigs.Keys.First(), item.Key);

                foreach (var coldata in item.Value)
                {
                    WebScrapDb.AddOrUpdate(tableName, new string[] { coldata.Key, coldata.Value.Display });
                }
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
    }
}

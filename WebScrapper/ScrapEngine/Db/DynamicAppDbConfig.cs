using DynamicDatabase.Config;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebReader.Csv;

namespace ScrapEngine.Db
{
    /// <summary>
    /// This class is used to read configuration files which contains metdaat information
    /// to create a database.
    /// It reads the files and extracts Database information, Tables, Columsn of each tables
    /// </summary>
    public class DynamicAppDbConfig : IDisposable
    {
        #region Properties

        /// <summary>
        /// A reference to the parent database context
        /// </summary>
        public IScrapDbContext ParentDbContext { get; protected set; }

        /// <summary>
        /// This data set stores the table-columns information and contains all data to create a new table
        /// </summary>
        public DbTablesDefinitionModel TableColumnConfigs { get; set; }

        /// <summary>
        /// This defines a table of constants used in the database
        /// </summary>
        public DbAppTopicConstantsDefinitionModel EnumConfigs { get; set; }

        /// <summary>
        /// this data set contians information / descriptions about the tables
        /// </summary>
        public DbTablesMetdataDefinitionModel TableMetadatas { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Constructor default
        /// </summary>
        public DynamicAppDbConfig()
        {
            TableColumnConfigs = new DbTablesDefinitionModel();
            EnumConfigs = new DbAppTopicConstantsDefinitionModel();
            TableMetadatas = new DbTablesMetdataDefinitionModel();
        }

        /// <summary>
        /// Do the initialization
        /// </summary>
        /// <param name="pDbContext"></param>
        public void Initialize(IScrapDbContext pDbContext)
        {
            ParentDbContext = pDbContext;
        }

        #endregion Constructor

        /// <summary>
        /// Read the web config files
        /// </summary>
        public void Read()
        {
            ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableMdt.AssertExists();
            ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableColumn.AssertExists();
            ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableEnum.AssertExists();

            TableColumnConfigs = CSVReader.Read<DbTablesDefinitionModel>(
                ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableColumn.FullPath);
            EnumConfigs = CSVReader.Read<DbAppTopicConstantsDefinitionModel>(
                ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableEnum.FullPath);
            TableMetadatas = CSVReader.Read<DbTablesMetdataDefinitionModel>(
                ParentDbContext.ParentEngine.AppTopicPath.DbScriptsTableMdt.FullPath);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        
        /// <summary>
        /// Read the table configurations of a database
        /// </summary>
        /// <param name="folderpath"></param>
        public void Read(string folderpath, string appTopic)
        {
            if (string.IsNullOrEmpty(folderpath)) folderpath = ".";

            using (CSVReader csvReader = new CSVReader(ConfigPathHelper.GetDbTableEnumConfigPath(folderpath, appTopic), EnumConfigs))
                csvReader.Read();
            using (CSVReader csvReader = new CSVReader(ConfigPathHelper.GetDbTableMetadataConfigPath(folderpath, appTopic), TableMetadatas))
                csvReader.Read();
            using (CSVReader csvReader = new CSVReader(ConfigPathHelper.GetDbTableColumnsConfigPath(folderpath, appTopic), TableColumnConfigs))
                csvReader.Read();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (TableColumnConfigs != null) TableColumnConfigs.Clear();
                    if (EnumConfigs != null) EnumConfigs.Clear();
                    if (TableMetadatas != null) TableMetadatas.Clear();
                }

                disposedValue = true;
            }
        }
        
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

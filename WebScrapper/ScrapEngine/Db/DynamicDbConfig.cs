using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCommon.Config;
using WebReader.Csv;

namespace ScrapEngine.Db
{
    /// <summary>
    /// This class is used to read configuration files which contains metdaat information
    /// to create a database.
    /// It reads the files and extracts Database information, Tables, Columsn of each tables
    /// </summary>
    public class DynamicDbConfig : IDisposable
    {
        /// <summary>
        /// This data set stores the table-columns information and contains all data to create a new table
        /// </summary>
        public Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnConfigs { get; set; }

        /// <summary>
        /// This defines a table of contants used in the database
        /// </summary>
        public Dictionary<string, Dictionary<int, string>> EnumConfigs { get; set; }

        /// <summary>
        /// this data set contians information / descriptions about the tables
        /// </summary>
        public Dictionary<string, ConfigDbTable> TableMetadatas { get; set; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public DynamicDbConfig() { }

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

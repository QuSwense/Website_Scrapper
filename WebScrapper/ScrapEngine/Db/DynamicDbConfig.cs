using DynamicDatabase.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WebCommon.Config;
using WebReader.Csv;

namespace ScrapEngine.Db
{
    public class DynamicDbConfig
    {
        public string FolderPath { get; protected set; }

        /// <summary>
        /// This data set stores the table informations
        /// </summary>
        public Dictionary<string, Dictionary<string, ConfigDbColumn>> TableMetadataConfigs { get; set; }

        /// <summary>
        /// This data set stores the table column informations
        /// </summary>
        public Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnMetadataConfigs { get; set; }

        /// <summary>
        /// This data set stores the table column rows informations
        /// </summary>
        public Dictionary<string, Dictionary<string, ConfigDbColumn>> TableColumnRowMetadataConfigs { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DynamicDbConfig(string folderPath)
        {
            FolderPath = folderPath;
            TableMetadataConfigs = new Dictionary<string, Dictionary<string, ConfigDbColumn>>();
            TableColumnMetadataConfigs = new Dictionary<string, Dictionary<string, ConfigDbColumn>>();
            TableColumnRowMetadataConfigs = new Dictionary<string, Dictionary<string, ConfigDbColumn>>();
        }

        /// <summary>
        /// Read the config files
        /// </summary>
        public void Read()
        {
            string tableMetadataFile = ConfigPathHelper.GetGenericDbScriptsTableMdtCsv(FolderPath);
            string tableColMetadataFile = ConfigPathHelper.GetGenericDbScriptsTableColMdtCsv(FolderPath);
            string tableColRowMetadataFile = ConfigPathHelper.GetGenericDbScriptsTableColRowMdtCsv(FolderPath);

            if (!File.Exists(tableMetadataFile)) throw new Exception("Unable to find Table metdata creation information file " + tableMetadataFile);
            if (!File.Exists(tableColMetadataFile)) throw new Exception("Unable to find Table column metdata creation information file " + tableColMetadataFile);
            if (!File.Exists(tableColRowMetadataFile)) throw new Exception("Unable to find Table column rows metdata creation information file " + tableColRowMetadataFile);

            using (CSVReader reader = new CSVReader(tableMetadataFile, TableMetadataConfigs)) reader.Read();
            using (CSVReader reader = new CSVReader(tableColMetadataFile, TableColumnMetadataConfigs)) reader.Read();
            using (CSVReader reader = new CSVReader(tableColRowMetadataFile, TableColumnRowMetadataConfigs)) reader.Read();
        }
    }
}

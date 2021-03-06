﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.Config;
using WebScrapper.Error;
using WebScrapper.Reader;
using WebScrapper.Reader.Meta;

namespace WebScrapper.Db
{
    public class DbConfigModel
    {
        public string AppTopic { get; set; }

        public Dictionary<string, Dictionary<string, ColumnDbConfigModel>> TableDbConfigs { get; set; }

        public Dictionary<string, Dictionary<int, string>> EnumConfigs { get; set; }

        public Dictionary<string, TableMetadataConfigModel> TableMetadata { get; set; }

        public DbConfigModel(string apptopic)
        {
            AppTopic = apptopic;
            TableDbConfigs = new Dictionary<string, Dictionary<string, ColumnDbConfigModel>>();
            EnumConfigs = new Dictionary<string, Dictionary<int, string>>();
            TableMetadata = new Dictionary<string, TableMetadataConfigModel>();
        }

        public void Read()
        {
            CSVReader.Read(ConfigHelper.GetDbTableEnumConfigPath(AppTopic), EnumConfigs);
            CSVReader.Read(ConfigHelper.GetDbTableMetadataConfigPath(AppTopic), TableMetadata);
            CSVReader.Read(ConfigHelper.GetDbTableColumnsConfigPath(AppTopic), TableDbConfigs);
        }

        public Dictionary<string, ColumnDbConfigModel> GetTable(string name)
        {
            TableDbConfigModel tableDb;

            WebScrapperException.Assert(() => TableDbConfigs.ContainsKey(name),
                    "Table not found");

            return TableDbConfigs[name];
        }
    }
}

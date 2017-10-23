using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebCommon.Config
{
    public class ConfigPathHelper
    {
        public static readonly string ScrapperAppsFolder = "ScrapperApps";
        public static readonly string ScrapperAppsDbScriptsFolder = "dbscripts";

        public static string GetAppConfigPath(string folderpath, string appTopic)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, appTopic + "Config.xml");
        }

        public static string GetDbTableEnumConfigPath(string folderpath, string appTopic)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_enum.csv");
        }

        public static string GetDbTableMetadataConfigPath(string folderpath, string appTopic)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_metadata.csv");
        }

        public static string GetDbTableColumnsConfigPath(string folderpath, string appTopic)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_columns.csv");
        }

        public static string GetScrapConfigPath(string folderpath, string appTopic)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, "Scrap", appTopic + "Scrap.xml");
        }

        public static string GetDbConfigPath(string folderpath, string appTopic, string extension)
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, appTopic + extension);
        }
    }
}

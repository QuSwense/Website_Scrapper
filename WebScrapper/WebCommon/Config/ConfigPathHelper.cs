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

        public static string GetAppConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, "AppConfig.xml");
        }

        public static string GetDbTableEnumConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_enum.csv");
        }

        public static string GetDbTableMetadataConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_metadata.csv");
        }

        public static string GetDbTableColumnsConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, ScrapperAppsDbScriptsFolder, "table_columns.csv");
        }

        public static string GetScrapConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, "Scrap", appTopic + "Scrap.xml");
        }

        public static string GetDbFilePath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic);
        }
    }
}

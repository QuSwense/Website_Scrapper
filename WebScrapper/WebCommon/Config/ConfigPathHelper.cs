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

        public static string GetGenericDbScriptsTableMdtCsv(string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, "DbScripts", "table_metadata.csv");
        }

        public static string GetGenericDbScriptsTableColMdtCsv(string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, "DbScripts", "table_column_mdt.csv");
        }

        public static string GetGenericDbScriptsTableColRowMdtCsv(string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, "DbScripts", "table_column_rows_mdt.csv");
        }

        public static string GetScrapperAppFolderPath(string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder);
        }

        public static string GetAppConfigPath(string appTopic, string folderpath = "")
        {
            return Path.Combine(folderpath, ScrapperAppsFolder, appTopic, "AppConfig.xml");
        }

        public static string GetAppGenericConfigPath(string folderpath = "")
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

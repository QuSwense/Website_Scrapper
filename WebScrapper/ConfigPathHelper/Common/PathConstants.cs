using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigPathHelper.Common
{
    /// <summary>
    /// All the constants used in this project on path
    /// </summary>
    public static class PathConstants
    {
        #region Folders

        public static string PrefixAppFolderName = "App";
        public static string PrefixScrapFolderName = "Scrap";
        public static string DbScriptsFolderName = "DbScripts";
        public static string ScrapperAppsFolderName = "ScrapperApps";
        public static string ConfigFolderName = "Config";

        #endregion Folders

        #region Files

        public static string SuffixConfigFileName = "Config.xml";
        public static string SuffixScrapFileName = "Scrap.xml";
        public static string TableColumnsFileName = "table_columns.csv";
        public static string TableEnumsFileName = "table_enums.csv";
        public static string TableMetadataFileName = "table_mdt.csv";
        public static string TableScrapMetadataFileName = "table_scrap_mdt.csv";
        public static string PerformanceMetadataFileName = "performance_mdt.csv";
        public static string ColumnScrapMetadataFileName = "col_scrap_mdt.csv";
        public static string AppConfigFileName = "AppConfig.xml";

        #endregion Files

        #region Others

        public static string PrefixAppFolderSearch = "App*";

        #endregion Others
    }
}

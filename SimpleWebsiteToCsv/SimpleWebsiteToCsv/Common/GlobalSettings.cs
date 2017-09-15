using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.Common
{
    public static class GlobalSettings
    {
        public static bool UseOfflineMode { get; set; }
        public static bool PopulateOfflineCIAFactbookUrl { get; set; }
        public static string TablePrefix { get; set; }
        public static class FolderPath
        {
            public static string Base = "Offline_WebPage";
            public static string Country
            {
                get { return Path.Combine(Base, "Country"); }
            }
        } 

        static GlobalSettings()
        {
            UseOfflineMode = true;
            PopulateOfflineCIAFactbookUrl = true;
            TablePrefix = "qusc_";
        }
    }
}

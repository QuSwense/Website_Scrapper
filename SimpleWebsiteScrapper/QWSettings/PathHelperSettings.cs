using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QWSettings
{
    /// <summary>
    /// The settings class to be used by other assemblies
    /// </summary>
    public class PathHelperSettings : HelperSettings<PathSettings>
    {
        /// <summary>
        /// Get the root folder path to store webpages offline.
        /// In case of empty, it sends the current assembly path.
        /// It does not creates the folder if not exists
        /// </summary>
        /// <returns>The full path of the folder</returns>
        public static string WebOfflineRootFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(SettingsObj.WebOfflineRootFolder)) return SettingsObj.WebOfflineRootFolder;
                else return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        /// <summary>
        /// Get the list of characters which is not allowed in Path and should be replaced
        /// </summary>
        public static string RegexUnwantedPathChars
        {
            get
            {
                return SettingsObj.UnwantedPathChars;
            }
        }
    }
}

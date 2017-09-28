using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QWWebScrap
{
    /// <summary>
    /// This class holds the global settings data
    /// </summary>
    public static class QWSettings
    {
        /// <summary>
        /// Sets the webpage offline save location
        /// If this is null or empty, then consider the relative location of the assembly
        /// use the method to get this value
        /// </summary>
        public static string WebOfflineRootFolder = "";

        /// <summary>
        /// A flag which sets if the offline Uri is to be generated from online url automatically
        /// The class <see cref="Model.UriHint"/> uses this flag to calculate the offline url which is used for
        /// Saving the data
        /// </summary>
        public static bool DoGenerateOfflineUriFromOnline = true;

        /// <summary>
        /// A helper class to read the settings for path
        /// </summary>
        public static class PathHelper
        {
            public static string GetWebOfflineRootFolder()
            {
                if (!string.IsNullOrEmpty(WebOfflineRootFolder)) return WebOfflineRootFolder;
                else return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }
    }
}

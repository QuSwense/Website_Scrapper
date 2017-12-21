using ConfigPathHelper.Common;
using System.Collections.Generic;
using System.IO;

namespace ConfigPathHelper
{
    /// <summary>
    /// A helper class to access a Application specific config paths
    /// The <see cref="AppGenericConfigPathHelper"/> class must be initialized before using this class
    /// </summary>
    public class AppTopicConfigPathHelper
    {
        #region Properties

        /// <summary>
        /// The application topic name
        /// </summary>
        public string AppTopicName { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public AppTopicConfigPathHelper() { }

        /// <summary>
        /// Constructor with application topic
        /// </summary>
        /// <param name="appTopic"></param>
        public AppTopicConfigPathHelper(string appTopic)
        {
            AppTopicName = appTopic;

            AppTopicMain = new PathGeneric(PathConstants.PrefixAppFolderName + AppTopicName,
                AppGenericConfigPathHelper.I.ScrapperApps);
            AppConfig = new PathGeneric(AppTopicName + PathConstants.SuffixConfigFileName, 
                AppTopicMain, true);
            ScrapConfig = new PathGeneric(PathConstants.PrefixScrapFolderName, AppTopicMain);
            AppTopicScrap = new PathGeneric(AppTopicName + PathConstants.SuffixScrapFileName,
                ScrapConfig, true);
            DbScripts = new PathGeneric(PathConstants.DbScriptsFolderName, AppTopicMain);
            DbScriptsTableColumn = new PathGeneric(PathConstants.TableColumnsFileName,
                DbScripts, true);
            DbScriptsTableEnum = new PathGeneric(PathConstants.TableEnumsFileName,
                DbScripts, true);
            DbScriptsTableMdt = new PathGeneric(PathConstants.TableMetadataFileName,
                DbScripts, true);
        }

        #endregion Constructor

        #region Accessor

        /// <summary>
        /// The application topic main folder path
        /// </summary>
        public PathGeneric AppTopicMain { get; protected set; }

        /// <summary>
        /// The application topic scrap folder
        /// </summary>
        public PathGeneric AppConfig { get; protected set; }

        /// <summary>
        /// The application topic scrap folder
        /// </summary>
        public PathGeneric ScrapConfig { get; protected set; }

        /// <summary>
        /// The application topic scrap config file name
        /// </summary>
        public PathGeneric AppTopicScrap { get; protected set; }

        /// <summary>
        /// The app topic specific database scripts folder name
        /// </summary>
        public PathGeneric DbScripts { get; protected set; }

        /// <summary>
        /// The name of table column file
        /// </summary>
        public PathGeneric DbScriptsTableColumn { get; protected set; }

        /// <summary>
        /// The name of table enums (constants to be used in the application)
        /// </summary>
        public PathGeneric DbScriptsTableEnum { get; protected set; }

        /// <summary>
        /// The name of table metdata file
        /// </summary>
        public PathGeneric DbScriptsTableMdt { get; protected set; }

        #endregion Accessor

        #region Static

        /// <summary>
        /// A static initializer of app topic specific folders / files path
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<AppTopicConfigPathHelper> GetAppTopics()
        {
            foreach (var folderAppTopic in 
                Directory.GetDirectories(AppGenericConfigPathHelper.I.ScrapperApps.FullPath,
                PathConstants.PrefixAppFolderSearch))
            {
                // Remove "App" from the folder name
                DirectoryInfo di = new DirectoryInfo(folderAppTopic);
                string appTopic = di.Name.Remove(0, 3);
                AppTopicConfigPathHelper appTopicConfigPath = new AppTopicConfigPathHelper(appTopic);

                yield return appTopicConfigPath;
            }
        }

        #endregion Static
    }
}

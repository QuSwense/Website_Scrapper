﻿using ConfigPathHelper.Common;

namespace ConfigPathHelper
{
    /// <summary>
    /// A helper function to determine all the paths accessed throughout the solution
    /// This is implemented as a singleton static instance as the values are independent of any threads
    /// These path are the global paths and not specific to any application topic path
    /// </summary>
    public class AppGenericConfigPathHelper
    {
        #region Singleton

        /// <summary>
        /// A static object to the self but inaccessible outside the class
        /// </summary>
        private static AppGenericConfigPathHelper _this;

        /// <summary>
        /// A public member to access this object
        /// </summary>
        public static AppGenericConfigPathHelper I
        {
            get
            {
                if (_this == null) _this = new AppGenericConfigPathHelper();
                return _this;
            }
        }

        /// <summary>
        /// Initialize with values for the generic config path
        /// </summary>
        /// <param name="rootPath"></param>
        public void Initialize(string rootPath)
        {
            RootPath = rootPath;
        }

        /// <summary>
        /// Constructor protected to make singleton
        /// </summary>
        protected AppGenericConfigPathHelper()
        {
            ScrapperApps = new PathGeneric(PathConstants.ScrapperAppsFolderName, RootPath);
            GlobalAppConfig = new PathGeneric(PathConstants.AppConfigFileName,
                ScrapperApps, true);
            GlobalConfig = new PathGeneric(PathConstants.ConfigFolderName, ScrapperApps);
            GlobalDbScripts = new PathGeneric(PathConstants.DbScriptsFolderName,
                GlobalConfig);
            DbScriptsTableScrapMdt = new PathGeneric(PathConstants.TableScrapMetadataFileName,
                GlobalDbScripts, true);
            DbScriptsPerformanceMdt = new PathGeneric(PathConstants.PerformanceMetadataFileName,
                GlobalDbScripts, true);
            DbScriptsTableMdt = new PathGeneric(PathConstants.TableMetadataFileName,
                GlobalDbScripts, true);
            DbScriptsColumnScrapMdt = new PathGeneric(PathConstants.ColumnScrapMetadataFileName,
                GlobalDbScripts, true);
        }

        #endregion Singleton

        #region Properties

        /// <summary>
        /// The path of the Scrapper apps root folder
        /// </summary>
        public string RootPath { get; protected set; }

        #endregion Properties

        #region Accessor

        /// <summary>
        /// The root scrapper apps folder name
        /// </summary>
        public PathGeneric ScrapperApps { get; protected set; }

        /// <summary>
        /// The global config file
        /// </summary>
        public PathGeneric GlobalAppConfig { get; protected set; }

        /// <summary>
        /// The global config folder name
        /// </summary>
        public PathGeneric GlobalConfig { get; protected set; }

        /// <summary>
        /// The global database scripts folder name
        /// </summary>
        public PathGeneric GlobalDbScripts { get; protected set; }

        /// <summary>
        /// The name of table column metdata file
        /// </summary>
        public PathGeneric DbScriptsTableScrapMdt { get; protected set; }

        /// <summary>
        /// The name of table column rows metdata file
        /// </summary>
        public PathGeneric DbScriptsPerformanceMdt { get; protected set; }

        /// <summary>
        /// The name of table metdata file
        /// </summary>
        public PathGeneric DbScriptsTableMdt { get; protected set; }

        /// <summary>
        /// The column scrap metdata file
        /// </summary>
        public PathGeneric DbScriptsColumnScrapMdt { get; protected set; }

        #endregion Accessor
    }
}

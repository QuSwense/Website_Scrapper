using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebCommon.PathHelp
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
            ScrapperApps = new PathGeneric("ScrapperApps");
            GlobalConfig = new PathGeneric("Config", ScrapperApps);
            GlobalDbScripts = new PathGeneric("DbScripts", GlobalConfig);
            DbScriptsTableColumnMdt = new PathGeneric("table_column_mdt.csv", GlobalDbScripts, true);
            DbScriptsTableColumnRowsMdt = new PathGeneric("table_column_rows_mdt.csv", GlobalDbScripts, true);
            DbScriptsTableMdt = new PathGeneric("table_mdt.csv", GlobalDbScripts, true);
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
        public PathGeneric DbScriptsTableColumnMdt { get; protected set; }

        /// <summary>
        /// The name of table column rows metdata file
        /// </summary>
        public PathGeneric DbScriptsTableColumnRowsMdt { get; protected set; }

        /// <summary>
        /// The name of table metdata file
        /// </summary>
        public PathGeneric DbScriptsTableMdt { get; protected set; }

        #endregion Accessor
    }
}

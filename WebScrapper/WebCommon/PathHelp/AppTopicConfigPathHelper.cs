using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WebCommon.PathHelp
{
    /// <summary>
    /// A helper class to access a Application specific config paths
    /// The <see cref="AppGenericConfigPathHelper"/> class must be initialized before using this class
    /// </summary>
    public class AppTopicConfigPathHelper
    {
        #region Properties

        /// <summary>
        /// The application topic 
        /// </summary>
        public string AppTopic { get; set; }

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
            AppTopic = appTopic;
        }

        /// <summary>
        /// Initialize the application topic data
        /// </summary>
        /// <param name="appTopic"></param>
        public void Initialize(string appTopic)
        {
            AppTopicMain = new PathGeneric("App" + AppTopic, AppGenericConfigPathHelper.I.ScrapperApps);
            ScrapConfig = new PathGeneric("Scrap", AppTopicMain);
            AppTopicScrap = new PathGeneric(AppTopic + "Scrap.xml", ScrapConfig, true);
            DbScripts = new PathGeneric("DbScripts", AppTopicMain);
            DbScriptsTableColumn = new PathGeneric("table_columns.csv", DbScripts, true);
            DbScriptsTableEnum = new PathGeneric("table_enums.csv", DbScripts, true);
            DbScriptsTableMdt = new PathGeneric("table_mdt.csv", DbScripts, true);
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
    }
}

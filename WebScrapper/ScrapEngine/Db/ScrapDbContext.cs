using ScrapEngine.Interfaces;
using System.Collections.Generic;
using ScrapEngine.Model;
using SqliteDatabase;
using SqliteDatabase.Model;
using WebCommon.Combinatorics;

namespace ScrapEngine.Db
{
    /// <summary>
    /// A class used to help in forming a wrapper between the actual Database activity and the 
    /// </summary>
    public class ScrapDbContext : IScrapDbContext
    {
        #region Properties

        /// <summary>
        /// The application name topic for which the web scrapper Database is to be generated
        /// </summary>
        public IScrapEngineContext ParentEngine { get; protected set; }
        
        /// <summary>
        /// Read the Database configuration
        /// </summary>
        public DynamicAppDbConfig MetaDbConfig { get; protected set; }

        /// <summary>
        /// The configuration for web scrapping
        /// </summary>
        public DatabaseContext WebScrapDb { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ScrapDbContext() { }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize(IScrapEngineContext parent)
        {
            ParentEngine = parent;

            // Read aplication specific database config
            MetaDbConfig = DynamicAppDbConfig.Init(this);
            MetaDbConfig.Read();

            // Create database context
            InitializeDbContext();
        }

        /// <summary>
        /// Initialize database context
        /// </summary>
        private void InitializeDbContext()
        {
            WebScrapDb = new DatabaseContext();
            
            WebScrapDb.Initialize(ParentEngine.AppTopicPath.AppTopicMain.FullPath,
                ParentEngine.AppTopicPath.AppTopic);

            if (ParentEngine.AppConfig.DoCreateDb())
                WebScrapDb.CreateDatabase();

            // Create database
            CreateDbContextTables();
        }

        #endregion Constructor

        #region Create

        /// <summary>
        /// Create all the application specific tables
        /// </summary>
        private void CreateDbContextTables()
        {
            WebScrapDb.Open();

            try
            {
                WebScrapDb.CreateMetadata();
                WebScrapDb.AddTableMetadata(MetaDbConfig.TableMetadatas);
                WebScrapDb.Create(MetaDbConfig.TableColumnConfigs);
            }
            finally
            {
                WebScrapDb.Close();
            }
        }

        /// <summary>
        /// Add or update the data scrapped from the webpages including the metadata information
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="row"></param>
        public void AddOrUpdate(ScrapElement scrapConfig, List<DynamicTableDataInsertModel> row)
        {
            // Add data
            WebScrapDb.AddOrUpdate(scrapConfig.TableName, row, scrapConfig.DoUpdateOnly);
        }

        /// <summary>
        /// To Add all combination of column values
        /// </summary>
        /// <param name="scrapConfig"></param>
        /// <param name="rows"></param>
        public void AddOrUpdate(ScrapElement scrapConfig, List<List<DynamicTableDataInsertModel>> rows)
        {
            // Create Combinations of data list
            Combinations<List<DynamicTableDataInsertModel>> rowCombinations =
                new Combinations<List<DynamicTableDataInsertModel>>(rows, rows.Count);

            foreach (var row in rowCombinations)
            {
                WebScrapDb.AddOrUpdate(scrapConfig.TableName, row, scrapConfig.DoUpdateOnly);
            }
        }

        /// <summary>
        /// LOad table with partial columns
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        public void AddMetadata(ScrapElement webScrapConfigObj)
        {
            int uid = AddTableScrapMetadata(webScrapConfigObj);
            AddColumnScrapMetadata(webScrapConfigObj, uid);
        }

        private void AddColumnScrapMetadata(ScrapElement webScrapConfigObj, int uid)
        {
            List<ColumnScrapMetadataModel> colScrapMdtModels = new List<ColumnScrapMetadataModel>();

            foreach(var colConfig in webScrapConfigObj.Columns)
            {
                ColumnScrapMetadataModel colScrapMdtModel = new ColumnScrapMetadataModel();
                colScrapMdtModel.ColumnName = colConfig.Name;
                if(!string.IsNullOrEmpty(webScrapConfigObj.TableName) &&
                    !string.IsNullOrEmpty(colConfig.Name))
                    colScrapMdtModel.DisplayName = MetaDbConfig.TableColumnConfigs[webScrapConfigObj.TableName][colConfig.Name].Display;
                colScrapMdtModel.Index = colConfig.Index;
                colScrapMdtModel.Uid = uid;
                colScrapMdtModel.XPath = colConfig.XPath;
                colScrapMdtModels.Add(colScrapMdtModel);
            }

            WebScrapDb.Add(colScrapMdtModels);
        }

        private int AddTableScrapMetadata(ScrapElement webScrapConfigObj)
        {
            TableScrapMetadataModel tblScrapMdtModel = new TableScrapMetadataModel();

            // Add table metdata scrap info
            List<string> Urls = new List<string>();
            List<string> XPaths = new List<string>();
            string name = "";

            // Get to the topmost Scrap node
            ScrapElement mainWebScrap = webScrapConfigObj;
            while (mainWebScrap != null)
            {
                Urls.Add(mainWebScrap.Url);

                if(mainWebScrap is ScrapHtmlTableElement)
                    XPaths.Add(((ScrapHtmlTableElement)mainWebScrap).XPath);
                if (!string.IsNullOrEmpty(mainWebScrap.TableName)) name = mainWebScrap.TableName;
                mainWebScrap = mainWebScrap.Parent;
            }

            Urls.Reverse();
            XPaths.Reverse();

            tblScrapMdtModel.Name = name;
            if (Urls.Count > 0) tblScrapMdtModel.Url1 = Urls[0];
            if (Urls.Count > 1) tblScrapMdtModel.Url2 = Urls[1];
            if (Urls.Count > 2) tblScrapMdtModel.Url3 = Urls[2];
            if (Urls.Count > 3) tblScrapMdtModel.Url4 = Urls[3];
            if (XPaths.Count > 0) tblScrapMdtModel.XPath1 = XPaths[0];
            if (XPaths.Count > 1) tblScrapMdtModel.XPath2 = XPaths[1];
            if (XPaths.Count > 2) tblScrapMdtModel.XPath3 = XPaths[2];
            if (XPaths.Count > 3) tblScrapMdtModel.XPath4 = XPaths[3];

            return WebScrapDb.Add(tblScrapMdtModel);
        }

        #endregion Create
    }
}

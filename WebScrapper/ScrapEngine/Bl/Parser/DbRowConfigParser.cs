using log4net;
using ScrapEngine.Db;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using ScrapEngine.Model.Scrap;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// Parse a single row of data and save it to the database
    /// </summary>
    public class DbRowConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ScrapColumnConfigParser));
        
        #region Helper Properties

        /// <summary>
        /// Reference to column scrap iterator args
        /// </summary>
        public ColumnScrapStateModel currentColumnScrapIteratorArgs
        {
            get
            {
                return configParser.StateModel.CurrentColumnScrapIteratorArgs;
            }
        }

        /// <summary>
        /// Reference to the list column
        /// </summary>
        public ScrapElement ScrapConfig
        {
            get
            {
                return currentColumnScrapIteratorArgs.Parent.ScrapConfigObj;
            }
        }

        /// <summary>
        /// The Meta database config object
        /// </summary>
        public DynamicAppDbConfig MetaDbConfig
        {
            get
            {
                return configParser.WebDbContext.MetaDbConfig;
            }
        }

        #endregion Helper Properties

        /// <summary>
        /// Scrap column config parser
        /// </summary>
        protected ScrapColumnConfigParser scrapColumnConfigParser;

        /// <summary>
        /// Scrap column config parser
        /// </summary>
        protected GroupColumnConfigParser groupColumnConfigParser;

        public DbRowConfigParser() { }

        public DbRowConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapColumnConfigParser = new ScrapColumnConfigParser(configParser);
            groupColumnConfigParser = new GroupColumnConfigParser(configParser);
        }

        /// <summary>
        /// Process
        /// </summary>
        public override void Process()
        {
            logger.DebugFormat("Scrapping data from online text file");
            ParseColumnsConfig();
            ColumnScrapIterator();
        }

        /// <summary>
        /// Parses Column element tags
        /// </summary>
        private void ParseColumnsConfig()
        {
            if (!configParser.StateModel.IsColumnMetadataUpdated)
            {
                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(ScrapConfig);
                configParser.StateModel.SetColumnMetadataFlag();
            }
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator()
        {
            var rows = new List<List<DynamicTableDataInsertModel>>();

            currentColumnScrapIteratorArgs.PreProcess();

            for (int indx = 0; indx < ScrapConfig.DbRow.Columns.Count; ++indx)
            {
                ColumnElement colElement = ScrapConfig.DbRow.Columns[indx];

                if (!configParser.StateModel.IsColumnDataAvailable(colElement))
                {
                    configParser.StateModel.SetColumnConfig(colElement);
                    
                    if(colElement is GroupColumnElement)
                    {
                        groupColumnConfigParser.Process();
                        configParser.StateModel.AddColumnData(groupColumnConfigParser.DbTableDataMapper(currentColumnScrapIteratorArgs.ResultColumnScrap,
                        colElement), colElement);
                    }
                    else
                    {
                        scrapColumnConfigParser.Process();
                        configParser.StateModel.AddColumnData(scrapColumnConfigParser.DbTableDataMapper(currentColumnScrapIteratorArgs.ResultColumnScrap,
                        colElement), colElement);
                    }
                }
            }

            UpdateScrappedData();
        }

        /// <summary>
        /// Update the rows of scrapped data
        /// </summary>
        /// <param name="rows"></param>
        private void UpdateScrappedData()
        {
            var rows = configParser.StateModel.GetColumnDataList();
            if (ScrapConfig.DbRow.Columns.Count > 0 && rows.Count > 0)
            {
                configParser.Performance.NewDbUpdate(rows, currentColumnScrapIteratorArgs);
                configParser.WebDbContext.AddOrUpdate(ScrapConfig, rows);
                configParser.Performance.FinalDbUpdate(rows, currentColumnScrapIteratorArgs);
            }
        }
    }
}

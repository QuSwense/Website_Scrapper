using log4net;
using ScrapEngine.Db;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
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
        public ColumnScrapIteratorArgs currentColumnScrapIteratorArgs
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

        public DbRowConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapColumnConfigParser = new ScrapColumnConfigParser(configParser);
        }

        /// <summary>
        /// Process
        /// </summary>
        public void Process()
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
                configParser.StateModel.SetColumnConfig(ScrapConfig.DbRow.Columns[indx]);
                scrapColumnConfigParser.Process();
                rows.Add(DbTableDataMapper(currentColumnScrapIteratorArgs.ResultColumnScrap,
                    ScrapConfig.DbRow.Columns[indx]));
            }

            UpdateScrappedData(rows);
        }

        /// <summary>
        /// Update the rows of scrapped data
        /// </summary>
        /// <param name="rows"></param>
        private void UpdateScrappedData(List<List<DynamicTableDataInsertModel>> rows)
        {
            if (ScrapConfig.DbRow.Columns.Count > 0 && rows.Count > 0)
            {
                configParser.Performance.NewDbUpdate(rows, currentColumnScrapIteratorArgs);
                configParser.WebDbContext.AddOrUpdate(ScrapConfig, rows);
                configParser.Performance.FinalDbUpdate(rows, currentColumnScrapIteratorArgs);
            }
        }

        /// <summary>
        /// For each data scrapped from the reference link
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="columnConfig"></param>
        /// <returns></returns>
        private List<DynamicTableDataInsertModel> DbTableDataMapper(ManipulateHtmlData manipulateHtml,
            ColumnElement columnConfig)
        {
            // A list to store multiple column values for >1 cardinal values
            var colValues = new List<DynamicTableDataInsertModel>();

            foreach (var result in manipulateHtml.Results)
            {
                var tableDataColumn = new DynamicTableDataInsertModel()
                {
                    Name = columnConfig.Name,
                    IsPk = columnConfig.IsUnique,
                    Value = result,
                    DataType =
                    MetaDbConfig.TableColumnConfigs[
                        ScrapConfig.TableName][columnConfig.Name].DataType
                };

                if (tableDataColumn.IsPk && string.IsNullOrEmpty(tableDataColumn.Value))
                    continue;
                colValues.Add(tableDataColumn);
            }

            return colValues;
        }
    }
}

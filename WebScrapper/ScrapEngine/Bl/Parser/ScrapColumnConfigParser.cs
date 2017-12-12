using log4net;
using ScrapEngine.Db;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using SqliteDatabase.Model;
using System.Collections.Generic;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic to parse the "Columns" Xml node and execute web scrapping
    /// process. This class uses the scrapped data to save it into the table column specified
    /// after the manipulations
    /// </summary>
    public class ScrapColumnConfigParser : AppTopicConfigParser
    {
        #region Properties

        /// <summary>
        /// Logger
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ScrapColumnConfigParser));

        /// <summary>
        /// The factory class to get the type of Manipulate child
        /// </summary>
        private ManipulateChildFactory manipulateChildFactory;

        #endregion Properties

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

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapColumnConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            manipulateChildFactory = new ManipulateChildFactory(configParser);
        }

        #endregion Constructor

        /// <summary>
        /// Process Column element tag for scrapping data from csv file
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

            for (int indx = 0; indx < ScrapConfig.Columns.Count; ++indx)
            {
                var columnConfig = ScrapConfig.Columns[indx];
                var manipulateHtml = new ManipulateHtmlData()
                {
                    Cardinality = columnConfig.Cardinal,
                    OriginalValue = currentColumnScrapIteratorArgs.GetDataIterator(indx)
                };
                manipulateHtml.Results.Add(manipulateHtml.OriginalValue);

                Manipulate(columnConfig, manipulateHtml);

                if (CheckSkipUpdate(manipulateHtml, columnConfig)) return;
                rows.Add(DbTableDataMapper(manipulateHtml, columnConfig));
            }

            UpdateScrappedData(rows);
        }

        /// <summary>
        /// Update the rows of scrapped data
        /// </summary>
        /// <param name="rows"></param>
        private void UpdateScrappedData(List<List<DynamicTableDataInsertModel>> rows)
        {
            if (ScrapConfig.Columns.Count > 0 && rows.Count > 0)
            {
                configParser.Performance.NewDbUpdate(rows, currentColumnScrapIteratorArgs);
                configParser.WebDbContext.AddOrUpdate(ScrapConfig, rows);
                configParser.Performance.FinalDbUpdate(rows, currentColumnScrapIteratorArgs);
            }
        }

        /// <summary>
        /// Process the logic to skip the storage of column values
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="columnConfig"></param>
        /// <returns></returns>
        private bool CheckSkipUpdate(ManipulateHtmlData manipulateHtml, ColumnElement columnConfig)
        {
            if (columnConfig.IsUnique && (manipulateHtml.Results.Count <= 0)) return true;

            if(columnConfig.SkipIfValues != null && columnConfig.SkipIfValues.Count > 0)
            {
                foreach (var result in manipulateHtml.Results)
                {
                    if (columnConfig.SkipIfValues.Contains(result)) return true;
                }
            }

            return false;
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

        /// <summary>
        /// Manipulate the scrapped column data
        /// </summary>
        /// <param name="scrapConfig"></param>
        /// <param name="manipulateNodeList"></param>
        /// <param name="dataNode"></param>
        /// <returns></returns>
        private void Manipulate(ColumnElement columnConfig, ManipulateHtmlData manipulateHtml)
        {
            logger.DebugFormat("For Column '{0}' Scrapped data '{1}'", 
                columnConfig.Name, manipulateHtml.OriginalValue);

            // Even if Scrapped data is null send to manipulation tag. As there can be a default
            if (columnConfig.Manipulation != null && columnConfig.Manipulation.ManipulateChilds.Count > 0)
            {
                foreach (var manipulateChild in columnConfig.Manipulation.ManipulateChilds)
                {
                    manipulateChildFactory.GetParser(manipulateChild).Process(manipulateHtml, manipulateChild);
                }
            }
        }   
    }
}

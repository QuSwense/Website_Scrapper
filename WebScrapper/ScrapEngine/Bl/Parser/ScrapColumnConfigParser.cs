using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using SqliteDatabase.Model;
using System.Collections.Generic;
using System.Xml;
using System;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic to parse the "Columns" Xml node and execute web scrapping
    /// process. This class uses the scrapped data to save it into the table column specified
    /// after the manipulations
    /// </summary>
    public class ScrapColumnConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Logger
        /// </summary>
        public static ILog logger = LogManager.GetLogger(typeof(ScrapColumnConfigParser));

        /// <summary>
        /// The factory class to get the type of Manipulate child
        /// </summary>
        private ManipulateChildFactory manipulateChildFactory;

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
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapColumnConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            manipulateChildFactory = new ManipulateChildFactory(configParser);
        }

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
                configParser.WebDbContext.AddMetadata(
                    currentColumnScrapIteratorArgs.Parent);
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
            bool doSkipDbAddUpdate = false;

            currentColumnScrapIteratorArgs.PreProcess();

            for (int indx = 0; indx < currentColumnScrapIteratorArgs.Parent.Columns.Count; ++indx)
            {
                var columnConfig = currentColumnScrapIteratorArgs.Parent.Columns[indx];

                var manipulateHtml = new ManipulateHtmlData()
                {
                    Cardinality = columnConfig.Cardinal,
                    OriginalValue = currentColumnScrapIteratorArgs.GetDataIterator(columnConfig)
                };
                manipulateHtml.Results.Add(manipulateHtml.OriginalValue);

                Manipulate(columnConfig, manipulateHtml);

                if (columnConfig.IsUnique && (manipulateHtml.Results.Count <= 0))
                {
                    doSkipDbAddUpdate = true;
                    break;
                }
                
                rows.Add(DbTableDataMapper(manipulateHtml, columnConfig));
            }

            if (!doSkipDbAddUpdate && currentColumnScrapIteratorArgs.Parent.Columns.Count > 0)
            {
                configParser.Performance.NewDbUpdate(rows, currentColumnScrapIteratorArgs);

                configParser.WebDbContext.AddOrUpdate(currentColumnScrapIteratorArgs.Parent, rows);

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
                    configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[
                        currentColumnScrapIteratorArgs.Parent.TableName][columnConfig.Name].DataType
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
            if (columnConfig.Manipulations != null && columnConfig.Manipulations.Count > 0)
            {
                foreach (var manipulateChild in columnConfig.Manipulations)
                {
                    manipulateChildFactory.GetParser(manipulateChild).Process(manipulateHtml, manipulateChild);
                }
            }
        }   
    }
}

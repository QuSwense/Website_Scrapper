using log4net;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using SqliteDatabase.Model;
using System.Collections.Generic;
using System.Xml;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic to parse the "Columns" Xml node and execute web scrapping
    /// process
    /// </summary>
    public class ScrapColumnConfigParser : AppTopicConfigParser
    {
        public static ILog logger = LogManager.GetLogger(typeof(ScrapColumnConfigParser));

        /// <summary>
        /// Split Manipulate tag
        /// </summary>
        private ManipulateChildFactory manipulateChildFactory;
        

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
        /// <param name="nodeIndex"></param>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        /// <param name="htmlNode"></param>
        /// <param name="fileLine"></param>
        public void Process(ColumnScrapIteratorArgs columnScrapIteratorArgs)
        {
            logger.DebugFormat("Scrapping data from online text file");
            ParseColumnsConfig(columnScrapIteratorArgs);
            ColumnScrapIterator(columnScrapIteratorArgs);
        }

        /// <summary>
        /// Parses Column element tags
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        private void ParseColumnsConfig(ColumnScrapIteratorArgs columnScrapIteratorArgs)
        {
            if (configParser.StateModel.IsColumnMetadataUpdated)
            {
                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(columnScrapIteratorArgs.Parent);
                configParser.StateModel.SetColumnMetadataFlag();
            }
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator(ColumnScrapIteratorArgs scrapArgs)
        {
            List<List<DynamicTableDataInsertModel>> rows = new List<List<DynamicTableDataInsertModel>>();
            bool doSkipDbAddUpdate = false;
            scrapArgs.PreProcess();
            for (int indx = 0; indx < scrapArgs.Parent.Columns.Count; ++indx)
            {
                // A list to store multiple column values for >1 cardinal values
                List<DynamicTableDataInsertModel> colValues = new List<DynamicTableDataInsertModel>();

                ColumnElement columnConfig = scrapArgs.Parent.Columns[indx];
                ManipulateHtmlData manipulateHtml = null;
                
                manipulateHtml = Manipulate(columnConfig, scrapArgs.GetDataIterator(columnConfig));

                if (columnConfig.IsUnique && (manipulateHtml.Results.Count <= 0))
                {
                    doSkipDbAddUpdate = true;
                    break;
                }

                foreach (var result in manipulateHtml.Results)
                {
                    DynamicTableDataInsertModel tableDataColumn = new DynamicTableDataInsertModel();

                    tableDataColumn.Name = columnConfig.Name;
                    tableDataColumn.IsPk = columnConfig.IsUnique;
                    tableDataColumn.Value = result;
                    tableDataColumn.DataType =
                        configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[scrapArgs.Parent.TableName][columnConfig.Name].DataType;

                    if (tableDataColumn.IsPk && string.IsNullOrEmpty(tableDataColumn.Value))
                        continue;
                    colValues.Add(tableDataColumn);
                }

                rows.Add(colValues);
            }

            if (!doSkipDbAddUpdate && scrapArgs.Parent.Columns.Count > 0)
            {
                configParser.Performance.NewDbUpdate(rows, scrapArgs);

                configParser.WebDbContext.AddOrUpdate(scrapArgs.Parent, rows);

                configParser.Performance.FinalDbUpdate(rows, scrapArgs);
            }
        }

        /// <summary>
        /// Manipulate the scrapped column data
        /// </summary>
        /// <param name="scrapConfig"></param>
        /// <param name="manipulateNodeList"></param>
        /// <param name="dataNode"></param>
        /// <returns></returns>
        private ManipulateHtmlData Manipulate(ColumnElement columnConfig, string scrappedData)
        {
            logger.DebugFormat("For Column '{0}' Scrapped data '{1}'", columnConfig.Name, scrappedData);

            ManipulateHtmlData result = new ManipulateHtmlData();
            result.OriginalValue = scrappedData;
            result.Results.Add(scrappedData);
            result.Cardinality = columnConfig.Cardinal;

            // Even if Scrapped data is null send to manipulation tag. As there can be a default
            if (columnConfig.Manipulations != null && columnConfig.Manipulations.Count > 0)
            {
                foreach (var manipulateChild in columnConfig.Manipulations)
                {
                    manipulateChildFactory.GetParser(manipulateChild).Process(result, manipulateChild);
                }
            }
            
            logger.DebugFormat("For Column '{0}' Final manipulated data '{1}'", columnConfig.Name, result.OriginalValue);

            return result;
        }   
    }
}

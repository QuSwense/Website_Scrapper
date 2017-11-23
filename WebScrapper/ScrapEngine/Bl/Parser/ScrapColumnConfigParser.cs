using HtmlAgilityPack;
using log4net;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using SqliteDatabase.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

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
        /// The Manipulate Config tag object parser
        /// </summary>
        protected ScrapManipulateConfigParser scrapManipulateConfigParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapColumnConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapManipulateConfigParser = new ScrapManipulateConfigParser(configParser);
        }

        /// <summary>
        /// Process Column element tag for scrapping data from Html Table
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        /// <param name="htmlNode"></param>
        public void Process(int nodeIndex, XmlNode scrapNode, 
            ScrapElement configScrap, HtmlNodeNavigator htmlNode)
        {
            logger.DebugFormat("Scrapping data from website for a Single Row.");
            ParseColumnsConfig(scrapNode, configScrap);
            ColumnScrapIterator(new ColumnScrapIteratorHtmlArgs()
            {
                NodeIndex = nodeIndex,
                ScrapConfig = configScrap,
                ScrapNode = scrapNode,
                WebHtmlNode = htmlNode
            });
        }

        /// <summary>
        /// Process Column element tag for scrapping data from csv file
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        /// <param name="htmlNode"></param>
        /// <param name="fileLine"></param>
        public void Process(int nodeIndex, XmlNode scrapNode, ScrapElement configScrap,
            HtmlNodeNavigator htmlNode, string fileLine)
        {
            logger.DebugFormat("Scrapping data from online text file");
            ParseColumnsConfig(scrapNode, configScrap);
            ColumnScrapIterator(new ColumnScrapIteratorFileArgs()
            {
                NodeIndex = nodeIndex,
                ScrapConfig = configScrap,
                ScrapNode = scrapNode,
                FileLine = fileLine
            });
        }

        /// <summary>
        /// Parses Column element tags
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="configScrap"></param>
        private void ParseColumnsConfig(XmlNode scrapNode, ScrapElement configScrap)
        {
            if (configScrap.Columns.Count <= 0)
            {
                XmlNodeList columnNodeList = scrapNode.SelectNodes(ColumnElement.TagName);

                if (columnNodeList != null && columnNodeList.Count > 0)
                {
                    foreach (XmlNode columnNode in columnNodeList)
                    {
                        configScrap.Columns.Add(
                            ParseColumnElement(columnNode, configScrap));
                    }
                }
            }

            if(configScrap.Columns.Count > 0)
                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(configScrap);
        }

        /// <summary>
        /// Parse the Column tag in config file
        /// </summary>
        /// <param name="nodeIndex"></param>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private ColumnElement ParseColumnElement(XmlNode columnNode, ScrapElement configScrap)
        {
            var webColumnConfigObj = ParseColumnElementAttributes(columnNode, configScrap);

            // Check if manipulate tag is present
            scrapManipulateConfigParser.Process(columnNode, webColumnConfigObj);

            return webColumnConfigObj;
        }

        /// <summary>
        /// Parse the Config Xml Column attribute
        /// </summary>
        /// <param name="columnNode"></param>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private ColumnElement ParseColumnElementAttributes(XmlNode columnNode, ScrapElement configScrap)
        {
            var webColumnConfigObj = 
                configParser.XmlConfigReader.ReadElement<ColumnElement>(columnNode);
            webColumnConfigObj.Parent = configScrap;
            webColumnConfigObj.Name = webColumnConfigObj.Name;

            return webColumnConfigObj;
        }

        /// <summary>
        /// This method scraps the actual data from the webpage as per the column info
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="webnodeNavigator"></param>
        private void ColumnScrapIterator<T>(T scrapArgs)
            where T : ColumnScrapIteratorArgs
        {
            List<List<DynamicTableDataInsertModel>> rows = new List<List<DynamicTableDataInsertModel>>();
            bool doSkipDbAddUpdate = false;
            scrapArgs.PreProcess();
            for (int indx = 0; indx < scrapArgs.ScrapConfig.Columns.Count; ++indx)
            {
                // A list to store multiple column values for >1 cardinal values
                List<DynamicTableDataInsertModel> colValues = new List<DynamicTableDataInsertModel>();

                ColumnElement columnConfig = scrapArgs.ScrapConfig.Columns[indx];
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
                        configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[scrapArgs.ScrapConfig.TableName][columnConfig.Name].DataType;

                    if (tableDataColumn.IsPk && string.IsNullOrEmpty(tableDataColumn.Value))
                        continue;
                    colValues.Add(tableDataColumn);
                }

                rows.Add(colValues);
            }

            if (!doSkipDbAddUpdate && scrapArgs.ScrapConfig.Columns.Count > 0)
            {
                configParser.Performance.NewDbUpdate(scrapArgs.NodeIndex.ToString());

                configParser.WebDbContext.AddOrUpdate(scrapArgs.ScrapConfig, rows);

                configParser.Performance.FinalDbUpdate(scrapArgs.NodeIndex.ToString());
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
            // manipulation defined
            scrapManipulateConfigParser.Process(columnConfig, result);

            logger.DebugFormat("For Column '{0}' Final manipulated data '{1}'", columnConfig.Name, result.OriginalValue);

            return result;
        }   
    }
}

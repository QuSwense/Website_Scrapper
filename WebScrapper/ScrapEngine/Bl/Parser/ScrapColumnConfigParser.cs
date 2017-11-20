using HtmlAgilityPack;
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

                // Load the table with partial columns in memory
                configParser.WebDbContext.AddMetadata(configScrap);
            }
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
            List<DynamicTableDataInsertModel> row = new List<DynamicTableDataInsertModel>();
            bool doSkipUpdate = false;
            scrapArgs.PreProcess();
            for (int indx = 0; indx < scrapArgs.ScrapConfig.Columns.Count; ++indx)
            {
                ColumnElement columnConfig = scrapArgs.ScrapConfig.Columns[indx];
                ManipulateHtmlData manipulateHtml = null;
                
                manipulateHtml = Manipulate(columnConfig, scrapArgs.GetDataIterator(columnConfig));

                DynamicTableDataInsertModel tableDataColumn = new DynamicTableDataInsertModel();

                if (columnConfig.IsUnique && string.IsNullOrEmpty(manipulateHtml.Value))
                {
                    doSkipUpdate = true;
                    break;
                }

                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsUnique;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.DataType =
                    configParser.WebDbContext.MetaDbConfig.TableColumnConfigs[scrapArgs.ScrapConfig.Name][columnConfig.Name].DataType;
            }

            if (!doSkipUpdate)
                configParser.WebDbContext.AddOrUpdate(scrapArgs.ScrapConfig, row);
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
            ManipulateHtmlData result = new ManipulateHtmlData();
            result.Value = scrappedData;

            scrapManipulateConfigParser.Process(columnConfig, result);
            return result;
        }   
    }
}

using DynamicDatabase.Model;
using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using WebCommon.Config;
using WebReader.Xml;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The main web site scrapper context class
    /// </summary>
    public class WebScrapHtmlContext
    {
        /// <summary>
        /// Reference to the Scrapper Engine context class
        /// </summary>
        public IScrapEngineContext EngineContext { get; protected set; }

        /// <summary>
        /// The web data rules configuration
        /// </summary>
        public WebDataConfig ScrapperRulesConfig { get; protected set; }

        /// <summary>
        /// The Html helper command class
        /// </summary>
        public HtmlScrapperCommand ScrapperCommand { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebScrapHtmlContext() { }

        /// <summary>
        /// Constructor initializes with parent engine
        /// </summary>
        /// <param name="engineContext"></param>
        public WebScrapHtmlContext(IScrapEngineContext engineContext)
        {
            EngineContext = engineContext;
        }

        /// <summary>
        /// Initialize the web scrapper html data
        /// </summary>
        public void Initialize()
        {
            using (DXmlReader reader = new DXmlReader())
                ScrapperRulesConfig = reader.Read<WebDataConfig>(ConfigPathHelper.GetScrapConfigPath(EngineContext.AppTopic));

            ScrapperCommand = new HtmlScrapperCommand();
        }

        /// <summary>
        /// Execute
        /// </summary>
        public void Run()
        {
            EngineContext.WebScrapDb.Open();

            // Loop through the instances of table to be modified
            RunMainScrap(ScrapperRulesConfig.Scraps);

            EngineContext.WebScrapDb.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scraps"></param>
        private void RunMainScrap(WebDataConfigScrap[] scraps)
        {
            // Loop through the instances of table to be modified
            foreach (WebDataConfigScrap scrapConfig in scraps)
            {
                if (scrapConfig.Type == EWebDataConfigType.TABLE)
                    UpdateUsingHtmlTable(scrapConfig);
                else if (scrapConfig.Type == EWebDataConfigType.CSV)
                    UpdateUsingCSV(scrapConfig);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scrapConfig"></param>
        private void UpdateUsingHtmlTable(WebDataConfigScrap scrapConfig)
        {
            HtmlNode htmlDoc = ScrapperCommand.LoadOnline(scrapConfig.Url);
            var navigator = (HtmlNodeNavigator)htmlDoc.CreateNavigator();
            var tableTrNodes = navigator.Select(scrapConfig.XPath);
            
            int count = 0;
            foreach (HtmlNodeNavigator node in tableTrNodes)
            {
                if (scrapConfig.Scraps != null)
                {
                    RunMainScrap(scrapConfig.Scraps);
                }
                else
                {
                    ColumnScrapIterator(count, scrapConfig,
                    (columnConfig) => Manipulate(columnConfig, node.SelectSingleNode(columnConfig.XPath)));
                }
                count++;
            }
        }

        /// <summary>
        /// For now assume that when the type='Csv" there is no Scrap child element.
        /// When it is Csv ignore other attributes except index
        /// </summary>
        /// <param name="scrapConfig"></param>
        private void UpdateUsingCSV(WebDataConfigScrap scrapConfig)
        {
            using (StreamReader reader = ScrapperCommand.LoadFile(scrapConfig.Url))
            {
                int count = 0;
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { '\t' });

                    ColumnScrapIterator(count, scrapConfig, (columnConfig) => split[columnConfig.Index]);
                    count++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnConfig"></param>
        /// <param name="dataNode"></param>
        /// <returns></returns>
        private ManipulateHtmlData Manipulate(WebDataConfigColumn columnConfig, XPathNavigator dataNode)
        {
            ManipulateHtmlData result = new ManipulateHtmlData();

            string data = "";
            if (dataNode != null) data = dataNode.Value;
            if (columnConfig.Manipulations != null)
            {
                foreach (WebDataConfigManipulate manipulate in columnConfig.Manipulations)
                {
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (manipulate.Trim != null)
                        {
                            data = data.Trim(manipulate.Trim.Data.ToCharArray());
                        }
                        if (manipulate.Splits != null)
                        {
                            foreach (WebDataConfigSplit splitConfig in manipulate.Splits)
                            {
                                string[] split = data.Split(splitConfig.Data.ToArray());
                                result.Value += split[splitConfig.Index];
                            }
                        }
                    }
                }
            }
            else
            {
                result.Value = data;
            }

            result.Url = dataNode.BaseURI;
            result.XPath = columnConfig.XPath;
            result.Value = HtmlEntity.DeEntitize(result.Value);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="scrapConfig"></param>
        /// <param name="GetValue"></param>
        private void ColumnScrapIterator(int count, WebDataConfigScrap scrapConfig, 
            Func<WebDataConfigColumn, ManipulateHtmlData> GetValue)
        {
            List<TableDataColumnModel> row = new List<TableDataColumnModel>();
            for (int indx = 0; indx < scrapConfig.Columns.Length; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                ManipulateHtmlData manipulateHtml = GetValue(columnConfig);
                TableDataColumnModel tableDataColumn = new TableDataColumnModel();
                row.Add(tableDataColumn);

                tableDataColumn.Name = columnConfig.Name;
                tableDataColumn.IsPk = columnConfig.IsPk;
                tableDataColumn.Value = manipulateHtml.Value;
                tableDataColumn.RowIndex = count;
                tableDataColumn.Url = manipulateHtml.Url;
                tableDataColumn.XPath = manipulateHtml.XPath;
            }

            EngineContext.WebScrapDb.AddOrUpdateRows(scrapConfig.Name, row);
        }
    }
}

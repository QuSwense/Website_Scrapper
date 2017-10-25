using System;
using System.Collections.Generic;
using WebScrapper.Config;
using WebScrapper.Db;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using WebScrapper.Web.Config;
using System.Xml.XPath;

namespace WebScrapper.Web
{
    /// <summary>
    /// The main Website scrapper class. This is a generic class.
    /// </summary>
    public class WebScrapperBL
    {
        /// <summary>
        /// The main application name which is same as the application name.
        /// </summary>
        private string appFolder;

        /// <summary>
        /// The dtaabase generator class
        /// </summary>
        private DbGeneratorBL dbGenerator;

        /// <summary>
        /// The application config
        /// </summary>
        private ApplicationConfig config;

        /// <summary>
        /// The web scrapper config
        /// </summary>
        private WebDataConfig scrapConfig;

        /// <summary>
        /// Constructor parameterless
        /// </summary>
        /// <param name="appFolder"></param>
        /// <param name="dbGenerator"></param>
        /// <param name="config"></param>
        /// <param name="scrapConfig"></param>
        public WebScrapperBL(string appFolder, DbGeneratorBL dbGenerator, ApplicationConfig config,
            WebDataConfig scrapConfig)
        {
            this.appFolder = appFolder;
            this.dbGenerator = dbGenerator;
            this.scrapConfig = scrapConfig;
            this.config = config;
        }

        /// <summary>
        /// Execute
        /// </summary>
        public void Run()
        {
            dbGenerator.OpenConnection();

            // Loop through the instances of table to be modified
            RunMainScrap(scrapConfig.Scraps);

            dbGenerator.CloseConnection();
        }

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

        private void UpdateUsingHtmlTable(WebDataConfigScrap scrapConfig)
        {
            HtmlNode htmlDoc = HtmlScrapperHelper.LoadOnline(scrapConfig.Url);
            var navigator = (HtmlNodeNavigator)htmlDoc.CreateNavigator();
            var tableTrNodes = navigator.Select(scrapConfig.XPath);

            //dbGenerator

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
            using (StreamReader reader = HtmlScrapperHelper.LoadFile(scrapConfig.Url))
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

        private string Manipulate(WebDataConfigColumn columnConfig, XPathNavigator dataNode)
        {
            string text = "", data = "";
            if (dataNode != null) data = dataNode.Value;
            if (columnConfig.Manipulations != null)
            {
                foreach(WebDataConfigManipulate manipulate in columnConfig.Manipulations)
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
                                text += split[splitConfig.Index];
                            }
                        }
                    }
                }
            }
            else
            {
                text = data;
            }

            return HtmlEntity.DeEntitize(text);
        }

        private void ColumnScrapIterator(int count, WebDataConfigScrap scrapConfig, Func<WebDataConfigColumn, string> GetValue)
        {
            List<TableDataColumnModel> row = new List<TableDataColumnModel>();
            for (int indx = 0; indx < scrapConfig.Columns.Length; ++indx)
            {
                WebDataConfigColumn columnConfig = scrapConfig.Columns[indx];
                row.Add(new TableDataColumnModel()
                {
                    Name = columnConfig.Name,
                    IsPk = columnConfig.IsPk,
                    Value = GetValue(columnConfig),
                    RowIndex = count
                });
            }

            dbGenerator.SaveOrUpdate(scrapConfig.Name, row);
        }
    }
}
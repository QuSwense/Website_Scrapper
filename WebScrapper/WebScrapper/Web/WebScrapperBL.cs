using System;
using System.Collections.Generic;
using WebScrapper.Config;
using WebScrapper.Db;
using System.Linq;
using HtmlAgilityPack;
using System.IO;
using WebScrapper.Web.Config;

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
                if (scrapConfig.Type == EWebDataConfigType.Table)
                    UpdateUsingHtmlTable(scrapConfig);
                else if (scrapConfig.Type == EWebDataConfigType.Csv)
                    UpdateUsingCSV(scrapConfig);
            }
        }

        private void UpdateUsingHtmlTable(WebDataConfigScrap scrapConfig)
        {
            HtmlNode htmlDoc = HtmlScrapperHelper.Load(scrapConfig.Url);

            if(scrapConfig.Scraps != null)
            {

            }
            HtmlNodeCollection tableTrNodes = htmlDoc.SelectNodes(scrapConfig.XPath);

            foreach (HtmlNode node in tableTrNodes)
            {
                ColumnScrapIterator(scrapConfig,
                    (columnConfig) => Manipulate(columnConfig, node.SelectSingleNode(columnConfig.xpath).InnerText));
            }
        }

        private void UpdateUsingConfig(WebDataConfigScrap scrapConfig)
        {

        }

        

        private void UpdateUsingTableRef(WebDataConfigScrap scrapConfig)
        {
            HtmlNode htmlDoc = HtmlScrapperHelper.Load(scrapConfig.Url);
            HtmlNodeCollection hrefCollection = htmlDoc.SelectNodes(scrapConfig.XPath);

            foreach(HtmlNode node in hrefCollection)
            {
                // Get full url
                Uri baseUrl = new Uri(scrapConfig.Url);
                Uri hrefUrl = new Uri(baseUrl, node.InnerText);
                HtmlNode htmlDoc1 = HtmlScrapperHelper.Load(hrefUrl.AbsoluteUri);

                if (!string.IsNullOrEmpty(scrapConfig.XPath))
                {
                    HtmlNodeCollection href1Collection = htmlDoc1.SelectNodes(scrapConfig.XPath);
                    foreach (HtmlNode node1 in href1Collection)
                    {
                        // Get full url
                        Uri baseUrl1 = new Uri(scrapConfig.Url);
                        Uri hrefUrl1 = new Uri(baseUrl, node.InnerText);
                        HtmlNode htmlDoc2 = HtmlScrapperHelper.Load(hrefUrl.AbsoluteUri);
                    }
                }
                else
                ColumnScrapIterator(scrapConfig,
                    (columnConfig) => Manipulate(columnConfig, htmlDoc.SelectSingleNode(columnConfig.xpath).InnerText));
            }
        }

        private void UpdateUsingCSV(WebDataConfigScrap scrapConfig)
        {
            using (StreamReader reader = HtmlScrapperHelper.LoadFile(scrapConfig.Url))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { '\t' });

                    ColumnScrapIterator(scrapConfig, (columnConfig) => split[columnConfig.index]);
                }
            }
        }

        private string Manipulate(WebDataConfigColumn columnConfig, string data)
        {
            string text = data;
            if (columnConfig.Manipulate != null)
            {
                if (columnConfig.Manipulate.Split != null)
                {
                    string[] split = data.Split(columnConfig.Manipulate.Split.data.ToArray());
                    text = split[columnConfig.Manipulate.Split.index];
                }
            }

            return text;
        }

        private void ColumnScrapIterator(ScrapWebDataConfigScrap tableScrapConfig, Func<ScrapWebDataConfigScrapColumn, string> getValue)
        {
            Dictionary<string, ColumnScrapModel> rowData = new Dictionary<string, ColumnScrapModel>();
            for (int indx = 0; indx < tableScrapConfig.Column.Length; ++indx)
            {
                ScrapWebDataConfigScrapColumn columnConfig = tableScrapConfig.Column[indx];
                ColumnScrapModel colScrap = new ColumnScrapModel();
                colScrap.IsPk = Convert.ToBoolean(columnConfig.ispk);
                colScrap.Value = getValue(columnConfig);
                rowData.Add(columnConfig.name, colScrap);
            }

            dbGenerator.AddRow(tableScrapConfig.name, rowData);
        }
    }
}
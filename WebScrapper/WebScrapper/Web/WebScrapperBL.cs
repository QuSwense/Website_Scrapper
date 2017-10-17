using System;
using System.Collections.Generic;
using WebScrapper.Config;
using WebScrapper.Db;
using System.Linq;
using HtmlAgilityPack;
using System.IO;

namespace WebScrapper.Web
{
    public class WebScrapperBL
    {
        private string appFolder;
        private DbGeneratorBL dbGenerator;
        private AppConfig config;
        private ScrapWebDataConfig scrapConfig;

        public WebScrapperBL(string appFolder, DbGeneratorBL dbGenerator, AppConfig config,
            ScrapWebDataConfig scrapConfig)
        {
            this.appFolder = appFolder;
            this.dbGenerator = dbGenerator;
            this.scrapConfig = scrapConfig;
            this.config = config;
        }

        public void Run()
        {
            dbGenerator.OpenConnection();

            // Loop through the instances of table to be modified
            foreach (ScrapWebDataConfigScrap tableScrapConfig in scrapConfig.Scrap)
            {
                if (tableScrapConfig.type == "table")
                    UpdateUsingHtmlTable(tableScrapConfig);
                else if (tableScrapConfig.type == "csv")
                    UpdateUsingCSV(tableScrapConfig);
                else if (tableScrapConfig.type == "tableref")
                    UpdateUsingTableRef(tableScrapConfig);
            }

            dbGenerator.CloseConnection();
        }

        private void UpdateUsingConfig(ScrapWebDataConfigScrap tableScrapConfig)
        {

        }

        private void UpdateUsingHtmlTable(ScrapWebDataConfigScrap tableScrapConfig)
        {
            HtmlNode htmlDoc = HtmlScrapperHelper.Load(tableScrapConfig.url);
            HtmlNodeCollection tableTrNodes = htmlDoc.SelectNodes(tableScrapConfig.xpath);

            foreach (HtmlNode node in tableTrNodes)
            {
                ColumnScrapIterator(tableScrapConfig,
                    (columnConfig) => Manipulate(columnConfig, node.SelectSingleNode(columnConfig.xpath).InnerText));
            }
        }

        private void UpdateUsingTableRef(ScrapWebDataConfigScrap tableScrapConfig)
        {
            HtmlNode htmlDoc = HtmlScrapperHelper.Load(tableScrapConfig.url);
            HtmlNodeCollection hrefCollection = htmlDoc.SelectNodes(tableScrapConfig.xpath);

            foreach(HtmlNode node in hrefCollection)
            {
                // Get full url
                Uri baseUrl = new Uri(tableScrapConfig.url);
                Uri hrefUrl = new Uri(baseUrl, node.InnerText);
                HtmlNode htmlDoc1 = HtmlScrapperHelper.Load(hrefUrl.AbsoluteUri);

                if (!string.IsNullOrEmpty(tableScrapConfig.xpath2))
                {
                    HtmlNodeCollection href1Collection = htmlDoc1.SelectNodes(tableScrapConfig.xpath2);
                    foreach (HtmlNode node1 in href1Collection)
                    {
                        // Get full url
                        Uri baseUrl1 = new Uri(tableScrapConfig.url);
                        Uri hrefUrl1 = new Uri(baseUrl, node.InnerText);
                        HtmlNode htmlDoc2 = HtmlScrapperHelper.Load(hrefUrl.AbsoluteUri);
                    }
                }
                else
                ColumnScrapIterator(tableScrapConfig,
                    (columnConfig) => Manipulate(columnConfig, htmlDoc.SelectSingleNode(columnConfig.xpath).InnerText));
            }
        }

        private void UpdateUsingCSV(ScrapWebDataConfigScrap tableScrapConfig)
        {
            using (StreamReader reader = HtmlScrapperHelper.LoadFile(tableScrapConfig.url))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { '\t' });

                    ColumnScrapIterator(tableScrapConfig, (columnConfig) => split[columnConfig.index]);
                }
            }
        }

        private string Manipulate(ScrapWebDataConfigScrapColumn columnConfig, string data)
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
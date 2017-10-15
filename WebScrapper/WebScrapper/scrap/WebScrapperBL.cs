using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScrapper.common;
using WebScrapper.config;
using WebScrapper.db;

namespace WebScrapper.scrap
{
    public class WebScrapperBL
    {
        private string appFolder;
        private DbGeneratorBL dbGenerator;
        private AppConfig config;
        private ScrapWebDataConfig scrapConfig;

        public WebScrapperBL(string appFolder, DbGeneratorBL dbGenerator, AppConfig config, ScrapWebDataConfig scrapConfig)
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
                else if(tableScrapConfig.type == "csv")
                    UpdateUsingCSV(tableScrapConfig);
                else if (tableScrapConfig.type == "tableref")
                    UpdateUsingTableRef(tableScrapConfig);
            }
            dbGenerator.CloseConnection();
        }

        private void UpdateUsingTableRef(ScrapWebDataConfigScrap tableScrapConfig)
        {
            TableDbConfigModel tableConfig = dbGenerator.DbConfig.GetTable(tableScrapConfig.name);

            HtmlNode htmlDoc = HtmlScrapperHelper.Load(tableScrapConfig.url);
            string href = htmlDoc.SelectSingleNode(tableScrapConfig.xpath).InnerText;

            // Get full url
            Uri baseUrl = new Uri(tableScrapConfig.url);
            Uri hrefUrl = new Uri(baseUrl, href);
            htmlDoc = HtmlScrapperHelper.Load(hrefUrl.AbsoluteUri);

            //foreach (HtmlNode node in tableTrNodes)
            //{
            //    Dictionary<string, ColumnScrapModel> rowData = new Dictionary<string, ColumnScrapModel>();
            //    for (int indx = 0; indx < tableScrapConfig.Column.Length; ++indx)
            //    {
            //        ScrapWebDataConfigScrapColumn columnConfig = tableScrapConfig.Column[indx];
            //        ColumnScrapModel colScrap = new ColumnScrapModel();
            //        colScrap.IsPk = Convert.ToBoolean(columnConfig.ispk);
            //        colScrap.Value = Manipulate(columnConfig, node.SelectSingleNode(columnConfig.xpath).InnerText);
            //        rowData.Add(columnConfig.name, colScrap);
            //    }

            //    dbGenerator.AddRow(tableScrapConfig.name, rowData);
            //}
        }

        private void UpdateUsingCSV(ScrapWebDataConfigScrap tableScrapConfig)
        {
            TableDbConfigModel tableConfig = dbGenerator.DbConfig.GetTable(tableScrapConfig.name);
            
            using (StreamReader reader = HtmlScrapperHelper.LoadFile(tableScrapConfig.url))
            {
                string line = "";
                while((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(new char[] { '\t' });

                    Dictionary<string, ColumnScrapModel> rowData = new Dictionary<string, ColumnScrapModel>();
                    for (int indx = 0; indx < tableScrapConfig.Column.Length; ++indx)
                    {
                        ScrapWebDataConfigScrapColumn columnConfig = tableScrapConfig.Column[indx];
                        ColumnScrapModel colScrap = new ColumnScrapModel();
                        colScrap.IsPk = Convert.ToBoolean(columnConfig.ispk);
                        colScrap.Value = split[columnConfig.index];
                        rowData.Add(columnConfig.name, colScrap);
                    }

                    dbGenerator.AddRow(tableScrapConfig.name, rowData);
                }
            }
        }

        private void UpdateUsingHtmlTable(ScrapWebDataConfigScrap tableScrapConfig)
        {
            TableDbConfigModel tableConfig = dbGenerator.DbConfig.GetTable(tableScrapConfig.name);

            HtmlNode htmlDoc = HtmlScrapperHelper.Load(tableScrapConfig.url);
            HtmlNodeCollection tableTrNodes = htmlDoc.SelectNodes(tableScrapConfig.xpath);

            foreach (HtmlNode node in tableTrNodes)
            {
                Dictionary<string, ColumnScrapModel> rowData = new Dictionary<string, ColumnScrapModel>();
                for (int indx = 0; indx < tableScrapConfig.Column.Length; ++indx)
                {
                    ScrapWebDataConfigScrapColumn columnConfig = tableScrapConfig.Column[indx];
                    ColumnScrapModel colScrap = new ColumnScrapModel();
                    colScrap.IsPk = Convert.ToBoolean(columnConfig.ispk);
                    colScrap.Value = Manipulate(columnConfig, node.SelectSingleNode(columnConfig.xpath).InnerText);
                    rowData.Add(columnConfig.name, colScrap);
                }

                dbGenerator.AddRow(tableScrapConfig.name, rowData);
            }
        }

        private string Manipulate(ScrapWebDataConfigScrapColumn columnConfig, string data)
        {
            string text = data;
            if(columnConfig.Manipulate != null)
            {
                if(columnConfig.Manipulate.Split != null)
                {
                    string[] split = data.Split(columnConfig.Manipulate.Split.data.ToArray());
                    text = split[columnConfig.Manipulate.Split.index];
                }
            }

            return text;
        }
    }
}

using HtmlAgilityPack;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteScrapper.CountryApp;
using WebsiteScrapper.Model;

namespace WebsiteScrapper
{
    public class CountryAppEngine
    {
        public static ILog logger = LogManager.GetLogger(typeof(HtmlScrapperHelper));

        private List<HtmlDocument> htmlDocs;

        public ISO3166CountryCodeModel ISO3166CountryCode { get; set; }
        public UNSDModel UNSD { get; set; }

        public CountryAppEngine() { }

        public void FetchISO3166Alpha2()
        {
            ISO3166CountryCode = new ISO3166CountryCodeModel()
            {
                Url = "https://www.iso.org/iso-3166-country-codes.html"
            };

            HtmlNode isoMainDocument = ISO3166CountryCode.Load();

            ISO3166CountryCode.WhatisISO3166 = HtmlScrapperHelper.FetchSingleParagraph(isoMainDocument, "//body/div[1]/div[2]//div[2]/p[1]", "What is ISO 3166?");
            ISO3166CountryCode.ISO3166CountryCode = HtmlScrapperHelper.FetchSingleParagraph(isoMainDocument, "//body/div[1]/div[3]//div[2]/p[4]", "ISO 3166-1:2013 country codes");

            ISO3166CountryCode.ISO31661Alpha2 = new ISO31661Alpha2Model()
            {
                Url = "https://en.wikipedia.org/wiki/ISO_3166-1_alpha-2"
            };

            HtmlNode iso31661a2WikiDocument = ISO3166CountryCode.ISO31661Alpha2.Load();
            HtmlNode iso31661a2WikiOfficialTable = iso31661a2WikiDocument.SelectSingleNode(
                "//div[@class='mw-parser-output']/table[3]");

            ISO3166CountryCode.ISO31661Alpha2.ISO31661Alpha2OfficialTable =
                HtmlScrapperHelper.FetchTable(iso31661a2WikiOfficialTable, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[2]", null, " Link" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[4]", null, " Link" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]", "id" },
                new string[] { "td[2]/a", "title" },
                new string[] { "td[2]/a", "href" },
                new string[] { "td[3]" },
                new string[] { "td[4]/a", "title" },
                new string[] { "td[4]/a", "href" },
                }
                );

            List<ScrapString> headers = ISO3166CountryCode.ISO31661Alpha2.ISO31661Alpha2OfficialTable.Headers;
            for (int indx = 0; indx < headers.Count; ++indx)
            {
                headers[indx].Reference
                = HtmlScrapperHelper.FetchSingleParagraph(isoMainDocument,
                    string.Format("//div[@class='mw-parser-output']/ul[1]/li[{0}]", indx))
                .Remove(headers[indx].Value).Remove("—");
            }

            HtmlNode iso31661a2WikiExceptionalTable = iso31661a2WikiDocument.SelectSingleNode(
                "//div[@class='mw-parser-output']/table[4]");

            ISO3166CountryCode.ISO31661Alpha2.ISO31661Alpha2ExceptionalTable =
                HtmlScrapperHelper.FetchTable(iso31661a2WikiExceptionalTable, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[2]", null, " Link" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[3]", null, " Link" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[4]", null, " Link" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]", "id" },
                new string[] { "td[2]/a", "title" },
                new string[] { "td[2]/a", "href" },
                new string[] { "td[3]/a", "title" },
                new string[] { "td[3]/a", "href" },
                new string[] { "td[4]/a", "title" },
                new string[] { "td[4]/a", "href" },
                }
                );
        }

        public void FetchUNSD()
        { 
            UNSD = new UNSDModel()
            {
                Url = "https://unstats.un.org/unsd/methodology/m49/"
            };

            HtmlNode unsdPageNode = UNSD.Load();

            UNSD.Reference = HtmlScrapperHelper.FetchSingleSentence(unsdPageNode, 
                "//div[@id='body']/section[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/h5[1]/em[1]",
                "standard.",
                "UNSD Methodology");

            HtmlNode unsdGeoGroupsENGNode = unsdPageNode.SelectSingleNode("//table[@id='GeoGroupsENG']");

            UNSD.UNSDGeoRegions = HtmlScrapperHelper.FetchTable(unsdGeoGroupsENGNode, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[2]", null, null, "Parent " },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]" },
                new string[] { ".", "data-tt-id" }, // tr node
                new string[] { ".", "data-tt-parent-id" }, // tr node
                new string[] { "td[3]" },
                new string[] { "td[4]" }
                }
                );
        }
    }
}

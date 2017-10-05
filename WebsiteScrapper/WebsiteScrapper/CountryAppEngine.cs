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
        public CIAFactbookModel CIAFactbook { get; set; }

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

        public void FetchCIAFactbook()
        {
            CIAFactbook = new CIAFactbookModel()
            {
                Url = "https://www.cia.gov/library/publications/the-world-factbook/"
            };

            CIAFactbook.CountriesGeography = new List<CIAWorldFactbookCountryGeographyModel>();
            CIAFactbook.CountriesPeopleAndSociety = new List<CIAWorldFactbookCountryPeopleAndSocietyModel>();
            CIAFactbook.CountriesGovernment = new List<CIAWorldFactbookCountryGovernmentModel>();

            HtmlNode countrySelectionNode = CIAFactbook.Load();
            HtmlNodeCollection countrySelectionNodes = countrySelectionNode.SelectNodes("div[@id='cntrySelect']//option[position() > 2]");

            for (int indx = 0; indx < countrySelectionNodes.Count; ++indx)
            {
                string countryName = countrySelectionNodes[indx].InnerText;
                Uri countryUrl;
                Uri.TryCreate(new Uri(CIAFactbook.Url), countrySelectionNodes[indx].Attributes["value"].Value, out countryUrl);

                CIAWorldFactbookCountryGeographyModel countryGeo = new CIAWorldFactbookCountryGeographyModel()
                {
                    Url = countryUrl.AbsoluteUri,
                    XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='Geography']]/following-sibling::li"
                };

                HtmlNode countryPageNode = countryGeo.Load();
                HtmlNode countryGeoNode = countryPageNode.SelectSingleNode(countryGeo.XPath);

                countryGeo.Location = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Location:')]/following-sibling::div[@class='category_data']");
                countryGeo.Coordinates = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Geographic coordinates:')]/following-sibling::div[@class='category_data']");
                countryGeo.TotalArea = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'total:')]/following-sibling::span");
                countryGeo.LandArea = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'land:')]/following-sibling::span");
                countryGeo.WaterArea = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'water:')]/following-sibling::span");
                countryGeo.CountryComparisionByAreaRank = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'country comparison to the world:')]/following-sibling::span");
                countryGeo.TotalLandBoundaries = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Land boundaries:')]/following-sibling::div/span[contains(text(), 'total:')]/following-sibling::span");
                countryGeo.BorderCountries = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Land boundaries:')]/following-sibling::div/span[contains(text(), 'border countries')]/following-sibling::span");
                countryGeo.Coastline = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Coastline:')]/following-sibling::div");
                countryGeo.MeanElevation = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'mean elevation:')]/following-sibling::span");
                countryGeo.LowestPoint = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'lowest point:')]");
                countryGeo.HighestPoint = HtmlScrapperHelper.FetchSingleText(countryGeoNode,
                    "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'highest point:')]");
                CIAFactbook.CountriesGeography.Add(countryGeo);

                CIAWorldFactbookCountryPeopleAndSocietyModel countryPeopleAndSociety = new CIAWorldFactbookCountryPeopleAndSocietyModel()
                {
                    Url = countryUrl.AbsoluteUri,
                    XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='People and Society']]/following-sibling::li"
                };

                HtmlNode countryPeopleAndSocietyNode = countryPageNode.SelectSingleNode(countryPeopleAndSociety.XPath);

                countryPeopleAndSociety.NationalityNoun = HtmlScrapperHelper.FetchSingleText(countryPeopleAndSocietyNode,
                    "div[@id='field' and contains(text(), 'Population:')]/following-sibling::div[@class='category_data']");
                countryPeopleAndSociety.NationalityAdjective = HtmlScrapperHelper.FetchSingleText(countryPeopleAndSocietyNode,
                    "div[@id='field' and contains(text(), 'Nationality:')]/following-sibling::div/span[contains(text(), 'adjective:')]/following-sibling::span");
                CIAFactbook.CountriesPeopleAndSociety.Add(countryPeopleAndSociety);

                CIAWorldFactbookCountryGovernmentModel countryGov = new CIAWorldFactbookCountryGovernmentModel()
                {
                    Url = countryUrl.AbsoluteUri,
                    XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='Government']]/following-sibling::li"
                };

                HtmlNode countryGovNode = countryPageNode.SelectSingleNode(countryPeopleAndSociety.XPath);

                countryGov.CountryConventionalLongName = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'conventional long form')]/following-sibling::span");


                countryGov.CountryConventionalShortName = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:');/following-sibling::div/span[contains(text(), 'conventional short form');/following-sibling::span");


                countryGov.CountryLocalLongForm = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:');/following-sibling::div/span[contains(text(), 'local long form');/following-sibling::span");


                countryGov.CountryLocalShortForm = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:');/following-sibling::div/span[contains(text(), 'local short form');/following-sibling::span");


                countryGov.CountryFormer = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:');/following-sibling::div/span[contains(text(), 'former');/following-sibling::span");


                countryGov.CountryEtymology = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Country name:');/following-sibling::div/span[contains(text(), 'etymology');/following-sibling::span");


                countryGov.GovernmentType = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Government type:');/following-sibling::div[@class='category_data']");


                countryGov.CapitalName = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Capital:');/following-sibling::div/span[contains(text(), 'name');/following-sibling::span");


                countryGov.CapitalCoordinates = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Capital:');/following-sibling::div/span[contains(text(), 'geographic coordinates');/following-sibling::span");


                countryGov.TimeDifference = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Capital:');/following-sibling::div/span[contains(text(), 'time difference');/following-sibling::span");


                countryGov.AdministrativeDivisions = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Administrative divisions:');/following-sibling::div[@class='category_data']");


                countryGov.Independence = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Independence:');/following-sibling::div[@class='category_data']");

                countryGov.ChiefofState = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Executive branch:');/following-sibling::div/span[contains(text(), 'chief of state:');/following-sibling::span");


                countryGov.HeadOfGovernment = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Executive branch:');/following-sibling::div/span[contains(text(), 'head of government:');/following-sibling::span");


                countryGov.Cabinet = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Executive branch:');/following-sibling::div/span[contains(text(), 'cabinet:');/following-sibling::span");


                countryGov.InternationalOrganizationParticipation = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'International organization participation:');/following-sibling::div[@class='category_data']");


                countryGov.DiplomaticInUSChiefofMission = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation in the US:');/following-sibling::div/span[contains(text(), 'chief of mission:');/following-sibling::span");


                countryGov.DiplomaticInUSChancery = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation in the US:');/following-sibling::div/span[contains(text(), 'chancery:');/following-sibling::span");


                countryGov.DiplomaticInUSTelephone = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation in the US:');/following-sibling::div/span[contains(text(), 'telephone:');/following-sibling::span");


                countryGov.DiplomaticInUSFAX = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation in the US:');/following-sibling::div/span[contains(text(), 'FAX:');/following-sibling::span");


                countryGov.DiplomaticInUSConsulateGeneral = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation in the US:');/following-sibling::div/span[contains(text(), 'consulate(s) general:');/following-sibling::span");


                countryGov.DiplomaticFromUSChiefofMission = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation from the US:');/following-sibling::div/span[contains(text(), 'chief of mission:');/following-sibling::span");


                countryGov.DiplomaticFromUSEmbassy = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation from the US:');/following-sibling::div/span[contains(text(), 'embassy:');/following-sibling::span");


                countryGov.DiplomaticFromUSMailingAddress = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation from the US:');/following-sibling::div/span[contains(text(), 'mailing address:');/following-sibling::span");


                countryGov.DiplomaticFromUSTelephone = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation from the US:');/following-sibling::div/span[contains(text(), 'telephone:');/following-sibling::span");


                countryGov.DiplomaticFromUSFAX = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'Diplomatic representation from the US:');/following-sibling::div/span[contains(text(), 'FAX:');/following-sibling::span");


                countryGov.NationalAnthemName = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'National anthem:');/following-sibling::div/span[contains(text(), 'name:');/following-sibling::span");


                countryGov.NationalAnthemLyricsMusic = HtmlScrapperHelper.FetchSingleText(countryGovNode, "div[@id='field' and contains(text(), 'National anthem:');/following-sibling::div/span[contains(text(), 'lyrics/music:');/following-sibling::span");

                CIAFactbook.CountriesGovernment.Add(countryGov);
            }
        }
    }
}

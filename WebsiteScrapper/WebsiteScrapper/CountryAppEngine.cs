using HtmlAgilityPack;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebsiteScrapper.CountryApp;
using WebsiteScrapper.Model;

namespace WebsiteScrapper
{
    public class CountryAppEngine
    {
        public static ILog logger = LogManager.GetLogger(typeof(HtmlScrapperHelper));

        public ISO3166CountryCodeModel ISO3166CountryCode { get; set; }
        public UNSDModel UNSD { get; set; }
        public CIAFactbookModel CIAFactbook { get; set; }
        public QuickgsLegislaturesModel CountriesLegislature { get; set; }
        public ScrapObject<List<WikiCurrentHeadOfStatesAndGovModel>> WikiCurrentHeadOfStatesAndGov { get; set; }
        public ScrapObject<List<UNDataCountryProfileModel>> UNDataList { get; set; }
        public GeoNamesDatabaseModel GeoNamesDatabase { get; set; }
        public WikiCountryCapitalsModel WikiCountryCapitals { get; set; }

        public CountryAppEngine() { }

        public void Parse()
        {
            FetchISO3166Alpha2();
            FetchUNSD();
            FetchCIAFactbook();
            FetchISOCountryDetails();
        }

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
            CIAFactbook.CountriesTransportation = new List<CIAFactbookTransportationModel>();

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

                CIAFactbookTransportationModel countryTransport = new CIAFactbookTransportationModel()
                {
                    Url = countryUrl.AbsoluteUri,
                    XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='Transportation']]/following-sibling::li"
                };

                HtmlNode countryTransportNode = countryPageNode.SelectSingleNode(countryTransport.XPath);

                countryTransport.NoOfRegisteredAirCarriers = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'National air transport system:')]/following-sibling::div/span[contains(text(), 'number of registered air carriers')]/following-sibling::span");
                countryTransport.InventoryOfRegisteredAircraftOperatedByAirCarriers = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'National air transport system:')]/following-sibling::div/span[contains(text(), 'inventory of registered aircraft operated by air carriers')]/following-sibling::span");
                countryTransport.CivilAircraftRegCountryCodePrefix = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'Civil aircraft registration country code prefix:')]/following-sibling::div");
                countryTransport.Airports = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'Airports:')]/following-sibling::div");
                countryTransport.AirportsWithPavedRunWaysTotal = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'Airports - with paved runways:')]/following-sibling::div/span[contains(text(), 'total')]/following-sibling::span");
                countryTransport.AirportsWithPavedRunWays = new List<ScrapString>();

                HtmlNodeCollection countryAirportsWIthPavedNodes = countryTransportNode.SelectNodes("div[@id='field' and contains(text(), 'Airports - with paved runways:')]/following-sibling::div[position() > 1 and preceding-sibling::div[@id='field']]");

                for(int indx1 = 0; indx1 < countryAirportsWIthPavedNodes.Count; ++indx1)
                {
                    ScrapString scrapObj = HtmlScrapperHelper.FetchSingleText(countryAirportsWIthPavedNodes[indx1], "span[2]");
                    scrapObj.Description = HtmlScrapperHelper.FetchSingleValue(countryAirportsWIthPavedNodes[indx1], "span[1]")
                        .Replace(":", "");
                    countryTransport.AirportsWithPavedRunWays.Add(scrapObj);
                }

                countryTransport.AirportsWithUnPavedRunWays = new List<ScrapString>();

                HtmlNodeCollection countryAirportsWithUnPavedNodes = countryTransportNode.SelectNodes("div[@id='field' and contains(text(), 'Airports - with unpaved runways:')]/following-sibling::div[position() > 1 and preceding-sibling::div[@id='field']]");

                for (int indx1 = 0; indx1 < countryAirportsWithUnPavedNodes.Count; ++indx1)
                {
                    ScrapString scrapObj = HtmlScrapperHelper.FetchSingleText(countryAirportsWIthPavedNodes[indx1], "span[2]");
                    scrapObj.Description = HtmlScrapperHelper.FetchSingleValue(countryAirportsWIthPavedNodes[indx1], "span[1]")
                        .Replace(":", "");
                    countryTransport.AirportsWithUnPavedRunWays.Add(scrapObj);
                }

                countryTransport.Heliports = HtmlScrapperHelper.FetchSingleText(countryTransportNode, "div[@id='field' and contains(text(), 'Heliports:')]/following-sibling::div");
            }
        }

        public void FetchISOCountryDetails()
        {
            ISO3166CountryCode.ISO3166CountryDetails = new List<ISO3166CountryDetailsModel>();
            ScrapTable scrapTableOfficialCOdes = ISO3166CountryCode.ISO31661Alpha2.ISO31661Alpha2OfficialTable;

            ISO3166CountryDetailsModel countryDetailsFirstRow = new ISO3166CountryDetailsModel()
            {
                Url = @"https://www.iso.org/obp/ui/#iso:code:3166:" + scrapTableOfficialCOdes.FirstRow[0].Value
            };

            HtmlNode firstRowNode = countryDetailsFirstRow.Load();

            FetchISOCountryDetails(firstRowNode, countryDetailsFirstRow);

            for (int indx = 0; indx < scrapTableOfficialCOdes.Value.Count; ++indx)
            {

            }
        }

        private void FetchISOCountryDetails(HtmlNode node, ISO3166CountryDetailsModel countryDetails)
        {
            if (node == null) throw new Exception("ISO Country Detail node not found");

            countryDetails.FullName = HtmlScrapperHelper.FetchSingleText(node,
                "//div[@class='core-view-summary']//div[@class='core-view-field-name' and contains(text(), 'Full name')]/following-sibling::div");
            countryDetails.Independent = HtmlScrapperHelper.FetchSingleText(node,
                "//div[@class='core-view-summary']//div[@class='core-view-field-name' and contains(text(), 'Independent')]/following-sibling::div");
            countryDetails.TerritoryName = HtmlScrapperHelper.FetchSingleText(node,
                "//div[@class='core-view-summary']//div[@class='core-view-field-name' and contains(text(), 'Territory name')]/following-sibling::div");

            HtmlNode addInfoNode = node.SelectSingleNode("//div[@class='country-additional-info']/table");

            if (addInfoNode == null)
                logger.Info("No Additional Info Table present");
            else
            {
                countryDetails.AdditionalInfos = HtmlScrapperHelper.FetchTable(addInfoNode, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]" },
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                }
                );
            }

            HtmlNode subdivisionsNode = node.SelectSingleNode("//div[@class='subdivision']/table");

            if (subdivisionsNode == null)
                logger.Info("No Subdivisions Table present");
            else
            {
                countryDetails.Subdivisions = HtmlScrapperHelper.FetchTable(subdivisionsNode, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[5]" },
                new string[] { "tr[1]/th[6]" },
                new string[] { "tr[1]/th[7]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]" },
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                new string[] { "td[4]" },
                new string[] { "td[5]" },
                new string[] { "td[6]" },
                new string[] { "td[7]" },
                }
                );
            }
        }

        private void FetchQuickgsLegislature()
        {
            CountriesLegislature = new QuickgsLegislaturesModel()
            {
                Url = "http://www.quickgs.com/parliament-names-of-different-countries/"
            };

            HtmlNode quickgsLegislatureNode = CountriesLegislature.Load();

            CountriesLegislature.AsiaOceaniaLegislatures = FetchQuickgsLegislature(
                quickgsLegislatureNode.SelectSingleNode("//table[@id='tablepress-38']"));

            CountriesLegislature.EuropeLegislatures = FetchQuickgsLegislature(
                quickgsLegislatureNode.SelectSingleNode("//table[@id='tablepress-40']"));

            CountriesLegislature.AmericaLegislatures = FetchQuickgsLegislature(
                quickgsLegislatureNode.SelectSingleNode("//table[@id='tablepress-41']"));

            CountriesLegislature.AfricaLegislatures = FetchQuickgsLegislature(
                quickgsLegislatureNode.SelectSingleNode("//table[@id='tablepress-42']"));
        }

        private ScrapTable FetchQuickgsLegislature(HtmlNode nodeTable)
        {
            return
               HtmlScrapperHelper.FetchTable(nodeTable, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[5]" },
                new string[] { "tr[1]/th[6]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]" },
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                new string[] { "td[4]" },
                new string[] { "td[5]" },
                new string[] { "td[6]" },
                }
                );
        }
        
        private void FetchWikiHeadOfStatesAndGov()
        {
            WikiCurrentHeadOfStatesAndGov = new ScrapObject<List<WikiCurrentHeadOfStatesAndGovModel>>()
            {
                Url = "https://en.wikipedia.org/wiki/List_of_current_heads_of_state_and_government"
            };

            HtmlNode wikiheadOfStateNode = WikiCurrentHeadOfStatesAndGov.Load();
            HtmlNodeCollection headOfStatesRows = wikiheadOfStateNode.SelectNodes("//div[@id='mw-content-text']/div[1]/table[1]//tr[position() > 1]");

            for(int indx = 0; indx < headOfStatesRows.Count; indx++)
            {
                WikiCurrentHeadOfStatesAndGovModel modelObj = new WikiCurrentHeadOfStatesAndGovModel();

                modelObj.CountryName = HtmlScrapperHelper.FetchSingleText(headOfStatesRows[indx], "th/a", "title");

                string headOfStates = headOfStatesRows[indx].SelectSingleNode("td[1]").InnerHtml;
                ParseHeadOfStates(headOfStates, modelObj.HeadOfStates);

                string headOfGovs = headOfStatesRows[indx].SelectSingleNode("td[1]").InnerHtml;
                ParseHeadOfStates(headOfGovs, modelObj.HeadOfGovs);

                WikiCurrentHeadOfStatesAndGov.Value.Add(modelObj);
            }
        }

        private void ParseHeadOfStates(string headOfStates, List<WikiHOSCommonHead> rootNode)
        {
            string[] lines = headOfStates.Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries);

            for(int indx = 0; indx < lines.Length; ++indx)
            {
                WikiHOSCommonHead headObj = new WikiHOSCommonHead();

                if (lines[indx].Contains("<small>"))
                    headObj.IsTransitional = true;

                XElement reader = new XElement(lines[indx]);
                var linkTags = reader.Elements("a").ToList();

                headObj.Type = linkTags[0].Attribute("title").Value;
                headObj.TypeUrl = "https://en.wikipedia.org" + linkTags[0].Attribute("href").Value;
                headObj.CurrentPerson = linkTags[0].Attribute("title").Value;
                headObj.CurrentPersonUrl = "https://en.wikipedia.org" + linkTags[0].Attribute("href").Value;

                // Find text beween 'a' tags
                Match match = Regex.Match(lines[indx], "</a>(.*)<a>");
                if(match != null && match.Success)
                {
                    string value = match.Value.Trim();

                    if (!string.IsNullOrEmpty(value))
                    {
                        string[] parse = value.Split(new char[] { '–' }).Select(x => x.Trim()).ToArray();

                        if(!string.IsNullOrEmpty(parse[0]))
                            headObj.Type += ' ' + parse[0].Trim();

                        if (!string.IsNullOrEmpty(parse[1]))
                            headObj.CurrentPerson = parse[1] + ' ' + headObj.CurrentPerson;
                    }
                }

                rootNode.Add(headObj);
            }
        }

        private void FetchUNDataList()
        {
            UNDataList = new ScrapObject<List<UNDataCountryProfileModel>>()
            {
                Url = "http://data.un.org/CountryProfile.aspx"
            };

            HtmlNode countryUnDataPageNode = UNDataList.Load();
            HtmlNodeCollection countryListNodes = countryUnDataPageNode.SelectNodes("//div[@id='divCountryList']/a");

            for(int indx = 0; indx < countryListNodes.Count; ++indx)
            {
                UNDataCountryProfileModel countryProfileObj = new UNDataCountryProfileModel()
                {
                    Url = new Uri(new Uri(UNDataList.Url), countryListNodes[indx].Attributes["href"].Value).AbsoluteUri
                };

                HtmlNode countryPageNode = countryProfileObj.Load();

                countryProfileObj.Region = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//div[@id='ctl00_main_CountryData']/div[3]/table//tr[2]");
                countryProfileObj.SurfaceAreaSqKm = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//div[@id='ctl00_main_CountryData']/div[3]/table//tr[3]");
                countryProfileObj.CapitalCity = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//div[@id='ctl00_main_CountryData']/div[3]/table//tr[6]");
                countryProfileObj.Currency = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//div[@id='ctl00_main_CountryData']/div[3]/table//tr[8]");
                countryProfileObj.UNMembershipDate = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//div[@id='ctl00_main_CountryData']/div[3]/table//tr[9]");
            }
        }

        private void FetchGeoNamesDatabase()
        {
            GeoNamesDatabase = new GeoNamesDatabaseModel()
            {
                Url = "http://www.geonames.org/countries/"
            };

            HtmlNode countryInfoPageNode = GeoNamesDatabase.Load();
            HtmlNode countryInfoTable = countryInfoPageNode.SelectSingleNode("//table[@id='countries']");

            GeoNamesDatabase.GeoNamesCountries = HtmlScrapperHelper.FetchTable(countryInfoTable, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[5]" },
                new string[] { "tr[1]/th[5]", null, " Url" },
                new string[] { "tr[1]/th[6]" },
                new string[] { "tr[1]/th[7]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]/a", "name" },
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                new string[] { "td[4]" },
                new string[] { "td[5]" },
                new string[] { "td[5]/a", "href", "http://www.geonames.org" },
                new string[] { "td[6]" },
                new string[] { "td[7]" },
                }
                );

            for(int row = 0; row < GeoNamesDatabase.GeoNamesCountries.Value.Count; ++row)
            {
                List<string> rowDataCountry = GeoNamesDatabase.GeoNamesCountries.Value[row];

                GeoNamesCountryInfoModel countryInfo = new GeoNamesCountryInfoModel()
                {
                    Url = rowDataCountry[5]
                };

                HtmlNode countryPageNode = countryInfo.Load();

                countryInfo.CountryNameOtherLangUrl = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[2]/table/tr[1]/td[2]/a[2]", "href");
                countryInfo.Currency = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[2]/table/tr[7]/td[2]");
                countryInfo.Neighbours.Value = HtmlScrapperHelper.FetchSingleValue(countryPageNode,
                    "//body/div[2]/table/tr[9]/td[2]").Split(new char[] { ',' }).Select(x => x.Trim()).ToList();
                countryInfo.Languages.Value = HtmlScrapperHelper.FetchSingleValue(countryPageNode,
                    "//body/div[2]/table/tr[8]/td[2]").Split(new char[] { ',' }).Select(x => x.Trim()).ToList();
                countryInfo.PostalCodeFormat = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[2]/table/tr[1]/td[10]");
                countryInfo.ISOAdministrativeDivisionsUrl = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[3]/table//tr[1]/td[2]/table//tr[1]/td//a[1]", "href");
                countryInfo.ISOAdministrativeDivisionsUrl.Value = "http://www.geonames.org/countries" +
                    countryInfo.ISOAdministrativeDivisionsUrl.Value;
                countryInfo.LargestCitiesUrl = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[3]/table//tr[1]/td[2]/table//tr[1]/td//a[3]", "href");
                countryInfo.LargestCitiesUrl.Value = "http://www.geonames.org/countries" +
                    countryInfo.LargestCitiesUrl.Value;
                countryInfo.HighestMountainsUrl = HtmlScrapperHelper.FetchSingleText(countryPageNode,
                    "//body/div[3]/table//tr[1]/td[2]/table//tr[1]/td//a[4]", "href");
                countryInfo.HighestMountainsUrl.Value = "http://www.geonames.org/countries" +
                    countryInfo.HighestMountainsUrl.Value;

                // Other names
                countryInfo.OtherLanguageCountriesNames = new ScrapTable()
                {
                    Url = countryInfo.CountryNameOtherLangUrl.Value,
                    XPath = "//table[@id='altnametable']"
                };

                HtmlNode altNameTableNode = countryInfo.OtherLanguageCountriesNames.Load().SelectSingleNode(countryInfo.OtherLanguageCountriesNames.XPath);

                countryInfo.OtherLanguageCountriesNames = HtmlScrapperHelper.FetchTable(
                    altNameTableNode, new string[][]
                {
                new string[] { "tr[1]/th[1]" },
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[1]" },
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                }
                );

                countryInfo.ISOAdminDivs = new ScrapTable()
                {
                    Url = countryInfo.ISOAdministrativeDivisionsUrl.Value,
                    XPath = "//table[@id='subdivtable1']"
                };

                HtmlNode isoAdminTableNode = countryInfo.ISOAdminDivs.Load().SelectSingleNode(countryInfo.ISOAdminDivs.XPath);

                countryInfo.ISOAdminDivs = HtmlScrapperHelper.FetchTable(
                    isoAdminTableNode, new string[][]
                {
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[3]" },
                new string[] { "tr[1]/th[4]" },
                new string[] { "tr[1]/th[5]", null, " Url" },
                new string[] { "tr[1]/th[5]" },
                new string[] { "tr[1]/th[6]" },
                new string[] { "tr[1]/th[7]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[2]" },
                new string[] { "td[3]" },
                new string[] { "td[4]" },
                new string[] { "td[5]//a[1]", "href" },
                new string[] { "td[5]//a[1]" },
                new string[] { "td[6]" },
                new string[] { "td[7]" },
                }
                );

                countryInfo.LargestCities = new ScrapTable()
                {
                    Url = countryInfo.LargestCitiesUrl.Value,
                    XPath = "//table[@class='restable sortable']"
                };

                HtmlNode largestCitiesTableNode = countryInfo.LargestCities.Load().SelectSingleNode(countryInfo.LargestCities.XPath);

                countryInfo.LargestCities = HtmlScrapperHelper.FetchTable(
                    largestCitiesTableNode, new string[][]
                {
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[4]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[2]//a[1]" },
                new string[] { "td[4]//a[1]" },
                }
                );

                countryInfo.HighestMountains = new ScrapTable()
                {
                    Url = countryInfo.HighestMountainsUrl.Value,
                    XPath = "//table[@class='restable sortable']"
                };

                HtmlNode highestMountainsTableNode = countryInfo.HighestMountains.Load().SelectSingleNode(countryInfo.HighestMountains.XPath);

                countryInfo.HighestMountains = HtmlScrapperHelper.FetchTable(
                    highestMountainsTableNode, new string[][]
                {
                new string[] { "tr[1]/th[2]" },
                new string[] { "tr[1]/th[4]" },
                },
                ".//tr[position() > 1]",
                new string[][]
                {
                new string[] { "td[2]//a[1]" },
                new string[] { "td[4]//a[1]" },
                }
                );
            }
        }

        private void FetchWikiCountryCapitals()
        {
            WikiCountryCapitals = new WikiCountryCapitalsModel()
            {
                Url = "https://en.wikipedia.org/wiki/List_of_national_capitals_in_alphabetical_order"
            };

            HtmlNode listOfCapitalsPageNode = WikiCountryCapitals.Load();
            HtmlNode capitalsTableNode = listOfCapitalsPageNode.SelectSingleNode("//div[@id='mw-content-text']/div/table[3]");
            HtmlNodeCollection trNodes = capitalsTableNode.SelectNodes(".//tr[position() > 1]");

            WikiCountryCapitals.Capitals = new List<List<string>>();
            for(int rowNode = 0; rowNode < trNodes.Count; ++rowNode)
            {
                List<string> capitalData = new List<string>();
                HtmlNodeCollection dataNodes = trNodes[rowNode].SelectNodes("td");

                capitalData.Add(dataNodes[1].InnerText);
            }
            //WikiCountryCapitals.Capitals = HtmlScrapperHelper.FetchTable(capitalsTableNode, new string[][]
            //    {
            //    new string[] { "tr[1]/th[1]" },
            //    new string[] { "tr[1]/th[1]", null, " Url" },
            //    new string[] { "tr[1]/th[2]" },
            //    },
            //    ,
            //    new string[][]
            //    {
            //    new string[] { "td[1]//a", "title" },
            //    new string[] { "td[1]//a", "href", "https://en.wikipedia.org" },
            //    new string[] { "td[2]" },
            //    });

            //for(int indx = 0; indx < WikiCountryCapitals.Capitals.Value.Count; ++indx)
            //{
            //    List<string> capitalRow = WikiCountryCapitals.Capitals.Value[indx];

            //    HtmlNode capitalPageNode = HtmlScrapperHelper.Load(capitalRow[0]);
            //    HtmlNode CoordinatesTrNode = capitalPageNode.SelectSingleNode("//div[@id='mw-content-text']//table[@class='infobox geography vcard']//tr[contains(text(), 'Coordinates: ')]");
            //}
        }
    }
}

using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [ReferenceSingle(XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='Government']]/following-sibling::li")]
    public class CIAWorldFactbookCountryGovernmentData
    {
        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'conventional long form')]/following-sibling::span")]
        public string CountryConventionalLongName { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'conventional short form')]/following-sibling::span")]
        public string CountryConventionalShortName { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'local long form')]/following-sibling::span")]
        public string CountryLocalLongForm { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'local short form')]/following-sibling::span")]
        public string CountryLocalShortForm { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'former')]/following-sibling::span")]
        public string CountryFormer { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Country name:')]/following-sibling::div/span[contains(text(), 'etymology')]/following-sibling::span")]
        public string CountryEtymology { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Government type:')]/following-sibling::div[@class='category_data']")]
        public string GovernmentType { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Capital:')]/following-sibling::div/span[contains(text(), 'name')]/following-sibling::span")]
        public string CapitalName { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Capital:')]/following-sibling::div/span[contains(text(), 'geographic coordinates')]/following-sibling::span")]
        public string CapitalCoordinates { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Capital:')]/following-sibling::div/span[contains(text(), 'time difference')]/following-sibling::span")]
        public string TimeDifference { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Administrative divisions:')]/following-sibling::div[@class='category_data']")]
        public string AdministrativeDivisions { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Independence:')]/following-sibling::div[@class='category_data']")]
        public string Independence { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Executive branch:')]/following-sibling::div/span[contains(text(), 'chief of state:')]/following-sibling::span")]
        public string ChiefofState { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Executive branch:')]/following-sibling::div/span[contains(text(), 'head of government:')]/following-sibling::span")]
        public string HeadOfGovernment { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Executive branch:')]/following-sibling::div/span[contains(text(), 'cabinet:')]/following-sibling::span")]
        public string Cabinet { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'International organization participation:')]/following-sibling::div[@class='category_data']")]
        public string InternationalOrganizationParticipation { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation in the US:')]/following-sibling::div/span[contains(text(), 'chief of mission:')]/following-sibling::span")]
        public string DiplomaticInUSChiefofMission { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation in the US:')]/following-sibling::div/span[contains(text(), 'chancery:')]/following-sibling::span")]
        public string DiplomaticInUSChancery { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation in the US:')]/following-sibling::div/span[contains(text(), 'telephone:')]/following-sibling::span")]
        public string DiplomaticInUSTelephone { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation in the US:')]/following-sibling::div/span[contains(text(), 'FAX:')]/following-sibling::span")]
        public string DiplomaticInUSFAX { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation in the US:')]/following-sibling::div/span[contains(text(), 'consulate(s) general:')]/following-sibling::span")]
        public string DiplomaticInUSConsulateGeneral { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation from the US:')]/following-sibling::div/span[contains(text(), 'chief of mission:')]/following-sibling::span")]
        public string DiplomaticFromUSChiefofMission { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation from the US:')]/following-sibling::div/span[contains(text(), 'embassy:')]/following-sibling::span")]
        public string DiplomaticFromUSEmbassy { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation from the US:')]/following-sibling::div/span[contains(text(), 'mailing address:')]/following-sibling::span")]
        public string DiplomaticFromUSMailingAddress { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation from the US:')]/following-sibling::div/span[contains(text(), 'telephone:')]/following-sibling::span")]
        public string DiplomaticFromUSTelephone { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Diplomatic representation from the US:')]/following-sibling::div/span[contains(text(), 'FAX:')]/following-sibling::span")]
        public string DiplomaticFromUSFAX { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'National anthem:')]/following-sibling::div/span[contains(text(), 'name:')]/following-sibling::span")]
        public string NationalAnthemName { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'National anthem:')]/following-sibling::div/span[contains(text(), 'lyrics/music:')]/following-sibling::span")]
        public string NationalAnthemLyricsMusic { get; set; }
    }
}

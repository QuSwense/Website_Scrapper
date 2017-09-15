using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ISO
{
    [QSUriRTTIDataSource(ClassPath = "SimpleWebsiteToCsv.WebPageDataSet.Wiki.ISO3166Alpha1",
        Property = "Alpha2Code", Format = "https://www.iso.org/obp/ui/#iso:code:3166:{0}")]
    public class ISO3166CountryInfo
    {
        [ReferenceSingle("(//div[@class='core-view-summary']/div[@class='core-view-line'])[4]/div[@class='core-view-field-value']")]
        public string FullName { get; set; }

        [ReferenceSingle("(//div[@class='core-view-summary']/div[@class='core-view-line'])[8]/div[@class='core-view-field-value']")]
        [BoolConversion(TruthValues = "Yes")]
        public bool IsIndependent { get; set; }

        [ReferenceSingle("(//div[@class='core-view-summary']/div[@class='core-view-line'])[10]/div[@class='core-view-field-value']")]
        public string Status { get; set; }

        [ReferenceSingle("(//div[@class='core-view-summary']/div[@class='core-view-line']/div[@class='core-view-field-name' and contains(text() = 'Remark')])/div[@class='core-view-field-value']")]
        public List<string> Remark { get; set; }

        [ReferenceCollection("//div[@id='country-additional-info']/table//tr[position() > 1]")]
        public class AdditionalInformation
        {
            [ReferenceSingle("td[position() = 1]")]
            public string AdministrativeLanguageAlpha2 { get; set; }

            [ReferenceSingle("td[position() = 2]")]
            public string AdministrativeLanguageAlpha3 { get; set; }

            [ReferenceSingle("td[position() = 3]")]
            public string LocalShortName { get; set; }
        }

        [ReferenceSingle("//div[@id='country-subdivisions']/following-sibling::p[preceding::p[contains(text(), 'List source')]]")]
        public class Subdivisions
        {
            [ReferenceCollection("following-sibling::p[preceding::p[contains(text(), 'List source')]]")]
            public List<SubdivisionsStatistics> Statistics { get; set; }

            [ReferenceSingle("p[contains(text(), 'List source')]/following-sibling::p[1]")]
            public string ListSource { get; set; }

            [ReferenceSingle("p[contains(text(), 'Code source')]/following-sibling::p[1]")]
            public string CodeSource { get; set; }

            [ReferenceCollection(".//table[@id='subdivision']//tr[position() > 1]")]
            public Dictionary<string, Subdivision> SubdivisionDetails { get; set; }

            public Subdivisions()
            {
                Statistics = new List<SubdivisionsStatistics>();
                SubdivisionDetails = new Dictionary<string, Subdivision>();
            }
        }
    }

    public class SubdivisionsStatistics
    {
        [ReferenceSingle("p[1]/b[@class='category-count']")]
        public int Count { get; set; }

        [ReferenceCollection("p[1]/span[@class='category-locales']")]
        [RegexSplit(@"(.*)\((\w+)\)")]
        public List<SubdivisionLanguageValue> NameInLangs { get; set; }

        public SubdivisionsStatistics()
        {
            NameInLangs = new List<SubdivisionLanguageValue>();
        }
    }

    public class SubdivisionLanguageValue
    {
        [QSIndex(0)]
        public string Name { get; set; }

        [QSIndex(1)]
        [Base(" )(")]
        public string Language { get; set; }
    }

    public class Subdivision
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Category { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        [UniqueDictionaryKey]
        public string ISO31662Alpha2 { get; set; }

        [ReferenceSingle("td[position() = 3]")]
        public string SubdivisionName { get; set; }

        [ReferenceSingle("td[position() = 4]")]
        public string LocalVariant { get; set; }

        [ReferenceSingle("td[position() = 5]")]
        public string LanguageCode { get; set; }

        [ReferenceSingle("td[position() = 6]")]
        public string RomanizationSystem { get; set; }

        [ReferenceSingle("td[position() = 7]")]
        public string ParentSubdivision { get; set; }
    }
}

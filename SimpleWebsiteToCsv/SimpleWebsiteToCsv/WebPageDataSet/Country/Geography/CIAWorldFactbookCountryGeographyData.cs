using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [CitationRegex(ClassName = "SimpleWebsiteToCsv.WebPageDataSet.CIACountryUrlData", PropertyName = "CountryURL")]
    [ReferenceSingle(XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='Geography']]/following-sibling::li")]
    public class CIAWorldFactbookCountryGeographyData
    {
        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Location:')]/following-sibling::div[@class='category_data']")]
        public string Location { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Geographic coordinates:')]/following-sibling::div[@class='category_data']")]
        public string Coordinates { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'total:')]/following-sibling::span")]
        public string TotalArea { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'land:')]/following-sibling::span")]
        public string LandArea { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'water:')]/following-sibling::span")]
        public string WaterArea { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Area:')]/following-sibling::div/span[contains(text(), 'country comparison to the world:')]/following-sibling::span")]
        public string CountryComparisionByAreaRank { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Land boundaries:')]/following-sibling::div/span[contains(text(), 'total:')]/following-sibling::span")]
        public string TotalLandBoundaries { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Land boundaries:')]/following-sibling::div/span[contains(text(), 'border countries')]/following-sibling::span")]
        public string BorderCountries { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Coastline:')]/following-sibling::div")]
        public string Coastline { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'mean elevation:')]/following-sibling::span")]
        public string MeanElevation { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'lowest point:')]")]
        public string LowestPoint { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Elevation:')]/following-sibling::div/span[contains(text(), 'highest point:')]")]
        public string HighestPoint { get; set; }
    }
}

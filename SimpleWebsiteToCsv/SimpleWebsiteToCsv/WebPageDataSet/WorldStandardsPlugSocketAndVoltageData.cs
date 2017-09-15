using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\Complete list_ Plug, socket & voltage by country - World Standards.html",
       UrlOnline = @"http://www.worldstandards.eu/electricity/plug-voltage-by-country/")]
    [ReferenceCollection(XPath = "//table[@class='table table-striped']//tr[position() > 1]")]
    public class WorldStandardsPlugSocketAndVoltageData
    {
        [ReferenceSingle(XPath = "td[position() = 1]")]
        public string CountryOrTerritoryName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]")]
        public string SinglePhaseVoltageInV { get; set; }

        [ReferenceSingle(XPath = "td[position() = 3]")]
        public string FrequencyInHz { get; set; }

        [ReferenceSingle(XPath = "td[position() = 4]/a")]
        public string PlugType { get; set; }
    }
}

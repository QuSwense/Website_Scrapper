using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\Internet country domains list _ Country Internet codes _ TLDs - World Standards.html",
       UrlOnline = @"http://www.worldstandards.eu/electricity/plug-voltage-by-country/")]
    [ReferenceCollection(XPath = "//table[@class='table table-striped']//tr[position() > 1]")]
    public class WorldStandardsTLDData
    {
        [ReferenceSingle(XPath = "td[position() = 1]")]
        public string CountryName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]")]
        public string TLDWithComment { get; set; }
    }
}

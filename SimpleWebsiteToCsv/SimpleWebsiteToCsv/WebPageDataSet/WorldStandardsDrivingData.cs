using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\List of left- & right-driving countries - World Standards.html",
       UrlOnline = @"http://www.worldstandards.eu/cars/list-of-left-driving-countries")]
    [ReferenceCollection(XPath = "//table[@class='table table-striped']//tr[position() > 1]")]
    public class WorldStandardsDrivingData
    {
        [ReferenceSingle(XPath = "td[position() = 1]")]
        public string CountryName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]")]
        public string DrivingDirection { get; set; }
    }
}

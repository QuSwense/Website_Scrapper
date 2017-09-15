using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOnline = "http://www.worldstandards.eu/electricity/plugs-and-sockets/",
        UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\Plug & socket types - World Standards.html")]
    [ReferenceCollection(XPath = "//div[@role='document']//div[@class='row-fluid']//div[@class='span3']/div[@class='tile']")]
    public class WorldStandardsPlugAndSocketTypeData
    {
        [ReferenceSingle(XPath = "h4[@class='tile-title']")]
        public string PlugTypeName { get; set; }
        [ReferenceSingle(XPath = "p/img[@class='img-rounded img-responsive']", Attribute = "src")]
        public string ImageUrl { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 1]")]
        public string Note { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 2]")]
        public string PinCount { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 3]")]
        public string GroundedorNot { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 4]")]
        public string Ampere { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 5]")]
        public string Voltage { get; set; }
        [ReferenceSingle(XPath = "ul/li[position() = 6]")]
        public string SocketCompatibleWithType { get; set; }
    }
}

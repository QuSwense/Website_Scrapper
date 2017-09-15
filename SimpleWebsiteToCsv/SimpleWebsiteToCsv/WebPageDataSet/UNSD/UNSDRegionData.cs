using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [QSReference(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\UNSD — Methodology.html",
        UrlOnline = @"https://unstats.un.org/unsd/methodology/m49")]
    [ReferenceCollection("//div[@id='GeoGroupsENG']/table//tr[position() > 1]")]
    public class UNSDRegionData
    {
        [UniqueDictionaryKey]
        [ReferenceSingle(Attribute = "data-tt-id")]
        public int? M49 { get; set; }

        [ReferenceSingle(Attribute = "data-tt-parent-id")]
        public int? ParentM49 { get; set; }

        [UniqueDictionaryKey]
        [ReferenceSingle("td[position() = 1]")]
        public string CountryName { get; set; }

        [UniqueDictionaryKey(true)]
        [ReferenceSingle("td[position() = 3]")]
        public string Iso3166Alpha3 { get; set; }

        [ReferenceSingle("//div[@id='ENG_DEVELOPED']/table//tr[position() > 1]/td[position() = 2 and contains(text(), '{0}')]",
            "M49")]
        public bool IsDevelopedRegion { get; set; }

        [ReferenceSingle("//div[@id='ENG_DEVELOPING']/table//tr[position() > 1]/td[position() = 2 and contains(text(), '{0}')]",
            "M49")]
        public bool IsDevelopingRegion { get; set; }

        [ReferenceSingle("//div[@id='ENG']/table//tr[position() > 1]/td[position() = 2 and contains(text(), '{0}')]",
            "M49")]
        public bool IsLDC { get; set; }

        [ReferenceSingle("//div[@id='ENG_LLDC']/table//tr[position() > 1]/td[position() = 2 and contains(text(), '{0}')]",
            "M49")]
        public bool IsLLDC { get; set; }

        [ReferenceSingle("//div[@id='ENG_SIDS']/table//tr[position() > 1]/td[position() = 2 and contains(text(), '{0}')]",
            "M49")]
        public bool IsSIDS { get; set; }
    }
}

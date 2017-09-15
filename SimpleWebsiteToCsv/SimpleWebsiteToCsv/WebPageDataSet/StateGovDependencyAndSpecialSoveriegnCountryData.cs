using SimpleWebsiteToCsv.Metadata;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\Dependencies and Areas of Special Sovereignty.html",
      UrlOnline = @"https://www.state.gov/s/inr/rls/10543.htm")]
    [ReferenceCollection(XPath = "//div[@class='table-responsive double-scroll']/table//tr[position() > 1]")]
    public class StateGovDependencyAndSpecialSoveriegnCountryData
    {
        [ReferenceSingle(XPath = "td[position() = 1]/p")]
        [RegexReplace(Pattern = @"<br>|\*|\+", ReplaceText = " ", Order = 1)]
        [RegexReplace(Pattern = @"\(see.*\)", ReplaceText = " ", Order = 2)]
        [RegexReplace(Pattern = @"[ ]{2,}", ReplaceText = " ", Order = 3)]
        public string RegionName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 2]/p")]
        [RegexReplace(Pattern = @"\!", ReplaceText = " ", Order = 1)]
        public string RegionLongName { get; set; }

        [ReferenceSingle(XPath = "td[position() = 3]/p")]
        [RegexReplace(Pattern = @"\(see.*\)", ReplaceText = " ", Order = 2)]
        [RegexReplace(Pattern = @"[ ]{2,}", ReplaceText = " ", Order = 3)]
        public string SovereigntyUnderCountry { get; set; }

        [ReferenceSingle(XPath = "td[position() = 4]/p")]
        public string GENCCode2A { get; set; }

        [ReferenceSingle(XPath = "td[position() = 5]/p")]
        public string GENCCod3A { get; set; }

        [ReferenceSingle(XPath = "td[position() = 6]/p")]
        [RegexReplace(Pattern = @"\(see.*\)", ReplaceText = " ", Order = 2)]
        [RegexReplace(Pattern = @"[ ]{2,}", ReplaceText = " ", Order = 3)]
        public string AdministrativeCenter { get; set; }
    }
}

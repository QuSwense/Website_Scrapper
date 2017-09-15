using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\Independent States in the World.html",
       UrlOnline = @"https://www.state.gov/s/inr/rls/4250.htm")]
    [ReferenceCollection(XPath = "//div[@class='table-responsive double-scroll']/table//tr[position() > 1]")]
    public class StateGovSoveriegnCountryData
    {
        [ReferenceSingle(XPath = "td[position() = 1]/p")]
        [RegexReplace(Pattern = @"<br>|\*|\+", ReplaceText = " ", Order = 1)]
        [RegexReplace(Pattern = @"\(see.*\)", ReplaceText = " ", Order = 2)]
        [RegexReplace(Pattern = @"[ ]{2,}", ReplaceText = " ", Order = 3)]
        public string CountryNameWithOptions { get; set; }
        [ReferenceSingle(XPath = "td[position() = 1]/p[contains(., '*')]")]
        public bool DiplomaticRelationWithUS { get; set; }
        [ReferenceSingle(XPath = "td[position() = 1]/p[contains(., '+')]")]
        public bool MemberOfUN { get; set; }
        [ReferenceSingle(XPath = "td[position() = 2]/p")]
        public string CountryNameLongForm { get; set; }
        [ReferenceSingle(XPath = "td[position() = 3]/p")]
        public string IsoAlpha2Code { get; set; }
        [ReferenceSingle(XPath = "td[position() = 4]/p")]
        public string IsoAlpha3Code { get; set; }
        [ReferenceSingle(XPath = "td[position() = 5]/p", UseInnerHtml = true)]
        [RegexReplace(Pattern = @"\(see.*\)", ReplaceText = " ", Order = 1)]
        [RegexReplace(Pattern = @"[ ]{2,}", ReplaceText = " ", Order = 2)]
        public FieldData CapitalCity { get; set; }

        public StateGovSoveriegnCountryData()
        {
            CapitalCity = new FieldData();
            CapitalCity.Parse = ProcessCapitalCity;
        }

        private void ProcessCapitalCity(object htmlText)
        {
            List<string> results = new List<string>();
            results.AddRange(htmlText.ToString().Split(new string[] { "<br>" }, StringSplitOptions.RemoveEmptyEntries));

            for (int indx = 0; indx < results.Count; ++indx)
            {
                results[indx] = results[indx].Replace(
                    @"<span style=""font - size: 0.875rem; font - family: &quot; Franklin Gothic Book & quot;, sans - serif; color: red; "">", "");
                results[indx] = results[indx].Replace(@"</span>", "");
                results[indx] = results[indx].Replace(
                    @"<span style=""font - size: 0.875rem; color: rgb(30, 51, 81); font - family: &quot; Franklin Gothic Book & quot;, sans - serif; "">", "");
                results[indx] = results[indx].Replace("!", "");
                results[indx] = results[indx].Replace("&nbsp;", "");
                results[indx] = results[indx].Trim();
            }

            CapitalCity.Result = results;
        }
    }
}

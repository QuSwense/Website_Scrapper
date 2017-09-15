using HtmlAgilityPack;
using SimpleWebsiteToCsv.Common;
using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [Citation(UrlOffline = @"file://C:\Subhadeep\Business\TestWebsites\The World Factbook — Central Intelligence Agency.html",
        UrlOnline = @"https://www.cia.gov/library/publications/resources/the-world-factbook/")]
    [ReferenceCollection(XPath = "//div[@id='cntrySelect']//select/option[position() > 1]")]
    public class CIACountryUrlData : ISteps
    {
        [ReferenceSingle()]
        public string CountryName { get; set; }

        [ReferenceSingle(Attribute = "value")]
        public FieldData CountryURL { get; set; }

        public string CountryUrlOffline { get; set; }

        public CIACountryUrlData()
        {
            CountryURL = new FieldData();
            CountryURL.Parse = ParseURL;
        }

        private void ParseURL(object value)
        {
            string result = value.ToString();
            Uri baseUri = new Uri("https://www.cia.gov/library/publications/resources/the-world-factbook/");
            Uri finalUri = new Uri(baseUri, result);
            
            CountryURL.Result = finalUri.AbsoluteUri;
        }

        public void Begin()
        {
            
        }

        public void Final()
        {
            // Save Country Page locally
            HtmlDocument document = WebpageHelper.GetHtmlDocument(CountryURL.Result.ToString());

            string filePath = Path.Combine(@"C:\Subhadeep\Business\TestWebsites\CIAWorldFactbook\", CountryName + ".html");
            document.Save(filePath);

            CountryUrlOffline = new Uri(filePath).AbsoluteUri;
        }
    }
}

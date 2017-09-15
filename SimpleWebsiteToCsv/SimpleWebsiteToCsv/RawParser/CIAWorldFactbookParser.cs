using SimpleWebsiteToCsv.Common;
using SimpleWebsiteToCsv.RawParser;
using SimpleWebsiteToCsv.WebPageDataSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.RawParser
{
    public class CIAWorldFactbookParser
    {
        public Dictionary<string, CIACountryUrlData> CIACountryUrls = new Dictionary<string, CIACountryUrlData>();

        public void Process()
        {
            if(!GlobalSettings.PopulateOfflineCIAFactbookUrl)
                new HttpWebpageEngine<CIACountryUrlData>().Parse();
            else
            {
                DirectoryInfo dirInfo = new DirectoryInfo(@"C:\Subhadeep\Business\TestWebsites\CIAWorldFactbook\");

                foreach(FileInfo fi in dirInfo.GetFiles("*.html"))
                {
                    CIACountryUrlData urlData = new CIACountryUrlData();
                    urlData.CountryUrlOffline = new Uri(fi.FullName).AbsoluteUri;
                    urlData.CountryName = fi.Name.Replace(".html", "");

                    CIACountryUrls.Add(urlData.CountryName, urlData);
                }
            }

            foreach(KeyValuePair<string, CIACountryUrlData> countryUrlObj in CIACountryUrls)
            {
                CIAWorldFactbookCountryGeographyData countrygeographyDataObj =
                    WebpageHelper.HtmlParserForSingle< CIAWorldFactbookCountryGeographyData>(countryUrlObj.Value.CountryUrlOffline);
            }
        }
    }
}

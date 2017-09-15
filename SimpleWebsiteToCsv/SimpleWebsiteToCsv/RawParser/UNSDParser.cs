using SimpleWebsiteToCsv.Common;
using SimpleWebsiteToCsv.WebPageDataSet;
using System.Collections.Generic;

namespace SimpleWebsiteToCsv.RawParser
{
    public class UNSDParser
    {
        public Dictionary<string, UNSDRegionData> UNSDRegions = new Dictionary<string, UNSDRegionData>();
        public Dictionary<string, UNSDRegionLDCData> UNSDLDCRegions = new Dictionary<string, UNSDRegionLDCData>();
        public Dictionary<string, UNSDRegionLDCData> UNSDRegionLLDCs = new Dictionary<string, UNSDRegionLDCData>();
        public Dictionary<string, UNSDRegionLDCData> UNSDRegionSIDSs = new Dictionary<string, UNSDRegionLDCData>();
        public Dictionary<string, UNSDDevelopedRegionData> UNSDDevelopedRegions = new Dictionary<string, UNSDDevelopedRegionData>();
        public Dictionary<string, UNSDDevelopingRegionData> UNSDDevelopingRegions = new Dictionary<string, UNSDDevelopingRegionData>();

        public void Process()
        {
            //WebpageHelper.HtmlTableParser(UNSDRegions, "Iso3166Alpha3");
            //WebpageHelper.HtmlTableParser(UNSDLDCRegions, "Iso3166Alpha3");
            //WebpageHelper.HtmlTableParser(UNSDRegionLLDCs, "Iso3166Alpha3");
            //WebpageHelper.HtmlTableParser(UNSDRegionSIDSs, "Iso3166Alpha3");
            WebpageHelper.HtmlParser(UNSDDevelopedRegions, "Iso3166Alpha3");
            //WebpageHelper.HtmlTableParser(UNSDDevelopingRegions, "Iso3166Alpha3");
        }
    }
}

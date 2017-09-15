using SimpleWebsiteToCsv.Common;
using SimpleWebsiteToCsv.WebPageDataSet;
using System.Collections.Generic;

namespace SimpleWebsiteToCsv.RawParser
{
    public class WorldStandardsParser
    {
        public Dictionary<string, WorldStandardsDrivingData> WorldStandardsDrivings = new Dictionary<string, WorldStandardsDrivingData>();
        public Dictionary<string, WorldStandardsPlugSocketAndVoltageData> WorldStandardsPlugSocketAndVoltages = new Dictionary<string, WorldStandardsPlugSocketAndVoltageData>();
        public Dictionary<string, WorldStandardsTLDData> WorldStandardsTLDs = new Dictionary<string, WorldStandardsTLDData>();

        public void Process()
        {
            WebpageHelper.HtmlParser(WorldStandardsDrivings, "CountryName");
            WebpageHelper.HtmlParser(WorldStandardsPlugSocketAndVoltages, "CountryName");
            WebpageHelper.HtmlParser(WorldStandardsTLDs, "CountryName");
        }
    }
}

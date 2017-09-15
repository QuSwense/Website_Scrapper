using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [QSReferenceFromDataSource("//div[@class='padtop padbottom padright']/div/p[position() = 1]")]
    [QSReferenceFromDataSource("//div[@class='body-copy']/p[position() = 4]",
        OnlineUrl = "https://www.gs1uk.org/our-industries/retail/selling-on-marketplaces/ultimate-guide-to-gtins-eans-and-upcs-for-amazon-ebay-google")]
    [QSReferenceFromDataSource("//div[@class='body-copy']/p[position() = 5]",
        OnlineUrl = "https://www.gs1uk.org/our-industries/retail/selling-on-marketplaces/ultimate-guide-to-gtins-eans-and-upcs-for-amazon-ebay-google")]
    [QSReferenceFromDataSource("//div[@class='body-copy']/p[position() = 6]",
        OnlineUrl = "https://www.gs1uk.org/our-industries/retail/selling-on-marketplaces/ultimate-guide-to-gtins-eans-and-upcs-for-amazon-ebay-google")]
    [QSURIDataSource(Online = "http://www.citymaphq.com/codes/gs1.html")]
    [QSPropertyStringSplit("-", PropertyName = "Code")]
    public class CMHGS1GTINPrefix : CMHCode<int>
    {
    }
}

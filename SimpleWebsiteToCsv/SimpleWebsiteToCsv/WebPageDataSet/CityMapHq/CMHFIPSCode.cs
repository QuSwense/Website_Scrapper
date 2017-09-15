using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [QSReference(Text = "This is now obsolete.")]
    [QSReferenceFromDataSource("//div[@class='padtop padbottom padright']/div/p[position() = 1]")]
    [QSURIDataSource(Online = "http://www.citymaphq.com/codes/fips.html")]
    public class CMHFIPSCode : CMHCode<string>
    {
    }
}

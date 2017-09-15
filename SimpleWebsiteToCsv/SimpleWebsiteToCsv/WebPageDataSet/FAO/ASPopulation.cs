using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.FAO
{
    [QSURIDataSource(Offline = @"ManualData\AQUASTAT\aquastat_geographyAndPopulation_LandUse.csv")]
    public class ASPopulation : ASDataFile { }
}

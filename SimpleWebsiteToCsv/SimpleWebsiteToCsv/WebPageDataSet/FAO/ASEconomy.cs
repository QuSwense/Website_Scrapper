using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.FAO
{
    [QSURIFileDataSourceAttribute(Offline = @"ManualData\AQUASTAT\Aquastat_GeophyAndPopulation_Economy.csv")]
    public class ASEconomy : ASDataFile { }
}

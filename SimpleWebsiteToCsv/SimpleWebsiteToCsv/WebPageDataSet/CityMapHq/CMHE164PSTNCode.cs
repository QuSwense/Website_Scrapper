﻿using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.CityMapHq
{
    [QSReferenceFromDataSource("//div[@class='padtop padbottom padright']/div/p[position() = 1]")]
    [QSURIDataSource(Online = "http://www.citymaphq.com/codes/e164.html")]
    public class CMHE164PSTNCode : CMHCode<string>
    {
    }
}

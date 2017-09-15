using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.ITU
{
    [QSReference("Maritime identification digits are used by radio communication facilities to identify their home country or base area in Digital Selective Calling (DSC), Automatic Transmitter Identification System (ATIS), and Automatic Identification System (AIS) messages as part of their Maritime Mobile Service Identities.")]
    [QSURIDataSource(Online = "https://www.itu.int/en/ITU-R/terrestrial/fmd/Pages/mid.aspx")]
    [ReferenceCollection("//table[@class='table table-striped table-condensed']//tr[position() > 1]")]
    public class ITUMaritimeIdentificationDigits
    {
        [ReferenceCollection("td[position() = 1]/span")]
        public List<string> Code { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Country { get; set; }
    }
}

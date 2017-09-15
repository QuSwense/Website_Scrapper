using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.GS1
{
    [QSReference(Text = "The Global Language of Business standards (Gs1) introduced the barcode in 1974. It is a global, neutral, non-profit standards organisation. Historically, The European Article Numbering (EAN) Association — later called GS1 — opens an office in Brussels. All legal UPC and EAN codes (including the ones we sell) must originate with GS1. GS1 is the worldwide standards organization who governs UPC and EAN codes. GS1 has member organizations in just about every country. It is true that the first 2 digits of a UPC or 3 digits of an EAN code indicate the GS1 country member organization by which that UPC or EAN code was issued. 'GS1 Prefixes do not provide identification of country of origin for a given product.'")]
    [Citation(LogoOnlineUrl = "https://www.gs1.org/sites/default/files/logo_resized.jpg",
        CitationString = "Only the commonly used GS1 codes are beung copied. No other data is used. Presented by GS1 for the purpose of disseminating information free of charge for the benefit of the public.",
        Copyright = "The Global Language of Business")]
    [ReferenceCollection("//table//tr")]
    public class GS1StandardCode
    {
        [ReferenceSingle("td[position() = 1]/p")]
        public string CodeRange { get; set; }

        [ReferenceSingle("td[position() = 2]/p")]
        public string Entity { get; set; }
    }
}

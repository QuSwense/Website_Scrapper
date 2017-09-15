using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Bank
{
    /// <summary>
    /// Another good one
    /// http://finaint.com/swift/AFGHANISTAN.html
    /// </summary>
    [CitationRegex(UrlOnline = "https://www.ifscswiftcodes.com/Bank-SWIFT-Codes/Countries-{0}.htm",
        FixedValues = new string[] { "A", "B", })]
    public class AllSWIFTCodeByCountry
    {
    }
}

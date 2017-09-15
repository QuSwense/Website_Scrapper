using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Wiki
{
    [QSReferenceFromDataSource(XPath = "//div[@class='mw-parser-output']/p[position() = 1]")]
    [QSURIDataSource(Online = "https://en.wikipedia.org/wiki/List_of_FIFA_country_codes")]
    [ReferenceCollection("//div[@id='mw-content-text']/div[@class='mw-parser-output']")]

    public class FIFAListOfCodes
    {
        [ReferenceCollection("table[position() = 1]//table[@class='wikitable']//tr[position() > 1]")]
        public FIFAMemberCode MemberCodes { get; set; }

        public FIFAIrregularCode IrregularCodes { get; set; }

        public FIFANonMemberCode NonMemberCodes { get; set; }
    }

    public class FIFACode
    {
        [ReferenceSingle("td[position() = 1]/span/a")]
        public string Name { get; set; }

        [ReferenceSingle("td[position() = 1]/span/a", Attribute = "href")]
        public string NationalTeamUrl { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Code { get; set; }
    }

    /// <summary>
    /// The XPath to get only the Parent Text as per
    /// https://stackoverflow.com/questions/14677492/inner-text-of-node-ignoring-inner-text-of-children
    /// </summary>
    [QSReferenceFromDataSource(XPath = "//div[@class='mw-parser-output']/p[position() = 4]/text()[1]")]
    public class FIFAMemberCode : FIFACode { }

    public class FIFACodeEx : FIFACode
    {
        [ReferenceSingle("td[position() = 3]")]
        public string Confederation { get; set; }
    }

    [QSReferenceFromDataSource(XPath = "//div[@class='mw-parser-output']/p[position() = 6]")]
    [ReferenceCollection("table[position() = 3]//table[@class='wikitable']//tr[position() > 1]")]
    public class FIFAIrregularCode : FIFACodeEx { }

    [QSReferenceFromDataSource(XPath = "//div[@class='mw-parser-output']/p[position() = 5]")]
    [ReferenceCollection("table[position() = 2]//table[@class='wikitable']//tr[position() > 1]")]
    public class FIFANonMemberCode : FIFACodeEx { }
}

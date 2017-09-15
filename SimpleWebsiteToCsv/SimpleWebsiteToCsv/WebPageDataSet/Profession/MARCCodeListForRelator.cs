using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Profession
{
    [Citation(Description = "The purpose of this list of relator terms and associated codes is to allow the relationship between an agent and a resource to be designated in bibliographic records.",
        UrlOnline = "https://www.loc.gov/marc/languages/language_code.html")]
    [ReferenceCollection("//table[@summary='relators arranged in code order']//tr[position() > 1]")]
    public class MARCCodeListForRelator
    {
        [ReferenceSingle("td[position() = 1]")]
        public string Code { get; set; }

        [ReferenceSingle("td[position() = 1]")]
        [RegexReplace(Pattern = @"\-(.*)", ReplaceText = "")]
        public bool IsDiscontinued { get; set; }

        [ReferenceSingle("td[position() = 2]")]
        public string Name { get; set; }
    }
}

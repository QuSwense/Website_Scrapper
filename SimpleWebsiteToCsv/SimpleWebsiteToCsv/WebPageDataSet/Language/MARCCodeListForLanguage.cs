using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Language
{
    [Citation(Description = "MARC (MAchine-Readable Cataloging) 21 formats are standards for the representation and communication of bibliographic and related information in machine-readable form. Working with the Library of Congress, American computer scientist Henriette Avram developed MARC in the 1960s to create records that could be read by computers and shared among libraries. A MARC record involves three elements: the record structure, the content designation, and the data content of the record.",
        UrlOnline = "https://www.loc.gov/marc/languages/language_code.html")]
    [ReferenceCollection("//table[@summary='language codes arranged in code order']//tr[position() > 1]")]
    public class MARCCodeListForLanguage
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

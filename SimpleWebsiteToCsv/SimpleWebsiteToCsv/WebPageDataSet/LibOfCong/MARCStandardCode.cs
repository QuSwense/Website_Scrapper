using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.LibOfCong
{
    [QSReference("MARC (MAchine-Readable Cataloging) 21 formats are standards for the representation and communication of bibliographic and related information in machine-readable form. Working with the Library of Congress, American computer scientist Henriette Avram developed MARC in the 1960s to create records that could be read by computers and shared among libraries. A MARC record involves three elements: the record structure, the content designation, and the data content of the record.")]
    [QSURIDataSource(Online = "https://www.loc.gov/marc/countries/countries_code.html")]
    [ReferenceCollection("//table[@summary='country codes arranged in code order']//tr[position() > 1]")]
    public class MARCStandardCode
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

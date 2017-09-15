using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.GENC
{
    /// <summary>
    /// Get the latest data from the URL
    /// https://nsgreg.nga.mil/genc/contentBaseline.jsp?authority=GENC?xyzallow
    /// </summary>
    [QSReference("https://nsgreg.nga.mil/genc/contentBaseline.jsp?authority=GENC?xyzallow")]
    [QSReferenceFromDataSource(OnlineUrl = "https://evs.nci.nih.gov/ftp1/FDA/GENC/About.html", XPath = "//body/p[position() = 1]")]
    [QSURIExcelDataSource(Online = "https://evs.nci.nih.gov/ftp1/FDA/GENC/NCIt-GENC_Terminology.xlsx")]
    public class GENCCodeFileData
    {
        [QSURIExcelFieldDataSource(SheetName = "17.02d")]
        public List<GENCCountryCodes> CountryCodes { get; set; }

        [QSURIExcelFieldDataSource(SheetName = "GENC Administrative Subdivision", StartRowIndex = 3)]
        public List<GENCCountryCodes> Subdivisions { get; set; }
    }

    [QSReferenceField("", Url = "https://evs.nci.nih.gov/ftp1/FDA/GENC/About.html")]
    public class GENCCountryCodes
    {
        [QSIndex(0)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 2]/td[position() = 2]")]
        public string NCItConceptCode { get; set; }

        [QSIndex(1)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 3]/td[position() = 2]")]
        public string NCItPreferredName { get; set; }

        [QSIndex(2)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 4]/td[position() = 2]")]
        public string GENCName_FDAStandard { get; set; }

        [UniqueDictionaryKey]
        [QSIndex(3)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 5]/td[position() = 2]")]
        public string GENC2LetterCode{ get; set; }

        [QSIndex(4)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 6]/td[position() = 2]")]
        public string GENC3LetterCode { get; set; }

        [QSIndex(5)]
        [QSReferenceFromDataSource(XPath = "//table//tr[position() = 7]/td[position() = 2]")]
        public int GENCNumericCode { get; set; }
    }

    public class GENCSubdivisions
    {
        [QSIndex(1)]
        [About("The 2 letter code for the geopolitical entity chosen by GENC.")]
        [RegexSplit(@"(\w+)\-.*")]
        public string GENC2LetterCode { get; set; }

        [UniqueDictionaryKey]
        [QSIndex(1)]
        [About("The 2 letter code for the geopolitical entity chosen by GENC.")]
        public string GENC6LetterCodeFDAStd { get; set; }

        [QSIndex(2)]
        [About("The preferred term of the geopolitical entity chosen by GENC & the FDA for the Subdivision.")]
        public string GENCName_FDAStandard { get; set; }

        [QSIndex(3)]
        [About("The preferred term of the geopolitical entity chosen by GENC & the FDA for the Subdivision.")]
        public string SubdivisionCategory { get; set; }
    }
}

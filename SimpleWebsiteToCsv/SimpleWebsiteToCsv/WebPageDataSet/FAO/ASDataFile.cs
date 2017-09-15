using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.Metadata.Attributes;
using SimpleWebsiteToCsv.Metadata.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.FAO
{
    [QSReference("http://www.fao.org/nr/water/aquastat/data/query/index.html")]
    [QSReferenceFromDataSource(OnlineUrl = "http://www.fao.org/about/en/", XPath = "//div[@class='csc-textpic-text']/p[position() = 1]")]
    [QSReferenceFromDataSource(OnlineUrl = "http://www.fao.org/statistics/en/", XPath = "//div[@id='c209068']/p[position() = 1]",
        CountSentences = 2)]
    [QSReferenceFromDataSource(OnlineUrl = "http://www.fao.org/nr/water/aquastat/help/index.stm",
        XPath = "//body/table//table//tr[position() = 1]/td[@class='main']/ol/li[position() = 3]/p[position() = 1]", CountSentences = 2)]
    [Citation(LogoOnlineUrl = "http://www.fao.org/fileadmin/templates/faoweb/images/FAO-logo.png",
       CitationString = "FAO. 2016. AQUASTAT Main Database, Food and Agriculture Organization of the United Nations (FAO). Website accessed on [12/09/2017 11:25]",
       Copyright = "FAO of the UN")]
    public class ASDataFile
    {
        [FieldOrder(1)]
        [FileIterator(StartIndex = 3, StopText = "\"\"")]
        public List<AQUASTATMainData> MainDataList { get; set; }

        [FieldOrder(2)]
        [FileIterator(StartIndex = 1, StartFromContentRegex = "Metadata:")]
        public List<AQUASTATNoteReference> NoteReferences { get; set; }
    }

    [QSRegex("((?<=\")[^\"]*(?=\"(,|$)+)|(?<=,|^)[^,\"]*(?=,|$))")]
    public class AQUASTATMainData
    {
        [QSIndex(0)]
        [RegexReplace(Pattern = @"\w+(|.*)", ReplaceText = "")]
        public string CountryCode { get; set; }

        [QSIndex(0)]
        [RegexReplace(Pattern = @"(\w+|).*", ReplaceText = "")]
        public string CountryName { get; set; }

        [QSIndex(1)]
        public int AreaId { get; set; }

        [QSIndex(2)]
        public string VariableName { get; set; }

        [QSIndex(3)]
        public int VariableId { get; set; }

        [QSIndex(4)]
        public int Year { get; set; }

        [QSIndex(5)]
        public double Value { get; set; }

        /// <summary>
        /// E - External data
        /// I - AQUASTAT estimate
        /// K - Aggregate data
        /// L - Modelled data
        /// </summary>
        [QSIndex(6)]
        public char Symbol { get; set; }

        [QSIndex(7)]
        public string Md { get; set; }

        [QSIndex(8)]
        public string NoteReference { get; set; }
    }

    [QSRegex(@"(?<Ref_id>\[\d+\]).*\|(?<Ref_Text>.*)")]
    public class AQUASTATNoteReference
    {
        [QSIndex(GroupName = "Ref_id")]
        public string NoteId { get; set; }

        [QSIndex(GroupName = "Ref_Text")]
        public string NoteText { get; set; }
    }
}

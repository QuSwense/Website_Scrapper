using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Country
{
    [CitationRegex(ClassName = "SimpleWebsiteToCsv.WebPageDataSet.Country.CountryAreaListWiki")]
    public class CountryInformationWiki
    {
        public string FlagUrl { get; set; }
        public string CoatofArmsUrl { get; set; }
        public string AnthemLine { get; set; }
        public string Capital { get; set; }
        public string CapitalCoordinates { get; set; }
    }
}

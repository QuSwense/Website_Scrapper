using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet
{
    [ReferenceSingle(XPath = "//ul[@class='expandcollapse']/li[//h2[@sectiontitle='People and Society']]/following-sibling::li")]
    public class CIAWorldFactbookCountryPeopleAndSocietyData
    {
        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Population:')]/following-sibling::div[@class='category_data']")]
        public string Population { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Nationality:')]/following-sibling::div/span[contains(text(), 'noun:')]/following-sibling::span")]
        public string NationalityNoun { get; set; }

        [ReferenceSingle(XPath = "div[@id='field' and contains(text(), 'Nationality:')]/following-sibling::div/span[contains(text(), 'adjective:')]/following-sibling::span")]
        public string NationalityAdjective { get; set; }
    }
}

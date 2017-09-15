using SimpleWebsiteToCsv.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.WebPageDataSet.Currency
{
    [Citation(Description = "This list contains the 180 currencies recognized as legal tender in United Nations (UN) member states, UN observer states, partially recognized or unrecognized states, and their dependencies. Dependencies and unrecognized states are listed here only if another currency is used in their territory that is different from the one of the state that administers them or has jurisdiction over them.",
        ReferenceUrl = "https://en.wikipedia.org/wiki/List_of_circulating_currencies",
        UrlOnline = "http://www.xe.com/currency/")]
    [ReferenceCollection("//ul[@id='popCurr']/li")]
    public class XECountryCurrency
    {
        [ReferenceSingle("a", Attribute = "href")]
        public string CurrencyUrl { get; set; }

        [ReferenceSingle("a/span")]
        [RegexSplit(@"(\w+)\s+\-\s+(\w+)")]
        public XECurrencyName ShortName { get; set; }

        [CitationRegex(PropertyName = "CurrencyUrl")]
        public XECurrencyDetails CurrencyDetails { get; set; }
    }

    public class XECurrencyName
    {
        [QSIndex(0)]
        public string ShortNameISO4217 { get; set; }

        [QSIndex(1)]
        public string FullName { get; set; }
    }

    public class XECurrencyDetails
    {
        public XECurrencyStats CurrencyStats { get; set; }
    }

    [ReferenceSingle("//div[@class='currencyStats']")]
    public class XECurrencyStats
    {
        [ReferenceSingle("p[position() = 2]")]
        [StringSplit(" ")]
        public XESymbol Symbol { get; set; }

        [ReferenceSingle("p[position() = 3]")]
        [RegexReplace(Pattern = "(Minor Unit:).*(=.*)")]
        public string MinorUnitValue { get; set; }

        [NotFixed]
        [ReferenceSingle("p[position() = 4]")]
        [RegexReplace(Pattern = "(Central Bank Rate:).*")]
        public string CentralBankRate { get; set; }
    }

    [ReferenceSingle("//div[@class='currencyProfile']")]
    public class XECurrencyProfile
    {
        [NotFixed]
        [ReferenceSingle("p[position() = 1]")]
        [RegexReplace(Pattern = "(Inflation:).*(%)")]
        public string InflationPercentage { get; set; }

        [ReferenceSingle("p[position() = 2]/span")]
        [StringSplit(",")]
        public List<string> NickNames { get; set; }

        [ReferenceSingle("p[position() = 3]", UseInnerHtml = true)]
        [RegexReplace(Pattern = @"<strong>Coins:</strong>")]
        [StringSplit("<br>")]
        public XECurrencyUsage CoinsUsed { get; set; }

        [ReferenceSingle("p[position() = 4]", UseInnerHtml = true)]
        [RegexReplace(Pattern = @"<strong>Banknotes:</strong>")]
        [StringSplit("<br>")]
        public XECurrencyUsage BanknotesUsed { get; set; }

        [ReferenceSingle("p[position() = 5]", UseInnerHtml = true)]
        [RegexReplace(Pattern = @"<strong>Central Bank:</strong>")]
        [StringSplit("<br>")]
        public XECentralBank CentralBankInfo { get; set; }

        [ReferenceSingle("p[position() = 6]", UseInnerHtml = true)]
        [RegexReplace(Pattern = @"<strong>Users:</strong>")]
        [RegexReplace(Pattern = @"<a.*")]
        [StringSplit(",")]
        public List<string> Countries { get; set; }
    }

    public class XESymbol
    {
        [QSIndex(1)]
        public string CurrencySymbol { get; set; }

        [QSIndex(2)]
        [RegexReplace(Pattern = @"\w+(:)", ReplaceText = "")]
        public string MinorUnitName { get; set; }

        [QSIndex(3)]
        public string MinorUnitSymbol { get; set; }
    }

    public class XECurrencyUsage
    {
        [QSIndex(0)]
        [RegexReplace(Pattern = @"Freq Used: ")]
        [StringSplit(",")]
        public List<string> FrequentlyUsed { get; set; }

        [QSIndex(1)]
        [RegexReplace(Pattern = @"Rarely Used: ")]
        [StringSplit(",")]
        public List<string> RarelyUsed { get; set; }
    }

    public class XECentralBank
    {
        [QSIndex(0)]
        public string Name { get; set; }

        [QSIndex(1)]
        [ReferenceSingle("a", Attribute = "href")]
        public string HomeUrl { get; set; }
    }
}

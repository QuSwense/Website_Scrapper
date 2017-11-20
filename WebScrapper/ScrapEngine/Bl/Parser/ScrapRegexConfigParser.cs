using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapRegexConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapRegexConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        public override ManipulateChildElement Process(XmlNode regexNode, ManipulateElement webConfigObj)
        {
            RegexElement configRegexObj = configParser.XmlConfigReader.ReadElement<RegexElement>(regexNode);
            configRegexObj.Pattern = HttpUtility.HtmlDecode(configRegexObj.Pattern);
            return configRegexObj;
        }

        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            RegexElement regexConfig = (RegexElement)manipulateChild;
            Match output = Regex.Match(result.Value,
                            regexConfig.Pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            result.Value = output.Groups[regexConfig.Index].Value;
        }
    }
}

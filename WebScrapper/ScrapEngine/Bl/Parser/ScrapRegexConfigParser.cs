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
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The business logic class which parses a Regex tag and interprets the results
    /// </summary>
    public class ScrapRegexConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapRegexConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Parse the Regex element
        /// </summary>
        /// <param name="regexNode"></param>
        /// <param name="webConfigObj"></param>
        /// <returns></returns>
        public override ManipulateChildElement Process(XmlNode regexNode, ManipulateElement webConfigObj)
        {
            RegexElement configRegexObj = configParser.XmlConfigReader.ReadElement<RegexElement>(regexNode);
            configRegexObj.Pattern = HttpUtility.HtmlDecode(configRegexObj.Pattern);
            return configRegexObj;
        }

        /// <summary>
        /// Process multiple results
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(result.OriginalValue)) return;

            RegexElement regexConfig = (RegexElement)manipulateChild;

            List<string> finalResults = new List<string>();
            Regex regexPattern = new Regex(regexConfig.Pattern);

            for(int i = 0; i < result.Results.Count; ++i)
            {
                Match output = regexPattern.Match(result.Results[i]);

                if (regexConfig.Index >= 0 && output.Groups.Count > regexConfig.Index)
                        finalResults.Add(output.Groups[regexConfig.Index].Value);
                else if (regexConfig.Index < 0)
                    foreach (Group group in output.Groups)
                        finalResults.Add(group.Value);
                else finalResults.Add(result.Results[i]);
            }

            result.Results = new List<string>(finalResults);
        }

        /// <summary>
        /// Assert regex element
        /// </summary>
        /// <param name="regexElement"></param>
        private void Assert(RegexElement regexElement)
        {
            if (string.IsNullOrEmpty(regexElement.Pattern))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_DATA_EMPTY);
            else if (regexElement.Index < 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_SPLIT_INDEX_INVALID,
                    new string[] { regexElement.Index.ToString() });
        }
    }
}

using ScrapEngine.Model;
using ScrapEngine.Model.ScrapXml;
using System.Collections.Generic;
using System.Xml;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The scrap replace config parser which manipulates the data with the regex option
    /// </summary>
    public class ScrapReplaceConfigParser : ScrapManipulateChildConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapReplaceConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }
        
        /// <summary>
        /// The main method to process the data
        /// </summary>
        /// <param name="manipulateHtml"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData manipulateHtml, ManipulateChildElement manipulateChild)
        {
            if (string.IsNullOrEmpty(manipulateHtml.OriginalValue)) return;

            ReplaceElement replaceConfig = (ReplaceElement)manipulateChild;

            List<string> finalResults = new List<string>();
            for (int i = 0; i < manipulateHtml.Results.Count; ++i)
                finalResults.Add(manipulateHtml.Results[i].Replace(replaceConfig.InString, replaceConfig.OutString));

            manipulateHtml.Results = new List<string>(finalResults);
        }

        private void Assert(ReplaceElement replaceElement)
        {
            if (string.IsNullOrEmpty(replaceElement.InString))
                throw new ScrapParserException(ScrapParserException.EErrorType.MANIPULATE_REPLACE_INSTRING_EMPTY);
        }
    }
}

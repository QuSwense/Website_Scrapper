using ScrapEngine.Model;
using ScrapEngine.Model.Scrap;
using System.Collections.Generic;
using System.Web;

namespace ScrapEngine.Bl.Parser
{
    public class ScrapHtmlDecodeConfigParser : ScrapManipulateChildConfigParser
    {
        public ScrapHtmlDecodeConfigParser() { }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapHtmlDecodeConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Virtual process
        /// </summary>
        /// <param name="result"></param>
        /// <param name="manipulateChild"></param>
        public override void Process(ManipulateHtmlData result, ManipulateChildElement manipulateChild)
        {
            List<string> finalList = new List<string>();

            for (int i = 0; i < result.Results.Count; i++)
            {
                finalList.Add(HttpUtility.HtmlDecode(result.Results[i]));
            }

            result.Results = new List<string>(finalList);
        }
    }
}

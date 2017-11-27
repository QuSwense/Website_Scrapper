using ScrapEngine.Interfaces;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The base class for the scrap xml app topic engine
    /// </summary>
    public class AppTopicConfigParser : IInnerBaseParser
    {
        /// <summary>
        /// The config parser
        /// </summary>
        protected WebScrapConfigParser configParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public AppTopicConfigParser(WebScrapConfigParser configParser)
        {
            this.configParser = configParser;
        }

        protected string Normalize(string htmlValue)
        {
            if (string.IsNullOrEmpty(htmlValue)) return htmlValue;
            return htmlValue.Replace("\\n", "\n").Replace("\\t", "\t");
        }
    }
}

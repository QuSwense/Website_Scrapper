using ScrapEngine.Interfaces;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The base class for the scrap xml app topic engine
    /// </summary>
    public class AppTopicConfigParser : IInnerBaseParser
    {
        /// <summary>
        /// The reference to the config parser engine
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

        public virtual void Reset() { }
    }
}

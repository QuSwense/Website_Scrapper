using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System;
using System.Xml.XPath;
using WebCommon.Error;

namespace ScrapEngine.Bl.Parser
{
    /// <summary>
    /// The base parser class for Scrap node
    /// </summary>
    public class ScrapConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Scrap column config parser
        /// </summary>
        protected ScrapColumnConfigParser scrapColumnConfigParser;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapConfigParser(WebScrapConfigParser configParser)
            : base(configParser)
        {
            scrapColumnConfigParser = new ScrapColumnConfigParser(configParser);
        }

        /// <summary>
        /// Start Processing from the Scrap Html node
        /// </summary>
        public virtual void Process() { }

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        protected void AssertLevelConstraint()
        {
            ScrapElement tmpObj = configParser.StateModel.CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj;
            int level = 0;
            for (; tmpObj != null && level <= configParser.AppConfig.ScrapMaxLevel();
                level++, tmpObj = tmpObj.Parent) ;

            if (level > configParser.AppConfig.ScrapMaxLevel() || level <= 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
                    level.ToString());
        }

        /// <summary>
        /// The Scrap element tag (and its child Scrap tags) should contain one and only one name 
        /// attribute
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        protected void AssertScrapNameAttribute()
        {
            bool isTableNameFound = false;
            string NameValue = null;
            ScrapElement tmpObj = configParser.StateModel.CurrentColumnScrapIteratorArgs.Parent.ScrapConfigObj;

            while (tmpObj != null)
            {
                if (!string.IsNullOrEmpty(tmpObj.Name))
                {
                    if (isTableNameFound)
                        throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_MULTIPLE);
                    isTableNameFound = true;
                    NameValue = tmpObj.Name;
                }

                tmpObj = tmpObj.Parent;
            }

            if (!isTableNameFound || string.IsNullOrEmpty(NameValue))
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_NAME_EMPTY);
        }

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        protected void ParseUrlValue(ScrapIteratorArgs childScrapIteratorArgs)
        {
            string urlValue = childScrapIteratorArgs.ScrapConfigObj.Url;

            if (childScrapIteratorArgs.ScrapConfigObj.Parent == null ||
                string.IsNullOrEmpty(childScrapIteratorArgs.ScrapConfigObj.Parent.Url)) return;
            if (urlValue != null && !urlValue.Contains("{parentValue}")) return;

            urlValue = urlValue.Replace("{parentValue}", "");

            if (string.IsNullOrEmpty(urlValue))
            {
                urlValue = new Uri(new Uri(childScrapIteratorArgs.ScrapConfigObj.Parent.Url),
                childScrapIteratorArgs.Parent.WebHtmlNode.Value).AbsoluteUri;
            }
            else
            {
                XPathNavigator htmlNode =
                    childScrapIteratorArgs.Parent.WebHtmlNode.SelectSingleNode(urlValue);
                urlValue = new Uri(new Uri(childScrapIteratorArgs.ScrapConfigObj.Parent.Url),
                htmlNode.Value).AbsoluteUri;
            }

            childScrapIteratorArgs.ScrapConfigObj.UrlCalculated = urlValue;
        }

        /// <summary>
        /// A common method to call COlumn parser with arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        protected void ProcessColumnParser()
        {
            // Check the constraints on the Scrap nodes
            // 1. Only maximum 4 levels is allowed
            // 2. Only one "name" tag should be present from the top level to bottom Scrap
            //    If multiple "name" tag is present throw error
            AssertLevelConstraint();
            AssertScrapNameAttribute();

            // Read the Column nodes which are the individual reader config nodes
            scrapColumnConfigParser.Process();
        }
    }
}

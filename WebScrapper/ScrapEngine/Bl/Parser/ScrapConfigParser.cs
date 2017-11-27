using HtmlAgilityPack;
using ScrapEngine.Model;
using ScrapEngine.Model.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
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
        public virtual void Process(ScrapIteratorArgs args) { }

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        protected void AssertLevelConstraint(ScrapElement webScrapConfigObj)
        {
            ScrapElement tmpObj = webScrapConfigObj;
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
        protected void AssertScrapNameAttribute(ScrapElement webScrapConfigObj)
        {
            bool isTableNameFound = false;
            string NameValue = null;
            ScrapElement tmpObj = webScrapConfigObj;

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
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapConfigObj"></param>
        /// <returns></returns>
        protected T ParseScrapElementAttributes<T>(ScrapIteratorArgs args)
            where T : ScrapElement, new()
        {
            var webScrapConfigObj =
                configParser.XmlConfigReader.ReadElement<T>(args.ScrapConfigNode);
            webScrapConfigObj.Parent = args.ScrapConfigObj;
            webScrapConfigObj.Url = ParseUrlValue(webScrapConfigObj, args.WebHtmlNode);
            return webScrapConfigObj;
        }

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        protected string ParseUrlValue(ScrapElement configScrap, HtmlNodeNavigator htmlNode)
        {
            Debug.Assert(!(configScrap == null));

            string urlValue = configScrap.Url;

            if (string.IsNullOrEmpty(urlValue) ||
                configScrap.Parent == null ||
                string.IsNullOrEmpty(configScrap.Parent.Url)) return urlValue;
            if (!urlValue.StartsWith("@")) return urlValue;
            if (htmlNode == null) return urlValue;

            if (urlValue.Contains("{parentValue}"))
            {
                urlValue = new Uri(new Uri(configScrap.Parent.Url),
                    htmlNode.Value).AbsoluteUri;
            }

            return urlValue;
        }

        /// <summary>
        /// Create new args to pass to Process method
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual ScrapIteratorArgs CreateArgs(ScrapIteratorArgs args, XmlNode nextChildNode) { return null; }

        /// <summary>
        /// A common method to call COlumn parser with arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        protected void ProcessColumnParser<T>(T args)
            where T : ColumnScrapIteratorArgs
        {
            // Check the constraints on the Scrap nodes
            // 1. Only maximum 4 levels is allowed
            // 2. Only one "name" tag should be present from the top level to bottom Scrap
            //    If multiple "name" tag is present throw error
            AssertLevelConstraint(args.ScrapConfig);
            AssertScrapNameAttribute(args.ScrapConfig);

            // Read the Column nodes which are the individual reader config nodes
            scrapColumnConfigParser.Process(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <param name="iteratorArgs"></param>
        protected void SetCurrentState<T>(ScrapIteratorArgs args, T newIterator, ref T currentIterator)
            where T : ScrapIteratorArgs
        {
            // Only the root of Scrap nodes contains the 'id' attribute
            if (!string.IsNullOrEmpty(newIterator.ScrapConfigObj.IdString))
            {
                currentIterator = newIterator;

                if(!configParser.WebScrapStates.ContainsKey(currentIterator.ScrapConfigObj.IdString))
                    configParser.WebScrapStates.Add(currentIterator.ScrapConfigObj.IdString, 
                        new List<ScrapIteratorArgs>());
                configParser.WebScrapStates[currentIterator.ScrapConfigObj.IdString].Add(currentIterator);
            }
            else
            {
                if (currentIterator != null)
                {
                    newIterator.Parent = currentIterator;
                    currentIterator.Child.Add(newIterator);
                    currentIterator = newIterator;
                }
            }
        }
    }
}

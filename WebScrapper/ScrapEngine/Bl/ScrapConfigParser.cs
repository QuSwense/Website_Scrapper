using HtmlAgilityPack;
using ScrapEngine.Interfaces;
using ScrapEngine.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebCommon.Error;

namespace ScrapEngine.Bl
{
    public class ScrapConfigParser : AppTopicConfigParser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configParser"></param>
        public ScrapConfigParser(WebScrapConfigParser configParser)
            : base(configParser) { }

        /// <summary>
        /// Check the maximum level of Scrap nodes allowed is 4
        /// </summary>
        /// <param name="webScrapConfigObj">The last child Scrap node</param>
        protected void AssertLevelConstraint(WebDataConfigScrap webScrapConfigObj)
        {
            WebDataConfigScrap tmpObj = webScrapConfigObj;
            int level = 0;
            for (; tmpObj != null && level <= configParser.AppConfig.ScrapMaxLevel(); level++, tmpObj = tmpObj.Parent) ;

            if (level > configParser.AppConfig.ScrapMaxLevel() || level <= 0)
                throw new ScrapParserException(ScrapParserException.EErrorType.SCRAP_LEVEL_INVALID,
                    level.ToString());
        }

        /// <summary>
        /// The Scrap element tag (and its child Scrap tags) should contain one and only one name 
        /// attribute
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        protected void AssertScrapNameAttribute(WebDataConfigScrap webScrapConfigObj)
        {
            bool isTableNameFound = false;
            string NameValue = null;
            WebDataConfigScrap tmpObj = webScrapConfigObj;

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
        protected T ParseScrapElementAttributes<T>(XmlNode scrapNode,
            WebDataConfigScrap parentConfigScrap, HtmlNodeNavigator htmlNode)
            where T : WebDataConfigScrap, new()
        {
            var webScrapConfigObj =
                configParser.XmlConfigReader.ReadElement<T>(scrapNode);
            webScrapConfigObj.Parent = parentConfigScrap;
            webScrapConfigObj.Url = ParseUrlValue(webScrapConfigObj, htmlNode);
            return webScrapConfigObj;
        }

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        protected string ParseUrlValue(WebDataConfigScrap configScrap, HtmlNodeNavigator htmlNode)
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
    }
}

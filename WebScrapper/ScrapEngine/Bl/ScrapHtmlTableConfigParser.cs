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

namespace ScrapEngine.Bl
{
    public class ScrapHtmlTableConfigParser : ScrapConfigParser
    {
        protected WebScrapParserStateModel startState;

        public ScrapHtmlTableConfigParser(WebScrapConfigParser configParser,
            WebScrapParserStateModel startState) : base(configParser)
        {
            this.startState = startState;
        }

        public void Run()
        {
            WebDataConfigScrapHtmlTable webScrapConfigObj =
                ParseScrapElementAttributes(startState);

            // This finally scraps the html webpage data
            List<HtmlNodeNavigator> webNodeNavigatorList = FetchHtmlTable(webScrapConfigObj);

            // Process
            int nodeIndex = 0;
            foreach (var webNodeNavigator in webNodeNavigatorList)
            {
                // Create state
                WebScrapParserStateModel scrapState = new WebScrapParserStateModel()
                {
                    ConfigScrap = webScrapConfigObj,
                    CurrentXmlNode = startState.CurrentXmlNode,
                    CurrentHtmlNode = webNodeNavigator
                };

                // Read the child Scraps nodes which are the individual reader config nodes
                configParser.ParseChildScrapNodes(scrapState);

                // Check the constraints on the Scrap nodes
                // 1. Only maximum 4 levels is allowed
                // 2. Only one "name" tag should be present from the top level to bottom Scrap
                //    If multiple "name" tag is present throw error
                CheckMaxLevelConstraint(webScrapConfigObj);
                CheckScrapNameAttribute(webScrapConfigObj);

                // Read the Column nodes which are the individual reader config nodes
                new ScrapColumnConfigParser(configParser,
                    new WebScrapParserColumnStateModel()
                    {
                        NodeIndex = nodeIndex,
                        ScrapState = scrapState
                    }, this).Run();

                nodeIndex++;
            }
        }

        /// <summary>
        /// Fetch the list of html nodes as per the web scrap html config from a Html table
        /// </summary>
        /// <param name="webScrapConfigObj"></param>
        /// <returns></returns>
        private List<HtmlNodeNavigator> FetchHtmlTable(WebDataConfigScrapHtmlTable webScrapConfigObj)
        {
            HtmlNode htmlDoc = configParser.ScrapperCommand.Load(webScrapConfigObj.Url);
            return configParser.ScrapperCommand.ReadNodes(htmlDoc, webScrapConfigObj.XPath);
        }

        /// <summary>
        /// Parse scrap element attributes
        /// </summary>
        /// <param name="scrapNode"></param>
        /// <param name="parentScrapConfigObj"></param>
        /// <returns></returns>
        private WebDataConfigScrapHtmlTable ParseScrapElementAttributes(WebScrapParserStateModel state)
        {
            WebDataConfigScrapHtmlTable webScrapConfigObj =
                configParser.XmlConfigReader.ReadElement<WebDataConfigScrapHtmlTable>(state.CurrentXmlNode);
            webScrapConfigObj.Parent = state.ConfigScrap;
            webScrapConfigObj.Url = ParseUrlValue(new WebScrapParserStateModel()
            {
                ConfigScrap = webScrapConfigObj,
                CurrentHtmlNode = state.CurrentHtmlNode
            });
            return webScrapConfigObj;
        }

        /// <summary>
        /// Parse url value
        /// </summary>
        /// <param name="urlValue"></param>
        /// <param name="scrapNode"></param>
        /// <returns></returns>
        private string ParseUrlValue(WebScrapParserStateModel state)
        {
            Debug.Assert(!(state == null || state.ConfigScrap == null));

            string urlValue = state.ConfigScrap.Url;

            if (string.IsNullOrEmpty(urlValue) ||
                state.ConfigScrap.Parent == null ||
                string.IsNullOrEmpty(state.ConfigScrap.Parent.Url)) return urlValue;
            if (!urlValue.StartsWith("@")) return urlValue;
            if (state.CurrentHtmlNode == null) return urlValue;

            if (urlValue.Contains("{parentValue}"))
            {
                urlValue = new Uri(new Uri(state.ConfigScrap.Parent.Url),
                    state.CurrentHtmlNode.Value).AbsoluteUri;
            }

            return urlValue;
        }
    }
}

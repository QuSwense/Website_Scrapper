using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ScrapEngine.Model
{
    /// <summary>
    /// This is a temporar ymodel class to store the current states of the web scrapping
    /// parser.
    /// </summary>
    public class WebScrapStateModel
    {
        /// <summary>
        /// Points to a Xml node of scrap config xml
        /// </summary>
        public XmlNode ConfigXmlNode { get; set; }

        /// <summary>
        /// An instance of the webdata config xml parsed xml node
        /// </summary>
        public WebDataConfigScrap ConfigScrapObj { get; set; }

        /// <summary>
        /// The html nodes while parsing the webpage using the config xml data
        /// </summary>
        public List<HtmlNodeNavigator> HtmlNodes { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configXmlNode"></param>
        /// <param name="configScrapObj"></param>
        /// <param name="HtmlNodes"></param>
        public WebScrapStateModel(XmlNode configXmlNode, WebDataConfigScrap configScrapObj, List<HtmlNodeNavigator> htmlNodes)
        {
            ConfigXmlNode = configXmlNode;
            ConfigScrapObj = configScrapObj;
            HtmlNodes = htmlNodes;
        }
    }
}

﻿using HtmlAgilityPack;
using System.Xml;

namespace ScrapEngine.Model
{
    public class WebScrapParserStateModel
    {
        public XmlNode CurrentXmlNode { get; set; }
        public ScrapElement ConfigScrap { get; set; }
        public HtmlNodeNavigator CurrentHtmlNode { get; set; }
    }
}

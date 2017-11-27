using HtmlAgilityPack;
using System.Xml.XPath;

namespace ScrapEngine.Model.Parser
{
    public class ColumnScrapIteratorHtmlArgs : ColumnScrapIteratorArgs
    {
        public HtmlNodeNavigator WebHtmlNode { get; set; }
        
        public override string GetDataIterator(ColumnElement columnConfig)
        {
            XPathNavigator htmlPathNav = WebHtmlNode.SelectSingleNode(columnConfig.XPath);
            if (htmlPathNav != null) return htmlPathNav.Value;
            else return null;
        }
    }
}

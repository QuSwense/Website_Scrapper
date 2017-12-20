using HtmlAgilityPack;
using System.Xml.XPath;

namespace ScrapEngine.Model.Parser
{
    public class ColumnScrapIteratorHtmlArgs : ColumnScrapIteratorArgs
    {
        public HtmlNodeNavigator WebHtmlNode { get; set; }
        
        public override string GetDataIterator()
        {
            var columnConfig = ColumnElementConfig;

            if (ColumnElementConfig.Level < 0)
            {
                XPathNavigator htmlPathNav = WebHtmlNode.SelectSingleNode(columnConfig.XPath);
                if (htmlPathNav != null)
                {
                    if (columnConfig.ValueAsInnerHtml) return htmlPathNav.InnerXml;
                    else return htmlPathNav.Value;
                }
                else return null;
            }
            else
                base.PreProcess();
            //{
            //    ScrapIteratorArgs scrapIteratorArgs = Parent;

            //    while (scrapIteratorArgs.ScrapConfigObj != null &&
            //        scrapIteratorArgs.ScrapConfigObj.Level != columnConfig.Level)
            //        scrapIteratorArgs = scrapIteratorArgs.Parent;

            //    if (scrapIteratorArgs == null || scrapIteratorArgs.ScrapConfigObj == null) return null;
            //    else
            //    {
            //        XPathNavigator htmlPathNav = scrapIteratorArgs.WebHtmlNode.SelectSingleNode(columnConfig.XPath);
            //        if (htmlPathNav != null)
            //        {
            //            if (columnConfig.ValueAsInnerHtml) return htmlPathNav.InnerXml;
            //            else return htmlPathNav.Value;
            //        }
            //    }
            //}

            return null;
        }
    }
}

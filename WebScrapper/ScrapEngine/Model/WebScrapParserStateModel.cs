using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ScrapEngine.Model
{
    public class WebScrapParserStateModel
    {
        public XmlNode CurrentXmlNode { get; set; }
        public WebDataConfigScrap ConfigScrap { get; set; }
        public HtmlNodeNavigator CurrentHtmlNode { get; set; }
    }
}

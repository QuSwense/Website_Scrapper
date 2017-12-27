using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The base arguments class for any Scrap type elemnt config
    /// </summary>
    public class ScrapStateModel : ParserStateModel
    {
        /// <summary>
        /// The final url calculated
        /// </summary>
        public string AbsoluteUrl { get; set; }

        /// <summary>
        /// The web html node
        /// </summary>
        public HtmlNodeNavigator WebHtmlNode { get; set; }
    }
}

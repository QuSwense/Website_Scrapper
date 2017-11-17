using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The root element of the scrap xml config
    /// </summary>
    public class WebDataConfigScrapHtmlTable : WebDataConfigScrap
    {
        /// <summary>
        /// The name of the Element tag
        /// </summary>
        public static string TagName = "ScrapHtmlTable";

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("xpath")]
        public string XPath { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebDataConfigScrapHtmlTable()
        {
        }
    }
}

using System.Collections.Generic;
using WebReader.Model;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The root element of the scrap xml config
    /// </summary>
    public class WebDataConfigScrap
    {
        /// <summary>
        /// The list of child Scrap element
        /// </summary>
        public List<WebDataConfigScrap> Scraps { get; set; }

        /// <summary>
        /// The name of the node
        /// </summary>
        [DXmlAttribute("name", IsMandatory =true)]
        public string Name { get; set; }

        /// <summary>
        /// The url
        /// </summary>
        [DXmlAttribute("url", IsMandatory = true)]
        public string Url { get; set; }

        /// <summary>
        /// The database table name where the web scrapped data is inserted / stored
        /// </summary>
        [DXmlAttribute("dbtbl")]
        public string DbTable { get; set; }

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("xpath", IsMandatory =true)]
        public string XPath { get; set; }

        /// <summary>
        /// The type of the scrap node
        /// </summary>
        [DXmlAttribute("type", IsMandatory =true)]
        public EWebDataConfigType Type { get; set; }

        /// <summary>
        /// The list of column nodes
        /// </summary>
        public List<WebDataConfigColumn> Columns { get; set; }

        /// <summary>
        /// The state of the node
        /// </summary>
        public WebPageHtmlState State { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        public WebDataConfigScrap Parent { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public WebDataConfigScrap()
        {
            State = new WebPageHtmlState();
            Scraps = new List<WebDataConfigScrap>();
            Columns = new List<WebDataConfigColumn>();
        }
    }
}

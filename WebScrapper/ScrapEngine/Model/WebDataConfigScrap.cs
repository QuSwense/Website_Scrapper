using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public abstract class WebDataConfigScrap
    {
        /// <summary>
        /// The list of child Scrap element
        /// </summary>
        public List<WebDataConfigScrap> Scraps { get; set; }

        /// <summary>
        /// The name of the node
        /// </summary>
        [DXmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// The url
        /// </summary>
        [DXmlAttribute("url")]
        public string Url { get; set; }

        /// <summary>
        /// The type of the scrap node
        /// </summary>
        [DXmlAttribute("doupdateonly")]
        public bool DoUpdateOnly { get; set; }

        /// <summary>
        /// The list of column nodes
        /// </summary>
        public List<WebDataConfigColumn> Columns { get; set; }

        /// <summary>
        /// The parent node
        /// </summary>
        public WebDataConfigScrap Parent { get; set; }

        public WebDataConfigScrap()
        {
            Scraps = new List<WebDataConfigScrap>();
            Columns = new List<WebDataConfigColumn>();
            DoUpdateOnly = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model
{
    public class WebDataConfigScrapCsv : WebDataConfigScrap
    {
        /// <summary>
        /// The name of the Element tag
        /// </summary>
        public static string TagName = "ScrapCsv";

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("delimiter")]
        public string Delimiter { get; set; }

        /// <summary>
        /// The Xpath
        /// </summary>
        [DXmlAttribute("skipfirst")]
        public int SkipFirstLines { get; set; }

        public WebDataConfigScrapCsv()
        {
            SkipFirstLines = 0;
            Delimiter = "\\t";
        }
    }
}

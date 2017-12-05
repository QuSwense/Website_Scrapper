using ScrapEngine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReader.Model;

namespace ScrapEngine.Model.ScrapXml
{
    /// <summary>
    /// The root element of a Xml configuration file for scrapping web data from a website or
    /// multiple websites.
    /// </summary>
    [DXmlElement(ScrapXmlConsts.WebDataNodeName)]
    public class WebDataElement
    {
        [DXmlElement(ScrapXmlConsts.ScrapCsvNodeName, DerivedType = typeof(ScrapCsvElement))]
        [DXmlElement(ScrapXmlConsts.ScrapHtmlTableNodeName, DerivedType = typeof(ScrapHtmlTableElement))]
        public List<ScrapElement> Scraps { get; set; }

        public WebDataElement()
        {
            Scraps = new List<ScrapElement>();
        }
    }
}

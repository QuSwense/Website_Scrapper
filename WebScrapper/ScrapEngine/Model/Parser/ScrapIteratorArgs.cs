using HtmlAgilityPack;
using System.Collections.Generic;
using System.Xml;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// The class which stores teh state of a Scrap Element configuration
    /// </summary>
    public class ScrapIteratorArgs
    {
        /// <summary>
        /// The parsed and interpreted Scrap element from the configutaion file
        /// </summary>
        public ScrapElement ScrapConfigObj { get; set; }

        /// <summary>
        /// Represents the raw Xml Node (part of XmlDOcument) which represents the
        /// <see cref="ScrapConfigObj"/>
        /// </summary>
        public XmlNode ScrapConfigNode { get; set; }

        /// <summary>
        /// The raw Html node of HtmlDocument element
        /// </summary>
        public HtmlNodeNavigator WebHtmlNode { get; set; }

        /// <summary>
        /// The parent reference
        /// </summary>
        public ScrapIteratorArgs Parent { get; set; }

        /// <summary>
        /// Child reference
        /// </summary>
        public List<ScrapIteratorArgs> Child { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ScrapIteratorArgs()
        {
            Child = new List<ScrapIteratorArgs>();
        }
    }
}

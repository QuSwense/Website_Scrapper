using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWWebScrap.Model
{
    /// <summary>
    /// This class contains the Xpath syntax to identify the node whose text needs to be
    /// scrapped.
    /// </summary>
    public class XPathHint
    {
        /// <summary>
        /// The Xpath string
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// The name of attribute we want to pick the value from.
        /// If null, then we are interested with only the Node text
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// A Html Node can be retrived from XPath using the method <see cref="HtmlAgilityPack.HtmlNodeCollection"/>
        /// If this is a single node, specify the property value as 0.
        /// In case this is a collection node, specify the value as -1.
        /// </summary>
        public int CollectionIndex { get; set; }

        /// <summary>
        /// Default constructor must for IList. Different to Parameterless constructor
        /// </summary>
        public XPathHint() { }

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="attrobute"></param>
        public XPathHint(string xpath, int collIndex = -1, string attribute = null)
        {
            XPath = xpath;
            Attribute = attribute;
            CollectionIndex = collIndex;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QWWebScrap.Model
{
    /// <summary>
    /// This class contains the full html reference data to identify a resource.
    /// It contains a Uri and an (Xpath, Attribute).
    /// Uri can be null, in that case, take the Uri from Parent
    /// </summary>
    public class HtmlPathHint
    {
        /// <summary>
        /// The Url of the resource.
        /// If this is null, then the Url of parent is obtained recursivley
        /// </summary>
        public UriHint Url { get; set; }

        /// <summary>
        /// The path of the resource in the resource
        /// </summary>
        public XPathHint Path { get; set; }

        /// <summary>
        /// Default constructor must for IList. Different to Parameterless constructor
        /// </summary>
        public HtmlPathHint() { }
    }
}

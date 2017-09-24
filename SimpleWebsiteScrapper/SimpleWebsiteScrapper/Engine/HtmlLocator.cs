using HtmlAgilityPack;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// A class which completely can identify any piece of data in a webpage using 
    /// <see cref="HtmlAgilityPack"/>
    /// </summary>
    public class HtmlLocator
    {
        /// <summary>
        /// If this is a collection of node, set it to -1,
        /// If this is a single node set it to 0
        /// </summary>
        public int CollectionIndex { get; set; }

        /// <summary>
        /// The Node collection object
        /// </summary>
        public HtmlNodeCollection NodeCollection { get; set; }

        /// <summary>
        /// The node attribute object. Null if not defined
        /// </summary>
        public HtmlAttribute NodeAttribute { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlLocator()
        {
            CollectionIndex = -1;
        }
    }
}

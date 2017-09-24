using System;

namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// A class to completely identify a piece of information needed to be retrieved from a uri.
    /// It contains the location of the resource as defined by <see cref="UrlNode"/> property and
    /// the location of the piece of data as defined by <see cref="XPathNode"/>
    /// </summary>
    public class HtmlPathProcessorNode
    {
        /// <summary>
        /// The property which stores the location of the resource either locally or online
        /// </summary>
        public UrlProcessorNode UrlNode { get; set; }

        /// <summary>
        /// The property which stores the location of the data to be scrapped
        /// </summary>
        public XPathProcessorNode XPathNode { get; set; }

        /// <summary>
        /// A static method to create a new instance of <see cref="XPathProcessorNode"/>
        /// </summary>
        /// <returns></returns>
        public static HtmlPathProcessorNode New() => new HtmlPathProcessorNode();

        /// <summary>
        /// A memebr method to clone data to 'this' node from another node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public HtmlPathProcessorNode Clone(HtmlPathProcessorNode location)
        {
            UrlNode = UrlProcessorNode.New().Clone(location.UrlNode);
            XPathNode = XPathProcessorNode.New().Clone(location.XPathNode);

            return this;
        }

        /// <summary>
        /// A static method to create a new node and clone data from an existing node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public static HtmlPathProcessorNode CloneNew(HtmlPathProcessorNode node) => New().Clone(node);

        /// <summary>
        /// Sets the url of the resource (Online by default)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="isoffline"></param>
        /// <returns></returns>
        public HtmlPathProcessorNode SetUrl(string url, bool isoffline = false)
        {
            if (UrlNode == null) UrlNode = UrlProcessorNode.New();
            if (!isoffline) UrlNode.Online = new Uri(url);
            return this;
        }
    }
}

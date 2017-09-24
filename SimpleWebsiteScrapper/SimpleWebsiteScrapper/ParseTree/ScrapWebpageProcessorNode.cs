namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// The main class derived from <see cref="ScrapBaseProcessorNode"/> which is stores all type of processing
    /// information to scrap a piece of data from a website. It also stores metadata information about the data scraped from the
    /// website.
    /// The location of the data is stored in the base class variable <see cref="ScrapBaseProcessorNode.HtmlFeature"/>
    /// </summary>
    public class ScrapWebpageProcessorNode : ScrapBaseProcessorNode
    {
        /// <summary>
        /// A list of copyright data (A must when you are scraping data fro ma website)
        /// One must provide and include the copyright information (if any) as unless it might be
        /// legal obligations
        /// </summary>
        public ScrapBaseProcessorNodeList Copyrights { get; set; }

        /// <summary>
        /// A reference is like Bibliography or links to the actual source from where the data is scrapped.
        /// It can be null in that case it inherits the property of its parent
        /// </summary>
        public ScrapBaseProcessorNodeList References { get; set; }

        /// <summary>
        /// List of child nodes.
        /// In case the current node is a <see cref="XPathProcessorNode.EHtmlNodeType.Single"/> as identified by
        /// <see cref="XPathProcessorNode.NodeType"/> present in <see cref="ScrapBaseProcessorNode.HtmlFeature"/>, 
        /// it contains only a single data. In case it is <see cref="XPathProcessorNode.EHtmlNodeType.Collection"/>,
        /// it contains a list of data.
        /// If this list is null then this node is a terminal node.
        /// </summary>
        public ScrapWebpageProcessorNodeList Nodes { get; set; }

        /// <summary>
        /// A default constructor as required by a <see cref="List{T}"/>
        /// It is different from a parameterless constructor
        /// </summary>
        public ScrapWebpageProcessorNode() { }

        /// <summary>
        /// Constructor with unique name
        /// </summary>
        /// <param name="name"></param>
        public ScrapWebpageProcessorNode(string name = "") : base(name) { }
    }
}

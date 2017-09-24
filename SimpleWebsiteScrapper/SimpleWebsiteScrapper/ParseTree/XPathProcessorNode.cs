namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// This class contains information on the specific element / attribute inside an html document
    /// It must contain <see cref="XPath"/> and <see cref="NodeType"/>. <see cref="Attribute"/> can be null.
    /// if <see cref="NodeType"/> is equal to <see cref="EHtmlNodeType.Single"/>, the <see cref="HtmlAgilityPack.HtmlNode"/> 
    /// is fetched else if equal to <see cref="EHtmlNodeType.Collection"/>, the
    /// <see cref="HtmlAgilityPack.HtmlNodeCollection"/> is fetched.
    /// If <see cref="NodeType"/> is <see cref="EHtmlNodeType.None"/> then the processor ignores the parsing.
    /// The <see cref="XPathProcessorNode"/> class instance identifies only one type of node at a time, either
    /// a Single node, a collection node or a Attribute node
    /// </summary>
    public class XPathProcessorNode : ProcessorNode<XPathProcessorNode>
    {
        /// <summary>
        /// Identifies the type of Node(s) that is needed to be fetched using this processor guidelines
        /// <see cref="EHtmlNodeType"/>
        /// </summary>
        public EHtmlNodeType NodeType { get; set; }

        /// <summary>
        /// XPath is a terminology in Html which means XML Path Language.
        /// </summary>
        public string XPath { get; set; }

        /// <summary>
        /// The name of the attribute on the node which is retrieved by <see cref="XPath"/> 
        /// If Attribute is present, ie., not null or empty then <see cref="XPathProcessorNode"/> class
        /// represents a Attribute node
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// A memebr method to clone data to 'this' node from another node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public override XPathProcessorNode Clone(XPathProcessorNode node)
        {
            if(node != null)
            {
                XPath = node.XPath;
                Attribute = node.Attribute;
            }

            return this;
        }
    }
}

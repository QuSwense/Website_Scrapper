namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// An enumerator class to identify the type of processing required to fetch data from the Html page.
    /// It identifies whether the <see cref="XPath"/> is to be used for retrieving single or collection of nodes
    /// Thereby, it makes a clear distinction between the calls <see cref="HtmlAgilityPack.HtmlNode.SelectSingleNode(string)"/>
    /// and <see cref="HtmlAgilityPack.HtmlNode.SelectNodes(string)"/>
    /// </summary>
    public enum EHtmlNodeType
    {
        /// <summary>
        /// The Default value. Any kind of processing is skipped
        /// </summary>
        None,

        /// <summary>
        /// The <see cref="XPath"/> defined refers to a Single Node.
        /// use <see cref="HtmlAgilityPack.HtmlNode.SelectSingleNode(string)"/> to get the nodes
        /// </summary>
        Single,

        /// <summary>
        /// The <see cref="XPath"/> defined refers to a collection of nodes
        /// Use <see cref="HtmlAgilityPack.HtmlNode.SelectNodes(string)"/> to get the node
        /// </summary>
        Collection
    }
}

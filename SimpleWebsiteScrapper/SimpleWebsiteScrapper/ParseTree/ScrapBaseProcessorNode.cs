using System;

namespace SimpleWebsiteScrapper.ParseTree
{
    /// <summary>
    /// A class which forms the basis of a unit of data and metadata of the data.
    /// This class contains all the information, that forms the basis for retrieval or
    /// manipulation of data retrieval from Online/Offline location. It also contains metadata
    /// information related to the data like copyright, image icon resources, etc.
    /// </summary>
    public class ScrapBaseProcessorNode : ProcessorNode<ScrapBaseProcessorNode>
    {
        /// <summary>
        /// The property which refers to the Parent node to which it belongs
        /// It can be of type <see cref="ScrapBaseProcessorNodeList"/>
        /// </summary>
        public ScrapBaseProcessorNodeList GetParent() { return (ScrapBaseProcessorNodeList)Parent; }

        /// <summary>
        /// The user input data processor
        /// </summary>
        public UserDataProcessorNode UserFeature { get; set; }

        /// <summary>
        /// The html feature data processor
        /// </summary>
        public HtmlPathProcessorNode HtmlFeature { get; set; }

        /// <summary>
        /// A default constructor as required by a <see cref="List{T}"/>
        /// It is different from a parameterless constructor
        /// </summary>
        public ScrapBaseProcessorNode() { }

        /// <summary>
        /// A constructor
        /// </summary>
        /// <param name="name">unique name</param>
        public ScrapBaseProcessorNode(string name = "")
        {
            Id = name;
        }

        /// <summary>
        /// A memebr method to clone data to 'this' node from another node
        /// </summary>
        /// <param name="node">The node from which to create the copy</param>
        /// <returns></returns>
        public override ScrapBaseProcessorNode Clone(ScrapBaseProcessorNode node)
        {
            // id should be unique and must not be copied
            Id = GetUniqueName("");
            UserFeature = UserDataProcessorNode.CloneNew(node.UserFeature);
            HtmlFeature = HtmlPathProcessorNode.CloneNew(node.HtmlFeature);

            return this;
        }

        /// <summary>
        /// Utility method to setup node data with Url value.
        /// </summary>
        /// <param name="url">The complete Url path</param>
        /// <param name="isoffline">Check to set data either offline or online not both. Default is online, hence <code>false</code> </param>
        /// <returns></returns>
        public ScrapBaseProcessorNode AddUrl(string url, bool isoffline = false)
        {
            CheckCreateHtmlPathUrlNode(true, false);
            if (isoffline) HtmlFeature.UrlNode.Offline = new Uri(url);
            else HtmlFeature.UrlNode.Online = new Uri(url);

            return this;
        }

        /// <summary>
        /// Utility method to setup node data with Url value. This takes in both the values
        /// </summary>
        /// <param name="onlineUrl"></param>
        /// <param name="offlineUrl"></param>
        /// <returns></returns>
        public ScrapBaseProcessorNode AddUrl(string onlineUrl, string offlineUrl)
        {
            CheckCreateHtmlPathUrlNode(true, false);
            HtmlFeature.UrlNode.Offline = new Uri(onlineUrl);
            HtmlFeature.UrlNode.Online = new Uri(offlineUrl);

            return this;
        }

        /// <summary>
        /// Set the XPath and attribute information
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="xpath">The XPath query to retrieve the resource / data</param>
        /// <param name="attributeName">Default is null</param>
        /// <returns></returns>
        public ScrapBaseProcessorNode AddXPath(EHtmlNodeType nodeType,
            string xpath, string attributeName = null)
        {
            CheckCreateHtmlPathNode();
            if (HtmlFeature.XPathNode == null) HtmlFeature.XPathNode = XPathProcessorNode.New();
            HtmlFeature.XPathNode.NodeType = nodeType;
            if (!string.IsNullOrEmpty(xpath)) HtmlFeature.XPathNode.XPath = xpath;
            if (!string.IsNullOrEmpty(attributeName)) HtmlFeature.XPathNode.Attribute = attributeName;

            return this;
        }

        /// <summary>
        /// A private method initialize the object as required for <see cref="HtmlPathProcessorNode"/>
        /// </summary>
        /// <param name="checkCreateUrl"></param>
        /// <param name="checkCreateXPath"></param>
        private void CheckCreateHtmlPathUrlNode(bool checkCreateUrl, bool checkCreateXPath)
        {
            CheckCreateHtmlPathNode();
            if (checkCreateUrl && HtmlFeature.UrlNode == null) HtmlFeature.UrlNode = UrlProcessorNode.New();
            if (checkCreateXPath && HtmlFeature.XPathNode == null) HtmlFeature.XPathNode = XPathProcessorNode.New();
        }

        /// <summary>
        /// A private utility method to check and create an instance of <see cref="HtmlPathProcessorNode"/>
        /// </summary>
        private void CheckCreateHtmlPathNode()
        {
            if (HtmlFeature == null) HtmlFeature = HtmlPathProcessorNode.New();
        }
    }
}

using HtmlAgilityPack;
using log4net;
using SimpleWebsiteScrapper.DataSet;
using SimpleWebsiteScrapper.ParseTree;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// A helper class used by the Weebscrapper engine to iterate over the node and its attributes
    /// </summary>
    public class ScrapHtmlNodeIterator
    {
        /// <summary>
        /// The log manager
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ScrapHtmlNodeIterator));

        /// <summary>
        /// The unique identifier of the node
        /// </summary>
        public string Id { get; protected set; }

        /// <summary>
        /// The Html resource locator that this iterator currently identifies with
        /// </summary>
        public HtmlLocator Locator { get; private set; }

        /// <summary>
        /// The Url of the webpage for the resource location.
        /// For the XPath node use <see cref="Locator"/>
        /// </summary>
        public string AbsoluteUri { get; set; }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="node"></param>
        public ScrapHtmlNodeIterator()
        {
            Locator = new HtmlLocator();
        }

        /// <summary>
        /// This method scraps the main webpage location data using the root node
        /// </summary>
        public void ScrapLocation(string id, HtmlPathProcessorNode htmlFeature, 
            ScrapHtmlNodeIterator parentNodeIterator)
        {
            this.Id = id;
            if(htmlFeature == null)
            {
                logger.ErrorFormat("Assigning null HtmlPathProcessorNode object is not allowed in iterator - '{0}-Node'", id);
            }

            AbsoluteUri = GetLocationLink(htmlFeature, parentNodeIterator);
            SetLocator(htmlFeature);
        }

        /// <summary>
        /// This method scraps the webpage data by itself if the html feature has Url and Xpath data
        /// Else, it uses the base node locator data to fetch data
        /// </summary>
        /// <param name="rootNodeIterator">The base node iterator</param>
        /// <param name="htmlFeature"></param>
        public void ScrapLocation(ScrapHtmlNodeIterator rootNodeIterator, HtmlPathProcessorNode htmlFeature)
        {
            Id = rootNodeIterator.Id;
            AbsoluteUri = GetLocationLink(htmlFeature, rootNodeIterator);
            SetLocator(htmlFeature);
        }

        /// <summary>
        /// Get the <see cref="HtmlReferenceData"/> data
        /// </summary>
        /// <returns></returns>
        public HtmlReferenceData GetHtmlLinkAttributeNode()
        {
            HtmlReferenceData linkAttrNode = new HtmlReferenceData();
            linkAttrNode.AbsoluteUri = AbsoluteUri;
            linkAttrNode.CollectionIndex = Locator.CollectionIndex;
            linkAttrNode.XPath = Locator.NodeCollection[Locator.CollectionIndex].XPath;
            linkAttrNode.Attribute = (Locator.NodeAttribute != null) ? Locator.NodeAttribute.Name : null;

            return linkAttrNode;
        }

        /// <summary>
        /// Set the node locator
        /// </summary>
        private void SetLocator(HtmlPathProcessorNode htmlFeature)
        {
            if (string.IsNullOrEmpty(htmlFeature.XPathNode.XPath)) return;

            HtmlNode documentNode = GlobalCacheStore.This.HtmlCache.Retrieve(AbsoluteUri);
            Locator.NodeCollection = documentNode.SelectNodes(htmlFeature.XPathNode.XPath);
            
            if (htmlFeature.XPathNode.NodeType == EHtmlNodeType.Single)
            {
                Locator.CollectionIndex = 0;
                if(!string.IsNullOrEmpty(htmlFeature.XPathNode.Attribute))
                {
                    Locator.NodeAttribute = Locator.NodeCollection[0].Attributes[htmlFeature.XPathNode.Attribute];
                }
            }
        }

        /// <summary>
        /// Get the text associated with the Html locator
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            if (Locator.NodeAttribute != null)
                return Locator.NodeAttribute.Value;
            else if (Locator.NodeCollection != null)
                return Locator.NodeCollection[Locator.CollectionIndex].InnerText;
            else
                return string.Empty;
        }

        /// <summary>
        /// Get the webpage link as per the global settings
        /// </summary>
        /// <returns></returns>
        private string GetLocationLink(HtmlPathProcessorNode htmlFeature, ScrapHtmlNodeIterator parentNodeIterator)
        {
            string absoluteUri = string.Empty;

            if (htmlFeature.UrlNode == null ||
                (htmlFeature.UrlNode.Offline == null && htmlFeature.UrlNode.Online == null))
            {
                logger.DebugFormat("Location Url not found for '{0}-node'", Id);
            }
            else
            {
                switch (GlobalSettings.DownloadMode)
                {
                    case GlobalSettings.EDownloadMode.OFFLINE_STRICT:
                        absoluteUri = htmlFeature.UrlNode.OfflineOrDefault(); break;

                    case GlobalSettings.EDownloadMode.OFFLINE_THEN_ONLINE:
                        absoluteUri = htmlFeature.UrlNode.OfflineOrOnlineOrDefault(); break;

                    case GlobalSettings.EDownloadMode.ONLINE:
                        absoluteUri = htmlFeature.UrlNode.OnlineOrDefault(); break;

                    default:
                        absoluteUri = string.Empty; break;
                }
            }

            if (string.IsNullOrEmpty(absoluteUri))
                absoluteUri = parentNodeIterator.AbsoluteUri;

            return absoluteUri;
        }
    }
}

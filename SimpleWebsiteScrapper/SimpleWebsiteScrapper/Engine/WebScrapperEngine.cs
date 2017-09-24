using System.Collections.Generic;
using log4net;
using SimpleWebsiteScrapper.ParseTree;
using SimpleWebsiteScrapper.DataSet;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// The base Engine class which does a semantic checking. It currently deos nothing except all other actual Website 
    /// Scrapper classes inherits from this class and defines the grammer for a page to scrap data
    /// </summary>
    public class WebScrapperEngine
    {
        /// <summary>
        /// The internal logger for the class
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(WebScrapperEngine));

        /// <summary>
        /// The root node of the processor node
        /// </summary>
        public ScrapWebpageProcessorNode RootNode { get; set; }

        /// <summary>
        /// The root data node
        /// </summary>
        public WebpageSectionData RootDataNode { get; set; }

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <param name="name"></param>
        public WebScrapperEngine(string name = "")
        {
            RootNode = new ScrapWebpageProcessorNode(name);
            RootDataNode = new WebpageSectionData();
        }

        public void Scrap()
        {
            Scrap(RootNode, RootDataNode, null);
        }

        /// <summary>
        /// The main method to start the parsing and scraping of data from webpage
        /// </summary>
        public ScrapHtmlNodeIterator Scrap(ScrapWebpageProcessorNode parentNode, WebpageSectionData dataNode,
            ScrapHtmlNodeIterator parentNodeIterator)
        {
            logger.Debug("Web Scrapper Started");

            if(parentNode == null)
            {
                logger.Info("Root Node for Scrapping is not yet initialized");
                return null;
            }

            ScrapHtmlNodeIterator nodeIterator = new ScrapHtmlNodeIterator();
            nodeIterator.ScrapLocation(parentNode.Id, parentNode.HtmlFeature, parentNodeIterator);

            // Loop through the References
            if (parentNode.References != null && parentNode.References.Count > 0)
            {
                for (int indx = 0; indx < parentNode.References.Count; ++indx)
                {
                    ScrapMetadata(parentNode.References, nodeIterator, dataNode.References, indx);
                }
            }

            // Loop through the Copyrights
            if (parentNode.Copyrights != null && parentNode.Copyrights.Count > 0)
            {
                for (int indx = 0; indx < parentNode.Copyrights.Count; ++indx)
                {
                    ScrapMetadata(parentNode.Copyrights, nodeIterator, dataNode.Copyrights, indx);
                }
            }

            if (parentNode.Nodes == null || parentNode.Nodes.Count == 0)
            {
                dataNode.ClassData = CreateDataNode(nodeIterator);
            }
            else
            {
                // Loop through the child nodes depending upon the node type
                if (parentNode.HtmlFeature.XPathNode.NodeType == EHtmlNodeType.Collection)
                {
                    for (int htmlIndx = 0; htmlIndx < nodeIterator.Locator.NodeCollection.Count; ++htmlIndx)
                    {
                        // Create a table with rows of data
                        List<WebpageSectionData> rowData = new List<WebpageSectionData>();
                        for (int indx = 0; indx < parentNode.Nodes.Count; ++indx)
                        {
                            WebpageSectionData childDataNode = new WebpageSectionData();
                            ScrapHtmlNodeIterator childNodeIterator = Scrap(parentNode.Nodes[indx], childDataNode, nodeIterator);
                            childDataNode.ClassData = CreateDataNode(childNodeIterator);

                            rowData.Add(childDataNode);
                        }
                    }
                }
            }

            logger.Debug("Web Scrapper Ended");

            return nodeIterator;
        }

        /// <summary>
        /// This method helps in getting the metadata for a data
        /// </summary>
        /// <param name="nodeList"></param>
        protected void ScrapMetadata(ScrapBaseProcessorNodeList processorNodeList, ScrapHtmlNodeIterator parentNodeIterator, 
            List<ProcessData> actualDataList, int indx)
        {
            ScrapHtmlNodeIterator nodeIterator = new ScrapHtmlNodeIterator();
            nodeIterator.ScrapLocation(parentNodeIterator, processorNodeList[indx].HtmlFeature);

            actualDataList.Add(CreateDataNode(nodeIterator));
        }


        /// <summary>
        /// Utility method to copy data from The Html Node iterator to Actual data node
        /// </summary>
        /// <param name="dataNode"></param>
        /// <param name="nodeIterator"></param>
        protected ProcessData CreateDataNode(ScrapHtmlNodeIterator nodeIterator)
        {
            ProcessData dataNode = new ProcessData();

            dataNode.Link = nodeIterator.GetHtmlLinkAttributeNode();
            dataNode.Text = nodeIterator.GetString();

            return dataNode;
        }
    }
}

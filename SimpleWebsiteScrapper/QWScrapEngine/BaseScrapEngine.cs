using HtmlAgilityPack;
using QWCommonDST.Cache;
using QWWebScrap.Model;
using QWWebScrap.OModel;
using QWCommonDST.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine
{
    /// <summary>
    /// This class is the base class for all Website scrapper business logic. The derived classes
    /// contains web scrapping tree creation logic. All website data to be scrapped should have a class 
    /// derived from this class. 
    /// </summary>
    public class BaseScrapEngine
    {
        /// <summary>
        /// The Grammer parser tree. The derived classes defines the grammer tree which defines the data
        /// to be scrapped.
        /// </summary>
        public List<WebSegmentTree> WebpageScrapperGrammer { get; set; }

        /// <summary>
        /// The parsed data from the website(s) using the grammer <see cref="WebpageScrapperGrammer"/>
        /// </summary>
        public List<ScrapWebData> ProcessedWebData { get; set; }

        /// <summary>
        /// A temporary stack of Html Node elements currently processed, processed at parent level
        /// which is only used during the processing of the Scrapper.
        /// This stacks the Element currently referred by <see cref="WebSegmentTree"/>
        /// </summary>
        private Stack<HtmlElementHeap> webSegmentElementStack;

        /// <summary>
        /// A temporary stack of Html Node elements currently processed, processed at parent level
        /// which is only used during the processing of the Scrapper.
        /// This stacks the Element currently referred by <see cref="SegmentMetadataTree"/>
        /// </summary>
        private Stack<HtmlElementHeap> metadataSegmentElementStack;
                
        /// <summary>
        /// Default constructor
        /// </summary>
        public BaseScrapEngine()
        {
            WebpageScrapperGrammer = new List<WebSegmentTree>();
        }

        /// <summary>
        /// Add root tree to the list
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected WebSegmentTree AddRoot(string name)
        {
            if (WebpageScrapperGrammer == null) WebpageScrapperGrammer = new List<WebSegmentTree>();
            WebSegmentTree rootTree = new WebSegmentTree();
            rootTree.Id = name;
            WebpageScrapperGrammer.Add(rootTree);

            return rootTree;
        }

        /// <summary>
        /// The main public method
        /// </summary>
        public void Parse()
        {
            try
            {
                // Initialize data types
                InitializeParserRequisites();

                // Loop through all the webpage parser trees
                WebpageScrapperGrammer.ForLoop((webSegment, indx) => {
                    ScrapWebData scrapWebData = new ScrapWebData();
                    OnWebSegmentTreeVisitorBFSTraversal(webSegment, scrapWebData);

                    ProcessedWebData.Add(scrapWebData);
                });
            }
            finally
            {
                ClearParserRequisites();
            }
        }

        /// <summary>
        /// Initialize all the Data structures required before parsing
        /// </summary>
        protected void InitializeParserRequisites()
        {
            // Register static event handlers
            webSegmentElementStack = new Stack<HtmlElementHeap>();
            ProcessedWebData = new List<ScrapWebData>();
        }

        /// <summary>
        /// Clear all the Data structures required before parsing
        /// </summary>
        protected void ClearParserRequisites()
        {
            // Must be cleared or there will be memory leak
            webSegmentElementStack.Clear();
            webSegmentElementStack = null;
        }

        /// <summary>
        /// Get or Load the Html node from the Document Store or from the stack
        /// </summary>
        /// <returns></returns>
        internal HtmlElementHeap GetOrLoadHtmlNode(HtmlPathHint htmlPath, Stack<HtmlElementHeap> stack)
        {
            HtmlElementHeap hapLink = null;

            // Check from Global cache if the document is already loaded
            if (htmlPath != null && htmlPath.Url != null)
            {
                HtmlDocCache htmldocCache = GlobalCacheStore.This.HtmlCache.Retrieve(htmlPath.Url.Online,
                    htmlPath.Url.Offline);
                hapLink = new HtmlElementHeap(htmldocCache.Document.DocumentNode);
                stack.Push(hapLink);
            }
            else
            {
                hapLink = stack.Peek();
            }

            return hapLink;
        }

        /// <summary>
        /// Calculate the current node type from the <see cref="HtmlPathHint"/> and the 
        /// parent <see cref="HtmlElementHeap"/>
        /// </summary>
        /// <param name="htmlPath"></param>
        /// <param name="hapLinkParent"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        internal HtmlElementHeap CalculateHtmlNode(HtmlPathHint htmlPath, HtmlElementHeap hapLinkParent)
        {
            HtmlElementHeap hapLinkChild = new HtmlElementHeap();

            if (htmlPath != null && htmlPath.Path != null)
            {
                if (hapLinkParent.Single != null)
                    hapLinkChild.Collection = hapLinkParent.Single.SelectNodes(htmlPath.Path.XPath);
                else if (hapLinkParent.Collection != null)
                    hapLinkChild.Collection = hapLinkParent.Collection[0].SelectNodes(htmlPath.Path.XPath);
                else
                    throw new Exception("Error in temporary statck Hap url");

                if (hapLinkChild.Collection != null)
                {
                    if (htmlPath.Path.CollectionIndex >= 0)
                    {
                        hapLinkChild.Single = hapLinkChild.Collection[htmlPath.Path.CollectionIndex];
                        hapLinkChild.CollectionIndex = htmlPath.Path.CollectionIndex;
                        hapLinkChild.Collection = null;

                        if (!string.IsNullOrEmpty(htmlPath.Path.Attribute))
                            hapLinkChild.Attribute = hapLinkChild.Single.Attributes[htmlPath.Path.Attribute];
                    }
                }
                else
                    throw new Exception("Node not found");
            }

            return hapLinkChild;
        }

        /// <summary>
        /// Process and parse Metadata tree
        /// </summary>
        /// <param name="metadataTree"></param>
        /// <param name="hapLinkChild"></param>
        internal List<ScrapMetadata> ProcessMetadata(List<SegmentMetadataTree> metadataTree, HtmlElementHeap hapLinkChild)
        {
            List<ScrapMetadata> metadataParsedList = new List<ScrapMetadata>();

            metadataTree.ForLoop((metadataChildTree, indx) =>
            {
                ScrapMetadata metadataChild = new ScrapMetadata();

                metadataSegmentElementStack = new Stack<HtmlElementHeap>();
                metadataSegmentElementStack.Push(hapLinkChild);

                OnSegmentMetadataTreeVisitorBFSTraversal(metadataChildTree, metadataChild);

                metadataParsedList.Add(metadataChild);

                metadataSegmentElementStack.Clear();
            });

            return metadataParsedList;
        }

        /// <summary>
        /// Calculate the row of data for a html single node in the grammer tree.
        /// It is a recursive function.
        /// </summary>
        /// <param name="webSegmentNodes"></param>
        /// <param name="htmlNodeCurrent"></param>
        /// <param name="stack"></param>
        /// <returns></returns>
        internal List<OT> CalculateRowData<T, OT>(List<T> webSegmentNodes, 
            HtmlNode htmlNodeCurrent, Stack<HtmlElementHeap> stack,
            Action<T, OT> actionMethod) where OT: new()
        {
            List<OT> singleRowChildNodes = new List<OT>();

            webSegmentNodes.ForLoop((childSegmentTree, indx) =>
            {
                OT scrapWebDataChild = new OT();
                stack.Push(new HtmlElementHeap(htmlNodeCurrent));

                actionMethod(childSegmentTree, scrapWebDataChild);

                stack.Pop();

                singleRowChildNodes.Add(scrapWebDataChild);
            });

            return singleRowChildNodes;
        }

        /// <summary>
        /// Calculate the data
        /// </summary>
        /// <param name="webSegment"></param>
        /// <param name="hapLinkChild"></param>
        /// <returns></returns>
        internal string CalculateScrappedData(SegmentMetadata segmentMetadata, HtmlElementHeap hapLinkChild)
        {
            string scrapWebDataText = "";
            if (hapLinkChild.Attribute != null)
                scrapWebDataText = hapLinkChild.Attribute.Value;
            else if (hapLinkChild.Single != null)
                scrapWebDataText = hapLinkChild.Single.InnerText;
            else if (hapLinkChild.Collection != null && hapLinkChild.CollectionIndex >= 0)
                scrapWebDataText = hapLinkChild.Collection[hapLinkChild.CollectionIndex].InnerText;
            else
                throw new Exception("There is an error getting the text from the link");

            if (segmentMetadata.Custom != null)
            {
                if (segmentMetadata.Custom.IsPath)
                {
                    if (segmentMetadata.Custom.DoAppendToHtmlHint)
                    {
                        Uri result;
                        Uri.TryCreate(new Uri(scrapWebDataText), segmentMetadata.Custom.Text, out result);
                        scrapWebDataText = result.AbsoluteUri;
                    }
                    else
                    {
                        Uri result;
                        Uri.TryCreate(new Uri(segmentMetadata.Custom.Text), scrapWebDataText, out result);
                        scrapWebDataText = result.AbsoluteUri;
                    }
                }
                else
                {
                    if (segmentMetadata.Custom.DoAppendToHtmlHint)
                        scrapWebDataText += segmentMetadata.Custom.Text;
                    else
                        scrapWebDataText = segmentMetadata.Custom.Text + scrapWebDataText;
                }
            }

            return scrapWebDataText;
        }

        /// <summary>
        /// The parser logic for parsing and scrapping a webpage tree grammer and store data in the scrapped 
        /// model
        /// </summary>
        /// <param name="webSegment"></param>
        /// <param name="scrapWebData"></param>
        protected void OnWebSegmentTreeVisitorBFSTraversal(WebSegmentTree webSegment, ScrapWebData scrapWebData)
        {
            scrapWebData.id = webSegment.Id;

            // Get and load the url document
            HtmlElementHeap hapLinkParent = GetOrLoadHtmlNode(webSegment.ActualData.HtmlPath, webSegmentElementStack);

            // A single node is required for further processing, if the parent node is collection provide the index as well
            HtmlElementHeap hapLinkChild = CalculateHtmlNode(webSegment.ActualData.HtmlPath, hapLinkParent);
            webSegmentElementStack.Push(hapLinkChild);

            // Process and parse References for this segment node
            scrapWebData.References = ProcessMetadata(webSegment.References, hapLinkChild);

            // Process and parse Copyrights for this segment node
            scrapWebData.Copyrights = ProcessMetadata(webSegment.Copyrights, hapLinkChild);

            // Child
            if(webSegment.Nodes != null)
            {
                scrapWebData.Nodes = new List<List<ScrapWebData>>();
                if (webSegment.ActualData.HtmlPath.Path.CollectionIndex >= 0)
                {
                    scrapWebData.Nodes.Add(CalculateRowData<WebSegmentTree, ScrapWebData>(webSegment.Nodes,
                            hapLinkChild.Single, webSegmentElementStack,
                            OnWebSegmentTreeVisitorBFSTraversal));
                }
                else
                {
                    // This is a collection node and we have looping here
                    hapLinkChild.Collection.ForLoop((hapLinkNodeChild, indxRow) => {
                        scrapWebData.Nodes.Add(CalculateRowData<WebSegmentTree, ScrapWebData>(webSegment.Nodes,
                            hapLinkNodeChild, webSegmentElementStack,
                            OnWebSegmentTreeVisitorBFSTraversal));
                    });
                }
            }
            else
            {
                scrapWebData.Text = new ScrapMetadata
                {
                    Id = webSegment.Id,
                    Text = CalculateScrappedData(webSegment.ActualData, hapLinkChild)
                };
            }

            webSegmentElementStack.Pop();
        }
        
        /// <summary>
        /// The metadata logic parser to parse data and store in output
        /// </summary>
        /// <param name="metadataTree"></param>
        /// <param name="scrapMetadata"></param>
        protected void OnSegmentMetadataTreeVisitorBFSTraversal(SegmentMetadataTree metadataTree, ScrapMetadata scrapMetadata)
        {
            scrapMetadata.Id = metadataTree.Id;

            // Get and load the url document
            HtmlElementHeap hapLinkParent = GetOrLoadHtmlNode(metadataTree.ActualData.HtmlPath, metadataSegmentElementStack);

            HtmlElementHeap hapLinkChild = CalculateHtmlNode(metadataTree.ActualData.HtmlPath, hapLinkParent);
            metadataSegmentElementStack.Push(hapLinkChild);
            
            if (metadataTree.Nodes != null)
            {
                scrapMetadata.Nodes = new List<List<ScrapMetadata>>();

                // This is where the difference with collection and single node happens
                if (hapLinkChild.Collection != null)
                {
                    scrapMetadata.Nodes.Add(CalculateRowData<SegmentMetadataTree, ScrapMetadata>(metadataTree.Nodes,
                            hapLinkChild.Single, webSegmentElementStack, OnSegmentMetadataTreeVisitorBFSTraversal));
                }
                else
                {
                    hapLinkChild.Collection.ForLoop((hapLinkNodeChild, indxRow) =>
                    {
                        scrapMetadata.Nodes.Add(CalculateRowData<SegmentMetadataTree, ScrapMetadata>(metadataTree.Nodes,
                            hapLinkChild.Single, webSegmentElementStack, OnSegmentMetadataTreeVisitorBFSTraversal));
                    });
                }
            }
            else
            {
                scrapMetadata.Text = CalculateScrappedData(metadataTree.ActualData, hapLinkChild);
            }
        }
    }
}

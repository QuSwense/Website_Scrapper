using HtmlAgilityPack;
using QWCommonDST.Cache;
using QWWebScrap.Model;
using QWWebScrap.OModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWScrapEngine
{
    public class BaseScrapEngine
    {
        internal class HAPTemp
        {
            public HtmlNodeCollection Collection;
            public HtmlNode Single;
            public HtmlAttribute Attribute;
            public int CollectionIndex = -1;
            public Uri Url;
        }

        public List<WebSegmentTree> WebpageScrapperGrammer { get; set; }

        private List<HAPTemp> stackHtmlNodes { get; set; }
        private List<HAPTemp> stackHtmlMetadataNodes { get; set; }

        public List<ScrapWebData> ProcessedWebData { get; set; }

        public BaseScrapEngine()
        {
            WebpageScrapperGrammer = new List<WebSegmentTree>();
            
        }

        protected WebSegmentTree AddRoot(string name)
        {
            if (WebpageScrapperGrammer == null) WebpageScrapperGrammer = new List<WebSegmentTree>();
            WebSegmentTree rootTree = new WebSegmentTree();
            rootTree.Id = name;
            WebpageScrapperGrammer.Add(rootTree);

            return rootTree;
        }

        public void Parse()
        {
            try
            {
                // register static event handlers
                stackHtmlNodes = new List<HAPTemp>();
                ProcessedWebData = new List<ScrapWebData>();

                if (WebpageScrapperGrammer != null)
                {
                    for (int indx = 0; indx < WebpageScrapperGrammer.Count; indx++)
                    {
                        WebSegmentTree webSegment = WebpageScrapperGrammer[indx];

                        ScrapWebData scrapWebData = new ScrapWebData();
                        OnWebSegmentTreeVisitorBFSTraversal(webSegment, scrapWebData);

                        ProcessedWebData.Add(scrapWebData);
                    }
                }
            }
            finally
            {
                // Must be cleared or there will be memory leak
                stackHtmlNodes.Clear();
                stackHtmlNodes = null;
            }
        }

        protected void OnWebSegmentTreeVisitorBFSTraversal(WebSegmentTree webSegment, ScrapWebData scrapWebData)
        {
            scrapWebData.id = webSegment.Id;

            // Get and load the url document
            HAPTemp hapLink = null;

            // Check from Global cache if the document is already loaded
            if (webSegment.ActualData.HtmlPath != null && webSegment.ActualData.HtmlPath.Url != null)
            {
                HtmlDocCache htmldocCache = GlobalCacheStore.This.HtmlCache.Retrieve(webSegment.ActualData.HtmlPath.Url.Online,
                    webSegment.ActualData.HtmlPath.Url.Offline);
                hapLink = new HAPTemp();
                hapLink.Url = htmldocCache.UrlUsed;
                hapLink.Single = htmldocCache.Document.DocumentNode;
                stackHtmlNodes.Add(hapLink);
            }
            else
            {
                hapLink = stackHtmlNodes.Last();
            }

            // A single node is required for further processing, if the parent node is collection provide the index as well
            HAPTemp hapLinkChild = new HAPTemp();

            // From the path get the Node collection object
            if (webSegment.ActualData.HtmlPath != null && webSegment.ActualData.HtmlPath.Path != null)
            {
                if (hapLink.Single != null)
                    hapLinkChild.Collection = hapLink.Single.SelectNodes(webSegment.ActualData.HtmlPath.Path.XPath);
                else if (hapLink.Collection != null)
                    hapLinkChild.Collection = hapLink.Collection[0].SelectNodes(webSegment.ActualData.HtmlPath.Path.XPath);
                else
                    throw new Exception("Error in temporary statck Hap url");
                hapLinkChild.Url = hapLink.Url;

                if (hapLinkChild.Collection != null)
                {
                    if (webSegment.ActualData.HtmlPath.Path.CollectionIndex >= 0)
                    {
                        hapLinkChild.Single = hapLinkChild.Collection[webSegment.ActualData.HtmlPath.Path.CollectionIndex];
                        hapLinkChild.CollectionIndex = webSegment.ActualData.HtmlPath.Path.CollectionIndex;
                        hapLinkChild.Collection = null;

                        if (!string.IsNullOrEmpty(webSegment.ActualData.HtmlPath.Path.Attribute))
                            hapLinkChild.Attribute = hapLinkChild.Single.Attributes[webSegment.ActualData.HtmlPath.Path.Attribute];
                    }
                }
                else
                    throw new Exception("Node not found");
            }

            stackHtmlNodes.Add(hapLinkChild);

            // References
            if (webSegment.References != null)
            {
                scrapWebData.References = new List<ScrapMetadata>();

                for (int indx = 0; indx < webSegment.References.Count; ++indx)
                {
                    SegmentMetadataTree metadataTree = webSegment.References[indx];
                    ScrapMetadata metadataChild = new ScrapMetadata();

                    stackHtmlMetadataNodes = new List<HAPTemp>();
                    stackHtmlMetadataNodes.Add(hapLinkChild);

                    OnSegmentMetadataTreeVisitorBFSTraversal(metadataTree, metadataChild);

                    scrapWebData.References.Add(metadataChild);

                    stackHtmlMetadataNodes.Clear();
                }
            }

            // Copyright
            if (webSegment.Copyrights != null)
            {
                scrapWebData.Copyrights = new List<ScrapMetadata>();

                for (int indx = 0; indx < webSegment.Copyrights.Count; ++indx)
                {
                    SegmentMetadataTree metadataTree = webSegment.Copyrights[indx];
                    ScrapMetadata metadataChild = new ScrapMetadata();

                    stackHtmlMetadataNodes = new List<HAPTemp>();
                    stackHtmlMetadataNodes.Add(hapLinkChild);

                    OnSegmentMetadataTreeVisitorBFSTraversal(metadataTree, metadataChild);

                    scrapWebData.Copyrights.Add(metadataChild);

                    stackHtmlMetadataNodes.Clear();
                }
            }

            // Child
            if(webSegment.Nodes != null)
            {
                scrapWebData.Nodes = new List<List<ScrapWebData>>();
                if (webSegment.ActualData.HtmlPath.Path.CollectionIndex >= 0)
                {
                    List<ScrapWebData> singleRowChildNodes = new List<ScrapWebData>();
                    // This is a single node as collection index is whole number
                    for (int indx = 0; indx < webSegment.Nodes.Count; ++indx)
                    {
                        WebSegmentTree childSegmentTree = webSegment.Nodes[indx];

                        ScrapWebData scrapWebDataChild = new ScrapWebData();
                        OnWebSegmentTreeVisitorBFSTraversal(childSegmentTree, scrapWebDataChild);
                        singleRowChildNodes.Add(scrapWebDataChild);
                    }

                    scrapWebData.Nodes.Add(singleRowChildNodes);
                }
                else
                {
                    // This is a collection node and we have looping here
                    for(int indxRow = 0; indxRow < hapLinkChild.Collection.Count; ++indxRow)
                    {
                        List<ScrapWebData> singleRowChildNodes = new List<ScrapWebData>();

                        for (int indx = 0; indx < webSegment.Nodes.Count; ++indx)
                        {
                            WebSegmentTree childSegmentTree = webSegment.Nodes[indx];

                            ScrapWebData scrapWebDataChild = new ScrapWebData();
                            HAPTemp hapTemp = new HAPTemp();
                            hapTemp.Single = hapLinkChild.Collection[indxRow];

                            stackHtmlNodes.Add(hapTemp);

                            OnWebSegmentTreeVisitorBFSTraversal(childSegmentTree, scrapWebDataChild);

                            stackHtmlNodes.Remove(hapTemp);

                            singleRowChildNodes.Add(scrapWebDataChild);
                        }

                        scrapWebData.Nodes.Add(singleRowChildNodes);
                    }
                }
            }
            else
            {
                scrapWebData.Text = new ScrapMetadata();
                scrapWebData.Text.Id = webSegment.Id;
                if (hapLinkChild.Attribute != null)
                    scrapWebData.Text.Text = hapLinkChild.Attribute.Value;
                else if (hapLinkChild.Single != null)
                    scrapWebData.Text.Text = hapLinkChild.Single.InnerText;
                else if (hapLinkChild.Collection != null && hapLinkChild.CollectionIndex >= 0)
                    scrapWebData.Text.Text = hapLinkChild.Collection[hapLinkChild.CollectionIndex].InnerText;
                else
                    throw new Exception("There is an error getting the text from the link");

                if(webSegment.ActualData.Custom != null)
                {
                    if(webSegment.ActualData.Custom.IsPath)
                    {
                        if (webSegment.ActualData.Custom.DoAppendToHtmlHint)
                        {
                            Uri result;
                            Uri.TryCreate(new Uri(scrapWebData.Text.Text), webSegment.ActualData.Custom.Text, out result);
                            scrapWebData.Text.Text = result.AbsoluteUri;
                        }
                        else
                        {
                            Uri result;
                            Uri.TryCreate(new Uri(webSegment.ActualData.Custom.Text), scrapWebData.Text.Text, out result);
                            scrapWebData.Text.Text = result.AbsoluteUri;
                        }
                    }
                    else
                    {
                        if (webSegment.ActualData.Custom.DoAppendToHtmlHint)
                            scrapWebData.Text.Text += webSegment.ActualData.Custom.Text;
                        else
                            scrapWebData.Text.Text = webSegment.ActualData.Custom.Text + scrapWebData.Text.Text;
                    }
                }
            }

            stackHtmlNodes.Remove(hapLinkChild);
        }
        
        protected void OnSegmentMetadataTreeVisitorBFSTraversal(SegmentMetadataTree metadataTree, ScrapMetadata scrapMetadata)
        {
            scrapMetadata.Id = metadataTree.Id;

            // Get and load the url document
            HAPTemp hapLink = null;

            // Check from Global cache if the document is already loaded
            if (metadataTree.ActualData.HtmlPath != null && metadataTree.ActualData.HtmlPath.Url != null)
            {
                HtmlDocCache htmldocCache = GlobalCacheStore.This.HtmlCache.Retrieve(metadataTree.ActualData.HtmlPath.Url.Online,
                    metadataTree.ActualData.HtmlPath.Url.Offline);
                hapLink = new HAPTemp();
                hapLink.Single = htmldocCache.Document.DocumentNode;
                hapLink.Url = htmldocCache.UrlUsed;
                stackHtmlMetadataNodes.Add(hapLink);
            }
            else
            {
                hapLink = stackHtmlMetadataNodes.Last();
            }

            HAPTemp metdatalinkChild = new HAPTemp();

            // From the path get the Node collection object
            if (metadataTree.ActualData.HtmlPath != null && metadataTree.ActualData.HtmlPath.Path != null)
            {
                if(hapLink.Single != null)
                    metdatalinkChild.Collection = hapLink.Single.SelectNodes(metadataTree.ActualData.HtmlPath.Path.XPath);
                else if(hapLink.Collection != null)
                    metdatalinkChild.Collection = hapLink.Collection[0].SelectNodes(metadataTree.ActualData.HtmlPath.Path.XPath);
                else
                    throw new Exception("Error in temporary statck Hap url");
                metdatalinkChild.Url = hapLink.Url;

                if (metdatalinkChild.Collection != null)
                {
                    if (metadataTree.ActualData.HtmlPath.Path.CollectionIndex >= 0)
                    {
                        metdatalinkChild.Single = metdatalinkChild.Collection[metadataTree.ActualData.HtmlPath.Path.CollectionIndex];
                        metdatalinkChild.CollectionIndex = metadataTree.ActualData.HtmlPath.Path.CollectionIndex;
                        metdatalinkChild.Collection = null;

                        if (!string.IsNullOrEmpty(metadataTree.ActualData.HtmlPath.Path.Attribute))
                            metdatalinkChild.Attribute = metdatalinkChild.Single.Attributes[metadataTree.ActualData.HtmlPath.Path.Attribute];
                    }
                }
            }

            stackHtmlMetadataNodes.Add(metdatalinkChild);

            if (metadataTree.Nodes != null)
            {
                scrapMetadata.Nodes = new List<List<ScrapMetadata>>();

                // This is where the difference with collection and single node happens
                if (metdatalinkChild.Collection != null)
                {
                    List<ScrapMetadata> scrapMetadataChildList = new List<ScrapMetadata>();
                    for (int indxRow = 0; indxRow < metadataTree.Nodes.Count; ++indxRow)
                    {
                        for (int indx = 0; indx < metadataTree.Nodes.Count; ++indx)
                        {
                            ScrapMetadata metadataChild = new ScrapMetadata();
                            OnSegmentMetadataTreeVisitorBFSTraversal(metadataTree.Nodes[indx], metadataChild);
                            scrapMetadataChildList.Add(metadataChild);
                        }
                    }
                    scrapMetadata.Nodes.Add(scrapMetadataChildList);
                }
                else
                {
                    for (int indxRow = 0; indxRow < metdatalinkChild.Collection.Count; ++indxRow)
                    {
                        List<ScrapMetadata> scrapMetadataChildList = new List<ScrapMetadata>();

                        for (int indx = 0; indx < metadataTree.Nodes.Count; ++indx)
                        {
                            ScrapMetadata metadataChild = new ScrapMetadata();

                            HAPTemp hapTemp = new HAPTemp();
                            hapTemp.Single = metdatalinkChild.Collection[indxRow];

                            stackHtmlMetadataNodes.Add(hapTemp);

                            OnSegmentMetadataTreeVisitorBFSTraversal(metadataTree.Nodes[indx], metadataChild);

                            stackHtmlMetadataNodes.Remove(hapTemp);
                            scrapMetadataChildList.Add(metadataChild);
                        }
                        scrapMetadata.Nodes.Add(scrapMetadataChildList);
                    }
                }
            }
            else
            {
                if (metdatalinkChild.Attribute != null)
                    scrapMetadata.Text = metdatalinkChild.Attribute.Value;
                else if (metdatalinkChild.Single != null)
                    scrapMetadata.Text = metdatalinkChild.Single.InnerText;
                else if (metdatalinkChild.Collection != null && metdatalinkChild.CollectionIndex >= 0)
                    scrapMetadata.Text = metdatalinkChild.Collection[metdatalinkChild.CollectionIndex].InnerText;
                else
                    throw new Exception("Error on setting path for Grammer");

                if (metadataTree.ActualData.Custom != null)
                {
                    if (metadataTree.ActualData.Custom.IsPath)
                    {
                        if (metadataTree.ActualData.Custom.DoAppendToHtmlHint)
                        {
                            Uri result;
                            Uri.TryCreate(new Uri(scrapMetadata.Text), metadataTree.ActualData.Custom.Text, out result);
                            scrapMetadata.Text = result.AbsoluteUri;
                        }
                        else
                        {
                            Uri result;
                            Uri.TryCreate(new Uri(metadataTree.ActualData.Custom.Text), scrapMetadata.Text, out result);
                            scrapMetadata.Text = result.AbsoluteUri;
                        }
                    }
                    else
                    {
                        if (metadataTree.ActualData.Custom.DoAppendToHtmlHint)
                            scrapMetadata.Text += metadataTree.ActualData.Custom.Text;
                        else
                            scrapMetadata.Text = metadataTree.ActualData.Custom.Text + scrapMetadata.Text;
                    }
                }
            }
        }
    }
}

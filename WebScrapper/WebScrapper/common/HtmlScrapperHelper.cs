using HtmlAgilityPack;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapper.common
{
    public static class HtmlScrapperHelper
    {
        private static ILog logger = LogManager.GetLogger(typeof(HtmlScrapperHelper));

        internal static HtmlNode Load(string url)
        {
            WebRequest webRequestObj = WebRequest.Create(url);

            if (url.Contains("http:/"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + url);
                return null;
            }
            else if (url.Contains("http:/"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    logger.ErrorFormat("Http web response for url {0} status code {1}", url, httpResponse.StatusCode);
                    return null;
                }
            }

            string htmlText = "";
            using (StreamReader reader = new StreamReader(webResponseObj.GetResponseStream()))
            {
                htmlText = reader.ReadToEnd();
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlText);

            return document.DocumentNode;
        }

        internal static StreamReader LoadFile(string url)
        {
            WebRequest webRequestObj = WebRequest.Create(url);

            if (url.Contains("http:/"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + url);
                return null;
            }
            else if (url.Contains("http:/"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    logger.ErrorFormat("Http web response for url {0} status code {1}", url, httpResponse.StatusCode);
                    return null;
                }
            }

            return new StreamReader(webResponseObj.GetResponseStream());
        }

        //internal static HtmlNode FetchSingle(HtmlNode htmlnode, string xPath)
        //{
        //    return htmlnode.SelectSingleNode(xPath);
        //}

        //internal static ScrapString FetchSingleParagraph(HtmlNode htmlnode, string xPath,
        //    string desc = null)
        //{
        //    ScrapString objScrapString = new ScrapString()
        //    {
        //        Description = desc,
        //        XPath = xPath,
        //        Value = htmlnode.SelectSingleNode(xPath).InnerText
        //    };
        //    return objScrapString;
        //}

        //internal static ScrapString FetchSingleSentence(HtmlNode htmlnode, string xPath, string endString,
        //    string desc = null)
        //{
        //    string text = htmlnode.SelectSingleNode(xPath).InnerText;
        //    int findIndex = text.IndexOf(endString);

        //    text = text.Substring(0, findIndex) + endString;
        //    ScrapString objScrapString = new ScrapString()
        //    {
        //        Description = desc,
        //        XPath = xPath,
        //        Value = text
        //    };
        //    return objScrapString;
        //}

        //internal static ScrapString FetchSingleText(HtmlNode htmlnode, string xPath,
        //    string attribute = null)
        //{
        //    string data = "";
        //    HtmlNode htmlNode = htmlnode.SelectSingleNode(xPath);

        //    if (htmlNode != null)
        //    {
        //        if (!string.IsNullOrEmpty(attribute))
        //        {
        //            HtmlAttribute attributeNode = htmlNode.Attributes[attribute];
        //            if (attributeNode != null)
        //                data = attributeNode.Value;
        //        }
        //        else
        //        {
        //            data = htmlNode.InnerText;
        //        }
        //    }

        //    ScrapString objScrapString = new ScrapString()
        //    {
        //        XPath = xPath,
        //        Attribute = attribute,
        //        Value = data
        //    };
        //    return objScrapString;
        //}

        //internal static string FetchSingleValue(HtmlNode htmlnode, string xPath,
        //    string attribute = null)
        //{
        //    string data = "";
        //    HtmlNode htmlNode = htmlnode.SelectSingleNode(xPath);

        //    if (!string.IsNullOrEmpty(attribute))
        //    {
        //        data = htmlNode.Attributes[attribute].Value;
        //    }
        //    else
        //    {
        //        data = htmlNode.InnerText;
        //    }

        //    return data;
        //}

        //internal static ScrapTable FetchTable(HtmlNode node, string[][] xPathsHeader, string dataCollectionXPath,
        //    string[][] xPathsValue)
        //{
        //    ScrapTable scrapTable = new ScrapTable();
        //    scrapTable.Headers = new List<ScrapString>();

        //    // Headers
        //    for (int indx = 0; indx < xPathsHeader.Length; ++indx)
        //    {
        //        string xPath = xPathsHeader[indx][0];
        //        string attribute = (xPathsHeader[indx].Length > 1) ? xPathsHeader[indx][1] : null;
        //        string append = (xPathsHeader[indx].Length > 2) ? xPathsHeader[indx][2] : null;
        //        string prepend = (xPathsHeader[indx].Length > 3) ? xPathsHeader[indx][3] : null;

        //        scrapTable.Headers[indx] = FetchSingleText(node, xPath, attribute);
        //        scrapTable.Headers[indx].Value = prepend + scrapTable.Headers[indx].Value + append;
        //    }

        //    HtmlNodeCollection htmlNodeCollection = node.SelectNodes(dataCollectionXPath);

        //    // First Row
        //    for (int indx = 0; indx < xPathsValue.Length; ++indx)
        //    {
        //        string xPath = xPathsValue[indx][0];
        //        string attribute = (xPathsValue[indx].Length > 1) ? xPathsValue[indx][1] : null;
        //        string append = (xPathsValue[indx].Length > 2) ? xPathsValue[indx][2] : null;
        //        string prepend = (xPathsValue[indx].Length > 3) ? xPathsValue[indx][3] : null;

        //        scrapTable.FirstRow[indx] = FetchSingleText(htmlNodeCollection[0], xPath, attribute);
        //        scrapTable.FirstRow[indx].Value = prepend + scrapTable.FirstRow[indx].Value + append;
        //    }

        //    // Subsequent Rows
        //    scrapTable.Value = new List<List<string>>();
        //    for (int row = 1; row < htmlNodeCollection.Count; ++row)
        //    {
        //        scrapTable.Value[row] = new List<string>();
        //        for (int indx = 0; indx < xPathsValue.Length; ++indx)
        //        {
        //            string xPath = xPathsValue[indx][0];
        //            string attribute = (xPathsValue[indx].Length > 1) ? xPathsValue[indx][1] : null;
        //            string append = (xPathsValue[indx].Length > 2) ? xPathsValue[indx][2] : null;
        //            string prepend = (xPathsValue[indx].Length > 3) ? xPathsValue[indx][3] : null;

        //            scrapTable.Value[row][indx] = FetchSingleValue(htmlNodeCollection[row], xPath, attribute);
        //            scrapTable.Value[row][indx] = prepend + scrapTable.Value[row][indx] + append;
        //        }
        //    }

        //    return scrapTable;
        //}
    }
}

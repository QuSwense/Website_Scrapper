﻿using HtmlAgilityPack;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.XPath;
using WebCommon.Error;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The class which helps in Website queries / commands like loading, querying xpath
    /// </summary>
    public class HtmlScrapperCommand
    {
        #region Properties

        /// <summary>
        /// Logger
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(HtmlScrapperCommand));

        #endregion Properties

        #region Check

        /// <summary>
        /// Check if the url is online web page
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool IsOnlineWebPage(string url)
            => url.Contains("http:/") || url.Contains("https:/");

        #endregion Check

        #region Load

        /// <summary>
        /// Set the <see cref="WebRequest"/> settings for Http pages
        /// </summary>
        /// <param name="httpWebRequestObj"></param>
        private void HttpWebRequestSettings(WebRequest webRequestObj)
        {
            HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
            httpWebRequestObj.Method = "GET";
            httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequestObj.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
        }

        /// <summary>
        /// Load a html page from online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlNode Load(string url)
        {
            WebRequest webRequestObj = WebRequest.Create(url);

            if (IsOnlineWebPage(url)) HttpWebRequestSettings(webRequestObj);

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + url);
                return null;
            }
            else if (IsOnlineWebPage(url))
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
                htmlText = reader.ReadToEnd();

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlText);

            return document.DocumentNode;
        }
        
        /// <summary>
        /// Load a file from the online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public StreamReader LoadFile(string url)
        {
            WebRequest webRequestObj = WebRequest.Create(url);

            if (IsOnlineWebPage(url)) HttpWebRequestSettings(webRequestObj);

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + url);
                return null;
            }
            else if (IsOnlineWebPage(url))
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

        /// <summary>
        /// Load Online webpage (using HAP utility method)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlNode LoadOnline(string url)
        {
            var htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            var doc = htmlWeb.Load(url);

            return doc.DocumentNode;
        }

        #endregion Load

        #region Read

        /// <summary>
        /// Read the html page and extract the node represented by the XPath
        /// </summary>
        /// <param name="htmlNode"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        public List<HtmlNodeNavigator> ReadNodes(HtmlNode htmlNode, string xPath)
        {
            var navigator = (HtmlNodeNavigator)htmlNode.CreateNavigator();
            var xpathNodeIterator = navigator.Select(xPath);

            AssertEmpty(xpathNodeIterator, xPath);

            return navigator.Select(xPath).Cast<HtmlNodeNavigator>().ToList();
        }

        #endregion Read

        #region Assert

        /// <summary>
        /// Assert / check the <see cref="XPathNodeIterator"/> object is empty or null
        /// </summary>
        /// <param name="xpathNodeIterator"></param>
        public static void AssertEmpty(XPathNodeIterator xpathNodeIterator, string xPath)
        {
            if (xpathNodeIterator == null || xpathNodeIterator.Count <= 0)
                throw new HtmlNodeException(xPath, HtmlNodeException.EErrorType.XPATH_NODE_NULL);
        }

        #endregion Assert
    }
}

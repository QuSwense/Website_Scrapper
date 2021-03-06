﻿using HtmlAgilityPack;
using log4net;
using ScrapEngine.Interfaces;
using ScrapException;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.XPath;

namespace ScrapEngine.Bl
{
    /// <summary>
    /// The class which helps in Website queries / commands like loading, querying xpath
    /// When using .NET 4.0 version of <see cref="System.Net"/> library, there was an issue of 
    /// downloading text file from an Url directly. Hence the Project was changed to use .NET 4.5.
    /// </summary>
    public class HtmlScrapperCommand : IScrapperCommand
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
            httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            httpWebRequestObj.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
            httpWebRequestObj.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.None;
            httpWebRequestObj.KeepAlive = true;
        }

        /// <summary>
        /// Common Load method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="fManipulateWebStream"></param>
        /// <returns></returns>
        private T Load<T>(string url, Func<WebResponse, T> fManipulateWebStream)
            where T: class
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, cert, chain, error) =>
                {
                    return true;
                };
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

            return fManipulateWebStream(webResponseObj);
        }

        /// <summary>
        /// Load a html page from online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private HtmlNode ManipulateWebStreamForPage(WebResponse webResponseObj)
        {
            Stream stream = webResponseObj.GetResponseStream();
            HttpWebResponse htmlWebResponse = (HttpWebResponse)webResponseObj;
            Encoding encoding = Encoding.GetEncoding(htmlWebResponse.CharacterSet);
            string htmlText = "";
            using (StreamReader reader = new StreamReader(stream, encoding))
                htmlText = reader.ReadToEnd();

            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(htmlText);

            return document.DocumentNode;
        }
        
        /// <summary>
        /// Load a file from the online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ManipulateWebStreamForFile(WebResponse webResponseObj)
        {
            Stream stream = webResponseObj.GetResponseStream();
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }

        /// <summary>
        /// Load a html page from online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public HtmlNode Load(string url)
            => Load(url, ManipulateWebStreamForPage);

        /// <summary>
        /// Load a file from the online or offline
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string LoadFile(string url)
            => Load(url, ManipulateWebStreamForFile);

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

using HtmlAgilityPack;
using log4net;
using System;
using System.IO;
using System.Net;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// The cache to store All HtmlLocators used in the application. This is benifitted in case same Url is used 
    /// at multiple places to get resource. In such case this class helps save the root node of type 
    /// <see cref="HtmlAgilityPack.HtmlNode"/> only once from the onlien resource.
    /// This saves multiple online call to same resource
    /// </summary>
    public class HtmlNodeCache : ResourceCache<HtmlNode>
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(HtmlNodeCache));

        /// <summary>
        /// Override the Load method to get the resouce from online / offline
        /// Loading an Html document multiple times from online is definitely costly.
        /// But it is also costly to load it from Local resouce.
        /// </summary>
        /// <param name="uri">The uri of the resouce</param>
        /// <returns></returns>
        public override HtmlNode Load(string absoulteUri)
        {
            Uri uriObj = new Uri(absoulteUri);

            string htmlText = "";

            WebRequest webRequestObj = WebRequest.Create(uriObj);

            if (uriObj.Scheme.Contains("http"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + absoulteUri);
                return null;
            }
            else if (uriObj.Scheme.Contains("http"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if(httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    logger.ErrorFormat("Http web response for url {0} status code {1}", absoulteUri, httpResponse.StatusCode);
                    return null;
                }
            }

            using (StreamReader reader = new StreamReader(webResponseObj.GetResponseStream()))
            {
                htmlText = reader.ReadToEnd();
            }

            HtmlDocument document = new HtmlDocument();
            new HtmlDocument().LoadHtml(htmlText);

            return document.DocumentNode;
        }
    }
}

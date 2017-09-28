using HtmlAgilityPack;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using QWSettings;

namespace QWCommonDST.Cache
{
    /// <summary>
    /// The cache to store All HtmlLocators used in the application. This is benifitted in case same Url is used 
    /// at multiple places to get resource. In such case this class helps save the root node of type 
    /// <see cref="HtmlAgilityPack.HtmlNode"/> only once from the onlien resource.
    /// This saves multiple online call to same resource
    /// </summary>
    public class HtmlDocCacheStore
    {
        /// <summary>
        /// The logger
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(HtmlDocCacheStore));

        /// <summary>
        /// The Main cache data which stores key <see cref="string"/> for each <see cref="TCache"/>
        /// </summary>
        protected Dictionary<string, HtmlDocCache> CacheTree { get; set; }

        public List<string> CacheOfflineSavedTree { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlDocCacheStore()
        {
            CacheTree = new Dictionary<string, HtmlDocCache>();
            CacheOfflineSavedTree = new List<string>();
        }

        /// <summary>
        /// This method loads the resource.
        /// It first checks from the cache, if found returns the resource, else
        /// it makes a raw call to fetch the resource
        /// </summary>
        /// <param name="TCacheKey"></param>
        /// <returns></returns>
        public HtmlDocCache Retrieve(Uri uriOnlineObj, Uri uriOfflineObj, bool isonline = true)
        {
            Uri uriObj = null;
            if (isonline && uriOnlineObj != null) uriObj = uriOnlineObj;
            else if ((!isonline || (isonline && uriOnlineObj == null)) && uriOfflineObj != null) uriObj = uriOfflineObj;

            if (uriObj == null) throw new Exception("Provide a valid uri");

            if (CacheTree.ContainsKey(uriObj.AbsoluteUri))
            {
                return CacheTree[uriObj.AbsoluteUri];
            }
            else
            {
                HtmlDocCache resource = Load(uriOnlineObj, uriOfflineObj, isonline);
                CacheTree.Add(uriObj.AbsoluteUri, resource);

                return resource;
            }
        }

        /// <summary>
        /// Override the Load method to get the resouce from online / offline
        /// Loading an Html document multiple times from online is definitely costly.
        /// But it is also costly to load it from Local resouce.
        /// </summary>
        /// <param name="uri">The uri of the resouce</param>
        /// <returns></returns>
        public HtmlDocCache Load(Uri uriOnlineObj, Uri uriOfflineObj, bool isonline = true)
        {
            HtmlDocCache cache = new HtmlDocCache();
            cache.UrlUsed = null;

            if (UrlHelperSettings.UseOfflineLinkPreference && uriOfflineObj != null)
            {
                cache.UrlUsed = uriOfflineObj;
                if (!File.Exists(uriOfflineObj.AbsolutePath))
                {
                    if (uriOnlineObj != null) cache.UrlUsed = uriOnlineObj;
                    else
                        throw new Exception("Provide a valid Url");
                }
                
            }
            else if (isonline && uriOnlineObj != null) cache.UrlUsed = uriOnlineObj;
            else if ((!isonline || (isonline && uriOnlineObj == null)) && uriOfflineObj != null) cache.UrlUsed = uriOfflineObj;
            else
                throw new Exception("Provide a valid Url");

            WebRequest webRequestObj = WebRequest.Create(cache.UrlUsed);

            if (cache.UrlUsed.Scheme.Contains("http"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                logger.Error("No web response found for " + cache.UrlUsed.AbsoluteUri);
                return null;
            }
            else if (cache.UrlUsed.Scheme.Contains("http"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if(httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    logger.ErrorFormat("Http web response for url {0} status code {1}", cache.UrlUsed.AbsoluteUri, httpResponse.StatusCode);
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

            cache.Document = document;

            if (IsOnline(cache.UrlUsed))
            {
                cache.IsOfflineUrl = false;

                if(uriOfflineObj != null)
                {
                    new FileInfo(uriOfflineObj.AbsolutePath).Directory.Create();
                    document.Save(uriOfflineObj.AbsolutePath);
                }
            }

            return cache;
        }

        /// <summary>
        /// Check if the URI is online
        /// </summary>
        /// <param name="absoluteUri"></param>
        /// <returns></returns>
        public bool IsOnline(Uri uriObj) => uriObj.AbsoluteUri.StartsWith("http:") 
            || uriObj.AbsoluteUri.StartsWith("https:");
    }

    public class HtmlCacheData
    {
        public HtmlDocument Document { get; set; }
        public bool IsOffline { get; set; }

        public HtmlCacheData()
        {
            IsOffline = false;
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ScrapEngine.Bl
{
    public class HtmlScrapperCommand
    {
        public HtmlNode Load(string url)
        {
            WebRequest webRequestObj = WebRequest.Create(url);

            if (url.Contains("http:/") || url.Contains("https:/"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
                httpWebRequestObj.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            if (webResponseObj == null)
            {
                //logger.Error("No web response found for " + url);
                return null;
            }
            else if (url.Contains("http:/") || url.Contains("https:/"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    //logger.ErrorFormat("Http web response for url {0} status code {1}", url, httpResponse.StatusCode);
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
        
        public StreamReader LoadFile(string url)
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
                //logger.Error("No web response found for " + url);
                return null;
            }
            else if (url.Contains("http:/"))
            {
                HttpWebResponse httpResponse = (HttpWebResponse)webResponseObj;
                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    //logger.ErrorFormat("Http web response for url {0} status code {1}", url, httpResponse.StatusCode);
                    return null;
                }
            }

            return new StreamReader(webResponseObj.GetResponseStream());
        }

        public HtmlNode LoadOnline(string url)
        {
            var htmlWeb = new HtmlWeb();
            htmlWeb.OverrideEncoding = Encoding.UTF8;
            var doc = htmlWeb.Load(url);

            return doc.DocumentNode;
        }
    }
}

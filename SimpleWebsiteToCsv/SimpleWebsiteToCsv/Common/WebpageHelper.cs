using HtmlAgilityPack;
using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.WebPageDataSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SimpleWebsiteToCsv.Common
{
    public class WebpageHelper
    {
        public static HtmlDocument GetHtmlDocument(string url)
        {
            Uri uriObj = new Uri(url);
            string htmlText = "";

            if (uriObj.Scheme.Contains("http"))
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uriObj);
                req.Method = "GET";
                req.Proxy.Credentials = CredentialCache.DefaultCredentials;
                HttpWebResponse objResponse = (HttpWebResponse)req.GetResponse();

                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    htmlText = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
            }
            else if(uriObj.Scheme.Contains("file"))
            {
                WebRequest req = WebRequest.Create(uriObj);
                WebResponse objResponse = req.GetResponse();

                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    htmlText = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlText);

            return document;
        }

        public static string Normalize(string value)
        {
            string result = value.Trim();
            result = result.Replace("&nbsp;", " ").Trim();
            return result;
        }

        public static void HtmlParser<TKey, T>(Dictionary<TKey, T> Table, string keyName)
            where T : new()
        {
            Type webPageDataSetClass = typeof(T);
            bool isUpdateTable = true;

            if (Table == null)
            {
                isUpdateTable = false;
                Table = new Dictionary<TKey, T>();
            }

            // Get reference attribute
            CitationAttribute refAttribute = (CitationAttribute)
                (webPageDataSetClass.GetCustomAttribute(typeof(CitationAttribute)));

            //Fix for "HtmlAgilityPack Drops Option End Tags"
            // https://stackoverflow.com/questions/293342/htmlagilitypack-drops-option-end-tags
            //
            HtmlNode.ElementsFlags.Remove("option");
            HtmlDocument document = 
                (GlobalSettings.UseOfflineMode)? GetHtmlDocument(refAttribute.UrlOffline)
                : GetHtmlDocument(refAttribute.UrlOnline);

            

            // Get Attributes of type
            ReferenceCollectionAttribute refCollAttribute = (ReferenceCollectionAttribute)
                (webPageDataSetClass.GetCustomAttribute(typeof(ReferenceCollectionAttribute)));

            if(refCollAttribute != null)
                HtmlParserForCollection(Table, keyName, refCollAttribute, document, webPageDataSetClass, isUpdateTable);
        }

        public static T HtmlParserForSingle<T>(string url) where T : new()
        {
            Type webPageDataSetClass = typeof(T);
            T result = new T();

            // Get reference attribute
            CitationAttribute refAttribute = (CitationAttribute)
                (webPageDataSetClass.GetCustomAttribute(typeof(CitationAttribute)));

            HtmlDocument document =
                (GlobalSettings.UseOfflineMode) ? GetHtmlDocument(url)
                : GetHtmlDocument(refAttribute.UrlOnline);

            ReferenceSingleAttribute refSingleAttribute = (ReferenceSingleAttribute)
                (webPageDataSetClass.GetCustomAttribute(typeof(ReferenceSingleAttribute), false));

            if (result is ISteps)
            {
                ((ISteps)result).Begin();
            }

            HtmlNode node = document.DocumentNode.SelectSingleNode(refSingleAttribute.XPath);

            PropertyInfo[] propertyInfos = webPageDataSetClass.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int indx = 0; indx < propertyInfos.Length; ++indx)
            {
                PopulateValue(propertyInfos[indx], node, result);
            }

            if(result is ISteps)
            {
                ((ISteps)result).Final();
            }

            return result;
        }

        private static void HtmlParserForCollection<TKey, T>(Dictionary<TKey, T> Table, string keyName, 
            ReferenceCollectionAttribute refCollAttribute, HtmlDocument document,
            Type webPageDataSetClass, bool isUpdateTable) where T : new()
        {
            PropertyInfo[] propertyInfos = webPageDataSetClass.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            HtmlNodeCollection nodeCollection = document.DocumentNode.SelectNodes(refCollAttribute.XPath);

            foreach (HtmlNode node in nodeCollection)
            {
                T row = new T();
                TKey key = default(TKey);

                if (row is ISteps)
                {
                    ((ISteps)row).Begin();
                }

                for (int indx = 0; indx < propertyInfos.Length; ++indx)
                {
                    
                    object value = PopulateValue(propertyInfos[indx], node, row);

                    if (string.Compare(propertyInfos[indx].Name, keyName, false) == 0)
                        key = (TKey)value;
                }

                if (isUpdateTable)
                {
                    if (Table.ContainsKey(key))
                    {
                        T existingRow = Table[key];

                        if (row is ICopyable<T>)
                        {
                            ((ICopyable<T>)row).CopyFrom(existingRow);
                        }
                    }
                    else
                    {
                        Console.WriteLine("The Key '{0}' not found in the table", key);
                    }
                }

                Table.Add(key, row);

                if (row is ISteps)
                {
                    ((ISteps)row).Final();
                }

                Console.WriteLine("Processed row for " + key);
            }
        }

        private static object PopulateValue<T>(PropertyInfo propertyInfo, HtmlNode node, T mainObj)
        {
            object value = null;

            ReferenceSingleAttribute singleAttribute = (ReferenceSingleAttribute)
                propertyInfo.GetCustomAttribute(typeof(ReferenceSingleAttribute));
            if (singleAttribute != null)
            {
                HtmlNode nodeData = node;

                if (!string.IsNullOrEmpty(singleAttribute.XPath))
                    nodeData = node.SelectSingleNode(singleAttribute.XPath);
                if (nodeData != null)
                {
                    if (!string.IsNullOrEmpty(singleAttribute.Attribute))
                    {
                        HtmlAttribute attributeNode = nodeData.Attributes[singleAttribute.Attribute];
                        if (attributeNode != null)
                            value = attributeNode.Value;
                        else
                            Console.WriteLine("No attribute '" + propertyInfo.Name + "'");
                    }
                    else
                    {
                        if (singleAttribute.UseInnerHtml)
                            value = nodeData.InnerHtml;
                        else
                            value = nodeData.InnerText;
                    }
                }
                else
                {
                    Console.WriteLine("No node data found for " + propertyInfo.Name);
                }

                // Normalize Value from attribute settings
                value = Normalize(value, propertyInfo, singleAttribute);

                // Set value
                if (value != null)
                {
                    if (propertyInfo.PropertyType == typeof(int))
                    {
                        propertyInfo.SetValue(mainObj, Convert.ToInt32(value));
                    }
                    else if (propertyInfo.PropertyType == typeof(string))
                    {
                        propertyInfo.SetValue(mainObj, value.ToString());
                    }
                    else if (propertyInfo.PropertyType == typeof(bool))
                    {
                        propertyInfo.SetValue(mainObj, (value != null));
                    }
                    else if (propertyInfo.PropertyType == typeof(FieldData))
                    {
                        ((FieldData)propertyInfo.GetValue(mainObj)).Parse(value);
                    }
                }
            }

            return value;
        }

        private static object Normalize(object value, PropertyInfo propertyInfo, ReferenceSingleAttribute singleAttribute)
        {
            if (value == null) return value;

            string valueText = value.ToString();
            if (singleAttribute.UseTrim)
                valueText = valueText.Trim();

            RegexReplaceAttribute[] regexReplaceAttrs = (RegexReplaceAttribute[])
                propertyInfo.GetCustomAttributes<RegexReplaceAttribute>();

            if (regexReplaceAttrs != null && regexReplaceAttrs.Length > 0)
            {
                // Sort by Order property ascending
                Array.Sort(regexReplaceAttrs, delegate (RegexReplaceAttribute x, RegexReplaceAttribute y)
                    { return (x.Order < y.Order) ? -1 : ((x.Order > y.Order) ? 1 : 0); });

                for (int indx = 0; indx < regexReplaceAttrs.Length; ++indx)
                {
                    valueText = Regex.Replace(valueText, regexReplaceAttrs[indx].Pattern, regexReplaceAttrs[indx].ReplaceText);
                }
            }

            return valueText;
        }
    }
}

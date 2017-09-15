using HtmlAgilityPack;
using log4net;
using SimpleWebsiteToCsv.Common;
using SimpleWebsiteToCsv.Metadata;
using SimpleWebsiteToCsv.WebPageDataSet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleWebsiteToCsv.RawParser
{
    /// <summary>
    /// A html Webpage parser.
    /// It uses a class definition to Get the information to parse the Webpage and store in 
    /// the type itself
    /// It is important for the type to follow certain conventions:
    /// 1. It must have a default constructor defined
    /// </summary>
    public class HttpWebpageEngine<T> where T : class, new()
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static ILog _logger = LogManager.GetLogger(typeof(HttpWebpageEngine<T>));

        /// <summary>
        /// The document representing the whole Html page
        /// </summary>
        private HtmlDocument _document;

        /// <summary>
        /// Represents the current Html node which is getting processed.
        /// A class which contains the <see cref="ReferenceSingleAttribute"/> is represented by this node.
        /// </summary>
        private HtmlNode _currentRootNode;

        /// <summary>
        /// Represents the current collection node. A class which contains the <see cref="ReferenceCollectionAttribute"/>
        /// is represented by this collection
        /// </summary>
        private HtmlNodeCollection _currentRootNodeCollection;

        /// <summary>
        /// The list of Public and instance properties
        /// </summary>
        private PropertyInfo[] _typeProperties;

        /// <summary>
        /// The Actual Parsed object which is the final result for a class type with <see cref="ReferenceSingleAttribute"/>
        /// </summary>
        private T _parsedObject;

        /// <summary>
        /// The Parsed dictionary object for a class with <see cref="ReferenceCollectionAttribute"/>
        /// Such class must have at least one property defined with <see cref="UniqueDictionaryKeyAttribute"/>.
        /// In case of multiple <see cref="UniqueDictionaryKeyAttribute"/>, each Key of the 'string' type
        /// is the name of the Property
        /// </summary>
        private Dictionary<string, Dictionary<object, T>> _parsedObjectCollection;

        /// <summary>
        /// Get the <see cref="ISteps"/> interface type
        /// </summary>
        private ISteps ParsedObjectSteps
        {
            get {  return _parsedObject as ISteps; }
        }

        /// <summary>
        /// The <see cref="CitationAttribute"/> object defined on the class
        /// </summary>
        private CitationAttribute _citationAttribute;

        /// <summary>
        /// The <see cref="ReferenceCollectionAttribute"/> object defined on the class
        /// </summary>
        private ReferenceCollectionAttribute _refCollectionAttribute;

        /// <summary>
        /// The <see cref="ReferenceSingleAttribute"/> object defined on the class. A class cannot have both 
        /// <see cref="ReferenceSingleAttribute"/> and <see cref="ReferenceCollectionAttribute"/>
        /// </summary>
        private ReferenceSingleAttribute _refSingleAttribute;

        /// <summary>
        /// The url used to load the webpage. It can be online, offline (or local) or custom provided
        /// </summary>
        public string WebpageUrl { get; protected set; }

        /// <summary>
        /// Public property to get the final object. This is null for collection type
        /// </summary>
        public T ParsedObject { get { return _parsedObject; } }
        
        /// <summary>
        /// Public property to get the desired dictionary object for the unique key column type
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public Dictionary<object, T> GetCollection(string columnName)
        {
            return _parsedObjectCollection[columnName];
        }

        public HttpWebpageEngine()
        {
            _citationAttribute = GetAttributeObject<CitationAttribute>();
            _typeProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public void Parse(string overrideUrl = "")
        {
            ParseReadHtmlDocument(overrideUrl);
            ParseTypeXPathAttributes();
        }

        private void ParseTypeXPathAttributes()
        {
            // When a XPATH will return a collection of Types
            _refCollectionAttribute = GetAttributeObject<ReferenceCollectionAttribute>();

            // Either a collection attribute or a single attribute for xpath should be present
            if(_refCollectionAttribute == null)
            {
                _refSingleAttribute = GetAttributeObject<ReferenceSingleAttribute>();

                if(_refSingleAttribute != null)
                {
                    ParseSingleType();
                }
            }
            else
            {
                ParseCollectionType();
            }
        }

        private void ParseReadHtmlDocument(string overrideUrl)
        {
            if (string.IsNullOrEmpty(overrideUrl))
            {
                if (_citationAttribute == null)
                    throw new Exception(string.Format("Unable to retrive CitationAttribute for class " + typeof(T).Name));

                WebpageUrl = (GlobalSettings.UseOfflineMode)? 
                    _citationAttribute.UrlOffline : _citationAttribute.UrlOnline;
            }
            else
            {
                WebpageUrl = overrideUrl;
            }

            ReadHtmlDocument(WebpageUrl);
        }

        private void ParseCollectionType()
        {
            _parsedObjectCollection = new Dictionary<string, Dictionary<object, T>>();

            // Get the unique keys and create the dictionaries
            for(int indx = 0; indx < _typeProperties.Length; ++indx)
            {
                PropertyInfo propertyInfo = _typeProperties[indx];

                UniqueDictionaryKeyAttribute uniqueKeyAttribute = GetAttributeObject<UniqueDictionaryKeyAttribute>(propertyInfo);
                if(uniqueKeyAttribute != null)
                {
                    _parsedObjectCollection.Add(propertyInfo.Name, new Dictionary<object, T>());
                }
            }

            if(_parsedObjectCollection.Count <= 0)
            {
                throw new Exception("No Unique attribute defined for " + typeof(T));
            }

            _currentRootNodeCollection = _document.DocumentNode.SelectNodes(_refCollectionAttribute.XPath);

            foreach(HtmlNode node in _currentRootNodeCollection)
            {
                _currentRootNode = node;
                ParseSingleType();

                // Add to dictionaries
                for (int indx = 0; indx < _typeProperties.Length; ++indx)
                {
                    PropertyInfo propertyInfo = _typeProperties[indx];

                    UniqueDictionaryKeyAttribute uniqueKeyAttribute = GetAttributeObject<UniqueDictionaryKeyAttribute>(propertyInfo);
                    if (uniqueKeyAttribute != null)
                    {
                        _parsedObjectCollection[propertyInfo.Name].Add(propertyInfo.GetValue(_parsedObject), _parsedObject);
                    }
                }

                _parsedObject = default(T);
            }
        }

        private void ParseSingleType()
        {
            _parsedObject = new T();
            
            if (ParsedObjectSteps != null) ParsedObjectSteps.Begin();

            _currentRootNode = _document.DocumentNode.SelectSingleNode(_refSingleAttribute.XPath);

            for (int indx = 0; indx < _typeProperties.Length; ++indx)
            {
                ParseAndPopulateValue(_typeProperties[indx]);
            }
        }

        private void ParseAndPopulateValue(PropertyInfo propertyInfo)
        {
            object value = null;
            HtmlNode nodeData = _currentRootNode;

            if (!string.IsNullOrEmpty(_refSingleAttribute.XPath))
                nodeData = nodeData.SelectSingleNode(_refSingleAttribute.XPath);
            if (nodeData != null)
            {
                if (!string.IsNullOrEmpty(_refSingleAttribute.Attribute))
                {
                    HtmlAttribute attributeNode = nodeData.Attributes[_refSingleAttribute.Attribute];
                    if (attributeNode != null)
                        value = attributeNode.Value;
                    else
                        Console.WriteLine("No attribute xpath defined for '" + propertyInfo.Name + "'");
                }
                else
                {
                    if (_refSingleAttribute.UseInnerHtml)
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
            value = ParseNormalizeValue(value, propertyInfo);

            // Set value
            if (value != null)
            {
                ParseSetValue(value, propertyInfo);
            }
        }

        private object ParseNormalizeValue(object value, PropertyInfo propertyInfo)
        {
            if (value == null) return value;

            string valueText = value.ToString();
            if (_refSingleAttribute.UseTrim)
                valueText = valueText.Trim();

            RegexReplaceAttribute[] regexReplaceAttrs = GetAttributeArray<RegexReplaceAttribute>(propertyInfo);

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

        private void ParseSetValue(object value, PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(int))
            {
                propertyInfo.SetValue(_parsedObject, Convert.ToInt32(value));
            }
            else if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(_parsedObject, value.ToString());
            }
            else if (propertyInfo.PropertyType == typeof(bool))
            {
                BoolConversionAttribute boolConversionAttrObj = GetAttributeObject<BoolConversionAttribute>(propertyInfo);
                if(boolConversionAttrObj != null)
                {
                    bool bConverted = boolConversionAttrObj.TruthValues.ToLower().Contains(value.ToString());
                    propertyInfo.SetValue(_parsedObject, bConverted);
                }
                else
                    propertyInfo.SetValue(_parsedObject, (value != null));
            }
            else if (propertyInfo.PropertyType == typeof(FieldData))
            {
                ((FieldData)propertyInfo.GetValue(_parsedObject)).Parse(value);
            }
        }

        private TAttribute GetAttributeObject<TAttribute>() where TAttribute : Attribute
        {
            return (TAttribute)typeof(T).GetCustomAttribute(typeof(TAttribute));
        }

        private TAttribute GetAttributeObject<TAttribute>(PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            return (TAttribute)propertyInfo.GetCustomAttribute(typeof(TAttribute));
        }

        private TAttribute[] GetAttributeArray<TAttribute>(PropertyInfo propertyInfo) where TAttribute : Attribute
        {
            return (TAttribute[])propertyInfo.GetCustomAttributes(typeof(TAttribute));
        }

        private void ReadHtmlDocument(string url)
        {
            Uri uriObj = new Uri(url);
            string htmlText = "";

            WebRequest webRequestObj = WebRequest.Create(uriObj);

            if (uriObj.Scheme.Contains("http"))
            {
                HttpWebRequest httpWebRequestObj = (HttpWebRequest)webRequestObj;
                httpWebRequestObj.Method = "GET";
                httpWebRequestObj.Proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            WebResponse webResponseObj = webRequestObj.GetResponse();

            using (StreamReader reader = new StreamReader(webResponseObj.GetResponseStream()))
            {
                htmlText = reader.ReadToEnd();
            }

            _document = new HtmlDocument();
            _document.LoadHtml(htmlText);
        }
    }
}

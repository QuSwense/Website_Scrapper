using System;
using System.Reflection;
using System.Xml;
using WebReader.Model;
using WebCommon.Extn;
using WebCommon.Error;
using log4net;

namespace WebReader.Xml
{
    /// <summary>
    /// A class helps in reading and parsing Xml file
    /// </summary>
    public class DXmlDocReader
    {
        #region Properties

        /// <summary>
        /// The private logger
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(DXmlSerializeReader));

        /// <summary>
        /// The file path
        /// </summary>
        public string FilePath { get; protected set; }

        /// <summary>
        /// The xml document
        /// </summary>
        private XmlDocument xmlDocument;
        
        #endregion Properties

        #region Create

        /// <summary>
        /// Constructor
        /// </summary>
        public DXmlDocReader() { }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Initialize(string filePath)
        {
            logger.DebugFormat("Initialize an instance of DXmlDocReader with file path '{0}'", filePath);

            FilePath = filePath;
            xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
        }

        /// <summary>
        /// A static initializer
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DXmlDocReader Create(string filePath)
        {
            DXmlDocReader xmlReader = new DXmlDocReader();
            xmlReader.Initialize(filePath);
            return xmlReader;
        }

        #endregion Create

        #region Read

        /// <summary>
        /// Read the nodes
        /// </summary>
        /// <param name="xpath"></param>
        public XmlNodeList ReadNodes(string xpath, XmlNode currentElement = null)
        {
            logger.DebugFormat("Read the next set of nodes using XPath '{0}'", xpath);

            if (currentElement == null)
                return xmlDocument.DocumentElement.SelectNodes(xpath);
            return currentElement.SelectNodes(xpath);
        }

        /// <summary>
        /// Parse and Read the attributes on the element and store it in the type object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadElement<T>(XmlNode currentElement) where T: new()
        {
            logger.DebugFormat("Read the current Xml node element tag <{0}> and its attributes into type '{1}'",
                currentElement.LocalName, typeof(T));

            T obj = new T();
            Type objType = typeof(T);

            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var item in properties)
            {
                DXmlAttributeAttribute attrAttributeObj = item.GetCustomAttribute<DXmlAttributeAttribute>();
                if(attrAttributeObj != null)
                {
                    XmlAttribute attribute = currentElement.Attributes[attrAttributeObj.Name];
                    if (attrAttributeObj.IsMandatory && attribute == null)
                        throw new XmlDocReaderException(attrAttributeObj.Name, XmlDocReaderException.EErrorType.ATRRIBUTE_VALUE_NULL);
                    if(attribute != null) item.SetValue(obj, item.PropertyType.ChangeType(attribute.Value));
                }
            }

            return obj;
        }

        #endregion Read
    }
}

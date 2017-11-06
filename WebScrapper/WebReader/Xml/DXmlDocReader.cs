using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using WebReader.Model;
using WebCommon.Extn;

namespace WebReader.Xml
{
    /// <summary>
    /// A class helps in reading and parsing Xml file
    /// </summary>
    public class DXmlDocReader
    {
        #region Properties

        /// <summary>
        /// The file path
        /// </summary>
        public string FilePath { get; protected set; }
        public object DXmlAttributeAttribute { get; private set; }

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
            FilePath = filePath;
            xmlDocument = new XmlDocument();
            xmlDocument.Load(filePath);
            currentElement = xmlDocument.DocumentElement;
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
            T obj = new T();
            Type objType = typeof(T);

            PropertyInfo[] properties = objType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var item in properties)
            {
                DXmlAttributeAttribute attrAttributeObj = objType.GetCustomAttribute<DXmlAttributeAttribute>();
                if(attrAttributeObj != null)
                {
                    item.SetValue(obj,
                        objType.ChangeType(currentElement.Attributes[attrAttributeObj.Name].Value));
                }
            }

            return obj;
        }

        #endregion Read
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebReader.Model;
using WebCommon.Extn;

namespace WebReader.Xml
{
    public class DXmlReader<T> where T : class, new()
    {
        private XmlReader xmlReader;
        private int currentDepth;
        Dictionary<string, XmlAttrPropertyMap> mapAttrProperties;
        Dictionary<string, XmlElementPropertyMap> mapElementProperties;

        internal class XmlAttrPropertyMap
        {
            public PropertyInfo PropInfo { get; set; }
            public DXmlAttributeAttribute AttrAttriibute { get; set; }

            public object InstanceObj { get; set; }
        }

        internal class XmlElementPropertyMap
        {
            public PropertyInfo PropInfo { get; set; }
            public DXmlElementAttribute ElementAttriibute { get; set; }

            public object InstanceObj { get; set; }
        }

        public T Root { get; set; }

        public void Read(string filePath)
        {
            xmlReader = XmlReader.Create(filePath);
            mapAttrProperties = new Dictionary<string, XmlAttrPropertyMap>();
            mapElementProperties = new Dictionary<string, XmlElementPropertyMap>();

            Root = new T();
            //CreatePropertyMap(typeof(T));

            currentDepth = xmlReader.Depth;
            object instanceObj = Root;
            Stack<object> stackInstanceObj = new Stack<object>();

            ParseAttributes(instanceObj, xmlReader);

            while (xmlReader.Read())
                if (xmlReader.IsStartElement())
                {
                    if(currentDepth > xmlReader.Depth)
                    {
                        // One of the child element of the current instance 
                        // should be initialized
                        stackInstanceObj.Push(instanceObj);
                        instanceObj = FetchChildElementInstance(instanceObj, xmlReader);
                    }
                    else if(currentDepth < xmlReader.Depth)
                    {
                        instanceObj = stackInstanceObj.Pop();
                        instanceObj = FetchChildElementInstance(instanceObj, xmlReader);
                    }
                    ParseAttributes(instanceObj, xmlReader);
                    currentDepth = xmlReader.Depth;
                }
        }

        private object FetchChildElementInstance(object instanceObj, XmlReader xmlReader)
        {
            Type instanceType = instanceObj.GetType();
            PropertyInfo[] propInfos = instanceType.GetProperties(BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute(typeof(DXmlElementAttribute)) != null).ToArray();

            if (propInfos != null && propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    List<DXmlElementAttribute> elemAttributes = propInfo.GetCustomAttributes<DXmlElementAttribute>().ToList();
                    DXmlElementAttribute elemAttribute = elemAttributes.Where(p => p.Name == xmlReader.Name).FirstOrDefault();
                    if(elemAttribute != null)
                    {
                        object value = propInfo.GetValue(instanceObj);
                        if(value == null)
                        {
                            propInfo.SetValue(instanceObj, Activator.CreateInstance(propInfo.PropertyType));
                        }
                    }
                }
            }

            return null;
        }

        private void ParseAttributes(object instanceObj, XmlReader xmlReader)
        {
            Type instanceType = instanceObj.GetType();
            if (xmlReader.HasAttributes)
            {
                PropertyInfo[] propInfos = instanceType.GetProperties(BindingFlags.Instance)
                    .Where(p => p.GetCustomAttribute(typeof(DXmlAttributeAttribute)) != null).ToArray();

                if(propInfos != null && propInfos.Length > 0)
                {
                    foreach (PropertyInfo propInfo in propInfos)
                    {
                        DXmlAttributeAttribute attrAttribute = propInfo.GetCustomAttribute<DXmlAttributeAttribute>();
                        string value = xmlReader.GetAttribute(attrAttribute.Name);
                        propInfo.SetValue(instanceObj, instanceType.ChangeType(value));
                    }
                }
            }
        }

        private void ParseElement(object instanceObj, XmlReader xmlReader)
        {
            if(mapAttrProperties.ContainsKey(xmlReader.Name))
            {

            }
        }

        private void CreatePropertyMap(Type typeNode)
        {
            PropertyInfo[] PropInfos = typeNode.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            List<PropertyInfo> reqPropInfos = new List<PropertyInfo>();

            foreach (var item in PropInfos)
            {
                if (item.PropertyType.IsClass)
                    CreatePropertyMap(item.PropertyType);
                else
                {
                    List<DXmlAttributeAttribute> attrAttributes = item.GetCustomAttributes<DXmlAttributeAttribute>().ToList();
                    if (attrAttributes != null)
                    {
                        foreach (var attrAttribute in attrAttributes)
                        {
                            mapAttrProperties.Add(attrAttribute.Name, new XmlAttrPropertyMap()
                            {
                                AttrAttriibute = attrAttribute,
                                PropInfo = item
                            });
                        }
                    }

                    List<DXmlElementAttribute> elementAttributes = item.GetCustomAttributes<DXmlElementAttribute>().ToList();
                    if (elementAttributes != null)
                    {
                        foreach (var elementAttribute in elementAttributes)
                        {
                            mapElementProperties.Add(elementAttribute.Name, new XmlElementPropertyMap()
                            {
                                ElementAttriibute = elementAttribute,
                                PropInfo = item
                            });
                        }
                    }
                }
            }
        }
    }
}

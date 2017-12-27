using ScrapException;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebCommon.Extn;
using WebReader.Model;

namespace WebReader.Xml
{
    /// <summary>
    /// A helper class for <see cref="DXmlReader{T}"/>.
    /// It Parses and processes the properties of the element class
    /// </summary>
    public class DXmlPropertyReader<T> where T : class, new()
    {
        #region Properties Private

        /// <summary>
        /// The Parent Xml reader instance
        /// </summary>
        private DXmlReader<T> Parent { get; set; }

        #endregion Properties Private

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DXmlPropertyReader(DXmlReader<T> parent)
        {
            Parent = parent;
        }

        #endregion Constructors

        #region Helpers

        /// <summary>
        /// Get an array of all properties for a type which contains <see cref="DXmlElementAttribute"/>
        /// </summary>
        /// <returns></returns>
        private PropertyInfo[] GetAllPropertiesForElement(Type instanceType)
        {
            return instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.HasCustomAttributes<DXmlElementAttribute>()).ToArray();
        }

        /// <summary>
        /// get an array of all properties for a type which contains <see cref="DXmlElementAttribute"/>
        /// </summary>
        /// <returns></returns>
        private PropertyInfo[] GetAllPropertiesForAttribute(Type instanceType)
        {
            return instanceType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.HasCustomAttributes<DXmlAttributeAttribute>()).ToArray();
        }

        /// <summary>
        /// Get the <see cref="DXmlElementAttribute"/> object for the xml node
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public DXmlElementAttribute GetElementAttribute(PropertyInfo propInfo, string nodeName)
        {
            List<DXmlElementAttribute> elemAttributes = propInfo.GetCustomAttributes<DXmlElementAttribute>().ToList();
            return elemAttributes.Where(p => p.Name == nodeName).FirstOrDefault();
        }

        #endregion Helpers

        #region Process Private Methods

        /// <summary>
        /// Create the property if its null.
        /// It also creates a list type. In case of object it may use Derived type
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="actualParentInstanceObj"></param>
        /// <param name="elemAttribute"></param>
        private void CreateAndInitializeProperty(PropertyInfo propInfo, object actualParentInstanceObj,
            DXmlElementAttribute elemAttribute)
        {
            object value = propInfo.GetValue(actualParentInstanceObj);

            // If the property is null create a new instance
            if (value == null)
            {
                propInfo.SetValue(actualParentInstanceObj,
                        Activator.CreateInstance(
                            (elemAttribute.DerivedType != null && !propInfo.PropertyType.IsGenericListType()) ?
                            elemAttribute.DerivedType : propInfo.PropertyType));
                value = propInfo.GetValue(actualParentInstanceObj);
            }

            if (propInfo.PropertyType.IsGenericListType())
                Parent.AddNewListTypeInstance(propInfo.PropertyType, elemAttribute, (IList)value);
        }

        #endregion Process Private Methods

        #region Process Public Methods

        /// <summary>
        /// Parse the properties of an element class type
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <param name="actualParentInstanceObj"></param>
        /// <returns></returns>
        public StateXmlParse FetchChildElementInstance(XmlReader xmlReader, object actualParentInstanceObj)
        {
            PropertyInfo[] propInfos = GetAllPropertiesForElement(actualParentInstanceObj.GetType());

            if (propInfos != null && propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    DXmlElementAttribute elemAttribute = GetElementAttribute(propInfo, xmlReader.Name);
                    if (elemAttribute != null)
                    {
                        CreateAndInitializeProperty(propInfo, actualParentInstanceObj, elemAttribute);

                        return new StateXmlParse()
                        {
                            ElemPropertyAttribute = elemAttribute,
                            InstanceObj = propInfo.GetValue(actualParentInstanceObj),
                            InstanceType = propInfo.PropertyType,
                            Accessed = 1,
                            PropInfo = propInfo
                        };
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Fetch and parse the attributes of the node and assign the values to the instance object properties
        /// Assumed that the properties are public
        /// </summary>
        public void ParseAndPopulateAttributes(XmlReader xmlReader, object actualInstanceObj)
        {
            if (xmlReader.HasAttributes)
            {
                PropertyInfo[] propInfos = GetAllPropertiesForAttribute(actualInstanceObj.GetType());

                if (propInfos != null && propInfos.Length > 0)
                {
                    foreach (PropertyInfo propInfo in propInfos)
                    {
                        DXmlAttributeAttribute attrAttribute = propInfo.GetCustomAttribute<DXmlAttributeAttribute>();
                        object value = xmlReader.GetAttribute(attrAttribute.Name) ?? attrAttribute.Default;

                        if (value != null)
                            propInfo.SetValue(actualInstanceObj, propInfo.PropertyType.ChangeType(value));
                        else if (value == null && attrAttribute.IsMandatory)
                            throw new ScrapXmlException(ScrapXmlException.EErrorType.MANDATORY_ATTRIBUTE_NOT_FOUND,
                                xmlReader.Name, propInfo.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Process the Properties Semantics
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        public void ProcessSemantics(Type type, object rootObj, object lastParentObj = null)
        {
            PropertyInfo[] props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in props)
            {
                bool bProcessOnlyOneElementAttr = false;
                foreach (var customAttrData in prop.CustomAttributes)
                {
                    ProcessSemantics(prop, customAttrData, rootObj, 
                        lastParentObj, ref bProcessOnlyOneElementAttr);
                }
            }
        }

        /// <summary>
        /// Process the Properties Semantics
        /// </summary>
        /// <param name="type"></param>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        private void ProcessSemantics(PropertyInfo prop,
            CustomAttributeData customAttrData, object rootObj, 
            object lastParentObj, ref bool bProcessOnlyOneElementAttr)
        {
            if (customAttrData.AttributeType == typeof(DXmlParentAttribute))
                ProcessSemanticsDXmlParent(prop, customAttrData, rootObj,
                        lastParentObj, ref bProcessOnlyOneElementAttr);
            else if (customAttrData.AttributeType == typeof(DXmlNormalizeAttribute))
                ProcessSemanticsDXmlNormalize(prop, customAttrData, rootObj,
                        lastParentObj, ref bProcessOnlyOneElementAttr);
            else if (customAttrData.AttributeType == typeof(DXmlElementAttribute))
                ProcessSemanticsDXmlElement(prop, customAttrData, rootObj,
                        lastParentObj, ref bProcessOnlyOneElementAttr);
        }

        /// <summary>
        /// Process semantics for <see cref="DXmlParentAttribute"/>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="customAttrData"></param>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        /// <param name="bProcessOnlyOneElementAttr"></param>
        private void ProcessSemanticsDXmlParent(PropertyInfo prop,
            CustomAttributeData customAttrData, object rootObj,
            object lastParentObj, ref bool bProcessOnlyOneElementAttr)
        {
            if((lastParentObj.GetType() == prop.PropertyType || 
                lastParentObj.GetType().IsSubclassOf(prop.PropertyType)))
                prop.SetValue(rootObj, lastParentObj);
        }

        /// <summary>
        /// Process semantics for <see cref="DXmlNormalizeAttribute"/>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="customAttrData"></param>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        /// <param name="bProcessOnlyOneElementAttr"></param>
        private void ProcessSemanticsDXmlNormalize(PropertyInfo prop,
            CustomAttributeData customAttrData, object rootObj,
            object lastParentObj, ref bool bProcessOnlyOneElementAttr)
        {
            if (prop.PropertyType != typeof(string))
                throw new ScrapXmlException(ScrapXmlException.EErrorType.NORMALIZE_ONLY_STRING_VALUE,
                    prop.Name);
            prop.SetValue(rootObj,
                DXmlNormalizeAttribute.Normalize((string)prop.GetValue(rootObj)));
        }

        /// <summary>
        /// Parse semantics for <see cref="DXmlElementAttribute"/>
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="customAttrData"></param>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        /// <param name="bProcessOnlyOneElementAttr"></param>
        private void ProcessSemanticsDXmlElement(PropertyInfo prop,
            CustomAttributeData customAttrData, object rootObj,
            object lastParentObj, ref bool bProcessOnlyOneElementAttr)
        {
            if (!bProcessOnlyOneElementAttr)
            {
                Parent.ProcessSemantics(prop.GetValue(rootObj), rootObj);
                bProcessOnlyOneElementAttr = true;
            }
        }

        #endregion Process Public Methods
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebReader.Model;
using WebCommon.Extn;
using System.Collections;

namespace WebReader.Xml
{
    /// <summary>
    /// A custom Xml reader which parses the Xml file using the <see cref="XmlReader"/> class.
    /// It takes a root class type as generic type argument which matches the root element of
    /// the xml file.
    /// The class uses the attribute class
    /// <see cref="DXmlAttributeAttribute"/>
    /// <see cref="DXmlElementAttribute"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DXmlReader<T> where T : class, new()
    {
        /// <summary>
        /// The xml reader object which is used for parsing
        /// </summary>
        private XmlReader xmlReader;

        /// <summary>
        /// The root class object instance.
        /// Access this object after the parsing of the xml file completed
        /// </summary>
        public T Root { get; set; }

        /// <summary>
        /// An internal helper class to maintain the state of the parsing
        /// </summary>
        public class State
        {
            public DXmlElementAttribute elemPropertyAttribute;
            public Type instanceType;
            public object instanceObj;
            public PropertyInfo propInfo;
            public int Accessed = 0;
        }

        /// <summary>
        /// A stack data structure to maintain a recursive list of states.
        /// We are not using recursion for the parsing logic
        /// </summary>
        public Stack<State> stackStates = new Stack<State>();

        /// <summary>
        /// The main parsing method to read an xml file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read(string filePath)
        {
            // Create the xml reader object
            xmlReader = XmlReader.Create(filePath);

            // Main the current depth during parsing
            // This value signifies the child / parent / sibling nodes
            int currentDepth = xmlReader.Depth;

            if (Root == null) Root = new T();

            // Stack the starting root node
            stackStates.Push(new State()
            {
                instanceType = typeof(T),
                instanceObj = Root,
                Accessed = 1
            });

            // Main loop
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    // Signifies child node
                    if (currentDepth < xmlReader.Depth)
                    {
                        // One of the child element of the current instance 
                        // should be initialized
                        stackStates.Push(FetchChildElementInstance(xmlReader, stackStates.Peek()));
                    }
                    // Signifies sibling node
                    else
                    {
                        FetchElementInstance(xmlReader);
                    }
                    ParseAttributes();
                    currentDepth = xmlReader.Depth;
                }
                else if (xmlReader.NodeType == XmlNodeType.EndElement)
                {
                    stackStates.Pop();
                    currentDepth--;
                }
            }
        }

        /// <summary>
        /// Fetch and process the sibling node
        /// At this time the current object should be initialized
        /// </summary>
        /// <param name="xmlReader"></param>
        public void FetchElementInstance(XmlReader xmlReader)
        {
            // Fetch the current state in the stack
            // This is actually the previous processed state
            State previousState = stackStates.Peek();

            Type type = previousState.instanceObj.GetType();

            // Check if the type is not list but is accessed multiple times
            if (previousState.Accessed > 1 &&
                !(type.IsGenericType &&
                    (type.GetGenericTypeDefinition() == typeof(List<>))))
                throw new Exception();

            if (previousState.propInfo != null)
            {
                DXmlElementAttribute elemAttribute = GetElementAttribute(previousState.propInfo, xmlReader.Name);

                if (elemAttribute == null)
                {
                    // It might be another sibling of parent object
                    State parentState = stackStates.ElementAt(1);
                    object actualParentInstanceObj = GetActualInstance(parentState);
                    PropertyInfo[] propInfos = GetAllPropertiesForElement(actualParentInstanceObj.GetType());

                    if (propInfos != null && propInfos.Length > 0)
                    {
                        foreach (PropertyInfo propInfo in propInfos)
                        {
                            elemAttribute = GetElementAttribute(propInfo, xmlReader.Name);
                            if (elemAttribute != null)
                            {
                                CreateAndInitializeProperty(propInfo, actualParentInstanceObj, elemAttribute);

                                previousState.elemPropertyAttribute = elemAttribute;
                                previousState.instanceObj = propInfo.GetValue(actualParentInstanceObj);
                                previousState.instanceType = propInfo.PropertyType;
                                previousState.Accessed++;
                                previousState.propInfo = propInfo;
                            }
                        }
                    }
                    else
                        throw new Exception();
                }

                if (type.IsGenericType &&
                        (type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    previousState.Accessed++;
                    Type[] genericType = type.GetGenericArguments();

                    if (genericType != null && genericType.Length > 0)
                    {
                        object valueObj = null;
                        if (elemAttribute.DerivedType != null)
                            valueObj = Activator.CreateInstance(elemAttribute.DerivedType);
                        else
                            valueObj = Activator.CreateInstance(genericType[0]);

                        if (type.IsGenericType &&
                            (type.GetGenericTypeDefinition() == typeof(List<>)))
                            ((IList)previousState.instanceObj).Add(valueObj);
                    }
                }
            }
        }

        /// <summary>
        /// Fetch and create the next child instance object for the xmlreader node
        /// This method uses the Stack to get the current state
        /// </summary>
        /// <param name="xmlReader"></param>
        private State FetchChildElementInstance(XmlReader xmlReader, State previousState)
        {
            object actualParentInstanceObj = GetActualInstance(previousState);
            PropertyInfo[] propInfos = GetAllPropertiesForElement(actualParentInstanceObj.GetType());

            if (propInfos != null && propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    DXmlElementAttribute elemAttribute = GetElementAttribute(propInfo, xmlReader.Name);
                    if (elemAttribute != null)
                    {
                        CreateAndInitializeProperty(propInfo, actualParentInstanceObj, elemAttribute);

                        return new State()
                        {
                            elemPropertyAttribute = elemAttribute,
                            instanceObj = propInfo.GetValue(actualParentInstanceObj),
                            instanceType = propInfo.PropertyType,
                            Accessed = 1,
                            propInfo = propInfo
                        };
                    }
                }
            }

            throw new Exception();
        }

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

            // If the property is list type create a new instance and add it
            if (propInfo.PropertyType.IsGenericListType())
            {
                Type[] genericType = propInfo.PropertyType.GetGenericArguments();

                if (genericType != null && genericType.Length > 0)
                {
                    object valueObj = Activator.CreateInstance(
                            (elemAttribute.DerivedType != null) ?
                            elemAttribute.DerivedType : genericType[0]);
                    ((IList)value).Add(valueObj);
                }
            }
        }

        /// <summary>
        /// Fetch and parse the attributes of the node and assign the values to the instance object properties
        /// Assumed that the properties are public
        /// </summary>
        private void ParseAttributes()
        {
            State currentState = stackStates.Peek();
            object actualInstanceObj = GetActualInstance(currentState);

            if (xmlReader.HasAttributes)
            {
                PropertyInfo[] propInfos = GetAllPropertiesForAttribute(actualInstanceObj.GetType());

                if (propInfos != null && propInfos.Length > 0)
                {
                    foreach (PropertyInfo propInfo in propInfos)
                    {
                        DXmlAttributeAttribute attrAttribute = propInfo.GetCustomAttribute<DXmlAttributeAttribute>();
                        string value = xmlReader.GetAttribute(attrAttribute.Name);
                        if (value != null)
                            propInfo.SetValue(actualInstanceObj, propInfo.PropertyType.ChangeType(value));
                    }
                }
            }
        }

        /// <summary>
        /// Get a new instance of the current state instance
        /// </summary>
        /// <returns></returns>
        private void GetNewInstance(State currentState)
        {
            if (currentState.instanceType.IsGenericType &&
                (currentState.instanceType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                Type[] genericType = currentState.instanceType.GetGenericArguments();

                if (genericType != null && genericType.Length > 0)
                {
                    object valueObj = null;
                    if (currentState.elemPropertyAttribute.DerivedType != null)
                        valueObj = Activator.CreateInstance(currentState.elemPropertyAttribute.DerivedType);
                    else
                        valueObj = Activator.CreateInstance(genericType[0]);
                    ((IList)currentState.instanceObj).Add(valueObj);
                }
            }
            //else
            //{
            //    if (currentState.elemAttribute.DerivedType != null)
            //        currentState.instanceObj = Activator.CreateInstance(currentState.elemAttribute.DerivedType);
            //    else
            //        currentState.instanceObj = Activator.CreateInstance(currentState.instanceType);
            //}
        }

        /// <summary>
        /// Get the instance object from the current state.
        /// In case of List object it returns the last instance object in the list
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private object GetActualInstance(State currentState)
        {
            Type instanceType = currentState.instanceObj.GetType();
            if (currentState.instanceType.IsGenericType &&
                (currentState.instanceType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                IList listObj = ((IList)currentState.instanceObj);
                return listObj[listObj.Count - 1];
            }
            return currentState.instanceObj;
        }

        #region Helpers

        /// <summary>
        /// Get the <see cref="DXmlElementAttribute"/> object for the xml node
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        private DXmlElementAttribute GetElementAttribute(PropertyInfo propInfo, string nodeName)
        {
            List<DXmlElementAttribute> elemAttributes = propInfo.GetCustomAttributes<DXmlElementAttribute>().ToList();
            return elemAttributes.Where(p => p.Name == nodeName).FirstOrDefault();
        }

        /// <summary>
        /// Get the <see cref="DXmlElementAttribute"/> object for the xml node
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        private DXmlElementAttribute GetElementAttribute(Type type, string nodeName)
        {
            List<DXmlElementAttribute> elemAttributes = type.GetCustomAttributes<DXmlElementAttribute>().ToList();
            return elemAttributes.Where(p => p.Name == nodeName).FirstOrDefault();
        }

        /// <summary>
        /// get an array of all properties for a type which contains <see cref="DXmlElementAttribute"/>
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

        #endregion Helpers
    }
}

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
            public DXmlElementAttribute elemAttribute;
            public Type instanceType;
            public object instanceObj;
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

            // Stack the starting root node
            stackStates.Push(new State()
            {
                instanceType = typeof(T),
                instanceObj = Root
            });

            // Main loop
            while (xmlReader.Read())
                if (xmlReader.IsStartElement())
                {
                    // Signifies child node
                    if (currentDepth < xmlReader.Depth)
                    {
                        // One of the child element of the current instance 
                        // should be initialized
                        FetchChildElementInstance(xmlReader);
                    }
                    // Signifies parent node
                    else if(currentDepth > xmlReader.Depth)
                    {
                        //instanceObj = stackInstanceObj.Pop();
                        //instanceObj = FetchChildElementInstance(instanceObj, xmlReader);
                    }
                    // Signifies sibling node
                    else
                    {
                        FetchElementInstance(xmlReader);
                    }
                    ParseAttributes();
                    currentDepth = xmlReader.Depth;
                }
                else if(xmlReader.NodeType == XmlNodeType.EndElement)
                    stackStates.Pop();
        }

        /// <summary>
        /// Fetch and create the next child instance object for the xmlreader node
        /// This method uses the Stack to get the current state
        /// </summary>
        /// <param name="xmlReader"></param>
        private void FetchChildElementInstance(XmlReader xmlReader)
        {
            // Fetch the current state in the stack
            // This is actually the previous processed state
            State currentState = stackStates.Peek();

            object actualParentInstanceObj = GetActualInstance(currentState);
            PropertyInfo[] propInfos = GetAllPropertiesForElement(actualParentInstanceObj.GetType());

            if (propInfos != null && propInfos.Length > 0)
            {
                foreach (PropertyInfo propInfo in propInfos)
                {
                    DXmlElementAttribute elemAttribute = GetElementAttribute(propInfo, xmlReader.Name);
                    if (elemAttribute != null)
                    {
                        //object value = propInfo.GetValue(actualParentInstanceObj);
                        //if (value == null)
                        //{
                        //    value = Activator.CreateInstance(propInfo.PropertyType);
                        //    propInfo.SetValue(actualParentInstanceObj, value);
                        //}

                        State childState = new State()
                        {
                            elemAttribute = elemAttribute,
                            instanceObj = propInfo.GetValue(actualParentInstanceObj),
                            instanceType = propInfo.PropertyType
                        };

                        GetNewInstance(childState);

                        stackStates.Push(childState);
                        return;
                    }
                }
            }

            throw new Exception();
        }

        /// <summary>
        /// Fetch and process the sibling node
        /// </summary>
        /// <param name="xmlReader"></param>
        public void FetchElementInstance(XmlReader xmlReader)
        {
            State currentState = stackStates.Peek();

            if (!(currentState.instanceObj == null || currentState.instanceObj is IList))
                throw new Exception();
            if (currentState.instanceType == null)
                throw new Exception();

            if(currentState.elemAttribute == null)
            {
                List<DXmlElementAttribute> elemAttributes = 
                    currentState.instanceType.GetCustomAttributes<DXmlElementAttribute>().ToList();
                currentState.elemAttribute = elemAttributes.Where(p => p.Name == xmlReader.Name).FirstOrDefault();
            }

            GetNewInstance(currentState);
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
                if(currentState.instanceObj == null)
                {
                    currentState.instanceObj = Activator.CreateInstance(currentState.instanceType);
                }

                Type[] genericType = currentState.instanceType.GetGenericArguments();

                if (genericType != null && genericType.Length > 0)
                {
                    object valueObj = null;
                    if (currentState.elemAttribute.DerivedType != null)
                        valueObj = Activator.CreateInstance(currentState.elemAttribute.DerivedType);
                    else
                        valueObj = Activator.CreateInstance(genericType[0]);
                    ((IList)currentState.instanceObj).Add(valueObj);
                }
            }
            else
            {
                if (currentState.elemAttribute.DerivedType != null)
                    currentState.instanceObj = Activator.CreateInstance(currentState.elemAttribute.DerivedType);
                else
                    currentState.instanceObj = Activator.CreateInstance(currentState.instanceType);
            }
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

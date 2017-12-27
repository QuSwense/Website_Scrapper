using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using WebReader.Model;
using WebCommon.Extn;
using System.Collections;
using ScrapException;

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
        #region Public Properties

        /// <summary>
        /// The root class object instance.
        /// Access this object after the parsing of the xml file completed
        /// </summary>
        public T Root { get; set; }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// The xml reader object which is used for parsing
        /// </summary>
        private XmlReader xmlReader;
        
        /// <summary>
        /// A stack data structure to maintain a recursive list of states.
        /// We are not using recursion for the parsing logic
        /// </summary>
        private Stack<StateXmlParse> stackStates;

        /// <summary>
        /// The Xml property reader class
        /// </summary>
        private DXmlPropertyReader<T> xmlPropertyReader;

        #endregion Internal Properties

        #region Public Methods

        /// <summary>
        /// The main parsing method to read an xml file.
        /// </summary>
        /// <param name="filePath"></param>
        public void Read(string filePath)
        {
            Init();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            // Create the xml reader object
            xmlReader = XmlReader.Create(filePath, settings);

            if (Root == null) Root = new T();

            // Stack the starting root node
            stackStates.Push(new StateXmlParse()
            {
                InstanceType = typeof(T),
                InstanceObj = Root,
                Accessed = 1
            });

            MainParserLoop(xmlReader.Depth);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Initialize
        /// </summary>
        private void Init()
        {
            stackStates = new Stack<StateXmlParse>();
            xmlPropertyReader = new DXmlPropertyReader<T>(this);
        }

        /// <summary>
        /// This method is the main loop for parsing
        /// </summary>
        /// <param name="currentDepth">This value signifies the child / parent / sibling nodes</param>
        private void MainParserLoop(int currentDepth)
        {
            try
            {
                // Main loop
                while (xmlReader.Read())
                {
                    if (xmlReader.IsStartElement())
                    {
                        ParseXmlElement(currentDepth);
                        ParseAndPopulateAttributes();
                        currentDepth = xmlReader.Depth;
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        stackStates.Pop();
                        currentDepth--;
                    }
                }

                // After the xml file is read into the data structure, now concentrate on the
                // Semantics Attributes
                ProcessSemantics(Root);
            }
            finally
            {
                xmlReader.Close();
                xmlReader = null;
                stackStates.Clear();
                stackStates = null;
            }
        }

        /// <summary>
        /// Parse the current xml element
        /// </summary>
        /// <param name="currentDepth">This value signifies the child / parent / sibling nodes</param>
        private void ParseXmlElement(int currentDepth)
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
        }

        #endregion Private Methods

        #region Xml File Read Helper

        /// <summary>
        /// Fetch and process the sibling node
        /// At this time the current object should be initialized
        /// </summary>
        /// <param name="xmlReader"></param>
        public void FetchElementInstance(XmlReader xmlReader)
        {
            // Fetch the current state in the stack
            // This is actually the previous processed state
            var previousState = stackStates.Peek();

            Type type = previousState.InstanceObj.GetType();

            // Check if the type is not list but is accessed multiple times
            if (previousState.Accessed > 1 &&
                !(type.IsGenericType &&
                    (type.GetGenericTypeDefinition() == typeof(List<>))))
                throw new XmlReaderException(XmlReaderException.EErrorType.NO_LIST_TYPE_MULTIPLE_ELEMENT,
                    xmlReader.Name);

            if (previousState.PropInfo != null)
                FetchElementInstanceFromPreviousState(previousState, type);
        }

        /// <summary>
        /// Fetch the element instance from the previous stack state
        /// </summary>
        /// <param name="previousState"></param>
        /// <param name="type"></param>
        private void FetchElementInstanceFromPreviousState(StateXmlParse previousState, Type type)
        {
            DXmlElementAttribute elemAttribute =
                    xmlPropertyReader.GetElementAttribute(previousState.PropInfo, xmlReader.Name);

            if (elemAttribute == null)
            {
                previousState = FetchChildElementInstance(xmlReader, stackStates.ElementAt(1));
                stackStates.Pop();
                stackStates.Push(previousState);
            }

            if (type.IsGenericListType())
            {
                AddNewListTypeInstance(type, elemAttribute, (IList)previousState.InstanceObj);
                previousState.Accessed++;
            }
        }

        /// <summary>
        /// Fetch and create the next child instance object for the xmlreader node
        /// This method uses the Stack to get the current state
        /// </summary>
        /// <param name="xmlReader"></param>
        /// <param name="previousState"></param>
        private StateXmlParse FetchChildElementInstance(XmlReader xmlReader, StateXmlParse previousState)
        {
            object actualParentInstanceObj = GetActualInstance(previousState);
            var currentState = 
                xmlPropertyReader.FetchChildElementInstance(xmlReader, actualParentInstanceObj);

            if (currentState == null)
                throw new ScrapXmlException(ScrapXmlException.EErrorType.ELEMENTATTRIBUTE_NOT_FOUND,
                    xmlReader.Name);

            return currentState;
        }

        /// <summary>
        /// If the property is list type create a new instance and add it
        /// </summary>
        /// <param name="propInfo"></param>
        /// <param name="elemAttribute"></param>
        /// <param name="listObj"></param>
        /// <returns></returns>
        public void AddNewListTypeInstance(Type type, DXmlElementAttribute elemAttribute, IList listObj)
        {
            Type[] genericType = type.GetGenericArguments();

            if (genericType != null && genericType.Length > 0)
            {
                object valueObj = Activator.CreateInstance(
                        (elemAttribute.DerivedType != null) ?
                        elemAttribute.DerivedType : genericType[0]);
                listObj.Add(valueObj);
            }
        }

        /// <summary>
        /// Fetch and parse the attributes of the node and assign the values to the instance object properties
        /// Assumed that the properties are public
        /// </summary>
        private void ParseAndPopulateAttributes()
        {
            StateXmlParse currentState = stackStates.Peek();
            object actualInstanceObj = GetActualInstance(currentState);

            xmlPropertyReader.ParseAndPopulateAttributes(xmlReader, actualInstanceObj);
        }

        /// <summary>
        /// Get the instance object from the current state.
        /// In case of List object it returns the last instance object in the list
        /// </summary>
        /// <param name="currentState"></param>
        /// <returns></returns>
        private object GetActualInstance(StateXmlParse currentState)
        {
            Type instanceType = currentState.InstanceObj.GetType();
            if (currentState.InstanceType.IsGenericType &&
                (currentState.InstanceType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                var listObj = ((IList)currentState.InstanceObj);
                return listObj[listObj.Count - 1];
            }
            return currentState.InstanceObj;
        }

        #endregion Xml File Read Helper

        #region Semantics

        /// <summary>
        /// Process all the Xml config object
        /// </summary>
        /// <param name="rootObj"></param>
        /// <param name="lastParentObj"></param>
        public void ProcessSemantics(object rootObj, object lastParentObj = null)
        {
            if (rootObj == null) return;
            Type type = rootObj.GetType();

            if (type.IsGenericListType())
                foreach (var item in (IList)rootObj)
                    ProcessSemantics(item, lastParentObj);
            else
                xmlPropertyReader.ProcessSemantics(type, rootObj, lastParentObj);
        }

        #endregion Semantics

        #region Helpers
        
        /// <summary>
        /// Get the <see cref="DXmlElementAttribute"/> object for the xml node
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        private DXmlElementAttribute GetElementAttribute(Type type, string nodeName)
        {
            var elemAttributes = 
                type.GetCustomAttributes<DXmlElementAttribute>().ToList();
            return elemAttributes.Where(p => p.Name == nodeName).FirstOrDefault();
        }

        #endregion Helpers
    }
}

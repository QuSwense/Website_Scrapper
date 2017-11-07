using log4net;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace WebReader.Xml
{
    /// <summary>
    /// A Xml file serialization reader
    /// </summary>
    public class DXmlSerializeReader : IDisposable
    {
        /// <summary>
        /// The private logger
        /// </summary>
        private ILog logger = LogManager.GetLogger(typeof(DXmlSerializeReader));

        /// <summary>
        /// Read a file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T Read<T>(string fileName)
        {
            XmlSerializer configXmlSerializer = new XmlSerializer(typeof(T));
            using (XmlReader reader = XmlReader.Create(fileName))
            {
                XmlDeserializationEvents deserializeEvents = new XmlDeserializationEvents();
                deserializeEvents.OnUnknownAttribute += OnUnknownAttribute;
                deserializeEvents.OnUnknownElement += OnUnknownElement;
                deserializeEvents.OnUnknownNode += OnUnknownNode;
                deserializeEvents.OnUnreferencedObject += OnUnreferencedObject;

                return (T)configXmlSerializer.Deserialize(reader, deserializeEvents);
            }
        }

        /// <summary>
        /// Exception on unreferenced object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            logger.ErrorFormat("An unreferenced object {0} for {1} while xml serialization",
                e.UnreferencedId, e.UnreferencedObject);
        }

        /// <summary>
        /// Exception on unknown node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnknownNode(object sender, XmlNodeEventArgs e)
        {
            logger.ErrorFormat("An unknown node {0} of type {3} found at Line {1}:{2} while xml serialization. Node Text: {4}",
                e.Name, e.LineNumber, e.LinePosition, e.NodeType, e.Text);
        }

        /// <summary>
        /// Exception on unknown element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnknownElement(object sender, XmlElementEventArgs e)
        {
            logger.ErrorFormat("An unknown element {0} found at Line {1}:{2} while xml serialization. Expected elements: {3}",
                e.Element.Name, e.LineNumber, e.LinePosition, e.ExpectedElements);
        }

        /// <summary>
        /// Exception on unknown attributes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            logger.ErrorFormat("An unknown attribute {0} found at Line {1}:{2} while xml serialization. Expected attributes: {3}",
                e.Attr.Name, e.LineNumber, e.LinePosition, e.ExpectedAttributes);
        }

        /// <summary>
        /// A static method to load the xml file into the class type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fullFilePath"></param>
        /// <returns></returns>
        public static T Load<T>(string fullFilePath)
        {
            using (DXmlSerializeReader dXmlSerializeReader = new DXmlSerializeReader())
                return dXmlSerializeReader.Read<T>(fullFilePath);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DXmlReader() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}

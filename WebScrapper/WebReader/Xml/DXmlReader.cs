using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebReader.Xml
{
    public class DXmlReader : IDisposable
    {
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

        private void OnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {

        }

        private void OnUnknownNode(object sender, XmlNodeEventArgs e)
        {

        }

        private void OnUnknownElement(object sender, XmlElementEventArgs e)
        {

        }

        public void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {

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

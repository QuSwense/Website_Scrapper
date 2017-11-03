using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace WebCommon.PathHelp
{
    public class ConfigHelper
    {
        public static T Read<T>(string fileName)
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

        private static void OnUnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {

        }

        private static void OnUnknownNode(object sender, XmlNodeEventArgs e)
        {

        }

        private static void OnUnknownElement(object sender, XmlElementEventArgs e)
        {

        }

        public static void OnUnknownAttribute(object sender, XmlAttributeEventArgs e)
        {

        }
    }
}
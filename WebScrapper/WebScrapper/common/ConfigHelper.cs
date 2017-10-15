using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WebScrapper.common
{
    public class ConfigHelper
    {
        public static T Read<T>(string fileName)
        {
            XmlSerializer configXmlSerializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(fileName))
            {
                reader.ReadToEnd();
                return (T)configXmlSerializer.Deserialize(reader);
            }
        }
    }
}

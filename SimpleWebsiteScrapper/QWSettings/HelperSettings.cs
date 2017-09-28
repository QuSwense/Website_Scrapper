using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QWSettings
{
    /// <summary>
    /// The abstract class for all settings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HelperSettings<T> where T : class
    {
        /// <summary>
        /// Refers to the settings class
        /// </summary>
        internal static T SettingsObj { get; set; }

        /// <summary>
        /// Static constructor
        /// </summary>
        static HelperSettings()
        {
            SettingsObj = Read();
        }

        /// <summary>
        /// Read the settings xml config file
        /// </summary>
        /// <returns></returns>
        protected static T Read()
        {
            T result = null;
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StreamReader(string.Format("{0}.xml", typeof(T).Name)))
            {
                using (var reader = XmlReader.Create(stream))
                    result = (T)serializer.Deserialize(reader);
            }

            return result;
        }
    }
}

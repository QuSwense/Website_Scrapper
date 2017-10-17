using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace WebScrapper.Config
{
    public class ConfigHelper
    {
        public static T Read<T>(string fileName)
        {
            XmlSerializer configXmlSerializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(fileName))
            {
                return (T)configXmlSerializer.Deserialize(reader);
            }
        }

        public static string GetAppConfigPath(string appTopic)
        {
            return Path.Combine("App", appTopic, appTopic + "Config.xml");
        }

        public static string GetDbTableEnumConfigPath(string appTopic)
        {
            return Path.Combine("App", appTopic, "dbscripts", "table_enum.csv");
        }

        public static string GetDbTableMetadataConfigPath(string appTopic)
        {
            return Path.Combine("App", appTopic, "dbscripts", "table_metadata.csv");
        }

        public static string GetDbTableColumnsConfigPath(string appTopic)
        {
            return Path.Combine("App", appTopic, "dbscripts", "table_columns.csv");
        }

        public static string GetScrapConfigPath(string appTopic)
        {
            return Path.Combine("App", appTopic, "Scrap", appTopic + "Scrap.xml");
        }
    }
}

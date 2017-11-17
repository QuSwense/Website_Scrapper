using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The class which contains the application configurations used by the application
    /// This class reads the application config xml file.
    /// It also contains the properties or methods for reading specific properties.
    /// </summary>
    [Serializable]
    [XmlRoot("AppConfig")]
    public class ApplicationConfig
    {
        /// <summary>
        /// The list of application keys
        /// </summary>
        [XmlElement("Key")]
        public ApplicationConfigKey[] Keys { get; set; }

        /// <summary>
        /// Reads the value of a key from the configuration file
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Key(string name)
        {
            if (Keys == null || Keys.Count() <= 0) return null;
            IEnumerable<ApplicationConfigKey> appConfigKeys = Keys.Where(item => item.Name == name);
            if (appConfigKeys == null || appConfigKeys.Count() <= 0) return null;
            return appConfigKeys.First().Value;
        }

        /// <summary>
        /// Get the database type
        /// </summary>
        /// <returns></returns>
        public string Db() => Key("db");

        /// <summary>
        /// Get the boolean check if the database needs to be created
        /// </summary>
        /// <returns></returns>
        public bool DoCreateDb() => Convert.ToBoolean(Key("dodbcreate"));

        /// <summary>
        /// Maximum Scrap level
        /// </summary>
        /// <returns></returns>
        public int ScrapMaxLevel()
        {
            string keyValue = Key("ScrapMaxLevel");
            if (string.IsNullOrEmpty(keyValue)) return 0;
            else return Convert.ToInt32(keyValue);
        }

        /// <summary>
        /// Method to clone this object and return a new object
        /// </summary>
        /// <returns></returns>
        public ApplicationConfig Clone()
        {
            ApplicationConfig newConfig = new ApplicationConfig();

            if(Keys != null)
                newConfig.Keys = Keys.Select(k => k.Clone()).ToArray();

            return newConfig;
        }

        /// <summary>
        /// When you read multiple application configuration files and wants to merge them.
        /// This method merges the current config file. It overwrites the current object values by
        /// the passed value if same key is present.
        /// </summary>
        /// <param name="configToOverwrite"></param>
        /// <returns></returns>
        public ApplicationConfig Union(ApplicationConfig configToOverwrite)
        {
            ApplicationConfig result = new ApplicationConfig();

            if (Keys != null && configToOverwrite.Keys == null)
                result.Keys = Keys.Select(k => new ApplicationConfigKey(k.Name, k.Value)).ToArray();
            else if (Keys == null && configToOverwrite.Keys != null)
                result.Keys = configToOverwrite.Keys.Select(k => new ApplicationConfigKey(k.Name, k.Value)).ToArray();
            else
            {
                List<ApplicationConfigKey> resultkeys = new List<ApplicationConfigKey>();
                foreach (var item in Keys)
                {
                    ApplicationConfigKey found = configToOverwrite.Keys.Where(k => k.Name == item.Name).FirstOrDefault();
                    if (found == null)
                        // This is a new key
                        resultkeys.Add(item.Clone());
                    else
                    {
                        // Overwrite the key
                        ApplicationConfigKey newClone = item.Clone();
                        newClone.Name = item.Name;
                        newClone.Value = found.Value;
                        resultkeys.Add(newClone);
                    }
                }
                result.Keys = resultkeys.ToArray();
            }

            return result;
        }
    }
}

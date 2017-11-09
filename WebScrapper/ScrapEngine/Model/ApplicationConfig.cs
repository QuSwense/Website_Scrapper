using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    [XmlRoot("AppConfig")]
    public class ApplicationConfig
    {
        [XmlElement("Key")]
        public ApplicationConfigKey[] Keys { get; set; }

        public string Key(string name)
        {
            return Keys.Where(item => item.Name == name).First().Value;
        }

        public string Db()
        {
            return Key("db");
        }

        public bool DoCreateDb()
        {
            return Convert.ToBoolean(Key("dodbcreate"));
        }

        public ApplicationConfig Clone()
        {
            ApplicationConfig newConfig = new ApplicationConfig();

            if(Keys != null)
                newConfig.Keys = Keys.Select(k => k.Clone()).ToArray();

            return newConfig;
        }

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
                        resultkeys.Add(item.Clone());
                    else
                    {
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

using System;
using System.Xml.Serialization;

namespace ScrapEngine.Model
{
    [Serializable]
    public class ApplicationConfigKey
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("value")]
        public string Value { get; set; }

        public ApplicationConfigKey() { }

        public ApplicationConfigKey(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public ApplicationConfigKey Clone()
        {
            return new ApplicationConfigKey(Name, Value);
        }
    }
}

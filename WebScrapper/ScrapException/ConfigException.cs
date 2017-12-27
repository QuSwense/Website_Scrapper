using System;

namespace ScrapException
{
    /// <summary>
    /// A class for any config exception
    /// </summary>
    public class ConfigException : Exception
    {
        public ConfigException() { }
        public ConfigException(string message) : base(message) { }
    }
}

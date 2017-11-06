using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Error
{
    public class AppTopicConfigException : ConfigException
    {
        public AppTopicConfigException() { }
        public AppTopicConfigException(string message) : base(message) { }
    }
}

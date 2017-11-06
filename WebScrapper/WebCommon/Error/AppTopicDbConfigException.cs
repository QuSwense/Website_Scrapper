using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Error
{
    public class AppTopicDbConfigException : AppTopicConfigException
    {
        public AppTopicDbConfigException() { }
        public AppTopicDbConfigException(string message) : base(message) { }
    }
}

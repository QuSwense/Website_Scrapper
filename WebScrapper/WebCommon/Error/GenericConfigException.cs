using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebCommon.Error
{
    public class GenericConfigException : ConfigException
    {
        public GenericConfigException() { }
        public GenericConfigException(string message) : base(message) { }
    }
}

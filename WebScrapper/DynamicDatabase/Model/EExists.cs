using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Model
{
    /// <summary>
    /// An enumerable which sets the check for existense of a context
    /// </summary>
    public enum EExists
    {
        NONE = 0,
        IN_MEMORY = 1,
        IN_DB = 2
    }
}

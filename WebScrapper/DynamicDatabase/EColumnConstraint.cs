using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    public enum EColumnConstraint
    {
        NONE = 1,
        NOTNULL = 2,
        UNQIUE = 4,
        PRIMARYKEY = 8,
        DEFAULT = 16
    }
}

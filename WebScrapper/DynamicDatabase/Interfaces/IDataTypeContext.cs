using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    public interface IDataTypeContext
    {
        string GetDataType(Type type);
        DbDataType ParseDataType(string typeName);
    }
}

using DynamicDatabase.Types;
using System;

namespace DynamicDatabase.Interfaces
{
    public interface IDataTypeContext
    {
        string GetDataType(Type type);
        DbDataType ParseDataType(string typeName);
    }
}

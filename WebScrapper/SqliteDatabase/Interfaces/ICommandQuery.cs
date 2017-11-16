using System.Collections.Generic;

namespace SqliteDatabase.Interfaces
{
    /// <summary>
    /// A common interface used by all the command interface classes
    /// </summary>
    public interface ICommandQuery
    {
        /// <summary>
        /// The SQL statement
        /// </summary>
        List<string> SQLs { get; }
    }
}

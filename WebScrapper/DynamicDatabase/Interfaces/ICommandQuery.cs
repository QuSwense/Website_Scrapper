using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
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

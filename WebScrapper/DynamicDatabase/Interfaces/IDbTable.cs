using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Interfaces
{
    /// <summary>
    /// An interface to the <see cref="DynamicTable{TDynRow, TDynColMetadata}"/> class.
    /// It is used as a way to call the methods of the class mostly for back reference.
    /// </summary>
    public interface IDbTable
    {
        /// <summary>
        /// Get index of a column
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        int GetColumnIndex(string name);

        /// <summary>
        /// get the list of Primary keys column names
        /// </summary>
        /// <returns></returns>
        List<string> GetPKNames();
    }
}

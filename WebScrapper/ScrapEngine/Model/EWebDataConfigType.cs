using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model
{
    /// <summary>
    /// The type of scrap node
    /// </summary>
    public enum EWebDataConfigType
    {
        /// <summary>
        /// The scrap node points to a Html table
        /// </summary>
        TABLE,

        /// <summary>
        /// The scrap node points to a csv file
        /// </summary>
        CSV
    }
}

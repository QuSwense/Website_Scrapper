using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapEngine.Interfaces
{
    /// <summary>
    /// A common interface which contains identity informations to identify the class
    /// </summary>
    public interface IIdentity
    {
        /// <summary>
        /// A name to display
        /// </summary>
        string Name { get; }
    }
}

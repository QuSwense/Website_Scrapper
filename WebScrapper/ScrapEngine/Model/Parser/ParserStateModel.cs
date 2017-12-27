using ScrapEngine.Bl.Parser;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapEngine.Model.Parser
{
    /// <summary>
    /// Parser state model which is an abstract base class model for storing
    /// states
    /// </summary>
    public abstract class ParserStateModel
    {
        /// <summary>
        /// The config element
        /// </summary>
        public IConfigElement Config { get; set; }

        /// <summary>
        /// The sibling index
        /// </summary>
        public int SiblingIndex { get; set; }

        /// <summary>
        /// The parent state model
        /// </summary>
        public ParserStateModel Parent { get; set; }
    }
}

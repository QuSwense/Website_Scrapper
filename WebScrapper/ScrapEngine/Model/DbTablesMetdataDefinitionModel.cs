using DynamicDatabase.Config;
using ScrapEngine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScrapEngine.Model
{
    /// <summary>
    /// This model resembles the structure of the csv file which contains the table definitions model
    /// </summary>
    public class DbTablesMetdataDefinitionModel : Dictionary<string, ConfigDbTable>, IIdentity
    {
        /// <summary>
        /// A name to display
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DbTablesMetdataDefinitionModel() { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        public DbTablesMetdataDefinitionModel(string name)
        {
            Name = name;
        }
    }
}

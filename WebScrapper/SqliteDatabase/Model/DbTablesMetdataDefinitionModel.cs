using SqliteDatabase.Interfaces;
using System.Collections.Generic;

namespace SqliteDatabase.Model
{
    /// <summary>
    /// This model resembles the structure of the csv file which contains the table definitions model
    /// </summary>
    public class DbTablesMetdataDefinitionModel : Dictionary<string, ConfigDbTable>, IPrimaryIdentity
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

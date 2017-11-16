using SqliteDatabase.Interfaces;
using System.Collections.Generic;

namespace SqliteDatabase.Model
{
    /// <summary>
    /// This maps a csv config file whcih contains dynamic database table definitions
    /// to create multiple tables at runtime
    /// </summary>
    public class DbTablesDefinitionModel : Dictionary<string, DbColumnsDefinitionModel>, IPrimaryIdentity
    {
        /// <summary>
        /// A name to display
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DbTablesDefinitionModel() { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        public DbTablesDefinitionModel(string name)
        {
            Name = name;
        }
    }
}

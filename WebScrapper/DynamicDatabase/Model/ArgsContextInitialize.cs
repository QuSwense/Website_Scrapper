using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicDatabase.Model
{
    /// <summary>
    /// The class which contains the arguments for initializing a database context
    /// </summary>
    public class ArgsContextInitialize
    {
        /// <summary>
        /// The path to the database file
        /// </summary>
        public string DbFilePath { get; set; }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The conenction string
        /// </summary>
        public string ConnectionString { get; set; }
    }
}

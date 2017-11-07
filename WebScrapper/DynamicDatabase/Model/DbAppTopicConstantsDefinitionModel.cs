using DynamicDatabase.Interfaces;
using System.Collections.Generic;

namespace DynamicDatabase.Model
{
    /// <summary>
    /// An application topic can use several constants within itself.
    /// This model resembles the structure of the csv file
    /// </summary>
    public class DbAppTopicConstantsDefinitionModel : Dictionary<string, Dictionary<int, string>>, IPrimaryIdentity
    {
        /// <summary>
        /// A name to display
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DbAppTopicConstantsDefinitionModel() { }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="name"></param>
        public DbAppTopicConstantsDefinitionModel(string name)
        {
            Name = name;
        }
    }
}
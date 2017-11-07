namespace DynamicDatabase.Types
{
    /// <summary>
    /// A class which represents a numeric data type
    /// </summary>
    public class DbIntDataType : DbDataType
    {
        /// <summary>
        /// The size of the numeric data
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DbIntDataType() { }

        /// <summary>
        /// Constructor with size
        /// </summary>
        /// <param name="count"></param>
        public DbIntDataType(int count)
        {
            this.Count = count;
        }

        /// <summary>
        /// Parameterized data types
        /// </summary>
        /// <param name="value"></param>
        public DbIntDataType(object value) : base(value) { }
    }
}

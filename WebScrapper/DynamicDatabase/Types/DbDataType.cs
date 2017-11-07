namespace DynamicDatabase.Types
{
    /// <summary>
    /// The abstract data type class. This class is the base class for any database data type
    /// class.
    /// </summary>
    public abstract class DbDataType
    {
        /// <summary>
        /// The actual data for the data type
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Default
        /// </summary>
        public DbDataType() { }

        /// <summary>
        /// Parameterized data types
        /// </summary>
        /// <param name="value"></param>
        public DbDataType(object value)
        {
            Value = value;
        }
    }
}

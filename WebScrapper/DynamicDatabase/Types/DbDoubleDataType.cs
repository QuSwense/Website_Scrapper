namespace DynamicDatabase.Types
{
    /// <summary>
    /// A class which represents a doublw oer real data type.
    /// </summary>
    public class DbDoubleDataType : DbDataType
    {
        /// <summary>
        /// The size of the data
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// The count of numbers after decimal places
        /// </summary>
        public int CountAfterDecimal { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DbDoubleDataType() { }

        /// <summary>
        /// Constructor with size and precision
        /// </summary>
        /// <param name="count"></param>
        /// <param name="afterDecimal"></param>
        public DbDoubleDataType(int count, int afterDecimal)
        {
            Count = count;
            CountAfterDecimal = afterDecimal;
        }
    }
}

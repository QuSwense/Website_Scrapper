namespace DynamicDatabase.Types
{
    /// <summary>
    /// Char data type class. This class represents any text data type.
    /// </summary>
    public class DbCharDataType : DbDataType
    {
        /// <summary>
        /// The total number characters
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Default
        /// </summary>
        public DbCharDataType() { }

        /// <summary>
        /// Constructor with count
        /// </summary>
        /// <param name="count"></param>
        public DbCharDataType(int count)
        {
            Count = count;
        }

        /// <summary>
        /// Copy the data from the parameter
        /// </summary>
        /// <param name="dataType"></param>
        public override void CopyFrom(DbDataType dataType)
        {
            base.CopyFrom(dataType);
            DbCharDataType dataTypeActual = dataType as DbCharDataType;
            if (dataTypeActual == null) return;
            Count = dataTypeActual.Count;
        }
    }
}

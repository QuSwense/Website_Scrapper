namespace DynamicDatabase.Types
{
    /// <summary>
    /// The class which represents the DateTime datatype of a database
    /// </summary>
    public class DbDateTimeDataType : DbDataType
    {
        /// <summary>
        /// The format of the DateTime as stored in the database
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public DbDateTimeDataType() { }

        /// <summary>
        /// Constructor with format
        /// </summary>
        /// <param name="format"></param>
        public DbDateTimeDataType(string format)
        {
            Format = format;
        }

        /// <summary>
        /// Copy the data from the parameter
        /// </summary>
        /// <param name="dataType"></param>
        public override void CopyFrom(DbDataType dataType)
        {
            base.CopyFrom(dataType);
            DbDateTimeDataType dataTypeActual = dataType as DbDateTimeDataType;
            if (dataTypeActual == null) return;
            Format = dataTypeActual.Format;
        }
    }
}

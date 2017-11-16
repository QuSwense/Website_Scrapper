namespace SqliteDatabase.Model
{
    /// <summary>
    /// The metdata table model for storing Column data extarct information
    /// </summary>
    public class DynamicTableDataInsertModel
    {
        /// <summary>
        /// The name of the column
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A check if this is a primary key of the table.
        /// This is also used to identify the unique column when doing insert / update
        /// </summary>
        public bool IsPk { get; set; }

        /// <summary>
        /// The value to be set for the column
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// The index for the row in the loop
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// The reference url from where the data is extracted
        /// </summary>
        public EConfigDbDataType DataType { get; set; }
        
    }
}

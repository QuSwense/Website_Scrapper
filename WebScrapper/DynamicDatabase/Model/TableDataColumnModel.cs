namespace DynamicDatabase.Model
{
    /// <summary>
    /// The metdata table model for storing Column data extarct information
    /// </summary>
    public class TableDataColumnModel
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
        public string Url { get; set; }

        /// <summary>
        /// The reference xpath from where the data is extracted
        /// </summary>
        public string XPath { get; set; }
    }
}

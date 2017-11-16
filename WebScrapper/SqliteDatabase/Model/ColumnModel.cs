namespace SqliteDatabase.Model
{
    public class ColumnModel
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public EConfigDbDataType DataType { get; set; }
    }
}

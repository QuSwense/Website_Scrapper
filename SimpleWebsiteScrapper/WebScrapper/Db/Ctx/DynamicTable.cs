using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using WebScrapper.Db.Config;

namespace WebScrapper.Db.Ctx
{
    /// <summary>
    /// Represents a Database table.
    /// </summary>
    /// <typeparam name="TDynRow"></typeparam>
    /// <typeparam name="TDynColMetadata"></typeparam>
    /// <typeparam name="TColDbConfig"></typeparam>
    /// <typeparam name="TDynCol"></typeparam>
    public class DynamicTable<TDynRow, TDynColMetadata, TColDbConfig, 
        TDynCol>
        where TDynRow : DynamicRow, new()
        where TDynColMetadata : DynamicColumnMetadata, new()
        where TColDbConfig : ColumnDbConfig
        where TDynCol : DynamicColumn<TDynColMetadata>, new()
    {
        /// <summary>
        /// The name of the table
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// The rows in the table. The data key is RowId.
        /// </summary>
        public Dictionary<string, TDynRow> Rows { get; protected set; }

        /// <summary>
        /// The list of column headers
        /// </summary>
        public Dictionary<string, TDynColMetadata> Headers { get; protected set; }

        /// <summary>
        /// Constructor default
        /// </summary>
        public DynamicTable() { }

        /// <summary>
        /// Constructor with table name
        /// </summary>
        /// <param name="tablename"></param>
        public DynamicTable(string tablename)
        {
            TableName = tablename;
        }

        /// <summary>
        /// Add header to the table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dynCol"></param>
        public void AddHeader(string key, TDynColMetadata dynCol)
        {
            if (Headers == null) Headers = new Dictionary<string, TDynColMetadata>();
            Headers.Add(key, dynCol);
        }

        /// <summary>
        /// Loop through the column configuration and create a new table
        /// </summary>
        /// <param name="configCols"></param>
        public void CreateTable(Dictionary<string, TColDbConfig> configCols)
        {
            foreach (var item in configCols)
            {
                TDynColMetadata colMetadata = new TDynColMetadata();
                colMetadata.Parse(item.Key, item.Value);
                AddHeader(item.Key, colMetadata);
            }
        }

        /// <summary>
        /// Create table from property type
        /// </summary>
        /// <param name="classProperties"></param>
        public void CreateTable(PropertyInfo[] classProperties)
        {
            foreach (PropertyInfo prop in classProperties)
            {
                TDynColMetadata colMetadata = new TDynColMetadata();
                colMetadata.Parse(prop);
                AddHeader(colMetadata.ColumnName, colMetadata);
            }
        }

        /// <summary>
        /// Load table metadata
        /// </summary>
        /// <param name="reader"></param>
        public void LoadTableMetadata(DbDataReader reader)
        {
            Headers = new Dictionary<string, TDynColMetadata>();

            while (reader.Read())
            {
                TDynColMetadata colMetadata = new TDynColMetadata();
                colMetadata.Parse(reader);
                Headers.Add(colMetadata.ColumnName, colMetadata);
            }
        }

        public void LoadData(DbDataReader reader)
        {

        }
    }
}

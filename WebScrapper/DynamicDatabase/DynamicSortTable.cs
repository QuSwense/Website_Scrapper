using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace DynamicDatabase
{
    public class DynamicSortTable : DynamicTable
    {
        /// <summary>
        /// Sort the rows by unique keys
        /// </summary>
        public Dictionary<string, IDbRow> SortRows { get; protected set; }

        /// <summary>
        /// The list of column names by which the rows are sorted
        /// </summary>
        public string[] ColumnSorter { get; protected set; }

        /// <summary>
        /// Load data in memory by Rowid
        /// </summary>
        /// <param name="reader"></param>
        public override void LoadData(DbDataReader reader, params string [] args)
        {
            if (Headers == null) throw new Exception("Table metadata must be loaded before data");

            Rows = new List<IDbRow>();

            if (args == null || args.Length <= 0) throw new Exception("No unique rows provided");

            ColumnSorter = args;
            SortRows = new Dictionary<string, IDbRow>();

            while (reader.Read())
            {
                var row = DynamicDbFactory.Create<IDbRow>();
                var pkList = new List<string>();
                row.Initialize(this);

                foreach (var item in Headers)
                {
                    row.AddorUpdate(item.ColumnName, reader.GetValue(item.Index));
                    if (args.Contains(item.ColumnName)) pkList.Add(row.Columns[item.ColumnName].ToString());
                }
                Rows.Add(row);
                SortRows.Add(string.Join(",", pkList), row);
            }
        }

        /// <summary>
        /// Add row of data
        /// </summary>
        /// <param name="row"></param>
        public override void AddorUpdate(List<string> pks, List<string> dataList)
        {
            string pkdata = string.Join(",", pks);
            IDbRow row = null;

            if (!SortRows.ContainsKey(pkdata))
            {
                row = DynamicDbFactory.Create<IDbRow>();
                row.Initialize(this);
                SortRows.Add(pkdata, row);
            }
            else
            {
                row = SortRows[pkdata];
            }

            row.AddorUpdate(dataList);
        }
    }
}

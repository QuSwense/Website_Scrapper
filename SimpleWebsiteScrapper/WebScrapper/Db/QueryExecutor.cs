using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using WebScrapper.Web;

namespace WebScrapper.Db
{
    public class QueryExecutor
    {
        private DbGeneratorBL generatorBl;
        private QueryGenerator queryGenerator;
        private List<TableDataColumnModel> rowData;

        public QueryExecutor(DbGeneratorBL generatorBl)
        {
            queryGenerator = new QueryGenerator();
            this.generatorBl = generatorBl;
        }

        public void SetTable(string name)
        {
            queryGenerator.Table = name;
        }

        public void SetRow(List<TableDataColumnModel> rowData)
        {
            this.rowData = rowData;
        }

        public void SaveOrUpdate()
        {
            if (IsRowExists() == 1)
                CreateUpdateDDL();
            else
                CreateInsertDDL();
            generatorBl.ExecuteDDL(queryGenerator.ToString());
        }

        private void CreateInsertDDL()
        {
            foreach (TableDataColumnModel col in rowData)
            {
                if (col.IsPk && string.IsNullOrEmpty(col.Value))
                {
                    queryGenerator.Invalidate();
                    return;
                }
                queryGenerator.Set(col.Name);
                queryGenerator.Filter(generatorBl.GetDataType(queryGenerator.Table, col.Name), col.Value);
            }
        }

        private void CreateUpdateDDL()
        {
            foreach (TableDataColumnModel col in rowData)
            {
                if (col.IsPk)
                {
                    if (string.IsNullOrEmpty(col.Value))
                    {
                        queryGenerator.Invalidate();
                        return;
                    }
                    queryGenerator.Filter(col.Name, generatorBl.GetDataType(queryGenerator.Table, col.Name), col.Value);
                }
                else
                {
                    queryGenerator.Set(col.Name, generatorBl.GetDataType(queryGenerator.Table, col.Name), col.Value);
                }
            }
        }

        private int IsRowExists()
        {
            foreach (TableDataColumnModel col in rowData)
            {
                if (col.IsPk)
                {
                    if (string.IsNullOrEmpty(col.Value)) return 0;
                    queryGenerator.Filter(col.Name, generatorBl.GetDataType(queryGenerator.Table, col.Name), col.Value);
                }
            }

            DbDataReader r = generatorBl.ExecuteDML(queryGenerator.ToString());
            int count = 0;
            if (r.Read()) count = r.GetInt32(0);

            return count;
        }
    }
}

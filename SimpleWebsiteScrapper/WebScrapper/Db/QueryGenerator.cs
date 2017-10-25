using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebScrapper.Db.Config;

namespace WebScrapper.Db
{
    public class QueryGenerator
    {
        public string Table { get; set; }
        protected List<string> setClauses;
        protected List<string> filterClauses;
        protected List<string> primaryKeys;
        protected EQueryMode queryMode;

        public QueryGenerator()
        {
            setClauses = new List<string>();
            filterClauses = new List<string>();
            primaryKeys = new List<string>();
        }

        public void Invalidate()
        {
            queryMode = EQueryMode.NONE;
        }

        public QueryGenerator CreateTable(string name)
        {
            queryMode = EQueryMode.CREATE_TABLE;
            Table = name;
            return this;
        }

        public QueryGenerator SelectCount(string name)
        {
            queryMode = EQueryMode.SELECT_COUNT;
            Table = name;
            return this;
        }

        public QueryGenerator Update(string name)
        {
            queryMode = EQueryMode.UPDATE;
            Table = name;
            return this;
        }

        public QueryGenerator Insert(string name)
        {
            queryMode = EQueryMode.INSERT;
            Table = name;
            return this;
        }

        public QueryGenerator Column(string name, string dataType, bool isUnqiue, bool isnnull, bool ispk)
        {
            filterClauses.Add(string.Format("{0} {1} {2} {3} ", name, dataType,
                isUnqiue ? "UNIQUE" : "", isnnull? "NOT NULL": ""));
            if(ispk) primaryKeys.Add(name);
            return this;
        }

        public QueryGenerator Filter(string name, EDataTypeDbConfig dataType, string value)
        {
            if(string.IsNullOrEmpty(value))
                filterClauses.Add(string.Format("{0} IS NULL ", name));
            else
                filterClauses.Add(string.Format("{0} = {1} ", name, GetValue(dataType, value)));
            return this;
        }

        public QueryGenerator Set(string name, EDataTypeDbConfig dataType, string value)
        {
            if (string.IsNullOrEmpty(value))
                setClauses.Add(string.Format("{0} IS NULL ", name));
            else
                setClauses.Add(string.Format("{0} = {1} ", name, GetValue(dataType, value)));
            return this;
        }

        public QueryGenerator Set(string name)
        {
            setClauses.Add(name);
            return this;
        }

        public QueryGenerator Filter(EDataTypeDbConfig dataType, string value)
        {
            filterClauses.Add(GetValue(dataType, value));
            return this;
        }

        public QueryGenerator Filter(Type dataType, string value)
        {
            filterClauses.Add(GetValue(dataType, value));
            return this;
        }

        private string GetValue(EDataTypeDbConfig dataType, string value)
        {
            string normvalue = value;

            // if single quote present then escape it
            normvalue = normvalue.Replace("'", "''");
            if (string.IsNullOrEmpty(value))
                normvalue = "null";
            else
                normvalue =(dataType == EDataTypeDbConfig.STRING) ? "'" + normvalue + "'" : normvalue;
            return normvalue;
        }

        private string GetValue(Type dataType, string value)
        {
            string normvalue = value;

            // if single quote present then escape it
            normvalue = normvalue.Replace("'", "''");
            if (string.IsNullOrEmpty(value))
                normvalue = "null";
            else
                normvalue = (dataType == typeof(string)) ? "'" + normvalue + "'" : normvalue;
            return normvalue;
        }

        public override string ToString()
        {
            string sql = "";
            if (EQueryMode.CREATE_TABLE == queryMode)
            {
                sql = string.Format("CREATE TABLE {0} ( {1} {2} )", Table, string.Join(",", filterClauses),
                    ((primaryKeys.Count > 0) ? string.Format(", PRIMARY KEY ({0})", string.Join(",", primaryKeys)) : "")
                    );
            }
            else if (EQueryMode.SELECT_COUNT == queryMode)
            {
                sql = string.Format("SELECT COUNT(*) FROM {0} WHERE {1}", Table, string.Join(",", filterClauses));
            }
            else if (EQueryMode.UPDATE == queryMode)
            {
                sql = string.Format("UPDATE {0} SET {1} WHERE {2}", Table, string.Join(",", setClauses), string.Join(",", filterClauses));
            }
            else if (EQueryMode.INSERT == queryMode)
            {
                sql = string.Format("INSERT INTO {0} ({1}) VALUES ({2})", Table, string.Join(",", setClauses), string.Join(",", filterClauses));
            }
            else
                return null;

            return sql;
        }
    }

    public enum EQueryMode
    {
        CREATE_TABLE,
        SELECT_COUNT,
        INSERT,
        UPDATE,
        NONE
    }
}

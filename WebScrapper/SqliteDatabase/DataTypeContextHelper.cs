using SqliteDatabase.Model;
using System;

namespace SqliteDatabase
{
    public class DataTypeContextHelper
    {
        public static string GetQueryFormat(string value, EConfigDbDataType dType)
        {
            if (string.IsNullOrEmpty(value))
                return "NULL";
            if (dType == EConfigDbDataType.BOOLEAN ||
                dType == EConfigDbDataType.DECIMAL ||
                dType == EConfigDbDataType.ENUM ||
                dType == EConfigDbDataType.NUMBER)
                return value;
            else
                return "'" + value + "'";
        }

        public static string GetQueryFormat(object value, EConfigDbDataType dType)
        {
            if (value == null || value == DBNull.Value || string.IsNullOrEmpty(value.ToString())) return "NULL";

            if (dType == EConfigDbDataType.BOOLEAN ||
                dType == EConfigDbDataType.DECIMAL ||
                dType == EConfigDbDataType.ENUM ||
                dType == EConfigDbDataType.NUMBER)
                return value.ToString();
            else
                return "'" + value.ToString() + "'";
        }

        public static EConfigDbDataType GetType(Type type)
        {
            if (type == typeof(string) || type == typeof(char))
                return EConfigDbDataType.STRING;
            else if (type == typeof(bool))
                return EConfigDbDataType.BOOLEAN;
            else if (type == typeof(DateTime))
                return EConfigDbDataType.DATETIME;
            else if (type == typeof(double) || type == typeof(float) || type == typeof(decimal))
                return EConfigDbDataType.DECIMAL;
            else if (type == typeof(int) || type == typeof(short) || type == typeof(long))
                return EConfigDbDataType.NUMBER;
            else
                return EConfigDbDataType.STRING;
        }

        public static string NormalizeValue(string value)
        {
            string normalizedValue = value;
            if (string.IsNullOrEmpty(value)) return normalizedValue;
            normalizedValue = normalizedValue.Replace("'", "''");

            return normalizedValue;
        }

        public static object NormalizeValue(object value)
        {
            if (value == null || value == DBNull.Value) return value;
            else if (value.GetType() == typeof(string))
                return value.ToString().Replace("'", "''");
            else return value;
        }

        public static string GetSqliteType(EConfigDbDataType type)
        {
            if (type == EConfigDbDataType.STRING) return "TEXT";
            else if (type == EConfigDbDataType.NUMBER) return "INTEGER";
            else if (type == EConfigDbDataType.DECIMAL) return "REAL";
            else return "TEXT";
        }
    }
}

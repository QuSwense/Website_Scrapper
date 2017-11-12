using DynamicDatabase;
using System.Data.Common;

namespace WebScrapper.Db.Ctx.sqlite
{
    /// <summary>
    /// The class which represents metdata about a Sqlite table column
    /// </summary>
    public class SqliteColumnMetadata : DynamicColumnMetadata
    {
        /// <summary>
        /// Parse the database reader.
        /// It is assumed that all Database Meta column info query will follow the same format
        /// Column 2 - name
        /// Column 3 - Data Type
        /// Column Rest - Constraints
        /// If not inherit a new class and override
        /// </summary>
        /// <param name="reader"></param>
        public override void Parse(DbDataReader reader)
        {
            ColumnName = reader.GetString(1);
            DataType = Table.DbContext.DbDataType.ParseDataType(reader.GetString(2));
            Constraint = ParseConstraint(reader);
        }

        /// <summary>
        /// Parses a sqlite metdata information collected from the PRAGMA query
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override EColumnConstraint ParseConstraint(DbDataReader reader)
        {
            if (reader.GetInt32(3) == 1) Constraint |= EColumnConstraint.NOTNULL;
            if (reader.GetValue(4) != null)
            {
                Constraint |= EColumnConstraint.DEFAULT;
                Default = reader.GetValue(4);
            }
            if (reader.GetInt32(5) == 1) Constraint |= EColumnConstraint.PRIMARYKEY;

            return Constraint;
        }
    }
}

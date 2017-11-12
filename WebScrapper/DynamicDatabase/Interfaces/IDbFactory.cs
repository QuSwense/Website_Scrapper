using DynamicDatabase.Config;
using DynamicDatabase.Model;
using System.Collections.Generic;
using System.Data.Common;

namespace DynamicDatabase.Interfaces
{
    public interface IDbFactory
    {
        /// <summary>
        /// Register types for sqlite
        /// </summary>
        void RegisterSqlite();

        /// <summary>
        /// Register types
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        void Register<TContract, TImplementation>() where TImplementation : TContract;

        /// <summary>
        /// Create a new instance of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>();

        /// <summary>
        /// Create and initialize a new Database table
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        IDbTable CreateDbTable(IDbContext dbContext, string tableName);

        /// <summary>
        /// Create and initialize a new Column metadata
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        IColumnMetadata CreateColumnMetadata(IDbTable table);

        /// <summary>
        /// Create and initialize a new Column metadata
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        IColumnHeaders CreateColumnHeaders(IDbTable tableParent, DbDataReader reader,
            Dictionary<string, ColumnLoadDataModel> columns);

        /// <summary>
        /// Create and initialize a new Column metadata
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        IColumnHeaders CreateColumnHeaders(IDbTable tableParent, DbColumnsDefinitionModel configCols);

        /// <summary>
        /// Create and initialize a new Column metadata
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        IColumnHeaders CreateColumnHeaders(IDbTable table, DbDataReader reader);

        /// <summary>
        /// Initialize using the database name type
        /// </summary>
        /// <param name="dbType"></param>
        void Initialize(string dbType);
    }
}

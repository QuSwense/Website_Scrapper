using DynamicDatabase.Interfaces;
using DynamicDatabase.Sqlite;
using DynamicDatabase.Types;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using Unity;
using Unity.Resolution;
using WebScrapper.Db.Ctx;

namespace DynamicDatabase
{
    /// <summary>
    /// The Factory class used to initialize the Database classes
    /// Manipulate this class to create different database context structure
    /// </summary>
    public static class DynamicDbFactory
    {
        /// <summary>
        /// Unity container
        /// </summary>
        private static readonly IUnityContainer container;

        /// <summary>
        /// Static constructor
        /// </summary>
        static DynamicDbFactory()
        {
            container = new UnityContainer();

            RegisterDefaults();
        }

        /// <summary>
        /// Register defaults
        /// </summary>
        private static void RegisterDefaults()
        {
            container.RegisterType<IColumnMetadata, DynamicColumnMetadata>();
            container.RegisterType<IDbRow, DynamicRow>();
            container.RegisterType<IColumnHeaders, DynamicColumnHeaders>();
            container.RegisterType<IDbCommand, DynamicDbCommand>();
            container.RegisterType<IDbTable, DynamicTable>();
            container.RegisterType<IDataTypeContext, DataTypeContext>();
        }

        /// <summary>
        /// Register types for sqlite
        /// </summary>
        public static void RegisterSqlite()
        {
            container.RegisterType<DbConnection, SQLiteConnection>();
            container.RegisterType<IDataTypeContext, SqliteDataTypeContext>();
            container.RegisterType<IDynamicDbConnection, SqliteDbConnection>();
            container.RegisterType<IDbCommand, SqliteDbCommand>();
        }

        /// <summary>
        /// Register types
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public static void Register<TContract, TImplementation>()
            where TImplementation : TContract
        {
            container.RegisterType<TContract, TImplementation>();
        }
        
        /// <summary>
        /// Create a new instance of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            return container.Resolve<T>();
        }
    }
}

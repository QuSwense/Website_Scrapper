using DynamicDatabase.Interfaces;
using DynamicDatabase.Sqlite;
using DynamicDatabase.Types;
using System.Data.Common;
using System.Data.SQLite;
using Unity;

namespace DynamicDatabase
{
    /// <summary>
    /// The Factory class used to initialize the Database classes
    /// Manipulate this class to create different database context structure
    /// </summary>
    public class DynamicDbFactory : IDbFactory
    {
        #region Properties

        /// <summary>
        /// Unity container
        /// </summary>
        private readonly IUnityContainer container;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Static constructor
        /// </summary>
        public DynamicDbFactory()
        {
            container = new UnityContainer();

            RegisterDefaults();
        }

        /// <summary>
        /// Initialize using the database name type
        /// </summary>
        /// <param name="dbType"></param>
        public void Initialize(string dbType)
        {
            if (string.Compare(dbType, "sqlite", true) == 0)
                RegisterSqlite();
            else
                RegisterSqlite();
        }

        /// <summary>
        /// Initialize using the database name type
        /// </summary>
        /// <param name="dbType"></param>
        public static DynamicDbFactory Init(string dbType)
        {
            DynamicDbFactory dbFactory = new DynamicDbFactory();
            dbFactory.Initialize(dbType);
            return dbFactory;
        }

        #endregion Constructor

        #region Register

        /// <summary>
        /// Register defaults
        /// </summary>
        private void RegisterDefaults()
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
        public void RegisterSqlite()
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
        public void Register<TContract, TImplementation>()
            where TImplementation : TContract
        {
            container.RegisterType<TContract, TImplementation>();
        }

        #endregion Register

        #region Create

        /// <summary>
        /// Create a new instance of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Create<T>()
        {
            return container.Resolve<T>();
        }

        #endregion Create
    }
}

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
        /// Register database type
        /// </summary>
        /// <param name="dbType"></param>
        void RegisterDb(string dbType);

        /// <summary>
        /// Create a new instance of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Create<T>();

        /// <summary>
        /// Initialize using the database name type
        /// </summary>
        /// <param name="dbType"></param>
        void Initialize(string dbType);
    }
}

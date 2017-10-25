using DynamicDatabase.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicDatabase
{
    /// <summary>
    /// The Factory class used to initialize the Database classes
    /// Manipulate this class to create different database context structure
    /// </summary>
    public static class DynamicDbFactory
    {
        /// <summary>
        /// Stores the type mapping structure
        /// </summary>
        private static Dictionary<Type, Type> typesStore;

        /// <summary>
        /// Stores default for each types used in the DynamicDatabase structures
        /// </summary>
        private static Dictionary<Type, Type> typesStoreDefault;

        /// <summary>
        /// Static constructor
        /// </summary>
        static DynamicDbFactory()
        {
            typesStore = new Dictionary<Type, Type>();
            typesStoreDefault = new Dictionary<Type, Type>();

            RegisterDefaults();
        }

        /// <summary>
        /// Register defaults
        /// </summary>
        private static void RegisterDefaults()
        {
            typesStoreDefault[typeof(IColumnMetadata)] = typeof(DynamicColumnMetadata);
            typesStoreDefault[typeof(IDbRow)] = typeof(DynamicRow);
            typesStoreDefault[typeof(IColumnHeaders)] = typeof(DynamicColumnHeaders);
            typesStoreDefault[typeof(IDbCommand)] = typeof(DynamicDbCommand);
            typesStoreDefault[typeof(IDbTable)] = typeof(DynamicTable);
        }

        /// <summary>
        /// Register types for sqlite
        /// </summary>
        public static void RegisterSqlite()
        {
            typesStore[typeof(DbConnection)] = typeof(SQLiteConnection);
        }

        /// <summary>
        /// Register types
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public static void Register<TContract, TImplementation>()
        {
            typesStore[typeof(TContract)] = typeof(TImplementation);
        }

        /// <summary>
        /// Used internally for index navigation
        /// </summary>
        private static int argsIndex;

        /// <summary>
        /// Create a new instance of the type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Create<T>()
        {
            argsIndex = 0;
            return (T)Create(typeof(T));
        }

        /// <summary>
        /// Create a new instance of type with arguments
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T Create<T>(params object[] args)
        {
            argsIndex = 0;
            return (T)Create(typeof(T), args);
        }

        /// <summary>
        /// Privately create an instance
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static object Create(Type contract, params object[] args)
        {
            Type implementation = typesStore[contract];

            if (implementation == null)
                implementation = typesStoreDefault[contract];

            if (implementation == null)
            {
                ConstructorInfo constructor = implementation.GetConstructors()[0];
                ParameterInfo[] constructorParameters = constructor.GetParameters();
                if (constructorParameters.Length == 0)
                {
                    return Activator.CreateInstance(implementation);
                }
                List<object> parameters = new List<object>(constructorParameters.Length);
                foreach (ParameterInfo parameterInfo in constructorParameters)
                {
                    parameters.Add(Create(parameterInfo.ParameterType, args));
                }
                return constructor.Invoke(parameters.ToArray());
            }
            else if(args != null && args.Length < argsIndex)
            {
                return args[argsIndex++];
            }
            else
            {
                if (contract.IsValueType)
                {
                    return Activator.CreateInstance(contract);
                }
                return null;
            }
        }
    }
}

using ScrapEngine.Bl;
using ScrapEngine.Db;
using ScrapEngine.Interfaces;
using Unity;

namespace ScrapEngine
{
    /// <summary>
    /// The factory class to instantiate and initialize classes
    /// </summary>
    public class ScrapFactory
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
        public ScrapFactory()
        {
            container = new UnityContainer();

            RegisterDefaults();
        }

        #endregion Constructor

        #region Register

        /// <summary>
        /// Register defaults
        /// </summary>
        private void RegisterDefaults()
        {
            container.RegisterType<IScrapEngineContext, ScrapEngineContext>();
            container.RegisterType<IScrapDbContext, ScrapDbContext>();
            container.RegisterType<IScrapHtmlContext, WebScrapHtmlContext>();
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

        /// <summary>
        /// Get the Scrap database context
        /// </summary>
        /// <returns></returns>
        public IScrapDbContext CreateDbContext(IScrapEngineContext engineContext)
        {
            IScrapDbContext objectContext = Create<IScrapDbContext>();
            objectContext.Initialize(engineContext);
            return objectContext;
        }

        /// <summary>
        /// Get the Scrap database context
        /// </summary>
        /// <returns></returns>
        public IScrapHtmlContext CreateHtmlContext(IScrapEngineContext engineContext)
        {
            IScrapHtmlContext objectContext = Create<IScrapHtmlContext>();
            objectContext.Initialize(engineContext);
            return objectContext;
        }

        #endregion Create
    }
}

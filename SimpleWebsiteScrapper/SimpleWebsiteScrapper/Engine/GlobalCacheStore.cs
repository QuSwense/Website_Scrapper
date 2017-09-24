namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// A singleton class initialized when the application is started and cleaned when 
    /// the application is closed
    /// </summary>
    public class GlobalCacheStore
    {
        /// <summary>
        /// A cache of Html resources
        /// </summary>
        public HtmlNodeCache HtmlCache { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected GlobalCacheStore()
        {
            HtmlCache = new HtmlNodeCache();
        }

        /// <summary>
        /// The singlton pattern. The static single instance which is not accesible publicly
        /// </summary>
        protected static GlobalCacheStore store;

        /// <summary>
        /// Get the only instance of the class
        /// </summary>
        public static GlobalCacheStore This
        {
            get
            {
                if (store == null)
                    store = new GlobalCacheStore();
                return store;
            }
        }
    }
}

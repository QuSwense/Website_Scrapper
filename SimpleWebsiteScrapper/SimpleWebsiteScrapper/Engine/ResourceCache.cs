using System.Collections.Generic;

namespace SimpleWebsiteScrapper.Engine
{
    /// <summary>
    /// An asbtract class which is used as helper for the Engine to cache data.
    /// It is important as per the performance of the application to maintain a level of cache
    /// </summary>
    public abstract class ResourceCache<TCache>
    {
        /// <summary>
        /// The Main cache data which stores key <see cref="string"/> for each <see cref="TCache"/>
        /// </summary>
        protected Dictionary<string, TCache> CacheTree { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ResourceCache()
        {
            CacheTree = new Dictionary<string, TCache>();
        }

        /// <summary>
        /// This method loads the resource.
        /// It first checks from the cache, if found returns the resource, else
        /// it makes a raw call to fetch the resource
        /// </summary>
        /// <param name="TCacheKey"></param>
        /// <returns></returns>
        public TCache Retrieve(string TCacheKey)
        {
            if (CacheTree.ContainsKey(TCacheKey))
            {
                return CacheTree[TCacheKey];
            }
            else
            {
                TCache resource = Load(TCacheKey);
                CacheTree.Add(TCacheKey, resource);

                return resource;
            }
        }

        /// <summary>
        /// LOad the resource from the actual source and not from cache.
        /// This is a call always made first time.
        /// This method should be overriden
        /// </summary>
        /// <param name="TCacheKey"></param>
        /// <returns></returns>
        public abstract TCache Load(string TCacheKey);
    }
}

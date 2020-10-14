using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Alachisoft.NCache.Data.Caching
{
    

    /// <summary>
    /// Provides methods to return Cache objects that are mapped to a named cache. 
    /// This class also enables programmatic configuration of the cache client. 
    /// </summary>
    public class DataCacheFactory:IDisposable
    {
        #region[    Constructors    ]
        /// <summary>
        /// configure cache based on the default configurations
        /// </summary>
        public DataCacheFactory()
        { }
        /// <summary>
        /// configure cache with a custom configuration file
        /// </summary>
        /// <param name="configuration"></param>
        public DataCacheFactory(DataCacheFactoryConfiguration configuration)
        {
            _initParams = new Alachisoft.NCache.Web.Caching.CacheInitParams();

            var cacheServerInfoList = new List<Alachisoft.NCache.Web.Caching.CacheServerInfo>(configuration.Servers.Count());
            foreach (DataCacheServerEndpoint _temp in configuration.Servers)
            {
                cacheServerInfoList.Add(new Alachisoft.NCache.Web.Caching.CacheServerInfo(_temp.HostName, _temp.CachePort));
            }

            _initParams.ServerList = cacheServerInfoList.ToArray();
        }
        #endregion
        
        /// <summary>
        /// Returns specified Cache Client
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
        public DataCache GetCache(string cacheName)
        {
            Alachisoft.NCache.Web.Caching.Cache theCache = null;
            string cache = ConfigurationManager.AppSettings[cacheName];
            if (cache!=null)
            {
                try
                {
                    if (_initParams != null)
                    {
                        theCache = NCache.Web.Caching.NCache.InitializeCache(cache, _initParams);
                    }
                    else
                    {
                        theCache = NCache.Web.Caching.NCache.InitializeCache(cache);
                    }
                    _dataCacheList.Add(theCache);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return new DataCache(theCache, cacheName);
        }

        /// <summary>
        /// returns the Default Cache Client
        /// </summary>
        /// <returns></returns>
        public DataCache GetDefaultCache()
        {
            Alachisoft.NCache.Web.Caching.Cache theCache = null;
            string cache = ConfigurationManager.AppSettings["Default"];
            try
            {
                theCache = NCache.Web.Caching.NCache.InitializeCache(cache);
                _dataCacheList.Add(theCache);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new DataCache(theCache, "Default");
        }

        /// <summary>
        /// terminates the Cache factory and disposes resources associated with it
        /// </summary>
        public void Dispose()
        {
            foreach (Alachisoft.NCache.Web.Caching.Cache cache in _dataCacheList)
            {
                cache.Dispose();
            }
        }

        #region[private members]
        private Alachisoft.NCache.Web.Caching.CacheInitParams _initParams = null;
        private List<Alachisoft.NCache.Web.Caching.Cache> _dataCacheList = new List<Alachisoft.NCache.Web.Caching.Cache>();
        #endregion
    }
}

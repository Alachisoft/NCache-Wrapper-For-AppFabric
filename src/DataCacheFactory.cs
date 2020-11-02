using System;
using System.Configuration;

namespace Alachisoft.NCache.Data.Caching
{
    public sealed class DataCacheFactory : IDisposable
    {
        private readonly DataCacheFactoryConfiguration _configuration;

        private static bool expirable;
        private static TimeSpan defaultTTL;
        private static string defaultCache;

        static DataCacheFactory()
        {
            var result = bool.TryParse(ConfigurationManager.AppSettings["Expirable"], out bool expirable1);

            if (result)
            {
                expirable = expirable1;
            }
            else
            {
                expirable = true;
            }

            result = TimeSpan.TryParse(ConfigurationManager.AppSettings["TTL"], out TimeSpan timeOut);

            if (result)
            {
                if (timeOut <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value in the application configuration file.");
                }
                defaultTTL = timeOut;
            }
            else
            {
                defaultTTL = new TimeSpan(0, 10, 0);
            }

            defaultCache = ConfigurationManager.AppSettings["Default"];
        }
        public DataCacheFactory()
        {

        }

        public DataCacheFactory(DataCacheFactoryConfiguration configuration)
        {
            _configuration = configuration.Clone() as DataCacheFactoryConfiguration;
        }

        public DataCache GetCache(string cacheName)
        {
            return new DataCache(cacheName, _configuration, expirable, defaultTTL);
        }

        public DataCache GetDefaultCache()
        {
            return new DataCache(defaultCache, null, true, new TimeSpan(0, 10, 0));
        }
        public void Dispose()
        {
        }
    }
}

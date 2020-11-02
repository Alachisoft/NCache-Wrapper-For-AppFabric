namespace Alachisoft.NCache.Data.Caching
{
    public delegate void DataCacheNotificationCallback(string cacheName, string regionName, string key, DataCacheItemVersion version, DataCacheOperations cacheOperation, DataCacheNotificationDescriptor nd);
}
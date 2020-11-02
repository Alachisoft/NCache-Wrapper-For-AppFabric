using System.Collections.Generic;

namespace Alachisoft.NCache.Data.Caching
{
    public delegate void DataCacheBulkNotificationCallback(string cacheName, IEnumerable<DataCacheOperationDescriptor> operations, DataCacheNotificationDescriptor nd);
}
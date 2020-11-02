using System;
using System.Collections.ObjectModel;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheItem
    {
        public string CacheName { get; internal set; }
        public string Key { get; internal set; }
        public string RegionName { get; internal set; }
        public ReadOnlyCollection<DataCacheTag> Tags { get; internal set; }
        public TimeSpan Timeout { get; internal set; }
        public object Value { get; internal set; }
        public DataCacheItemVersion Version { get; internal set; }
    }
}

using Newtonsoft.Json;
using System;

namespace Alachisoft.NCache.Data.Caching
{
    [Serializable]
    public class DataCacheOperationDescriptor : BaseOperationNotification
    {
        [JsonConstructor]
        public DataCacheOperationDescriptor(string cacheName, string regionName, string key, DataCacheOperations opType, DataCacheItemVersion version) : base(cacheName, opType, version)
        {
            RegionName = regionName;
            Key = key;
        }

        public string RegionName { get; }
        public string Key { get; }

        public override string ToString()
        {
            if (this == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(this);
        }
    }
}

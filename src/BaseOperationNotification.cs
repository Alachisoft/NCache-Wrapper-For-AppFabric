using Newtonsoft.Json;

namespace Alachisoft.NCache.Data.Caching
{
    public class BaseOperationNotification
    {
        [JsonConstructor]
        public BaseOperationNotification(string cacheName, DataCacheOperations opType, DataCacheItemVersion version)
        {
            CacheName = cacheName;
            OperationType = opType;
            Version = version;
        }

        public string CacheName { get; }
        public DataCacheOperations OperationType { get; }
        public DataCacheItemVersion Version { get; }

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

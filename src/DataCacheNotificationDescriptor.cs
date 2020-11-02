using Newtonsoft.Json;
using System.Threading;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheNotificationDescriptor
    {
        internal DataCacheNotificationDescriptor(
            string cacheName)
        {
            CacheName = cacheName;
            DelegateId = Interlocked.Increment(ref globalDelegateID);
        }

        internal DataCacheNotificationDescriptor(DataCacheNotificationDescriptor other)
        {
            CacheName = other.CacheName;
            DelegateId = other.DelegateId;
        }
        public string CacheName { get; }
        public long DelegateId { get; }

        private static long globalDelegateID = 0L;

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

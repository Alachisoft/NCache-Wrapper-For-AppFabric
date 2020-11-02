using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheNotificationProperties
    {
        public DataCacheNotificationProperties(long maxQueueLength, TimeSpan pollInterval)
        {
            MaxQueueLength = maxQueueLength;
            PollInterval = pollInterval;
        }

        internal DataCacheNotificationProperties(DataCacheNotificationProperties other)
        {
            MaxQueueLength = other.MaxQueueLength;
            PollInterval = other.PollInterval;
        }
        public long MaxQueueLength { get; private set; }
        public TimeSpan PollInterval { get; private set; }

        public override bool Equals(object obj)
        {
            if (this is null)
            {
                if (obj is null)
                {
                    return true;
                }

                return false;
            }

            if (!(this is null) && (obj is null))
            {
                return false;
            }

            var other = obj as DataCacheNotificationProperties;

            if (other is null)
            {
                return false;
            }

            return MaxQueueLength == other.MaxQueueLength && PollInterval == other.PollInterval;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode();
            }
        }
    }
}

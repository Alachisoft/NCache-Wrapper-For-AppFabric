using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheLocalCacheProperties
    {
        public DataCacheLocalCacheProperties()
        {
            DefaultTimeout = TimeSpan.Zero;
            InvalidationPolicy = DataCacheLocalCacheInvalidationPolicy.TimeoutBased;
            IsEnabled = false;
            ObjectCount = 50000L;
        }
        public DataCacheLocalCacheProperties(long objectCount, TimeSpan defaultTimeout, DataCacheLocalCacheInvalidationPolicy invalidationPolicy)
        {
            ObjectCount = objectCount;
            DefaultTimeout = defaultTimeout;
            InvalidationPolicy = invalidationPolicy;
        }

        internal DataCacheLocalCacheProperties(DataCacheLocalCacheProperties other)
        {
            DefaultTimeout = other.DefaultTimeout;
            ObjectCount = other.ObjectCount;
            InvalidationPolicy = other.InvalidationPolicy;
            IsEnabled = other.IsEnabled;
        }
        public TimeSpan DefaultTimeout { get; }
        public DataCacheLocalCacheInvalidationPolicy InvalidationPolicy { get; }
        public bool IsEnabled { get; }
        public long ObjectCount { get; }

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

            var other = obj as DataCacheLocalCacheProperties;

            if (other is null)
            {
                return false;
            }

            return DefaultTimeout == other.DefaultTimeout && ObjectCount == other.ObjectCount && IsEnabled == other.IsEnabled && InvalidationPolicy == other.InvalidationPolicy;
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

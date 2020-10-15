using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheLocalCacheProperties
    {
        #region[    Constructors    ]
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
        #endregion

        public TimeSpan DefaultTimeout { get; private set; }
        public DataCacheLocalCacheInvalidationPolicy InvalidationPolicy { get; private set; }
        public bool IsEnabled { get; private set; }
        public long ObjectCount { get; private set; }

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

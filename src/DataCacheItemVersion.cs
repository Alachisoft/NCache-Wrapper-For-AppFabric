using Alachisoft.NCache.Client;
using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheItemVersion : IComparable<DataCacheItemVersion>
    {
        internal DataCacheItemVersion(CacheItemVersion itemVersion)
        {
            this.itemVersion = itemVersion;
        }
        internal CacheItemVersion itemVersion { get; }

        public static bool operator ==(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return true;
            }
            else if (!ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return false;
            }
            else if (ReferenceEquals(null, left) && !ReferenceEquals(null, right))
            {
                return false;
            }
            else
            {
                return left.itemVersion.Version == right.itemVersion.Version;
            }
        }

        public static bool operator <(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return false;
            }
            else if (ReferenceEquals(null, left) && !ReferenceEquals(null, right))
            {
                return true;
            }
            else if (!ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return false;
            }
            else
            {
                return left.itemVersion.Version < right.itemVersion.Version;
            }
        }

        public static bool operator !=(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            return !(left == right);
        }

        public static bool operator >(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            if (ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return false;
            }
            else if (ReferenceEquals(null, left) && !ReferenceEquals(null, right))
            {
                return false;
            }
            else if (!ReferenceEquals(null, left) && ReferenceEquals(null, right))
            {
                return true;
            }
            else
            {
                return left.itemVersion.Version > right.itemVersion.Version;
            }
        }

        public override bool Equals(object obj)
        {
            DataCacheItemVersion other = obj as DataCacheItemVersion;
            return this == other;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (this == null)
                {
                    return base.GetHashCode();
                }
                else
                {
                    return ("DataCacheItemVersion" + itemVersion.Version).GetHashCode();
                }
            }
        }
        public int CompareTo(DataCacheItemVersion other)
        {
            if (this == other)
            {
                return 0;
            }
            else if (this < other)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}

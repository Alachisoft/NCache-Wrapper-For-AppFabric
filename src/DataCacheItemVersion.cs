using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Web.Caching;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheItemVersion : IComparable<DataCacheItemVersion>
    {
        internal CacheItemVersion _itemVersion;

        public static bool operator !=(DataCacheItemVersion left, DataCacheItemVersion right) 
        {
            if ((object.ReferenceEquals(null, left)) && (object.ReferenceEquals(null, right)))
            {
                return false;
            }
            else if (object.ReferenceEquals(null, right))
            {
                if (object.ReferenceEquals(null, left))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (left._itemVersion.Version != right._itemVersion.Version)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool operator <(DataCacheItemVersion left, DataCacheItemVersion right) 
        {
            if ((object.ReferenceEquals(null, left)) && (object.ReferenceEquals(null, right)))
            {
                return false;
            }
            else if (object.ReferenceEquals(null, right))
            {
                if (!object.ReferenceEquals(null, left))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (left._itemVersion.Version < right._itemVersion.Version)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool operator ==(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            if (object.ReferenceEquals(right, null) && object.ReferenceEquals(right, null))
            {
                return true;
            }
            else if (object.ReferenceEquals(right, null))
            {
                if (!object.ReferenceEquals(left, null))
                {
                    return false;
                }
                else
                    return true;
            }
            else
            {
                if (left._itemVersion.Version == right._itemVersion.Version)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool operator >(DataCacheItemVersion left, DataCacheItemVersion right)
        {
            if (object.ReferenceEquals(right, null) && object.ReferenceEquals(right, null))
            {
                return false;
            }
            else if (object.ReferenceEquals(right, null))
            {
                if (!object.ReferenceEquals(left, null))
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (left._itemVersion.Version > right._itemVersion.Version)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int CompareTo(DataCacheItemVersion other)
        {
            if (other == null)
            {
                return 1;
            }
            if (this._itemVersion.Version == other._itemVersion.Version)
            {
                return 0;
            }
            else if (this._itemVersion.Version < other._itemVersion.Version)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

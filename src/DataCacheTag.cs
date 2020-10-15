using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Runtime;
using Alachisoft.NCache.Runtime.Caching;

namespace Alachisoft.NCache.Data.Caching
{

    public class DataCacheTag
    {
        #region [   Constructor ]
        public DataCacheTag(String tag)
        {
            _tag = new Tag(tag);
        }
        #endregion

        internal Tag _tag;

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

            var other = obj as DataCacheTag;

            if (other is null)
            {
                return false;
            }

            return _tag.Equals(other._tag);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (this is null)
                {
                    return base.GetHashCode();
                }

                return _tag.GetHashCode();
            }
        }

        public override string ToString()
        {
            return _tag.ToString();
        }
    }
}

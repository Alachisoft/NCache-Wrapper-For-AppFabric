using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheTag
    {
        internal readonly string tag;

        public DataCacheTag(string tg)
        {
            tag = string.IsNullOrWhiteSpace(tg) ? throw new ArgumentNullException(nameof(tg), "Value cannot be null.") : tg.Trim();
        }
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

            return tag.Equals(other.tag);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (this is null)
                {
                    return base.GetHashCode();
                }

                return tag.GetHashCode();
            }
        }

        public override string ToString()
        {
            return tag;
        }
    }
}

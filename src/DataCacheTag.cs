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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheLocalCacheProperties
    {
        #region[    Constructors    ]
        public DataCacheLocalCacheProperties()
        { }
        public DataCacheLocalCacheProperties(long objectCount, TimeSpan defaultTimeout, DataCacheLocalCacheInvalidationPolicy invalidationPolicy) 
        { }
        #endregion
        
        public TimeSpan DefaultTimeout { get; private set; }
        public DataCacheLocalCacheInvalidationPolicy InvalidationPolicy { get; private set; }
        public bool IsEnabled { get; private set; }
        public long ObjectCount { get; private set; }
    }
}

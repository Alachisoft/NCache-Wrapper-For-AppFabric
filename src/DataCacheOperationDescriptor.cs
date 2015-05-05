using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    [Serializable]
    public class DataCacheOperationDescriptor : BaseOperationNotification
    {
        #region[    Constructor ]
        public DataCacheOperationDescriptor(string cacheName, string regionName, string key, DataCacheOperations opType, DataCacheItemVersion version)
        { }
        #endregion

        public string RegionName { get; set; }
        public string Key { get; set; }
    }
}

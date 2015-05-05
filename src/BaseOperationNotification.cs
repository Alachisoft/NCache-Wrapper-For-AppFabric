using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class BaseOperationNotification
    {
        public string CacheName{get; set;}
        public DataCacheOperations OperationType { get; set; }
        public DataCacheItemVersion Version { get; set; }

        #region[    Constructors    ]
        public BaseOperationNotification(string caheName, DataCacheOperations opType, DataCacheItemVersion version)
        { }
        internal BaseOperationNotification()
        { }
        #endregion
    }
}

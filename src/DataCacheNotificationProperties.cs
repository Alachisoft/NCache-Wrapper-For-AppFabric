using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheNotificationProperties
    {
        #region[    Constructor ]
        public DataCacheNotificationProperties(long maxQueueLength, TimeSpan pollInterval)
        { }
        #endregion

        public long MaxQueueLength { get; private set; }
        public TimeSpan PollInterval { get; private set; }
    }
}

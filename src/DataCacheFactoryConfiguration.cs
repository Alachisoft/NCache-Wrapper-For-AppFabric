using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheFactoryConfiguration :ICloneable
    {
        #region[    Constructor ]
        public DataCacheFactoryConfiguration()
        { }
        #endregion

        public TimeSpan ChannelOpenTimeout { get; set; }
        public DataCacheLocalCacheProperties LocalCacheProperties { get; set; }
        public int MaxConnectionsToServer { get; set; }
        public DataCacheNotificationProperties NotificationProperties { get; set; }
        public TimeSpan RequestTimeout { get; set; }
        public DataCacheSecurity SecurityProperties { get; set; }
        public IEnumerable<DataCacheServerEndpoint> Servers { get; set; }
        public DataCacheTransportProperties TransportProperties { get; set; }

        public object Clone()
        {
            return this;
        }
    }
}

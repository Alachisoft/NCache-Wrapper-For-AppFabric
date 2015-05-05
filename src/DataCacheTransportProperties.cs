using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheTransportProperties :ICloneable
    {
        #region[    Constructor ]
        public DataCacheTransportProperties()
        { }
        #endregion

        public TimeSpan ChannelInitializationTimeout { get; set; }
        public int ConnectionBufferSize { get; set; }
        public long MaxBufferPoolSize { get; set; }
        public int MaxBufferSize { get; set; }
        public TimeSpan MaxOutputDelay { get; set; }
        public TimeSpan ReceiveTimeout { get; set; }

        public object Clone()
        {
            return this;
        }
    }
}

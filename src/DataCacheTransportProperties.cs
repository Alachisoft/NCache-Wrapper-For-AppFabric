using System;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheTransportProperties : ICloneable
    {
        public TimeSpan ChannelInitializationTimeout { get; set; }
        public int ConnectionBufferSize { get; set; }
        public long MaxBufferPoolSize { get; set; }
        public int MaxBufferSize { get; set; }
        public TimeSpan MaxOutputDelay { get; set; }
        public TimeSpan ReceiveTimeout { get; set; }

        public object Clone()
        {
            if (ReferenceEquals(this, null))
            {
                return null;
            }

            return new DataCacheTransportProperties
            {
                ChannelInitializationTimeout = this.ChannelInitializationTimeout,
                ConnectionBufferSize = this.ConnectionBufferSize,
                MaxBufferPoolSize = this.MaxBufferPoolSize,
                MaxBufferSize = this.MaxBufferSize,
                MaxOutputDelay = this.MaxOutputDelay,
                ReceiveTimeout = this.ReceiveTimeout
            };
        }
    }
}

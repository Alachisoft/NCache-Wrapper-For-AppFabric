using System;
using System.Collections.Generic;
using System.Linq;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheFactoryConfiguration
    {
        public TimeSpan ChannelOpenTimeout { get; set; } = TimeSpan.FromSeconds(3);
        public DataCacheLocalCacheProperties LocalCacheProperties { get; set; }
        public int MaxConnectionsToServer { get; set; }
        public DataCacheNotificationProperties NotificationProperties { get; set; }
        public TimeSpan RequestTimeout { get; set; } = TimeSpan.FromSeconds(15000);
        public DataCacheSecurity SecurityProperties { get; set; }
        public IEnumerable<DataCacheServerEndpoint> Servers { get; set; }
        public DataCacheTransportProperties TransportProperties { get; set; }

        public object Clone()
        {
            if (ReferenceEquals(this, null))
            {
                return null;
            }

            return new DataCacheFactoryConfiguration
            {
                ChannelOpenTimeout = this.ChannelOpenTimeout,
                LocalCacheProperties = this.LocalCacheProperties == null ? null : new DataCacheLocalCacheProperties(this.LocalCacheProperties),
                MaxConnectionsToServer = this.MaxConnectionsToServer,
                NotificationProperties = this.NotificationProperties == null ? null : new DataCacheNotificationProperties(this.NotificationProperties),
                RequestTimeout = this.RequestTimeout,
                SecurityProperties = this.SecurityProperties == null ? null : new DataCacheSecurity(this.SecurityProperties),
                TransportProperties = this.TransportProperties.Clone() as DataCacheTransportProperties,
                Servers = InitializeServers(this.Servers)
            };
        }

        private static IEnumerable<DataCacheServerEndpoint> InitializeServers(IEnumerable<DataCacheServerEndpoint> servers)
        {
            if (servers == null)
            {
                return null;
            }

            var serverCopies = new List<DataCacheServerEndpoint>(servers.Count());

            foreach (var server in servers)
            {
                serverCopies.Add(new DataCacheServerEndpoint(server.HostName, server.CachePort));
            }

            return serverCopies;
        }
    }
}

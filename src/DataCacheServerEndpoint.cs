using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheServerEndpoint
    {
        #region[    Constructors    ]
        internal DataCacheServerEndpoint() 
        { }
        public DataCacheServerEndpoint(string hostName, int cachePort)
        {
            this.HostName = hostName;
            this.CachePort = cachePort;
        }
        #endregion

        public int CachePort { get; set; }
        public string HostName { get; set; }
    }
}

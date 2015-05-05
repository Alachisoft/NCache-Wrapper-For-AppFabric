using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheSecurity
    {
        #region[    Constructors ]
        public DataCacheSecurity() 
        { }
        public DataCacheSecurity(DataCacheSecurityMode securityMode, DataCacheProtectionLevel protectionLevel) 
        { }
        #endregion

        public DataCacheProtectionLevel ProtectionLevel { get; private set; }
        public DataCacheSecurityMode SecurityMode { get; private set; }
    }
}

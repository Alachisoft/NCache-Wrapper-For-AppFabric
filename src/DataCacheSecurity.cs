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
        {
            ProtectionLevel = DataCacheProtectionLevel.None;
            SecurityMode = DataCacheSecurityMode.None;
        }
        public DataCacheSecurity(DataCacheSecurityMode securityMode, DataCacheProtectionLevel protectionLevel) 
        {
            ProtectionLevel = protectionLevel;
            SecurityMode = securityMode;
        }

        internal DataCacheSecurity(DataCacheSecurity other)
        {
            ProtectionLevel = other.ProtectionLevel;
            SecurityMode = other.SecurityMode;
        }
        #endregion

        public DataCacheProtectionLevel ProtectionLevel { get; private set; }
        public DataCacheSecurityMode SecurityMode { get; private set; }

        public override bool Equals(object obj)
        {
            if (this is null)
            {
                if (obj is null)
                {
                    return true;
                }

                return false;
            }

            if (!(this is null) && (obj is null))
            {
                return false;
            }

            var other = obj as DataCacheSecurity;

            if (other is null)
            {
                return false;
            }

            return ProtectionLevel == other.ProtectionLevel && SecurityMode == other.SecurityMode;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                if (this is null)
                {
                    return base.GetHashCode();
                }

                return (ProtectionLevel + "" + SecurityMode).GetHashCode();
            }
        }
    }
}

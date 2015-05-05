using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheNotificationDescriptor
    {
        public string CacheName
        {
            get { return _cacheName; }
            internal set {_cacheName= value; }
        }
        public long DelegateId
        {
            get { return _delegateId; }
            internal set { _delegateId = value; }
        }

        #region [private members]
        private string _cacheName;
        private long _delegateId;
        #endregion
    }
}

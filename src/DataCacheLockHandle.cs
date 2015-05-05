using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Web.Caching;
namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheLockHandle
    {
        #region[    Constructor ]
        public DataCacheLockHandle()
        { }
        #endregion

        public override string ToString()
        {
            return base.ToString();
        }
        internal LockHandle _lockHandle{get;set;} 
    }
}

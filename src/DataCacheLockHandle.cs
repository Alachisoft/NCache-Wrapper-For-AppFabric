using Alachisoft.NCache.Client;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheLockHandle
    {
        internal DataCacheLockHandle(LockHandle lockHandle)
        {
            LockHandle = lockHandle;
        }
        public override string ToString()
        {
            if (this == null || this.LockHandle == null)
                return null;

            return LockHandle.LockId;
        }
        internal LockHandle LockHandle { get; }
    }
}

using Alachisoft.NCache.Client;

namespace Alachisoft.NCache.Data.Caching
{
    internal interface ICallBackHandler
    {
        CacheEventDescriptor NCacheEventDescriptor { get; set; }
    }
}

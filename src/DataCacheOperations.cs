using System;

namespace Alachisoft.NCache.Data.Caching
{
    [Flags]
    public enum DataCacheOperations
    {
        AddItem = 1,
        ReplaceItem = 2,
        RemoveItem = 4,
        CreateRegion = 8,
        RemoveRegion = 16,
        ClearRegion = 32
    }
}

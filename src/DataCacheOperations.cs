using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        ClearRegion = 32,
    }
}

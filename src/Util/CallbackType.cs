using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    /// <summary>
    /// defines the type of call backs to allow calling
    /// of appropriate functions
    /// </summary>
    [Flags]
    enum CallbackType
    {
        CacheLevelCallback = 1,
        ItemSpecificCallback = 2,
        RegionSpecificItemCallback = 3,
        RegionSpecificCallback = 4,
        AddRegion = 5,
        RemoveRegion = 6,
        ClearRegion = 7,
    }
}

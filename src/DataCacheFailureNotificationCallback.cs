using System;
using System.Collections.Generic;
namespace Alachisoft.NCache.Data.Caching
{
    public delegate void DataCacheFailureNotificationCallback(string cacheName, DataCacheNotificationDescriptor nd);
}
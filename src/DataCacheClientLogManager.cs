using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Alachisoft.NCache.Data.Caching
{
    public static class DataCacheClientLogManager
    {
        public static TraceLevel ChangeLogLevel(TraceLevel level)
        {
            return TraceLevel.Off;
        }
        public static void SetSink(DataCacheTraceSink traceSink, TraceLevel traceLevel)
        { }
    }
}

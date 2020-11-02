namespace Alachisoft.NCache.Data.Caching
{
    public static class DataCacheErrorSubStatus
    {
        public const int None = -1;
        public const int NotPrimary = 1;
        public const int NoWriteQuorum = 2;
        public const int ReplicationQueueFull = 3;
        public const int KeyLatched = 4;
        public const int CacheServerUnavailable = 5;
        public const int Throttled = 6;
        public const int QuotaExceeded = 9;
        public const int ReadThroughKeyContention = 7;
        public const int ServiceMemoryShortage = 8;
        public const int InternalError = 10;
        public const int ReplicationFailed = 11;
    }
}


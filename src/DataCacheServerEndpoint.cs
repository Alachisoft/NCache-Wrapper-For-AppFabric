namespace Alachisoft.NCache.Data.Caching
{
    public class DataCacheServerEndpoint
    {
        internal DataCacheServerEndpoint()
        { }
        public DataCacheServerEndpoint(string hostName, int cachePort)
        {
            this.HostName = hostName;
            this.CachePort = cachePort;
        }
        public int CachePort { get; set; }
        public string HostName { get; set; }

        public override bool Equals(object obj)
        {
            if (this is null)
            {
                if (obj is null)
                {
                    return true;
                }

                return false;
            }

            if (!(this is null) && (obj is null))
            {
                return false;
            }

            var other = obj as DataCacheServerEndpoint;

            if (other is null)
            {
                return false;
            }

            return CachePort == other.CachePort && HostName == other.HostName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (this is null)
                {
                    return base.GetHashCode();
                }

                return (CachePort + HostName).GetHashCode();
            }
        }
    }
}

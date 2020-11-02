using Newtonsoft.Json;

namespace Alachisoft.NCache.Data.Caching
{
    internal class CacheKey
    {
        [JsonConstructor]
        internal CacheKey()
        {

        }

        public string Key { get; set; }

        public string Region { get; set; }

        public override string ToString()
        {
            if (this == null)
            {
                return null;
            }

            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }

        internal static CacheKey Deserialize(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<CacheKey>(cacheKey, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }
    }
}

using Newtonsoft.Json;

namespace Alachisoft.NCache.Data.Caching
{
    internal class CacheTag
    {
        [JsonConstructor]
        internal CacheTag()
        { }

        public string Tag { get; set; }
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

        internal static CacheTag Deserialize(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<CacheTag>(cacheKey, new JsonSerializerSettings
            {
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        }
    }
}

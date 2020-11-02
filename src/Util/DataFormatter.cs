using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Dependencies;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Alachisoft.NCache.Data.Caching
{
    internal static class DataFormatter
    {
        internal static string MarshalKey(string key, string region = null)
        {
            var cacheKey = new CacheKey
            {
                Key = key.Trim(),
                Region = string.IsNullOrWhiteSpace(region) ? null : region.Trim()
            };

            return cacheKey.ToString();
        }


        internal static string MarshalRegionKey(string region)
        {
            return $"{Constants.CREATE_REGION_KEY}{region.Trim()}";
        }

        internal static string UnMarshalKey(string cacheKeyString)
        {
            var cacheKey = CacheKey.Deserialize(cacheKeyString);

            return cacheKey.Key;
        }


        internal static string[] SplitKeyAndRegion(string cacheKeyString)
        {
            var cacheKey = CacheKey.Deserialize(cacheKeyString);
            return new string[]
            {
                cacheKey.Key,
                cacheKey.Region == null ? "Default_Region" : cacheKey.Region
            };
        }

        internal static Tag MarshalTag(DataCacheTag dataCacheTag, string region)
        {
            var cacheTag = new CacheTag
            {
                Region = string.IsNullOrWhiteSpace(region) ? null : region.Trim(),
                Tag = dataCacheTag.tag.Trim()
            };

            return new Tag(cacheTag.ToString());
        }

        internal static Tag MarshalRegionTag(string region)
        {
            var cacheTag = new CacheTag
            {
                Tag = null,
                Region = region.Trim()
            };

            return new Tag(cacheTag.ToString());
        }

        internal static Tag[] MarshalTags(IEnumerable<DataCacheTag> dataCacheTags, string region, bool includeRegion = true)
        {
            List<Tag> cacheTags = null;
            if (dataCacheTags != null)
            {
                if (!string.IsNullOrWhiteSpace(region) && includeRegion)
                {
                    cacheTags = new List<Tag>(dataCacheTags.Count() + 1);
                    cacheTags.Add(MarshalRegionTag(region));
                }
                else
                {
                    cacheTags = new List<Tag>(dataCacheTags.Count());
                }

                cacheTags.AddRange(dataCacheTags.Select(x =>
                {
                    var cacheTag = new CacheTag
                    {
                        Tag = x.tag.Trim(),
                        Region = string.IsNullOrWhiteSpace(region) ? null : region.Trim()
                    };
                    return new Tag(cacheTag.ToString());
                }));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(region))
                {
                    cacheTags = new List<Tag>(1);
                    cacheTags.Add(MarshalRegionTag(region));
                }
            }

            if (cacheTags == null)
            {
                return null;
            }

            return cacheTags.ToArray();
        }

        internal static CacheItem CreateCacheItem(object value, IEnumerable<DataCacheTag> tags, string region, bool expirable, TimeSpan timeout)
        {
            var cacheItem = new CacheItem(value);

            var tagsArray = MarshalTags(tags, region);

            if (tagsArray != null)
            {
                cacheItem.Tags = tagsArray;
            }

            if (!string.IsNullOrWhiteSpace(region))
            {
                cacheItem.Dependency = new KeyDependency(MarshalRegionKey(region));
                cacheItem.Group = region.Trim();
            }
            else
            {
                cacheItem.Group = "Default_Region";
            }

            if (timeout <= TimeSpan.Zero)
            {
                throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
            }

            if (expirable)
            {
                cacheItem.Expiration = new Expiration(ExpirationType.Absolute, timeout);
            }

            return cacheItem;
        }

        internal static DataCacheItem ConvertToDataCacheItem(string key, string cacheName, CacheItem cacheItem)
        {
            var dataCacheItem = new DataCacheItem
            {
                Key = key.Trim(),
                CacheName = cacheName,
                RegionName = cacheItem.Group,
                Timeout = cacheItem.Expiration.ExpireAfter,
                Value = cacheItem.GetValue<object>(),
                Version = new DataCacheItemVersion(cacheItem.Version)
            };

            var dataCacheTags = UnMarshalTags(cacheItem.Tags);

            if (dataCacheTags != null)
            {
                dataCacheItem.Tags = new ReadOnlyCollection<DataCacheTag>(dataCacheTags);
            }

            return dataCacheItem;
        }

        private static DataCacheTag[] UnMarshalTags(IEnumerable<Tag> tags)
        {
            if (tags == null || tags.Count() == 0)
            {
                return null;
            }

            var tagsArray = tags.ToArray();

            List<DataCacheTag> dataCacheTags = new List<DataCacheTag>(tags.Count());

            for (int i = 0; i < tagsArray.Length; i++)
            {
                var cacheTag = CacheTag.Deserialize(tagsArray[i].ToString());

                if (!string.IsNullOrEmpty(cacheTag.Tag))
                {
                    dataCacheTags.Add(new DataCacheTag(cacheTag.Tag));
                }

            }

            return dataCacheTags.ToArray();
        }
    }
}

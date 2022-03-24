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
        private static string defaultRegion = "Default_Region";

        internal static string MarshalKey(string key, string region = null)
        {
            var cacheKey = new CacheKey
            {
                Key = key.Trim(),
                Region = string.IsNullOrWhiteSpace(region) ? defaultRegion : region.Trim()
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
            /// get data with tag from specific  region
            if (!string.IsNullOrEmpty(region))
            {
                var cacheTag = new CacheTag
                {
                    Region = region.Trim(),
                    Tag = dataCacheTag.tag.Trim()
                };
                return new Tag(cacheTag.ToString());
            }

            return new Tag(dataCacheTag.tag.Trim());
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


        internal static Tag[] MarshalTagsAndRegion(IEnumerable<DataCacheTag> dataCacheTags, string region)
        {
            List<Tag> cacheTags = new List<Tag>();
            if (dataCacheTags != null)
            {
                // if region is not null then add data with tag+region to specify data with tag in specific region
                if (!string.IsNullOrWhiteSpace(region))
                {
                    cacheTags.AddRange(dataCacheTags.Select(x =>
                    {
                        var cacheTag = new CacheTag
                        {
                            Tag = x.tag.Trim(),
                            Region = region.Trim()
                        };

                        return new Tag(cacheTag.ToString());
                    }));
                }

                else
                {
                    // add all tags independent from region 
                    cacheTags.AddRange(dataCacheTags.Select(x =>
                    {
                        return new Tag(x.tag.Trim());
                    }));
                }

            }

            if (cacheTags == null)
            {
                return null;
            }

            return cacheTags.ToArray();
        }


        internal static Tag[] MarshalTags(IEnumerable<DataCacheTag> dataCacheTags, string region, bool includeRegion = true)
        {
            List<Tag> cacheTags = new List<Tag>();
            if (dataCacheTags != null)
            {
                // if region is not null then add data with tag+region to specify data with tag in specific region

                cacheTags.AddRange(dataCacheTags.Select(x =>
                {
                    var cacheTag = new CacheTag
                    {
                        Tag = x.tag.Trim(),
                        Region = string.IsNullOrWhiteSpace(region) ? defaultRegion : region.Trim()
                    };

                    return new Tag(cacheTag.ToString());
                }));


                if (includeRegion)
                {
                    // add all tags independent from region 
                    cacheTags.AddRange(dataCacheTags.Select(x =>
                    {
                        return new Tag(x.tag.Trim());
                    }));
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

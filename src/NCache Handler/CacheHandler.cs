using Alachisoft.NCache.Client;
using Alachisoft.NCache.Common.ErrorHandling;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Runtime.Events;
using Alachisoft.NCache.Runtime.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Alachisoft.NCache.Data.Caching
{
    internal class CacheHandler
    {
        private readonly ICache _cache;
        private readonly TimeSpan _defaultTimeout;
        private readonly bool _expirable;
        private readonly string _cacheName;
        private readonly ConcurrentDictionary<string, ICallBackHandler> _callbackMap;
        private string defaultRegion= "Default_Region";

        internal CacheHandler(string cacheName, CacheConnectionOptions connectionOptions, bool expirable, TimeSpan defaultTimeout)
        {
            _cacheName = cacheName.Trim();
            _expirable = expirable;

            _defaultTimeout = defaultTimeout;


            if (connectionOptions == null)
            {
                _cache = CacheManager.GetCache(cacheName.Trim());
            }
            else
            {
                _cache = CacheManager.GetCache(cacheName.Trim(), connectionOptions);
            }

            _callbackMap = new ConcurrentDictionary<string, ICallBackHandler>();
        }


        internal object Get(string key)
        {
           return Get(key, defaultRegion);
        }

        internal object Get(string key, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                return _cache.Get<object>(cacheKey);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal object Get(string key, out DataCacheItemVersion version)
        {
            return Get(key, out version, defaultRegion);
        }

        internal object Get(string key, out DataCacheItemVersion version, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);


                version = null;
                CacheItemVersion cacheItemVersion = null;

                var item = _cache.Get<object>(cacheKey, ref cacheItemVersion);

                if (item != null)
                {
                    version = new DataCacheItemVersion(cacheItemVersion);
                }

                return item;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }



        internal DataCacheItemVersion Add(string key, object value)
        {
            // create default region 
            CreateRegion(defaultRegion);
               
            // add data with default region
            return Add(key, value, defaultRegion);
        }

      
        internal DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags)
        {
            CreateRegion(defaultRegion);
            return Add(key, value, tags, defaultRegion);
        }

        internal DataCacheItemVersion Add(string key, object value, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, _defaultTimeout);

                var cacheItemVersion = _cache.Add(cacheKey, cacheItem);

                return new DataCacheItemVersion(cacheItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.KEY_ALREADY_EXISTS)
                    {
                        throw new DataCacheException("Key already exists.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.KeyAlreadyExists
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Add(string key, object value, TimeSpan timeout)
        {
            CreateRegion(defaultRegion);
            return Add(key, value, timeout, defaultRegion);
        }

        internal DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, _defaultTimeout);

                var cacheItemVersion = _cache.Add(cacheKey, cacheItem);

                return new DataCacheItemVersion(cacheItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.KEY_ALREADY_EXISTS)
                    {
                        throw new DataCacheException("Key already exists.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.KeyAlreadyExists
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Add(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags)
        {
            CreateRegion(defaultRegion);
            return Add(key, value,timeout, tags, defaultRegion);
        }

        internal DataCacheItemVersion Add(string key, object value, TimeSpan timeout, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, timeout);

                var cacheItemVersion = _cache.Add(cacheKey, cacheItem);

                return new DataCacheItemVersion(cacheItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.KEY_ALREADY_EXISTS)
                    {
                        throw new DataCacheException("Key already exists.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.KeyAlreadyExists
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Add(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, timeout);

                var cacheItemVersion = _cache.Add(cacheKey, cacheItem);

                return new DataCacheItemVersion(cacheItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.KEY_ALREADY_EXISTS)
                    {
                        throw new DataCacheException("Key already exists.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.KeyAlreadyExists
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, defaultRegion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, version, defaultRegion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, tags, defaultRegion);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, _defaultTimeout);

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, TimeSpan timeout)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, timeout, defaultRegion);
               
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put( key, value,  version, tags, defaultRegion);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, _defaultTimeout);
                cacheItem.Version = version.itemVersion;

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_WITH_VERSION_DOESNT_EXIST)
                    {
                        throw new DataCacheException("Invalid item version", ex)
                        {
                            ErrorCode = DataCacheErrorCode.CacheItemVersionMismatch
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeout)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, version, timeout, defaultRegion);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, _defaultTimeout);

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, timeout, tags, defaultRegion);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, TimeSpan timeout, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, timeout);

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, _defaultTimeout);
                cacheItem.Version = version.itemVersion;

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_WITH_VERSION_DOESNT_EXIST)
                    {
                        throw new DataCacheException("Invalid item version", ex)
                        {
                            ErrorCode = DataCacheErrorCode.CacheItemVersionMismatch
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeout, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                CreateRegion(defaultRegion);
                return Put(key, value, version, timeout, tags, defaultRegion);
            }
            catch (Exception e)
            {
               throw e;
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeout, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, timeout);
                cacheItem.Version = version.itemVersion;

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_WITH_VERSION_DOESNT_EXIST)
                    {
                        throw new DataCacheException("Invalid item version", ex)
                        {
                            ErrorCode = DataCacheErrorCode.CacheItemVersionMismatch
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, timeout);

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, timeout);
                cacheItem.Version = version.itemVersion;

                var updatedItemVersion = _cache.Insert(cacheKey, cacheItem);

                return new DataCacheItemVersion(updatedItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_WITH_VERSION_DOESNT_EXIST)
                    {
                        throw new DataCacheException("Invalid item version", ex)
                        {
                            ErrorCode = DataCacheErrorCode.CacheItemVersionMismatch
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found in cache.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }



        internal bool Remove(string key)
        {
            return Remove(key, defaultRegion);
        }

        internal bool Remove(string key, DataCacheItemVersion version)
        {

            return Remove(key,version, defaultRegion);
        }

        internal bool Remove(string key, DataCacheLockHandle lockHandle)
        {
            return Remove(key,lockHandle, defaultRegion);
        }

        internal bool Remove(string key, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var result = _cache.Remove(cacheKey, out object removedItem);

                return removedItem != null;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal bool Remove(string key, DataCacheItemVersion version, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                var result = _cache.Remove(cacheKey, out object removedItem, null, version.itemVersion, null);

                return removedItem != null;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_WITH_VERSION_DOESNT_EXIST)
                    {
                        return false;
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal bool Remove(string key, DataCacheLockHandle lockHandle, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var result = _cache.Remove(cacheKey, out object removedItem, lockHandle.LockHandle, null, null);

                return removedItem != null;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        return false;
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }


        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(DataCacheTag tag, string region)
        {
            try
            {
                if (tag == null)
                {
                    throw new ArgumentNullException(nameof(tag), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var objectsByTag = _cache.SearchService.GetByTag<object>(DataFormatter.MarshalTag(tag, region));

                if (objectsByTag == null || objectsByTag.Count == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }

                var result = new List<KeyValuePair<string, object>>(objectsByTag.Count());

                string key;
                foreach (var entry in objectsByTag)
                {
                    key = DataFormatter.UnMarshalKey(entry.Key);
                    result.Add(new KeyValuePair<string, object>(key, entry.Value));
                }

                return result;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var objectsByTag = _cache.SearchService.GetByTags<object>(DataFormatter.MarshalTags(tags, region, false), TagSearchOptions.ByAnyTag);


                if (objectsByTag == null || objectsByTag.Count == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }

                var result = new List<KeyValuePair<string, object>>(objectsByTag.Count());

                string key;
                foreach (var entry in objectsByTag)
                {
                    key = DataFormatter.UnMarshalKey(entry.Key);
                    result.Add(new KeyValuePair<string, object>(key, entry.Value));
                }

                return result;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var objectsByTag = _cache.SearchService.GetByTags<object>(DataFormatter.MarshalTags(tags, region, false), TagSearchOptions.ByAllTags);

                if (objectsByTag == null || objectsByTag.Count == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }

                var result = new List<KeyValuePair<string, object>>(objectsByTag.Count());

                string key;
                foreach (var entry in objectsByTag)
                {
                    key = DataFormatter.UnMarshalKey(entry.Key);
                    result.Add(new KeyValuePair<string, object>(key, entry.Value));
                }
                 
                return result;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var regionTag = DataFormatter.MarshalRegionTag(region);

                var objectsByTag = _cache.SearchService.GetByTag<object>(regionTag);

                if (objectsByTag == null || objectsByTag.Count() == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }

                var result = new List<KeyValuePair<string, object>>(objectsByTag.Count());

                string key;
                foreach (var entry in objectsByTag)
                {
                    key = DataFormatter.UnMarshalKey(entry.Key);
                    result.Add(new KeyValuePair<string, object>(key, entry.Value));
                }

                return result;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys)
        {
            return BulkGet(keys, defaultRegion);
        }

        internal IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys, string region)
        {
            try
            {
                if (keys == null)
                {
                    throw new ArgumentNullException(nameof(keys), "Value cannot be null.");
                }

                if (keys.Any(x => x == null))
                {
                    throw new ArgumentException("A key is passed with value null.", nameof(keys));
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (keys.Count() == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }

                var cacheKeys = keys.Select(x => DataFormatter.MarshalKey(x, region));

                var cachedDict = _cache.GetBulk<object>(cacheKeys);

                if (cachedDict == null || cachedDict.Count() == 0)
                {
                    return new List<KeyValuePair<string, object>>();
                }


                var result = new List<KeyValuePair<string, object>>(cachedDict.Count());

                string key;
                foreach (var entry in cachedDict)
                {
                    key = DataFormatter.UnMarshalKey(entry.Key);
                    result.Add(new KeyValuePair<string, object>(key, entry.Value));
                }

                return result;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItem GetCacheItem(string key)
        {
            return GetCacheItem(key, defaultRegion);
        }

        internal DataCacheItem GetCacheItem(string key, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItem = _cache.GetCacheItem(cacheKey);

                if (cacheItem == null)
                {
                    return null;
                }

                return DataFormatter.ConvertToDataCacheItem(key, _cacheName, cacheItem);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal object GetIfNewer(string key, ref DataCacheItemVersion version)
        {
            return GetIfNewer(key, ref version, defaultRegion);
        }

        internal object GetIfNewer(string key, ref DataCacheItemVersion version, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (version == null)
                {
                    throw new ArgumentNullException(nameof(version), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                var cacheItemVersion = version.itemVersion;


                return _cache.GetIfNewer<object>(cacheKey, ref cacheItemVersion);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle)
        {
            return GetAndLock(key,timeout, out lockHandle, defaultRegion);
        }


        internal object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, bool forceLock)
        {
            throw new DataCacheException("Operation Not Supported", new NotSupportedException("NCache does not support caching on non-existing keys"))
            {
                ErrorCode = DataCacheErrorCode.OperationNotSupported
            };
        }

        internal object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                LockHandle cacheLockHandle = null;
                var cacheItem = _cache.GetCacheItem(cacheKey, true, timeout, ref cacheLockHandle);

                if (cacheItem != null)
                {
                    lockHandle = new DataCacheLockHandle(cacheLockHandle);

                    return cacheItem.GetValue<object>();
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Item locked.")
                        {
                            ErrorCode = DataCacheErrorCode.ObjectLocked
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key not found.")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region not found.")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }


        internal object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, string region, bool forceLock)
        {
            throw new DataCacheException("Operation Not Supported", new NotSupportedException("NCache does not support caching on non-existing keys"))
            {
                ErrorCode = DataCacheErrorCode.OperationNotSupported
            };
        }



        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle)
        {
            return PutAndUnlock(key, value, lockHandle, defaultRegion);
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags)
        {
            return PutAndUnlock(key, value, lockHandle,tags, defaultRegion);
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }


                var cacheKey = DataFormatter.MarshalKey(key, region);
                var cacheLockHandle = lockHandle.LockHandle;

                var oldCacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (oldCacheItem != null)
                {
                    var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, _defaultTimeout);
                    var cacheItemVersion = _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);

                    return new DataCacheItemVersion(cacheItemVersion);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }

            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeout)
        {
            return PutAndUnlock(key, value, lockHandle, timeout, defaultRegion);
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                var cacheLockHandle = lockHandle.LockHandle;

                var oldCacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (oldCacheItem != null)
                {
                    var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, _defaultTimeout);

                    var cacheItemVersion = _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);

                    return new DataCacheItemVersion(cacheItemVersion);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }

            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeout, IEnumerable<DataCacheTag> tags)
        {
            return PutAndUnlock(key, value, lockHandle,timeout, tags, defaultRegion);
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeout, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                var cacheLockHandle = lockHandle.LockHandle;

                var oldCacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (oldCacheItem != null)
                {
                    var cacheItem = DataFormatter.CreateCacheItem(value, null, region, _expirable, timeout);

                    var cacheItemVersion = _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);

                    return new DataCacheItemVersion(cacheItemVersion);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }

            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (tags == null)
                {
                    throw new ArgumentNullException(nameof(tags), "Value cannot be null.");
                }

                if (tags.Count() == 0 || tags.Any(x => x == null))
                {
                    throw new ArgumentException("Either the collection passed is empty or one of the tags passed is null.", nameof(tags));
                }

                if (timeout <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeout));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                var cacheLockHandle = lockHandle.LockHandle;

                var oldCacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (oldCacheItem != null)
                {
                    var cacheItem = DataFormatter.CreateCacheItem(value, tags, region, _expirable, timeout);

                    var cacheItemVersion = _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);

                    return new DataCacheItemVersion(cacheItemVersion);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region does not exist")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }

            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region not found")
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }


        internal void Unlock(string key, DataCacheLockHandle lockHandle)
        {
            Unlock(key, lockHandle, defaultRegion);
        }

        internal void Unlock(string key, DataCacheLockHandle lockHandle, TimeSpan timeout)
        {
            Unlock(key, lockHandle,timeout, defaultRegion);
        }

        internal void Unlock(string key, DataCacheLockHandle lockHandle, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                LockHandle cacheLockHandle = lockHandle.LockHandle;
                var cacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (cacheItem != null)
                {
                    _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle.")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key not found.")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region not found.")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region does not exist.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal void Unlock(string key, DataCacheLockHandle lockHandle, TimeSpan timeOut, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (lockHandle == null)
                {
                    throw new ArgumentNullException(nameof(lockHandle), "Value cannot be null.");
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (timeOut <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeOut));
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);

                LockHandle cacheLockHandle = lockHandle.LockHandle;
                var cacheItem = _cache.GetCacheItem(cacheKey, false, new TimeSpan(0, 0, 2), ref cacheLockHandle);

                if (cacheItem != null)
                {
                    cacheItem.Expiration = new Expiration(ExpirationType.Absolute, timeOut);

                    _cache.Insert(cacheKey, cacheItem, null, cacheLockHandle, true);
                }
                else
                {
                    if (cacheLockHandle.LockId != null)
                    {
                        throw new DataCacheException("Invalid lock handle.")
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else
                    {
                        if (_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                        {
                            throw new DataCacheException("Key not found.")
                            {
                                ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                            };
                        }
                        else
                        {
                            throw new DataCacheException("Region not found.")
                            {
                                ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                            };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.ITEM_LOCKED)
                    {
                        throw new DataCacheException("Invalid lock handle", ex)
                        {
                            ErrorCode = DataCacheErrorCode.InvalidCacheLockHandle
                        };
                    }
                    else if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region does not exist.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }




        internal void ResetObjectTimeout(string key, TimeSpan timeOut)
        {
            ResetObjectTimeout(key, timeOut, defaultRegion);
        }

        internal void ResetObjectTimeout(string key, TimeSpan timeOut, string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                }

                if (timeOut <= TimeSpan.Zero)
                {
                    throw new ArgumentException("Time-out should be a positive value.", nameof(timeOut));
                }

                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var cacheKey = DataFormatter.MarshalKey(key, region);
                var cacheItem = _cache.GetCacheItem(cacheKey);

                if (cacheItem == null)
                {
                    if (!_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                    {
                        throw new DataCacheException("Region does not exist.")
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw new DataCacheException("Key not found.")
                        {
                            ErrorCode = DataCacheErrorCode.KeyDoesNotExist
                        };
                    }
                }

                cacheItem.Expiration = new Expiration(ExpirationType.Absolute, timeOut);

                _cache.Insert(cacheKey, cacheItem);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.Common.DEPENDENCY_KEY_NOT_FOUND)
                    {
                        throw new DataCacheException("Region does not exist.", ex)
                        {
                            ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                        };
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal bool CreateRegion(string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                _cache.Add(DataFormatter.MarshalRegionKey(region), region);
                return true;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    if (ex.ErrorCode == ErrorCodes.BasicCacheOperations.KEY_ALREADY_EXISTS)
                    {
                        return false;
                    }
                    else
                    {
                        throw CommonCacheExceptions(ex);
                    }
                }
                else
                {
                    throw e;
                }
            }
        }

        internal bool RemoveRegion(string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                var result = _cache.Remove(DataFormatter.MarshalRegionKey(region), out object removedItem);

                return removedItem != null;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        internal void ClearRegion(string region)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                {
                    throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                }

                if (!_cache.Contains(DataFormatter.MarshalRegionKey(region)))
                {
                    throw new DataCacheException("Region not found")
                    {
                        ErrorCode = DataCacheErrorCode.RegionDoesNotExist
                    };
                }

                _cache.Insert(DataFormatter.MarshalRegionKey(region), region);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }



        internal string GetSystemRegionName(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException(key);
            }

            return "Default_Region";
        }


        internal IEnumerable<string> GetSystemRegions()
        {
            yield return "Default_Region";
        }



        internal DataCacheNotificationDescriptor AddCacheLevelCallback(DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return GetDataCacheNotificationDescriptor(null, null, filter, callBack, CallbackType.CacheLevelCallback);
        }

        internal DataCacheNotificationDescriptor AddRegionLevelCallback(string region, DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return GetDataCacheNotificationDescriptor(null, region, filter, callBack, CallbackType.RegionSpecificCallback);
        }

        internal DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return GetDataCacheNotificationDescriptor(key, defaultRegion, filter, callBack, CallbackType.ItemSpecificCallback);
        }

        internal DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback callBack, string region)
        {
            return GetDataCacheNotificationDescriptor(key, region, filter, callBack, CallbackType.RegionSpecificItemCallback);
        }

        internal DataCacheNotificationDescriptor AddCacheLevelBulkCallback(DataCacheBulkNotificationCallback callBack)
        {
            try
            {
                if (callBack == null)
                {
                    throw new ArgumentNullException(nameof(callBack), "Value cannot be null.");
                }

                var notificationDescriptor = new DataCacheNotificationDescriptor(_cacheName);

                var bulkCacheHandler = new BulkCallbackHandler
                {
                    CacheId = _cacheName,
                    BulkCallback = callBack,
                    NotificationDescriptor = new DataCacheNotificationDescriptor(notificationDescriptor)
                };

                bulkCacheHandler.NCacheEventDescriptor = _cache.MessagingService.RegisterCacheNotification(bulkCacheHandler.OnCacheDataModification, EventType.ItemAdded | EventType.ItemRemoved | EventType.ItemUpdated, EventDataFilter.Metadata);

                var result = _callbackMap.TryAdd(notificationDescriptor.ToString(), bulkCacheHandler);

                if (!result)
                {
                    throw new Exception("Notification descriptor with given delegate ID already exists");
                }

                return notificationDescriptor;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        
        internal DataCacheNotificationDescriptor AddFailureNotificationCallback(DataCacheFailureNotificationCallback callBack)
        {
            throw new DataCacheException("Operation Not Supported", new NotSupportedException("NCache does not support failure notifications"))
            {
                ErrorCode = DataCacheErrorCode.OperationNotSupported
            };
        }

        internal void RemoveCallback(DataCacheNotificationDescriptor nd)
        {
            try
            {
                if (nd == null)
                {
                    throw new ArgumentNullException(nameof(nd), "Notification descriptor is null");
                }

                var ndString = nd.ToString();

                var result = _callbackMap.TryGetValue(ndString, out ICallBackHandler value);
                if (result)
                {
                    _cache.MessagingService.UnRegisterCacheNotification(value.NCacheEventDescriptor);
                }
                _callbackMap.TryRemove(ndString, out value);
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }


        private DataCacheNotificationDescriptor GetDataCacheNotificationDescriptor(string key, string region, DataCacheOperations filter, DataCacheNotificationCallback callback, CallbackType type)
        {
            try
            {
                if (callback == null)
                {
                    throw new ArgumentNullException(nameof(callback), "Value cannot be null.");
                }

                if (type == CallbackType.RegionSpecificCallback || type == CallbackType.RegionSpecificItemCallback)
                {
                    if (string.IsNullOrWhiteSpace(region))
                    {
                        throw new ArgumentNullException(nameof(region), "Value cannot be null.");
                    }
                }

                if (type == CallbackType.RegionSpecificItemCallback || type == CallbackType.ItemSpecificCallback)
                {
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new ArgumentNullException(nameof(key), "Value cannot be null.");
                    }
                }

                var notificationDescriptor = new DataCacheNotificationDescriptor(_cacheName);

                var callBackHandler = new CallbackHandler
                {
                    Region = region,
                    CacheId = _cacheName,
                    Callback = callback,
                    Operation = filter,
                    NotificationDescriptor = new DataCacheNotificationDescriptor(notificationDescriptor),
                    Key = key,
                    Type = type
                };

                var eventType = CallbackHandler.GetEventType(filter);

                callBackHandler.NCacheEventDescriptor = _cache.MessagingService.RegisterCacheNotification(new CacheDataNotificationCallback(callBackHandler.OnCacheDataModification), eventType, EventDataFilter.Metadata);

                var result = _callbackMap.TryAdd(notificationDescriptor.ToString(), callBackHandler);

                if (!result)
                {
                    throw new Exception("Notification descriptor with given delegate ID already exists");
                }

                return notificationDescriptor;
            }
            catch (Exception e)
            {
                CacheException ex = e as CacheException;
                if (ex != null)
                {
                    throw CommonCacheExceptions(ex);
                }
                else
                {
                    throw e;
                }
            }
        }

        private static Exception CommonCacheExceptions(CacheException ex)
        {
            if (ex.ErrorCode == ErrorCodes.Common.NO_SERVER_AVAILABLE)
            {
                return new DataCacheException("Server timeout", ex)
                {
                    ErrorCode = DataCacheErrorCode.Timeout
                };

            }
            else if (ex.ErrorCode == ErrorCodes.Common.CONNECTIVITY_LOST)
            {
                return new DataCacheException("Connectivity lost", ex)
                {
                    ErrorCode = DataCacheErrorCode.RetryLater,
                    SubStatus = DataCacheErrorSubStatus.CacheServerUnavailable
                };
            }
            else
            {
                return new DataCacheException("Undefined", ex)
                {
                    ErrorCode = DataCacheErrorCode.UndefinedError
                };
            }
        }
    }
}

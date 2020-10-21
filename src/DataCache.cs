using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Alachisoft.NCache.Web.Caching;
using Alachisoft.NCache.Web;
using Alachisoft.NCache.Runtime.Caching;
using Alachisoft.NCache.Data.Caching.Handler;

namespace Alachisoft.NCache.Data.Caching
{
    /// <summary>
    /// flags that specify the type of removal operation
    /// </summary>
    [Flags]
    internal enum RemoveOperation
    {
        VersionBased = 1,
        KeyBased = 2,
        LockBased = 3,
    }

    /// <summary>
    /// The primary object that is used by cache-enabled applications for storing and retrieving objects from the cache cluster. 
    /// An instance of this object is referred to as the cache client. 
    /// </summary>
   
    public class DataCache
    {
        #region [           Constructor           ]
        internal DataCache(Alachisoft.NCache.Web.Caching.Cache nCache, string cacheName)
        {
            this._NCache = nCache;
            this._CacheName = cacheName;
            _cache = new CacheHandler(_NCache);
            _formatter = new DataFormatter();
            _callabackMap = new Dictionary<DataCacheNotificationDescriptor, CallbackHandler>();
        }
        #endregion

        #region [            Cache.Add            ]
        /// <summary>
        /// Adds an object to the cache.
        /// </summary>
        /// <param name="key">A unique value that is used to store and retrieve the object from cache.</param>
        /// <param name="value">The object saved to the cache cluster.</param>
        /// <returns>A Alachisoft.NCache.Data.Caching.CacheItemVersion object that represents the version
        /// of the object saved to the cache under the key value.
        /// </returns>
        public DataCacheItemVersion Add(string key, object value)
        {
            return Add(key, value, null, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Adds an object to the cache. Provides the option to save the object to a
        /// region.
        /// </summary>
        /// <param name="region">The name of the region to which to save the object. 
        /// </param>
        /// <param name="key">A unique value that is used to store and retrieve the object from cache.</param>
        /// <param name="value">The object saved to the cache cluster.</param>
        /// <returns>A Alachisoft.NCache.Data.Caching.CacheItemVersion object that represents the version
        /// of the object saved to the cache under the key value.</returns>
        public DataCacheItemVersion Add( string key, object value,string region)
        {
            return Add(key, value, null, TimeSpan.Zero, region);
        }

        /// <summary>
        /// Adds an object to the cache. Provides the option to save the object to a
        /// region and also specify an object time-out.
        /// </summary>
        /// <param name="region">The name of the region to which to save the object. </param>
        /// <param name="key">A unique value that is used to store and retrieve the object from cache.</param>
        /// <param name="value">The object saved to the cache cluster.</param>
        /// <param name="timeOut">he duration of time that the object resides in the cache before expiration.</param>
        /// <returns>A Alachisoft.NCache.Data.Caching.CacheItemVersion object that represents the version
        /// of the object saved to the cache under the key value.</returns>
        public DataCacheItemVersion Add( string key, object value, TimeSpan timeOut,string region)
        {
            return Add(key, value, null, timeOut, region);
        }

        /// <summary>
        /// Adds an object to the cache. Provides the option to save the object to a
        /// region and also specify object tags.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            return Add(key, value, tags, TimeSpan.Zero, region);
        }

        /// <summary>
        /// Adds an object to the cache. Provides the option to save the object to a
        /// region and also specify an object time-out and tags.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags, TimeSpan timeOut, string region)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            if (value == null)
            {
                throw new DataCacheException("Operation Failed");
            }
           return _cache.Add(key, value, tags, timeOut, region);
        }

        /// <summary>
        ///Adds an object to the cache; with tags and a timeout period specified.  
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeOut, IEnumerable<DataCacheTag> tags)
        {
            return Add(key, value, tags, timeOut, null);
        }

        /// <summary>
        ///Adds an object to the cache with tags specified. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags)
        {
            return Add(key, value, tags, TimeSpan.Zero, null);
        }

        /// <summary>
        ///Adds an object to the cache with timeout period specified. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeOut)
        {
            return Add(key, value, null, timeOut, null);
        }
        #endregion

        #region [           Cache.Clear           ]
        /// <summary>
        /// Clears the contents of a region
        /// </summary>
        /// <param name="region"></param>
        public void ClearRegion(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Region Does not exist");
            }
            else
            {
                _cache.RemoveRegionData(region,CallbackType.ClearRegion);
            }
        } 
        #endregion

        #region [       Cache.CreateRegion        ]
        /// <summary>
        /// Creates a region
        /// </summary>
        /// <param name="region"></param>
        /// <param name="evictable"></param>
        /// <returns></returns>
        public bool CreateRegion(string region)
        {
            _cache.CreateRegion(region);
            return true;
        }
        #endregion

        #region [           Cache.Get             ]
        /// <summary>
        /// Gets an object by specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key)
        {
            DataCacheItemVersion version = null;
            return Get(key, out version,null);
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string key, string region)
        {
            DataCacheItemVersion version = null;
            return Get(key, out version, region);
        }

        /// <summary>
        /// Gets an object from cache using the specified key. 
        /// You may also provide the version to get the specific version of a key, 
        /// if that version is still the most current in the cache. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object Get(string key, out DataCacheItemVersion Version)
        {
            return Get(key, out Version, null);
        }

        /// <summary>
        /// Gets an object from the specified region by using the specified key. 
        /// You may also provide the version to obtain the specific version of a key, 
        /// if that version is still the most current in the region. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object Get(string key, out DataCacheItemVersion version, string region)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Inavlid key");
            }
            return _cache.Get(key, out version, region);
        } 
        #endregion

        #region [          Cache.GetObject Operations         ]
        /// <summary>
        /// Gets a list of all cached objects in the specified region that 
        /// have the specified tag. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(DataCacheTag tag, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Inavlid Region");
            }
            return _cache.GetObjectsByTag(region, tag);
         }

        /// <summary>
        /// Gets a list of all cached objects in the specified region that 
        /// have any of  the specified tags. 
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<DataCacheTag> tags, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Inavlid Region");
            }
            return _cache.GetObjectsByAnyTag(tags,region);
        }

        /// <summary>
        /// Gets a list of all cached objects in the specified region that 
        /// have all  listed tags. 
        /// </summary>
        /// <param name="tags"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<DataCacheTag> tags, string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Inavlid Region");
            }
            return _cache.GetObjectsByAllTags(tags,region);
        }
        
        /// <summary>
        /// returns a list of all the objects in a region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            if (string.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Inavlid Region");
            }
            return _cache.GetObjectsInRegion(region);
        }

        /// <summary>
        /// returns the objects from a specified region, whose keys have been provided
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys, string region)
        {
            return _cache.GetBulk(keys, region);
        }

        public IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys)
        {
            return BulkGet(keys, null);
        }
        #endregion
               
        #region [        Cache.GetAndLock         ]
        /// <summary>
        /// Returns and locks the cached object.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockTimeOut"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public object GetAndLock(string key, TimeSpan lockTimeOut, out DataCacheLockHandle appLockHandle)
        {
            return GetAndLock(key, lockTimeOut, out appLockHandle, null, false);
        }

        /// <summary>
        /// Returns and locks the cached object from a region
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="lockTimeOut"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public object GetAndLock(string key, TimeSpan lockTimeOut, out DataCacheLockHandle appLockHandle, string region)
        {
            return GetAndLock(key, lockTimeOut, out appLockHandle, region, false);
        }

        /// <summary>
        /// Returns and locks the cached object from a region 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeOut"></param>
        /// <param name="appLockHandle"></param>
        /// <param name="region"></param>
        /// <param name="forceLock"></param>
        /// <returns></returns>
        public object GetAndLock(string key, TimeSpan timeOut, out DataCacheLockHandle appLockHandle,string region, bool forceLock)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            return _cache.GetLock(key, timeOut, out appLockHandle, region, forceLock);
        }

        public object GetAndLock(string key, TimeSpan timeOut, out DataCacheLockHandle appLockHandle, bool forceLock)
        {
            return GetAndLock(key,timeOut,out appLockHandle,null,forceLock);
        }
        #endregion

        #region [       Cache.GetCacheItem        ]
        /// <summary>
        /// Gets a CacheItem object to retrieve all information associated with your cached object in the cluster
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataCacheItem GetCacheItem(string key)
        {
            try
            {
                return GetCacheItem(key, null);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("GetCacheItem opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets a CacheItem object to retrieve all information associated with the cached object in the cluster
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public DataCacheItem GetCacheItem(string key, string region)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            try
            {
                DataCacheItem item = _cache.GetCacheItem(key, region, _CacheName);
                if (item is DataCacheItem)
                {
                    return item;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region [        Cache.GetIfNewer         ]
        /// <summary>
        /// Gets an object from the cache, but only if a newer version of the object occurs in the cache. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object GetIfNewer(string key, ref DataCacheItemVersion version)
        {
            try
            {
                return  GetIfNewer( key, ref version, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("GetIfNewer opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets an object from the specified region, but only if a newer version of the object occurs in the region. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public object GetIfNewer(string key, ref DataCacheItemVersion version, string region)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            try
            {
                return _cache.GetIfNewer(key,ref version, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("GetIfNewer opertaion failed." + ex.Message, ex);
            }
        } 
        #endregion

        #region [   Cache.getSystemRegions  ]
        /// <summary>
        /// Provides Region name against a specific key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemRegionName(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            return _cache.GetSystemRegionName(key);
        }

        /// <summary>
        /// returns List of Default Regions
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetSystemRegions()
        {
            string name= "_Default";
            yield return name;
        }
        #endregion

        #region [           Cache.Put             ]
        /// <summary>
        /// Adds a new object to the cache or replaces one if it is already located in the cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value)
        {
            try
            {
                return Put(key, value, null, TimeSpan.Zero, null, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates the specified cache object if it has the specified version ID
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion)
        {
            try
            {
                return Put(key, value, oldVersion, TimeSpan.Zero, null, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// This overload allows you to save your object to a region. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, string region)
        {
            try
            {
                return Put(key, value, null, TimeSpan.Zero, null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// Also updates the expiration value. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut) 
        {
            try
            {
                return Put(key, value, null, timeOut, null, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// Also updates the associated tags. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags) 
        {
            try
            {
                return Put(key, value, null, TimeSpan.Zero, tags, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// Also updates the tags. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                return Put(key, value, oldVersion, TimeSpan.Zero, tags, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// Also updates the expiration value. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, TimeSpan timeOut)
        {
            try
            {
                return Put(key, value, oldVersion, timeOut, null, null);

            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// Also updates the expiration value and associated tags. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                return Put(key, value, null, timeOut, tags, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// This overload allows you to save your object to a region and supports optimistic concurrency by accepting version information as a parameter.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string region, string key, object value, DataCacheItemVersion oldVersion)
        {
            try
            {
                return Put(key, value, oldVersion, TimeSpan.Zero, null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// This overload allows you to save your object to a region and specify an object time-out that overrides the default settings for the cache. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put( string key, object value, TimeSpan timeOut,string region)
        {
            try
            {
                return Put(key, value, null, timeOut, null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed."+ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache in a 
        /// particular region. Also updates the associated tags. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            //ignoring the Tags
            try
            {
                return Put(key, value, null, TimeSpan.Zero, tags, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        ///Adds a new object to the cache or replaces one if it already occurs in the cache in a particular region. 
        /// Also updates the associated tags.  
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <param name="oldVersion"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                return Put(key, value, oldVersion, TimeSpan.Zero, tags, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache in a particular region. 
        /// Also updates the associated tags and the tiemout value. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                return Put(key, value, null, timeOut, tags, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// This overload allows you to save your object to a region and specify an object expiration that overrides the default settings for the cache. 
        /// This method also supports optimistic concurrency by accepting version information as a parameter. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, TimeSpan timeOut, string region)
        {
            try
            {
                return Put(key, value, oldVersion, timeOut, null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed."+ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache. 
        /// This overload allows you to save your object to a region and specify an object expiration that overrides the default settings for the cache. 
        /// This method also supports optimistic concurrency by accepting version information as a parameter. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="oldVersion"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, string region)
        {
            try
            {
                return Put(key, value, oldVersion, TimeSpan.Zero, null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Put opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Adds a new object to the cache or replaces one if it already occurs in the cache in a particular region. 
        /// Also updates the associated tags and the associated timeout value. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="tags"></param>
        /// <param name="oldVersion"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, TimeSpan timeOut, IEnumerable<DataCacheTag> tags, string region)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid key");
            }
            if(value == null )
            {
                throw new DataCacheException("Invalid Value");
            }
            
            DataCacheItemVersion itemVersion= _cache.Put(key,value,oldVersion,timeOut,tags,region);
            if (itemVersion is DataCacheItemVersion)
            {
                return itemVersion;
            }
            else
                throw new DataCacheException("Operation failure");
        } 
        #endregion

        #region [        Cache.PutAndLock         ]
        /// <summary>
        /// Updates the locked object and then releases the lock. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="timeOut"></param>
        /// <param name="lockHandle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, TimeSpan timeOut, DataCacheLockHandle lockHandle)
        {
            try
            {
                return PutAndUnlock(key,value,lockHandle,timeOut, null,null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndLock opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates the locked object and then releases the lock in a particular region. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lockHandle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags, string region)
        {
            try
            {
                return PutAndUnlock(key,value,lockHandle, TimeSpan.Zero, tags,region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndUnLock opertaion failed." + ex.Message, ex);
            }
        }

        ///<summary>
        /// Updates the locked object and then releases the lock. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
         ///</summary>
         ///<param name="region"></param>
         ///<param name="key"></param>
         ///<param name="value"></param>
         ///<param name="timeOut"></param>
         ///<param name="lockHandle"></param>
         ///<param name="tags"></param>
         ///<returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut, IEnumerable<DataCacheTag> tags, string region)
        {
            if(string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid Key");
            }
            if(value == null )
            {
                throw new DataCacheException("Invalid Value");
            }
            DataCacheItemVersion itemVersion= _cache.PutAndUnlock(key,value,lockHandle,timeOut,tags,region);
            if(itemVersion is DataCacheItemVersion)
            {
                return itemVersion;
            }
            else
                throw new DataCacheException("Invalid LockHandle");
        }

        /// <summary>
        /// Updates the locked object and then releases the lock. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle)
        {
            try
            {
               return PutAndUnlock(key,value, lockHandle, TimeSpan.Zero, null,null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndLock opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates the locked object and then releases the lock in a particular region. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lockHandle"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, string region) 
        {
            try
            {
                return PutAndUnlock(key, value, lockHandle, TimeSpan.Zero,null, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndUnLock opertaion failed." + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Updates the locked object and then releases the lock. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lockHandle"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags)
        {
            try
            {
                return PutAndUnlock(key, value, lockHandle, TimeSpan.Zero, tags, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndLock opertaion failed." + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Updates the locked object and then releases the lock in a particular region. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="lockHandle"></param>
        /// <param name="timeOut"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut, string region) 
        {
            try
            {
                 return PutAndUnlock(key, value,lockHandle,timeOut,null,region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("PutAndUnLock opertaion failed." + ex.Message, ex);
            }
        }
        #endregion

        #region [         Cache.Remove            ]
        /// <summary>
        /// Removes a cached object from the cache. 
        /// </summary>
        /// <param name="key"></param>
        public bool Remove(string key)
        {
            try
            {
                return Remove(key, null, null, null, RemoveOperation.KeyBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes a cached object from the cache. 
        /// This overload supports optimistic concurrency by accepting a CacheItemVersion object.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="version"></param>
        public bool Remove(string key, DataCacheItemVersion version)
        {
            try
            {
                return Remove(key, null, null, version, RemoveOperation.VersionBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes a cached object from the cache. 
        /// This overload also allows you to specify the region. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        public bool Remove( string key,string region)
        {
            try
            {
                return Remove(key, null, region, null, RemoveOperation.KeyBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// Removes a cached object from the cache. 
        /// This overload also allows you to specify the region and supports optimistic concurrency by accepting a CacheItemVersion object. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="version"></param>
        public bool Remove( string key, DataCacheItemVersion version,string region)
        {
            try
            {
                return Remove(key, null, region, version, RemoveOperation.VersionBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes a cached object from the cache. 
        /// This overload also allows you to specify the lockHandle. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public bool Remove(string key, DataCacheLockHandle lockHandle)
        {
            try
            {
                return Remove(key, lockHandle, null, null, RemoveOperation.LockBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes a cached object from the cache. 
        /// This overload also allows you to specify the region and LockHandle. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool Remove(string key, DataCacheLockHandle lockHandle, string region)
        {
            try
            {
                return Remove(key, lockHandle, region, null, RemoveOperation.LockBased);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception ex)
            {
                throw new DataCacheException("Remove opertaion failed." + ex.Message, ex);
            }
        }

        /// <summary>
        /// Deletes a region. All cached objects inside the region are also removed.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        public bool RemoveRegion(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new DataCacheException("Region Does not exist");
            }
            else
            {
                try
                {
                    return _cache.RemoveRegionData(region,CallbackType.RemoveRegion);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        #region [    Cache.ResetObjectTimeout     ]
        /// <summary>
        /// Resets the object time-out, defining how long objects reside in the cache before expiring. 
        /// The value specified for the object overrides the default settings for the cache. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newTimeout"></param>
        /// <returns></returns>
        public bool ResetObjectTimeout(string key, TimeSpan newTimeout)
        {
            try
            {
               return ResetObjectTimeout(key, newTimeout,null);
            }
            catch (DataCacheException )
            {
                return false;
            }
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// Resets the object time-out, defining how long objects reside in the cache before expiring. 
        /// The value specified for the object overrides the default settings for the cache. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="newTimeout"></param>
        /// <returns></returns>
        public bool ResetObjectTimeout(string key, TimeSpan newTimeout, string region)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("key cannot be null");
            }
            try
            {
                if (newTimeout != null)
                {
                    return _cache.ResetObjectTimeout(key, newTimeout, region);
                }
                else
                {
                    return _cache.ResetObjectTimeout(key, TimeSpan.Zero, region);
                }
            }
            catch (DataCacheException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        #region [   Call back operations    ]
        /// <summary>
        /// Sets Cache level notification for operations occuring in all regions
        /// </summary>
        /// <param name="clientCallback"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddCacheLevelBulkCallback(DataCacheBulkNotificationCallback clientCallback)
        {
            // Implementation not provided due to lack of compatibility provided by NCache
            return null;
        }

        /// <summary>
        /// Sets Cache level notification for operations occuring in all regions
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="clientCallback"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddCacheLevelCallback(DataCacheOperations filter, DataCacheNotificationCallback clientCallback)
        {
            DataCacheNotificationDescriptor descriptor = CreateNotificationDescriptor();
            CallbackHandler callback = new CallbackHandler();
            
            callback.Region = null;
            callback.CacheId = _CacheName;
            callback.Callback = clientCallback;
            callback.Operation = filter;
            callback.NotificationDescriptor = descriptor;
            callback.Type = CallbackType.CacheLevelCallback;
            callback.Key = null;
            
            if((filter & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
            {
                callback.NCacheEventDescriptor =  _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemAdded), Runtime.Events.EventType.ItemAdded, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if((filter & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
            {
                callback.NCacheEventDescriptor = _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemUpdated), Runtime.Events.EventType.ItemUpdated, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
            {
                callback.NCacheEventDescriptor = _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemRemoved), Runtime.Events.EventType.ItemRemoved, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if ((filter & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion)
            {
                _cache.RegisterRegionCallBack(CallbackType.ClearRegion);
            }
            if ((filter & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion)
            {
                _cache.RegisterRegionCallBack(CallbackType.AddRegion);
            }
            if ((filter & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion)
            {
                _cache.RegisterRegionCallBack(CallbackType.RemoveRegion);
            }
          
            _callabackMap.Add(descriptor,callback);

            return descriptor;
        }

        /// <summary>
        /// Removes previously registered cache notifications
        /// </summary>
        /// <param name="notificationDiscriptor"></param>
        public void RemoveCallback(DataCacheNotificationDescriptor notificationDiscriptor)
        {
           if(_callabackMap.ContainsKey(notificationDiscriptor))
           {
                CallbackHandler _callback= _callabackMap[notificationDiscriptor];
               
                if (_callback.Type == CallbackType.CacheLevelCallback)
                {
                    if ((_callback.Operation) == DataCacheOperations.AddItem || (_callback.Operation) == DataCacheOperations.RemoveItem || (_callback.Operation) == DataCacheOperations.ReplaceItem)
                    {
                        _NCache.UnRegisterCacheNotification(_callback.NCacheEventDescriptor);
                    }

                    if ((_callback.Operation) == DataCacheOperations.ClearRegion)
                    {
                        _cache.UnRegisterRegionCallBack(CallbackType.ClearRegion);
                    }
                    if ((_callback.Operation) == DataCacheOperations.CreateRegion)
                    {
                        _cache.UnRegisterRegionCallBack(CallbackType.AddRegion);
                    }
                    if ((_callback.Operation) == DataCacheOperations.RemoveRegion)
                    {
                        _cache.UnRegisterRegionCallBack(CallbackType.RemoveRegion);
                    }
                }
                else if (_callback.Type == CallbackType.ItemSpecificCallback)
                {
                    if (_callback.Region == "null")
                    {
                        _NCache.UnRegisterKeyNotificationCallback(_formatter.MarshalKey(_callback.Key), _callback.OnSpecificItemUpdate, _callback.OnSpecificItemRemoved);
                    }
                }
                else if (_callback.Type == CallbackType.RegionSpecificItemCallback)
                {
                    _NCache.UnRegisterKeyNotificationCallback(_formatter.MarshalKey(_callback.Key,_callback.Region), _callback.OnSpecificItemUpdate, _callback.OnSpecificItemRemoved);
                }
           }
        }

        /// <summary>
        /// Sets notification call back for failures
        /// </summary>
        /// <param name="failureCallback"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddFailureNotificationCallback(DataCacheFailureNotificationCallback failureCallback)
        {
            //: Not implemented due to lack of support provided by NCache
            return null;
        }

        /// <summary>
        /// Sets item level call back 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filter"></param>
        /// <param name="clientCallback"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback clientCallback) 
        {
            DataCacheNotificationDescriptor descriptor = CreateNotificationDescriptor();
                              
            CallbackHandler callback = new CallbackHandler();

            callback.Region = null;
            callback.CacheId = _CacheName;
            callback.Callback = clientCallback;
            callback.Operation = filter;
            callback.NotificationDescriptor = descriptor;
            callback.Type = CallbackType.ItemSpecificCallback;
            callback.Key = key;

            if ((filter & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
            {
                if ((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                {
                    _NCache.RegisterKeyNotificationCallback(_formatter.MarshalKey(key), callback.OnSpecificItemUpdate, callback.OnSpecificItemRemoved);
                }
                else
                {
                    _NCache.RegisterKeyNotificationCallback(_formatter.MarshalKey(key), callback.OnSpecificItemUpdate, null);
                }
            }
            else if ((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
            {
                _NCache.RegisterKeyNotificationCallback(_formatter.MarshalKey(key), null, callback.OnSpecificItemRemoved);
            }
          
            _callabackMap.Add(descriptor, callback);

            return descriptor;
        }

        /// <summary>
        /// Sets item specific call back against a  given region
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filter"></param>
        /// <param name="clientCallback"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback clientCallback, string region)
        {
            DataCacheNotificationDescriptor descriptor = CreateNotificationDescriptor();
                           
            CallbackHandler callback = new CallbackHandler();
           
            callback.Region = region;
            callback.CacheId = _CacheName;
            callback.Callback = clientCallback;
            callback.Operation = filter;
            callback.NotificationDescriptor = descriptor;
            callback.Type = CallbackType.RegionSpecificItemCallback;
            callback.Key = key;
                    
            if ((filter & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
            {
                if ((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                {
                    _NCache.RegisterKeyNotificationCallback(_formatter.MarshalKey(key,region), callback.OnSpecificItemUpdate, callback.OnSpecificItemRemoved);
                }
                else
                {
                    _NCache.RegisterKeyNotificationCallback(key, callback.OnSpecificItemUpdate, null);
                }
            }
            else if ((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
            {
                _NCache.RegisterKeyNotificationCallback(_formatter.MarshalKey(key, region), null, callback.OnSpecificItemRemoved);
            }

            _callabackMap.Add(descriptor, callback);
            return descriptor;
        }

        /// <summary>
        /// Sets region level call back 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="filter"></param>
        /// <param name="clientCallback"></param>
        /// <returns></returns>
        public DataCacheNotificationDescriptor AddRegionLevelCallback(string region, DataCacheOperations filter, DataCacheNotificationCallback clientCallback)
        {
            DataCacheNotificationDescriptor descriptor = CreateNotificationDescriptor();

            CallbackHandler callback = new CallbackHandler();
            callback.Region = region;
            callback.CacheId = _CacheName;
            callback.Callback = clientCallback;
            callback.Operation = filter;
            callback.NotificationDescriptor = descriptor;
            callback.Type = CallbackType.RegionSpecificCallback;
            callback.Key = null;

            if ((filter & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
            {
                callback.NCacheEventDescriptor = _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemAdded), Runtime.Events.EventType.ItemAdded, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if ((filter & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
            {
                callback.NCacheEventDescriptor = _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemUpdated), Runtime.Events.EventType.ItemUpdated, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if ((filter & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
            {
                callback.NCacheEventDescriptor = _NCache.RegisterCacheNotification(callback.GetNCacheNotificationCallback(clientCallback, Runtime.Events.EventType.ItemRemoved), Runtime.Events.EventType.ItemRemoved, Runtime.Events.EventDataFilter.DataWithMetadata);
            }
            if ((filter & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion)
            {
                //
                //NCache.CacheCleared += new CacheClearedCallback(callback.onRegionClear);
            }
            if ((filter & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion)
            {
                //: No support provided yet due to lack of compatibility by NCache
            }
            if ((filter & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion)
            {
                // No support provided yet due to lack of compatibility by NCache
            }

            _callabackMap.Add(descriptor, callback);
            return descriptor;
           
        }
        #endregion
        
        #region [          Cache.Unlock           ]
        /// <summary>
        /// Releases locked objects. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public bool Unlock(string key, DataCacheLockHandle appLockHandle)
        {
            try
            {
                return Unlock(key, appLockHandle, TimeSpan.Zero, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Releases locked objects. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object. 
        /// Additionally, this overload allows you to specify an object time-out that overrides the default settings for the cache. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Unlock(string key, DataCacheLockHandle appLockHandle, TimeSpan timeOut)
        {
            try
            {
                return Unlock(key, appLockHandle, timeOut, null);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Releases locked objects. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <returns></returns>
        public bool Unlock( string key, DataCacheLockHandle appLockHandle,string region)
        {
            try
            {
                return Unlock(key, appLockHandle, TimeSpan.Zero, region);
            }
            catch (ArgumentNullException ex)
            {
                throw new DataCacheException("Argument cannot be null", ex);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Releases locked objects. 
        /// This method supports pessimistic concurrency by making sure that the appropriate LockHandle is used for unlocking the object. Additionally, this overload allows you to specify an object time-out that overrides the default settings for the cache. 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="lockHandle"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public bool Unlock(string key, DataCacheLockHandle appLockHandle, TimeSpan timeOut,string region)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Invalid Key");
            }
            try
            {
                return _cache.Unlock(key, appLockHandle, timeOut, region);
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        #region [        Public Properties        ]
        /// <summary>
        /// Provides access of Cached Data by using array notation
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            
            get
            {
                String _key = _formatter.MarshalKey(key);
                return _NCache.Get(_key);
            }
            set
            {
                try
                {
                    String _key = _formatter.MarshalKey(key);
                    if (value == null)
                    {
                        _NCache.Remove(_key);
                    }
                    else
                    {
                        _NCache.Insert(_key, value);
                    }
                }
                catch (Exception ex)
                {
                    throw new DataCacheException("Opertaion failed." + ex.Message, ex);
                }
            }
        }
        #endregion

        #region [       Internal Properties       ]
        internal Alachisoft.NCache.Web.Caching.Cache _NCache { get; set; }
        internal string _CacheName { get; set; }
        #endregion

        #region [         Private Methods         ]
        private bool Remove(string key, DataCacheLockHandle lockHandle, string region, DataCacheItemVersion version,RemoveOperation opCode)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new DataCacheException("Key Cannot be Null");
            }
            else
            {
               return _cache.Remove(key, lockHandle, region, version, opCode);
            }
        }
        private DataCacheNotificationDescriptor CreateNotificationDescriptor()
        {
             DataCacheNotificationDescriptor descriptor = new DataCacheNotificationDescriptor();
            descriptor.CacheName= _CacheName;
            descriptor.DelegateId= System.Threading.Interlocked.Increment(ref _delegateID);
            return descriptor;
        }
        #endregion

        #region [   private members ]
        private CacheHandler _cache;
        private DataFormatter _formatter;
        private long _delegateID;
        private Dictionary<DataCacheNotificationDescriptor, CallbackHandler> _callabackMap;
        #endregion
    }
}

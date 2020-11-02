using Alachisoft.NCache.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Alachisoft.NCache.Data.Caching
{
    public class DataCache
    {
        private readonly CacheHandler _cacheHandler;

        internal DataCache(string cacheName, DataCacheFactoryConfiguration cacheConfiguration, bool expirable, TimeSpan defaultTTL)
        {
            if (string.IsNullOrWhiteSpace(cacheName))
            {
                throw new ArgumentNullException(nameof(cacheName), "Value cannot be null");
            }

            if (cacheConfiguration == null)
            {
                _cacheHandler = new CacheHandler(cacheName, null, expirable, defaultTTL);
            }
            else
            {
                CacheConnectionOptions connectionOptions = new CacheConnectionOptions
                {
                    ConnectionTimeout = cacheConfiguration.ChannelOpenTimeout,
                    ClientRequestTimeOut = cacheConfiguration.RequestTimeout
                };

                if (cacheConfiguration.Servers != null && cacheConfiguration.Servers.Count() > 0)
                {
                    var serverList = new List<ServerInfo>(cacheConfiguration.Servers.Count());

                    foreach (var server in cacheConfiguration.Servers)
                    {
                        serverList.Add(new ServerInfo(server.HostName, server.CachePort));
                    }

                    connectionOptions.ServerList = serverList;
                }

                _cacheHandler = new CacheHandler(cacheName, connectionOptions, expirable, defaultTTL);
            }
        }

        public object this[string key]
        {
            get
            {
                return _cacheHandler.Get(key);
            }
            set
            {
                if (value == null)
                {
                    _cacheHandler.Remove(key);
                }
                else
                {
                    _cacheHandler.Put(key, value);
                }
            }
        }
        public object Get(string key)
        {
            return _cacheHandler.Get(key);
        }
        public object Get(string key, string region)
        {
            return _cacheHandler.Get(key, region);
        }
        public object Get(string key, out DataCacheItemVersion version)
        {
            return _cacheHandler.Get(key, out version);
        }
        public object Get(string key, out DataCacheItemVersion version, string region)
        {
            return _cacheHandler.Get(key, out version, region);
        }



        public DataCacheItemVersion Add(string key, object value)
        {
            return _cacheHandler.Add(key, value);
        }
        public DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Add(key, value, tags);
        }
        public DataCacheItemVersion Add(string key, object value, string region)
        {
            return _cacheHandler.Add(key, value, region);
        }
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeout)
        {
            return _cacheHandler.Add(key, value, timeout);
        }
        public DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Add(key, value, tags, region);
        }
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Add(key, value, timeout, tags);
        }
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeout, string region)
        {
            return _cacheHandler.Add(key, value, timeout, region);
        }
        public DataCacheItemVersion Add(string key, object value, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Add(key, value, timeout, tags, region);
        }



        public DataCacheItemVersion Put(string key, object value)
        {
            return _cacheHandler.Put(key, value);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version)
        {
            return _cacheHandler.Put(key, value, version);
        }
        public DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Put(key, value, tags);
        }
        public DataCacheItemVersion Put(string key, object value, string region)
        {
            return _cacheHandler.Put(key, value, region);
        }
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut)
        {
            return _cacheHandler.Put(key, value, timeOut);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Put(key, value, version, tags);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, string region)
        {
            return _cacheHandler.Put(key, value, version, region);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeOut)
        {
            return _cacheHandler.Put(key, value, version, timeOut);
        }
        public DataCacheItemVersion Put(string key, object value, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Put(key, value, tags, region);
        }
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Put(key, value, timeOut, tags);
        }
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut, string region)
        {
            return _cacheHandler.Put(key, value, timeOut, region);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Put(key, value, version, tags, region);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeOut, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.Put(key, value, version, timeOut, tags);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeOut, string region)
        {
            return _cacheHandler.Put(key, value, version, timeOut, region);
        }
        public DataCacheItemVersion Put(string key, object value, TimeSpan timeOut, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Put(key, value, timeOut, tags, region);
        }
        public DataCacheItemVersion Put(string key, object value, DataCacheItemVersion version, TimeSpan timeout, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.Put(key, value, version, timeout, tags, region);
        }



        public bool Remove(string key)
        {
            return _cacheHandler.Remove(key);
        }
        public bool Remove(string key, DataCacheItemVersion version)
        {
            return _cacheHandler.Remove(key, version);
        }
        public bool Remove(string key, DataCacheLockHandle lockHandle)
        {
            return _cacheHandler.Remove(key, lockHandle);
        }
        public bool Remove(string key, string region)
        {
            return _cacheHandler.Remove(key, region);
        }
        public bool Remove(string key, DataCacheItemVersion version, string region)
        {
            return _cacheHandler.Remove(key, version, region);
        }
        public bool Remove(string key, DataCacheLockHandle lockHandle, string region)
        {
            return _cacheHandler.Remove(key, lockHandle, region);
        }



        public IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(DataCacheTag tag, string region)
        {
            return _cacheHandler.GetObjectsByTag(tag, region);
        }
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.GetObjectsByAnyTag(tags, region);
        }
        public IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.GetObjectsByAllTags(tags, region);
        }
        public IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region)
        {
            return _cacheHandler.GetObjectsInRegion(region);
        }


        public IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys)
        {
            return _cacheHandler.BulkGet(keys);
        }
        public IEnumerable<KeyValuePair<string, object>> BulkGet(IEnumerable<string> keys, string region)
        {
            return _cacheHandler.BulkGet(keys, region);
        }



        public DataCacheItem GetCacheItem(string key)
        {
            return _cacheHandler.GetCacheItem(key);
        }
        public DataCacheItem GetCacheItem(string key, string region)
        {
            return _cacheHandler.GetCacheItem(key, region);
        }



        public object GetIfNewer(string key, ref DataCacheItemVersion version)
        {
            return _cacheHandler.GetIfNewer(key, ref version);
        }
        public object GetIfNewer(string key, ref DataCacheItemVersion version, string region)
        {
            return _cacheHandler.GetIfNewer(key, ref version, region);
        }



        public object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle)
        {
            return _cacheHandler.GetAndLock(key, timeout, out lockHandle);
        }
        [Obsolete("NCache does not support locking on non-existing objects", true)]
        public object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, bool forceLock)
        {
            throw new NotSupportedException("NCache does not support locking on non-existing objects");
        }
        public object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, string region)
        {
            return _cacheHandler.GetAndLock(key, timeout, out lockHandle, region);
        }
        [Obsolete("NCache does not support locking on non-existing objects", true)]
        public object GetAndLock(string key, TimeSpan timeout, out DataCacheLockHandle lockHandle, string region, bool forceLock)
        {
            throw new NotSupportedException("NCache does not support locking on non-existing objects");
        }



        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, tags);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, string region)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, region);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, timeOut);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, tags, region);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut, IEnumerable<DataCacheTag> tags)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, timeOut, tags);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut, string region)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, timeOut, region);
        }
        public DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut, IEnumerable<DataCacheTag> tags, string region)
        {
            return _cacheHandler.PutAndUnlock(key, value, lockHandle, timeOut, tags, region);
        }



        public void Unlock(string key, DataCacheLockHandle lockHandle)
        {
            _cacheHandler.Unlock(key, lockHandle);
        }
        public void Unlock(string key, DataCacheLockHandle lockHandle, TimeSpan timeOut)
        {
            _cacheHandler.Unlock(key, lockHandle, timeOut);
        }
        public void Unlock(string key, DataCacheLockHandle lockHandle, string region)
        {
            _cacheHandler.Unlock(key, lockHandle, region);
        }
        public void Unlock(string key, DataCacheLockHandle lockHandle, TimeSpan timeOut, string region)
        {
            _cacheHandler.Unlock(key, lockHandle, timeOut, region);
        }



        public void ResetObjectTimeout(string key, TimeSpan timeOut)
        {
            _cacheHandler.ResetObjectTimeout(key, timeOut);
        }
        public void ResetObjectTimeout(string key, TimeSpan timeOut, string region)
        {
            _cacheHandler.ResetObjectTimeout(key, timeOut, region);
        }



        public bool CreateRegion(string region)
        {
            return _cacheHandler.CreateRegion(region);
        }
        public bool RemoveRegion(string region)
        {
            return _cacheHandler.RemoveRegion(region);
        }
        public void ClearRegion(string region)
        {
            _cacheHandler.ClearRegion(region);
        }


        public string GetSystemRegionName(string key)
        {
            return _cacheHandler.GetSystemRegionName(key);
        }
        public IEnumerable<string> GetSystemRegions()
        {
            return _cacheHandler.GetSystemRegions();
        }



        public DataCacheNotificationDescriptor AddCacheLevelCallback(DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return _cacheHandler.AddCacheLevelCallback(filter, callBack);
        }
        public DataCacheNotificationDescriptor AddRegionLevelCallback(string region, DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return _cacheHandler.AddRegionLevelCallback(region, filter, callBack);
        }
        public DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback callBack)
        {
            return _cacheHandler.AddItemLevelCallback(key, filter, callBack);
        }
        public DataCacheNotificationDescriptor AddItemLevelCallback(string key, DataCacheOperations filter, DataCacheNotificationCallback callBack, string region)
        {
            return _cacheHandler.AddItemLevelCallback(key, filter, callBack, region);
        }
        public DataCacheNotificationDescriptor AddCacheLevelBulkCallback(DataCacheBulkNotificationCallback callBack)
        {
            return _cacheHandler.AddCacheLevelBulkCallback(callBack);
        }

        [Obsolete("NCache does not support failure notifications", true)]
        internal DataCacheNotificationDescriptor AddFailureNotificationCallback(DataCacheFailureNotificationCallback callBack)
        {
            throw new NotSupportedException();
        }
        public void RemoveCallback(DataCacheNotificationDescriptor nd)
        {
            _cacheHandler.RemoveCallback(nd);
        }


    }
}

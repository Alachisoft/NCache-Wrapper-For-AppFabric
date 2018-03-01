using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Web.Caching;
using Alachisoft.NCache.Web;
using Alachisoft.NCache.Runtime.Caching;
using System.Collections;
using System.Configuration;
using System.Runtime.Serialization;

namespace Alachisoft.NCache.Data.Caching.Handler
{
    /// <summary>
    /// provides handler for all internal NCache calls
    /// </summary>
    internal class CacheHandler
    {
        #region [private members]
        
        private DataFormatter _formatter;
        private Cache _NCache;
        private bool _addRegionCheck;
        private bool _removeRegionCheck;
        private bool _clearRegionCheck;
        #endregion

        #region[    Constructor    ]
        internal CacheHandler(Alachisoft.NCache.Web.Caching.Cache cache)
        {
           
            _NCache = cache;
            _addRegionCheck=false;
            _removeRegionCheck = false;
            _clearRegionCheck = false;
            _formatter = new DataFormatter();
           
        }
        #endregion


        internal DataCacheItemVersion Add(string key, object value, IEnumerable<DataCacheTag> tags, TimeSpan timeOut, string region)
        {
            string expirationCheck = ConfigurationManager.AppSettings["Expirable"];
            
            try
            {
                CacheItem _item=null;
                if (expirationCheck.Equals("True"))
                {
                    if (timeOut == TimeSpan.Zero)
                    {
                        string TTL = ConfigurationManager.AppSettings["TTL"];
                        TimeSpan tempTimeOut = TimeSpan.Parse(TTL);
                        if (!String.IsNullOrWhiteSpace(TTL) && tempTimeOut != TimeSpan.Zero)
                        {
                            _item = _formatter.CreateCacheItem(value, tags, region, tempTimeOut);
                        }
                        else if (String.IsNullOrWhiteSpace(TTL))
                        {
                            _item = _formatter.CreateCacheItem(value, tags, region, TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        _item = _formatter.CreateCacheItem(value, tags, region,timeOut);
                    }
                }
                else if (expirationCheck.Equals("False"))
                {
                    _item = _formatter.CreateCacheItem(value, tags, region);
                }
                string _key = _formatter.MarshalKey(key,region);
                CacheItemVersion _version = _NCache.Add(_key, _item);
                return _formatter.ConvertToAPVersion(_version);
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        internal void Clear()
        {
            try
            {
                _NCache.Clear();
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        internal void CreateRegion(string region)
        {
            if (_addRegionCheck == true)
            {
                _NCache.RaiseCustomEvent(region, CallbackType.AddRegion);
            }
        }
       
        internal void RegisterRegionCallBack(CallbackType opcode)
        {
            CallbackHandler _callbackHandle = new CallbackHandler();
            if (opcode == CallbackType.AddRegion)
            {
                _NCache.CustomEvent += new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _addRegionCheck = true;
            }
            else if (opcode == CallbackType.ClearRegion)
            {
                _NCache.CustomEvent += new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _clearRegionCheck = true;
            }
            else if (opcode == CallbackType.RemoveRegion)
            {
                _NCache.CustomEvent += new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _removeRegionCheck = true;
            }
        }

        internal void UnRegisterRegionCallBack(CallbackType opcode)
        {
            CallbackHandler _callbackHandle = new CallbackHandler();
            if (opcode == CallbackType.AddRegion)
            {
                _NCache.CustomEvent -= new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _addRegionCheck = false;
            }
            else if (opcode == CallbackType.ClearRegion)
            {
                _NCache.CustomEvent -= new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _clearRegionCheck = false;
            }
            else if (opcode == CallbackType.RemoveRegion)
            {
                _NCache.CustomEvent -= new CustomEventCallback(_callbackHandle.RegisterRegionCallback);
                _removeRegionCheck = false;
            }
        }

        internal object Get(string key, out DataCacheItemVersion version, string region)
        {
            string _key;
            object obj;
            version = (DataCacheItemVersion)FormatterServices.GetUninitializedObject(typeof(DataCacheItemVersion));
            if (string.IsNullOrWhiteSpace(region))
            {
                _key = _formatter.MarshalKey(key, null);
            }
            else
            {
                _key = _formatter.MarshalKey(key, region);
            }
            try
            {
                if (version != null)
                {
                    CacheItemVersion _cItemVersion = _formatter.ConvertToNCacheVersion(version);
                    obj = _NCache.Get(_key, ref _cItemVersion);
                    if(_cItemVersion != null)
                        if (_cItemVersion.Version != null)
                            version._itemVersion = _cItemVersion;
                }
                else
                {
                    obj = _NCache.Get(_key);
                }
                if (obj != null)
                {
                    return obj;
                }
                else
                    return null;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetBulk(IEnumerable<string> keys, string region)
        {
            string[] keyList = _formatter.MarshalKey(keys, region);
            IDictionary _temp = _NCache.GetBulk(keyList, DSReadOption.None);
            IDictionaryEnumerator _tempEnumerator = (IDictionaryEnumerator)_temp.GetEnumerator();
            while (_tempEnumerator.MoveNext())
            {
                DictionaryEntry item = _tempEnumerator.Entry;
                yield return new KeyValuePair<string, object>(item.Key.ToString(), item.Value);
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByTag(string region, DataCacheTag tag)
        {
            Hashtable getByTagResult = new Hashtable();
            
            Tag nTag  = _formatter.MarshalTag(tag,region);
           
            getByTagResult = _NCache.GetByTag(nTag);
           
            if (getByTagResult != null)
            {
                foreach (DictionaryEntry entry in getByTagResult)
                {
                    object _item =(object) entry.Value;
                    yield return new KeyValuePair<string, object>(_formatter.UnMarshalKey((string)entry.Key), _item);
                }
            }
        }
        
        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByAnyTag(IEnumerable<DataCacheTag> tags, string region)
        {
            Hashtable _resultSet = new Hashtable();
            Tag[] nTags = _formatter.MarshalTags(tags, region);
            
            _resultSet = _NCache.GetByAnyTag(nTags);

            if (_resultSet != null)
            {
                foreach (DictionaryEntry entry in _resultSet)
                {
                    object _item = (object)entry.Value;
                    yield return new KeyValuePair<string, object>(_formatter.UnMarshalKey((string)entry.Key), _item);
                }
            }
        }

        internal IEnumerable<KeyValuePair<string, object>> GetObjectsByAllTags(IEnumerable<DataCacheTag> tags, string region)
        {
            Hashtable _resultSet = new Hashtable();
            Tag[] nTags = _formatter.MarshalTags(tags, region);

            _resultSet = _NCache.GetByAllTags(nTags);

            if (_resultSet != null)
            {
                foreach (DictionaryEntry entry in _resultSet)
                {
                    object _item = (object)entry.Value;
                    yield return new KeyValuePair<string, object>(_formatter.UnMarshalKey((string)entry.Key), _item);
                }
            }
        }
        
        internal IEnumerable<KeyValuePair<string, object>> GetObjectsInRegion(string region) 
        {
            IDictionary getByTagResult = _NCache.GetGroupData(region,null);
            if (getByTagResult != null)
            {
                foreach (DictionaryEntry entry in getByTagResult)
                {
                    object _item = (object)entry.Value;
                    yield return new KeyValuePair<string, object>(_formatter.UnMarshalKey((string)entry.Key), _item);
                }
            }
        }

        internal object GetLock(string key, TimeSpan timeOut, out DataCacheLockHandle appLockHandle, string region, bool forceLock)
        {
            appLockHandle = new DataCacheLockHandle();
            object obj;
            LockHandle lockHandle = _formatter.ConvertToNCacheLockHandle(appLockHandle);
            try
            {
                if (string.IsNullOrWhiteSpace(region))
                {
                    key = _formatter.MarshalKey(key, null);
                }
                else
                {
                    key = _formatter.MarshalKey(key, region);
                }
                object cacheGetAndLockResult = _NCache.Get(key, timeOut, ref lockHandle, true);
                if (cacheGetAndLockResult == null)
                {
                    obj = _NCache.Get(key, timeOut, ref lockHandle, true);
                }
                else
                {
                    obj = cacheGetAndLockResult;
                }
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        internal DataCacheItem GetCacheItem(string key, string region,string CacheName)
        {
            try
            {
                string _key;
                DataCacheItem _dataCacheItem;
                if (string.IsNullOrWhiteSpace(region))
                {
                    _key = _formatter.MarshalKey(key, null);
                }
                else
                {
                    _key = _formatter.MarshalKey(key, region);
                }
                _dataCacheItem = _formatter.ConvertToDataCacheItem(_NCache.GetCacheItem(_key));
                _dataCacheItem.Key = key;
                _dataCacheItem.CacheName = CacheName;

                return _dataCacheItem;
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }        
        internal object GetIfNewer(string key, ref DataCacheItemVersion version, string region)
        {
            
            string _key = _formatter.MarshalKey(key,region);
            CacheItemVersion nVersion = _formatter.ConvertToNCacheVersion(version);
            object obj = _NCache.GetIfNewer(_key, null, null, ref nVersion);

            if (nVersion != null)
            {
                version._itemVersion = nVersion;
            }
            return obj;
       
        }
       
        internal string GetSystemRegionName(string key)
        {
            string _key = _formatter.MarshalKey(key);
            CacheItem _item= _NCache.GetCacheItem(_key);
            if (_item != null)
            {
                return _item.Group;
            }
            else
            {
                return null;
            }
        }
        
        internal DataCacheItemVersion Put(string key, object value, DataCacheItemVersion oldVersion, TimeSpan timeOut,
            IEnumerable<DataCacheTag> tags, string region)
        {
            CacheItem _item=_formatter.CreateCacheItem(value, tags, region, timeOut);
            if (oldVersion != null)
            {
                _item.Version = oldVersion._itemVersion;
            }
            string _key;

            if (string.IsNullOrWhiteSpace(region))
            {
                _key = _formatter.MarshalKey(key, null);
            }
            else
            {
                _key = _formatter.MarshalKey(key, region);
            }
#if PROFESSIONAL                    
                  
#else
            CacheItemVersion _version = _NCache.Insert(_key,_item);
            return _formatter.ConvertToAPVersion(_version);
#endif
        }

        internal DataCacheItemVersion PutAndUnlock(string key, object value, DataCacheLockHandle lockHandle, TimeSpan timeOut,
            IEnumerable<DataCacheTag> tags, string region)
        {
            CacheItem _item = _formatter.CreateCacheItem(value, tags, region, timeOut);
            string _key;
            if (string.IsNullOrWhiteSpace(region))
            {
                _key = _formatter.MarshalKey(key);
            }
            else
                _key = _formatter.MarshalKey(key, region);

            LockHandle nLockHandle = _formatter.ConvertToNCacheLockHandle(lockHandle);
            CacheItemVersion _ver = _NCache.Insert(_key, _item, nLockHandle, true);
            return _formatter.ConvertToAPVersion(_ver);
        }

        internal bool Remove(string key, DataCacheLockHandle lockHandle, string region, DataCacheItemVersion version,RemoveOperation opCode)
        {
            if (String.IsNullOrWhiteSpace(region))
            {
                key = _formatter.MarshalKey(key);
            }
            else
            {
                key = _formatter.MarshalKey(key, region);
            }
            
            if (opCode== RemoveOperation.LockBased)
            {
                LockHandle nLockHandle = _formatter.ConvertToNCacheLockHandle(lockHandle);
                if (_NCache.Remove(key, nLockHandle) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(opCode== RemoveOperation.VersionBased) 
            {
                CacheItemVersion itemVersion = _formatter.ConvertToNCacheVersion(version);
                if (_NCache.Remove(key,itemVersion) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (opCode == RemoveOperation.KeyBased)
            {
                if (_NCache.Remove(key) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        internal bool RemoveRegionData(string region,CallbackType opCode)
        {
            try
            {
                _NCache.RemoveGroupData(region, null);
                if (opCode == CallbackType.ClearRegion)
                {
                    if (_clearRegionCheck == true)
                    {
                        _NCache.RaiseCustomEvent(region, CallbackType.ClearRegion);
                    }
                }
                else if (opCode == CallbackType.RemoveRegion)
                {
                    if(_removeRegionCheck == true)
                    _NCache.RaiseCustomEvent(region, CallbackType.RemoveRegion);
                }

                return true;
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        internal bool ResetObjectTimeout(string key, TimeSpan newTimeOut, string region)
        {
            string _key;

            if (string.IsNullOrWhiteSpace(region))
            {
                _key = _formatter.MarshalKey(key, null);
            }
            else
            {
                _key = _formatter.MarshalKey(key, region);
            }
            CacheItem _item = (CacheItem)_NCache.Get(_key);
            if (newTimeOut != TimeSpan.Zero)
            {
                _item.AbsoluteExpiration = DateTime.Now.Add(newTimeOut); 
            }
            else
            {
                _item.AbsoluteExpiration = System.DateTime.Now.AddMinutes(10.0);
            }
            if (_NCache.Insert(_key, _item) != null)
            {
                return true;
            }
            else
                return false;
        }

        internal bool Unlock(string key, DataCacheLockHandle appLockHandle, TimeSpan timeOut, string region)
        {
            appLockHandle = new DataCacheLockHandle();
            LockHandle lockHandle = _formatter.ConvertToNCacheLockHandle(appLockHandle);

            string _key;

            if (string.IsNullOrWhiteSpace(region))
            {
                _key = _formatter.MarshalKey(key, null);
            }
            else
            {
                _key = _formatter.MarshalKey(key, region);
            }

            if (timeOut != TimeSpan.Zero)
            {
                CacheItem _item = (CacheItem)_NCache.Get(_key);
                _item.AbsoluteExpiration = DateTime.Now.Add(timeOut);

                if (_NCache.Insert(_key, _item, lockHandle, true) != null)
                {
                    _NCache.Unlock(_key, lockHandle);
                }
                else
                    return false;
            }
            else
            {
                _NCache.Unlock(_key, lockHandle);
            }
            return true;
        }
           
    }
}

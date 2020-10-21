using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Web.Caching;

namespace Alachisoft.NCache.Data.Caching
{
    class CallbackHandler
    {
        public string CacheId { get; set; }

        public string Region { get; set; }

        public DataCacheNotificationDescriptor NotificationDescriptor { get; set; }

        internal CacheEventDescriptor NCacheEventDescriptor { get; set; }

        public DataCacheOperations Operation { get; set; }

        public DataCacheNotificationCallback Callback { get; set; }

        internal CallbackType Type {get; set;}

        internal string Key { get; set; }

        private DataFormatter _formatter = new DataFormatter();

        public void OnItemAdded(string key)
        {
            string[] keyRegion=_formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.AddItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.AddItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void OnItemUpdate(string key)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void OnItemRemove(string key,object obj, CacheItemRemovedReason reason)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void OnRegionClear(object region)
        {
            if (Callback != null && (Operation & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion)
            {
                Callback(CacheId, (string)region, null, null, DataCacheOperations.ClearRegion, NotificationDescriptor);
            }
        }

        public void OnRegionAdd(object region)
        {
            if (Callback != null && (Operation & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion)
            {
                Callback(CacheId, (string)region, null, null, DataCacheOperations.CreateRegion, NotificationDescriptor);
            }
        }

        public void OnRegionDeletion(object region)
        {
            if (Callback != null && (Operation & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion)
            {
                Callback(CacheId, (string)region, null, null, DataCacheOperations.RemoveRegion, NotificationDescriptor);
            }
        }

        public void OnSpecificItemUpdate(string key)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.ItemSpecificCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificItemCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void OnSpecificItemRemoved(string key,object value,Alachisoft.NCache.Web.Caching.CacheItemRemovedReason reason)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.ItemSpecificCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificItemCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void RegisterRegionCallback(object region, object opCode)
        {
            CallbackType operation= (CallbackType) opCode;
            if (operation == CallbackType.AddRegion)
            {
                this.OnRegionAdd(region);
            }
            if (operation == CallbackType.ClearRegion)
            {
                this.OnRegionClear(region);
            }
            if (operation == CallbackType.RemoveRegion)
            {
                this.OnRegionDeletion(region);
            }
        }

        public CacheDataNotificationCallback GetNCacheNotificationCallback(DataCacheNotificationCallback callback, Runtime.Events.EventType nEvent)
        {
            if(nEvent.ToString().Equals("ItemAdded"))
            {
                return new CacheDataNotificationCallback(NCacheItemAddedCallback);
            }
            if (nEvent.ToString().Equals("ItemUpdated"))
            {
                return new CacheDataNotificationCallback(NCacheItemUpdatedCallback);
            }
            if (nEvent.ToString().Equals("ItemRemoved"))
            {
                return new CacheDataNotificationCallback(NCacheItemRemovedCallback);
            }
            return null;
        }

        public void NCacheItemAddedCallback(string key, CacheEventArg args)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.AddItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.AddItem, NotificationDescriptor);
                    }
                }
            }
        }

        public void NCacheItemUpdatedCallback(string key, CacheEventArg args)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                    }
                }
            }
        }
        
        public void NCacheItemRemovedCallback(string key, CacheEventArg args)
        {
            string[] keyRegion = _formatter.SplitKeyAndRegion(key);
            if (Type == CallbackType.CacheLevelCallback)
            {
                if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                {
                    Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                }
            }
            else if (Type == CallbackType.RegionSpecificCallback)
            {
                if (Region == keyRegion[1])
                {
                    if (Callback != null && (Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                    {
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                    }
                }
            }
        }
    }
}

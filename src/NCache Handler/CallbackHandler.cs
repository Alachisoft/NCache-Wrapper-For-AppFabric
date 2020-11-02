using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Events;
using System;

namespace Alachisoft.NCache.Data.Caching
{
    internal class CallbackHandler : ICallBackHandler
    {
        internal string CacheId { get; set; }
        internal string Region { get; set; }
        internal DataCacheNotificationDescriptor NotificationDescriptor { get; set; }

        internal DataCacheOperations Operation { get; set; }
        internal DataCacheNotificationCallback Callback { get; set; }
        internal CallbackType Type { get; set; }
        internal string Key { get; set; }
        public CacheEventDescriptor NCacheEventDescriptor { get; set; }

        internal void OnCacheDataModification(string key, CacheEventArg args)
        {
            if (args.EventType == EventType.ItemAdded)
            {
                OnItemAdded(key, args);
            }
            else if (args.EventType == EventType.ItemUpdated)
            {
                OnItemUpdate(key, args);
            }
            else
            {
                OnItemRemove(key, args);
            }
        }
        private void OnItemAdded(string key, CacheEventArg args)
        {
            if (key.StartsWith(Constants.CREATE_REGION_KEY))
            {
                var region = key.Replace(Constants.CREATE_REGION_KEY, "");
                if (Type == CallbackType.CacheLevelCallback || (Type == CallbackType.RegionSpecificCallback && Region == region))
                {
                    if (Callback != null && (Operation & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion)
                    {
                        Callback(CacheId, region, "", null, DataCacheOperations.CreateRegion, NotificationDescriptor);
                    }
                }
            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);
                if (Validate(Type, keyRegion, Key, Region, Callback))
                {
                    if ((Operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem)
                    {
                        var cacheItemVersion = args.Item.CacheItemVersion;
                        var version = new DataCacheItemVersion(cacheItemVersion);
                        Callback(CacheId, keyRegion[1], keyRegion[0], version, DataCacheOperations.AddItem, NotificationDescriptor);
                    }
                }
            }
        }
        private void OnItemUpdate(string key, CacheEventArg args)
        {
            if (key.StartsWith(Constants.CREATE_REGION_KEY))
            {
                var region = key.Replace(Constants.CREATE_REGION_KEY, "");
                if (Type == CallbackType.CacheLevelCallback || (Type == CallbackType.RegionSpecificCallback && Region == region))
                {
                    if (Callback != null && (Operation & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion)
                    {
                        Callback(CacheId, region, "", null, DataCacheOperations.ClearRegion, NotificationDescriptor);
                    }
                }
            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);
                if (Validate(Type, keyRegion, Key, Region, Callback))
                {
                    if ((Operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem)
                    {
                        var cacheItemVersion = args.Item.CacheItemVersion;
                        var version = new DataCacheItemVersion(cacheItemVersion);
                        Callback(CacheId, keyRegion[1], keyRegion[0], version, DataCacheOperations.ReplaceItem, NotificationDescriptor);
                    }
                }
            }

        }
        private void OnItemRemove(string key, CacheEventArg args)
        {
            if (key.StartsWith(Constants.CREATE_REGION_KEY))
            {
                var region = key.Replace(Constants.CREATE_REGION_KEY, "");

                if (Type == CallbackType.CacheLevelCallback || (Type == CallbackType.RegionSpecificCallback && Region == region))
                {
                    if (Callback != null && (Operation & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion)
                    {
                        Callback(CacheId, region, "", null, DataCacheOperations.RemoveRegion, NotificationDescriptor);
                    }
                }
            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);

                if (Validate(Type, keyRegion, Key, Region, Callback))
                {
                    if ((Operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem)
                    {
                        var cacheItemVersion = args.Item.CacheItemVersion;
                        var version = new DataCacheItemVersion(cacheItemVersion);
                        Callback(CacheId, keyRegion[1], keyRegion[0], null, DataCacheOperations.RemoveItem, NotificationDescriptor);
                    }
                }
            }
        }

        private static bool Validate(CallbackType type, string[] keyRegion, string key, string region, DataCacheNotificationCallback callBack)
        {
            if (callBack == null)
            {
                return false;
            }
            else if (type == CallbackType.CacheLevelCallback)
            {
                return true;
            }
            else if (type == CallbackType.RegionSpecificCallback && region == keyRegion[1])
            {
                return true;
            }
            else if (type == CallbackType.ItemSpecificCallback && key == keyRegion[0])
            {
                return true;
            }
            else if (type == CallbackType.RegionSpecificItemCallback && key == keyRegion[0] && region == keyRegion[1])
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        internal static EventType GetEventType(DataCacheOperations operation)
        {
            EventType eventType = (EventType)0;

            var addItem = (operation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem;

            var updateItem = (operation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem;

            var removeItem = (operation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem;

            var createRegion = (operation & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion;

            var removeRegion = (operation & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion;

            var clearRegion = (operation & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion;

            if (addItem || createRegion || clearRegion)
            {
                eventType |= EventType.ItemAdded;
            }

            if (updateItem)
            {
                eventType |= EventType.ItemUpdated;
            }

            if (removeItem || removeRegion)
            {
                eventType |= EventType.ItemRemoved;
            }

            return eventType;
        }

    }
}

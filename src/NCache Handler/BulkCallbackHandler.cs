using Alachisoft.NCache.Client;
using Alachisoft.NCache.Runtime.Events;

namespace Alachisoft.NCache.Data.Caching
{
    internal class BulkCallbackHandler : ICallBackHandler
    {
        internal string CacheId { get; set; }
        internal DataCacheBulkNotificationCallback BulkCallback { get; set; }
        internal DataCacheNotificationDescriptor NotificationDescriptor { get; set; }
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
            DataCacheOperationDescriptor descriptor;
            if (key.StartsWith($"{Constants.CREATE_REGION_KEY}"))
            {
                var region = key.Replace($"{Constants.CREATE_REGION_KEY}", "");
                descriptor = new DataCacheOperationDescriptor(CacheId, region, "", DataCacheOperations.CreateRegion, null);

            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);
                var cacheItemVersion = args.Item.CacheItemVersion;
                var version = new DataCacheItemVersion(cacheItemVersion);
                descriptor = new DataCacheOperationDescriptor(CacheId, keyRegion[1], keyRegion[0], DataCacheOperations.AddItem, version);
            }

            BulkCallback(CacheId, new DataCacheOperationDescriptor[] { descriptor }, NotificationDescriptor);
        }

        private void OnItemUpdate(string key, CacheEventArg args)
        {
            DataCacheOperationDescriptor descriptor;
            if (key.StartsWith($"{Constants.CREATE_REGION_KEY}"))
            {
                var region = key.Replace($"{Constants.CREATE_REGION_KEY}", "");
                descriptor = new DataCacheOperationDescriptor(CacheId, region, "", DataCacheOperations.ClearRegion, null);

            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);
                var cacheItemVersion = args.Item.CacheItemVersion;
                var version = new DataCacheItemVersion(cacheItemVersion);
                descriptor = new DataCacheOperationDescriptor(CacheId, keyRegion[1], keyRegion[0], DataCacheOperations.ReplaceItem, version);
            }

            BulkCallback(CacheId, new DataCacheOperationDescriptor[] { descriptor }, NotificationDescriptor);
        }

        private void OnItemRemove(string key, CacheEventArg args)
        {
            DataCacheOperationDescriptor descriptor;

            if (key.StartsWith($"{Constants.CREATE_REGION_KEY}"))
            {
                var region = key.Replace($"{Constants.CREATE_REGION_KEY}", "");
                descriptor = new DataCacheOperationDescriptor(CacheId, region, "", DataCacheOperations.RemoveRegion, null);
            }
            else
            {
                var keyRegion = DataFormatter.SplitKeyAndRegion(key);
                var cacheItemVersion = args.Item.CacheItemVersion;
                var version = new DataCacheItemVersion(cacheItemVersion);
                descriptor = new DataCacheOperationDescriptor(CacheId, keyRegion[1], keyRegion[0], DataCacheOperations.RemoveItem, version);
            }

            BulkCallback(CacheId, new DataCacheOperationDescriptor[] { descriptor }, NotificationDescriptor);

        }

    }
}

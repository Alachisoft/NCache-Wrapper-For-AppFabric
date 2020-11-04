using Alachisoft.NCache.Data.Caching;
using System;
using System.Threading;

namespace NCacheAppFabricConsoleUI
{
    internal static class NotificationTests
    {
        private static string clearedRegion = "filler";
        private static string removedRegion = "filler";
        private static string createdRegion = "filler";

        private static string createdCacheItemKey = "filler";
        private static string updatedCacheItemKey = "filler";
        private static string removedCacheItemKey = "filler";

        private static string createdRegionItemKey = "filler";
        private static string updatedRegionItemKey = "filler";
        private static string removedRegionItemKey = "filler";

        private static DataCacheOperations operations = DataCacheOperations.AddItem | DataCacheOperations.RemoveItem | DataCacheOperations.ReplaceItem | DataCacheOperations.CreateRegion | DataCacheOperations.ClearRegion | DataCacheOperations.RemoveRegion;

        private static void NotificationCallBack(string cacheName, string regionName, string key, DataCacheItemVersion version, DataCacheOperations cacheOperation, DataCacheNotificationDescriptor nd)
        {
            if ((cacheOperation & DataCacheOperations.CreateRegion) == DataCacheOperations.CreateRegion)
            {
                createdRegion = regionName;
            }

            if ((cacheOperation & DataCacheOperations.ClearRegion) == DataCacheOperations.ClearRegion)
            {
                clearedRegion = regionName;
            }

            if ((cacheOperation & DataCacheOperations.RemoveRegion) == DataCacheOperations.RemoveRegion)
            {
                removedRegion = regionName;
            }

            if ((cacheOperation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem && regionName.StartsWith("Default_Region"))
            {
                removedCacheItemKey = key;
            }

            if ((cacheOperation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem && regionName.StartsWith("Default_Region"))
            {
                createdCacheItemKey = key;
            }

            if ((cacheOperation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem && regionName.StartsWith("Default_Region"))
            {
                updatedCacheItemKey = key;
            }

            if ((cacheOperation & DataCacheOperations.RemoveItem) == DataCacheOperations.RemoveItem && !regionName.StartsWith("Default_Region"))
            {
                removedRegionItemKey = key;
            }

            if ((cacheOperation & DataCacheOperations.AddItem) == DataCacheOperations.AddItem && !regionName.StartsWith("Default_Region"))
            {
                createdRegionItemKey = key;
            }

            if ((cacheOperation & DataCacheOperations.ReplaceItem) == DataCacheOperations.ReplaceItem && !regionName.StartsWith("Default_Region"))
            {
                updatedRegionItemKey = key;
            }
        }


        internal static void CheckCacheLevelNotifications()
        {
            try
            {
                var notificationDescriptor = Program.myDefaultCache.AddCacheLevelCallback(operations, NotificationCallBack);

                var myKey = Guid.NewGuid().ToString();
                var myRegion = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                Thread.Sleep(3000);

                if (createdCacheItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome($"Add cache item notification received.");
                }
                else
                {
                    Logger.PrintFailureOutcome("Add cache item notification failure");
                }

                Program.myDefaultCache.Put(myKey, Program.myObjectForCaching + 2);

                Thread.Sleep(3000);

                if (updatedCacheItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome($"Replace cache item notification received.");
                }
                else
                {
                    Logger.PrintFailureOutcome("Replace cache item notification failure");
                }

                Program.myDefaultCache.Remove(myKey);

                Thread.Sleep(3000);

                if (removedCacheItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome($"Remove cache item notification received.");
                }
                else
                {
                    Logger.PrintFailureOutcome("Remove cache item notification failure");
                }

                Program.myDefaultCache.CreateRegion(myRegion);

                Thread.Sleep(3000);

                if (createdRegion == myRegion)
                {
                    Logger.PrintSuccessfulOutcome("Create region notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Create region notification failure");
                }

                Program.myDefaultCache.ClearRegion(myRegion);

                Thread.Sleep(3000);

                if (clearedRegion == myRegion)
                {
                    Logger.PrintSuccessfulOutcome("Clear region notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Clear region notification failure");
                }


                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

                Thread.Sleep(3000);

                if (createdRegionItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome("Add region item notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Add region item notification failure");
                }

                Program.myDefaultCache.Put(myKey, Program.myObjectForCaching + 2, myRegion);

                Thread.Sleep(3000);

                if (updatedRegionItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome("Replace region item notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Replace region item notification failure");
                }

                Program.myDefaultCache.Remove(myKey, myRegion);

                Thread.Sleep(3000);

                if (removedRegionItemKey == myKey)
                {
                    Logger.PrintSuccessfulOutcome("Remove region item notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Remove region item notification failure");
                }

                Program.myDefaultCache.RemoveRegion(myRegion);

                Thread.Sleep(3000);

                if (removedRegion == myRegion)
                {
                    Logger.PrintSuccessfulOutcome("Remove region notification received");
                }
                else
                {
                    Logger.PrintFailureOutcome("Remove region notification failure");
                }

                Program.myDefaultCache.RemoveCallback(notificationDescriptor);
            }
            catch (Exception e)
            {
                Logger.PrintDataCacheException(e);
            }
            finally
            {
                Logger.PrintBreakLine();
            }
        }
    }
}

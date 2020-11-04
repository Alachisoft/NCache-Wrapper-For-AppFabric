using Microsoft.ApplicationServer.Caching;
using System;
using System.Configuration;

namespace NativeAppFabricConsoleUI
{
    class Program
    {
        static DataCacheFactory myCacheFactory;
        internal static string myObjectForCaching = "This is my Object";
        internal static DataCache myDefaultCache;

        static void Main(string[] args)
        {
            try
            {
                PrepareClient();
                RunSampleTest();
            }
            catch (Exception exp)
            {

                Console.WriteLine(exp.ToString());
            }

            Console.ReadLine();
        }

        private static void RunSampleTest()
        {
            RunAddItemTests();

            RunGetItemTests();

            RunPutItemTests();

            RunRemoveItemTests();

            RunExpirationTests();

            RunCreateRegionTests();

            RunAddItemInRegionTests();

            RunGetItemInRegionTests();

            RunPutItemInRegionTests();

            RunRemoveItemInRegionTests();

            RunClearRegionTests();

            RunRemoveRegionTests();

            RunOptimisticConcurrencyTests();

            RunPessimisticConcurrencyTests();

            RunSearchTests();

            RunBulkGetTests();

            RunNotificationTests();
        }

        private static void PrepareClient()
        {
            myCacheFactory = new DataCacheFactory();
            var cacheName = ConfigurationManager.AppSettings["CacheId"];
            myDefaultCache = myCacheFactory.GetCache(cacheName);
        }

        private static void RunCreateRegionTests()
        {
            Logger.WriteHeaderLine("CREATE REGION TESTS");

            CreateRegionTests.CreateRegionWithNonEmptyString();

            CreateRegionTests.RecreateRegionWithNonEmptyString();

            CreateRegionTests.CreateRegionWithNullString();

            Logger.WriteFooterLine("CREATE REGION TESTS");
        }

        private static void RunClearRegionTests()
        {
            Logger.WriteHeaderLine("CLEAR REGION TESTS");

            ClearRegionTests.ClearExistingRegion();
            ClearRegionTests.ClearNonExistingRegion();
            ClearRegionTests.ClearNullRegion();

            Logger.WriteFooterLine("CLEAR REGION TESTS");
        }

        private static void RunRemoveRegionTests()
        {
            Logger.WriteHeaderLine("REMOVE REGION TESTS");

            RemoveRegionTests.RemoveExistingRegion();

            RemoveRegionTests.RemoveNonExistingRegion();

            RemoveRegionTests.RemoveRegionWithNullInput();

            Logger.WriteFooterLine("REMOVE REGION TESTS");
        }

        private static void RunAddItemTests()
        {
            Logger.WriteHeaderLine("ADD ITEM IN CACHE TESTS");

            AddInCacheTests.AddKeyValuePair();
            AddInCacheTests.AddExistingKeyValuePair();
            AddInCacheTests.AddKeyValuePairWithNullObject();
            AddInCacheTests.AddKeyValuePairWithNullKey();

            Logger.WriteFooterLine("ADD ITEM IN CACHE TESTS");
        }

        private static void RunGetItemTests()
        {
            Logger.WriteHeaderLine("GET ITEM IN CACHE TESTS");

            GetInCacheTests.GetExistingKey();
            GetInCacheTests.GetNonExistingKey();
            GetInCacheTests.GetNullKey();
            GetInCacheTests.GetExistingKeyItemVersion();
            GetInCacheTests.GetNonExistingKeyItemVersion();
            GetInCacheTests.GetExistingKeyWithOutdatedItemVersion();

            Logger.WriteFooterLine("GET ITEM IN CACHE TESTS");
        }

        private static void RunPutItemTests()
        {
            Logger.WriteHeaderLine("PUT ITEM IN CACHE TESTS");

            PutInCacheTests.PutExistingKey();
            PutInCacheTests.PutNonExistingKey();
            PutInCacheTests.PutKeyWithNullValue();

            Logger.WriteFooterLine("PUT ITEM IN CACHE TESTS");
        }

        private static void RunRemoveItemTests()
        {
            Logger.WriteHeaderLine("REMOVE ITEM IN CACHE TESTS");

            RemoveInCacheTests.RemoveExistingKey();
            RemoveInCacheTests.RemoveNonExistingKey();
            RemoveInCacheTests.RemoveNullKey();

            Logger.WriteFooterLine("REMOVE ITEM IN CACHE TESTS");
        }

        private static void RunExpirationTests()
        {
            Logger.WriteHeaderLine("EXPIRATION TESTS");

            ExpirationTests.AddKeyValuePairWithNegativeTimeSpan();
            ExpirationTests.AddKeyValuePairWithZeroTimeSpan();
            ExpirationTests.PutKeyValuePairWithNegativeTimeSpan();
            ExpirationTests.PutKeyValuePairWithZeroTimeSpan();
            ExpirationTests
                .ResetObjectTimeoutOnNonExistingRegionItem();

            Logger.WriteFooterLine("EXPIRATION TESTS");
        }

        private static void RunAddItemInRegionTests()
        {
            Logger.WriteHeaderLine("ADD ITEM IN REGION TESTS");

            AddInRegionTests.AddKeyValuePairInExistingRegion();
            AddInRegionTests.AddKeyValuePairInNonExistingRegion();
            AddInRegionTests.AddKeyValuePairInNullRegion();

            Logger.WriteFooterLine("ADD ITEM IN REGION TESTS");
        }

        private static void RunPutItemInRegionTests()
        {
            Logger.WriteHeaderLine("PUT ITEM IN REGION TESTS");

            PutInRegionTests.PutKeyValuePairInExistingRegion();
            PutInRegionTests.PutKeyValuePairInNonExistingRegion();
            PutInRegionTests.PutKeyValuePairInNullRegion();

            Logger.WriteFooterLine("PUT ITEM IN REGION TESTS");
        }

        private static void RunGetItemInRegionTests()
        {
            Logger.WriteHeaderLine("GET ITEM IN REGION TESTS");

            GetInRegionTests.GetExistingKeyInRegion();
            GetInRegionTests.GetKeyInNonExistingRegion();
            GetInRegionTests.GetKeyInNullRegion();

            Logger.WriteFooterLine("GET ITEM IN REGION TESTS");
        }

        private static void RunRemoveItemInRegionTests()
        {
            Logger.WriteHeaderLine("REMOVE ITEM IN REGION TESTS");

            RemoveInRegionTests.RemoveExistingKeyInRegion();
            RemoveInRegionTests.RemoveKeyInNonExistingRegion();
            RemoveInRegionTests.RemoveKeyInNullRegion();

            Logger.WriteFooterLine("REMOVE ITEM IN REGION TESTS");
        }

        private static void RunOptimisticConcurrencyTests()
        {
            Logger.WriteHeaderLine("OPTIMISTIC CONCURRENCY TESTS");

            OptimisticConcurrencyTests
                .GetExistingKeyItemVersion();

            OptimisticConcurrencyTests
                .GetExistingKeyWithOutdatedItemVersion();

            OptimisticConcurrencyTests
                .GetNonExistingKeyItemVersion();

            OptimisticConcurrencyTests
                .GetIfNewerObjectWithoutUpdate();

            OptimisticConcurrencyTests
                .GetIfNewerObjectWithUpdate();

            OptimisticConcurrencyTests
                .GetIfNewerObjectWithNullVersion();

            OptimisticConcurrencyTests
                .PutExistingKeyWithNullVersion();

            OptimisticConcurrencyTests
                .PutExistingKeyWithOutdatedVersion();

            OptimisticConcurrencyTests
                .PutNonExistingKeyWithNullVersion();

            OptimisticConcurrencyTests
                .RemoveExistingKeyWithNullVersion();

            OptimisticConcurrencyTests
                .RemoveExistingKeyWithOutdatedVersion();

            Logger.WriteFooterLine("OPTIMISTIC CONCURRENCY TESTS");
        }

        private static void RunPessimisticConcurrencyTests()
        {
            Logger.WriteHeaderLine("PESSIMISTIC CONCURRENCY TESTS");

            PessimisticConcurrencyTests
                .GetLockOnExistingKey();

            PessimisticConcurrencyTests
                .GetLockOnNonExistingKey();

            PessimisticConcurrencyTests
                .GetLockOnLockedKey();

            PessimisticConcurrencyTests
                .GetLockOnNonExistingRegion();

            PessimisticConcurrencyTests
                .GetLockOnNullRegion();

            PessimisticConcurrencyTests
                .GetLockWithNegativeLockTimeout();

            PessimisticConcurrencyTests
                .GetLockWithZeroLockTimeout();

            PessimisticConcurrencyTests
                .PutAndUnlockWithValidLockHandle();

            PessimisticConcurrencyTests
                .PutAndUnlockWithValidLockHandleNegativeExpirationTimeout();

            PessimisticConcurrencyTests
                .PutAndUnlockWithValidLockHandleZeroLockTimeout();

            PessimisticConcurrencyTests
                .PutAndUnlockWithInvalidLockHandle();

            PessimisticConcurrencyTests
                .PutAndUnlockWithNullLockHandle();

            PessimisticConcurrencyTests
                .UnlockWithValidLockHandle();

            PessimisticConcurrencyTests
                .UnlockWithValidLockHandleNegativeExpirationTimeout();

            PessimisticConcurrencyTests
                .UnlockWithValidLockHandleZeroLockTimeout();

            PessimisticConcurrencyTests
                .UnlockWithInvalidLockHandle();

            PessimisticConcurrencyTests
                .UnlockWithNullLockHandle();

            PessimisticConcurrencyTests
                .RemoveWithValidLockHandle();

            PessimisticConcurrencyTests
                .RemoveWithInvalidLockHandle();

            PessimisticConcurrencyTests
                .RemoveWithNullLockHandle();


            Logger.WriteFooterLine("PESSIMISTIC CONCURRENCY TESTS");
        }

        private static void RunSearchTests()
        {
            Logger.WriteHeaderLine("SEARCH TESTS");

            TagsTests.AddItemWithTagsInRegion();

            TagsTests.AddItemWithNullTagArrayInRegion();

            TagsTests
                .AddItemWithTagArrayHavingNullElementsInRegion();

            TagsTests.AddItemWithZeroLengthTagArrayInRegion();

            TagsTests.GetItemsInExistingRegion();

            TagsTests.GetItemsInNonExistingRegion();

            TagsTests.GetItemsByTagInExistingRegion();

            TagsTests.GetItemsByTagInNonExistingRegion();

            TagsTests.GetItemsByNonExistingTagInExistingRegion();

            TagsTests.GetItemsByNullTagInExistingRegion();

            TagsTests.GetItemsByAnyTagInExistingRegion();

            TagsTests.GetItemsByAnyNonExistingTagInExistingRegion();

            TagsTests.GetItemsByAnyNullTagInExistingRegion();

            TagsTests.GetItemsByAnyNullTagArrayInExistingRegion();

            TagsTests.GetItemsByAnyZeroTagArrayInExistingRegion();

            TagsTests.GetItemsByAllTagInExistingRegion();

            Logger.WriteFooterLine("SEARCH TESTS");
        }

        private static void RunBulkGetTests()
        {
            Logger.WriteHeaderLine("BULK GET TESTS");

            BulkGetTests.BulkGetExistingItemsInExistingRegion();

            BulkGetTests.BulkGetItemsInNonExistingRegion();

            BulkGetTests.BulkGetZeroItemsFromExistingRegion();

            BulkGetTests.BulkGetZeroItemsInNonExistingRegion();

            BulkGetTests.BulkGetNullItemsFromExistingRegion();

            BulkGetTests.BulkGetNullItemsInNonExistingRegion();

            Logger.WriteFooterLine("BULK GET TESTS");
        }

        private static void RunNotificationTests()
        {
            Logger.WriteHeaderLine("NOTIFICATION TESTS");

            NotificationTests.CheckCacheLevelNotifications();

            Logger.WriteFooterLine("NOTIFICATION TESTS");
        }
    }
}

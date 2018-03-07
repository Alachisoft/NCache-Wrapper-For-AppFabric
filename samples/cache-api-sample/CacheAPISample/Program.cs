//Orignally created by Microsoft.
//Modified by Alachisoft to work with NCache Wrapper for AppFabric.

namespace CacheAPISample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
	//Only need to change the namespace from  Microsoft.ApplicationServer.Caching to Alachisoft.NCache.Data.Caching
    //using Microsoft.ApplicationServer.Caching; 
    using Alachisoft.NCache.Data.Caching;

    class Program
    {
        DataCacheFactory myCacheFactory;
        DataCache myDefaultCache;
        string myObjectForCaching = "This is my Object";

        static void Main(string[] args)
        {
            try
            {
                Program program = new Program();
                program.PrepareClient();
                program.RunSampleTest();

                Console.WriteLine("Press any key to continue ...");
                Console.ReadLine();

            }
            catch(Exception exp)
            {
                Console.WriteLine(exp.ToString());
            }
        }

        public void RunSampleTest()
        {
            DataCacheItemVersion itemVersion;
            string item;

            //
            // CREATING A REGION
            //
            // Point to remember - This region will be re-created everytime this sampletest is run.            
            Console.WriteLine();
            Console.WriteLine("Creating Region for general use in default cache");
            string myRegion = "MyRegion";

            if (!CreateRegion(myRegion))
            {
                return;
            }

            Console.WriteLine();
            //
            // CREATING A REGION For the LoadTest Sample
            //
            // Point to remember - This region will be re-created everytime this sampletest is run
            Console.WriteLine("Creating Region for the load test in the default cache");
            string myLoadTestRegion = "LetsLoadTheRegion";

            if (!CreateRegion(myLoadTestRegion))
            {
                return;
            }

            Console.WriteLine();
            //
            // TESTING SIMPLE Add/Get on default cache
            //
            // no regions
            //
            // Need to catch exception here to allow the program to run continuously
            // Try this variation
            // - Comment the try catch on this block of code
            // - Run this program twice within 10 mins (default timeout for data eviction from cache)
            // - Result, Add will throw a exception
            // - Run this program after 10 mins
            // - Result, Program will run ok
            // Try this variation
            // - Put a BreakPoint at the Get("KeyToMyString") call
            // - Run the sample test after 10 mins
            // - Get will fail
            Console.WriteLine("----------------------");
            Console.WriteLine("Testing Simple Add/Get");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = <none>");
            Console.WriteLine("Tags        = <none>");
            Console.WriteLine("Version     = <none>");

            try
            {
                // Store the object in the default Cache with a Add
                if ((itemVersion = myDefaultCache.Add("KeyToMyString", myObjectForCaching)) != null)
                {
                    Console.WriteLine("PASS--->Add-Object Added to Cache [key=KeyToMyString]");
                }
                else
                {
                    Console.WriteLine("**FAIL--->Add-Object did not add to cache - FAIL");
                }

                // Do a Simple Get using valid Key from the default Cache
                if ((item = (string)myDefaultCache.Get("KeyToMyString")) != null)
                {
                    Console.WriteLine("PASS--->Get-Object Get from cache [key=KeyToMyString]");
                }
                else
                {
                    Console.WriteLine("**FAIL--->Get-Object did not Get from cache [key=KeyToMyString]");
                }

                // Do a Simple Get using an invalid Key from the default Cache
                if ((item = (string)myDefaultCache.Get("InCorrectKeySpecified")) == null)
                {
                    Console.WriteLine("PASS--->Get-Object did not Get, since invalid key specified [key=InCorrectKeySpecified]");
                }
                else
                {
                    Console.WriteLine("**FAIL--->Get-Object Get from cache, unexpected result");
                }
            }
            catch (DataCacheException ex)
            {
                Console.WriteLine("**FAIL--->Add-Get-This is failing probably because you are running this");
                Console.WriteLine("          sample test within 10mins (default TTL for the named cache) in clusterconfig.xml");
                Console.WriteLine("          To get this working, in the admin tool");
                Console.WriteLine("          - restart-cachecluster");
                Console.Write("**FAIL--->Distributed Cache Generated Exception:");
                Console.WriteLine(ex.ToString());
            }

            //
            // TESTING SIMPLE Add/Get on default cache USING Region
            //
            // without Tags
            // without version
            //
            Console.WriteLine("----------------------");
            Console.WriteLine("Testing Simple Add/Get");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = " + myRegion);
            Console.WriteLine("Tags        = <none>");
            Console.WriteLine("Version     = <none>");

            try
            {
                // Store the object in a region with a Add
                if ((itemVersion = myDefaultCache.Add("KeyToMyString", myObjectForCaching, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Add-Object Added to Cache [key=KeyToMyString]");
                }
                else
                {
                    Console.WriteLine("**FAIL----->Add-Object did not add to cache");
                }

                // Do a Simple Get using valid Key from a named region
                if ((item = (string)myDefaultCache.Get("KeyToMyString", myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Get-Object Get from cache [key=KeyToMyString]");
                }
                else
                {
                    Console.WriteLine("**FAIL----->Get-Object did not Get from cache [key=KeyToMyString]");
                }

                // Do a Simple Get with invalid key
                if ((item = (string)myDefaultCache.Get("InvalidKey", myRegion)) != null)
                {
                    Console.WriteLine("**FAIL----->Get-Object returned from Cache, should not since key is invalid [key=InvalidKey]");
                }
                else
                {
                    Console.WriteLine("PASS----->Get-Object did not Get from cache. Expected since key is invalid [key=InvalidKey]");
                }
            }
            catch (DataCacheException ex)
            {
                Console.Write("**FAIL----->Add-Get-Distributed Cache Generated Exception:");
                Console.WriteLine(ex.ToString());
                // Will never get this error since we are Removing existing regions and creating new ones
            }

            //
            // TESTING SIMPLE Add/GetAndLock using Region
            //
            // without Tags
            // without version
            //
            // Try this variation
            // - Put a BreakPoint on the second GetAndLock, and hold the execution for 5 seconds.
            //   It will return the object and lock the object for 10 seconds. Since the first lock has expired.
            //   Additionally : Study behaviour of Put and PutAndUnlock
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Testing Simple Add/Get/GetAndLock/GetIfNewer/Put/PutAndUnlock");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = " + myRegion);
            Console.WriteLine("Tags        = <none>");
            Console.WriteLine("Version     = <none>");

            DataCacheItemVersion myVersionBeforeChange = null, myVersionAfterChange = null, myVersionChangedOnceMore = null;
            DataCacheLockHandle lockHandle;
            string myKey = "KeyToMyStringTryingLock";

            try
            {
                // Initialize the object with a Add
                if ((itemVersion = myDefaultCache.Add(myKey, myObjectForCaching, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Add-Object Added to Cache [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("**FAIL----->Add-Object did not add to cache [key={0}]", myKey);
                }

                // Do a Simple Get, lock the object for 5 seconds
                if ((item = (string)myDefaultCache.GetAndLock(myKey,
                                                              new TimeSpan(0, 0, 5), out lockHandle, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->GetAndLock-Object Get from cache [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("**FAIL----->GetAndLock-Object did not Get from cache [key={0}]", myKey);
                }

                // Do a optimistic Get
                if ((item = (string)myDefaultCache.Get(myKey, out myVersionBeforeChange, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Get-Object returned. Get will always pass. Will not wait");
                    Console.WriteLine("          on a updating object. Current Version will be returned. [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("**FAIL----->Get-Object did not return. [key={0}]", myKey);
                }

                try
                {
                    // Do a one more Simple Get, and attempt lock the object for 10 seconds
                    if ((item = (string)myDefaultCache.GetAndLock(myKey,
                                                                  new TimeSpan(0, 0, 10), out lockHandle, myRegion)) != null)
                    {
                        Console.WriteLine("**FAIL----->GetAndLock-Object Get from cache [key={0}]", myKey);
                    }
                    else
                    {
                        // Since a exception will catch it, this will never return null
                        Console.WriteLine("PASS----->GetAndLock-Object did not Get from cache [key={0}]", myKey);
                    }
                }
                catch (DataCacheException ex)
                {
                    Console.WriteLine("PASS----->GetAndLock hit a exception, because Object is already locked. [key={0}]", myKey);
                    Console.Write("PASS----->GetAndLock-Distributed Cache Generated Exception:");
                    Console.WriteLine(ex.ToString());
                }

                // Get the Object only if the version has changed
                if ((item = (string)myDefaultCache.GetIfNewer(myKey, ref myVersionBeforeChange, myRegion)) != null)
                {
                    Console.WriteLine("**FAIL----->GetIfNewer-Object changed. Should not return as Object has");
                    Console.WriteLine("            not been changed. [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("PASS----->GetIfNewer-Object has not changed. Hence did not return. [key={0}]", myKey);
                }

                // Now update the object with a Put                
                if ((myVersionAfterChange = (DataCacheItemVersion)myDefaultCache.Put(myKey,
                                                    myObjectForCaching + "Put1", myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Put1-null-version-Object changed. Put will pass even if Object");
                    Console.WriteLine("          is locked. Object will also be unlocked. [key={0}]", myKey);
                    myObjectForCaching += "Put1";
                }
                else
                {
                    Console.WriteLine("PASS----->Put1-null-version-Object did not change. [key={0}]", myKey);
                }

                // Object with older version changed
                if ((item = (string)myDefaultCache.GetIfNewer(myKey, ref myVersionBeforeChange, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->GetIfNewer-Object has been changed. [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("**FAIL----->GetIfNewer-Object did not return. Put ");
                    Console.WriteLine("            did modify the Object. Should return. [key={0}]", myKey);
                }

                // Object with newer version after Put
                if ((item = (string)myDefaultCache.GetIfNewer(myKey, ref myVersionAfterChange, myRegion)) != null)
                {
                    Console.WriteLine("**FAIL----->GetIfNewer-Object with newer version not changed.");
                    Console.WriteLine("            Should not return. [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("PASS----->GetIfNewer-Object with newer version not changed. [key={0}]", myKey);
                }

                // Object with newer version after Put
                if ((myVersionChangedOnceMore = (DataCacheItemVersion)myDefaultCache.Put(myKey,
                                                    myObjectForCaching + "Put2", myVersionBeforeChange, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Put2-version from Put1-Object changed. [key={0}]", myKey);
                    myObjectForCaching += "Put2";
                }
                else
                {
                    Console.WriteLine("**FAIL----->Put2-version from Put1-Object did not change. [key={0}]", myKey);
                }

                try
                {
                    // Try the above PutAndUnlock                 
                    if ((myVersionChangedOnceMore = (DataCacheItemVersion)myDefaultCache.PutAndUnlock(myKey,
                                                        myObjectForCaching + "Put3", lockHandle, myRegion)) != null)
                    {
                        Console.WriteLine("PASS----->PutAndUnlock-Object updated and unlocked. [key={0}]", myKey);
                        myObjectForCaching += "Put3";
                    }
                    else
                        Console.WriteLine("**FAIL----->PutAndUnlock-Object should have updated and unlocked. [key={0}]", myKey);
                }
                catch (DataCacheException ex)
                {
                    Console.WriteLine("PASS----->PutAndUnlock-Expected exception since Object is already unlocked. [key={0}]", myKey);
                    Console.Write("PASS---->PutAndUnlock-Distributed Cache Generated Exception:");
                    Console.WriteLine(ex.ToString());
                }

                // Unlock Object
                try
                {
                    myDefaultCache.Unlock(myKey, lockHandle, myRegion);
                    Console.WriteLine("PASS----->Unlock-Object unlocked. [key={0}]", myKey);
                }
                catch (DataCacheException ex)
                {
                    Console.WriteLine("PASS----->Unlock-Expected exception since Object is already unlocked. [key={0}]", myKey);
                    Console.Write("PASS----->Unlock-Distributed Cache Generated Exception:");
                    Console.WriteLine(ex.Message);
                }

                // Finally, Test the state of object should be "This is my Object.Put1Put2"
                if ((item = (string)myDefaultCache.Get(myKey, out myVersionChangedOnceMore, myRegion)) ==
                    myObjectForCaching)
                {
                    Console.WriteLine("PASS----->Get-Object retrived from cache. [key={0}]", myKey);
                }
                else
                {
                    Console.WriteLine("**FAIL----->Get-Object was not retrived from cache. [key={0}]", myKey);
                }
            }
            catch (DataCacheException ex)
            {
                Console.Write("**FAIL---->Add-Get-GetAndLock-GetIfVersionMismatch-Put-PutAndUnlock-Distributed Cache Generated Exception:");
                Console.WriteLine(ex.ToString());
            }

            //
            // TESTING SIMPLE Add/Get ON REGION with Version
            //
            // without Tags
            // Try this
            // - Put a BreakPoint on the second Put and wait for 5 seconds before releaseing.
            DataCacheItem cacheItem1, cacheItem2;
            DataCacheItemVersion cacheItemVersion;
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("Testing Simple Add/GetCacheItem/Put");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = " + myRegion);
            Console.WriteLine("Tags        = <none>");
            Console.WriteLine("Version     = yes");

            string KeyToMyStringWithVersion = "KeyToMyStringWithVersion";
            try
            {
                // Add an object to a region
                if ((itemVersion = myDefaultCache.Add(KeyToMyStringWithVersion, myObjectForCaching, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Add-Object Added to Cache. [key={0}]", KeyToMyStringWithVersion);
                }
                else
                {
                    Console.WriteLine("**FAIL----->Add-Object did not add to cache [key={0}]", KeyToMyStringWithVersion);
                }
                
                // Get the object added to the Cache
                if ((cacheItem1 = myDefaultCache.GetCacheItem(KeyToMyStringWithVersion, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->GetCacheItem-Object Get from cache [key={0}]", KeyToMyStringWithVersion);
                }
                else
                {
                    Console.WriteLine("**FAIL----->GetCacheItem-Object did not Get from cache [key={0}]", KeyToMyStringWithVersion);
                }

                // Get another copy of the same object (used to remember the version)
                if ((cacheItem2 = myDefaultCache.GetCacheItem(KeyToMyStringWithVersion, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->GetCacheItem-Object Get from cache [key={0}]", KeyToMyStringWithVersion);
                }
                else
                {
                    Console.WriteLine("**FAIL----->GetCacheItem-Object did not Get from cache [key={0}]", KeyToMyStringWithVersion);
                }
                
                // Add a newer version of the object to the cache, supply the version as well to ensure that we are updating
                // the cache only if we have the latest version
                if ((cacheItemVersion = myDefaultCache.Put(KeyToMyStringWithVersion,
                                                       (object)cacheItem1.Value, cacheItem1.Version, myRegion)) != null)
                {
                    Console.WriteLine("PASS----->Put-Object updated successfully [key={0}]", KeyToMyStringWithVersion);
                    Console.WriteLine("          New version {0} Old version", cacheItemVersion > cacheItem2.Version ? ">" : "<=");
                }
                else
                {
                    Console.WriteLine("**FAIL----->Put-Object did not update successfully [key={0}]", KeyToMyStringWithVersion);
                }

                // Try to add an object when the version of the object in the Cache is newer, it will fail
                if ((cacheItemVersion = myDefaultCache.Put(KeyToMyStringWithVersion,
                                                       (object)cacheItem2.Value, cacheItem2.Version, myRegion)) != null)
                {
                    Console.WriteLine("**FAIL----->Put-Object update. Update to new version work.  [key={0}]", KeyToMyStringWithVersion);
                }
                else // this will throw a exception, so the else will not run if the object is locked.
                {
                    Console.WriteLine("PASS----->Put-Object did not update. Update to new version worked.  [key={0}]", KeyToMyStringWithVersion);
                }
            }
            catch (DataCacheException ex)
            {
                Console.WriteLine("PASS----->Put-Object-Expected behaviour since Object is newer");
                Console.Write("PASS----->Distributed Cache Generated Exception:");
                Console.WriteLine(ex.InnerException.Message);
            }

            // Testing simple Add/Get on a Region with Tags
            // without Version
            // Each object will have a unique key
            // Each object can have multiple tags, hence the tag[]
            // Multiple Objects can have the same tag
            Console.WriteLine("----------------------");
            Console.WriteLine("Testing Simple Add/GetByTag");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = " + myRegion);
            Console.WriteLine("Tags        = yes");
            Console.WriteLine("Version     = <none>");

            const int totalTags = 5;
            DataCacheTag[] someTags = new DataCacheTag[totalTags] { new DataCacheTag("Tag1"), new DataCacheTag("Tag2"), 
                                                               new DataCacheTag("Tag3"), new DataCacheTag("Tag4"), 
                                                              new DataCacheTag("Tag5") };
            List<DataCacheTag> allMyTags = someTags.ToList();
            IEnumerable<KeyValuePair<string, object>> getByTagReturnKeyValuePair;
            int totalObjects = 10;

            try
            {
                for (int objectid = 0; objectid < totalObjects; objectid++)
                {
                    // Add an object to the Cache with tags
                    if ((itemVersion = myDefaultCache.Add("MyKey" + objectid.ToString(),
                                                    (object)myObjectForCaching, allMyTags, myRegion)) != null)
                    {
                        Console.WriteLine("PASS----->Add-Object " +
                                                       "MyKey" + objectid.ToString() +
                                                       " added to Cache, with all tags");
                    }
                    else
                    {
                        Console.WriteLine("**FAIL----->Add-Object did not add to cache");
                    }
                }

                for (int objectid = 0; objectid < totalObjects; objectid++)
                {
                    for (int tagid = 0; tagid < totalTags; tagid++)
                    {
                        // Get the object from Cache using Tag
                        if ((getByTagReturnKeyValuePair = myDefaultCache.GetObjectsByTag(allMyTags[tagid], myRegion)) != null)
                        {
                            Console.WriteLine("PASS----->GetByTag-Object " +
                                                getByTagReturnKeyValuePair.ElementAt(tagid).Key +
                                                " get from cache. Using Tag " + tagid.ToString());
                        }
                        else
                        {
                            Console.WriteLine("**FAIL----->GetByTag-Object did not Get from cache");
                        }
                    }
                }
            }
            catch (DataCacheException ex)
            {
                Console.WriteLine("**FAIL----->Add-GetByTag-This is failing probably because you are running this sample test");
                Console.WriteLine("          within 10mins (default timeout)");
                Console.Write("**FAIL----->Distributed Cache Generated Exception:");
                Console.WriteLine(ex.ToString());

            }


            // Simple Load testing
            // Object size every line starts with 100 bytes and grows "exponentially".

            int iterateMax = 10;
            string lotOfData = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            Console.WriteLine("----------------------");
            Console.WriteLine("Testing Simple Add/Get");
            Console.WriteLine("Cache       = default");
            Console.WriteLine("Region      = " + myLoadTestRegion);
            Console.WriteLine("Tags        = <none>");
            Console.WriteLine("Version     = <none>");
            long totalSizeAdded = 0;
            long iterate;

            for (iterate = 0; iterate < iterateMax; iterate++)
            {
                try
                {
                    // Lets know how much we want to add to cache before the add
                    Console.Write(lotOfData.Length);
                    if ((itemVersion = myDefaultCache.Add(iterate.ToString(), (object)lotOfData, myLoadTestRegion)) != null)
                    {
                        Console.Write(" PASS----->Add" + iterate + " ");
                    }
                    else
                    {
                        Console.WriteLine("**FAIL----->Add-Object did not add to cache - FAIL");
                    }

                    if ((item = (string)myDefaultCache.Get(iterate.ToString(), myLoadTestRegion)) != null)
                    {
                        Console.WriteLine(item.Length + " PASS-->Get" + iterate + " ");
                    }
                    else
                    {
                        Console.WriteLine("**FAIL----->Get-Object did not Get from cache");
                        totalSizeAdded += lotOfData.Length;
                        lotOfData += lotOfData;
                    }
                }
                catch (DataCacheException ex)
                {
                    Console.WriteLine("**FAIL----->Add-Get-This is failing probably because you are running this sample test");
                    Console.WriteLine("         within 10mins (default timeout)");
                    Console.Write("**FAIL----->Distributed Cache Generated Exception:");
                    Console.WriteLine(ex.ToString());
                }
            }

            Console.WriteLine("Total Size added " + totalSizeAdded);
            Console.ReadKey();
        }

        private bool CreateRegion(string myRegion)
        {
            bool createRegionStatus = myDefaultCache.CreateRegion(myRegion);

            if (createRegionStatus == true)
            {
                Console.WriteLine("PASS--->CreateRegion " + myRegion);
            }
            else
            {
                Console.WriteLine("**FAIL--->CreateRegion-This is probably failing since you are creating a region");
                Console.WriteLine("          that already exists in the cache");
                Console.WriteLine();
                Console.WriteLine("Recovering from above failure");
                myDefaultCache.RemoveRegion(myRegion);
                Console.WriteLine("PASS--->RemoveRegion " + myRegion);

                createRegionStatus = myDefaultCache.CreateRegion(myRegion);

                if (createRegionStatus == true)
                {
                    Console.WriteLine("PASS--->CreateRegion " + myRegion);
                }
                else
                {
                    Console.WriteLine("**FAIL-->CreateRegion {0}: Aborting the program ", myRegion);
                }
            }

            return createRegionStatus;
        }

        private void PrepareClient()
        {
            //-------------------------
            // Configure Cache Client 
            //-------------------------

            //Define Array for 1 Cache Host
            List<DataCacheServerEndpoint> servers = new List<DataCacheServerEndpoint>(1);

            //Specify Cache Host Details 
            //  Parameter 1 = host name
            //  Parameter 2 = cache port number
            servers.Add(new DataCacheServerEndpoint("localhost", 22233));

            //Create cache configuration
            DataCacheFactoryConfiguration configuration = new DataCacheFactoryConfiguration();

            //Set the cache host(s)
            configuration.Servers = servers;

            //Set default properties for local cache (local cache disabled)
            configuration.LocalCacheProperties = new DataCacheLocalCacheProperties();

            //Disable exception messages since this sample works on a cache aside
            DataCacheClientLogManager.ChangeLogLevel(System.Diagnostics.TraceLevel.Off);

            //Pass configuration settings to cacheFactory constructor
            myCacheFactory = new DataCacheFactory(configuration);

            //Get reference to named cache called "default"
            myDefaultCache = myCacheFactory.GetCache("CacheId");
        }
    }
}
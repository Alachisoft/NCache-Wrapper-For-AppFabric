using Alachisoft.NCache.Data.Caching;
using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class OptimisticConcurrencyTests
    {
		internal static void GetExistingKeyItemVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing key item version from cache");

				var myKey = Guid.NewGuid().ToString();
				var addVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var obj = Program.myDefaultCache.Get(myKey, out DataCacheItemVersion version);

				if (version == addVersion)
				{
					Logger.PrintSuccessfulOutcome("Item version for existing item retrieved");
				}
				else
				{
					Logger.PrintFailureOutcome("Item version for existing item not equal to version it was added with");
				}
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

		internal static void GetNonExistingKeyItemVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting non existing key from cache");

				var myKey = Guid.NewGuid().ToString();
				var obj = Program.myDefaultCache.Get(myKey, out DataCacheItemVersion version);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item version for non existing item not null");
				}
				else
				{
					Logger.PrintFailureOutcome("Item version for non-existing item null");
				}
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

		internal static void GetExistingKeyWithOutdatedItemVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing key from cache with outdated version");

				var myKey = Guid.NewGuid().ToString();

				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				var newVersion = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching + 2);

				var obj = Program.myDefaultCache.Get(myKey, out oldVersion);

				if (obj != null && oldVersion == newVersion)
				{
					Logger.PrintSuccessfulOutcome("Item with updated version returned");
				}
				else
				{
					if (obj == null)
					{
						Logger.PrintFailureOutcome("Retrieval with old item version returned null");
					}
				}
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

		internal static void GetIfNewerObjectWithoutUpdate()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting null from cache using GetIfNewer for un-updated key");

				var myKey = Guid.NewGuid().ToString();
				var addVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var obj = Program.myDefaultCache.GetIfNewer(myKey, ref addVersion);

				if (obj == null)
				{
					Logger.PrintSuccessfulOutcome("Item not updated in cache so null returned");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not updated in cache but item gotten");
				}
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

		internal static void GetIfNewerObjectWithUpdate()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting newer key item version from cache with GetIfNewer");

				var myKey = Guid.NewGuid().ToString();

				var addVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var putVersion = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching);
				var obj = Program.myDefaultCache.GetIfNewer(myKey, ref addVersion);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item updated in cache so object returned with old version");
				}
				else
				{
					Logger.PrintFailureOutcome("Item updated in cache but null returned with old version");
				}
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

		internal static void GetIfNewerObjectWithNullVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting newer key item version from cache with GetIfNewer using null version");

				var myKey = Guid.NewGuid().ToString();
				var addVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				DataCacheItemVersion version = null;
				var obj = Program.myDefaultCache.GetIfNewer(myKey, ref version);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Existing object returned with null item version");
				}
				else
				{
					Logger.PrintFailureOutcome("Existing object not returned with null item version");
				}
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

		internal static void PutExistingKeyWithOutdatedVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting existing key-value pair in cache with outdated item version");

				var myKey = Guid.NewGuid().ToString();

				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				var newVersion = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching);

				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, oldVersion);

				if (version != null)
				{
					Logger.PrintFailureOutcome("Existing key-value pair with outdated version updated in cache");
				}
				else
				{
					Logger.PrintSuccessfulOutcome("Existing key-value pair with outdated version not added to cache");
				}
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

		internal static void PutExistingKeyWithNullVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Puting existing key-value pair in cache with null item version");

				var myKey = Guid.NewGuid().ToString();
				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, (DataCacheItemVersion)null);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Existing key-value pair with null version updated in cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Existing key-value pair with null version not added to cache");
				}
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

		internal static void PutNonExistingKeyWithNullVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Puting non-existing existing key-value pair in cache with null item version");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, (DataCacheItemVersion)null);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Non-existing key-value pair with null version added in cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Non-existing key-value pair with null version not added to cache");
				}
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


		internal static void RemoveExistingKeyWithOutdatedVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing key from cache with outdated version");

				var myKey = Guid.NewGuid().ToString();

				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				var newVersion = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching + 2);

				var result = Program.myDefaultCache.Remove(myKey, oldVersion);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation on existing key with outdated version successful");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation on existing key with outdated version failed");
				}
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


		internal static void RemoveExistingKeyWithNullVersion()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing existing key from cache with null version");

				var myKey = Guid.NewGuid().ToString();

				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);


				var result = Program.myDefaultCache.Remove(myKey, (DataCacheItemVersion)null);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation on existing key with null version successful");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation on existing key with null version failed");
				}
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

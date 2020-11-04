using Alachisoft.NCache.Data.Caching;
using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class GetInCacheTests
    {
        internal static void GetExistingKey()
        {
			try
			{
				Logger.PrintTestStartInformation("Getting existing key from cache");

				var myKey = Guid.NewGuid().ToString();
				Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var obj = Program.myDefaultCache.Get(myKey);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully gotten from cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not found in cache");
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

		internal static void GetNonExistingKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting non existing key from cache");

				var myKey = Guid.NewGuid().ToString();
				var obj = Program.myDefaultCache.Get(myKey);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Non-existing item gotten from cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Non-existing item not found in cache");
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

		internal static void GetNullKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting item against null key from cache");
				var obj = Program.myDefaultCache.Get(null);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item for null key not null");
				}
				else
				{
					Logger.PrintFailureOutcome("Item for null key null");
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
				
				var obj = Program.myDefaultCache.Get(myKey, out  oldVersion);

				if (obj != null && oldVersion == newVersion)
				{
					Logger.PrintSuccessfulOutcome("Item version for updated version returned");
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
	}
}

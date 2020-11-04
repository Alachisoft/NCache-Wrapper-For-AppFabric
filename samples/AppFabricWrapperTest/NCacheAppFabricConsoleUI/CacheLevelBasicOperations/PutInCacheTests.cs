using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class PutInCacheTests
    {
        internal static void PutNonExistingKey()
        {
			try
			{
				Logger.PrintTestStartInformation("Puting non-existing key-value pair in cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Non existing key-value pair added to cache");
				}	
				else
				{
					Logger.PrintFailureOutcome("Non existing key-value pair not added to cache");
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

		internal static void PutExistingKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Puting existing key-value pair in cache");

				var myKey = Guid.NewGuid().ToString();
				var oldVersion = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching);

				if (version != oldVersion)
				{
					Logger.PrintSuccessfulOutcome("Existing key-value pair successfully updated in cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Existing key-value pair not updated in cache");
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

		internal static void PutKeyWithNullValue()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting key-value pair in cache with null value");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Put(myKey, null);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("key-value pair with null value successfully added in cache");
				}
				else
				{
					Logger.PrintFailureOutcome("key-value pair with null value not added in cache");
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

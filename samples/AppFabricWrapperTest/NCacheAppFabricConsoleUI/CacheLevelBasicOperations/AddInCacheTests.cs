using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class AddInCacheTests
    {
        internal static void AddKeyValuePair()
        {
			try
			{
				Logger.PrintTestStartInformation("Adding non-existing key-value pair into cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to cache");
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

		internal static void AddExistingKeyValuePair()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding existing key-value pair to cache");
				var myKey = Guid.NewGuid().ToString();

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to cache");
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

		internal static void AddKeyValuePairWithNullObject()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non-existing key-value pair with null object to cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Add(myKey, null);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to cache");
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

		internal static void AddKeyValuePairWithNullKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding key-value pair with null key to cache");
				var version = Program.myDefaultCache.Add(null, Program.myObjectForCaching);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to cache");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to cache");
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

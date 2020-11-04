using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class RemoveInCacheTests
    {
        internal static void RemoveNonExistingKey()
        {
			try
			{
				Logger.PrintTestStartInformation("Removing non-existing key from cache");

				var myKey = Guid.NewGuid().ToString();
				var result = Program.myDefaultCache.Remove(myKey);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation on non-existing key successful");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation on non-existing key failed");
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

		internal static void RemoveExistingKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing existing key from cache");

				var myKey = Guid.NewGuid().ToString();
				Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

				var result = Program.myDefaultCache.Remove(myKey);

				var obj = Program.myDefaultCache.Get(myKey);

				if (result && obj == null)
				{
					Logger.PrintSuccessfulOutcome("Remove operation on existing key successful");
				}
				else
				{
					if (result && obj != null)
					{
						Logger.PrintFailureOutcome("Item not removed but remove result gave a fake success result"); 
					}

					if (!result && obj == null)
					{
						Logger.PrintFailureOutcome("Remove operation on existing key successful but remove result came out false");
					}

					if (!result && obj != null)
					{
						Logger.PrintFailureOutcome("Remove operation on existing key failed and item not removed");
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

		internal static void RemoveNullKey()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing null key from cache");


				var result = Program.myDefaultCache.Remove(null);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation on null key successful");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation on null key failed");
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

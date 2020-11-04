using System;

namespace NativeAppFabricConsoleUI
{
    internal static class ExpirationTests
    {
		internal static void PutKeyValuePairWithZeroTimeSpan()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting key-value pair with zero expiration timeout to cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, TimeSpan.Zero);

				var obj = Program.myDefaultCache.GetCacheItem(myKey);

				if (version != null)
				{
					if (obj != null)
					{
						var timeOut = obj.Timeout;
						Logger.PrintSuccessfulOutcome($"Item with zero expiration timeout successfully inserted into cache and is persisting with expiration {timeOut}");
					}
					else
					{
						Logger.PrintSuccessfulOutcome("Item with zero expiration timeout successfully inserted into cache but removed");
					}
				}
				else
				{
					Logger.PrintFailureOutcome("Item with zero expiration timeout could not be inserted into cache");
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

		internal static void PutKeyValuePairWithNegativeTimeSpan()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting key-value pair with negative expiration timeout to cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, TimeSpan.FromSeconds(-5));

				var obj = Program.myDefaultCache.GetCacheItem(myKey);

				if (version != null)
				{
					if (obj != null)
					{
						var timeOut = obj.Timeout;
						Logger.PrintSuccessfulOutcome($"Item with negative expiration timeout successfully inserted into cache with expiration {timeOut}");
					}
					else
					{
						Logger.PrintSuccessfulOutcome("Item with zero expiration timeout successfully inserted into cache but removed");
					}
				}
				else
				{
					Logger.PrintFailureOutcome("Item with negative expiration timeout could not be inserted into cache");
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

		internal static void AddKeyValuePairWithZeroTimeSpan()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding key-value pair with zero expiration timeout to cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, TimeSpan.Zero);

				var obj = Program.myDefaultCache.GetCacheItem(myKey);

				if (version != null)
				{
					if (obj != null)
					{
						var timeOut = obj.Timeout;
						Logger.PrintSuccessfulOutcome($"Item with zero expiration timeout successfully added into cache and is persisting with expiration {timeOut}");
					}
					else
					{
						Logger.PrintSuccessfulOutcome("Item with zero expiration timeout successfully added into cache but removed");
					}
				}
				else
				{
					Logger.PrintFailureOutcome("Item with zero expiration timeout could not be added into cache");
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

		internal static void AddKeyValuePairWithNegativeTimeSpan()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding key-value pair with negative expiration timeout to cache");

				var myKey = Guid.NewGuid().ToString();
				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, TimeSpan.FromSeconds(-5));

				var obj = Program.myDefaultCache.GetCacheItem(myKey);

				if (version != null)
				{
					if (obj != null)
					{
						var timeOut = obj.Timeout;
						Logger.PrintSuccessfulOutcome($"Item with negative expiration timeout successfully added into cache with expiration {timeOut}");
					}
					else
					{
						Logger.PrintSuccessfulOutcome("Item with zero expiration timeout successfully added into cache but removed");
					}
				}
				else
				{
					Logger.PrintFailureOutcome("Item with negative expiration timeout could not be added into cache");
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

		internal static void ResetObjectTimeoutOnNonExistingRegionItem()
		{
			try
			{
				Logger.PrintTestStartInformation("Resetting expiration timeout on key in non-existing region");

				var myRegion = Guid.NewGuid().ToString();
				var myKey = Guid.NewGuid().ToString();

				Program.myDefaultCache.ResetObjectTimeout(myKey, new TimeSpan(0, 10, 0), myRegion);

				Logger.PrintSuccessfulOutcome("Expiration reset successful");
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

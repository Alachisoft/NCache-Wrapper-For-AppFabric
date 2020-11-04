using System;
using System.Collections.Generic;
using System.Linq;

namespace NativeAppFabricConsoleUI
{
    internal static class BulkGetTests
    {
        internal static void BulkGetExistingItemsInExistingRegion()
        {
			try
			{
				Logger.PrintTestStartInformation("Getting existing items in bulk from existing region");

				var myKey = Guid.NewGuid().ToString();
				var myRegion = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);
				Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				var objects = Program.myDefaultCache.BulkGet(new[] { myKey }, myRegion);

				if (objects != null && objects.Count() == 1)
				{
					Logger.PrintSuccessfulOutcome("Items retrieved in bulk call from existing region");
				}
				else
				{
					Logger.PrintFailureOutcome("Items not retrieved from existing region");
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

		internal static void BulkGetItemsInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting items in bulk from non existing region");

				var myKey = Guid.NewGuid().ToString();
				var myRegion = Guid.NewGuid().ToString();

				var objects = Program.myDefaultCache.BulkGet(new[] { myKey }, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null enumerable returned as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("Zero length enumerable returned as expected");
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

		internal static void BulkGetZeroItemsFromExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting zero items in bulk from existing region");

				var myRegion = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				var objects = Program.myDefaultCache.BulkGet(new List<string>(), myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null enumerable returned as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("Zero length enumerable returned as expected");
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

		internal static void BulkGetZeroItemsInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting zero items in bulk from non existing region");

				var myRegion = Guid.NewGuid().ToString();

				var objects = Program.myDefaultCache.BulkGet(new List<string>(), myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null enumerable returned as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("Zero length enumerable returned as expected");
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

		internal static void BulkGetNullItemsFromExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting null items in bulk from existing region");

				var myRegion = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				var objects = Program.myDefaultCache.BulkGet(new string[] { null }, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null enumerable returned as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("Zero length enumerable returned as expected");
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

		internal static void BulkGetNullItemsInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting null items in bulk from non existing region");

				var myKey = Guid.NewGuid().ToString();
				var myRegion = Guid.NewGuid().ToString();

				var objects = Program.myDefaultCache.BulkGet(new string[] { null }, myRegion);

				if (objects == null)
				{
					Logger.PrintSuccessfulOutcome("Null enumerable returned as expected");
				}
				else if (objects.Count() == 0)
				{
					Logger.PrintSuccessfulOutcome("Zero length enumerable returned as expected");
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

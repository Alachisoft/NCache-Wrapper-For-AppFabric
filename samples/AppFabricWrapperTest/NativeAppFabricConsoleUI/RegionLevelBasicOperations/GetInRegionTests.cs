using System;

namespace NativeAppFabricConsoleUI
{
    internal static class GetInRegionTests
    {
		internal static void GetExistingKeyInRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting existing key from region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);
				var obj = Program.myDefaultCache.Get(myKey, myRegion);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully gotten from existing region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not found in existing region");
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

		internal static void GetKeyInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting key from non existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				var obj = Program.myDefaultCache.Get(myKey, myRegion);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully gotten from non existing region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not found in non existing region");
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

		internal static void GetKeyInNullRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Getting key from null region");

				string myRegion = null;
				string myKey = Guid.NewGuid().ToString();

				var obj = Program.myDefaultCache.Get(myKey, myRegion);

				if (obj != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully gotten from null region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item not found in null region");
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

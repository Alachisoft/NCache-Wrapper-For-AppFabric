using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class AddInRegionTests
    {
        internal static void AddKeyValuePairInExistingRegion()
        {
			try
			{
				Logger.PrintTestStartInformation("Adding non-existing key-value pair to existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to region");
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

		internal static void AddKeyValuePairInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non-existing key-value pair to non-existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to non existing region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to non existing region");
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

		internal static void AddKeyValuePairInNullRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Adding non-existing key-value pair to null region");

				string myRegion = null;
				string myKey = Guid.NewGuid().ToString();

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully added to null region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be added to null region");
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

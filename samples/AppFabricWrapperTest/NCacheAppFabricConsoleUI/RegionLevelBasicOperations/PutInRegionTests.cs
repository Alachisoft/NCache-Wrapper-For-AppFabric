using System;

namespace NCacheAppFabricConsoleUI
{
	internal static class PutInRegionTests
	{
		internal static void PutKeyValuePairInExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting non-existing key-value pair to existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully inserted in region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be inserted in region");
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

		internal static void PutKeyValuePairInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting non-existing key-value pair to non-existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				var version = Program.myDefaultCache.Put(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully inserted into non existing region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be inserted into non existing region");
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

		internal static void PutKeyValuePairInNullRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Putting non-existing key-value pair to null region");

				string myRegion = null;
				string myKey = Guid.NewGuid().ToString();

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				if (version != null)
				{
					Logger.PrintSuccessfulOutcome("Item successfully inserted into null region");
				}
				else
				{
					Logger.PrintFailureOutcome("Item could not be inserted into null region");
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

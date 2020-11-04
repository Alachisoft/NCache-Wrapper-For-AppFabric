using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class RemoveInRegionTests
    {
        internal static void RemoveExistingKeyInRegion()
        {
			try
			{
				Logger.PrintTestStartInformation("Removing existing key from existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				Program.myDefaultCache.CreateRegion(myRegion);

				var version = Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

				var result = Program.myDefaultCache.Remove(myKey, myRegion);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation successful on existing key in existing region");
				}
				else
				{
					Logger.PrintSuccessfulOutcome("Remove operation failed on existing key in existing region");
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

		internal static void RemoveKeyInNonExistingRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing key from non existing region");

				string myRegion = Guid.NewGuid().ToString();
				string myKey = Guid.NewGuid().ToString();

				var result = Program.myDefaultCache.Remove(myKey, myRegion);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation successful on key in non existant region");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation failed on key in non existant region");
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

		internal static void RemoveKeyInNullRegion()
		{
			try
			{
				Logger.PrintTestStartInformation("Removing key from null region");

				string myRegion = null;
				string myKey = Guid.NewGuid().ToString();

				var result = Program.myDefaultCache.Remove(myKey, myRegion);

				if (result)
				{
					Logger.PrintSuccessfulOutcome("Remove operation successful on key in null region");
				}
				else
				{
					Logger.PrintFailureOutcome("Remove operation failed on key in null region");
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

using System;

namespace NativeAppFabricConsoleUI
{
    internal static class ClearRegionTests
    {
        internal static void ClearExistingRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Clearing existing region");
                string myRegion = Guid.NewGuid().ToString();
                string myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.CreateRegion(myRegion);

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);

                Program.myDefaultCache.ClearRegion(myRegion);

                var obj = Program.myDefaultCache.Get(myKey, myRegion);


                if (obj == null)
                {
                    Logger.PrintSuccessfulOutcome($"Region {myRegion} successfully cleared");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Region {myRegion} could not be cleared");
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

        internal static void ClearNonExistingRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Clearing non existing region");
                string myRegion = Guid.NewGuid().ToString();

                Program.myDefaultCache.ClearRegion(myRegion);

                Logger.PrintSuccessfulOutcome($"Non existing region successfully cleared");
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

        internal static void ClearNullRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Clearing null region");
                string myRegion = null;

                Program.myDefaultCache.ClearRegion(myRegion);

                Logger.PrintSuccessfulOutcome($"Null region successfully cleared");
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

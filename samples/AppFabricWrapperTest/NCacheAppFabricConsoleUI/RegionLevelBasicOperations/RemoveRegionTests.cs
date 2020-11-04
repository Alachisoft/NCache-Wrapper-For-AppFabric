using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class RemoveRegionTests
    {
        internal static void RemoveExistingRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing existing region");
                string myRegion = Guid.NewGuid().ToString();
                string myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.CreateRegion(myRegion);

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching, myRegion);
                var result = Program.myDefaultCache.RemoveRegion(myRegion);

                var obj = Program.myDefaultCache.Get(myKey, myRegion);

                if (result && obj == null)
                {
                    Logger.PrintSuccessfulOutcome($"Existing region {myRegion} successfully removed along with all containing objects");
                }
                else
                {
                    if (result && obj != null)
                    {
                        Logger.PrintFailureOutcome($"Existing region {myRegion} removed but objects residing");
                    }
                    else if (!result && obj == null)
                    {
                        Logger.PrintFailureOutcome($"Existing region {myRegion} not removed but objects removed");
                    }
                    else
                    {
                        Logger.PrintFailureOutcome($"Existing region {myRegion} not removed and objects not removed");
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

        internal static void RemoveNonExistingRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing non-existing region");
                string myRegion = Guid.NewGuid().ToString();
                var result = Program.myDefaultCache.RemoveRegion(myRegion);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome($"Non existing region {myRegion} successfully removed");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Non existing region {myRegion} not removed");
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

        internal static void RemoveRegionWithNullInput()
        {
            try
            {
                Logger.PrintTestStartInformation("Removing region with null name");
                string myRegion = null;
                var result = Program.myDefaultCache.RemoveRegion(myRegion);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome($"Region with null name successfully removed");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Region with null name not removed");
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

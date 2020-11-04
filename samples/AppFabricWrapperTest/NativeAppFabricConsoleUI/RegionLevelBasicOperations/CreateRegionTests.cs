using System;

namespace NativeAppFabricConsoleUI
{
    internal static class CreateRegionTests
    {
        internal static void CreateRegionWithNonEmptyString()
        {
            try
            {
                Logger.PrintTestStartInformation("Creating Region with non-empty string value");
                string myRegion = Guid.NewGuid().ToString();
                var result = Program.myDefaultCache.CreateRegion(myRegion);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome($"Region {myRegion} successfully created");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Region {myRegion} could not be created");
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

        internal static void RecreateRegionWithNonEmptyString()
        {
            try
            {
                Logger.PrintTestStartInformation("Creating Another Region with Same Name");
                string myRegion = Guid.NewGuid().ToString();
                var result = Program.myDefaultCache.CreateRegion(myRegion);
                result = Program.myDefaultCache.CreateRegion(myRegion);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome($"Region {myRegion} successfully recreated");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Region {myRegion} could not be recreated");
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

        internal static void CreateRegionWithNullString()
        {
            try
            {
                Logger.PrintTestStartInformation("Creating Another Region with NULL Region Name");
                string myRegion = null;
                var result = Program.myDefaultCache.CreateRegion(myRegion);

                if (result)
                {
                    Logger.PrintSuccessfulOutcome($"Region successfully created");
                }
                else
                {
                    Logger.PrintFailureOutcome($"Region could not be created");
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

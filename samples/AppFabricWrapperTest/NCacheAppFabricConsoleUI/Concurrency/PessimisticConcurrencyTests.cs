using Alachisoft.NCache.Data.Caching;
using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class PessimisticConcurrencyTests
    {
        internal static void GetLockOnExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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

        internal static void GetLockOnNonExistingKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting non existing item from cache and acquiring lock on it");

                var myKey = Guid.NewGuid().ToString();
                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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

        internal static void GetLockOnLockedKey()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting locked object and acquiring lock on it");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                var obj2 = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle2);

                if (lockHandle2 != null && obj2 != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and re-locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not re-locked");
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

        internal static void GetLockWithNegativeLockTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it with negative lock timeout");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(-5), out DataCacheLockHandle lockHandle);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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

        internal static void GetLockWithZeroLockTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it with zero lock timeout");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.Zero, out DataCacheLockHandle lockHandle);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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

        internal static void GetLockOnNonExistingRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting item from non existing region and acquiring lock on it");

                var myRegion = Guid.NewGuid().ToString();
                var myKey = Guid.NewGuid().ToString();
                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle, myRegion);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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

        internal static void GetLockOnNullRegion()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting item from null region and acquiring lock on it");

                var myKey = Guid.NewGuid().ToString();
                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle, null);

                if (lockHandle != null && obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked");
                }
                else
                {
                    if (lockHandle != null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Locked but no object retrieved");
                    }

                    if (lockHandle == null && obj != null)
                    {
                        Logger.PrintFailureOutcome("Object retrieved but lockhandle null");
                    }

                    if (lockHandle == null && obj == null)
                    {
                        Logger.PrintFailureOutcome("Object not retrieved and not lockhandle");
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


        internal static void PutAndUnlockWithValidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it using PutAndUnlock");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                var version = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, lockHandle);

                if (version != null)
                {
                    Logger.PrintSuccessfulOutcome("Object inserted and unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void PutAndUnlockWithValidLockHandleNegativeExpirationTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with negative expiration timeout using PutAndUnlock");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                var version = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, lockHandle, TimeSpan.FromSeconds(-5));

                if (version != null)
                {
                    Logger.PrintSuccessfulOutcome("Object inserted and unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void PutAndUnlockWithValidLockHandleZeroLockTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with zero expiration timeout using PutAndUnlock");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                var version = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, lockHandle, TimeSpan.Zero);

                if (version != null)
                {
                    Logger.PrintSuccessfulOutcome("Object inserted and unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void PutAndUnlockWithInvalidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with invalid lock handle using PutAndUnlock");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);


                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                var version = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, lockHandle);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle2);

                var version2 = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, lockHandle);

                if (version != null)
                {
                    Logger.PrintSuccessfulOutcome("Object inserted and unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void PutAndUnlockWithNullLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with null lock handle using PutAndUnlock");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);


                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                var version = Program.myDefaultCache.PutAndUnlock(myKey, Program.myObjectForCaching, null);

                if (version != null)
                {
                    Logger.PrintSuccessfulOutcome("Object inserted and unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void UnlockWithValidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with unlock call using Unlock");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Unlock(myKey, lockHandle);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out lockHandle);

                if (obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked after unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void UnlockWithValidLockHandleNegativeExpirationTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with unlock call with negative expiration timeout");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Unlock(myKey, lockHandle, TimeSpan.FromSeconds(-5));

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out lockHandle);

                if (obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked after unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void UnlockWithValidLockHandleZeroLockTimeout()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with unlock call with zero expiration timeout");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Unlock(myKey, lockHandle, TimeSpan.Zero);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out lockHandle);

                if (obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked after unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void UnlockWithInvalidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with unlock call using invalid lock handle");

                var myKey = Guid.NewGuid().ToString();
                var myOtherKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(65), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Unlock(myKey, lockHandle);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(65), out DataCacheLockHandle lockHandle2);

                Program.myDefaultCache.Unlock(myKey, lockHandle);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(65), out lockHandle);

                if (obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked after unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void UnlockWithNullLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with unlock call using null lock handle");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);


                Program.myDefaultCache.Unlock(myKey, null);

                obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out lockHandle);

                if (obj != null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked after unlocked");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not unlocked");
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

        internal static void RemoveWithValidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with remove unlock call");

                var myKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);

                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(5), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Remove(myKey, lockHandle);

                obj = Program.myDefaultCache.Get(myKey);

                if (obj == null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked and then removed with lock");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not removed with lock");
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

        internal static void RemoveWithInvalidLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with remove unlock call using invalid lock handle");

                var myKey = Guid.NewGuid().ToString();
                var myOtherKey = Guid.NewGuid().ToString();
                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);
                Program.myDefaultCache.Add(myOtherKey, Program.myObjectForCaching);


                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                var obj2 = Program.myDefaultCache.GetAndLock(myOtherKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle2);

                var result = Program.myDefaultCache.Remove(myKey, lockHandle2);

                obj = Program.myDefaultCache.Get(myKey);

                if (obj == null)
                {
                    Logger.PrintSuccessfulOutcome($"Object retrieved and locked and then removed with invalid lock. RemoveWithLockResult:{result}");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not removed with invalid lock");
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

        internal static void RemoveWithNullLockHandle()
        {
            try
            {
                Logger.PrintTestStartInformation("Getting existing key item from cache and acquiring lock on it and then unlocking it with remove unlock call using null lock handle");

                var myKey = Guid.NewGuid().ToString();

                Program.myDefaultCache.Add(myKey, Program.myObjectForCaching);


                var obj = Program.myDefaultCache.GetAndLock(myKey, TimeSpan.FromSeconds(15), out DataCacheLockHandle lockHandle);

                Program.myDefaultCache.Remove(myKey, (DataCacheLockHandle)null);

                obj = Program.myDefaultCache.Get(myKey);

                if (obj == null)
                {
                    Logger.PrintSuccessfulOutcome("Object retrieved and locked and then removed");
                }
                else
                {
                    Logger.PrintFailureOutcome("Object not removed with lock");
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

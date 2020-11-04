
namespace NCacheAppFabricConsoleUI
{
    internal static class ErrorCodeLookup
    {
        internal static string Description(int errorCode)
        {
            string description = "";
            switch (errorCode)
            {
                case 1:
                    {
                        description = "CacheItemVersionMismatch";
                    }
                    break;
                case 2:
                    {
                        description = "RegistryKeyOpenFailure";
                    }
                    break;
                case 3:
                    {
                        description = "InvalidArgument";
                    }
                    break;
                case 4:
                    {
                        description = "UndefinedError";
                    }
                    break;
                case 5:
                    {
                        description = "RegionDoesNotExist";
                    }
                    break;
                case 6:
                    {
                        description = "KeyDoesNotExist";
                    }
                    break;
                case 7:
                    {
                        description = "RegionAlreadyExists";
                    }
                    break;
                case 8:
                    {
                        description = "KeyAlreadyExists";
                    }
                    break;
                case 9:
                    {
                        description = "NamedCacheDoesNotExist";
                    }
                    break;
                case 10:
                    {
                        description = "MaxNamedCacheCountExceeded";
                    }
                    break;
                case 11:
                    {
                        description = "ObjectLocked";
                    }
                    break;
                case 12:
                    {
                        description = "ObjectNotLocked";
                    }
                    break;
                case 13:
                    {
                        description = "InvalidCacheLockHandle";
                    }
                    break;
                case 14:
                    {
                        description = "InvalidEnumerator";
                    }
                    break;
                case 15:
                    {
                        description = "NotificationInvalidationNotSupported";
                    }
                    break;
                case 16:
                    {
                        description = "ConnectionTerminated";
                    }
                    break;
                case 17:
                    {
                        description = "RetryLater";
                    }
                    break;
                case 18:
                    {
                        description = "Timeout";
                    }
                    break;
                case 19:
                    {
                        description = "ClientServerVersionMismatch";
                    }
                    break;
                case 20:
                    {
                        description = "SerializationException";
                    }
                    break;
                case 21:
                    {
                        description = "ServerNull";
                    }
                    break;
                case 22:
                    {
                        description = "OperationNotSupported";
                    }
                    break;

            }

            return description;
        }
    }
}

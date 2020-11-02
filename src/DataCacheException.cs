using System;
using System.Runtime.Serialization;

namespace Alachisoft.NCache.Data.Caching
{
    [Serializable]
    public class DataCacheException : Exception, ISerializable
    {
        public DataCacheException()
        { }

        public DataCacheException(string message) : base(message)
        {
            Message = message;
        }

        public DataCacheException(string message, Exception exception) : base(message, exception)
        {
            Message = message;
        }

        protected DataCacheException(SerializationInfo _info, StreamingContext _context)
        { }


        public int ErrorCode { get; internal set; }
        public int SubStatus { get; internal set; } = -1;
        public override string Message { get; }

    }
}

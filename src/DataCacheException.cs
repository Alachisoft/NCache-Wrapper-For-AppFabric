using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Alachisoft.NCache.Data.Caching
{
    /// <summary>
    /// Used for cache-related exceptions. 
    /// </summary>\
    [Serializable]
    public class DataCacheException : Exception, ISerializable
    {
        #region [   Constructors   ]
        public DataCacheException()
        {
        }
        public DataCacheException(string reason) : base(reason) 
        { }
        public DataCacheException(string reason, Exception innerException) : base(reason, innerException) 
        { }
        protected DataCacheException(SerializationInfo _info, StreamingContext _context) 
        { }
        #endregion

        public int ErrorCode { get; set; }
        public int SubStatus { get; set; }
        public override string HelpLink { get; set; }
        public  string Message { get; set; }
    }
}

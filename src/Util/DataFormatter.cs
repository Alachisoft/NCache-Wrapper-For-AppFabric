using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alachisoft.NCache.Web.Caching;
using Alachisoft.NCache.Runtime.Caching;
using System.Collections;
using System.Collections.ObjectModel;
namespace Alachisoft.NCache.Data.Caching
{
    /// <summary>
    /// class that converts user data to NCache Specific format
    /// </summary>
   internal class DataFormatter
   {
       
       internal string[] _delimeter;// delimeter for extraction; this also acts as suffix
       internal string _defaultRegion;// name of the default region
       internal string _prefix; // character to be appended before the region

       internal DataFormatter() 
       {
          
           _delimeter = new string[1];
           _delimeter[0] = "$~";
           _defaultRegion = "_Default";
           _prefix = "$";
       }
       
       #region [Key formatter Functions]
       internal string MarshalKey(string key)
       {
           string _transformedKey = _prefix + _defaultRegion + _delimeter[0] + key;
           return _transformedKey;
       }
       
       internal string[] MarshalKey(IEnumerable<string> keys,string region)
       {
           List<String> _tempKeyList = new List<String>();
           IEnumerator _enumerator = keys.GetEnumerator();
           while (_enumerator.MoveNext())
           {
               if (string.IsNullOrWhiteSpace(region))
               {
                   _tempKeyList.Add(MarshalKey((string)_enumerator.Current));
               }
               else 
               {
                   _tempKeyList.Add(MarshalKey((string)_enumerator.Current, region));
               }
           }
           string[] keyList = _tempKeyList.ToArray();
           return keyList;
       }


       internal string MarshalKey(string key,string region)
       {
           string _transformedKey;
           if (!string.IsNullOrWhiteSpace(region))
           {
               _transformedKey = _prefix + region + _delimeter[0] + key;
           }
           else
           {
               _transformedKey = MarshalKey(key);
           }
               return _transformedKey;
       }

       internal string UnMarshalKey(string key)
       {
           string[] _splitList = key.Split(_delimeter,StringSplitOptions.RemoveEmptyEntries);
           return _splitList[1];
       }
       #endregion

       #region [Tag formatter Functions]
       /// <summary>
       /// creates region tag for the functions with no region specified
       /// </summary>
       /// <returns></returns>
       internal string returnDefaultRegionTag()
       {
          
           return _defaultRegion;
       }
       internal Tag MarshalTag(DataCacheTag dataCacheTag,string region)
       {
           Tag tag;
           if (dataCacheTag != null)
           {
               
               Tag simpleTag = ConvertToNCacheTag(dataCacheTag);
               if (string.IsNullOrWhiteSpace(region))
               {

                   tag = new Tag(_prefix + _defaultRegion + _delimeter[0] + simpleTag.ToString());

               }
               else
               {

                   tag = new Tag(_prefix + region + _delimeter[0] + simpleTag.ToString());

               }
               return tag;
           }
           else 
           {
               return null;
           }
          
 
       }
       internal Tag[] MarshalTags(IEnumerable<DataCacheTag> dataCachaTag,string region)
       {
           if (dataCachaTag != null)
           {
               Tag[] simpleTags = ConvertToNCacheTag(dataCachaTag);
               Tag[] returningTags = new Tag[simpleTags.Length + 1];
               if (string.IsNullOrWhiteSpace(region))
               {
                   returningTags[0] = new Tag(_defaultRegion);
                   for (int i = 0; i < simpleTags.Length; i++)
                   {
                       returningTags[i + 1] = new Tag(_prefix + _defaultRegion + _delimeter[0] + simpleTags[i].ToString());
                   }
               }
               else
               {
                   returningTags[0] = new Tag(region);
                   for (int i = 0; i < simpleTags.Length; i++)
                   {
                       returningTags[i + 1] = new Tag(_prefix + region + _delimeter[0] + simpleTags[i].ToString());
                   }
               }
               return  returningTags;
           }
           else 
           {
               Tag[] regionTag = new Tag[1];
               if (string.IsNullOrWhiteSpace(region))
               {
                   regionTag[0] = new Tag(_defaultRegion);
               }
               else 
               {
                   regionTag[0] = new Tag(region);
               }
               return regionTag;
           }
           
       }

      

       internal DataCacheTag[] UnMarshalTags(Tag[] tags)
       {
           for (int i = 0; i < tags.Length; i++)
           {
               string[] _splitList = tags[i].ToString().Split(_delimeter, StringSplitOptions.RemoveEmptyEntries);
               tags[i] = new Tag(_splitList[0]);
           }
           DataCacheTag[] _dataCachaTag = ConvertToDataCacheTag(tags);
           return _dataCachaTag;
       }
       #endregion

       internal string[] SplitKeyAndRegion(string key)
       {
           string[] keyList = key.Split(_delimeter, StringSplitOptions.RemoveEmptyEntries);
           string[] regionSplit = keyList[0].Split('~');
           string[] returnList = new string[2];
           returnList[0] = keyList[1];// key on 0 index
           returnList[1] = regionSplit[0];// region on index 1
           return returnList;
       }
       
       #region [DataCacheTag- NCache Tag]

       internal Tag[] ConvertToNCacheTag(IEnumerable<DataCacheTag> dataCachaTag)
       {
           List<Tag> _tag = new List<Tag>();
           try
           {
               //get Stringed tags from dataCachetag
               foreach (DataCacheTag _temp in dataCachaTag)
               {
                   _tag.Add(_temp._tag);
               }

               //
               //Converting arraylist to tagList
               //

               Tag[] tempList = _tag.ToArray();
               

               return tempList;
           }
           catch (Exception exp)
           {
               throw exp;
           }
       }

       internal Tag ConvertToNCacheTag(DataCacheTag dataCachaTag)
       {
           try
           {
               Tag _NCacheTag = dataCachaTag._tag;
               return _NCacheTag;
           }
           catch (Exception exp)
           {
               throw exp;
           }
       }

       internal DataCacheTag[] ConvertToDataCacheTag(Tag[] tags)
       {
           List<string> _nCacheTag = new List<string>();
           foreach (Tag _tag in tags)
           {
               _nCacheTag.Add(_tag.ToString());
           }
           String[] tempList = _nCacheTag.ToArray();
           DataCacheTag[] DataCacheTagList = new DataCacheTag[tempList.Length];

           for (int i = 0; i < tempList.Length; i++)
           {
               DataCacheTagList[i] = new DataCacheTag(tempList[i]);
           }
           return DataCacheTagList;
       }
       #endregion

       #region [DataCacheLockHandle- NCache LockHandle]
       public LockHandle ConvertToNCacheLockHandle(DataCacheLockHandle dLockHandle)
       {
           LockHandle _lockHandle = new LockHandle();
           _lockHandle = dLockHandle._lockHandle;

           return _lockHandle;
       }
       #endregion

       #region [DataCacheItem- NCache CacheItem]

       internal CacheItem CreateCacheItem(object value, IEnumerable<DataCacheTag> tags, string region, TimeSpan timeOut)
       {
           CacheItem _item = new CacheItem(value);

           if (tags == null && String.IsNullOrWhiteSpace(region))
           {
               _item.Tags = MarshalTags(null, null);
               _item.Group = returnDefaultRegionTag();
           }
           else if (tags == null && !String.IsNullOrWhiteSpace(region))
           {
               _item.Tags = MarshalTags(null, region);
               _item.Group = region;
           }
           else
           {
               _item.Tags = MarshalTags(tags, null);
               _item.Group = returnDefaultRegionTag();
           }

           if (timeOut == TimeSpan.Zero)
           {
               _item.AbsoluteExpiration = System.DateTime.Now.AddMinutes(10.0);
           }
           else
           {

               _item.AbsoluteExpiration = DateTime.Now.Add(timeOut);
           }
           return _item;
       }

       internal CacheItem CreateCacheItem(object value, IEnumerable<DataCacheTag> tags, string region)
       {
           CacheItem _item = new CacheItem(value);

           if (tags == null && String.IsNullOrWhiteSpace(region))
           {
               _item.Tags = MarshalTags(null, null);
               _item.Group = returnDefaultRegionTag();
           }
           else if (tags == null && !String.IsNullOrWhiteSpace(region))
           {
               _item.Tags = MarshalTags(null, region);
               _item.Group = region;
           }
           else if (tags != null && String.IsNullOrWhiteSpace(region))
           {
               _item.Tags = MarshalTags(tags, null);
               _item.Group = returnDefaultRegionTag();
           }
           else
           {
               _item.Tags = MarshalTags(tags, region);
               _item.Group = returnDefaultRegionTag();
           }
           return _item;
       }

       private TimeSpan ConvertToTimeSpan(DateTime date)
       {
           TimeSpan _timeSpan;
           
           DateTime baseDate = new DateTime();
           baseDate = DateTime.Now;
           _timeSpan = date - baseDate;
           return _timeSpan;
       }

       public DataCacheItem ConvertToDataCacheItem(CacheItem cItem)
       {
           DataCacheItem _dataCacheItem = new DataCacheItem();
           _dataCacheItem.Tags = new ReadOnlyCollection<DataCacheTag>(UnMarshalTags(cItem.Tags));
           _dataCacheItem.Timeout = ConvertToTimeSpan(cItem.AbsoluteExpiration);
           _dataCacheItem.Version = ConvertToAPVersion(cItem.Version);
           _dataCacheItem.Value = cItem.Value;
           _dataCacheItem.RegionName = cItem.Group;
           return _dataCacheItem;

       }
       #endregion

       #region [DataCacheItemVersion- NCache ItemVersion]
       public DataCacheItemVersion ConvertToAPVersion(CacheItemVersion cItemVersion)
       {
           DataCacheItemVersion _dcVersion = new DataCacheItemVersion();
           _dcVersion._itemVersion = cItemVersion;
           return _dcVersion;
       }

       public CacheItemVersion ConvertToNCacheVersion(DataCacheItemVersion dItemVersion)
       {
           CacheItemVersion _nCVersion = dItemVersion._itemVersion;
           return _nCVersion;
       }
       #endregion
   }
}

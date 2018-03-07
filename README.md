# NCache Wrapper For AppFabric

## Introduction
NCache is a powerful distributed .NET caching solution. To use NCache as the caching solution to your existing AppFabric application simply
follow the minimalistic instructions in this guide.

## Prerequisites

**STEP 1:**

Add the library "Alachisoft.NCache.Data.Caching.dll" to the references of your application and remove the following libraries:

``` batchfile
- "Microsoft.ApplicationServer.Caching.Client.dll"

- "Microsoft.ApplicationServer.Caching.Core.dll"
```


**STEP 2:**

Find the following lines in your project and replace them with *Alachisoft.NCache.Data.Caching*.

	-- "Microsoft.ApplicationServer.Caching;"
	OR
	-- " Microsoft.ApplicationServer.Caching.Client;"
	-- " Microsoft.ApplicationServer.Caching.Core;"

**STEP 3:**

Add the following `<appSettings>` tag in your app.config or web.config file in your project.

```xml
<appSettings>
	<add key="CacheId" value="mycache"/> <!-- This is the name of the cache-->
	<add key="Expirable" value="False"/> <!-- Default flag whether items added in cache should be expirable or permenant -->
	<add key="TTL" value="6:12:14"/> <!-- Expiration time in Hour, Minutes, Seconds format for TimeSpan -->
</appSettings>
```

That is it! Now build your solution and your application will be using NCache as its cache store.

**NOTE:** The regions are like caches in NCache so in order to use your existing regions you must register the regions in NCache as caches.
For e.g. If you have a region named "default" currently working in your application then you should create a cache by the name "default"
using NCache Manager or manually editing configurations (If using Community/OpenSource version).

"Alachisoft.NCache.Sdk" nuget package is currently referred in the NCacheWrapperForAppFabric project that is used for Enterprise edition, in order to use this Nuget with OpenSource/Community edition you need to refer Alachisoft.NCache.OPenSource.SDK/Alachisoft.NCache.Community.SDK  respectively.

## Additional Resources

### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

### Programmers' Guide
The complete programmers guide of NCache is available at:
http://www.alachisoft.com/resources/docs/ncache/ncache-programmers-guide.pdf

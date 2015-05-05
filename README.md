# NCache Wrapper For AppFabric
AppFabric NCache Wrapper Guide:

NCache is a powerful distributed .NET caching solution. To use NCache as the caching solution to your existing AppFabric application simply 
follow the minimalistic instructions in this guide.

STEP 1:

Add the library "Alachisoft.NCache.Data.Caching.dll" to the references of your application and remove the following libraries:

	-- "Microsoft.ApplicationServer.Caching.Client.dll"
	-- "Microsoft.ApplicationServer.Caching.Core.dll"
	
STEP 2:

Find the following lines in your project and replace them with "using Alachisoft.NCache.Data.Caching;"

	--"using Microsoft.ApplicationServer.Caching;" 
						OR
	--"using Microsoft.ApplicationServer.Caching.Client;"
	--"using Microsoft.ApplicationServer.Caching.Core;"
	
STEP 3: 

Add the following <appSettings> tag in your app.config or web.config file in your project.
	
	<appSettings>
    		<add key="mycache" value="mycache"/> <!-- This is the name of the cache, key and value should be same -->
    		<add key="Expirable" value="False"/> <!-- Default flag whether items added in cache should be expirable or permenant -->
  	</appSettings>

That is it! Now build your solution with .NET framework 4 and your applciation will be using NCache.

NOTE: The regions are like caches in NCache so in order to use your existing regions you must register the regions in NCache as caches.
For eg. If you have a region named "default" currently working in your application then you should register a cache by the name "default" 
using NCache Manager or command line tools( if using Open Source version). Configure the cache and set the policies through the command line 
tools as NCache cofiguration are compile-time unlike AppFabrics runtime configurations.
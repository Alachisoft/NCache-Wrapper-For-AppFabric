# NCache Wrapper For AppFabric

### Table of contents

* [Introduction](#introduction)
* [Prerequisites To Using the NCache AppFabric Wrapper](#prerequisites-to-using-the-ncache-appfabric-wrapper)
* [Running the Sample Application](#running-the-sample-application)
* [Additional Resources](#additional-resources)
* [Technical Support](#technical-support)
* [Copyrights](#copyrights)

### Introduction
NCache is a powerful distributed .NET caching solution. To use NCache as the caching solution to your existing AppFabric application simply
follow the minimalistic instructions in this guide.

### Prerequisites To Using the NCache AppFabric Wrapper

- **STEP 1:**
   
   Install [AppFabric.Wrapper.NCache](https://www.nuget.org/packages/AppFabric.Wrapper.NCache/) NuGet in your application. After installing the Nuget package in your AppFabric application, [**client.ncconf**](https://www.alachisoft.com/resources/docs/ncache/admin-guide/client-config.html) and [**config.ncconf**](https://www.alachisoft.com/resources/docs/ncache/admin-guide/config-ncconf.html) files will be included in your project. 

  The project **config.ncconf** file is used to configure a [local in-process NCache server](https://www.alachisoft.com/resources/docs/ncache/admin-guide/local-cache.html) that can be used to test your application without have to install NCache. For out-of-process local caches and clustered caches, the **config.ncconf** files used in the cache servers of your cache cluster will reside in the **%NCHOME%/Config** folder of each of the servers. Here **%NCHOME%** refers to the NCache installation directory e.g. **C:\Program Files\NCache**.

- **STEP 2:**

  The **client.ncconf** file can be used to configure the cache client for accessing the NCache cluster. This is optional in the case where NCache installation is available on the client machine whereby the client.ncconf file will be found in **%NCHOME%\Config** folder. 

  When the application is run, the NCache client library that comes with the NCache SDK will first look in the project folder for the **client.ncconf** file. Failing to find it there, it will then look in the NCache installation directory. 

  In either case, the **client.ncconf** file is crucial to configuring the underlying NCache cache handle which will be used to perform the actual NCache CRUD operations. At the very least, the **client.ncconf** file should have an entry for the Id of the cache that will be used together with the IP address of atleast one of the cache servers.

  An example of a **client.ncconf** file can be found in the [sample application](./samples/cache-api-sample/CacheAPISample/).

  **STEP 3:**

  Make the following changes in your application ***.csproj*** and ***.cs*** files:

  - Remove the references to the following AppFabric libraries:
    ```batchfile
    - Microsoft.ApplicationServer.Caching.Client.dll
    - Microsoft.ApplicationServer.Caching.Core.dll
    ```
  - Replace the following namespaces with Alachisoft.NCache.Data.Caching in the ***using*** statements of your source files:

    ```batchfile
    - Microsoft.ApplicationServer.Caching
    - Microsoft.ApplicationServer.Caching.Client
    - Microsoft.ApplicationServer.Caching.Core
    ```

- **STEP 4:**

  Add the following keys and values to the ***appSettings*** section of your app.config or web.config file:

  - Key = “CacheId”	Value = “\<The name of the cache>”

    This is the Id of the cache that will be used. Make sure that the **client.ncconf** file has an entry with this Id so as to be able to configure a cache client to access the cache. This is required if your application is explicitly calling a named cache. 

    In your app fabric application, using the [**DataCacheFactory**](./src/DataCacheFactory.cs) 
  ***GetCache(string cacheName)*** method, use ***CacheId*** string value as the argument.

  - Key = “Default” &nbsp;&nbsp;&nbsp;&nbsp; Value = “\<The name of the default cache>”

    This is the Id of the default cache that will be used. Make sure that the client.ncconf file has an entry with this Id so as to be able to configure a cache client to access the cache. 
    This is required if your application is calling the default cache using the [**DataCacheFactory**](./src/DataCacheFactory.cs) 
  ***GetDefaultCache()*** method.

  - Key = “Expirable” &nbsp;&nbsp;&nbsp;&nbsp; Value = “True” | “False”
            
    This is a Boolean value flag that determines whether the cached objects written to from the client application should have expiry set on them or not. Setting this value to ***True*** adds expiry to cached items while ***False*** will have the cached object without an expiration set on them.

  - Key = “TTL” &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Value = “\<hh>:\<mm>:\<ss>”
 
    This is the expiration time set on cached items with the format of ***hrs : minutes : seconds***. TTL has no effect if ***Expirable*** is set to ***False***. If the ***Expirable*** key is set to ***True*** and no TTL is given, then the default expiration time of **10 min** will be used on the cached items.

- **STEP 5:**

   Make sure that the cache with the Id given in the **client.ncconf** file with the given cache server configurations (IP addresses and ports) is created and running. If you are using a local in-process cache as configured in the project **config.ncconf** file, then that server is already available and there is no need to create and run it.


Following these steps, you should now be able to run your AppFabric application using NCache as the distributed caching solution.

### Running the Sample Application

In order to demonstrate the use of the NCache AppFabric Wrapper library, you can use the accompanying [sample](./samples/cache-api-sample/).
The sample uses an in-proc local cache configured using the project      [**config.ncconf**](./samples/cache-api-sample/CacheAPISample/config.ncconf) file.

Before running the application, replace the cache server name given by the cache/server tag with you the IP address of your machine in the project [**client.ncconf**](./samples/cache-api-sample/CacheAPISample/client.ncconf) file:
```xml

<?xml version="1.0" encoding="UTF-8"?>
  <configuration>
    <ncache-server 
	connection-retries="5" 
	retry-connection-delay="0" 
	retry-interval="1" 
	command-retries="3" 
	command-retry-interval="0.1" 
	client-request-timeout="90" 
	connection-timeout="5" port="9800"/>
    <cache 
	id="democache" 
	client-cache-id="" 
	client-cache-syncmode="optimistic" 
	default-readthru-provider="" 
	default-writethru-provider="" 
	load-balance="True" 
	enable-client-logs="False" 
	log-level="error">
    <server 
	name="**.**.**.**"/>
    </cache>
  </configuration>

``` 
If you want to test the application against a remote NCache cluster, modify the project **client.ncconf** file by replacing *myCache* value in the cache id attribute with the name of the clustered cache and replace the server name attribute with the IP address of one of the cache servers.



## Additional Resources

### Documentation
The complete online documentation for NCache is available at:
http://www.alachisoft.com/resources/docs/#ncache

### Programmers' Guide
The complete programmers guide of NCache is available at:
https://www.alachisoft.com/resources/docs/ncache/prog-guide/

### Technical Support

Alachisoft &copy; provides various sources of technical support. 

- Please refer to http://www.alachisoft.com/support.html to select a support resource you find suitable for your issue.
- To request additional features in the future, or if you notice any discrepancy regarding this document, please drop an email to [support@alachisoft.com](mailto:support@alachisoft.com).

### Copyrights

&copy; 2020 Alachisoft 

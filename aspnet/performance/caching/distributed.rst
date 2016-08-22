:version: 1.0.0

Working with a Distributed Cache
================================

分布式缓存使用
================================

By `Steve Smith`_

作者：`Steve Smith`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

Distributed caches can improve the performance and scalability of ASP.NET Core apps, especially when hosted in a cloud or server farm environment. This article explains how to work with ASP.NET Core's built-in distributed cache abstractions and implementations.

分布式缓存可以提升ASP.NET Core应用的性能和可伸缩性，特别是部署到云服务器或服务器群环境。这篇文章介绍如何使用ASP.NET Core内置的抽象和实现的分布式缓存。

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/distributed/sample>`__

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/distributed/sample>`__

What is a Distributed Cache
---------------------------

什么是分布式缓存
------------------

A distributed cache is shared by multiple app servers (see :ref:`caching-basics`). The information in the cache is not stored in the memory of individual web servers, and the cached data is available to all of the app's servers. This provides several advantages:

分布式缓存可被多个应用服务器共享（见:ref:`caching-basics`）。缓存中的信息并非保存在每个Web服务器的内存中，而缓存的数据对所有的应用服务器都是可用的。这带来多项优势：

1. Cached data is coherent on all web servers. Users don't see different results depending on which web server handles their request

1. 缓存数据对所有服务器都是贯通的。哪一个Web服务器处理请求并返回的结果对用户并看不出不同之处。

2. Cached data survives web server restarts and deployments. Individual web servers can be removed or added without impacting the cache.

2. 缓存数据可以在Web服务器重启或部署时幸免于难。

3. The source data store has fewer requests made to it (than with multiple in-memory caches or no cache at all).

3. 可以降低对资源数据库请求（与复合内存缓存或甚至无缓存情况相比）。

.. note:: If using a SQL Server Distributed Cache, some of these advantages are only true if a separate database instance is used for the cache than for the app's source data.

.. 说明:: 如果使用SQL Server分布式缓存，与应用的资源缓存数据相比以上优势仅部分有效，如果使用分离的数据库实例。

Like any cache, a distributed cache can dramatically improve an app's responsiveness, since typically data can be retrieved from the cache much faster than from a relational database (or web service).

如同一般缓存，分布式缓存可以显著地提升应用响应，因为通用数据从缓存获取比从关系数据库（或web服务）更快。

Cache configuration is implementation specific. This article describes how to configure both Redis and SQL Server distributed caches. Regardless of which implementation is selected, the app interacts with the cache using a common IDistributedCache_ interface.

缓存配置是特殊的实现。这篇文章介绍如何配置Redis与SQL Server分布式缓存。不管采用何种实现，应用与缓存的交互都是使用通用的IDistributedCache_接口。

The IDistributedCache Interface
-------------------------------

IDistributedCache接口
-------------------------

The IDistributedCache_ interface includes synchronous and asynchronous methods. The interface allows items to be added, retrieved, and removed from the distributed cache implementation. The IDistributedCache_ interface includes the following methods:

IDistributedCache_接口包括异步和同步方法。接口允许从分布式缓存实现中添加、获取、删除条目。IDistributedCache_接口包括以下方法：

Get, GetAsync

Get, GetAsync

  Takes a string key and retrieves a cached item as a ``byte[]`` if found in the cache.

  以字符串作为键，检索缓存项，如果缓存中存在返回 ``byte[]`` 。
  
Set, SetAsync

Set, SetAsync

  Adds an item (as ``byte[]``) to the cache using a string key.

  以字符串键添加一项缓存项（ ``byte[]``类型）。
  
Refresh, RefreshAsync

Refresh, RefreshAsync

  Refreshes an item in the cache based on its key, resetting its sliding expiration timeout (if any).

  基于键刷新缓存项，重置其滑动过期超时（如果存在）。
  
Remove, RemoveAsync

Remove, RemoveAsync

  Removes a cache entry based on its key.
  
  基于键删除缓存项。  

To use the IDistributedCache_ interface:

使用IDistributedCache_接口：

  1. Specify the dependencies needed in *project.json*.
  
  1. 在*project.json*中指定所需要的依赖项。
  
  2. Configure the specific implementation of IDistributedCache_ in your ``Startup`` class's ``ConfigureServices`` method, and add it to the container there.
  
  2. 在``Startup`` 类的 ``ConfigureServices`` 方法配置的IDistributedCache_接口的具体实现，并将其添加到容器（container）。
  
  3. From the app's :doc:`/fundamentals/middleware` or MVC controller classes, request an instance of IDistributedCache_ from the constructor. The instance will be provided by :doc:`/fundamentals/dependency-injection` (DI).
  
  3. 从应用程序或MVC控制器类的构造函数生成IDistributedCache_的实例。该实例是由:doc:`/fundamentals/dependency-injection` 依赖注入(DI)提供的。

.. note:: There is no need to use a Singleton or Scoped lifetime for IDistributedCache_ instances (at least for the built-in implementations). You can also create an instance wherever you might need one (instead of using :doc:`/fundamentals/dependency-injection`), but this can make your code harder to test, and violates the `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle/>`_.

.. 说明:: 对IDistributedCache_实例无需使用单例或生命周期作用域（至少对内置实现而言）。然而在需要的时候你也可以创建一个实例（而不是用 :doc:`/fundamentals/dependency-injection`）。

The following example shows how to use an instance of IDistributedCache_ in a simple middleware component:

以下示例展示了如何在一个简单的中间件组件中使用IDistributedCache_：

.. literalinclude:: distributed/sample/src/DistCacheSample/StartTimeHeader.cs
  :language: c#
  :linenos:
  :emphasize-lines: 15,18,21,27-31

In the code above, the cached value is read, but never written. In this sample, the value is only set when a server starts up, and doesn't change. In a multi-server scenario, the most recent server to start will overwrite any previous values that were set by other servers. The ``Get`` and ``Set`` methods use the ``byte[]`` type. Therefore, the string value must be converted using ``Encoding.UTF8.GetString`` (for ``Get``) and ``Encoding.UTF8.GetBytes`` (for ``Set``).

以上代码中，缓存值被读取，但未被写入。这个例子中，值只在服务启动时设置，没有修改。在多服务器方案中，最后启动的服务将重写之前启动的其他服务设置的值。``Get`` 和 ``Set``方法使用``byte[]``类型。因此，字符串必须要使用``Encoding.UTF8.GetString`` ( 对``Get``) 和 ``Encoding.UTF8.GetBytes`` (对 ``Set``) 作以转换。

The following code from *Startup.cs* shows the value being set:

以下代码展示了在 *Startup.cs* 中设置值：

.. literalinclude:: distributed/sample/src/DistCacheSample/Startup.cs
  :language: c#
  :linenos:
  :lines: 58-66
  :emphasize-lines: 2,4-6

.. note:: Since IDistributedCache_ is configured in the ``ConfigureServices`` method, it is available to the ``Configure`` method as a parameter. Adding it as a parameter will allow the configured instance to be provided through DI.

.. note:: 只要在``ConfigureServices`` 方法中配置了IDistributedCache_方法，则其便可以当作参数用在 ``Configure`` 方法中。将其以参数的形式添加，即使得该配置实例可以通过DI提供。

Using a Redis Distributed Cache
-------------------------------

使用Redis分布式缓存
----------------------

`Redis <http://redis.io>`_ is an open source in-memory data store, which is often used as a distributed cache. You can use it locally, and you can configure an `Azure Redis Cache <https://azure.microsoft.com/en-us/services/cache/>`_ for your Azure-hosted ASP.NET Core apps. Your ASP.NET Core app configures the cache implementation using a ``RedisDistributedCache`` instance.

`Redis <http://redis.io>`_ 是一个开源的内存数据库，通常被用作分布式缓存。你可以在本机使用它，也可以为你在Azure部署的ASP.NET Core应用配置一个`Azure Redis 缓存 <https://azure.microsoft.com/en-us/services/cache/>`_ 。在你的ASP.NET Core应用使用 ``RedisDistributedCache``实例配置缓存实现。

You configure the Redis implementation in ``ConfigureServices`` and access it in your app code by requesting an instance of IDistributedCache_ (see the code above).

你要使用 ``ConfigureServices``配置Redis缓存实现，并通过在你的应用代码中生成一个IDistributedCache_实例来访问它。

In the sample code, a ``RedisCache`` implementation is used when the server is configured for a ``Staging`` environment. Thus the ``ConfigureStagingServices`` method configures the ``RedisCache``:

在示例代码中，``RedisCache`` 实现被用在配置为 ``Staging``环境的服务。然后由 ``ConfigureStagingServices`` 方法配置 ``RedisCache``：

.. literalinclude:: distributed/sample/src/DistCacheSample/Startup.cs
  :language: c#
  :linenos:
  :lines: 27-40
  :emphasize-lines: 8-13

.. note:: To install Redis on your local machine, install the chocolatey package http://chocolatey.org/packages/redis-64/ and run ``redis-server`` from a command prompt.

.. 注意:: 要在你本地计算机上安装Redis，安装chocolatey包 http://chocolatey.org/packages/redis-64/ 然后通过命令行方式运行 ``redis-server`` 。

Using a SQL Server Distributed Cache
------------------------------------

使用SQL Server分布式缓存
------------------------------------

The SqlServerCache implementation allows the distributed cache to use a SQL Server database as its backing store. To create SQL Server table you can use sql-cache tool, the tool creates a table with the name and schema you specify. 

To use sql-cache tool add SqlConfig.Tools to the tools section of the project.json file and run dotnet restore.

.. literalinclude:: distributed/sample/src/DistCacheSample/project.json
  :language: c#
  :linenos:
  :lines: 14-20
  :emphasize-lines: 6

Test SqlConfig.Tools by running the following command

.. code-block:: none

   C:\DistCacheSample\src\DistCacheSample>dotnet sql-cache create --help

sql-cache tool  will display usage, options and command help, now you can create tables into sql server, running "sql-cache create" command :

.. code-block:: none

  C:\DistCacheSample\src\DistCacheSample>dotnet sql-cache create "Data Source=(localdb)\v11.0;Initial Catalog=DistCache;Integrated Security=True;" dbo TestCache
  info: Microsoft.Extensions.Caching.SqlConfig.Tools.Program[0]
      Table and index were created successfully.

The created table have the following schema:

.. image:: distributed/_static/SqlServerCacheTable.png

Like all cache implementations, your app should get and set cache values using an instance of IDistributedCache_, not a ``SqlServerCache``. The sample implements ``SqlServerCache`` in the ``Production`` environment (so it is configured in ``ConfigureProductionServices``).

正如所有的缓存实现，你的应用需要使用IDistributedCache_实例读取和设置值，而非``SqlServerCache``。这个例子在``Production``环境实现``SqlServerCache``（因此是在 ``ConfigureProductionServices``实现）。

.. literalinclude:: distributed/sample/src/DistCacheSample/Startup.cs
  :language: c#
  :linenos:
  :lines: 42-56
  :emphasize-lines: 7-12

.. note:: The ``ConnectionString`` (and optionally, ``SchemaName`` and ``TableName``) should typically be stored outside of source control (such as UserSecrets), as they may contain credentials.

.. note:: ``ConnectionString`` (及非必要的 ``SchemaName`` 与 ``TableName``)通常应该不受源代码托管（就如用户隐私），因为它们可能包含凭证信息。

Recommendations
---------------

建议
------------

When deciding which implementation of IDistributedCache_ is right for your app, choose between Redis and SQL Server based on your existing infrastructure and environment, your performance requirements, and your team's experience. If your team is more comfortable working with Redis, it's an excellent choice. If your team prefers SQL Server, you can be confident in that implementation as well. Note that A traditional caching solution stores data in-memory which allows for fast retrieval of data. You should store commonly used data in a cache and store the entire data in a backend persistent store such as SQL Server or Azure Storage.

当你要决定何种IDistributedCache_实现对你的应用更合适，在Redis与SQL Server之间选择时，取决于你的现存架构和环境，你的性能需求，和你的团队成员经验。如果你的团队更适合用Redis，那既是不错的选择。如果你的团队倾向于 SQL Server，你也可以确定使用这一实现。注意：习惯的缓存解决方案在内存存放数据，以实现数据快速检索。你应该在缓存中存放常用数据，而在后台用持久化存储数据库存储完整数据，比如SQL Server和Azure Storage。

Redis Cache is a caching solution which gives you high throughput and low latency as compared to SQL Cache. Also, you should avoid using the in-memory implementation (``MemoryCache``) in multi-server environments.

Redis缓存是一种为你提供高通吐量、低延迟的缓存解决方案，与SQL缓存相比。因此，你应该避免在多服务器环境下使用内存缓存实现（``MemoryCache``）。

Azure Resources:

Azure资源：

- `Redis Cache on Azure <https://azure.microsoft.com/en-us/documentation/services/redis-cache/>`_

- `Azure平台的Redis缓存 <https://azure.microsoft.com/en-us/documentation/services/redis-cache/>`_

- `SQL Database on Azure <https://azure.microsoft.com/en-us/documentation/services/sql-database/>`_

- `Azure平台SQL数据库 <https://azure.microsoft.com/en-us/documentation/services/sql-database/>`_

.. tip:: The in-memory implementation of IDistributedCache_ should only be used for testing purposes or for applications that are hosted on just one server instance.

.. tip:: IDistributedCache_的内存方式实现只应该用在以测试目的部署在单个服务器实例的应用。

.. _IDistributedCache: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Distributed/IDistributedCache/index.html

.. _IDistributedCache: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Distributed/IDistributedCache/index.html

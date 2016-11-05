:version: 1.0.0-rc2

In Memory Caching
=================

内存缓存（Memory Caching）
===============================

By `Steve Smith`_

作者：`Steve Smith`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

Caching involves keeping a copy of data in a location that can be accessed more quickly than the source
data. ASP.NET Core has rich support for caching in a variety of ways, including keeping data in memory on the
local server, which is referred to as *in memory caching*.

缓存用于保存一份数据备份在某个位置，与从数据源相比，从这个位置可以更快的访问到数据。 ASP.NET Core 用多种方式提供了对缓存的全面支持，包括缓存数据在本地服务器，这也被称为 *内存缓存* 。

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/memory/sample>`_

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/memory/sample>`_

.. _caching-basics:

Caching Basics
--------------

缓存基础
----------

Caching can dramatically improve the performance and scalability of ASP.NET applications, by eliminating unnecessary requests to external data sources for data that changes infrequently.

缓存可以显著地提高 ASP.NET 应用的性能与可伸缩性，降低甚至消除对非频繁更新的数据的外部数据源的非必要请求。

.. note:: Caching in all forms (in-memory or distributed, including session state) involves making a copy of data in order to optimize performance. The copied data should be considered ephemeral - it could disappear at any time. Apps should be written to not depend on cached data, but use it when available.

.. note:: 所有形式的缓存（在内存或分布式的，包括Session状态）解决方式都是利用数据副本来优化性能。复制的数据被认为是转瞬即逝的————它可以在任意时刻销毁。应用开发不应该依赖与缓存数据，而只在可用的时候使用它。

ASP.NET supports several different kinds of caches, the simplest of which is represented by the `IMemoryCache <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/IMemoryCache/index.html>`_ interface, which represents a cache stored in the memory of the local web server.

ASP.NET 支持多种不同的缓存方式， `IMemoryCache <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/IMemoryCache/index.html>`_ 接口代表了一种最简单的方式，即表示缓存数据在本地服务器内存中。

You should always write (and test!) your application such that it can use cached data if it's available, but otherwise will work correctly using the underlying data source.

你应该始终这样开发你的应用，它在缓存数据有效的时候使用缓存，但在缓存无效的情况下使用基本数据源也能正常运行。

An in-memory cache is stored in the memory of a single server hosting an ASP.NET app. If an app is hosted by multiple servers in a web farm or cloud hosting environment, the servers may have different values in their local in-memory caches. Apps that will be hosted in server farms or on cloud hosting should use a :doc:`distributed cache <distributed>` to avoid cache consistency problems.

内存缓存是存储于部署 ASP.NET 应用的单个服务器的内存中。如果应用是部署在多服务器集群或云主机环境，服务器可能在各自的本地内存缓存中会有不同的值。将要部署在服务器集群或云主机的应用应该使用 :doc:`distributed cache <distributed>` 来避免一致性问题。

.. tip:: A common use case for caching is data-driven navigation menus, which rarely change but are frequently read for display within an application. Caching results that do not vary often but which are requested frequently can greatly improve performance by reducing round trips to out of process data stores and unnecessary computation.

.. tip:: 缓存通常的使用场景是数据驱动的导航菜单，它们很少改变却要频繁的读取来在应用中显示。缓存那些频繁被请求却很少改变的结果可以大幅提高性能，通过减少往返过程的数据存储和不必要的计算。

Configuring In Memory Caching
-----------------------------

配置内存缓存
---------------

To use an in memory cache in your ASP.NET application, add the following dependencies to your *project.json* file:

在 ASP.NET 中使用内存缓存，先在你的 *project.json* 文件添加下面的依赖：

.. literalinclude:: memory/sample/src/CachingSample/project.json
  :linenos:
  :lines: 7-13
  :emphasize-lines: 4

Caching in ASP.NET Core is a *service* that should be referenced from your application by :doc:`/fundamentals/dependency-injection`. To register the caching service and make it available within your app, add the following line to your ``ConfigureServices`` method in ``Startup``:

缓存在 ASP.NET Core 是一项 *service* ，它应该通过 :doc:`/fundamentals/dependency-injection` 引用到你的应用中。在你的应用中注册缓存服务使其生效，要在你的 ``Startup`` 的 ``ConfigureServices`` 方法里添加下面的行：

.. literalinclude:: memory/sample/src/CachingSample/Startup.cs
  :linenos:
  :lines: 12-15
  :dedent: 8
  :emphasize-lines: 3

You utilize caching in your app by requesting an instance of ``IMemoryCache`` in your controller or middleware constructor. In the sample for this article, we are using a simple middleware component to handle requests by returning customized greeting. The constructor is shown here:

你在应用中使用缓存，是通过请求你的控制器或中间件构造函数中的 ``IMemoryCache`` 实例。在这篇文章的示例中，我们使用一个简单的中间件组件来处理请求，返回自定义问候语。构造器如下：

.. literalinclude:: memory/sample/src/CachingSample/Middleware/GreetingMiddleware.cs
  :linenos:
  :lines: 19-28
  :dedent: 8
  :emphasize-lines: 2,7

Reading and Writing to a Memory Cache
-------------------------------------

内存缓存读写
---------------

The middleware's ``Invoke`` method returns the cached data when it's available.

中间件的 ``Invoke`` 方法返回缓存的数据当它可用时。

There are two methods for accessing cache entries:

有两种方法访问缓存对象：

``Get``
  ``Get`` will return the value if it exists, but otherwise returns ``null``.

``Get``
  ``Get`` 将在值存在时返回值，否在返回 ``null``.

``TryGet``
  ``TryGet`` will assign the cached value to an ``out`` parameter and return true if the entry exists. Otherwise it returns false.

``TryGet``
    ``TryGet`` 将缓存的数据分配到 ``out`` 参数并返回 true 。否则返回 false。

Use the ``Set`` method to write to the cache. ``Set`` accepts the key to use to look up the value, the value to be cached, and a set of ``MemoryCacheEntryOptions``. The ``MemoryCacheEntryOptions`` allow you to specify absolute or sliding time-based cache expiration, caching priority, callbacks, and dependencies. These options are detailed below.

使用 ``Set`` 方法写入缓存。 ``Set`` 支持使用键来查询值、要缓存的值、 ``MemoryCacheEntryOptions`` 集合。 ``MemoryCacheEntryOptions`` 允许你指定基于绝对或滑动的过期时间，缓存优先级，回调，和依赖。具体选项如下。

The sample code (shown below) uses the ``SetAbsoluteExpiration`` method on ``MemoryCacheEntryOptions`` to cache greetings for one minute.

示例代码（下面所示）在 ``MemoryCacheEntryOptions`` 上使用 ``SetAbsoluteExpiration`` 方法来缓存问候语一分钟。

.. literalinclude:: memory/sample/src/CachingSample/Middleware/GreetingMiddleware.cs
  :linenos:
  :lines: 30-58
  :dedent: 8
  :emphasize-lines: 7,10,16-18

In addition to setting an absolute expiration, a sliding expiration can be used to keep frequently requested items in the cache:

除设置绝对过期时间之外，使用滑动过期时间可以在缓存中持续保留被频繁请求的项：

.. code-block:: c#

  // keep item in cache as long as it is requested at least
  // once every 5 minutes

  // 只要项目被请求，就在缓存中保留项目
  // 每 5 分钟一次...

  new MemoryCacheEntryOptions()
    .SetSlidingExpiration(TimeSpan.FromMinutes(5))

To avoid having frequently-accessed cache entries growing too stale (because their sliding expiration is constantly reset), you can combine absolute and sliding expirations:

为避免不断访问地缓存变得过于陈旧（由于其滑动过期不断重置），你可以合并使用绝对和滑动过期时间：

.. code-block:: c#

  // keep item in cache as long as it is requested at least
  // once every 5 minutes...
  // but in any case make sure to refresh it every hour

  // 只要项目被请求，就在缓存中保留项目
  // 每 5 分钟一次...
  // 但在任何情况下，一定要每小时刷新它

  new MemoryCacheEntryOptions()
    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
    .SetAbsoluteExpiration(TimeSpan.FromHours(1))

By default, an instance of ``MemoryCache`` will automatically manage the items stored, removing entries when necessary in response to memory pressure in the app. You can influence the way cache entries are managed by setting their `CacheItemPriority <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/CacheItemPriority/index.html>`_ when adding the item to the cache. For instance, if you have an item you want to keep in the cache unless you explicitly remove it, you would use the ``NeverRemove`` priority option:

默认情况下， ``MemoryCache`` 实例将自动管理存储的对象，必要时，在应用程序出现内存压力时删除对象。你可以控制管理的缓存对象的方式，通过在将它们添加到内存的时候设置它们的 `CacheItemPriority <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/CacheItemPriority/index.html>`_ 。例如，如果你有一项内容想保留在缓存中，除非显式删除它，你可以使用 ``NeverRemove`` 优先级选项：

.. code-block:: c#

  // keep item in cache indefinitely unless explicitly removed

  // 无限期地保留在缓存中，除非显式删除

  new MemoryCacheEntryOptions()
    .SetPriority(CacheItemPriority.NeverRemove))

When you do want to explicitly remove an item from the cache, you can do so easily using the ``Remove`` method:

当你想要显式地删除一项内容，你可以使用 ``Remove`` 方法很方便地实现：

.. code-block:: c#

  cache.Remove(cacheKey);

Cache Dependencies and Callbacks
--------------------------------

缓存依赖与回调
--------------

You can configure cache entries to depend on other cache entries, the file system, or programmatic tokens, evicting the entry in response to changes. You can register a callback, which will run when a cache item is evicted.

你可以配置缓存对象依赖于其它缓存项、文件系统或编程的指令，响应对缓存项的更改回收。你可以注册一个回调，当一个缓存项被回收时执行。

.. literalinclude:: memory/sample/test/CachingSample.Tests/MemoryCacheTests.cs
  :linenos:
  :lines: 22-41
  :dedent: 8
  :emphasize-lines: 6-11,18

The callback is run on a different thread from the code that removes the item from the cache.

回调是运行在与删除缓存对象的代码不同地线程上。

.. warning:: If the callback is used to repopulate the cache it is possible other requests for the cache will take place (and find it empty) before the callback completes, possibly resulting in several threads repopulating the cached value.

.. warning:: 如果回调被用来刷新缓存，可能在回调执行完之前会发生其它对缓存的请求，将会导致几个线程重新填充缓存的值。

Possible `eviction reasons <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/EvictionReason/index.html>`_ are:

可能的 `回收原因 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Caching/Memory/EvictionReason/index.html>`_ 有:

None
  No reason known.

None
  未知原因。

Removed
  缓存项被手动调用 ``Remove()`` 方法而删除。

Replaced
  The item was overwritten.

Replaced
  缓存项被重写。

Expired
  The item timed out.

Expired
  缓存项已过期。

TokenExpired
  The token the item depended upon fired an event.

TokenExpired
  缓存项的依赖触发了一个事件。

Capacity
  The item was removed as part of the cache's memory management process.

Capacity
  缓存项因缓存管理进程而被清除。

You can specify that one or more cache entries depend on a ``CancellationTokenSource`` by adding the expiration token to the ``MemoryCacheEntryOptions`` object. When a cached item is invalidated, call ``Cancel`` on the token, which will expire all of the associated cache entries (with a reason of ``TokenExpired``). The following unit test demonstrates this:

你可以指定一个或多个缓存对象依赖与一个在 ``CancellationTokenSource`` ，通过向 ``MemoryCacheEntryOptions`` 对象添加过期指令。当一个缓存项无效时，调用 ``Cancel`` 指令，这将使所有关联的缓存项过期（过期原因为 ``TokenExpired``）。以下的单元测试对此做了演示：

.. literalinclude:: memory/sample/test/CachingSample.Tests/MemoryCacheTests.cs
  :linenos:
  :lines: 43-64
  :dedent: 8
  :emphasize-lines: 7,16,21

Using a ``CancellationTokenSource`` allows multiple cache entries to all be expired without the need to create a dependency between cache entries themselves (in which case, you must ensure that the source cache entry exists before it is used as a dependency for other entries).

使用 ``CancellationTokenSource`` 允许多缓存项同时过期而不需要在各缓存项之间创建依赖（在这种情况下，必需由你确保缓存项在作为其它项的依赖时是存在的）。

Cache entries will inherit triggers and timeouts from other entries accessed while creating the new entry. This approach ensures that subordinate cache entries expire at the same time as related entries.

缓存项将从创建新项时访问的其它项继承触发器与超时。这个方法确保下属项目作为相关项而同时过期。

.. literalinclude:: memory/sample/test/CachingSample.Tests/MemoryCacheTests.cs
  :linenos:
  :lines: 66-94
  :dedent: 8
  :emphasize-lines: 7,11,13,23-24

.. note:: When one cache entry is used to create another, the new one copies the existing entry's expiration tokens and time-based expiration settings, if any. It is not expired in response to manual removal or updating of the existing entry.

.. note:: 当一个缓存项被用来创建另一个，新项复制了已存在项的过期时间指令和基于时间的过期设置，甚至其它的。缓存项在响应手动删除或更新现有项之前是不会过期的。

Other Resources
---------------

其它资料
----------

- :doc:`distributed`

- :doc:`distributed`

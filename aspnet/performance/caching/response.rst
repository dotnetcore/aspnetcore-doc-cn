:version: 1.0.0-rc1

Response Caching
================

响应缓存
================

`Steve Smith`_

作者：`Steve Smith`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/response/sample>`_

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/performance/caching/response/sample>`_

What is Response Caching
------------------------

什么是响应缓存
------------------------

*Response caching* refers to specifying cache-related headers on HTTP responses made by ASP.NET Core MVC actions. These headers specify how you want client and intermediate (proxy) machines to cache responses to certain requests (if at all). This can reduce the number of requests a client or proxy makes to the web server, since future requests for the same action may be served from the client or proxy's cache. In this case, the request is never made to the web server.

*Response caching* 与 ASP.NET Core MVC 输出的与缓存相关的特定 HTTP 头有关。这些 HTTP 头指示针对特定请求（也许是所有）你想让客户端和中间体（代理）机器间如何缓存响应。这可以减少客户端或代理对服务端的请求次数，因为再次对同一操作的请求将由客户端或代理的缓存响应。这种情况下，实际并未请求到服务端。

.. image:: response/_static/proxy-and-cache.png

The primary HTTP header used for caching is ``Cache-Control``. The `HTTP 1.1 specification <https://tools.ietf.org/html/rfc7234#section-5.2>`_ details many options for this directive. Three common directives are:

主要用于缓存的 HTTP 头是 ``Cache-Control``。  `HTTP 1.1 规范 <https://tools.ietf.org/html/rfc7234#section-5.2>`_ 详细描述了关于此指令的许多选项。三个通常的指令是：

public
  Indicates that the response may be cached.

public
  指示响应可被缓存。

private
  Indicates the response is intended for a single user and **must not** be cached by a shared cache. The response could still be cached in a private cache (for instance, by the user's browser).

private
  指示响应是准备用于单一用户，且 **绝不能** 被公开的缓存。 响应也可以缓存为私有缓存（针对实例，被用户浏览器）。

no-cache
  Indicates the response **must not** be used by a cache to satisfy any subsequent request (without successful revalidation with the origin server).

no-cache
  指示响应 **绝不能** 被缓存以满足后续请求（与源服务器没有成功的验证）。

.. note:: **Response caching does not cache responses on the web server**. It differs from `output caching <http://www.asp.net/mvc/overview/older-versions-1/controllers-and-routing/improving-performance-with-output-caching-cs>`_, which would cache responses in memory on the server in earlier versions of ASP.NET and ASP.NET MVC. Output caching middleware is planned to be added to ASP.NET Core in a future release.

.. note:: **响应缓存不会在服务器缓存响应**。它区别于 `output caching <http://www.asp.net/mvc/overview/older-versions-1/controllers-and-routing/improving-performance-with-output-caching-cs>`_, 即缓存响应到服务端内存，在早期 ASP.NET MVC。输出缓存中间件计划在 ASP.NET Core 未来版本中加入。

Additional HTTP headers used for caching include ``Pragma`` and ``Vary``, which are described below. Learn more about `Caching in HTTP from the specification <https://tools.ietf.org/html/rfc7234#section-3>`_.

此外，HTTP 头用来缓存包括 ``Pragma`` 和 ``Vary`` ，它们将在下文介绍。了解更多关于 `HTTP中的缓存从规范 <https://tools.ietf.org/html/rfc7234#section-3>`_。

ResponseCache Attribute
-----------------------

响应缓存特性
-------------

The ResponseCacheAttribute_ is used to specify how a controller action's headers should be set to control its cache behavior. The attribute has the following properties, all of which are optional unless otherwise noted.

响应缓存特性被用来指定控制器方法头被如何设置以控制它的缓存行为。该特性具有以下属性，全都是可选的，除非特别说明。

Duration ``int``
  The maximum duration (in seconds) the response should be cached. **Required** unless ``NoStore`` is ``true``.

Duration ``int``
  响应被缓存的最长持续时间（秒）。 **必需** 除非 ``NoStore`` 设置为 ``true``。

Location ``ResponseCacheLocation``
  The location where the response may be cached. May be ``Any``, ``None``, or ``Client``. Default is ``Any``.

Location  ``ResponseCacheLocation``
  响应缓存的位置。可以是 ``Any``, ``None``, 或 ``Client``. 默认为 ``Any``.

NoStore ``bool``
  Determines whether the value should be stored or not, and overrides other property values. When ``true``, ``Duration`` is ignored and ``Location`` is ignored for values other than ``None``.

NoStore ``bool``
  决定值是否被存储，且重写其它属性。当为 ``true``, ``Duration`` 将被忽略且 ``Location`` 被忽略除了 ``None``.

VaryByHeader ``string``
  When set, a ``vary`` response header will be written with the response.

VaryByHeader ``string``
  一旦设置, ``vary`` 响应头将在响应中被写.

CacheProfileName ``string``
  When set, determines the name of the cache profile to use.

CacheProfileName ``string``
  一旦设置，决定缓存配置使用的名称。

Order ``int``
  The order of the filter (from IOrderedFilter_).

Order ``int``
  过滤器的顺序（来自 IOrderedFilter_）。

The ResponseCacheAttribute_ is used to configure and create (via IFilterFactory_) a ResponseCacheFilter_, which performs the work of writing the appropriate HTTP headers to the response. The filter will first remove any existing headers for ``Vary``, ``Cache-Control``, and ``Pragma``, and then will write out the appropriate headers based on the properties set in the ResponseCacheAttribute_.

ResponseCacheAttribute_ 用来配置和创建 (通过 IFilterFactory_) ResponseCacheFilter_, 它承载了在响应中写入适当的 HTTP 头的工作。过滤器将针对 ``Vary``, ``Cache-Control``, 和 ``Pragma`` 首先删除所有存在的头然后基于响应缓存特性的属性的设置输出适当的头。

The ``Vary`` Header
^^^^^^^^^^^^^^^^^^^

 ``Vary`` 头
^^^^^^^^^^^^^^^^^^^

This header is only written when the ``VaryByHeader`` property is set, in which case it is set to that property's value.

这个头只有在 ``VaryByHeader`` 属性被设置时输出，在这种情况下，设置该属性的值。

``NoStore`` and ``Location.None``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

``NoStore`` 与 ``Location.None``
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

``NoStore`` is a special property that overrides most of the other properties. When this property is set to ``true``, the ``Cache-Control`` header will be set to "no-store". Additionally, if ``Location`` is set to ``None``, then ``Cache-Control`` will be set to "no-store, no-cache" and ``Pragma`` is likewise set to ``no-cache``. (If ``NoStore`` is ``false`` and ``Location`` is ``None``, then both ``Cache-Control`` and ``Pragma`` will be set to ``no-cache``).

``NoStore`` 是会重写其它大多数属性的特殊属性。当这个属性被设置为 ``true`` ， ``Cache-Control`` 头会被设置为 "no-store" 。另外， ``Location`` 如果设置为 ``None`` ， ``Cache-Control`` 将被设置为 "no-store, no-cache" ，且同样地 ``Pragma`` 也被设置为 ``no-cache`` 。（如果  ``NoStore`` 是  ``false`` 且 ``Location`` 是 ``None`` ，则 ``Cache-Control`` 和 ``Pragma`` 将被设置为 ``no-cache``）。

A good scenario in which to set ``NoStore`` to ``true`` is error pages. It's unlikely you would want to respond to a user's request with the error response a different user previously generated, and such responses may include stack traces and other sensitive information that shouldn't be stored on intermediate servers. For example:

何时设置 ``NoStore`` 为 ``true`` 最好的场景是错误页面。没有可能你响应给用户的一个请求是错误页面，却是前一个用户产生的，这类响应或许包含堆栈跟踪等其它敏感信息而不能存储在中间服务器上。

.. literalinclude:: response/sample/src/ResponseCacheSample/Controllers/HomeController.cs
  :lines: 30-34
  :emphasize-lines: 1
  :dedent: 8
  :language: c#

This will result in the following headers:

这将产生如下头信息的结果：

.. code-block:: javascript

  Cache-Control: no-store,no-cache
  Pragma: no-cache

Location and Duration
^^^^^^^^^^^^^^^^^^^^^

位置和延续时间
^^^^^^^^^^^^^^^^^^^^^

To enable caching, ``Duration`` must be set to a positive value and ``Location`` must be either ``Any`` (the default) or ``Client``. In this case, the ``Cache-Control`` header will be set to the location value followed by the "max-age" of the response.

开启缓存， ``Duration`` 必需设置为一个有效值且 ``Location`` 必需为 ``Any`` 或 ``Client``。这种情况下， ``Cache-Control`` 头将按照响应的 "max-age" 被存储在 “Location” 指定的位置。

.. note:: ``Location``'s options of ``Any`` and ``Client`` translate into ``Cache-Control`` header values of ``public`` and ``private``, respectively. As noted previously, setting ``Location`` to ``None`` will set both ``Cache-Control`` and ``Pragma`` headers to ``no-cache``.

.. note:: ``Location``的选项 ``Any`` 和 ``Client`` 翻译到 ``Cache-Control`` 头的值分别为 ``public`` 和 ``private``。如前面交待的，设置 ``Location`` 为 ``None`` 将同时设置 ``Cache-Control`` 和 ``Pragma`` 头为 ``no-cache``。

Below is an example showing the headers produced by setting ``Duration`` and leaving the default ``Location`` value.

下面是一个例子展示了设置 ``Duration`` 并使用 ``Location`` 默认值所产生的头。

.. literalinclude:: response/sample/src/ResponseCacheSample/Controllers/HomeController.cs
  :lines: 22-28
  :emphasize-lines: 1
  :dedent: 8
  :language: c#

Produces the following headers:

产生了下面的头：

.. code-block:: javascript

  Cache-Control: public,max-age=60

Cache Profiles
^^^^^^^^^^^^^^

缓存配置
^^^^^^^^^^^^^^

Instead of duplicating ``ResponseCache`` settings on many controller action attributes, cache profiles can be configured as options when setting up MVC in the ``ConfigureServices`` method in ``Startup``. Values found in a referenced cache profile will be used as the defaults by the ``ResponseCache`` attribute, and will be overridden by any properties specified on the attribute.

为了在多数控制器方法复制 ``ResponseCache`` 特性设置，在 MVC ``Startup`` 中的 ``ConfigureServices`` 方法里，缓存配置可以配置为选项。引用的缓存配置中发现的值将被 ``ResponseCache`` 特性用作默认值，并且可以被任意特性中定义的属性所重写。

Setting up a cache profile:

建立缓存配置：

.. literalinclude:: response/sample/src/ResponseCacheSample/Startup.cs
  :start-after: Use this method to add services to the container.
  :end-before: // This
  :emphasize-lines: 5-15
  :dedent: 8
  :language: c#

Referencing a cache profile:

引用缓存配置：

.. literalinclude:: response/sample/src/ResponseCacheSample/Controllers/HomeController.cs
  :lines: 5-12,35
  :emphasize-lines: 1,4
  :dedent: 4
  :language: c#

.. tip:: The ``ResponseCache`` attribute can be applied both to actions (methods) as well as controllers (classes). Method-level attributes will override the settings specified in class-level attributes.

.. tip::  ``ResponseCache`` 特性可以被用在操作（方法）上，也可以用在控制器（类）上。方法级别的特性将重写类级别特性的设置。

In the above example, a class-level attribute specifies a duration of 30 seconds while a method-level attributes references a cache profile with a duration set to 60 seconds.

在以上的例子中，类级别特性指明了存续时间为30秒，但方法级别的特性引用了设置存续时间为60秒的缓存配置。

The resulting header:

结果头信息：

.. code-block:: javascript

  Cache-Control: public,max-age=60

.. _ResponseCacheAttribute: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ResponseCacheAttribute/index.html
.. _IOrderedFilter: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Filters/IOrderedFilter/index.html
.. _IFilterFactory: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Filters/IFilterFactory/index.html
.. _ResponseCacheFilter: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Filters/ResponseCacheFilter/index.html

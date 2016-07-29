Routing
=======
路由
=======

作者 `Ryan Nowak <https://github.com/rynowak>`__, `Steve Smith`_, 和 `Rick Anderson`_

翻译 `stoneniqiu <http://www.cnblogs.com/stoneniqiu/>`

Routing is used to map requests to route handlers. Routes are configured when the application starts up, and can extract values from the URL that will be used for request processing. Routing functionality is also responsible for generating links using the defined routes in ASP.NET apps.

路由是用来把请求映射到路由处理程序。应用程序一启动就配置了路由，并且可以从URL中提取值用于处理请求。它还负责使用ASP.NET应用程序中定义的路由来生成链接。


This document covers the low level ASP.NET Core routing. For ASP.NET Core MVC routing, see :doc:`/mvc/controllers/routing`

这份文档涵盖了初级的ASP.NET核心路由. 对于 ASP.NET 核心 MVC 路由, 请查看 :doc:`/mvc/controllers/routing`

.. contents:: Sections
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/routing/sample>`__

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/routing/sample>`__

Routing basics
----------------
路由基础
----------------

Routing uses *routes* (implementations of :dn:iface:`~Microsoft.AspNetCore.Routing.IRouter`) to:

路由使用 *routes*类 (:dn:iface:`~Microsoft.AspNetCore.Routing.IRouter`的实现) 做到:

- map incoming requests to *route handlers*
- 映射传入的请求到*路由处理程序*
- generate URLs used in responses
- 生成响应中使用的URLs

Generally an app has a single collection of routes. The route collection is processed in order. Requests look for a match in the route collection by :ref:`URL-Matching-ref`. Responses use routing to genenerate URLs.

一般来说，一个应用会有单个路由集合. 这个集合会按顺序处理. 请求会在这个路由集合里按照:ref:`URL-Matching-ref`来查找匹配. 响应使用路由生成URLs.


Routing is connected to the :doc:`middleware <middleware>` pipeline by the :dn:class:`~Microsoft.AspNetCore.Builder.RouterMiddleware` class. :doc:`ASP.NET MVC </mvc/overview>` adds routing to the middleware pipeline as part of its configuration. To learn about using routing as a standalone component, see using-routing-middleware_.

 路由通过 :dn:class:`~Microsoft.AspNetCore.Builder.RouterMiddleware` 类连接到 :doc:`middleware <middleware>`管道.  作为配置的一部分:doc:`ASP.NET MVC </mvc/overview>` 可以增加路由到中间件管道. 要了解如何使用路由作为独立组件, 请看 using-routing-middleware_.


.. _URL-Matching-ref:

URL matching
^^^^^^^^^^^^
URL 匹配
^^^^^^^^^^^^

URL matching is the process by which routing dispatches an incoming request to a *handler*. This process is generally based on data in the URL path, but can be extended to consider any data in the request. The ability to dispatch requests to separate handlers is key to scaling the size and complexity of an application.

路由匹配指的是路由调度请求到一个处理程序的过程。这个过程通常是基于URL路径中的数据，但可以扩展到请求中的任何数据.调度请求到不同处理程序的能力是应用调节自身大小和复杂度的关键。

Incoming requests enter the :dn:cls:`~Microsoft.AspNetCore.Builder.RouterMiddleware` which calls the :dn:method:`~Microsoft.AspNetCore.Routing.IRouter.RouteAsync` method on each route in sequence. The :dn:iface:`~Microsoft.AspNetCore.Routing.IRouter` instance chooses whether to *handle* the request by setting the :dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.Handler` to a non-null :dn:delegate:`~Microsoft.AspNetCore.Http.RequestDelegate`. If a handler is set a route, it will be invoked to process the request and no further routes will be processed. If all routes are executed, and no handler is found for a request, the middleware calls *next* and the next middleware in the request pipeline is invoked.

请求调用序列中每个路由的异步方法来进入路由中间件。:dn:iface:`~Microsoft.AspNetCore.Routing.IRouter`实例通过设置:dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.Handler`为一个不为空的:dn:delegate:`~Microsoft.AspNetCore.Http.RequestDelegate`来选择是否处理请求。如果一个处理程序已经设置了路由，那么它就将被调用来处理这个请求，并且不会有其他的路由再去处理。如果所有的路由都执行了，请求还没有找到处理程序，那么中间件就会调用next方法，从而下一个在请求管道中的中间件就被调用了。

The primary input to ``RouteAsync`` is the :dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.HttpContext` associated with the current request. The ``RouteContext.Handler`` and :dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.RouteData` are outputs that will be set after a successful match.

``RouteAsync``的主要输入是和当前请求关联的:dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.HttpContext`。在一个成功匹配之后，``RouteContext.Handler``和:dn:cls:`~Microsoft.AspNetCore.Routing.RouteContext` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteContext.RouteData`会作为输出。

A successful match during ``RouteAsync`` also will set the properties of the ``RouteContext.RouteData`` to appropriate values based on the request processing that was done. The ``RouteContext.RouteData`` contains important state information about the *result* of a route when it successfully matches a request.

在``RouteAsync``执行期间，一个成功匹配会基于已经完成的请求处理设置 ``RouteContext.RouteData``的属性为合适的值。当一个路由成功匹配了一个请求时，``RouteContext.RouteData`包含了重要的关于匹配结果的状态信息

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.Values` is a dictionary of *route values* produced from the route. These values are usually determined by tokenizing the URL, and can be used to accept user input, or to make further dispatching decisions inside the application.

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.Values`是一个从路由产生的*路由值*字典.这些值通常由标记化的URL确定的，可以用来接收用户输入，或者用来在应用内部做更深层的调度决定。

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.DataTokens`  is a property bag of additional data related to the matched route. ``DataTokens`` are provided to support associating state data with each route so the application can make decisions later based on which route matched. These values are developer-defined and do **not** affect the behavior of routing in any way. Additionally, values stashed in data tokens can be of any type, in contrast to route values which must be easily convertable to and from strings.

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.DataTokens`是相关的匹配路由附加数据的属性包。提供``数据令牌``支持与每个路由相关的状态数据，这样应用基于匹配的路由可以迟点做出决定。这些数据是开发人员定义的，不会式影响路由的行为。而且，数据令牌中的值可以是任何类型，对比路由值，它可以很容易的转成字符串。

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.Routers` is a list of the routes that took part in successfully matching the request. Routes can be nested inside one another, and the ``Routers`` property reflects the path through the logical tree of routes that resulted in a match. Generally the first item in ``Routers`` is the route collection, and should be used for URL generation. The last item in ``Routers`` is the route that matched.

:dn:cls:`~Microsoft.AspNetCore.Routing.RouteData` :dn:prop:`~Microsoft.AspNetCore.Routing.RouteData.Routers` 是一个成功匹配请求的路由列表.路由可以彼此嵌套，而且``Routers``属性反映了请求通过路由逻辑树导致匹配的路径。一般来说，``Routers``中的第一项就是一个路由集合,而且应该用来生成URL.``Routers``中的最后一项就是已匹配路由.


URL generation
^^^^^^^^^^^^^^
URL 生成
^^^^^^^^^^^^^^

URL generation is the process by which routing can create a URL path based on a set of route values. This allows for a logical separation between your handlers and the URLs that access them.

URL 生成是指的路由基于一系列的路由值创建一个URL路径的过程. 这允许你的处理程序和能访问它们的URL直接有一个逻辑分离。


URL generation follows a similar iterative process, but starts with user or framework code calling into the :dn:method:`~Microsoft.AspNetCore.Routing.IRouter.GetVirtualPath` method of the route collection. Each *route* will then have its ``GetVirtualPath`` method called in sequence until until a non-null :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` is returned.

路由生成遵循一个类似的迭代过程，但开始于用户或框架代码调用到路由集合的GetVirtualPath方法时。每个路由的``GetVirtualPath``方法都会被调用，直到返回一个不为空的 :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData`.

The primary inputs to ``GetVirtualPath`` are:

``GetVirtualPath``的主要输入是：

- :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathContext` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathContext.HttpContext`
- :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathContext` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathContext.Values`
- :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathContext` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathContext.AmbientValues`

Routes primarily use the route values provided by the ``Values`` and ``AmbientValues`` to decide where it is possible to generate a URL and what values to include. The ``AmbientValues`` are the set of route values that were produced from matching the current request with the routing system. In contrast, ``Values`` are the route values that specify how to generate the desired URL for the current operation. The ``HttpContext`` is provided in case a route needs to get services or additional data associated with the current context.

路由主要使用``Values`` 和 ``AmbientValues`` 提供的路由值来决定在哪儿生成一个URL以及包含什么值. ``AmbientValues``是随着路由系统匹配当前请求而产生的一系列路由值。 相反，``Values``是用于指定如何生成当前操作所需的URL的路由值。提供 ``HttpContext``是以防路由需要获取服务或当前上下文相关的数据。

.. tip:: Think of ``Values`` as being a set of overrides for the ``AmbientValues``. URL generation tries to reuse route values from the current request to make it easy to generate URLs for links using the same route or route values.

.. 建议:: ``Values`` 看做是对``AmbientValues``的重载.URL生成尝试重用来自当前请求的路由值，以便使用相同路由或路由值的链接更容易生成URL。


The output of ``GetVirtualPath`` is a :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData`. ``VirtualPathData`` is a parallel of ``RouteData``; it contains the ``VirtualPath`` for the output URL as well as the some additional properties that should be set by the route.

``GetVirtualPath``的输出是一个:dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData`.``VirtualPathData``是一个并行的 ``RouteData``;它包含了输出URL的虚拟路径以及应该由路由设置的一些额外的属性。


The :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.VirtualPath`
property contains the *virtual path* produced by the route. Depending on your needs you may need to process the path further. For instance, if you want to render the generated URL in HTML you need to prepend the base path of the application.

:dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.VirtualPath`属性包含了路由生成的虚拟路径.根据你的需求，可能需要进一步处理的。例如，如果你想在HTML中呈现生成的URL，你需要预先设置好应用的基础路径。

The :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.Router` is a reference to the route that successfully generated the URL.

:dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.Router`是一个成功生成URL路由的参考。

The :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.DataTokens` properties is a dictionary of additional data related to the route that generated the URL. This is the parallel of ``RouteData.DataTokens``.

:dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathData` :dn:prop:`~Microsoft.AspNetCore.Routing.VirtualPathData.DataTokens` 属性是一个关联到生成URL的路由的附加数据的字典集合，这个和``RouteData.DataTokens``是并行的。


Creating routes
^^^^^^^^^^^^^^^
创建路由
^^^^^^^^^^^^^^^

Routing provides the :dn:cls:`~Microsoft.AspNetCore.Routing.Route` class as the standard implementation of ``IRouter``. ``Route`` uses the *route template* syntax to define patterns that will match against the URL path when :dn:method:`~Microsoft.AspNetCore.Routing.IRouter.RouteAsync` is called. ``Route`` will use the same route template to generate a URL when :dn:method:`~Microsoft.AspNetCore.Routing.IRouter.GetVirtualPath` is called.

路由提供了 :dn:cls:`~Microsoft.AspNetCore.Routing.Route` 类作为``IRouter``的标准实现。当调用:dn:method:`~Microsoft.AspNetCore.Routing.IRouter.RouteAsync`方法时,``Route``使用 *路由模板*语法定义匹配URL路径的模式。当调用:dn:method:`~Microsoft.AspNetCore.Routing.IRouter.GetVirtualPath`方法时，``Route``会使用相同的路由模板生成URL.

Most applications will create routes by calling ``MapRoute`` or one of the similar extension methods defined on :dn:iface:`~Microsoft.AspNetCore.Routing.IRouteBuilder`. All of these methods will create an instance of ``Route`` and add it to the route collection.

大多数的应用会通过调用``MapRoute``方法或者定义在:dn:iface:`~Microsoft.AspNetCore.Routing.IRouteBuilder`接口上的一个类似的扩展方法来创建路由。所有的这些方法会创建一个``Route`` 实例并添加到路由集合中。

.. note:: :dn:method:`~Microsoft.AspNetCore.Builder.MapRouteRouteBuilderExtensions.MapRoute` doesn't take a route handler parameter - it only adds routes that will be handled by the :dn:prop:`~Microsoft.AspNetCore.Routing.IRouteBuilder.DefaultHandler`. Since the default handler is an :dn:iface:`~Microsoft.AspNetCore.Routing.IRouter`, it may decide not to handle the request. For example, ASP.NET MVC is typically configured as a default handler that only handles requests that match an available controller and action. To learn more about routing to MVC, see :doc:`/mvc/controllers/routing`.

.. 注意:: :dn:method:`~Microsoft.AspNetCore.Builder.MapRouteRouteBuilderExtensions.MapRoute`不需要路由处理参数--它只添加将被 :dn:prop:`~Microsoft.AspNetCore.Routing.IRouteBuilder.DefaultHandler`处理的路由。由于默认处理程序是一个:dn:iface:`~Microsoft.AspNetCore.Routing.IRouter`对象，可能决定不去处理请求。MVC通常配置了一个默认处理程序，它只处理能匹配到可用的控制器和操作的请求。了解更多关于MVC的路由,请点击:doc:`/mvc/controllers/routing`.



This is an example of a ``MapRoute`` call used by a typical ASP.NET MVC route definition:

这是一个典型的ASP.NET MVC路由调用``MapRoute``的例子

.. code-block:: c#

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");

This template will match a URL path like ``/Products/Details/17`` and extract the route values ``{ controller = Products, action = Details, id = 17 }``. The route values are determined by splitting the URL path into segments, and matching each segment with the *route parameter* name in the route template. Route parameters are named. They are defined by enclosing the parameter name in braces ``{ }``.

这个模板会匹配像``/Products/Details/17``这样的URL路径，而且提取的路由值是``{ controller = Products, action = Details, id = 17 }``.这个路由值由几个URL段组成，通过路由参数名称匹配路由模板中的每一段.路由参数被命名，它们是由大括号``{ }``中的参数名定义的。


The template above could also match the URL path ``/`` and would produce the values ``{ controller = Home, action = Index }``. This happens because the ``{controller}`` and ``{action}`` route parameters have default values, and the ``id`` route parameter is optional. An equals ``=`` sign followed by a value after the route parameter name defines a default value for the parameter. A question mark ``?`` after the route parameter name defines the parameter as optional. Route parameters with a default value *always* produce a route value when the route matches - optional parameters will not produce a route value if there was no corresponding URL path segment.

上面的这个模板也能匹配URL路径 ``/``,而且还将产生值为``{ controller = Home, action = Index }``。这是因为``{controller}`` 和``{action}``都定义了默认值，而且 ``id``是可选的.路由参数名等号``=`` 后面的值是作为该参数的默认值。而参数名后面的问号 ``?``标记着这个参数是可选的。带有默认参数的路由值*always*是会在路由匹配的时候产生一个路由值--而如果没有相应的URL路径段，可选参数将不会产生一个路由值。


See route-template-reference_ for a thorough description of route template features and syntax.
查看路由模板特性和语法的完整描述请移步 route-template-reference_


This example includes a *route constraint*:

这个示例包含了一个 *路由约束*:

.. code-block:: c#

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id:int}");

This template will match a URL path like ``/Products/Details/17``, but not ``/Products/Details/Apples``. The route parameter definition ``{id:int}`` defines a *route constraint* for the ``id`` route parameter. Route constraints implement ``IRouteConstraint`` and inspect route values to verify them. In this example the route value ``id`` must be convertable to an integer. See route-constraint-reference_ for a more detailed explaination of route constraints that are provided by the framework.

Additional overloads of ``MapRoute`` accept values for ``constraints``, ``dataTokens``, and ``defaults``. These additional parameters of ``MapRoute`` are defined as type ``object``. The typical usage of these parameters is to pass an anonymously typed object, where the property names of the anonymous type match route parameter names.

The following two examples create equivalent routes:

.. code-block:: c#

    routes.MapRoute(
        name: "default_route",
        template: "{controller}/{action}/{id?}",
        defaults: new { controller = "Home", action = "Index" });

    routes.MapRoute(
        name: "default_route",
        template: "{controller=Home}/{action=Index}/{id?}");

.. tip:: The inline syntax for defining constraints and defaults can be more convenient for simple routes. However, there are features such as data tokens which are not supported by inline syntax.

.. review-required: changed template and add MVC controller sample

This example demonstrates a few more features:

.. code-block:: c#

  routes.MapRoute(
    name: "blog",
    template: "Blog/{*article}",
    defaults: new { controller = "Blog", action = "ReadArticle" });

This template will match a URL path like ``/Blog/All-About-Routing/Introduction`` and will extract the values ``{ controller = Blog, action = ReadArticle, article = All-About-Routing/Introduction }``. The default route values for ``controller`` and ``action`` are produced by the route even though there are no corresponding route parameters in the template. Default values can be specified in the route template. The ``article`` route parameter is defined as a *catch-all* by the appearance of an asterix ``*`` before the route parameter name. Catch-all route parameters capture the remainder of the URL path, and can also match the empty string.

This example adds route constraints and data tokens:

.. code-block:: c#

  routes.MapRoute(
      name: "us_english_products",
      template: "en-US/Products/{id}",
      defaults: new { controller = "Products", action = "Details" },
      constraints: new { id = new IntRouteConstraint() },
      dataTokens: new { locale = "en-US" });

This template will match a URL path like ``/en-US/Products/5`` and will extract the values ``{ controller = Products, action = Details, id = 5 }`` and the data tokens ``{ locale = en-US }``.


.. image:: routing/_static/tokens.png

.. _url-generation:

URL generation
^^^^^^^^^^^^^^^
The ``Route`` class can also perform URL generation by combining a set of route values with its route template. This is logically the reverse process of matching the URL path.

.. tip:: To better understand URL generation, imagine what URL you want to generate and then think about how a route template would match that URL. What values would be produced? This is the rough equivalent of how URL generation works in the ``Route`` class.

This example uses a basic ASP.NET MVC style route:

.. code-block:: c#

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");

With the route values ``{ controller = Products, action = List }``, this route will generate the URL ``/Products/List``. The route values are substituted for the corresponding route parameters to form the URL path. Since ``id`` is an optional route parameter, it's no problem that it doesn't have a value.

With the route values ``{ controller = Home, action = Index }``, this route will generate the URL ``/``. The route values that were provided match the default values so the segments corresponding to those values can be safely omitted. Note that both URLs generated would round-trip with this route definition and produce the same route values that were used to generate the URL.

.. tip:: An app using ASP.NET MVC should use :dn:cls:`~Microsoft.AspNetCore.Mvc.Routing.UrlHelper` to generate URLs instead of calling into routing directly.

For more details about the URL generation process, see url-generation-reference_.

.. _using-routing-middleware:

Using Routing Middleware
-------------------------
To use routing middleware, add it to the **dependencies** in *project.json*:

``"Microsoft.AspNetCore.Routing": <current version>``

Add routing to the service container in *Startup.cs*:

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :dedent: 8
  :language: c#
  :lines: 11-14
  :emphasize-lines: 3

Routes must configured in the ``Configure`` method in the ``Startup`` class. The sample below uses these APIs:

- :dn:cls:`~Microsoft.AspNetCore.Routing.RouteBuilder`
- :dn:method:`~Microsoft.AspNetCore.Routing.RouteBuilder.Build`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapGet`  Matches only HTTP GET requests
- :dn:method:`~Microsoft.AspNetCore.Builder.RoutingBuilderExtensions.UseRouter`

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :dedent: 8
  :start-after: // Routes must configured in Configure
  :end-before: // Show link generation when no routes match.

The table below shows the resposes with the given URIs.

===================== ====================================================
URI                    Response
===================== ====================================================
/package/create/3     Hello! Route values: [operation, create], [id, 3]
/package/track/-3     Hello! Route values: [operation, track], [id, -3]
/package/track/-3/    Hello! Route values: [operation, track], [id, -3]
/package/track/       <Fall through, no match>
GET /hello/Joe        Hi, Joe!
POST /hello/Joe       <Fall through, matches HTTP GET only>
GET /hello/Joe/Smith  <Fall through, no match>
===================== ====================================================

If you are configuring a single route, call ``app.UseRouter`` passing in an ``IRouter`` instance. You won't need to call ``RouteBuilder``.

The framework provides a set of extension methods for creating routes such as:

- :dn:method:`~Microsoft.AspNetCore.Builder.MapRouteRouteBuilderExtensions.MapRoute`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapGet`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapPost`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapPut`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapDelete`
- :dn:method:`~Microsoft.AspNetCore.Routing.RequestDelegateRouteBuilderExtensions.MapVerb`

Some of these methods such as ``MapGet`` require a :dn:delegate:`~Microsoft.AspNetCore.Http.RequestDelegate` to be provided. The ``RequestDelegate`` will be used as the *route handler* when the route matches. Other methods in this family allow configuring a middleware pipeline which will be used as the route handler. If the *Map* method doesn't accept a handler, such as ``MapRoute``, then it will use the :dn:prop:`~Microsoft.AspNetCore.Routing.IRouteBuilder.DefaultHandler`.

The ``Map[Verb]`` methods use constraints to limit the route to the HTTP Verb in the method name. For example, see `MapGet <https://github.com/aspnet/Routing/blob/1.0.0/src/Microsoft.AspNetCore.Routing/RequestDelegateRouteBuilderExtensions.cs#L85-L88>`__ and `MapVerb <https://github.com/aspnet/Routing/blob/1.0.0/src/Microsoft.AspNetCore.Routing/RequestDelegateRouteBuilderExtensions.cs#L156-L180>`__.

.. _route-template-reference:

Route Template Reference
------------------------
Tokens within curly braces (``{ }``) define *route parameters* which will be bound if the route is matched. You can define more than one route parameter in a route segment, but they must be separated by a literal value. For example ``{controller=Home}{action=Index}`` would not be a valid route, since there is no literal value between ``{controller}`` and ``{action}``. These route parameters must have a name, and may have additional attributes specified.

Literal text other than route parameters (for example, ``{id}``) and the path separator ``/`` must match the text in the URL. Text matching is case-insensitive and based on the decoded representation of the URLs path. To match the literal route parameter delimiter ``{`` or  ``}``, escape it by repeating the character (``{{`` or ``}}``).

URL patterns that attempt to capture a filename with an optional file extension have additional considerations. For example, using the template ``files/{filename}.{ext?}`` -
When both ``filename`` and ``ext`` exist, both values will be populated. If only ``filename`` exists in the URL, the route matches because the trailing period ``.`` is  optional. The following URLs would match this route:

- ``/files/myFile.txt``
- ``/files/myFile.``
- ``/files/myFile``

You can use the ``*`` character as a prefix to a route parameter to bind to the rest of the URI - this is called a *catch-all* parameter. For example, ``blog/{*slug}`` would match any URI that started with ``/blog`` and had any value following it (which would be assigned to the ``slug`` route value). Catch-all parameters can also match the empty string.

Route parameters may have *default values*, designated by specifying the default after the parameter name, separated by an ``=``. For example, ``{controller=Home}`` would define ``Home`` as the default value for ``controller``. The default value is used if no value is present in the URL for the parameter. In addition to default values, route parameters may be optional (specified by appending a ``?`` to the end of the parameter name, as in ``id?``). The difference between optional and "has default" is that a route parameter with a default value always produces a value; an optional parameter has a vaule only when one is provided.

Route parameters may also have constraints, which must match the route value bound from the URL. Adding a colon ``:`` and constraint name after the route parameter name specifies an *inline constraint* on a route parameter. If the constraint requires arguments those are provided enclosed in parentheses ``( )`` after the constraint name. Multiple inline constraints can be specified by appending another colon ``:`` and constraint name. The constraint name is passed to the :dn:iface:`~Microsoft.AspNetCore.Routing.IInlineConstraintResolver` service to create an instance of :dn:iface:`~Microsoft.AspNetCore.Routing.IRouteConstraint` to use in URL processing. For example, the route template ``blog/{article:minlength(10)}`` specifies the ``minlength`` constraint with the argument ``10``. For more description route constraints, and a listing of the constraints provided by the framework, see route-constraint-reference_.

The following table demonstrates some route templates and their behavior.


+-----------------------------------+--------------------------------+------------------------------------------------+
| Route Template                    | Example Matching URL           | Notes                                          |
+===================================+================================+================================================+
| hello                             | | /hello                       | | Only matches the single path ‘/hello’        +
+-----------------------------------+--------------------------------+------------------------------------------------+
|{Page=Home}                        | | /                            | | Matches and sets ``Page`` to ``Home``        |
+-----------------------------------+--------------------------------+------------------------------------------------+
|{Page=Home}                        | | /Contact                     | | Matches and sets ``Page`` to ``Contact``     |
+-----------------------------------+--------------------------------+------------------------------------------------+
| {controller}/{action}/{id?}       | | /Products/List               | | Maps to ``Products`` controller and ``List`` |
|                                   | |                              | | action                                       |
+-----------------------------------+--------------------------------+------------------------------------------------+
| {controller}/{action}/{id?}       | | /Products/Details/123        | | Maps to ``Products`` controller and          |
|                                   | |                              | | ``Details`` action.  ``id`` set to 123       |
+-----------------------------------+--------------------------------+------------------------------------------------+
| {controller=Home}/                | |   /                          | | Maps to ``Home`` controller and ``Index``    |
|            {action=Index}/{id?}   | |                              | | method; ``id`` is ignored.                   |
+-----------------------------------+--------------------------------+------------------------------------------------+

Using a template is generally the simplest approach to routing. Constraints and defaults can also be specified outside the route template.

.. tip:: Enable :doc:`logging` to see how the built in routing implementations, such as ``Route``, match requests.

.. _route-constraint-reference:

Route Constraint Reference
--------------------------
Route constraints execute when a ``Route`` has matched the syntax of the incoming URL and tokenized the URL path into route values. Route constraints generally inspect the route value associated via the route template and make a simple yes/no decision about whether or not the value is acceptable. Some route constraints use data outside the route value to consider whether the request can be routed. For example, the `HttpMethodRouteConstraint <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Routing/Constraints/HttpMethodRouteConstraint/index.html#httpmethodrouteconstraint-class>`_ can accept or reject a request based on its HTTP verb.

.. warning:: Avoid using constraints for **input validation**, because doing so means that invalid input will result in a 404 (Not Found) instead of a 400 with an appropriate error message. Route constraints should be used to **disambiguate** between similar routes, not to validate the inputs for a particular route.

The following table demonstrates some route constraints and their expected behavior.

.. TODO to-do when we migrate to MD, make sure this table doesn't require a scroll bar

.. list-table:: Inline Route Constraints
  :header-rows: 1

  * - constraint
    - Example
    - Example Match
    - Notes
  * - ``int``
    - {id:int}
    - 123
    - Matches any integer
  * - ``bool``
    - {active:bool}
    - true
    - Matches ``true`` or ``false``
  * - ``datetime``
    - {dob:datetime}
    - 2016-01-01
    - Matches a valid ``DateTime`` value (in the invariant culture - see `options <http://msdn.microsoft.com/en-us/library/aszyst2c(v=vs.110).aspx>`_)
  * - ``decimal``
    - {price:decimal}
    - 49.99
    - Matches a valid ``decimal`` value
  * - ``double``
    - {weight:double}
    - 4.234
    - Matches a valid ``double`` value
  * - ``float``
    - {weight:float}
    - 3.14
    - Matches a valid ``float`` value
  * - ``guid``
    - {id:guid}
    - 7342570B-<snip>
    - Matches a valid ``Guid`` value
  * - ``long``
    - {ticks:long}
    - 123456789
    - Matches a valid ``long`` value
  * - ``minlength(value)``
    - {username:minlength(5)}
    - steve
    - String must be at least 5 characters long.
  * - ``maxlength(value)``
    - {filename:maxlength(8)}
    - somefile
    - String must be no more than 8 characters long.
  * - ``length(min,max)``
    - {filename:length(4,16)}
    - Somefile.txt
    - String must be at least 8 and no more than 16 characters long.
  * - ``min(value)``
    - {age:min(18)}
    - 19
    - Value must be at least 18.
  * - ``max(value)``
    - {age:max(120)}
    - 91
    - Value must be no more than 120.
  * - ``range(min,max)``
    - {age:range(18,120)}
    - 91
    - Value must be at least 18 but no more than 120.
  * - ``alpha``
    - {name:alpha}
    - Steve
    - String must consist of alphabetical characters.
  * - ``regex(expression)``
    - {ssn:regex(\d{3}-\d{2}-\d{4})}
    - 123-45-6789
    - String must match the provided regular expression.
  * - ``required``
    - {name:required}
    - Steve
    - Used to enforce that a non-parameter value is present during during URL generation.

.. warning:: Route constraints that verify the URL can be converted to a CLR type (such as ``int`` or ``DateTime``) always use the invariant culture - they assume the URL is non-localizable. The framework-provided route constraints do not modify the values stored in route values. All route values parsed from the URL will be stored as strings. For example, the `Float route constraint <https://github.com/aspnet/Routing/blob/1.0.0/src/Microsoft.AspNetCore.Routing/Constraints/FloatRouteConstraint.cs#L44-L60>`__ will attempt to convert the route value to a float, but the converted value is used only to verify it can be converted to a float.

.. tip:: To constrain a parameter to a known set of possible values, you can use a regular expression ( for example ``{action:regex(list|get|create)}``. This would only match the ``action`` route value to ``list``, ``get``, or ``create``. If passed into the constraints dictionary, the string "list|get|create" would be equivalent. Constraints that are passed in the constraints dictionary (not inline within a template) that don't match one of the known constraints are also treated as regular expressions.

.. _url-generation-reference:

URL Generation Reference
------------------------
The example below shows how to generate a link to a route given a dictionary of route values and a ``RouteCollection``.

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :start-after: // Show link generation when no routes match.
  :end-before: // End of app.Run
  :dedent: 12

The ``VirtualPath`` generated at the end of the sample above is ``/package/create/123``.

The second parameter to the :dn:cls:`~Microsoft.AspNetCore.Routing.VirtualPathContext` constructor is a collection of `ambient values`. Ambient values provide convenience by limiting the number of values a developer must specify within a certain request context. The current route values of the current request are considered ambient values for link generation. For example, in an ASP.NET MVC app if you are in the ``About`` action of the ``HomeController``, you don't need to specify the controller route value to link to the ``Index`` action (the ambient value of ``Home`` will be used).

Ambient values that don't match a parameter are ignored, and ambient values are also ignored when an explicitly-provided value overrides it, going from left to right in the URL.

Values that are explicitly provided but which don't match anything are added to the query string. The following table shows the result when using the route template ``{controller}/{action}/{id?}``.

.. list-table:: Generating links with ``{controller}/{action}/{id?}`` template
  :header-rows: 1


  * - Ambient Values
    - Explicit Values
    - Result

  * - controller="Home"
    - action="About"
    - ``/Home/About``
  * - controller="Home"
    - controller="Order",action="About"
    - ``/Order/About``
  * - controller="Home",color="Red"
    - action="About"
    - ``/Home/About``
  * - controller="Home"
    - action="About",color="Red"
    - ``/Home/About?color=Red``


If a route has a default value that doesn't correspond to a parameter and that value is explicitly provided, it must match the default value. For example:

.. code-block:: c#

  routes.MapRoute("blog_route", "blog/{*slug}",
    defaults: new { controller = "Blog", action = "ReadPost" });

Link generation would only generate a link for this route when the matching values for controller and action are provided.

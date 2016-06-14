.. _fundamentals-middleware:

Middleware 
==========

中间件
========

作者： `Steve Smith`_ and `Rick Anderson`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

.. contents:: Sections:
  :local:
  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/middleware/sample>`__

`访问或下载样例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/middleware/sample>`__

What is middleware
------------------

什么是中间件
------------------

Middleware are software components that are assembled into an application pipeline to handle requests and responses. Each component chooses whether to pass the request on to the next component in the pipeline, and can perform certain actions before and after the next component is invoked in the pipeline. Request delegates are used to build the request pipeline. The request delegates handle each HTTP request.

中间件是用于组成应用程序管道来处理请求和响应的组成部分。管道内的每一个组件都可以选择是否将请求交给下一个组件、并在调用管道下一个组件之前和之后执行某些操作。请求委托被用来建立请求管道，请求委托处理每一个 HTTP 请求。

Request delegates are configured using `Run <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/RunExtensions/index.html>`__, `Map <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/MapExtensions/index.html?highlight=microsoft.aspnet.builder.mapextensions#Microsoft.AspNet.Builder.MapExtensions.Map>`__, and `Use <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/UseExtensions/index.html?highlight=microsoft.aspnet.builder.useextensions#Microsoft.AspNet.Builder.UseExtensions.Use>`__ extension methods on the `IApplicationBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html>`_ type that is passed into the ``Configure`` method in the ``Startup`` class. An individual request delegate can be specified in-line as an anonymous method, or it can be defined in a reusable class. These reusable classes  are `middleware`, or `middleware components`. Each middleware component in the request pipeline is responsible for invoking the next component in the pipeline, or short-circuiting the chain if appropriate.

请求委托通过使用 `IApplicationBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html>`_ 类型的 `Run <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/RunExtensions/index.html>`__、`Map <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/MapExtensions/index.html?highlight=microsoft.aspnet.builder.mapextensions#Microsoft.AspNet.Builder.MapExtensions.Map>`__ 以及 `Use <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/UseExtensions/index.html?highlight=microsoft.aspnet.builder.useextensions#Microsoft.AspNet.Builder.UseExtensions.Use>`__ 扩展方法来配置，并通过 ``Configure`` 方法传给 ``Startup`` 类。每个单独的请求委托都可以被指定内嵌为一个匿名方法，或其定义在一个可重用的类中。这些可重用的类被称作``中间件``或``中间件组件``。每个位于请求管道内的中间件组件负责调用管道中下一个组件，或适时短路调用链。

:doc:`/migration/http-modules` explains the difference between request pipelines in ASP.NET Core and the previous versions and provides more middleware samples.

:doc:`/migration/http-modules` 解释了请求管道在 ASP.NET Core 和之前版本之间的区别，并提供了更多中间件样例。

Creating a middleware pipeline with IApplicationBuilder
-------------------------------------------------------

用 IApplicationBuilder 创建中间件管道
-------------------------------------------------------

The ASP.NET request pipeline consists of a sequence of request delegates, called one after the next, as this diagram shows (the thread of execution follows the black arrows):

ASP.NET 请求管道由一系列的请求委托所构成，它们一个接着一个被调用，如下如所示（该执行线程按黑色箭头的顺序执行）：

.. image:: middleware/_static/request-delegate-pipeline.png

Each delegate has the opportunity to perform operations before and after the next delegate. Any delegate can choose to stop passing the request on to the next delegate, and instead handle the request itself. This is referred to as short-circuiting the request pipeline, and is desirable because it allows unnecessary work to be avoided. For example, an authorization middleware might only call the next delegate if the request is authenticated; otherwise it could short-circuit the pipeline and return a "Not Authorized" response. Exception handling delegates need to be called early on in the pipeline, so they are able to catch exceptions that occur in deeper calls within the pipeline.

每个委托在下一个委托之前和之后都有机会执行操作。任何委托都能选择停止传递到下一个委托，转而自己处理该请求。这被叫做请求管道的短路，而且是一种有意义的设计，因为它可以避免不必要的工作。比方说，一个授权（authorization）中间件只有在通过身份验证之后才调用下一个委托，否则它就会被短路并返回「Not Authorized」的响应。异常处理委托需要在管道的早期被调用，这样它们就能够捕捉到发生在管道内更深层次出现的异常了。

You can see an example of setting up the request pipeline in the default web site template that ships with Visual Studio 2015. The ``Configure`` method adds the following middleware components:

你可以看一下 Visual Studio 2015 附带的默认 Web 站点模板关于请求管道设置的例子。``Configure`` 方法增加了下列这些中间件组件：

#. Error handling (for both development and non-development environments)
#. IIS HttpPlatformHandler reverse proxy module. This module handles forwarded Windows Authentication, request schemes, remote IPs, and so on.
#. Static file server
#. Authentication
#. MVC


#. 错误处理（同时针对于开发环境和非开发环境）
#. IIS HttpPlatformHandler 反向代理模块。此模块处理转发 Windows 身份验证、请求机制、远程 IP 地址等信息。
#. 静态文件服务器
#. 身份验证
#. MVC

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 58-86
  :dedent: 8
  :emphasize-lines: 8-10,14,17,19,23

In the code above (in non-development environments), `UseExceptionHandler <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/ExceptionHandlerExtensions/index.html>`__ is the first middleware added to the pipeline, therefore will catch any exceptions that occur in later calls. 

上面非开发环境的代码中，`UseExceptionHandler <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/ExceptionHandlerExtensions/index.html>`__ 是第一个被加入到管道中的中间件，因此将会捕获之后调用中出现的任何异常。

The `static file module <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html>`__ provides no authorization checks. Any files served by it, including those under *wwwroot* are publicly available. If you want to serve files based on authorization:

`静态文件模块 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html>`__ 不提供授权检查，任何文件——包括那些位于 *wwwroot* 下的文件——都是公开的可被访问的。如果你想基于授权来提供这些文件，那么：

#. Store them outside of *wwwroot* and any directory accessible to the static file middleware.  
#. Deliver them through a controller action, returning a `FileResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/FileResult/index.html>`__ where authorization is applied.

#. 将它们存放在 *wwwroot* 外面以及任何静态文件中间件都可访问得到的目录。  
#. 利用控制器操作来判断授权是否允许，如果允许则通过返回 `FileResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/FileResult/index.html>`__ 来提供它们。

A request that is handled by the static file module will short circuit the pipeline. (see :doc:`static-files`.) If the request is not handled by the static file module, it's passed on to the `Identity module <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/BuilderExtensions/index.html#methods>`__, which performs authentication. If the request is not authenticated, the pipeline is short circuited. If the request does not fail authentication, the last stage of this pipeline is called, which is the MVC framework.

被静态文件模块处理的请求会在管道中被短路（参见 :doc:`static-files`）。如果请求未被交由静态文件模块来处理，那么它就会被传给 `Identity 模块 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/BuilderExtensions/index.html#methods>`__ 执行身份验证。如果未通过身份验证，则管道将被短路。如果请求的身份验证没有失败，则管道的最后一站是 MVC 框架。

.. note:: The order in which you add middleware components is generally the order in which they take effect on the request, and then in reverse for the response. This can be critical to your app’s security, performance and functionality. In the code above, the `static file middleware <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html>`__ is called early in the pipeline so it can handle requests and short circuit without going through unnecessary components. The authentication middleware is added to the pipeline before anything that handles requests that need to be authenticated. Exception handling must be registered before other middleware components in order to catch exceptions thrown by those components. 

.. note:: 你添加中间件组件的顺序通常会影响到它们处理请求的顺序，然后在响应时则以相反的顺序返回。这对应用程序安全、性能和功能很关键。在上面的代码中，`静态文件中间件 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html>`__ 在管道的早期被调用，这样就能处理并及时短路管道，以避免请求走到不必要的组件中。身份验证中间件被添加在所有需要身份认证的处理请求的前面。异常处理必须被注册在其它中间件之前以便捕获其它组件的异常。

The simplest possible ASP.NET application sets up a single request delegate that handles all requests. In this case, there isn't really a request "pipeline", so much as a single anonymous function that is called in response to every HTTP request.

最简单的 ASP.NET 应用程序是使用单个请求委托来处理所有请求。事实上在这种情况下并不存在所谓的「管道」，调用单个匿名函数以相应每个 HTTP 请求。

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 23-26
	:dedent: 12

The first ``App.Run`` delegate terminates the pipeline. In the following example, only the first delegate ("Hello, World!") will run.

第一个 ``App.Run`` 委托中断了管道。在下面的例子中，只有第一个委托（「Hello, World!」）会被运行。

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 20-31
	:emphasize-lines: 5
	:dedent: 8

You chain multiple request delegates together; the ``next`` parameter represents the next delegate in the pipeline. You can terminate (short-circuit) the pipeline by *not* calling the `next` parameter. You can typically perform actions both before and after the next delegate, as this example demonstrates:

将多个请求委托彼此链接在一起；``next`` 参数表示管道内下一个委托。如果*不*调用 `next` 参数，就可中断（短路）管道。你通常可以在执行下一个委托之前和之后执行一些操作，如下例所示：

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 34-49
	:emphasize-lines: 5,8,14
	:dedent: 8

.. warning:: Avoid modifying ``HttpResponse`` after invoking next, one of the next components in the pipeline may have written to the response, causing it to be sent to the client.

.. warning:: 应当避免在修改了 ``HttpResponse`` 之后还调用管道内下一个会修改响应的组件，从而导致它被送到客户端处。

.. note:: This ``ConfigureLogInline`` method is called when the application is run with an environment set to ``LogInline``. Learn more about :doc:`environments`. We will be using variations of ``Configure[Environment]`` to show different options in the rest of this article. The easiest way to run the samples in Visual Studio is with the ``web`` command, which is configured in *project.json*. See also :doc:`startup`.

.. note:: 当应用程序用 ``LogInline`` 环境设置来运行时，这个 ``ConfigureLogInline`` 方法就会被调动。要了解更多请访问环境 :doc:`environments` 一章。本文剩下的篇幅将使用变化的 ``Configure[Environment]`` 来展示不同的选项。 Visual Studio 中运行示例代码的最简单办法是使用 ``web`` 命令，该命令由 *project.json* 文件所配置。也可参考 :doc:`startup` 。

In the above example, the call to ``await next.Invoke()`` will call into the next delegate ``await context.Response.WriteAsync("Hello from " + _environment);``. The client will receive the expected response ("Hello from LogInline"), and the server's console output includes both the before and after messages:

在上例中，当调用 ``await next.Invoke()`` 时，会进入下一个委托并调用 ``await context.Response.WriteAsync("Hello from " + _environment);`` 。客户端会如期得到相应（“Hello from LogInline”），同时服务端这边的控制台将先后输出如下信息：

.. image:: middleware/_static/console-loginline.png

..  _middleware-run-map-use:

Run, Map, and Use
^^^^^^^^^^^^^^^^^

Run，Map 与 Use
^^^^^^^^^^^^^^^^^

You configure the HTTP pipeline using `Run <https://docs.asp.snet/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/RunExtensions/index.html>`__, `Map <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/MapExtensions/index.html>`__,  and `Use <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/UseExtensions/index.html>`__. The ``Run`` method short circuits the pipeline (that is, it will not call a ``next`` request delegate). Thus, ``Run`` should only be called at the end of your pipeline. ``Run`` is a convention, and some middleware components may expose their own Run[Middleware] methods that should only run at the end of the pipeline. The following two middleware are equivalent as the ``Use`` version doesn't use the ``next`` parameter:

你可以通过 `Run <https://docs.asp.snet/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/RunExtensions/index.html>`__、`Map <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/MapExtensions/index.html>`__ 和 `Use <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/UseExtensions/index.html>`__ 配置 HTTP 管道。``Run`` 方法将会短路管道（因为它不会调用 ``next`` 请求委托）。``Run`` 是一种惯例，有些中间件组件可能会暴露他们自己的 Run[Middleware] 方法，而这些方法只能在管道末尾处运行。下面这两个中间件等价的，其中有用到 ``Use`` 的版本没有使用 ``next`` 参数：

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 65-79
	:emphasize-lines: 3,11
	:dedent: 8

.. note:: The `IApplicationBuilder  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html>`__ interface exposes a single ``Use`` method, so technically they're not all *extension* methods.

.. note:: `IApplicationBuilder  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html>`__ 接口向外暴露了一个 ``Use`` 方法，因此从技术上来说它们并不完全是*扩展*方法。

We've already seen several examples of how to build a request pipeline with ``Use``. ``Map*`` extensions are used as a convention for branching the pipeline. The current implementation supports branching based on the request's path, or using a predicate. The ``Map`` extension method is used to match request delegates based on a request's path. ``Map`` simply accepts a path and a function that configures a separate middleware pipeline. In the following example, any request with the base path of ``/maptest`` will be handled by the pipeline configured in the ``HandleMapTest`` method.

我们已经看了几个关于如何通过 ``Use`` 构建请求管道的例子，同时约定了 ``Map*`` 扩展被用于分支管道。当前的实现已支持基于请求路径或使用谓词来进入分支。``Map`` 扩展方法用于匹配基于请求路径的请求委托。``Map`` 只接受路径，并配置单独的中间件管道的功能。在下例中，任何基于路径 ``/maptest`` 的请求都会被管道中所配置的 ``HandleMapTest`` 方法所处理。

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 81-93
	:emphasize-lines: 11
	:dedent: 8

.. note:: When ``Map`` is used, the matched path segment(s) are removed from ``HttpRequest.Path`` and appended to ``HttpRequest.PathBase`` for each request.

.. note:: 当使用了 ``Map``，每个请求所匹配的路径段将从 ``HttpRequest.Path`` 中移除，并附加到 ``HttpRequest.PathBase`` 中。

In addition to path-based mapping, the ``MapWhen`` method supports predicate-based middleware branching, allowing separate pipelines to be constructed in a very flexible fashion. Any predicate of type ``Func<HttpContext, bool>`` can be used to map requests to a new branch of the pipeline. In the following example, a simple predicate is used to detect the presence of a query string variable ``branch``:

除基于路径的映射外，``MapWhen`` 方法还支持基于谓词的中间件分支，允许以非常灵活的方式构建单独的管道。任何 ``Func<HttpContext, bool>`` 类型的谓语都被用于将请求映射到新的管到分支。在下例中使用了一个简单的谓词来检测查询字符串变量 ``branch`` 是否存在：

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 95-113
	:emphasize-lines: 5,11-13
	:dedent: 8

Using the configuration shown above, any request that includes a query string value for ``branch`` will use the pipeline defined in the ``HandleBranch`` method (in this case, a response of "Branch used."). All other requests (that do not define a query string value for ``branch``) will be handled by the delegate defined on line 17.

使用了上述设置后，任何包含请求字符 ``branch`` 的请求将使用定义于 ``HandleBranch`` 方法内的管道（其响应就将是“Branch used.”）。其他请求（即没有为 ``branch`` 定义查询字符串值）将被第 17 行所定义的委托处理。

You can also nest Maps:

也可以嵌套映射：

.. code-block:: javascript  

   app.Map("/level1", level1App => {
       level1App.Map("/level2a", level2AApp => {
           // "/level1/level2a"
           //...
       });
       level1App.Map("/level2b", level2BApp => {
           // "/level1/level2b"
           //...
       });
   });
   
Built-in middleware
-------------------

内置中间件
-------------------

ASP.NET ships with the following middleware components:

ASP.NET 带来了下列中间件组件：

.. list-table:: Middleware
  :header-rows: 1

  *  - Middleware
     - Description
  *  - :doc:`Authentication </security/authentication/index>`
     - Provides authentication support.
  *  - :doc:`CORS </security/cors>`
     - Configures Cross-Origin Resource Sharing.
  *  - :doc:`Diagnostics <diagnostics>`
     - Includes support for error pages and runtime information.
  *  - :doc:`Routing <routing>`
     - Define and constrain request routes.
  *  - :ref:`Session <session>`
     - Provides support for managing user sessions.
  *  - :doc:`Static Files <static-files>`
     - Provides support for serving static files, and directory browsing.

.. list-table:: 中间件
  :header-rows: 1

  *  - 中间件
     - 描述
  *  - :doc:`身份验证（Authentication） </security/authentication/index>`
     - 提供身份验证支持。
  *  - :doc:`跨域资源共享（CORS） </security/cors>`
     - 配置跨域资源共享。CORS 全称为 Cross-Origin Resource Sharing。
  *  - :doc:`诊断（Diagnostics） <diagnostics>`
     - 包含对错误页面和运行时信息的支持。
  *  - :doc:`路由（Routing） <routing>`
     - 定义和约定请求路由。
  *  - :ref:`会话（Session） <session>`
     - 提供对管理用户会话（session）的支持。
  *  - :doc:`静态文件 <static-files>`
     - 提供对静态文件服务于目录浏览的支持。

.. _middleware-writing-middleware:

Writing middleware
------------------

编写中间件
------------------

The `CodeLabs middleware tutorial <https://github.com/Microsoft-Build-2016/CodeLabs-WebDev/tree/master/Module2-AspNetCore>`__ provides a good introduction to writing middleware.

`CodeLabs 中间件教程 <https://github.com/Microsoft-Build-2016/CodeLabs-WebDev/tree/master/Module2-AspNetCore>`__ 提供了清晰明确地介绍了如何编写中间件。

For more complex request handling functionality, the ASP.NET team recommends implementing the middleware in its own class, and exposing an ``IApplicationBuilder`` extension method that can be called from the ``Configure`` method. The simple logging middleware shown in the previous example can be converted into a middleware class that takes in the next ``RequestDelegate`` in its constructor and supports an ``Invoke`` method as shown:

对于更复杂的请求处理功能，ASP.NET 团队推荐在他们自己的类中实现中间件，并通过 ``IApplicationBuilder`` 扩展方法暴露，这样就能通过 ``Configure`` 方法来调用。之前演示的简易日志中间件就能被转换为一个中间件类（middleware class）：只要在其构造函数中获得下一个 ``RequestDelegate`` 并提供一个 ``Invoke`` 方法，如下所示：

.. literalinclude:: middleware/sample/src/MiddlewareSample/RequestLoggerMiddleware.cs
	:language: c#
	:caption: RequestLoggerMiddleware.cs
	:emphasize-lines: 13, 19

The middleware follows the `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle/>`_ and exposes all of its dependencies in its constructor. Middleware can take advantage of the `UseMiddleware<T>`_ extension to inject services directly into their constructors, as shown in the example below. Dependency injected services are automatically filled, and the extension takes a ``params`` array of arguments to be used for non-injected parameters.

中间件遵循 `显式依赖原则 <http://deviq.com/explicit-dependencies-principle/>`_ 并在其构造函数中暴露所有依赖项。中间件能够利用到 `UseMiddleware<T>`_ 扩展方法的优势，直接通过他们的构造函数注入服务，就像下面的例子所示。扩展所用到的 ``params`` 参数数组可用于非注入参数，依赖注入服务是自动完成填充的。

.. literalinclude:: middleware/sample/src/MiddlewareSample/RequestLoggerExtensions.cs
	:language: c#
	:caption: RequestLoggerExtensions.cs
	:lines: 5-11
	:emphasize-lines: 5
	:dedent: 4

Using the extension method and associated middleware class, the ``Configure`` method becomes very simple and readable.

通过使用扩展方法和相关中间件类，``Configure`` 方法变得非常简洁和高可读性。

.. literalinclude:: middleware/sample/src/MiddlewareSample/Startup.cs
	:language: c#
	:lines: 51-62
	:emphasize-lines: 6
	:dedent: 8

Although ``RequestLoggerMiddleware`` requires an ``ILoggerFactory`` parameter in its constructor, neither the ``Startup`` class nor the ``UseRequestLogger`` extension method need to explicitly supply it. Instead, it is automatically provided through dependency injection performed within ``UseMiddleware<T>``.

尽管 ``RequestLoggerMiddleware`` 在其构造函数中需要 ``ILoggerFactory`` 参数，但无论是 ``Startup`` 类还是 ``UseRequestLogger`` 扩展方法都不需要显式依赖之。相反，它将自动地通过内置的 ``UseMiddleware<T>`` 来执行依赖注入以提供之。

Testing the middleware (by setting the ``Hosting:Environment`` environment variable to ``LogMiddleware``) should result in output like the following (when using WebListener):

测试中间件（通过给 ``LogMiddleware`` 设置 ``Hosting:Environment`` 环境变量）会输出下图的结果（当时用了 WebListener 时）：

.. image:: middleware/_static/console-logmiddleware.png

.. note:: The `UseStaticFiles <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html#meth-Microsoft.AspNet.Builder.StaticFileExtensions.UseStaticFiles>`_ extension method (which creates the `StaticFileMiddleware <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/StaticFiles/StaticFileMiddleware/index.html>`_) also uses ``UseMiddleware<T>``. In this case, the ``StaticFileOptions`` parameter is passed in, but other constructor parameters are supplied by ``UseMiddleware<T>`` and dependency injection.

.. note:: `UseStaticFiles <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/StaticFileExtensions/index.html#meth-Microsoft.AspNet.Builder.StaticFileExtensions.UseStaticFiles>`_ 扩展方法（该方法会创建 `StaticFileMiddleware <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/StaticFiles/StaticFileMiddleware/index.html>`_ ）同样也使用了 ``UseMiddleware<T>``。所以除了 ``StaticFileOptions``  参数被传入之外，构造函数的其他参数都由 ``UseMiddleware<T>`` 和依赖注入所提供。

Additional Resources
--------------------

扩展资源
--------------------

- `CodeLabs middleware tutorial <https://github.com/Microsoft-Build-2016/CodeLabs-WebDev/tree/master/Module2-AspNetCore>`__
- `CodeLabs 中间件教程 <https://github.com/Microsoft-Build-2016/CodeLabs-WebDev/tree/master/Module2-AspNetCore>`__
- `Sample code used in this doc <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/middleware/sample>`_
- `本文档使用 的样例文件 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/middleware/sample>`_
- :doc:`/migration/http-modules`
- :doc:`startup`
- :doc:`request-features`

.. _UseMiddleware<T>: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/UseMiddlewareExtensions/index.html#meth-Microsoft.AspNet.Builder.UseMiddlewareExtensions.UseMiddleware<TMiddleware>

.. _application-startup:

Application Startup
===================

应用程序启动
===================

作者： `Steve Smith`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

ASP.NET Core provides complete control of how individual requests are handled by your application. The ``Startup`` class is the entry point to the application, setting up configuration and wiring up services the application will use. Developers configure a request pipeline in the ``Startup`` class that is used to handle all requests made to the application.

ASP.NET Core 为你的应用程序提供了处理每个请求的完整控制。``Startup`` 类是应用程序的入口点（entry point），该类中可以设置配置（configuration）并且将应用程序将要使用的服务连接起来。开发人员可以在 ``Startup`` 类中配置请求管道，该请求管道将用于应用程序处理所有的请求。

.. contents:: Sections:
  :local:
  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

The Startup class
-----------------

Startup 类
-----------------

In ASP.NET Core, the ``Startup`` class provides the entry point for an application, and is required for all applications. It's possible to have environment-specific startup classes and methods (see :doc:`environments`), but regardless, one ``Startup`` class will serve as the entry point for the application. ASP.NET searches the primary assembly for a class named ``Startup`` (in any namespace). You can specify a different assembly to search using the `Hosting:Application` configuration key. It doesn't matter whether the class is defined as ``public``; ASP.NET will still load it if it conforms to the naming convention. If there are multiple ``Startup`` classes, this will not trigger an exception. ASP.NET will select one based on its namespace (matching the project's root namespace first, otherwise using the class in the alphabetically first namespace).

在 ASP.NET Core 中，``Startup`` 类提供了应用程序的入口点，而且在所有应用程序中都有 ``Startup`` 类。可能会有存在特定环境的启动类和方法（参见 :doc:`environments` ），但无论如何， ``Startup`` 类都将被充当为应用程序的启动点。ASP.NET 会在主程序集中搜索名为 ``Startup`` 的类（在任何命名空间下）。你可以指定一个其它程序集用于检索，只需要使用 `Hosting:Application` 配置键。ASP.NET 并不关心 ``Startup`` 类是不是定义为 ``public``，ASP.NET 将继续加载之（只要符合命名规范即可）。如果有多个 ``Startup`` 类，这并不会触发异常，ASP.NET 将基于命名空间选择其中一个（匹配项目的根命名空间优先，否则基于命名空间首字母排序后的第一个）。

The ``Startup`` class can optionally accept dependencies in its constructor that are provided through :doc:`dependency injection <dependency-injection>`.  Typically, the way an application will be configured is defined within its Startup class's constructor (see :doc:`configuration`). The Startup class must define a ``Configure`` method, and may optionally also define a ``ConfigureServices`` method, which will be called when the application is started.

``Startup`` 类能可选地在构造函数中接受依赖项（通过 :doc:`依赖注入 <dependency-injection>` 提供）。通常而言，应用程序的配置都是定义于 Startup 类的构造函数之中（参见 :doc:`configuration` ）。Startup 类必须定义 ``Configure`` 方法，可选择定义一个 ``ConfigureServices`` 方法，这些将在应用程序启动时被调用。

The Configure method
--------------------

Configure 方法
--------------------

The ``Configure`` method is used to specify how the ASP.NET application will respond to individual HTTP requests. At its simplest, you can configure every request to receive the same response. However, most real-world applications require more functionality than this. More complex sets of pipeline configuration can be encapsulated in :doc:`middleware <middleware>` and added using extension methods on IApplicationBuilder_.

``Configure`` 用于指定 ASP.NET 应用程序将如何响应每一个 HTTP 请求。简单来说，你可以配置每个请求都接收相同的响应。然而，大多数现实世界应用程序需要比这多得多的功能。更复杂的管道配置可以封装于 :doc:`中间件（middleware） <middleware>` 之中，并通过扩展方法添加到 IApplicationBuilder_ 上。

Your ``Configure`` method must accept an IApplicationBuilder_ parameter. Additional services, like ``IHostingEnvironment`` and ``ILoggerFactory`` may also be specified, in which case these services will be :doc:`injected <dependency-injection>` by the server if they are available. In the following example from the default web site template, you can see several extension methods are used to configure the pipeline with support for `BrowserLink <http://www.asp.net/visual-studio/overview/2013/using-browser-link>`_, error pages, static files, ASP.NET MVC, and Identity.

``Configure`` 方法必须接受一个 IApplicationBuilder_ 参数。一些额外服务（比如 ``IHostingEnvironment`` 或 ``ILoggerFactory``）也可以被指定，这些服务（如果它们可用）将会被服务器 :doc:`注入 <dependency-injection>` 进来。在下例（源于默认的 Web 站点模板）中可见多个扩展方法被用于配置管道以支持 `BrowserLink <http://www.asp.net/visual-studio/overview/2013/using-browser-link>`_ 、错误页、静态文件、ASP.NET MVC 以及 Identity。

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :linenos:
  :lines: 58-86
  :dedent: 8
  :emphasize-lines: 8-10,14,17,19,23

Each ``Use`` extension method adds :doc:`middleware <middleware>` to the request pipeline. For instance, the ``UseMvc`` extension method adds the :doc:`routing <routing>` middleware to the request pipeline and configures :doc:`MVC </mvc/index>` as the default handler.

每个 ``Use`` 扩展方法都会把一个 :doc:`中间件 <middleware>` 加入请求管道。比如 ``UseMvc`` 扩展方法会把 :doc:`路由中间件 <routing>` 加进请求管道，并把 :doc:`MVC </mvc/index>` 配置为默认的处理器。

You can learn all about middleware and using IApplicationBuilder_ to define your request pipeline in the :doc:`middleware` topic.

在 :doc:`middleware` 一章中，你可以了解到更多有关中间件的信息，并使用 IApplicationBuilder_ 定义请求管道。

The ConfigureServices method
----------------------------

ConfigureServices 方法
----------------------------

Your ``Startup`` class can optionally include a ``ConfigureServices`` method for configuring services that are used by your application. The ``ConfigureServices`` method is a public method on your ``Startup`` class that takes an IServiceCollection_ instance as a parameter and optionally returns an ``IServiceProvider``. The ``ConfigureServices`` method is called before ``Configure``. This is important, because some features like ASP.NET MVC require certain services to be added in ``ConfigureServices`` before they can be wired up to the request pipeline.

你的 ``Startup`` 类能可选地包含一个 ``ConfigureServices`` 方法用来配置用于应用程序内的服务。``ConfigureServices`` 方法是 ``Startup`` 类中的公开方法，通过参数获取一个 IServiceCollection_ 实例并可选地返回 ``IServiceProvider``。``ConfigureServices`` 需要在 ``Configure`` 之前被调用。这一点非常重要，这是因为像 ASP.NET MVC 中的某些功能，需要从 ``ConfigureServices`` 中请求某些服务，而这些服务需要在接入请求管道之前先被加入 ``ConfigureServices`` 中。

Just as with ``Configure``, it is recommended that features that require substantial setup within ``ConfigureServices`` be wrapped up in extension methods on IServiceCollection_. You can see in this example from the default web site template that several ``Add[Something]`` extension methods are used to configure the app to use services from Entity Framework, Identity, and MVC:

正如通过 ``Configure``，推荐在 IServiceCollection_ 上使用扩展方法来包装含有大量配置细节的 ``ConfigureServices``。你可在本例（使用了默认的 Web 站点模板）中看到几个 ``Add[Something]`` 扩展方法被用于设置应用程序，以便能够使用 Entity Framework、Identity 和 MVC：

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :linenos:
  :lines: 40-55
  :dedent: 8
  :emphasize-lines: 4,7,11

Adding services to the services container makes them available within your application via :doc:`dependency injection <dependency-injection>`. Just as the ``Startup`` class is able to specify dependencies its methods require as parameters, rather than hard-coding to a specific implementation, so too can your middleware, MVC controllers and other classes in your application.

通过 :doc:`依赖注入（dependency injection） <dependency-injection>` 可将服务加入服务容器，使其在应用程序中可用。正如 ``Startup`` 类能将指定的依赖项作为其方法参数——而不是硬编码（hard-coding）来实例化特定实现——对于中间件、MVC 控制器以及应用程序中的其它类来说都可以做到这一点。

The ``ConfigureServices`` method is also where you should add configuration option classes, like ``AppSettings`` in the example above, that you would like to have available in your application. See the :doc:`configuration` topic to learn more about configuring options.

``ConfigureServices`` 方法同样是可以增加配置选项类的地方（如上例中的 ``AppSettings``），只要你想让它在你的应用程序中生效。更多有关配置选项的信息请阅读 :doc:`configuration` 。

Services Available in Startup
-----------------------------

在启动时服务可用 
-----------------------------

ASP.NET Core provides certain application services and objects during your application's startup. You can request certain sets of these services by simply including the appropriate interface as a parameter on your ``Startup`` class's constructor or one of its ``Configure`` or ``ConfigureServices`` methods. The services available to each method in the ``Startup`` class are described below. The framework services and objects include:

ASP.NET Core 在应用程序启动期间提供了一些应用服务和对象。你可以非常简单地使用这些服务，只需要在在 ``Startup`` 类的构造函数或是它的 ``Configure`` 与 ``ConfigureServices`` 方法（的其中一个）包含合适的接口即可。下面定义了在 ``Startup`` 类中对每个方法可用的服务。框架服务和对象包括：

IApplicationBuilder
  Used to build the application request pipeline. Available only to the ``Configure`` method in ``Startup``. Learn more about :doc:`request-features`.
  被用于构建应用程序的请求管道。只可以在 ``Startup`` 中的 ``Configure`` 方法里使用。更多请阅读 :doc:`request-features` 。

IApplicationEnvironment
  Provides access to the application properties, such as ``ApplicationName``, ``ApplicationVersion``, and ``ApplicationBasePath``. Available to the ``Startup`` constructor and ``Configure`` method.
  提供了访问应用程序属性，类似于 ``ApplicationName``、``ApplicationVersion`` 以及 ``ApplicationBasePath``。可以在 ``Startup`` 的构造函数和 ``Configure`` 方法中使用。

IHostingEnvironment
  Provides the current ``EnvironmentName``, ``WebRootPath``, and web root file provider. Available to the ``Startup`` constructor and ``Configure`` method.
  提供了当前的 ``EnvironmentName``、``WebRootPath`` 以及 Web 根文件提供者。可以在 ``Startup`` 的构造函数和 ``Configure`` 方法中使用。

ILoggerFactory
  Provides a mechanism for creating loggers. Available to the ``Startup`` constructor and ``Configure`` method. Learn more about :doc:`logging`.
  提供了创建日志器的机制。可以在 ``Startup``  的构造函数或 ``Configure``  方法中使用。更多请阅读 :doc:`logging` 。

IServiceCollection
  The current set of services configured in the container. Available only to the ``ConfigureServices`` method, and used by that method to configure the services available to an application.
  当前容器中各服务的配置集合。只可在 ``ConfigureServices`` 方法中被使用，通过在该方法中配置可使服务在应用程序中可用。

Looking at each method in the ``Startup`` class in the order in which they are called, the following services may be requested as parameters:

看看 ``Startup`` 类中的每一个方法，排序按照调用它们的顺序，下面的服务或可被作为参数：

Startup Constructor
- ``IApplicationEnvironment``
- ``IHostingEnvironment``
- ``ILoggerFactory``

ConfigureServices
- ``IServiceCollection``

Configure
- ``IApplicationBuilder``
- ``IApplicationEnvironment``
- ``IHostingEnvironment``
- ``ILoggerFactory``

.. note:: Although ``ILoggerFactory`` is available in the constructor, it is typically configured in the ``Configure`` method. Learn more about :doc:`logging`.

.. note:: 尽管 ``ILoggerFactory`` 在构造函数中可用，但它通常在 ``Configure`` 方法中配置。具体可阅读 :doc:`logging` 。

Additional Resources
--------------------

扩展阅读
--------------------

- :doc:`environments`
- :doc:`middleware`
- :doc:`owin`

.. _IApplicationBuilder: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html
.. _IServiceCollection: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/DependencyInjection/IServiceCollection/index.html

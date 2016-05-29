Introduction to ASP.NET Core
ASP.NET Core介绍
============================
============================

By `Daniel Roth`_
作者：Daniel Roth` 翻译：王健

ASP.NET Core is a significant redesign of ASP.NET. This topic introduces the new concepts in ASP.NET Core and explains how they help you develop modern web apps.
ASP.NET CORE 是ASP.NET的一个重要的重新设计。这个主题将介绍ASP.NET Core 的新概念，并讲述它如何帮助你开发现代化的web应用。

.. contents:: Sections:
  :local:
  :depth: 1
  
  .. contents:: 节:
  :local:
  :depth: 1
  
What is ASP.NET Core?
什么是ASP.NET Core?
---------------------
---------------------

ASP.NET Core is a new open-source and cross-platform framework for building modern cloud-based Web applications using .NET. We built it from the ground up to provide an optimized development framework for apps that are either deployed to the cloud or run on-premises. It consists of modular components with minimal overhead, so you retain flexibility while constructing your solutions. You can develop and run your ASP.NET Core applications cross-platform on Windows, Mac and Linux. ASP.NET Core is fully open source on `GitHub <https://github.com/aspnet/home>`_.
ASP.NET Core 是使用.NET来构建基于云的现代化网络应用的一种新开源和跨平台的框架。我们重新设计它来为部署在云上或运行在本地的应用程序提供一种优化过的开发框架。它由最小开销的模块化组件组成，让您构建解决方案的同时保持灵活性。您可以开发并跨平台运行 ASP.NET Core 应用程序在Windows，Mac 和 Linux 上。ASP.NET Core 完全开源于`GitHub` <https://github.com/aspnet/home>.

Why build ASP.NET Core?
为什么开发 ASP.NET Core?
-----------------------
-----------------------

The first preview release of ASP.NET came out almost 15 years ago as part of the .NET Framework.  Since then millions of developers have used it to build and run great web applications, and over the years we have added and evolved many, many capabilities to it.
ASP.NET 的第一个预览版作为.NET Framework 的一部分，出来了差不多15年。从那时起，数以百万计的开发人员使用它来构建和运行伟大的Web应用程序，多年来我们已经为它加入并发展了很多很多的功能。

With ASP.NET Core we are making a number of architectural changes that make the core web framework much leaner and more modular. ASP.NET Core is no longer based on System.Web.dll, but is instead based on a set of granular and well factored NuGet packages allowing you to optimize your app to have just what you need. You can reduce the surface area of your application to improve security, reduce your servicing burden and also to improve performance in a true pay-for-what-you-use model.
使用ASP.NET Core，我们带来了一系列的变化，使核心网络架构更为精简和更具模块化。ASP.NET Core 不再基于 System.Web.dll，而是基于一组细化和良好分解的NuGet包让您可以优化您的应用程序并且只取您所需要的部分。您可以减少应用程序的外部区域，以提高安全性，降低您的维护负担，也在真正的为你所付费使用的部分提高了性能。

ASP.NET Core is built with the needs of modern Web applications in mind, including a unified story for building Web UI and Web APIs that integrate with today's modern client-side frameworks and development workflows. ASP.NET Core is also built to be cloud-ready by introducing environment-based configuration and by providing built-in dependency injection support.
ASP.NET Core 是在现代化的web应用程序的需求下构建的，其中包括构建与今天的现代化客户端框架和开发工作流程集成的统一规范的Web用户界面和Web API（的需求）。ASP.NET Core通过引入基于环境的配置和提供内置的依赖注入实现了cloud-ready。

To appeal to a broader audience of developers, ASP.NET Core supports cross-platform development on Windows, Mac and Linux. The entire ASP.NET Core stack is open source and encourages community contributions and engagement. ASP.NET Core comes with a new, agile project system in Visual Studio while also providing a complete command-line interface so that you can develop using the tools of your choice.
为了吸引更广泛的开发者受众，ASP.NET Core 支持 Windows，Mac和Linux 的跨平台开发。整个 ASP.NET Core 协议栈是开源的，并鼓励社区的贡献和参与。ASP.NET Core 在 Visual Studio 中配备了一个新的，灵活的项目系统，同时还提供了完整的命令行界面，使您可以使用您选择的工具进行开发。

In summary, with ASP.NET Core you gain the following foundational improvements:

- New light-weight and modular HTTP request pipeline
- Ability to host on IIS or self-host in your own process
- Built on `.NET Core`_, which supports true side-by-side app versioning
- Ships entirely as NuGet packages
- Integrated support for creating and using NuGet packages
- Single aligned web stack for Web UI and Web APIs
- Cloud-ready environment-based configuration
- Built-in support for dependency injection
- New tooling that simplifies modern web development
- Build and run cross-platform ASP.NET apps on Windows, Mac and Linux
- Open source and community focused

综上所述，通过ASP.NET Core您将获得以下基本改进：

 - 新的轻量级，模块化的HTTP请求管道
 - 能够托管在IIS或自托管在自己的过程
 - 构建在`.NET Core`的app支持真正的版本并行
 - 完全通过NuGet包进行装载
 - 对创建和使用的NuGet包提供完整支持
 - 对Web UI和Web API的单对准网络堆栈
 - Cloud-ready基于环境的配置
 - 内置支持依赖注入
 - 新的工具，简化了现代Web开发
 - 构建并运行在Windows，Mac和Linux的跨平台ASP.NET应用
 - 开源和关注社区






Application anatomy
应用程序剖析
-------------------
-------------------

ASP.NET Core applications are defined using a public ``Startup`` class:
ASP.NET Core 应用程序定义了一个public的Startup类：

.. code-block:: c#

  public class Startup
  {
      public void ConfigureServices(IServiceCollection services)
      {
      }

      public void Configure(IApplicationBuilder app)
      {
      }
  }

.. code-block:: c#

  public class Startup
  {
      public void ConfigureServices(IServiceCollection services)
      {
      }

      public void Configure(IApplicationBuilder app)
      {
      }
  }

The ``ConfigureServices`` method defines the services used by your application and the ``Configure`` method is used to define what middleware makes up your request pipeline. See :doc:`/fundamentals/startup` for more details.
``ConfigureServices``方法定义了应用程序上使用的服务，``Configure``方法用于定义由哪些中间件来装配您的请求管线。请参阅：doc:`/fundamentals/startup`了解更多详情

Services
服务
--------
--------

A service is a component that is intended for common consumption in an application. Services are made available through dependency injection. ASP.NET Core includes a simple built-in inversion of control (IoC) container that supports constructor injection by default, but can be easily replaced with your IoC container of choice. See :doc:`/fundamentals/dependency-injection` for more details.

服务是用于在应用程序共同消费的组件。服务是通过依赖注入提供。 ASP.NET Core 包括简单的内置控制反转容器，默认情况下支持构造函数注入，但是可以轻松替换为您自己选择的IOC容器。请参阅：DOC：`/基础/依赖性的injection`了解更多详情。

Services in ASP.NET Core come in three varieties: singleton, scoped and transient. Transient services are created each time they’re requested from the container. Scoped services are created only if they don’t already exist in the current scope. For Web applications, a container scope is created for each request, so you can think of scoped services as per request. Singleton services are only ever created once.

ASP.NET Core服务有三种类型：单例，范围和瞬态的。瞬态服务是每一次从容器中请求时创建的。范围服务，只有当他们还没有在当前范围存在的时候创建。对于Web应用程序，为每个请求创建一个容器范围，所以你可以把（每一个）范围服务当作每一次请求，依据客户需求。单例服务仅仅只创建一次。

Middleware
中间件
----------
----------

In ASP.NET Core you compose your request pipeline using :doc:`/fundamentals/middleware`. ASP.NET Core middleware perform asynchronous logic on an ``HttpContext`` and then optionally  invoke the next middleware in the sequence or terminate the request directly. You generally "Use" middleware by invoking a corresponding extension method on the ``IApplicationBuilder`` in your ``Configure`` method.

在ASP.NET Core您使用中间件：DOC：`/基础/ middleware`来组装您的请求管道。 ASP.NET Core中间件在``HttpContext``中执行异步逻辑，然后任选调用序列中的下一个中间件或直接终止该请求。通常您在您的``IApplicationBuilder``中的``Configure``方法里调用相应的扩展方法来“使用”中间件。

ASP.NET Core comes with a rich set of prebuilt middleware:
ASP.NET Core 配备了丰富的预先构建的中间件：

- :doc:`/fundamentals/static-files`
- :doc:`/fundamentals/routing`
- :doc:`/fundamentals/diagnostics`
- :doc:`/security/authentication/index`

- :doc:`/fundamentals/static-files`
- :doc:`/fundamentals/routing`
- :doc:`/fundamentals/diagnostics`
- :doc:`/security/authentication/index`

You can also author your own :doc:`custom middleware </fundamentals/middleware>`.
您也可以开发您自己的中间件。:doc:`custom middleware </fundamentals/middleware>`.

You can use any `OWIN <http://owin.org>`_-based middleware with ASP.NET Core. See :doc:`/fundamentals/owin` for details.
您可以在 ASP.NET Core上使用任何基于`OWIN <http://owin.org>`的中间件。

Servers
服务器
-------
-------

The ASP.NET Core hosting model does not directly listen for requests, but instead relies on an HTTP :doc:`server </fundamentals/servers>` implementation to surface the request to the application as a set of feature interfaces that can be composed into an HttpContext. ASP.NET Core includes a managed cross-platform web server, called :ref:`Kestrel <kestrel>`, that you would typically run behind a production web server like `IIS <https://iis.net>`__ or `nginx <http://nginx.org>`__.

ASP.NET的核心托管模型不直接侦听请求，而是依赖于一种HTTP服务器:doc:`server </fundamentals/servers>`实现使对应用程序的请求暴露为一套能被组装进HttpContext的功能接口。 ASP.NET Core 包括托管的跨平台Web服务器，叫做：ref:`Kestrel <kestrel>`，你通常会在生产web服务器如`IIS<https://iis.net>`__或` nginx<http://nginx.org>`背后运行它。

Web root
Web根节点
--------
--------

The Web root of your application is the root location in your project from which HTTP requests are handled (ex. handling of static file requests). The Web root of an ASP.NET Core application is configured using the "webroot" property in your project.json file.

您的应用程序根节点是您项目中处理HTTP请求的根路径（例如处理静态文件的请求）。一个ASP.NET Core应用程序的Web根目录用在您的project.json文件中的“webroot”的属性进行配置。


Configuration
配置
-------------
-------------

ASP.NET Core uses a new configuration model for handling of simple name-value pairs that is not based on System.Configuration or web.config. This new configuration model pulls from an ordered set of configuration providers. The built-in configuration providers support a variety of file formats (XML, JSON, INI) and also environment variables to enable environment-based configuration. You can also write your own custom configuration providers. Environments, like Development and Production, are a first-class notion in ASP.NET Core and can also be set up using environment variables:

ASP.NET Core 使用了一种新的处理简单键值对的配置模型，它并非基于System.Configuration或web.config。这种新的配置模型从配置提供程序的有序集合中获取配置数据。这种内置的配置提供程序支持多种文件格式（XML，JSON，INI），也提供环境变量用于启用基于环境的配置。您也可以编写自己的自定义配置提供程序。环境，如研发和生产，是ASP.NET的核心第一级概念，并且，也可以使用环境变量设置：

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 22-34
  :dedent: 12

See :doc:`/fundamentals/configuration` for more details on the new configuration system and :doc:`/fundamentals/environments` for details on how to work with environments in ASP.NET Core.

请参阅 doc:`/fundamentals/configuration`来获取更多关于新的配置系统的信息，参阅doc:`/fundamentals/environments`来获取更多关于在ASP.NET Core 的环境的信息。


Client-side development
客户端开发
-----------------------
-----------------------

ASP.NET Core is designed to integrate seamlessly with a variety of client-side frameworks, including :doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` and :doc:`Bootstrap </client-side/bootstrap>`. See :doc:`/client-side/index` for more details.
ASP.NET Core被设计成与多种客户端框架无缝集成，包括:doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` and :doc:`Bootstrap </client-side/bootstrap>`。请参阅 :doc:`/client-side/index`获取更多详情。

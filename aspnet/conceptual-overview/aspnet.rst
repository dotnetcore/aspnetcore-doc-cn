ASP.NET Core介绍
============================

作者： `Daniel Roth`_
翻译：王健

ASP.NET CORE 是ASP.NET的一个重要的重新设计。这个主题将介绍ASP.NET Core 的新概念，并讲述它如何帮助你开发现代化的web应用。

.. contents:: Sections:
  :local:
  :depth: 1
  什么是ASP.NET Core?
  为什么开发 ASP.NET Core?
  应用程序剖析
  服务
  中间件
  服务器
  配置
  客户端开发
  
什么是ASP.NET Core?
---------------------

ASP.NET Core 是使用.NET来构建基于云的现代化网络应用的一种新开源和跨平台的框架。我们重新设计它来为部署在云上或运行在本地的应用程序提供一种优化过的开发框架。它由最小开销的模块化组件组成，让您构建解决方案的同时保持灵活性。您可以开发并跨平台运行 ASP.NET Core 应用程序在Windows，Mac 和 Linux 上。ASP.NET Core 完全开源于`GitHub` <https://github.com/aspnet/home>.

为什么开发 ASP.NET Core?
-----------------------

ASP.NET 的第一个预览版作为.NET Framework 的一部分，出来了差不多15年。从那时起，数以百万计的开发人员使用它来构建和运行伟大的Web应用程序，多年来我们已经为它加入并发展了很多很多的功能。

使用ASP.NET Core，我们带来了一系列的变化，使核心网络架构更为精简和更具模块化。ASP.NET Core 不再基于 System.Web.dll，而是基于一组细化和良好分解的NuGet包让您可以优化您的应用程序并且只取您所需要的部分。您可以减少应用程序的外部区域，以提高安全性，降低您的维护负担，也在真正的为你所付费使用的部分提高了性能。

ASP.NET Core 是在现代化的web应用程序的需求下构建的，其中包括构建与今天的现代化客户端框架和开发工作流程集成的统一规范的Web用户界面和Web API（的需求）。ASP.NET Core通过引入基于环境的配置和提供内置的依赖注入实现了cloud-ready。

为了吸引更广泛的开发者受众，ASP.NET Core 支持 Windows，Mac和Linux 的跨平台开发。整个 ASP.NET Core 协议栈是开源的，并鼓励社区的贡献和参与。ASP.NET Core 在 Visual Studio 中配备了一个新的，灵活的项目系统，同时还提供了完整的命令行界面，使您可以使用您选择的工具进行开发。

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


应用程序剖析
-------------------

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

``ConfigureServices``方法定义了应用程序上使用的服务，``Configure``方法用于定义由哪些中间件来装配您的请求管线。请参阅：doc:`/fundamentals/startup`了解更多详情

服务
--------

服务是用于在应用程序共同消费的组件。服务是通过依赖注入提供。 ASP.NET Core 包括简单的内置控制反转容器，默认情况下支持构造函数注入，但是可以轻松替换为您自己选择的IOC容器。请参阅：DOC：`/基础/依赖性的injection`了解更多详情。

ASP.NET Core服务有三种类型：单例，范围和瞬态的。瞬态服务是每一次从容器中请求时创建的。范围服务，只有当他们还没有在当前范围存在的时候创建。对于Web应用程序，为每个请求创建一个容器范围，所以你可以把（每一个）范围服务当作每一次请求，依据客户需求。单例服务仅仅只创建一次。

中间件
----------

在ASP.NET Core您使用中间件：DOC：`/基础/ middleware`来组装您的请求管道。 ASP.NET Core中间件在``HttpContext``中执行异步逻辑，然后任选调用序列中的下一个中间件或直接终止该请求。通常您在您的``IApplicationBuilder``中的``Configure``方法里调用相应的扩展方法来“使用”中间件。

ASP.NET Core 配备了丰富的预先构建的中间件：

- :doc:`/fundamentals/static-files`
- :doc:`/fundamentals/routing`
- :doc:`/fundamentals/diagnostics`
- :doc:`/security/authentication/index`


您也可以开发您自己的中间件。:doc:`custom middleware </fundamentals/middleware>`.

您可以在 ASP.NET Core上使用任何基于`OWIN <http://owin.org>`的中间件。

服务器
-------

ASP.NET的核心托管模型不直接侦听请求，而是依赖于一种HTTP服务器:doc:`server </fundamentals/servers>`实现使对应用程序的请求暴露为一套能被组装进HttpContext的功能接口。 ASP.NET Core 包括托管的跨平台Web服务器，叫做：ref:`Kestrel <kestrel>`，你通常会在生产web服务器如`IIS<https://iis.net>`__或` nginx<http://nginx.org>`背后运行它。

Web根节点
--------

您的应用程序根节点是您项目中处理HTTP请求的根路径（例如处理静态文件的请求）。一个ASP.NET Core应用程序的Web根目录用在您的project.json文件中的“webroot”的属性进行配置。

配置
-------------

ASP.NET Core 使用了一种新的处理简单键值对的配置模型，它并非基于System.Configuration或web.config。这种新的配置模型从配置提供程序的有序集合中获取配置数据。这种内置的配置提供程序支持多种文件格式（XML，JSON，INI），也提供环境变量用于启用基于环境的配置。您也可以编写自己的自定义配置提供程序。环境，如研发和生产，是ASP.NET的核心第一级概念，并且，也可以使用环境变量设置：

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 22-34
  :dedent: 12

请参阅 doc:`/fundamentals/configuration`来获取更多关于新的配置系统的信息，参阅doc:`/fundamentals/environments`来获取更多关于在ASP.NET Core 的环境的信息。


客户端开发
-----------------------

ASP.NET Core被设计成与多种客户端框架无缝集成，包括:doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` and :doc:`Bootstrap </client-side/bootstrap>`。请参阅 :doc:`/client-side/index`获取更多详情。

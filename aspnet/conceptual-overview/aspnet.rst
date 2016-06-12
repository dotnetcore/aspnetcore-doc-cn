ASP.NET Core 简介
============================

作者： `Daniel Roth`_、`Rick Anderson`_ 和 `Shaun Luttin <https://twitter.com/dicshaunary>`_

翻译：`王健 <https://github.com/wjhgzx>`_

ASP.NET Core 是ASP.NET的一个重要的重新设计。这个主题将介绍ASP.NET Core 的新概念，并讲述它如何帮助你开发现代化的web应用。

.. contents:: Sections：
  :local:
  :depth: 1

什么是ASP.NET Core?
---------------------

ASP.NET Core是一个新的开源、跨平台的框架被用来构建现代基于云的网络连接的应用程序，比如web应用、物联网应用和移动后端。ASP.NET Core应用程序可以运行在.NET Core或完整的.NET Framework框架上。这个框架被优化过可以支持开发的程序部署在云上或本地运行。它由最小开销的模块化组件组成，让您构建解决方案的同时保持灵活性。您可以开发ASP.NET Core 应用程序，并可以跨平台运行在Windows、Mac 和 Linux 上。ASP.NET Core 的开源代码在 `GitHub <https://github.com/aspnet/home>`_.

为什么开发 ASP.NET Core?
-----------------------

ASP.NET 的第一个预览版作为.NET Framework 的一部分，出来了差不多15年。从那时起，数以百万计的开发人员使用它来构建和运行伟大的Web应用程序，多年来我们已经为它加入并发展了很多很多的功能。

ASP.NET Core为我们带来了一系列的变化，使核心网络架构更为精简和更具模块化。ASP.NET Core 不再基于 *System.Web.dll* ，而是基于一组细化和良好分解的 `NuGet <http://www.nuget.org/>`__ 包，让您可以优化您的应用程序并且只取您所需要的部分。您可以减少应用程序的外部区域，以提高安全性，降低您的维护负担，也在真正的为您所付费使用的部分提高了性能。

通过ASP.NET Core您将获得以下基本改进：

 - 统一的方式构建web UI 和 web API
 - 集成了现代化的 :doc:`客户端类库 </client-side/index>` 和开发工作流
 - 基于环境的cloud-ready的 :doc:`配置系统 </fundamentals/configuration>`
 - 内置支持依赖注入
 - 新的轻量级，模块化的HTTP请求管道
 - 能够托管在IIS或自托管在自己的过程
 - 基于 `.NET Core`_ ，支持真正的APP版本并行
 - 完全通过 `NuGet`_ 包进行加载
 - 新的工具，简化了现代Web开发
 - 生成并运行在Windows、Mac和Linux的跨平台ASP.NET应用程序
 - 开源和关注社区

应用程序剖析
-------------------

.. comment  在RC1中，WebHostBuilder的工作隐藏在dnx.executable中

ASP.NET Core 应用程序是一个简单控制台应用程序，它在 Main 方法中创建了一个web server。

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#

``Main`` 采用建造者模式使用 `WebHostBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/WebHostBuilder/index.html>`__ 来创建web 应用程序宿主。这个构造器提供定义web server (例如 ``UseKestrel``)和 startup ( ``UseStartup``) 类的方法。在上边示例中，使用了 Kestrel web 服务器，但是也可以使用其他的 web 服务器。我们将在下一个章节中展示更多关于 ``UseStartup`` 的细节。``WebHostBuilder`` 提供很多可选的方法，包括使用 ``UseIISIntegration`` 方法 在 IIS 和 IIS Express 寄宿，通过 ``UseContentRoot`` 方法来指定内容目录根节点。``Build`` 和 ``Run`` 方法生成用于寄宿应用程序的 ``IWebHost``，并开始侦听传入的 HTTP 请求。

Startup
---------------------------

 ``WebHostBuilder`` 中 ``UseStartup`` 方法指定了您应用程序中的 ``Startup`` 类：

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#
    :lines: 6-17
    :dedent: 4
    :emphasize-lines: 7

``Startup`` 类是定义请求处理管道的地方，并能够配置应用程序所需要的服务。 ``Startup`` 类必须是公开的并包含以下方法：

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


- ``ConfigureServices`` 方法定义了应用程序上使用的服务(见下边的 Services_ ) （例如 ASP.NET MVC Core framework, Entity Framework Core, Identity 等）
- ``Configure`` 方法定义了请求管线中的 :doc:`中间件 </fundamentals/middleware>`

- 查看详细内容 :doc:`/fundamentals/startup` 

服务
--------

服务是在应用程序共同使用的组件。应用程序通过依赖注入来提供服务。 ASP.NET Core 包括简单的内置控制反转（Ioc）容器，默认情况下支持构造函数注入，但是可以轻松替换为您自己选择的IOC容器。除了松耦合的好处之外，DI使服务在您的应用程序中处处可用。例如，:doc:`日志 </fundamentals/logging>` 在您的应用程序中处处可用。查看更多信息 :doc:`/fundamentals/dependency-injection`。


中间件
----------

在ASP.NET Core您使用中间件 :doc:`/fundamentals/middleware` 来组装您的请求管道。 ASP.NET Core中间件在 ``HttpContext`` 中执行异步逻辑，然后调用序列中的下一个中间件或直接终止该请求。通常您在 ``IApplicationBuilder`` 中的 ``Configure`` 方法里调用相应的 ``UseXYZ`` 扩展方法来“使用”中间件。

ASP.NET Core 配备了丰富的预先构建的中间件：

- :doc:`Static files </fundamentals/static-files>`
- :doc:`/fundamentals/routing`
- :doc:`/fundamentals/diagnostics`
- :doc:`/security/authentication/index`

您也可以命名您自己的中间件。 :doc:`custom middleware </fundamentals/middleware>`。

您可以在 ASP.NET Core上使用任何基于 `OWIN <http://owin.org>`_ 的中间件。 查看细节 :doc:`/fundamentals/owin` 。

服务器
-------

ASP.NET的核心托管模型不直接侦听请求，而是依赖于HTTP :doc: `服务器 </fundamentals/servers>` 实现，将请求转发给应用程序。转发的请求被包装为一组特征的接口，应用程序将其组装进 ``HttpContext`` 。 ASP.NET Core 包括托管的跨平台Web服务器，叫做 :ref: `Kestrel <kestrel>`，你通常会在生产web服务器如 `IIS<https://iis.net>`__或 `nginx <http://nginx.org>`__.

内容根节点
------------

内容根节点是应用程序使用任何内容的根路径。 默认情况下，内容根节点与可执行程序寄宿应用程序的应用程序根路径相同。其他位置可以用 `WebHostBuilder` 指定。

Web 根节点
--------

您应用程序 web 根节点是项目中处理HTTP请求的根路径（例如处理静态文件的请求）。当使用静态文件中间件，只有 web 根文件夹中的文件可以被访问；其他在内容根节点的文件 **不能** 被远程访问。默认的web 根路径是 `<content root>/wwwroot`，您可以用 `WebHostBuilder` 来指定一个不同的路径。 

配置
-------------

ASP.NET Core 使用了一种新的处理简单键值对的配置模型，它并非基于 ``System.Configuration`` 或 *web.config* 。这种新的配置模型从配置提供程序的有序集合中获取配置数据。这种内置的配置提供程序支持多种文件格式（XML，JSON，INI），也提供环境变量用于启用基于环境的配置。您也可以编写自己的自定义配置提供程序。

查看更多信息 :doc:`/fundamentals/configuration`。

环境
------

环境，如研发和生产环境，是ASP.NET Core 头等概念，可以使用环境变量设置：
查看更多信息 :doc:`/fundamentals/environments` 。

使用 ASP.NET Core MVC 来构建 web UI 和 web API
------------------------------------------------

- 您可以使用模型-视图-控制器（MVC）模式来创建分解良好和可测试的web应用程序。See :doc:`/mvc/index` and :doc:`/testing/index`.

- 您可以创建HTTP服务，支持多种格式，并且对内容协商提供完整支持。See :doc:`/mvc/models/formatting`

- `Razor <http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c>`__ 提供一种 多产的语言来创建 :doc:`视图 </mvc/views/index>`

- :doc:`Tag Helpers </mvc/views/tag-helpers/intro>` 使服务器端代码能够在 Razor 文件中参与创建和渲染HTML元素

- 您可以通过自定义或者内置的格式化工具（JSON, XML）来创建提供完整支持的内容协商 HTTP服务。

- :doc:`/mvc/models/model-binding` 自动从HTTP请求映射数据到action方法参数中。

- :doc:`/mvc/models/validation` 自动执行客户端和服务器端验证。

客户端开发
-----------------------

ASP.NET Core被设计成与多种客户端框架无缝集成, 包括 :doc:`AngularJS </client-side/angular>`、 :doc:`KnockoutJS </client-side/knockout>` 和 :doc:`Bootstrap </client-side/bootstrap>`. 查看更多细节 :doc:`/client-side/index` 。

下一步
----------

- :doc:`/tutorials/first-mvc-app/index`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`
:version: 1.0.0-rc1

Servers
=======

服务器
=======

原文：`Servers <https://docs.asp.net/en/latest/fundamentals/servers.html>`_

作者：`Steve Smith`_

翻译：`谢炀(Kiler) <https://github.com/kiler398/>`_

校对：`许登洋(Seay) <https://github.com/SeayXu>`_、`姚阿勇(Dr.Yao) <https://github.com/mengshuaiyang>`_

ASP.NET Core is completely decoupled from the web server environment that hosts the application. ASP.NET Core supports hosting in IIS and IIS Express, and self-hosting scenarios using the Kestrel and WebListener HTTP servers. Additionally, developers and third party software vendors can create custom servers to host their ASP.NET Core apps.

ASP.NET Core 已完全从承载应用程序的 Web 服务器环境中分离。ASP.NET Core 可以承载于 IIS 和 IIS Express ，以及使用 Kestrel 和 WebListener HTTP Server 的自承载环境中。此外，开发人员和第三方软件供应商可以创建自定义的服务器来承载 ASP.NET Core 应用程序。

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/servers/sample>`__

Servers and commands
--------------------

服务器和命令
--------------------

ASP.NET Core was designed to decouple web applications from the underlying HTTP server. Traditionally, ASP.NET apps have been windows-only hosted on Internet Information Server (IIS). The recommended way to run ASP.NET Core applications on Windows is using IIS as a reverse-proxy server. The HttpPlatformHandler module in IIS manages and proxies requests to an HTTP server hosted out-of-process. ASP.NET Core ships with two different HTTP servers:


ASP.NET Core 旨在将 Web 应用程序从底层 HTTP 服务器分离出来。过去，ASP.NET 应用一直只在 Windows 中承载于 IIS 上。在 Windows 上运行 ASP.NET Core 应用程序的推荐方法是将 IIS 作为一个反向代理服务器来使用。IIS 中的 HttpPlatformHandler 模块管理并分发请求给一个进程外的HTTP 服务器。ASP.NET Core 附带两个不同的 HTTP 服务器：

- Microsoft.AspNetCore.Server.Kestrel (AKA Kestrel, cross-platform)
- Microsoft.AspNetCore.Server.WebListener (AKA WebListener, Windows-only, preview)

- Microsoft.AspNetCore.Server.Kestrel （AKA Kestrel，跨平台）
- Microsoft.AspNetCore.Server.WebListener （AKA WebListener，仅 Windows，预览版）


ASP.NET Core does not directly listen for requests, but instead relies on the HTTP server implementation to surface the request to the application as a set of :doc:`feature interfaces <request-features>` composed into an HttpContext. While WebListener is Windows-only, Kestrel is designed to run cross-platform. You can configure your application to be hosted by any or all of these servers by specifying commands in your *project.json* file. You can even specify an application entry point for your application, and run it as an executable (using ``dotnet run``) rather than hosting it in a separate process.


ASP.NET Core 不直接监听请求，而是依靠 HTTP 服务器的实现将请求作为组成 HttpContext 的一组功能接口暴露给应用程序。尽管WebListener只是Window专用的，但Kestrel则是被设计为跨平台运行的。你可以通过在 *project.json* 文件中指定命令来配置你的应用程序承载于任何一个或全部的服务器。你甚至可以为应用程序指定程序入口点，作为一个可执行文件运行（使用 ``dotnet run``），而不是承载到不同的进程。 

The default web host for ASP.NET apps developed using Visual Studio is IIS Express functioning as a reverse proxy server for Kestrel. The "Microsoft.AspNetCore.Server.Kestrel" and "Microsoft.AspNetCore.Server.IISIntegration" dependencies are included in *project.json* by default, even with the Empty web site template. Visual Studio provides support for multiple profiles, associated with IIS Express. You can manage these profiles and their settings in the **Debug** tab of your web application project's Properties menu or from the *launchSettings.json* file.

用 Visual Studio 开发的 ASP.NET 应用程序默认的 Web 托管服务器采用了 Kestrel 做反向代理服务器的 IIS Express， *project.json* 文件默认包含 “Microsoft.AspNetCore.Server.Kestrel” 和 “Microsoft.AspNetCore.Server.IISIntegration” 依赖，即使采用空网站模板。Visual Studio 也提供了多种方式来把网站关联到 IISExpress。你可以在你的 web 应用程序项目的属性菜单的 **Debug** 选项卡中或者 *launchSettings.json* 文件中管理这些配置和参数。

.. image:: /fundamentals/servers/_static/serverdemo-properties.png

The sample project for this article is configured to support each server option in the *project.json* file:

本文的示例项目被配置成支持每个服务器的选项在 *project.json* 文件中：

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/project.json
  :lines: 1-17
  :emphasize-lines: 12-13
  :linenos:
  :language: json
  :caption: project.json (truncated)

The ``run`` command will launch the application from the ``void main`` method. The ``run`` command configures and starts an instance of ``Kestrel``.

``run`` 命令会通过调用 ``void main`` 方法启动应用程序。 ``run`` 命令配置和启动一个 ``Kestrel`` 实例。

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/Program.cs
  :linenos:
  :emphasize-lines: 32-40
  :language: c#
  :caption: program.cs


Supported Features by Server
----------------------------

服务支持的 Features
----------------------------

ASP.NET defines a number of :doc:`request-features`. The following table lists the WebListener and Kestrel support for request features.

ASP.NET 定义了一系列 :doc:`request-features` 。下表列出了所有 WebListener 和 Kestrel 支持 Features。

.. list-table::
  :header-rows: 1

  * - Feature
    - WebListener
    - Kestrel
  * - IHttpRequestFeature
    - Yes
    - Yes
  * - IHttpResponseFeature
    - Yes
    - Yes
  * - IHttpAuthenticationFeature
    - Yes
    - No
  * - IHttpUpgradeFeature
    - Yes (with limits)
    - Yes
  * - IHttpBufferingFeature
    - Yes
    - No
  * - IHttpConnectionFeature
    - Yes
    - Yes
  * - IHttpRequestLifetimeFeature
    - Yes
    - Yes
  * - IHttpSendFileFeature
    - Yes
    - No
  * - IHttpWebSocketFeature
    - No*
    - No*
  * - IRequestIdentifierFeature
    - Yes
    - No
  * - ITlsConnectionFeature
    - Yes
    - Yes
  * - ITlsTokenBindingFeature
    - Yes
    - No
    
    
.. list-table::
  :header-rows: 1

  * - Feature
    - WebListener
    - Kestrel
  * - IHttpRequestFeature
    - 是
    - 是
  * - IHttpResponseFeature
    - 是
    - 是
  * - IHttpAuthenticationFeature
    - 是
    - 否
  * - IHttpUpgradeFeature
    - 是（有限制）
    - 是
  * - IHttpBufferingFeature
    - 是
    - 否
  * - IHttpConnectionFeature
    - 是
    - 是
  * - IHttpRequestLifetimeFeature
    - 是
    - 是
  * - IHttpSendFileFeature
    - 是
    - 否
  * - IHttpWebSocketFeature
    - 否*
    - 否*
  * - IRequestIdentifierFeature
    - 是
    - 否
  * - ITlsConnectionFeature
    - 是
    - 是
  * - ITlsTokenBindingFeature
    - 是
    - 否

Configuration options
^^^^^^^^^^^^^^^^^^^^^

配置选项
^^^^^^^^^^^^^^^^^^^^^

You can provide configuration options (by command line parameters or a configuration file) that are read on server startup.

在服务器启动时你可以提供可读取的配置选项（命令行参数或配置文件）。

The ``Microsoft.AspNetCore.Hosting`` command supports server parameters (such as ``Kestrel`` or ``WebListener``) and a ``server.urls`` configuration key. The ``server.urls`` configuration key is a semicolon-separated list of URL prefixes that the server should handle.

``Microsoft.AspNetCore.Hosting`` 命令支持服务器参数（例如 ``Kestrel`` 或 ``WebListener`` ）还有 ``server.urls`` 配置项。 ``server.urls`` 配置键值是一系列以分号分隔的服务器必须处理 URL 前缀列表。

The *project.json* file shown above demonstrates how to pass the ``server.urls`` parameter directly:

上面的 *project.json* 文件演示了如何直接传递 ``server.urls`` 参数：

.. code-block:: javascript

  "web": "Microsoft.AspNetCore.Kestrel --server.urls http://localhost:5004"

Alternately, a  JSON configuration file can be used,

另外，也可以使用 JSON 配置文件。

.. code-block:: javascript

  "kestrel": "Microsoft.AspNetCore.Hosting"

The ``hosting.json`` can include the settings the server will use (including the server parameter, as well):

``hosting.json`` 可以作为服务器设置的参数使用（也可以包括服务器参数）：

.. code-block:: json

  {
    "server": "Microsoft.AspNetCore.Server.Kestrel",
    "server.urls": "http://localhost:5004/"
  }

Programmatic configuration
^^^^^^^^^^^^^^^^^^^^^^^^^^

编码化的配置
^^^^^^^^^^^^^^^^^^^^^^^^^^

The server hosting the application can be referenced programmatically via the IApplicationBuilder_ interface, available in the ``Configure`` method in ``Startup``. IApplicationBuilder_ exposes Server Features of type IFeatureCollection_. ``IServerAddressesFeature`` only expose a ``Addresses`` property, but different server implementations may expose additional functionality. For instance, WebListener exposes ``AuthenticationManager`` that can be used to configure the server's authentication:

托管应用程序的服务器可以通过在 ``Startup`` 类的 ``Configure`` 方法中调用 IApplicationBuilder_ 接口来引用。 IApplicationBuilder_ 将服务器 Features 暴露为 IFeatureCollection_ 类型。 ``IServerAddressesFeature`` 只公开了 ``Addresses`` 属性，但不同的服务器实现可能会暴露更多的 Features ，例如，WebListener 公开了可用于配置服务器的认证的 ``AuthenticationManager`` ：

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/Startup.cs
  :linenos:
  :lines: 37-54
  :emphasize-lines: 3,6-7,10,15
  :language: c#
  :dedent: 8


IIS and IIS Express
-------------------

IIS 与 IIS Express
-------------------

IIS is the most feature rich server, and includes IIS management functionality and access to other IIS modules. Hosting ASP.NET Core no longer uses the ``System.Web`` infrastructure used by prior versions of ASP.NET.

IIS 是最功能丰富的应用服务器，包括 IIS 管理功能和访问其他 IIS 模块。托管 ASP.NET Core 不再使用由 ASP.NET 之前版本中使用的 ``System.Web`` 基础库。

ASP.NET Core Module
^^^^^^^^^^^^^^^^^^^

ASP.NET Core 模块
^^^^^^^^^^^^^^^^^^^

In ASP.NET Core on Windows, the web application is hosted by an external process outside of IIS. The ASP.NET Core Module is a native IIS  module that is used to proxy requests to external processes that it manages. See :doc:`/hosting/aspnet-core-module` for more details.

Windows 上的 ASP.NET Core ， Web 应用程序宿主在 IIS 以外的进程上的。ASP.NET Core 模块是一个原生的 IIS 模块用来代理请求到管理的进程，更多参考 :doc:`/hosting/aspnet-core-module` 。

.. _weblistener:

WebListener
-----------

WebListener is a Windows-only HTTP server for ASP.NET Core. It runs directly on the `Http.Sys kernel driver <http://www.iis.net/learn/get-started/introduction-to-iis/introduction-to-iis-architecture>`_, and has very little overhead.

WebListener 是 ASP.NET Core 的 Windows 专用 HTTP 服务器。它直接运行在 `Http.Sys kernel driver <http://www.iis.net/learn/get-started/introduction-to-iis/introduction-to-iis-architecture>`_ 之上，并且具有非常小的开销。

You can add support for WebListener to your ASP.NET application by adding the "Microsoft.AspNetCore.Server.WebListener" dependency in *project.json* and the following command:

你可以通过在 *project.json* 里面添加 "Microsoft.AspNetCore.Server.WebListener" 依赖以及下面的命令让你的 ASP.NET 应用程序支持 WebListener ：

.. code-block:: javascript

  "web": "Microsoft.AspNetCore.Hosting --server Microsoft.AspNetCore.Server.WebListener --server.urls http://localhost:5000"

.. note:: WebListener is currently still in preview.

.. note:: WebListener 还处于预览版状态。

.. _kestrel:

Kestrel
-------

Kestrel is a cross-platform web server based on `libuv <https://github.com/libuv/libuv>`_, a cross-platform asynchronous I/O library. You add support for Kestrel by including ``Microsoft.AspNetCore.Server.Kestrel`` in your project's dependencies listed in *project.json*.

Kestrel 是一个基于 `libuv <https://github.com/libuv/libuv>`_ 的 Web 服务器，一个跨平台的异步 I/O 库。你可以通过在 *project.json* 依赖列表中包含 ``Microsoft.AspNetCore.Server.Kestrel`` 依赖来支持 Kestrel 。

Learn more about working with Kestrel to create :doc:`/tutorials/your-first-mac-aspnet`.

了解更多关于创建 Kestrel 的细节 :doc:`/tutorials/your-first-mac-aspnet` 。

.. note:: Kestrel is designed to be run behind a proxy (for example IIS or Nginx) and should not be deployed directly facing the Internet.

.. note:: Kestrel 是设计在反向代理服务器（例如 IIS 或者 Nginx ）之后的，并且不是直接面向 Internet 部署的。

Choosing a server
-----------------

服务器的选择
-----------------

If you intend to deploy your application on a Windows server, you should run IIS as a reverse proxy server that manages and proxies requests to Kestrel. If deploying on Linux, you should run a comparable reverse proxy server such as Apache or Nginx to proxy requests to Kestrel (see :doc:`/publishing/linuxproduction`).

如果你打算在 Windows 服务器上部署你的应用程序，你应该用 IIS 作为反向代理服务器来管理和代理发送到 Kestrel 的请求。如果在 Linux 上部署，你应该运行类似反向代理服务器，如 Apache 或 Nginx 的来代理发送到 Kestrel 的请求（更多参考 :doc:`/publishing/linuxproduction` ）。

Custom Servers
--------------

自定义服务器
--------------

You can create your own server in which to host ASP.NET apps, or use other open source servers. When implementing your own server, you're free to implement just the feature interfaces your application needs, though at a minimum you must support IHttpRequestFeature_ and IHttpResponseFeature_.

你可以创建你自己的服务器中承载 ASP.NET 应用程序，或使用其他开源服务器。当你实现自己的服务器，你可以自由地实现只是你的应用程序的所需要 Feature 功能接口，不过至少需要支持 IHttpRequestFeature_ 和 IHttpResponseFeature_ 。

Since Kestrel is open source, it makes an excellent starting point if you need to implement your own custom server. Like all of ASP.NET Core, you're welcome to `contribute <https://github.com/aspnet/KestrelHttpServer/blob/dev/CONTRIBUTING.md>`_ any improvements you make back to the project.

因为 Kestrel 是开源的，如果你需要实现自己的自定义服务器，是一个不错的起点。像所有的 ASP.NET Core，欢迎你 `贡献 <https://github.com/aspnet/KestrelHttpServer/blob/dev/CONTRIBUTING.md>` 自己的提交来回馈和改进这个项目。

Kestrel currently supports a limited number of feature interfaces, but additional features will be added in the future. 

Kestrel 当前支持有限数量的 Feature 接口，但是后续会增加更多 Features 支持。

Additional Reading
------------------

附加阅读
------------------

- :doc:`request-features`

.. _IApplicationBuilder: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Builder/IApplicationBuilder/index.html
.. _IFeatureCollection: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Http/Features/IFeatureCollection/index.html
.. _IHttpRequestFeature: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Http/Features/IHttpRequestFeature/index.html
.. _IHttpResponseFeature: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Http/Features/IHttpResponseFeature/index.html
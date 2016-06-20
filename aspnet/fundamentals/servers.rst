服务器
=======

作者： `Steve Smith`_  
翻译：  谢炀(kiler)   
校对：

ASP.NET Core 完全从托管应用程序的Web服务器环境脱离。 ASP.NET Core支持宿主在IIS、IIS Express、使用Kestrel的自托管脚本或者WebListener HTTP服务器。此外，开发人员和第三方软件供应商可以创建自定义的服务器来承载ASP.NET Core应用程序。

.. contents:: 章节:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/servers/sample>`__

服务器和命令
--------------------

ASP.NET Core 的设计目标是从底层HTTP服务器分离出Web应用程序。传统的方式下，ASP.NET应用程序只能托管在基于window的IIS服务器上。ASP.NET Core应用程序在window上运行推荐使用IIS作为反向代理服务器。ASP.NET Core附带两个不同的HTTP服务器，在IIS中的HttpPlatformHandler模块管理和HTTP服务器托管进程外的代理请求:

- Microsoft.AspNet.Server.WebListener (AKA WebListener, Windows-专用)
- Microsoft.AspNet.Server.Kestrel (AKA Kestrel, 跨平台)

ASP.NET Core 不直接监听请求，而是依赖于 HTTP 服务器实现的一组集成在 HttpContext中的Feature接口来将请求暴露给应用程序 。
尽管WebListener只是Window专用的，但Kestrel则是被设计为跨平台运行的。你可以通过指定配置*project.json*文件中的命令来配置让你的应用程序托管到一个或者多个应用服务器。你甚至可以为应用程序指定的应用程序入口点，作为一个可执行文件运行（使用``dotnet run``），而不是托管到独立的进程。

Visual Studio开发应用程序默认Web托管服务器为采用了Kestrel做反向代理服务器的IIS Express ，*project.json*文件默认包含“Microsoft.AspNet.Server.Kestrel”和“Microsoft.AspNet.IISPlatformHandler”依赖，即使采用空网站模板。Visual Studio也提供了多种方式来把网站关联到IIS或者其他*project.json*配置好的``commands``去。你可以在你的web应用程序项目的属性菜单的**Debug**选项卡中或者*launchSettings.json*文件中管理这些配置和参数，

.. image:: /fundamentals/servers/_static/serverdemo-properties.png

.. 注意:: IIS不支持命令. Visual Studio的启动IIS Express并加载应用程序所选择的配置文件。本文的示例项目被配置成支持多个服务器的选项在*project.json*文件中:

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/project.json
  :lines: 1-17
  :emphasize-lines: 12-13
  :linenos:
  :language: javascript
  :caption: project.json (truncated)

``run``命令会通过调用``void main``方法启动应用程序. ``run``命令配置启动``Kestrel``实例.

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/Program.cs
  :linenos:
  :emphasize-lines: 32-40
  :language: c#
  :caption: program.cs


服务无支持的Features
----------------------------

ASP.NET定义了一系列:doc:`request-features`. 下表列出了所有WebListener和Kestrel支持Features.

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
    - 是 (有限制)
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

配置选项
^^^^^^^^^^^^^^^^^^^^^

在服务器启动可以提供可读取的配置选项（命令行参数或配置文件）


``Microsoft.AspNet.Hosting``命令支持服务器参数（例如``Kestrel``或``WebListener``）还有``server.urls``配置项。``server.urls``配置键值是一系列以分号分隔的服务器必须处理URL前缀列表。


上面的*project.json*文件演示了如何直接传递``server.urls``参数:

.. code-block:: javascript

  "web": "Microsoft.AspNet.Kestrel --server.urls http://localhost:5004"

另外, 也可以使用JSON配置文件,

.. code-block:: javascript

  "kestrel": "Microsoft.AspNet.Hosting"

``hosting.json``可以作为服务器设置的参数使用 (也可以包括服务器参数):

.. code-block:: json

  {
    "server": "Microsoft.AspNet.Server.Kestrel",
    "server.urls": "http://localhost:5004/"
  }

编码化的配置
^^^^^^^^^^^^^^^^^^^^^^^^^^


托管应用程序的服务器可以通过在``Startup``类的``Configure``方法中调用IApplicationBuilder_接口来引用。IApplicationBuilder_ 将服务器Features暴露为IFeatureCollection_ 类型。 ``IServerAddressesFeature``只公开了``Addresses``属性，但不同的服务器实现可能会暴露更多的Features，例如，WebListener公开了可用于配置服务器的认证的``AuthenticationManager``：

.. literalinclude:: servers/sample/ServersDemo/src/ServersDemo/Startup.cs
  :linenos:
  :lines: 37-54
  :emphasize-lines: 3,6-7,10,15
  :language: c#
  :dedent: 8


IIS 和 IIS Express
-------------------

IIS是最功能丰富的应用服务器服务器，包括IIS管理功能和访问其他IIS模块。托管ASP.NET Core不再使用由ASP.NET之前版本中使用的``System.Web``基础库。

HTTPPlatformHandler
^^^^^^^^^^^^^^^^^^^
Windows上的ASP.NET Core，Web应用程序宿主在IIS以外的进程上的。HTTP处理平台是一个负责HTTP侦听的过程管理以及管理代理请求到进程的IIS7.5+模块。

.. _weblistener:

WebListener
-----------

WebListener是ASP.NET Core的Windows专用HTTP服务器。它直接运行在`Http.Sys kernel driver <http://www.iis.net/learn/get-started/introduction-to-iis/introduction-to-iis-architecture>`_ 之上，并且具有非常小的开销。

您可以通过在 *project.json* 里面添加 "Microsoft.AspNet.Server.WebListener" 依赖以及下面的命令让你的 ASP.NET 应用程序支持 WebListener：

.. code-block:: javascript

  "web": "Microsoft.AspNet.Hosting --server Microsoft.AspNet.Server.WebListener --server.urls http://localhost:5000"

.. _kestrel:

Kestrel
-------

Kestrel 是一个基于`libuv <https://github.com/libuv/libuv>`_的Web服务器, 一个跨平台的异步I/O库. 你可以通过在*project.json*依赖列表中包含``Microsoft.AspNet.Server.Kestrel``依赖来支持Kestrel.

了解更多如何创建Kestrel的细节:doc:`/tutorials/your-first-mac-aspnet`.

服务器的选择
-----------------

如果您打算在Windows服务器上部署部署你的应用程序，您应该用IIS作为反向代理服务器来管理和代理发送到Kestrel的请求。如果在Linux上部署，您应该运行类似反向代理服务器，如Apache或Nginx的来代理发送到Kestrel的请求。

对于自托管方案，如运行在`Service Fabric <https://azure.microsoft.com/en-us/services/service-fabric/>`_，我们建议在非IIS环境使用Kestrel。但是，如果您需要在自托管的环境下使用Windows身份验证，你应该选择WebListener。

自定义服务器
--------------

你可以创建你自己的服务器中承载ASP.NET应用程序，或使用其他开源服务器。当你实现自己的服务器，你可以自由地实现只是你的应用程序的所需要Feature功能接口，，不过至少需要支持IHttpRequestFeature_ 和 IHttpResponseFeature_.

因为Kestrel是开源的，如果你需要实现自己的自定义服务器，只是一个不错的起点。像所有的ASP.NET Core，欢迎您`贡献 <https://github.com/aspnet/KestrelHttpServer/blob/dev/CONTRIBUTING.md>`自己的提交来回馈和改进这个项目。

Kestrel 当前支持有限数量的Feature接口, 但是后续会增加更多Features支持. 

附加阅读
------------------

- :doc:`request-features`

.. _IApplicationBuilder: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Builder/IApplicationBuilder/index.html
.. _IFeatureCollection: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IFeatureCollection/index.html
.. _IHttpRequestFeature: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpRequestFeature/index.html
.. _IHttpResponseFeature: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpResponseFeature/index.html
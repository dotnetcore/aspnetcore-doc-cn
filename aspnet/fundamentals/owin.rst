Open Web Interface for .NET (OWIN)
========================================

.NET开放Web接口（OWIN）
========================================

作者 `Steve Smith`_
翻译：  谢炀(kiler)

ASP.NET Core 支持 OWIN（即 Open Web Server Interface for .NET 的首字母缩写）, OWIN的目标是用于解耦Web Server和Web Application. 此外, OWIN为中间件定义了一个标准方法用处理单个请求以及相关联的响应. ASP.NET Core 的程序和中间件可以和 基于 OWIN 应用程序, 服务器,以及中间件相互交互。

.. contents:: 章节:
  :local:
  :depth: 1

`查看下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__

在 ASP.NET 管道中运行 OWIN 中间件
-----------------------------------------------

ASP.NET Core 对于 OWIN 的支持基于 ``Microsoft.AspNet.Owin`` 包. 你可以在你的应用程序把这个包作为一个依赖导入到你的 *project.json* 文件里来实现对 OWIN 支持, 如下所示 :

.. literalinclude:: owin/sample/src/OwinSample/project.json
  :language: javascript
  :lines: 7-11
  :emphasize-lines: 4

OWIN 中间件遵循 `OWIN 标准 <http://owin.org/spec/spec/owin-1.0.0.html>`_, OWIN 标准定义了一系列 ``IDictionary<string, object>`` 需要用到的属性接口, 并且规定了某些键值必须被设置 (例如 ``owin.ResponseBody``). 我们可以基于 OWIN 标准构建一个简单的中间件的例子来显示 "Hello World", 如下所示:

.. literalinclude:: owin/sample/src/OwinSample/Startup.cs
  :language: c#
  :lines: 27-40
  :dedent: 8

在上面的例子中, 注意该方法在返回一个 ``Task`` 和接受了一个 OWIN 所必需的 ``IDictionary<string, object>`` 数据. 方法里面, 该参数用于从从环境字典对象检索 ``owin.ResponseBody`` 以及 ``owin.ResponseHeaders`` 对象. 一旦头信息被适当设定为内容被返回时, 返回展示响应流被异步写入的任务。

添加 OWIN 中间到 ASP.NET 管道是最简单的办法是使用 ``UseOwin`` 扩展方法完成。参考上面所示的 ``OwinHello`` 方法，将它添加到管道是一个简单的事情：

.. literalinclude:: owin/sample/src/OwinSample/Startup.cs
  :language: c#
  :lines: 19-25
  :dedent: 8


当然你也可以在 OWIN 管道中配置其他 actions 来替代。请记住，响应头信息只能在第一次写入响应流的时机之前修改，所以正确的配置您的管道。

.. note:: 因为性能原因同时调用多个 ``UseOwin`` 是不被鼓励的。 OWIN 组件如果能组合在一起将运作是最好的。

.. code-block:: c#

  app.UseOwin(pipeline =>
  {
      pipeline(next =>
    {
        // do something before
        return OwinHello;
        // do something after
    });
  });

.. note:: ASP.NET Core 中的对 OWIN 支持是 `Katana 项目 <http://katanaproject.codeplex.com/>`_ 的进化. Katana项目的 ``IAppBuilder`` 组件被 ``IApplicationBuilder`` 替换了, 但是你使用了现有的基于 Katana 的中间件, 你会在你的 ASP.NET Core 应用程序作中为桥梁用到它, 更多参考 `Owin.IAppBuilderBridge GitHub 案例 <https://github.com/aspnet/Entropy/tree/master/samples/Owin.IAppBuilderBridge>`_.

在基于 OWIN 的服务器上宿主 ASP.NET
---------------------------------------------

基于 OWIN 的服务器可以宿主 ASP.NET 应用程序, 因为 ASP.NET 符合 OWIN 规范.  `Nowin <https://github.com/Bobris/Nowin>`_ 就是其中之一, 一个.NET 的 OWIN Web 服务器。在本文的例子中，我已经包含一个非常简单的项目并引用 Nowin 并用它来创建一个能够自托管 ASP.NET 核心的一个简单的服务器。

.. literalinclude:: owin/sample/src/NowinSample/NowinServerFactory.cs
  :emphasize-lines: 13,19,22,27,41
  :linenos:
  :language: c#

IServerFactory_ 是一个需要Initialize 和 Start 方法的接口. Initialize 方法必须返回 IFeatureCollection_ 实例, 我们会弹出一个 ``INowinServerInformation`` 对象包含服务器名称 (具体实施方式可能会提供其他功能).在本例中,  ``NowinServerInformation`` 类被定义为工厂类的内部类，, 作为必须项被 ``Initialize`` 返回.

``Initialize`` 的职责是配置服务器, 在本示例中是通过一系列 fluent API 调用硬编码服务器监听端口5000的请求（对任何外部IP）。注意 fluent 配置最后一行的 ``builder`` 变量指定了请求会被私有方法 ``HandleRequest`` 所处理。

``Start`` 方法在 ``Initialize`` 方法之后调用接受 ``Initialize`` 方法创建的 IFeatureCollection_ 对象, 以及 ``Func<IFeatureCollection, Task>`` 回调。这个回调最终被分配到了一个本地字段并且会在每个请求的私有方法  ``HandleRequest``  中调用（这个是在 ``Initialize`` 方法中绑定的）。

上述操作就绪以后，所有的需要使用自定义服务器运行 ASP.NET 应用程序的设置都在下面的 *project.json* 文件的命令中：

.. literalinclude:: owin/sample/src/NowinSample/project.json
  :emphasize-lines: 14
  :linenos:
  :language: json
  :lines: 1-16

当应用程序运行起来以后， 这个命令将会搜索其中包含的 ``IServerFactory`` 实现的名为 "NowinSample" 包，。如果找到了，它将初始化以及按照上述方式启动服务器。了解更多关于内置 ASP.NET :doc:`/fundamentals/servers`。

在基于OWIN服务器上运行 ASP.NET Core ，并且使用 WebSockets 支持
-----------------------------------------------------------------------

如何基于OWIN的服务器“功能，可以通过ASP.NET核心加以利用另一个例子是获得像WebSockets的功能。在前面的例子中使用的.NET OWIN Web服务器具有内置的网络插座，可通过一个ASP.NET的核心应用加以利用的支持。下面的例子显示了支持网络套接字和简单的回显然后通过WebSockets发送到服务器的任何一个简单的Web应用程序。
基于OWIN的服务器的 features 如何被 ASP.NET Core 提升的另一个例子是附加新的功能（如 WebSockets），在前面的例子中使用的 .NET OWIN Web 服务器具备内置的 Web Sockets 支持，可以被 ASP.NET Core 应用程序所调用。

下面的例子显示了支持 Web Sockets 以及任何通过 WebSockets 发送到服务器内容的一个回显功能。

.. literalinclude:: owin/sample/src/NowinWebSockets/Startup.cs
  :lines: 11-
  :language: c#
  :linenos:
  :emphasize-lines: 7, 9-10

这个 `例子  <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__ 和前一个配置一样使用相同 ``NowinServerFactory`` - 唯一的区别是在该应用程序是如何在其 ``Configure`` 方法是如何配置的。用 `一个简单的 websocket 客户端 <https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en>`_ 的简单测试表明，应用程序按预期工作：

.. image:: owin/_static/websocket-test.png


OWIN 键值
---------
 
OWIN 重度依赖一个 ``IDictionary<string,object>`` 对象用来在一个完整的 HTTP 请求/响应交互中通讯信息。ASP.NET Core 实现所有的 OWIN 规范中列出的要求的必需和可选的以及自身实现的键。在OWIN规范不要求任何键是可选的，并且可以仅在某些情况下可以使用。 在使用 OWIN 键的时候, 参阅 `OWIN Key Guidelines and Common Keys <http://owin.org/spec/spec/CommonKeys.html>`_ 是一个好习惯。

请求数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - owin.RequestScheme
    - ``String``
    -
  * - owin.RequestMethod
    - ``String``
    -
  * - owin.RequestPathBase
    - ``String``
    -
  * - owin.RequestPath
    - ``String``
    -
  * - owin.RequestQueryString
    - ``String``
    -
  * - owin.RequestProtocol
    - ``String``
    -
  * - owin.RequestHeaders
    - ``IDictionary<string,string[]>``
    -
  * - owin.RequestBody
    - ``Stream``
    - 
 
请求数据 (OWIN v1.1.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - owin.RequestId
    - ``String``
    - Optional

响应数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - owin.ResponseStatusCode
    - ``int``
    - Optional
  * - owin.ResponseReasonPhrase
    - ``String``
    - Optional
  * - owin.ResponseHeaders
    - ``IDictionary<string,string[]>``
    - 
  * - owin.ResponseBody
    - ``Stream``
    -

其他数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - 键
    - 值 (类型)
    - 描述
  * - owin.CallCancelled
    - ``CancellationToken``
    -
  * - owin.Version
    - ``String``
    -

通用键值
^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - 键
    - 值 (类型)
    - 描述
  * - ssl.ClientCertificate
    - ``X509Certificate``
    -
  * - ssl.LoadClientCertAsync
    - ``Func<Task>``
    -
  * - server.RemoteIpAddress
    - ``String``
    -
  * - server.RemotePort
    - ``String``
    -
  * - server.LocalIpAddress
    - ``String``
    -
  * - server.LocalPort
    - ``String``
    -
  * - server.IsLocal
    - ``bool``
    -
  * - server.OnSendingHeaders
    - ``Action<Action<object>,object>``
    -

发送文件 v0.3.0
^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - sendfile.SendAsync
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-SendFile-Extension-v0.3.0.htm>`_
    - 每请求

Opaque v0.3.0
^^^^^^^^^^^^^


.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - opaque.Version
    - ``String``
    -
  * - opaque.Upgrade
    - ``OpaqueUpgrade``
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-OpaqueStream-Extension-v0.3.0.htm>`__
  * - opaque.Stream
    - ``Stream``
    -
  * - opaque.CallCancelled
    - ``CancellationToken``
    -

WebSocket v0.3.0
^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - 键
    - 值 (类型)
    - 描述
  * - websocket.Version
    - ``String``
    -
  * - websocket.Accept
    - ``WebSocketAccept``
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.AcceptAlt
    -
    - Non-spec
  * - websocket.SubProtocol
    - ``String``
    - 参考 `RFC6455 Section 4.2.2 <https://tools.ietf.org/html/rfc6455#section-4.2.2>`_ 章节 5.5
  * - websocket.SendAsync
    - ``WebSocketSendAsync``
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.ReceiveAsync
    - ``WebSocketReceiveAsync``
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.CloseAsync
    - ``WebSocketCloseAsync``
    - 参考 `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.CallCancelled
    - ``CancellationToken``
    -
  * - websocket.ClientCloseStatus
    - ``int``
    - 可选
  * - websocket.ClientCloseDescription
    - ``String``
    - 可选

汇总
-------

ASP.NET Core 内置支持 OWIN 标准, 在基于 OWIN的服务器或者支持基于 OWIN 中间件的 ASP.NET Core 服务器提供兼容性来运行ASP.NET Core 应用程序。

附录资源
--------------------

- :doc:`middleware`
- :doc:`servers`

.. _IFeatureCollection: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IFeatureCollection/index.html
.. _IServerFactory: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/Server/IServerFactory/index.html

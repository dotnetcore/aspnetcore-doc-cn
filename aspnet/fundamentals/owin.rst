:version: 1.0.0

Open Web Interface for .NET (OWIN)
==================================

.NET开放Web接口（OWIN）
========================================

By `Steve Smith`_ and  `Rick Anderson`_

作者 `Steve Smith`_ 、 `Rick Anderson`_

翻译：  谢炀(kiler)

ASP.NET Core supports OWIN, the Open Web Interface for .NET, which allows web applications to be decoupled from web servers. In addition, OWIN defines a standard way for middleware to be used in a pipeline to handle individual requests and associated responses. ASP.NET Core applications and middleware can interoperate with OWIN-based applications, servers, and middleware.

ASP.NET Core 支持 OWIN（即 Open Web Server Interface for .NET 的首字母缩写），OWIN的目标是用于解耦Web Server和Web Application。此外， OWIN为中间件定义了一个标准方法用处理单个请求以及相关联的响应。ASP.NET Core 的程序和中间件可以和 OWIN-based 应用程序、服务器以及中间件相互交互。

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__

`查看下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__

Running OWIN middleware in the ASP.NET pipeline
-----------------------------------------------

在 ASP.NET 管道中运行 OWIN 中间件
-----------------------------------------------

ASP.NET Core's OWIN support is deployed as part of the ``Microsoft.AspNetCore.Owin`` package. You can import OWIN support into your project by adding this package as a dependency in your *project.json* file:

ASP.NET Core 对于 OWIN 的支持基于 ``Microsoft.AspNetCore.Owin`` 包。你可以在你的应用程序把这个包作为一个依赖导入到你的 *project.json* 文件里来实现对 OWIN 支持， 如下所示 ：

.. literalinclude:: owin/sample/src/OwinSample/project.json
  :language: javascript
  :lines: 7-11
  :emphasize-lines: 4

OWIN middleware conforms to the `OWIN specification <http://owin.org/spec/spec/owin-1.0.0.html>`_, which requires a ``Func<IDictionary<string, object>, Task>`` interface, and specific keys be set (such as ``owin.ResponseBody``). The following simple OWIN middleware displays "Hello World":

OWIN 中间件遵循 `OWIN 标准 <http://owin.org/spec/spec/owin-1.0.0.html>`_， OWIN 标准定义了一系列 ``IDictionary<string, object>`` 需要用到的属性接口， 并且规定了某些键值必须被设置 (例如 ``owin.ResponseBody``)。 下面的简单的中间件的例子来显示 "Hello World"：

.. literalinclude:: owin/sample/src/OwinSample/Startup.cs
  :language: c#
  :lines: 26-40
  :dedent: 8

The sample signature returns a ``Task`` and accepts an ``IDictionary<string, object>`` as required by OWIN.

OWIN 最简单的方法签名是接收一个 ``IDictionary<string, object>`` 输入参数并且返回 ``Task`` 结果。

Adding OWIN middleware to the ASP.NET pipeline is most easily done using the ``UseOwin`` extension method. Given the ``OwinHello`` method shown above, adding it to the pipeline is a simple matter:

添加 OWIN 中间到 ASP.NET 管道是最简单的办法是使用 ``UseOwin`` 扩展方法完成。参考上面所示的 ``OwinHello`` 方法，将它添加到管道是一个简单的事情：

.. literalinclude:: owin/sample/src/OwinSample/Startup.cs
  :language: c#
  :lines: 18-25
  :dedent: 8

You can configure other actions to take place within the OWIN pipeline.

当然你也可以在 OWIN 管道中配置其他 actions 来替代。

.. note:: Response headers should only be modified prior to the first write to the response stream.
.. note:: Multiple calls to ``UseOwin`` is discouraged for performance reasons. OWIN components will operate best if grouped together.

.. note:: 响应头信息只能在第一次写入响应流的时机之前修改。
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

Using ASP.NET Hosting on an OWIN-based server
---------------------------------------------

在基于 OWIN 的服务器上宿主 ASP.NET
---------------------------------------------

OWIN-based servers can host ASP.NET applications. One such server is `Nowin <https://github.com/Bobris/Nowin>`_, a .NET OWIN web server. In the sample for this article, I've included a project that references Nowin and uses it to create an ``IServer`` capable of self-hosting ASP.NET Core.

基于 OWIN 的服务器可以宿主 ASP.NET 应用程序，   `Nowin <https://github.com/Bobris/Nowin>`_ 就是其中之一，一个.NET 的 OWIN Web 服务器。在本文的例子中，我已经包含一个非常简单的项目并引用 Nowin 并用它来创建一个能够自托管 ASP.NET 核心的一个简单的服务器。

.. literalinclude:: owin/sample/src/NowinSample/NowinServer.cs
  :emphasize-lines: 15
  :language: c#

``IServer`` is an interface that requires an ``Features`` property and a ``Start`` method.

``IServer`` 是一个需要 ``Features`` 属性和 ``Start`` 方法的接口。

``Start`` is responsible for configuring and starting the server, which in this case is done through a series of fluent API calls that set addresses parsed from the IServerAddressesFeature. Note that the fluent configuration of the ``_builder`` variable specifies that requests will be handled by the ``appFunc`` defined earlier in the method. This ``Func`` is called on each request to process incoming requests.

``Start`` 的职责是配置和启动服务器，在本示例中是通过一系列 fluent API 调用IServerAddressesFeature硬编码服务器地址来监听请求。注意 fluent 的 ``builder`` 变量指定了请求会被方法 ``appFunc`` 所处理。 ``Func`` 方法在每一个请求被处理前调用。

We'll also add an ``IWebHostBuilder`` extension to make it easy to add and configure the Nowin server.

我们同样会添加 ``IWebHostBuilder`` 扩展来使得 Nowin 服务器易于添加和配置。

.. literalinclude:: owin/sample/src/NowinSample/NowinWebHostBuilderExtensions.cs
  :emphasize-lines: 11
  :language: c#

With this in place, all that's required to run an ASP.NET application using this custom server is the following command in *project.json*:

上述操作就绪以后，所有的需要使用自定义服务器运行 ASP.NET 应用程序的设置都在下面的 *project.json* 文件的命令中：

.. literalinclude:: owin/sample/src/NowinSample/Program.cs
  :emphasize-lines: 15
  :language: c#

Learn more about ASP.NET :doc:`/fundamentals/servers`.

了解更多关于 ASP.NET :doc:`/fundamentals/servers`。

Run ASP.NET Core on an OWIN-based server and use its WebSockets support
-----------------------------------------------------------------------

在 OWIN-based 服务器上运行 ASP.NET Core 并使用 WebSockets 支持
-----------------------------------------------------------------------

Another example of how OWIN-based servers' features can be leveraged by ASP.NET Core is access to features like WebSockets. The .NET OWIN web server used in the previous example has support for Web Sockets built in, which can be leveraged by an ASP.NET Core application. The example below shows a simple web app that supports Web Sockets and echoes back everything sent to the server through WebSockets.

如何基于OWIN的服务器功能，可以通过ASP.NET核心加以利用另一个例子是获得像WebSockets的功能。在前面的例子中使用的.NET OWIN Web服务器具有内置的网络插座，可通过一个ASP.NET的核心应用加以利用的支持。下面的例子显示了支持网络套接字和简单的回显然后直接通过WebSockets发送到服务器的任何一个简单的Web应用程序。

.. literalinclude:: owin/sample/src/NowinWebSockets/Startup.cs
  :lines: 11-
  :language: c#
  :linenos:
  :emphasize-lines: 7, 9-10

This `sample  <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__ is configured using the same ``NowinServer`` as the previous one - the only difference is in how the application is configured in its ``Configure`` method. A test using `a simple websocket client <https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en>`_ demonstrates  the application:

这个 `例子  <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/owin/sample>`__ 和前一个配置一样使用相同 ``NowinServer`` - 唯一的区别是在该应用程序是如何在其 ``Configure`` 方法是如何配置的。用 `一个简单的 websocket 客户端 <https://chrome.google.com/webstore/detail/simple-websocket-client/pfdhoblngboilpfeibdedpjgfnlcodoo?hl=en>`_ 的演示实际效果：

.. image:: owin/_static/websocket-test.png


OWIN keys
---------

OWIN 键值
---------

OWIN depends heavily on an ``IDictionary<string,object>`` used to communicate information throughout an HTTP Request/Response exchange. ASP.NET Core implements all of the required and optional keys outlined in the OWIN specification, as well as some of its own. Note that any keys not required in the OWIN specification are optional and may only be used in some scenarios. When working with OWIN keys, it's a good idea to review the list of `OWIN Key Guidelines and Common Keys <http://owin.org/spec/spec/CommonKeys.html>`_

OWIN 依赖一个 ``IDictionary<string,object>`` 对象用来在一个完整的 HTTP 请求/响应交互中通讯信息。ASP.NET Core 实现所有的 OWIN 规范中列出的要求的必需和可选的以及自身实现的键。在OWIN规范不要求任何键是可选的，并且可以仅在某些情况下可以使用。 在使用 OWIN 键的时候，参阅 `OWIN Key Guidelines and Common Keys <http://owin.org/spec/spec/CommonKeys.html>`_ 是一个好习惯。

Request Data (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

请求数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - Key
    - Value (type)
    - Description
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
 
Request Data (OWIN v1.1.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

请求数据 (OWIN v1.1.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - Key
    - Value (type)
    - Description
  * - owin.RequestId
    - ``String``
    - Optional

.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - owin.RequestId
    - ``String``
    - 可选项

Response Data (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^^

响应数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - Key
    - Value (type)
    - Description
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
    
.. list-table::
  :header-rows: 1

  * - 键
    - 值 (类型)
    - 描述
  * - owin.ResponseStatusCode
    - ``int``
    - 可选项
  * - owin.ResponseReasonPhrase
    - ``String``
    - 可选项
  * - owin.ResponseHeaders
    - ``IDictionary<string,string[]>``
    - 
  * - owin.ResponseBody
    - ``Stream``
    -

Other Data (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

其他数据 (OWIN v1.0.0)
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - Key
    - Value (type)
    - Description
  * - owin.CallCancelled
    - ``CancellationToken``
    -
  * - owin.Version
    - ``String``
    -
    
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

Common Keys
^^^^^^^^^^^

通用键值
^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - Key
    - Value (type)
    - Description
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

SendFiles v0.3.0
^^^^^^^^^^^^^^^^

发送文件 v0.3.0
^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1

  * - Key
    - Value (type)
    - Description
  * - sendfile.SendAsync
    - See `delegate signature <http://owin.org/spec/extensions/owin-SendFile-Extension-v0.3.0.htm>`_
    - Per Request
    
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

  * - Key
    - Value (type)
    - Description
  * - opaque.Version
    - ``String``
    -
  * - opaque.Upgrade
    - ``OpaqueUpgrade``
    - See `delegate signature <http://owin.org/spec/extensions/owin-OpaqueStream-Extension-v0.3.0.htm>`__
  * - opaque.Stream
    - ``Stream``
    -
  * - opaque.CallCancelled
    - ``CancellationToken``
    -
    
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
    - See `delegate signature <http://owin.org/spec/extensions/owin-OpaqueStream-Extension-v0.3.0.htm>`__
  * - opaque.Stream
    - ``Stream``
    -
  * - opaque.CallCancelled
    - ``CancellationToken`

WebSocket v0.3.0
^^^^^^^^^^^^^^^^

.. list-table::
  :header-rows: 1
  
  * - Key
    - Value (type)
    - Description
  * - websocket.Version
    - ``String``
    -
  * - websocket.Accept
    - ``WebSocketAccept``
    - See `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.AcceptAlt
    -
    - Non-spec
  * - websocket.SubProtocol
    - ``String``
    - See `RFC6455 Section 4.2.2 <https://tools.ietf.org/html/rfc6455#section-4.2.2>`_ Step 5.5
  * - websocket.SendAsync
    - ``WebSocketSendAsync``
    - See `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.ReceiveAsync
    - ``WebSocketReceiveAsync``
    - See `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.CloseAsync
    - ``WebSocketCloseAsync``
    - See `delegate signature <http://owin.org/spec/extensions/owin-WebSocket-Extension-v0.4.0.htm>`__.
  * - websocket.CallCancelled
    - ``CancellationToken``
    -
  * - websocket.ClientCloseStatus
    - ``int``
    - Optional
  * - websocket.ClientCloseDescription
    - ``String``
    - Optional
    
    
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
    - 没有规定
  * - websocket.SubProtocol
    - ``String``
    - 参考 `RFC6455 Section 4.2.2 <https://tools.ietf.org/html/rfc6455#section-4.2.2>`_ Step 5.5
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
    - 可选项
  * - websocket.ClientCloseDescription
    - ``String``
    - 可选项

Additional Resources
--------------------

附录资源
--------------------

- :doc:`middleware`
- :doc:`servers`

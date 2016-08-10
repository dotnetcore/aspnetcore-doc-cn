Request Features
================

请求功能
================

By `Steve Smith`_

作者： `Steve Smith`_  

翻译： 谢炀(kiler)   

校对： 姚阿勇(Dr.Yao)

Individual web server features related to how HTTP requests and responses are handled have been factored into separate interfaces. These abstractions are used by individual server implementations and middleware to create and modify the application's hosting pipeline.

涉及到如何处理 HTTP 请求以及响应的独立 Web 服务器功能已经被分解成独立的接口，这些抽象被独立的服务器实现和中间件用于创建和修改应用程序的托管管道。

.. contents:: Sections:
  :local:
  :depth: 1

Feature interfaces
------------------

功能接口
------------------

ASP.NET Core defines a number of HTTP feature interfaces in :dn:ns:`Microsoft.AspNetCore.Http.Features` which are used by servers to identify the features they support. The following feature interfaces handle requests and return responses:

ASP.NET Core 定义了许多 `HTTP 功能接口 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/index.html>`, 给服务器用来判断支持哪些功能。Web 服务器最基础功能就是处理请求并返回响应，下面是实现这个功能所涉及的接口：

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestFeature`
  Defines the structure of an HTTP request, including the protocol, path, query string, headers, and body.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestFeature`
  定义HTTP请求的结构, 包括协议, 路径, 查询字符串, 请求头以及正文.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpResponseFeature`
  Defines the structure of an HTTP response, including the status code, headers, and body of the response.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpResponseFeature`
  定义HTTP响应的结构, 包括状态码, 响应头以及响应正文.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.Authentication.IHttpAuthenticationFeature`
  Defines support for identifying users based on a ``ClaimsPrincipal`` and specifying an authentication handler.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.Authentication.IHttpAuthenticationFeature`
  定义对基于 ``ClaimsPrincipal`` 识别用户的支持以及指定验证处理程序。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpUpgradeFeature`
  Defines support for :rfc:`HTTP Upgrades <2616#section-14.42>`, which allow the client to specify which additional protocols it would like to use if the server wishes to switch protocols.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpUpgradeFeature`
  定义对 :rfc:`HTTP 升级 <2616#section-14.42>` 的支持, 允许客户端在服务器希望切换协议的时候指定自己想要使用的协议。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpBufferingFeature`
  Defines methods for disabling buffering of requests and/or responses.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpBufferingFeature`
  定义用于禁用请求和/或响应的缓冲的方法。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature`
  Defines properties for local and remote addresses and ports.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpBufferingFeature`
  定义本地和远程地址以及端口的属性。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestLifetimeFeature`
  Defines support for aborting connections, or detecting if a request has been terminated prematurely, such as by a client disconnect.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestLifetimeFeature`
  定义支持中止连接，或者对请求提前终止的检测，比如客户断开连接等原因。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature`
  Defines a method for sending files asynchronously.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature`
  定义一个异步发送文件的方法。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpWebSocketFeature`
  Defines an API for supporting web sockets.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpWebSocketFeature`
  定义一个支持 Web Sockets 的 API。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestIdentifierFeature`
  Adds a property that can be implemented to uniquely identify requests.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestIdentifierFeature`
  添加一个可以实现唯一标识请求的属性。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature`
  Defines ``ISessionFactory`` and ``ISession`` abstractions for supporting user sessions.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature`
  定义 ``ISessionFactory`` 和 ``ISession`` 抽象接口以支持用户会话。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature`
  Defines an API for retrieving client certificates.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature`
  定义一个检索客户端证书的 API。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsTokenBindingFeature`
  Defines methods for working with TLS token binding parameters.
  
:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsTokenBindingFeature`
  定义用来处理 TLS token 绑定参数的方法。

.. note:: :dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature` is not a server feature, but is implemented by the :dn:cls:`~Microsoft.AspNetCore.Session.SessionMiddleware` (see :doc:`/fundamentals/app-state`).

.. note::  :dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature` 不是一个服务器功能, 而是由 :dn:cls:`~Microsoft.AspNetCore.Session.SessionMiddleware` 实现的 （见 :doc:`/fundamentals/app-state`）。  
  
Feature collections
-------------------

功能集合
-------------------

The :dn:prop:`~Microsoft.AspNetCore.Http.HttpContext.Features` property of :dn:cls:`~Microsoft.AspNetCore.Http.HttpContext` provides an interface for getting and setting the available HTTP features for the current request. Since the feature collection is mutable even within the context of a request, middleware can be used to modify the collection and add support for additional features.

:dn:prop:`~Microsoft.AspNetCore.Http.HttpContext.Features` 的 :dn:cls:`~Microsoft.AspNetCore.Http.HttpContext` 属性提供了一个接口用于获取和设置当前请求可用的 HTTP 功能。由于功能集合在请求上下文中都是可变的，那么中间件也可以用来修改集合以及添加对额外的功能支持。

Middleware and request features
-------------------------------

中间件和请求特性
-------------------------------

While servers are responsible for creating the feature collection, middleware can both add to this collection and consume features from the collection. For example, the :dn:cls:`~Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware` accesses the :dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature` feature. If the feature exists, it is used to send the requested static file from its physical path. Otherwise, a slower alternative method is used to send the file. When available, the :dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature` allows the operating system to open the file and perform a direct kernel mode copy to the network card.

虽然服务器是负责创建功能集合的，但中间件既可以给集合添加功能也可以从中取用功能。例如，静态文件中间件 :dn:cls:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature` 就会使用文件发送功能 :dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature` 。如果该功能存在，则用它把请求的物理路径中的静态文件发送出去，否则，会采用一个比较慢的发送文件的备用方法。当功能可用的时候，``IHttpSendFileFeature`` 允许操作系统打开文件，并且直接执行内核模式拷贝到网卡。

Additionally, middleware can add to the feature collection established by the server. Existing features can even be replaced by middleware, allowing the middleware to augment the functionality of the server. Features added to the collection are available immediately to other middleware or the underlying application itself later in the request pipeline.

此外，中间件可以添加到由服务器建立的功能集合里面。中间件甚至可以取代现有的功能，允许中间件增加服务器的功能。添加到集合中的功能对请求管道中靠后面的其他中间件或者基础应用程序本身会立即生效。

By combining custom server implementations and specific middleware enhancements, the precise set of features an application requires can be constructed. This allows missing features to be added without requiring a change in server, and ensures only the minimal amount of features are exposed, thus limiting attack surface area and improving performance.

通过结合自定义的服务器实现和特定的中间件增强，可以构造出应用程序所需的精炼的功能集合。这使得无需改动服务器就可以添加缺失的功能，并确保只有最小数量的功能被公开，从而减少攻击面并提供性能。

Summary
-------

总结
-------

Feature interfaces define specific HTTP features that a given request may support. Servers define collections of features, and the initial set of features supported by that server, but middleware can be used to enhance these features.

功能接口定义给定请求可能支持的特殊功能。服务器定义功能集合，以及该服务器所支持功能的初始集，而中间件则可用来增强这些功能。 

Additional Resources
--------------------

其他资源
--------------------

- :doc:`servers`
- :doc:`middleware`
- :doc:`owin`
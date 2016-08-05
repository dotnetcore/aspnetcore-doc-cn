请求功能
================

作者： `Steve Smith`_  

翻译： 谢炀(kiler)   

校对： 姚阿勇(Dr.Yao)

涉及到如何处理 HTTP 请求以及响应的独立 Web 服务器功能已经被分解成独立的接口，这些抽象被独立的服务器实现和中间件用于创建和修改应用程序的托管管道。

.. contents:: 章节:
  :local:
  :depth: 1

功能接口
------------------

ASP.NET Core 在 :dn:ns:`Microsoft.AspNetCore.Http.Features` 里定义了许多 HTTP 功能接口, 给服务器用来判断支持哪些功能。下面的功能接口处理请求并返回响应：

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestFeature`
  定义HTTP请求的结构, 包括协议, 路径, 查询字符串, 请求头以及正文.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpResponseFeature`
  定义HTTP响应的结构, 包括状态码, 响应头以及响应正文.

:dn:iface:`~Microsoft.AspNetCore.Http.Features.Authentication.IHttpAuthenticationFeature`
  定义对基于 ``ClaimsPrincipal`` 识别用户的支持以及指定验证处理程序。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpUpgradeFeature`
  定义对 :rfc:`HTTP 升级 <2616#section-14.42>` 的支持, 允许客户端在服务器希望切换协议的时候指定自己想要使用的协议。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpBufferingFeature`
  定义用于禁用请求和/或响应的缓冲的方法。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature`
  定义本地和远程地址以及端口的属性。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestLifetimeFeature`
  定义支持中止连接，或者对请求提前终止的检测，比如客户断开连接等原因。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature`
  定义一个异步发送文件的方法。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpWebSocketFeature`
  定义一个支持 Web Sockets 的 API。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpRequestIdentifierFeature`
  添加一个可以实现唯一标识请求的属性。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature`
  定义 ``ISessionFactory`` 和 ``ISession`` 抽象接口以支持用户会话。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsConnectionFeature`
  定义一个检索客户端证书的 API。

:dn:iface:`~Microsoft.AspNetCore.Http.Features.ITlsTokenBindingFeature`
  定义用来处理 TLS token 绑定参数的方法。

.. 注意::  :dn:iface:`~Microsoft.AspNetCore.Http.Features.ISessionFeature` 不是一个服务器功能, 而是由 :dn:cls:`~Microsoft.AspNetCore.Session.SessionMiddleware` 实现的 （见 :doc:`/fundamentals/app-state`）。
  
功能集合
-------------------

:dn:cls:`~Microsoft.AspNetCore.Http.HttpContext` 的 :dn:prop:`~Microsoft.AspNetCore.Http.HttpContext.Features` 属性提供了一个接口用于获取和设置当前请求可用的 HTTP 功能。由于功能集合在请求上下文中都是可变的，那么中间件也可以用来修改集合以及添加对额外的功能支持。

中间件和请求特性
-------------------------------

虽然服务器是负责创建功能集合的，但中间件既可以给集合添加功能也可以从中取用功能。例如，静态文件中间件 :dn:cls:`~Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware` 就会使用文件发送功能 :dn:iface:`~Microsoft.AspNetCore.Http.Features.IHttpSendFileFeature` 。如果该功能存在，则用它把请求的物理路径中的静态文件发送出去，否则，会采用一个比较慢的发送文件的备用方法。当功能可用的时候，``IHttpSendFileFeature`` 允许操作系统打开文件，并且直接执行内核模式拷贝到网卡。

此外，中间件可以添加到由服务器建立的功能集合里面。中间件甚至可以取代现有的功能，允许中间件增加服务器的功能。添加到集合中的功能对请求管道中靠后面的其他中间件或者基础应用程序本身会立即生效。


通过结合自定义的服务器实现和特定的中间件增强，可以构造出应用程序所需的精炼的功能集合。这使得无需改动服务器就可以添加缺失的功能，并确保只有最小数量的功能被公开，从而减少攻击面并提供性能。

总结
-------

功能接口定义给定请求可能支持的特殊功能。服务器定义功能集合，以及该服务器所支持功能的初始集，而中间件则可用来增强这些功能。 

其他资源
--------------------

- :doc:`servers`
- :doc:`middleware`
- :doc:`owin`

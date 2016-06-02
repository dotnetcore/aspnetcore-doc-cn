请求Features
================

作者： `Steve Smith`_  
翻译：  谢炀(kiler)   
校对：

涉及到如何处理特定Web服务器对于HTTP请求和响应的功能已被分解成独立的接口,这些抽象化的接口被特定服务器的实现和中间件用来创建和修改应用程序的托管管道.

.. contents:: 章节:
  :local:
  :depth: 1

Feature 接口
------------------

ASP.NET Core 定义了一系列 `HTTP Feature 接口 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/index.html>`_, 提供给服务器来判断哪些功能是支持的. Web服务器的最基本的特征是能够处理请求并返回应答，通过下面定义的特征接口：

`IHttpRequestFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpRequestFeature/index.html>`_
  定义HTTP请求的结构, 包括协议, 路径, 查询字符串, 请求头,还有正文.

`IHttpResponseFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpResponseFeature/index.html>`_
  定义HTTP响应的结构, 包括状态码, 请求头, 响应正文.

`IHttpAuthenticationFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/Authentication/IHttpAuthenticationFeature/index.html>`_
  定义了基于``ClaimsPrincipal``用户验证的支持以及指定验证处理程序.

`IHttpUpgradeFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpUpgradeFeature/index.html>`_
  定义`HTTP 升级 <http://tools.ietf.org/html/rfc2616#section-14.42>`_支持, 允许客户端在服务器希望切换协议的时候能够能够指定到对应的协议.

`IHttpBufferingFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpBufferingFeature/index.html>`_
  定义用于禁用请求和响应的缓冲方法.

`IHttpConnectionFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpConnectionFeature/index.html>`_
  定义了本地和远程地址和端口的属性。

`IHttpRequestLifetimeFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpRequestLifetimeFeature/index.html>`_
  定义用于退出的连接、或者检测到一个请求已被提前终止（如客户端中断）的支持。

`IHttpSendFileFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpSendFileFeature/index.html>`_
  定义异步发送文件的方法.

`IHttpWebSocketFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpWebSocketFeature/index.html>`_
  定义支持Web Sockets的API。

`IHttpRequestIdentifierFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/IHttpRequestIdentifierFeature/index.html>`_
  添加属性可以实现唯一标识请求。

`ISessionFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/ISessionFeature/index.html>`_
  定义``ISessionFactory``和``ISession``接口支持抽象用户会话。

`ITlsConnectionFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/ITlsConnectionFeature/index.html>`_
  定义检索客户端证书的API。

`ITlsTokenBindingFeature <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/ITlsTokenBindingFeature/index.html>`_
  定义用来处理TLS token绑定参数的方法。

.. 注意:: ``ISessionFeature`` 不是一个服务器功能, 但是被``SessionMiddleware``实现了 (见 :doc:`/fundamentals/app-state`).
  
Feature集合
-------------------

`HttpContext.Features <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/HttpContext/index.html#prop-Microsoft.AspNet.Http.HttpContext.Features>`_ 属性提供了一个接口用于获取和设置当前请求可用的HTTP Feature。因为Feature集合是可变的，即使在一个请求上下文中，中间件也具备修改Feature集合能力，进而提供添加额外的Feature的支持。

中间件和请求特性
-------------------------------

虽然服务器的职责是创建Feature集合，中间件也可添加到这个Feature集合里面去并被使用。例如，`StaticFileMiddleware'访问``IHttpSendFileFeature``Feature。如果该Feature存在，它被用来读取物理路径并发送静态文件请求。否则，会采用一个慢得多的解决办法方法用于发送文件。当Feature可用的时候，``IHttpSendFileFeature``允许操作系统打开文件，并且直接执行内核模式拷贝到网卡。

此外，中间件可以添加到由服务器建立的Feature集合里面。现有的Feature甚至可以由中间件所取代，允许中间件增加的服务器的功能。当Feature添加到集合以后，可立即提供给其他中间件或请求管道底层的应用程序使用。

.. note:: Use the `FeatureCollectionExtensions <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Http/Features/FeatureCollectionExtensions/index.html>`__ to easily get and set features on the ``HttpContext``.

通过结合自定义服务器的实现和具体的中间件增强，可以精确的构造所需要的Feature集合的应用。这使得在添加减少Feature的时候无需对服务器进行改动，并确保只有最小数量的量Feature被对外公开，从而减少被攻击的表面积并提高性能。

总结
-------

Feature接口定义了请求可以支持HTTP Feature。服务器定义Feature集合，以及该服务器初始设置的的Feature，但中间件也可用于增强这些Feature。 

其他资源
--------------------

- :doc:`servers`
- :doc:`middleware`
- :doc:`owin`

:version: 1.0.0-rc1

Enabling Cross-Origin Requests (CORS)
=====================================

启用跨域请求 (CORS)
=====================================

By `Mike Wasson`_

作者 `Mike Wasson`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

Browser security prevents a web page from making AJAX requests to another domain. This restriction is called the *same-origin policy*, and prevents a malicious site from reading sensitive data from another site. However, sometimes you might want to let other sites make cross-origin requests to your web app.

浏览器安全性阻止网页向另一个域名通过AJAX发起请求。这项限制被称为 *同源策略*，以防止恶意网站从其他网站读取敏感数据。不过某些时候，你可能需要让其他站点向你的网站应用发起跨域请求。

`Cross Origin Resource Sharing <http://www.w3.org/TR/cors/>`_ (CORS) is a W3C standard that allows a server to relax the same-origin policy. Using CORS, a server can explicitly allow some cross-origin requests while rejecting others. CORS is safer and more flexible than earlier techniques such as `JSONP <http://en.wikipedia.org/wiki/JSONP>`_. This topic shows how to enable CORS in your ASP.NET Core application.

`跨域资源共享 <http://www.w3.org/TR/cors/>`_ (CORS) 是一项允许服务器放宽同源策略的W3C标准。基于跨域资源共享，服务器可以明确地在拒绝其它跨域请求的同时允许一部分跨域请求。跨域资源共享比早期诸如`JSONP <http://en.wikipedia.org/wiki/JSONP>`_之类的技术更灵活、更安全。这篇文章介绍了如何在你的ASP.NET Core应用中开启跨域资源共享。

.. contents:: Sections:
  
  :local:
  :depth: 1

What is "same origin"?
----------------------

什么是“同源”？
----------------------

Two URLs have the same origin if they have identical schemes, hosts, and ports. (`RFC 6454 <http://tools.ietf.org/html/rfc6454>`_)

两条URL具有相同的模式（Scheme）、主机（Host）、及端口（Port）则为“同源”。(`RFC 6454 <http://tools.ietf.org/html/rfc6454>`_)

These two URLs have the same origin:

以下两个URL为同源：

- \http://example.com/foo.html
- \http://example.com/bar.html

These URLs have different origins than the previous two:

与上面两个相比，以下URL则不同源：

- \http://example.net - Different domain
- \http://example.com:9000/foo.html - Different port
- \https://example.com/foo.html - Different scheme
- \http://www.example.com/foo.html - Different subdomain

.. note:: Internet Explorer does not consider the port when comparing origins.

.. 备注:: IE浏览器对端口号不作为同源判断依据。

Setting up CORS
---------------

设置CORS
---------------

To setup CORS for your application you use the ``Microsoft.AspNetCore.Cors`` package. In your project.json file, add the following:

为你的应用程序设置CORS你需要应用 ``Microsoft.AspNetCore.Cors`` 包。在你的project.json文件，添加如下内容：

.. literalinclude:: cors/sample/src/CorsExample1/project.json
  :language: none
  :lines: 5,6,9
  :emphasize-lines: 2
  
Add the CORS services in Startup.cs:

在Startup.cs添加CORS服务：

.. literalinclude:: cors/sample/src/CorsExample1/Startup.cs
  :language: csharp
  :lines: 9-12
  :dedent: 8

Enabling CORS with middleware
-----------------------------

使用中间件启用CORS
-----------------------------

To enable CORS for your entire application add the CORS middleware to your request pipeline using the ``UseCors`` extension method. Note that the CORS middleware must proceed any defined endpoints in your app that you want to support cross-origin requests (ex. before any call to ``UseMvc``).

要为你的应用全局启用CORS，需使用 ``UseCors`` 扩展方法为你的请求管道添加CORS中间件。注意：CORS中间件必须在每一个你想要支持跨域请求的终端布置。

You can specify a cross-origin policy when adding the CORS middleware using the ``CorsPolicyBuilder`` class. There are two ways to do this. The first is to call UseCors with a lambda:

通过使用 ``CorsPolicyBuilder`` 类来添加CORS中间件，你可以指定同源策略。

.. literalinclude:: cors/sample/src/CorsExample1/Startup.cs
  :language: csharp
  :lines: 15-18, 24
  :dedent: 8

The lambda takes a CorsPolicyBuilder object. I’ll describe all of the configuration options later in this topic. In this example, the policy allows cross-origin requests from "\http://example.com" and no other origins.

lambda提供了一个CorsPolicyBuilder对象。稍后在这篇文章里，我将描述所有的配置项。在这个例子中，将实现只允许从“\http://example.com”的跨域请求。

Note that CorsPolicyBuilder has a fluent API, so you can chain method calls:

需要注意的是，CorsPolicyBuilder有个fluent API，所以你可以链式调用：

.. literalinclude:: cors/sample/src/CorsExample3/Startup.cs
  :language: csharp
  :lines: 21-24
  :dedent: 12
  :emphasize-lines: 3

The second approach is to define one or more named CORS policies, and then select the policy by name at run time.

第二讲将定义一到两个命名的CORS策略，然后在运行时依据名称选择策略。

.. literalinclude:: cors/sample/src/CorsExample2/Startup.cs
  :language: csharp
  :lines: 9-17,19-26,27
  :dedent: 8

This example adds a CORS policy named "AllowSpecificOrigin". To select the policy, pass the name to UseCors.

这个示例中，添加一个命名为 “AllowSpecificOrigin” 的CORS策略。通过传递名称到UseCors方法来选择策略。

.. _cors-policy-options:

Enabling CORS in MVC
--------------------

在MVC中启用跨域资源共享（CORS）
-------------------------------------

You can alternatively use MVC to apply specific CORS per action, per controller, or globally for all controllers. When using MVC to enable CORS the same CORS services are used, but the CORS middleware is not.

你可以使用MVC选择性地为单个Action、单个Controller、或者所有的Controller全局性的应用指定的CORS。

Per action
^^^^^^^^^^

单个方法（Action）
^^^^^^^^^^^^^^^^^^

To specify a CORS policy for a specific action add the ``[EnableCors]`` attribute to the action. Specify the policy name.

为特定的Action添加 ``[EnableCors]`` 特性以使用特定的CORS策略。并指定策略名称。

.. literalinclude:: cors/sample/src/CorsMvc/Controllers/HomeController.cs
    :language: csharp
    :lines: 7-13
    :dedent: 4

Per controller
^^^^^^^^^^^^^^

单个控制器（Controller）
^^^^^^^^^^^^^^^^^^^^^^^

To specify the CORS policy for a specific controller add the ``[EnableCors]`` attribute to the controller class. Specify the policy name.

为特定的Controller添加 ``[EnableCors]``特性以使用特定的CORS策略。并指定策略名称。

.. literalinclude:: cors/sample/src/CorsMvc/Controllers/HomeController.cs
    :language: csharp
    :lines: 6-8
    :dedent: 4

Globally
^^^^^^^^

全局地（Globally）
^^^^^^^^^^^^^^^^^^

You can enable CORS globally for all controllers by adding the ``CorsAuthorizationFilterFactory`` filter to the global filter collection:

你可以通过将 ``CorsAuthorizationFilterFactory`` 添加到全局过滤器集合来全局性的开启CORS。

.. literalinclude:: cors/sample/src/CorsMvc/Startup.cs
    :language: csharp
    :lines: 13-15,26-30
    :dedent: 8

The precedence order is: Action, controller, global. Action-level policies take precedence over controller-level policies, and controller-level policies take precedence over global policies.

优先级： Action，Controller，Global。

Disable CORS
^^^^^^^^^^^^

禁用CORS
^^^^^^^^^^^^

To disable CORS for a controller or action, use the ``[DisableCors]`` attribute.

要禁用控制器或方法的跨域资源共享，可使用``[DisableCors]`` 特性。

.. literalinclude:: cors/sample/src/CorsMvc/Controllers/HomeController.cs
    :language: csharp
    :lines: 15-19
    :dedent: 4

CORS policy options
-------------------

CORS 策略配置选项
-------------------

This section describes the various options that you can set in a CORS policy.

这一节描述你可以采用的各种CORS策略设置。

- `Set the allowed origins`_
- `Set the allowed HTTP methods`_
- `Set the allowed request headers`_
- `Set the exposed response headers`_
- `Credentials in cross-origin requests`_
- `Set the preflight expiration time`_

For some options it may be helpful to read `How CORS works`_ first.

对个别选项，先阅读 `How CORS works`_ 可能很有帮助。

Set the allowed origins
^^^^^^^^^^^^^^^^^^^^^^^

设置允许范围配置选项
^^^^^^^^^^^^^^^^^^^^^^^

To allow one or more specific origins:

允许一个或多个指定源：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN01
  :end-before: END01
  :dedent: 16

To allow all origins:

允许所有源：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN02
  :end-before: END02
  :dedent: 16

Consider carefully before allowing requests from any origin. It means that literally any website can make AJAX calls to your app.

要允许所有跨域请求前请谨慎考虑。这将使得任意网站都可以直接向你的应用直接发起AJAX请求。

Set the allowed HTTP methods
^^^^^^^^^^^^^^^^^^^^^^^^^^^^

设置允许的HTTP方法
^^^^^^^^^^^^^^^^^^

To specify which HTTP methods are allowed to access the resource.

指定哪些HTTP方法被允许访问资源。

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN03
  :end-before: END03
  :dedent: 16

To allow all HTTP methods:

允许所有HTTP方法：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN04
  :end-before: END04
  :dedent: 16

This affects pre-flight requests and Access-Control-Allow-Methods header.

这影响到预请求和Access-Control-Allow-Methods头信息。

Set the allowed request headers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

设置允许请求的头信息
^^^^^^^^^^^^^^^^^^^^

A CORS preflight request might include an Access-Control-Request-Headers header, listing the HTTP headers set by the application (the so-called "author request headers").

一条CORS预请求可能包含一个Access-Control-Request-Headers头信息，列出了应用的HTTP标头设置（号称“作者请求标头（author request headers）”。

To whitelist specific headers:

白名单指定标头：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN05
  :end-before: END05
  :dedent: 16

To allow all author request headers:

允许所有作者请求标头（author request headers）：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN06
  :end-before: END06
  :dedent: 16

Browsers are not entirely consistent in how they set Access-Control-Request-Headers. If you set headers to anything other than "*", you should include at least "accept", "content-type", and "origin", plus any custom headers that you want to support.

各浏览器对设置Access-Control-Request-Headers信息并非全部一致。如果你设置头信息为“*”以外的任意内容，你需要至少包含“accept”、“content-type”和“orign”，以及其他你想要支持的自定义头信息。

Set the exposed response headers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

设置可见的响应头信息
^^^^^^^^^^^^^^^^^^^^

By default, the browser does not expose all of the response headers to the application. (See http://www.w3.org/TR/cors/#simple-response-header.) The response headers that are available by default are:

默认情况下，浏览器并非暴露应用所有的响应头信息。(参见 http://www.w3.org/TR/cors/#simple-response-header.) 默认可用的响应头信息如下：

- Cache-Control
- Content-Language
- Content-Type
- Expires
- Last-Modified
- Pragma

The CORS spec calls these *simple response headers*. To make other headers available to the application:

CORS规范将这些称为 *简单响应头信息*。使应用的其他头信息可用：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN07
  :end-before: END07
  :dedent: 16

Credentials in cross-origin requests
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

跨域请求中的信任凭证
^^^^^^^^^^^^^^^^^^^^

Credentials require special handling in a CORS request. By default, the browser does not send any credentials with a cross-origin request. Credentials include cookies as well as HTTP authentication schemes. To send credentials with a cross-origin request, the client must set XMLHttpRequest.withCredentials to true.

在CORS中，信任凭证需要特殊处理。默认情况下，浏览器在跨域请求中不发送任何凭证信息。凭证信息包括cookies，同时也是HTTP的验证模式。要在跨域请求中发送凭证信息，客户端必须设置XMLHttpRequest.withCredentials为true。

Using XMLHttpRequest directly:

直接使用XMLHttpRequest

.. code-block:: js

    var xhr = new XMLHttpRequest();
    xhr.open('get', 'http://www.example.com/api/test');
    xhr.withCredentials = true;

In jQuery:

jQuery方式:

.. code-block:: js

    $.ajax({
        type: 'get',
        url: 'http://www.example.com/home',
        xhrFields: {
            withCredentials: true
        }

In addition, the server must allow the credentials. To allow cross-origin credentials:

此外，服务器必须启用凭证。以允许使用跨域凭据：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN08
  :end-before: END08
  :dedent: 16

Now the HTTP response will include an Access-Control-Allow-Credentials header, which tells the browser that the server allows credentials for a cross-origin request.

现在HTTP响应将包含一条Access-Control-Allow-Credentials头信息，以此告知浏览器此服务器允许使用跨域请求凭证机制。

If the browser sends credentials, but the response does not include a valid Access-Control-Allow-Credentials header, the browser will not expose the response to the application, and the AJAX request fails.

如果浏览器发送凭证信息，但响应中不包含有效的Access-Control-Allow-Credentials头信息，浏览器将不显示应用响应内容而是产生AJAX请求错误。

Be very careful about allowing cross-origin credentials, because it means a website at another domain can send a logged-in user’s credentials to your app on the user’s behalf, without the user being aware. The CORS spec also states that setting origins to "*" (all origins) is invalid if the Access-Control-Allow-Credentials header is present.

对允许使用跨域凭据，一定要格外注意，因为这意味着网站可以被从其他域在用户本身不知情的情况下向你的应用发送一个已登陆用户的凭证来代表该用户。CORS规范也指出在存在Access-Control-Allow-Credentials 头信息的情况下将跨域策略设置为“*”（所有源）是无效的。

Set the preflight expiration time
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

预请求有效期设置
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The Access-Control-Max-Age header specifies how long the response to the preflight request can be cached. To set this header:

Access-Control-Max-Age指示预请求响应被缓存时长。设置头信息为：

.. literalinclude:: cors/sample/src/CorsExample4/Startup.cs
  :language: csharp
  :start-after: BEGIN09
  :end-before: END09
  :dedent: 16

.. _cors-how-cors-works:

How CORS works
--------------

CORS工作原理
---------------

This section describes what happens in a CORS request, at the level of the HTTP messages. It’s important to understand how CORS works, so that you can configure the your CORS policy correctly, and troubleshoot if things don’t work as you expect.

这一节描述在HTTP消息层面，一个CORS请求引发了什么。理解CORS运行原理是非常重要的，以致于你可以正确的配置你的CORS策略，且在不能如期运行的时候为你答疑解惑。

The CORS specification introduces several new HTTP headers that enable cross-origin requests. If a browser supports CORS, it sets these headers automatically for cross-origin requests; you don’t need to do anything special in your JavaScript code.

CORS规范介绍了多个新的关于使用跨域请求的HTTP头信息。如果浏览器支持CORS，它将为跨域请求自动设置这些头信息；你不需要在JavaScript中做任何事。

Here is an example of a cross-origin request. The "Origin" header gives the domain of the site that is making the request::

这是一个跨域请求的示例。“Origin”标头显示了请求来源站点的域名::

    GET http://myservice.azurewebsites.net/api/test HTTP/1.1
    Referer: http://myclient.azurewebsites.net/
    Accept: */*
    Accept-Language: en-US
    Origin: http://myclient.azurewebsites.net
    Accept-Encoding: gzip, deflate
    User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)
    Host: myservice.azurewebsites.net

If the server allows the request, it sets the Access-Control-Allow-Origin header. The value of this header either matches the Origin header, or is the wildcard value "*", meaning that any origin is allowed.::

如果服务器允许请求，它将设置Access-Control-Allow-Origin header。头信息的值可以是匹配Origin标头，也可以是意味着任意域均被允许的通配符值“*”。

    HTTP/1.1 200 OK
    Cache-Control: no-cache
    Pragma: no-cache
    Content-Type: text/plain; charset=utf-8
    Access-Control-Allow-Origin: http://myclient.azurewebsites.net
    Date: Wed, 20 May 2015 06:27:30 GMT
    Content-Length: 12

    Test message

If the response does not include the Access-Control-Allow-Origin header, the AJAX request fails. Specifically, the browser disallows the request. Even if the server returns a successful response, the browser does not make the response available to the client application.

如果响应不包括Access-Control-Allow-Origin头信息，则AJAX请求失败。具体而言，浏览器不允许请求。尽管服务器返回了成功的响应，但对客户端应用浏览器依然会将响应当作不可用。

Preflight Requests
^^^^^^^^^^^^^^^^^^

预请求
^^^^

For some CORS requests, the browser sends an additional request, called a "preflight request", before it sends the actual request for the resource.

对个别CORS请求，在发送正式请求前，浏览器会发送额外的请求，被称为“预请求”。

The browser can skip the preflight request if the following conditions are true:

在以下情况下，浏览器可以省略预请求：

- The request method is GET, HEAD, or POST, and
- The application does not set any request headers other than Accept, Accept-Language, Content-Language, Content-Type, or Last-Event-ID, and
- The Content-Type header (if set) is one of the following:

- 请求方法为GET、HEAD、或POST，且
- 应用不设置任何除Accept、Accept-Language、Content-Language、Content-Type、或Last-Event-ID之外的请求头信息，且
- Content-Type头信息（如果存在）值为以下之一：

  - application/x-www-form-urlencoded
  - multipart/form-data
  - text/plain

The rule about request headers applies to headers that the application sets by calling setRequestHeader on the XMLHttpRequest object. (The CORS specification calls these "author request headers".) The rule does not apply to headers the browser can set, such as User-Agent, Host, or Content-Length.

有关请求头信息的规则可适用于通过调用XMLHttpRequest对象的setRequestHeader方法构建的应用。（CORS规范将此称为“作者请求标头（author request headers）”。）

Here is an example of a preflight request::

这是一个预请求的例子::

    OPTIONS http://myservice.azurewebsites.net/api/test HTTP/1.1
    Accept: */*
    Origin: http://myclient.azurewebsites.net
    Access-Control-Request-Method: PUT
    Access-Control-Request-Headers: accept, x-my-custom-header
    Accept-Encoding: gzip, deflate
    User-Agent: Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)
    Host: myservice.azurewebsites.net
    Content-Length: 0

The pre-flight request uses the HTTP OPTIONS method. It includes two special headers:

预请求使用HTTP的OPTIONS方法。它包含两项特定头信息：

- Access-Control-Request-Method: The HTTP method that will be used for the actual request.

- Access-Control-Request-Method: 此HTTP方法将被用在正式请求。

- Access-Control-Request-Headers: A list of request headers that the application set on the actual request. (Again, this does not include headers that the browser sets.)

- Access-Control-Request-Headers：应用设置的正式的请求标头。（另外，这不包括浏览器设置的头信息。）

Here is an example response, assuming that the server allows the request::

这是一个假定服务器允许请求的示例::

    HTTP/1.1 200 OK
    Cache-Control: no-cache
    Pragma: no-cache
    Content-Length: 0
    Access-Control-Allow-Origin: http://myclient.azurewebsites.net
    Access-Control-Allow-Headers: x-my-custom-header
    Access-Control-Allow-Methods: PUT
    Date: Wed, 20 May 2015 06:33:22 GMT

The response includes an Access-Control-Allow-Methods header that lists the allowed methods, and optionally an Access-Control-Allow-Headers header, which lists the allowed headers. If the preflight request succeeds, the browser sends the actual request, as described earlier.

响应包括一条含有允许请求方法列表的Access-Control-Allow-Methods头信息，和按需使用的一条含有允许请求标头列表的Access-Control-Allow-Headers头信息。如果预请求成功，浏览器则发送如前描述的正式请求。
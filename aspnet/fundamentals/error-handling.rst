:version: 1.0.0-rc1

错误处理
=============

Error Handling
==============

作者： `Steve Smith`_  

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_ 

校对 `高嵩(jack2gs) <https://github.com/jack2gs>`_  

When errors occur in your ASP.NET app, you can handle them in a variety of ways, as described in this article.

当你的ASP.NET应用发生错误的时候, 你可以采用本文所述的各种方法来处理这些问题。

.. contents:: 章节
	:local:
	:depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/error-handling/sample>`_

`查看或者下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/error-handling/sample>`_

Configuring an Exception Handling Page
--------------------------------------

配置错误处理页面
--------------------

You configure the pipeline for each request in the ``Startup`` class's ``Configure()`` method (learn more about :doc:`startup`). You can add a simple exception page, meant only for use during development, very easily. All that's required is to add a dependency on ``Microsoft.AspNetCore.Diagnostics`` to the project and then add one line to ``Configure()`` in ``Startup.cs``:

你在 ``Startup`` 类的 ``Configure()`` 方法中为每一个请求配置管道 (更多内容请参考 :doc:`startup`)。 你可以轻松的添加一个仅仅适用于开发阶段的简单异常页面。只需要在项目中添加 ``Microsoft.AspNetCore.Diagnostics`` 依赖，并且添加一行代码到 ``Startup.cs`` 的 ``Configure()`` 方法里面：

.. literalinclude:: error-handling/sample/src/ErrorHandlingSample/Startup.cs
	:language: c#
	:lines: 21-29
	:dedent: 8
	:emphasize-lines: 6,8

The above code includes a check to ensure the environment is development before adding the call to ``UseDeveloperExceptionPage``. This is a good practice, since you typically do not want to share detailed exception information about your application publicly while it is in production. :doc:`Learn more about configuring environments <environments>`.

The sample application includes a simple mechanism for creating an exception:

以上代码包含一个检查，以确保添加调用 ``UseDeveloperExceptionPage`` 的环境是开发环境。这是一个好的实践，因为你通常情况下并不希望在应用程序已经处于生产环境的情况下，将你的应用程序的详细异常信息对外公开. :doc:`详细了解如何配置环境 <environments>`。

示例应用程序包括一个创建异常的简单机制的例子：

.. literalinclude:: error-handling/sample/src/ErrorHandlingSample/Startup.cs
	:language: c#
	:lines: 58-77
	:dedent: 8
	:emphasize-lines: 5-8

If a request includes a non-empty querystring parameter for the variable ``throw`` (e.g. a path of ``/?throw=true``), an exception will be thrown. If the environment is set to ``Development``, the developer exception page is displayed:

如果请求中包含一个变量名为 ``throw`` 的非空查询字符串 (例如，路径： ``/?throw=true``), 那么就会抛出一个异常。如果环境被设置为 ``Development`` ， 开发者异常页面将会被显示：

.. image:: error-handling/_static/developer-exception-page.png

When not in development, it's a good idea to configure an exception handler path using the ``UseExceptionHandler`` middleware:

当不在开发模式下, 建议使用 ``UseExceptionHandler`` 方法来配置一个错误处理路径：

.. code-block:: c#

  app.UseExceptionHandler("/Error");


Using the Developer Exception Page
----------------------------------

The developer exception page displays useful diagnostics information when an unhandled exception occurs within the web processing pipeline. The page includes several tabs with information about the exception that was triggered and the request that was made. The first tab includes a stack trace:

使用开发者异常页面
----------------------

当Web请求中发生无法捕获异常的时候，开发者异常页面会显示有用的调试信息。页面包含几个选项卡页面来显示Web请求中引发的异常信息。 第一个选项卡页面包含错误堆栈跟踪信息：

.. image:: error-handling/_static/developer-exception-page.png

The next tab shows the query string parameters, if any:

第二个选项卡页面显示查询字符串信息，如果有的话：

.. image:: error-handling/_static/developer-exception-page-query.png

In this case, you can see the value of the ``throw`` parameter that was passed to this request. This request didn't have any cookies, but if it did, they would appear on the Cookies tab. You can see the headers that were passed in the last tab:

在这个案例里面，你可以看到 ``throw`` 参数的值被传递到了请求。这个请求不包含任何cookies，但是如果有任何cookies，他们的值会显示在cookies选项卡页面。你可以在最后一个选项卡页面查看到头信息：

.. image:: error-handling/_static/developer-exception-page-headers.png

.. _status-code-pages:

Configuring Status Code Pages
-----------------------------

By default, your app will not provide a rich status code page for HTTP status codes such as 500 (Internal Server Error) or 404 (Not Found). You can configure the ``StatusCodePagesMiddleware`` adding this line to the ``Configure`` method:

配置状态码页面
----------------------

在系统默认情况下，你的应用程序无法为Http状态码返回（例如：500 (服务器内部错误) or 404 (文件无法找到)）提供一个富文本的HTTP状态码页面。你可以在 ``Configure`` 方法中加入一行 ``StatusCodePagesMiddleware`` 代码：

.. code-block:: c#

  app.UseStatusCodePages();

By default, this middleware adds very simple, text-only handlers for common status codes. For example, the following is the result of a 404 Not Found status code:

在系统默认情况下，系统会为普通的http状态码添加一个非常简单纯文本的处理，例如，下面是404无法找到文件状态码返回的结果。

.. image:: error-handling/_static/default-404-status-code.png

The middleware supports several different extension methods. You can pass it a custom lamdba expression:

中间件提供不同的扩展方法，你也可以使用自定义lamba表达式来配置参数:

.. code-block:: c#

  app.UseStatusCodePages(context => 
    context.HttpContext.Response.SendAsync("Handler, status code: " +
    context.HttpContext.Response.StatusCode, "text/plain"));

Alternately, you can simply pass it a content type and a format string:

另外, 你也可以直接简单的传递一个内容类型和一个格式化字符串:

.. code-block:: c#

  app.UseStatusCodePages("text/plain", "Response, status code: {0}");

The middleware can handle redirects (with either relative or absolute URL paths), passing the status code as part of the URL:

中间件也能处理重定向请求 (无论是绝对路径还是相对路径), 把状态码作为URL的一部分进行传递:

.. code-block:: c#

  app.UseStatusCodePagesWithRedirects("~/errors/{0}");

在上面的案例中, 客户端浏览器遇到 ``302 / Found``状态码返回时，会重定向到指定的页面.

Alternately, the middleware can re-execute the request from a new path format string:

另外，中间件也提供设置一个新的路径字符串的方式来重新执行请求。

.. code-block:: c#

  app.UseStatusCodePagesWithReExecute("/errors/{0}");

In the above case, the client browser will see a ``302 / Found`` status and will redirect to the URL provided.

Alternately, the middleware can re-execute the request from a new path format string:

方法 ``UseStatusCodePagesWithReExecute`` 会返回原始的浏览器状态码页面，但是也会执行路径中指定的处理程序。

如果你需要对某些请求禁止状态码页面, 可以使用以下代码:

.. code-block:: c#

  var statusCodePagesFeature = context.Features.Get<IStatusCodePagesFeature>();
  if (statusCodePagesFeature != null)
  {
    statusCodePagesFeature.Enabled = false;
  }

Limitations of Exception Handling During Client-Server Interaction
------------------------------------------------------------------

Web apps have certain limitations to their exception handling capabilities because of the nature of disconnected HTTP requests and responses. Keep these in mind as you design your app's exception handling behavior.

#. Once the headers for a response have been sent, you cannot change the response's status code, nor can any exception pages or handlers run. The response must be completed or the connection aborted.
#. If the client disconnects mid-response, you cannot send them the rest of the content of that response.
#. There is always the possibility of an exception occuring one layer below your exception handling layer.
#. Don't forget, exception handling pages can have exceptions, too. It's often a good idea for production error pages to consist of purely static content.

Following the above recommendations will help ensure your app remains responsive and is able to gracefully handle exceptions that may occur.

错误处理在CS交互模式下的限制
-----------------------------------

Web应用在错误处理功能上因为断开HTTP请求和响应的特性有些特别的限制，有的应用程序，在你设计错误处理行为时请注意以下几点。

#. 一旦请求文件头发送出去以后，你就无法再修改响应的状态码了，无论是任何异常页面或处理程序都无法执行。响应必须完成或者连接中断退出。
#. 如果客户端在响应中期断开，你无法把当前响应的剩余内容发送给客户端。
#. 在你的错误处理层之下，总是有可能存在有例外的一层。
#. 不要忘了，错误处理页面也会产生异常. 生产环境异常页面采用纯静态页面是个不错的建议。

遵从上述建议将有助于确保您的应用程序保持响应，并且能很好地处理应用程序可能发生的异常。

Server Exception Handling
-------------------------

In addition to the exception handling logic in your app, the server hosting your app will perform some exception handling. If the server catches an exception before the headers have been sent it will send a 500 Internal Server Error response with no body. If it catches an exception after the headers have been sent it must close the connection. Requests that are not handled by your app will be handled by the server, and any exception that occurs will be handled by the server's exception handling. Any custom error pages or exception handling middleware or filters you have configured for your app will not affect this behavior.

服务器错误处理
-------------------------

除了你的应用程序中的错误处理逻辑，托管应用程序的服务器也将执行一些错误处理。如果服务器在头信息发送出去之前捕获到异常，它会发出不带主体的500内部服务器错误响应。如果在头文件发送出去之后捕获到异常必须关闭连接。那些不是被你的应用程序处理的请求将被服务器处理，并且发生的任何异常将被服务器的错误处理机制来处理。任何在你的应用程序里面配置好自定义错误页、错误处理中间件、过滤器都无法影响此行为。

.. _startup-error-handling:

Startup Exception Handling
--------------------------

One of the trickiest places to handle exceptions in your app is during its startup. Only the hosting layer can handle exceptions that take place during app startup. Exceptions that occur in your app's startup can also impact server behavior. For example, to enable SSL in Kestrel, one must configure the server with ``KestrelServerOptions.UseHttps()``. If an exception happens before this line in ``Startup``, then by default hosting will catch the exception, start the server, and display an error page on the non-SSL port. If an exception happens after that line executes, then the error page will be served over HTTPS instead.

Startup 错误处理
--------------------------

 处理异常最为最棘手的地方是在你的应用程序的启动的时候。只有承载层可以处理应用程序的启动过程中发生的异常。应用程序启动时发生的异常也会影响服务器的行为。例如，要启用SSL in Kestrel，有些必须用 ``KestrelServerOptions.UseHttps()`` 配置服务器。如果一个异常在 ``Startup`` 代码行之前发生，则默认情况下托管将捕获异常，并启动服务器，然后在非SSL端口上显示一个错误页面。如果有异常情况发生该行执行后， 则错误页面将通过HTTPS服务生效。

ASP.NET MVC Error Handling
--------------------------

:doc:`MVC </mvc/index>` apps have some additional options when it comes to handling errors, such as configuring exception filters and performing model validation.

ASP.NET MVC 错误处理
-----------------------

:doc:`MVC </mvc/index>` 当应用程序涉及到错误处理的时候有一些额外的选项，比如配置异常过滤器以及执行模型验证.

Exception Filters
^^^^^^^^^^^^^^^^^

Exception filters can be configured globally or on a per-controller or per-action basis in an :doc:`MVC </mvc/index>` app. These filters handle any unhandled exception that occurs during the execution of a controller action or another filter, and are not called otherwise. Exception filters are detailed in :doc:`filters </mvc/controllers/filters>`.

.. tip:: Exception filters are good for trapping exceptions that occur within MVC actions, but they're not as flexible as error handling middleware. Prefer middleware for the general case, and use filters only where you need to do error handling *differently* based on which MVC action was chosen.

异常过滤器
^^^^^^^^^^^^^

异常过滤器可以在 :doc:`MVC </mvc/index>` 应用程序的全局范围内或者每个Controller或者每个Action的基础在进行配置。这些过滤器会处理controller action或其他过滤器的执行过程中发生的任何未处理的异常，其他情况这不会被调用。异常过滤器更多内容请见 :doc:`过滤器 </mvc/controllers/filters>`。

.. tip:: 异常过滤器诱捕MVC Action中发生的异常的一个良好的机制，但他们不如错误处理中间件灵活。一般情况下尽可能使用中间件，只有当在你需要在处理异常的时候需要特别指定某些MVC action的时候，过滤器才被建议使用。

Handling Model State Errors
^^^^^^^^^^^^^^^^^^^^^^^^^^^

:doc:`Model validation </mvc/models/validation>` occurs prior to each controller action being invoked, and it is the action method’s responsibility to inspect ``ModelState.IsValid`` and react appropriately. In many cases, the appropriate reaction is to return some kind of error response, ideally detailing the reason why model validation failed. 

Some apps will choose to follow a standard convention for dealing with model validation errors, in which case a :doc:`filter </mvc/controllers/filters>` may be an appropriate place to implement such a policy. You should test how your actions behave with valid and invalid model states (learn more about :doc:`testing controller logic </mvc/controllers/testing>`).

处理模型状态错误
^^^^^^^^^^^^^^^^^^

:doc:`模型验证 </mvc/models/validation>` 发生模型验证每个controller action被调用之前，Action方法的职责是检查 ``ModelState.IsValid`` 并作出适当的交互反映。在大部分情况下，特定的交互会返回特定的错误的响应，最好详细说明模型验证失败的原因。

有些应用程序会选择遵循标准惯例处理模型验证错误，在这种情况下，过滤器可以作为某些策略的实现场所。您应该测试你的Action是否与有效和无效的模型状态有关联（了解更多有关 :doc:`测试controller逻辑</mvc/controllers/testing>`）的行为。

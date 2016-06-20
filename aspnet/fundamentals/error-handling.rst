异常处理
=============

作者： `Steve Smith`_  
翻译：  谢炀(kiler)   
校对：

当你的ASP.NET应用发生错误的时候, 你可以采用本文所述的各种方法来处理这些问题。

.. contents:: 章节
	:local:
	:depth: 1

`查看或者下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/error-handling/sample>`_

配置异常处理页面
--------------------

你在 ``Startup`` 类的 ``Configure()`` 方法中为每一个请求配置管道 (更多内容请参考 :doc:`startup`)。 你可以轻松的添加一个仅仅适用于开发阶段的简单异常页面。所有上面所述的一切需要在项目中添加 ``Microsoft.AspNet.Diagnostics`` 依赖，并且添加一行代码到 ``Startup.cs`` 的 ``Configure()`` 方法里面：

.. literalinclude:: error-handling/sample/src/ErrorHandlingSample/Startup.cs
	:language: c#
	:lines: 21-29
	:dedent: 8
	:emphasize-lines: 6,8

以上代码包含一段检测代码来确认在调用 ``UseDeveloperExceptionPage`` 之前环境是否就绪。这是一个好的方法，因为你通常情况下并不希望在应用程序已经处于生产环境的情况下，将你的应用程序的详细异常信息对外公开. :doc:`详细了解如何配置环境 <environments>`。

示例应用程序包括创建一个异常简单的机制的例子：

.. literalinclude:: error-handling/sample/src/ErrorHandlingSample/Startup.cs
	:language: c#
	:lines: 58-63
	:dedent: 12
	:emphasize-lines: 3-6

如果请求中包含一个变量名为 ``throw`` 的非空查询字符串 (例如，路径： ``/?throw=true``), 那么就会抛出一个异常。如果环境被设置为 ``Development`` ， 开发者异常页面将会被显示：

.. image:: error-handling/_static/developer-exception-page.png

当不在开发模式下, 建议使用 ``UseExceptionHandler`` 方法来配置一个异常处理路径：

.. code-block:: c#

  app.UseExceptionHandler("/Error");

使用开发者异常页面
----------------------

当Web请求中发生无法捕获异常的时候，开发者异常页面会显示有用的调试信息。页面包含几个选项卡页面来显示Web请求中引发的异常信息。 第一个选项卡页面包含错误堆栈跟踪信息：

.. image:: error-handling/_static/developer-exception-page.png

第二个选项卡页面显示查询字符串信息，如果有的话：

.. image:: error-handling/_static/developer-exception-page-query.png

在这个案例里面，你可以看到 ``throw`` 参数的值被传递到了请求。这个请求不包含任何cookies，但是如果有任何cookies，他们的值会显示在cookies选项卡页面。你可以在最后一个选项卡页面查看到头文件信息：

.. image:: error-handling/_static/developer-exception-page-headers.png

.. _status-code-pages:

配置状态码页面
----------------------

在系统默认情况下，你的应用程序无法为Http状态码返回（例如：500 (服务器内部错误) or 404 (文件无法找到)）提供一个富文本的HTTP状态码页面。你可以在 ``Configure`` 方法中加入一行 ``StatusCodePagesMiddleware`` 代码：

.. code-block:: c#

  app.UseStatusCodePages();


在系统默认情况下，系统会为普通的http状态码添加一个非常简单纯文本的处理，例如，下面是404无法找到文件状态码返回的结果。

.. image:: error-handling/_static/default-404-status-code.png

中间件提供不同的扩展方法，你也可以使用自定义lamba表达式来配置参数:

.. code-block:: c#

  app.UseStatusCodePages(context => 
    context.HttpContext.Response.SendAsync("Handler, status code: " +
    context.HttpContext.Response.StatusCode, "text/plain"));

另外, 你也可以直接简单的传递一个内容类型和一个格式化字符串:

.. code-block:: c#

  app.UseStatusCodePages("text/plain", "Response, status code: {0}");

中间件也能处理重定向请求 (无论是绝对路径还是相对路径), 把状态码作为URL的一部分进行传递:

.. code-block:: c#

  app.UseStatusCodePagesWithRedirects("~/errors/{0}");

在上面的案例中, 客户端浏览器遇到 ``302 / Found``状态码返回时，会重定向到指定的页面.

Alternately, the middleware can re-execute the request from a new path format string:

另外，中间件也提供设置一个新的路径字符串的方式来重新执行请求。

.. code-block:: c#

  app.UseStatusCodePagesWithReExecute("/errors/{0}");

方法 ``UseStatusCodePagesWithReExecute`` 会返回原始的浏览器状态码页面，但是也会同时执行设置页面里面的代码。

如果你需要对某些请求禁止状态码页面, 可以使用以下代码:

.. code-block:: c#

  var statusCodePagesFeature = context.Features.Get<IStatusCodePagesFeature>();
  if (statusCodePagesFeature != null)
  {
    statusCodePagesFeature.Enabled = false;
  }

异常处理在CS交互模式下的限制
-----------------------------------

Web应用在异常处理功能上因为断开HTTP请求和响应的特性有些特别的限制，在你设计有的应用程序的异常处理行为是请注意以下几点。

#. 一旦请求文件头发送出去以后，你就无法再修改响应的状态码了，无论是任何异常页面或处理程序都无法执行。响应必须完成或者连接中断退出。
#. 如果客户端在响应中期断开，你无法把当前响应的剩余内容发送给客户端。
#. 在你的异常处理层之下，总是有可能存在有例外的一层。
#. 不要忘了，异常处理页面也会产生异常. 生产环境异常页面采用纯静态页面是个不错的建议。

基于上述建议将有助于确保您的应用程序保持响应，并且能很好地处理应用程序可能发生的异常。

服务器异常处理
-------------------------

除了你的应用程序中的异常处理逻辑，托管应用程序的服务器也将执行一些异常处理。如果服务器在头文件发送出去之前捕获到异常，它会发出不带主体的500内部服务器错误响应。如果在头文件发送出去之后捕获到异常必须关闭连接。请求不是被你的应用程序处理而是被服务器处理，而发生任何异常将被服务器的异常处理机制来处理。任何在你的应用程序里面配置好自定义错误页、异常处理中间件、过滤器都无法影响此行为。

.. _startup-error-handling:

Startup 异常处理
--------------------------

处理异常最为最棘手的地方是在你的应用程序的启动的时候。只有承载层可以处理应用程序的启动过程中发生的异常。发生在你的应用程序的启动异常也会影响服务器的行为。例如，要启用SSL in Kestrel，有些必须用 ``app.UseKestrelHttps()`` 配置服务器。如果一个异常在 ``Startup`` 代码行之前发生，则默认情况下托管将捕获异常，启动服务器，在非SSL端口上显示一个错误页面。如果有异常情况发生该行执行后，则错误页面将通过HTTPS服务来代替。

ASP.NET MVC 异常处理
-----------------------

:doc:`MVC </mvc/index>` 当应用程序涉及到错误处理的时候有一些额外的选项，比如配置异常过滤器以及执行模型验证.

异常过滤器
^^^^^^^^^^^^^

异常过滤器可以在 :doc:`MVC </mvc/index>` 应用程序的全局范围内或者每个Controller或者每个Action的基础在进行配置。这些过滤器会处理controller action或其他过滤器的执行过程中发生的任何未处理的异常，其他情况这不会被调用。异常过滤器更多内容请见 :doc:`过滤器 </mvc/controllers/filters>`。

.. tip:: 异常过滤器诱捕MVC Action中发生的异常的一个良好的机制，但他们不如异常处理中间件灵活。一般情况下尽可能使用中间件，只有当在你需要在处理异常的时候需要特别指定某些MVC action的时候，过滤器才被建议使用。

处理模型状态错误
^^^^^^^^^^^^^^^^^^

:doc:`模型验证 </mvc/models/validation>` 发生模型验证每个controller action被调用之前，Action方法的职责是检查 ``ModelState.IsValid`` 并作出适当的交互反映。在大部分情况下，特定的交互会返回特定的错误的响应，最好详细说明为什么模型验证失败的原因。

有些应用程序会选择遵循标准惯例处理模型验证错误，在这种情况下，过滤器可以作为某些策略的实现场所。您应该测试你的Action是否与有效和无效的模型状态有关联（了解更多有关 :doc:`测试controller逻辑</mvc/controllers/testing>`）的行为。

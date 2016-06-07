Diagnostics
============

诊断
=======

作者： `Steve Smith`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

ASP.NET Core includes a number of new features that can assist with diagnosing problems.

ASP.NET Core 包含了许多有助于诊断问题的新特性。

.. contents:: Sections
  :local:
  :depth: 1

The developer error page
------------------------

开发者的错误页面
------------------------

You can view the details of unhandled exceptions by specifying a developer error page. This topic is described in :doc:`error-handling`.

你可以通过指定一个开发者的错误页面来查看未处理异常的细节。关于这个话题可以参考 :doc:`error-handling` 。


The welcome page
----------------

欢迎页面
----------------

Another extension method you may find useful, especially when you're first spinning up a new ASP.NET Core application, is the ``UseWelcomePage()`` method. Add it to ``Configure()`` like so:

``UseWelcomePage()`` 方法是另一个对你而言可能有用的扩展方法，尤其是当你第一次启动 ASp.NET Core 应用程序。就像这样把它加进 ``Configure()`` ：

.. code-block:: c#

    app.UseWelcomePage();

Once included, this will handle all requests (by default) with a cool hello world page that uses embedded images and fonts to display a rich view, as shown here:

引入之后，它将处理所有默认请求为显示一个酷酷哒的 hello world 页面（包含了图片和字体的丰富视图），如下图：

.. image:: diagnostics/_static/welcome-page.png

You can optionally configure the welcome page to only respond to certain paths. The code shown below will configure the page to only be displayed for the ``/welcome`` path (other paths will be ignored, and will fall through to other handlers):

你可以只为某些路径设定为返回欢迎页面。根据下方代码的配置，只有 ``/welcome`` 路径才会显示欢迎页面（其他路径将被忽略，并进入其它处理程序）。

.. code-block:: c#

  app.UseWelcomePage("/welcome");

Glimpse
-------

Glimpse is a plug-in that provides a tremendous amount of insight into your ASP.NET Core application, directly from the browser. Glimpse can be added to your in app in just a few simple steps:

- Add a dependency on the "Glimpse" package in ``project.json``
- Call ``services.AddGlimpse`` in ``ConfigureServices``
- Call ``app.UseGlimpse`` in ``Configure``

Glimpse 是一个插件，它使得访客可以直接从浏览器中获得 ASP.NET Core 应用程序的大量内部信息。只需要简单几步即可让你的应用程序用上 Glimpse：

- 在 ``project.json`` 中增加 Glimpse 包依赖
- 在 ``ConfigureServices`` 中调用 ``services.AddGlimpse``
- 在 ``Configure`` 中调用 ``app.UseGlimpse``

Run your app on localhost, and you should see Glimpse information bar at the bottom of the browser window. `View a walkthrough of setting up Glimpse for ASP.NET Core <http://blog.getglimpse.com/2015/11/19/installing-glimpse-v2-beta1/>`_.

在本地主机（localhost）运行应用程序，你可以在浏览器窗口的底部看到 Glimpse 信息栏。`查看在 ASP.NET Core 中配置 Glimpse 的演示 <http://blog.getglimpse.com/2015/11/19/installing-glimpse-v2-beta1/>`_ 。


Logging
-------

日志
-------

ASP.NET Core includes a great deal of built-in logging that can assist with diagnosing many app issues. In many cases, just enabling logging is sufficient to provide the diagnostic information developers need to identify problems with their app. Logging is enabled and configured in your app's ``Startup`` class.

ASP.NET Core 中包含了大量内建的日志以便于诊断许多应用程序问题。在许多情况下，只需启用日志便能提供开发人员足够的诊断信息以确定应用程序问题的所在。可以在 ``Startup`` 类中启用并配置日志。

:doc:`Learn more about configuring logging in your ASP.NET Core app <logging>`.

:doc:`了解更多关于在 ASP.NET Core 应用程序中配置日志的资料 <logging>`.

.. note:: `Application Insights <https://azure.microsoft.com/en-us/documentation/articles/app-insights-asp-net-five/>`_ can provide production diagnostic information in a cloud-based, searchable format.

.. note:: `Application Insights <https://azure.microsoft.com/en-us/documentation/articles/app-insights-asp-net-five/>`_ 可以提供具有可检索格式的基于云的生产环境诊断信息。
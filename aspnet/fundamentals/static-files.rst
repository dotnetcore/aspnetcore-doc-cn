.. _fundamentals-static-files:

静态文件处理
=============

作者： `Rick Anderson`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： `谢炀(kiler398) <http://github.com/kiler398>`_

Static files, such as HTML, CSS, image, and JavaScript, are assets that an ASP.NET Core app can serve directly to clients.

静态文件（static files），诸如 HTML、CSS、图片和 JavaScript 之类的资源会被 ASP.NET Core 应用直接提供给客户端。

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/static-files/sample>`__

`查看或下载样式代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/static-files/sample>`__

.. contents:: 章节:
  :local:
  :depth: 1

静态文件服务
--------------------

Static files are typically located in the ``web root`` (*<content-root>/wwwroot*) folder. See Content root and Web root in  :doc:`/intro` for more information. You generally set the content root to be the current directory so that your project's ``web root`` will be found while in development.

静态文件通常位于 `web root`` (*<content-root>/wwwroot*)文件夹下。
更多有关 Content root 或 Web root 的信息请访问 :doc:`/intro` 。
你通常会把项目对当前目录设置为 Content root，这样项目的 ``web root`` 就可以在开发阶段被明确。

.. literalinclude:: ../../common/samples/WebApplication1/src/WebApplication1/Program.cs
  :language: c#
  :lines: 12-22
  :emphasize-lines: 5
  :dedent: 8

Static files can be stored in any folder under the ``web root`` and accessed with a relative path to that root. For example, when you create a default Web application project using Visual Studio, there are several folders created within the *wwwroot*  folder - *css*, *images*, and *js*. The URI to access an image in the *images* subfolder:

静态文件能够被保存在网站根目录下的任意文件夹内，并通过相对根的路径来访问。比方说，当你通过 Visual Studio 创建了个默认的 Web 应用程序项目，在 *wwwroot* 目录下会多出几个文件夹：*css*、*images* 以及 *js* 文件夹。形如下例的 URL 能够直接访问 ``images`` 目录下的图片：

- \http://<app>/images/<imageFileName>
- \http://localhost:9189/images/banner3.svg

In order for static files to be served, you must configure the :doc:`middleware` to add static files to the pipeline. The static file middleware can be configured by adding a dependency on the *Microsoft.AspNetCore.StaticFiles* package to your project and then calling the :dn:method:`~Microsoft.AspNetCore.Builder.StaticFileExtensions.UseStaticFiles` extension method from ``Startup.Configure``:

为了能够启用静态文件服务，你必须配置中间件（:doc:`middleware`），把静态文件中间件加入到管道内。静态文件中间件能够通过下述方法来配置：在你的项目中增加 *Microsoft.AspNetCore.StaticFiles* 包依赖，然后从 ``Startup.Configure`` 调用 :dn:method:`~Microsoft.AspNetCore.Builder.StaticFileExtensions.UseStaticFiles` 扩展方法：

.. literalinclude:: static-files/sample/StartupStaticFiles.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :emphasize-lines: 3
  :dedent: 8

``app.UseStaticFiles();`` makes the files in ``web root`` (*wwwroot* by default) servable. Later I'll show how to make other directory contents servable with ``UseStaticFiles``.

``app.UseStaticFiles();`` 使得 ``web root`` (*wwwroot* by default) 下的文件可以被访问。随后我将展示如何通过使用 ``UseStaticFiles`` 将其他目录下的内容也向外提供服务。

You must include "Microsoft.AspNetCore.StaticFiles" in the *project.json* file.

你必须在 *project.json* 文件中包含 “Microsoft.AspNetCore.StaticFiles”。

.. note:: ``web root`` defaults to the *wwwroot* directory, but you can set the ``web root`` directory with :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseWebRoot`. See :doc:`/intro` for more information.

.. note:: ``web root`` 的默认目录是 *wwwroot*，但你可以通过 :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseWebRoot` 来设置 ``web root`` 。具体可参考 :doc:`/intro` 。

Suppose you have a project hierarchy where the static files you wish to serve are outside the ``web root``. For example:

假设你有一个有层次结构的项目，你希望其中静态文件的位于 ``web root`` 的外部，比如：

  - wwwroot

    - css
    - images
    - ...

  - MyStaticFiles

    - test.png

For a request to access *test.png*, configure the static files middleware as follows:

对于访问 *test.png* 的请求，可以如此配置静态文件中间件：

.. literalinclude:: static-files/sample/StartupTwoStaticFiles.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :emphasize-lines: 5-10
  :dedent: 8

A request to ``http://<app>/StaticFiles/test.png`` will serve the *test.png* file.

在请求 ``http://<app>/StaticFiles/test.png`` 时，就能访问到 *test.png* 文件。


静态文件授权
-------------------------

The static file module provides **no** authorization checks. Any files served by it, including those under *wwwroot* are publicly available. To serve files based on authorization:

静态文件模块并**不**提供授权检查。任何通过该模块提供访问的文件，包括位于 *wwwroot* 下的文件都是公开的。为了给文件提供授权：

- Store them outside of *wwwroot* and any directory accessible to the static file middleware **and**
- Serve them through a controller action, returning a :dn:class:`~Microsoft.AspNetCore.Mvc.FileResult` where authorization is applied

- 将文件保存在 *wwwroot* 之外并将目录设置为可悲静态文件中间件访问到，**同时**
- 通过一个控制器的 Action 来访问它们，通过授权后返回 :dn:class:`~Microsoft.AspNetCore.Mvc.FileResult`


允许直接浏览目录
---------------------------

Directory browsing allows the user of your web app to see a list of directories and files within a specified directory. Directory browsing is disabled by default for security reasons (see Considerations_). To enable directory browsing, call the :dn:method:`~Microsoft.AspNetCore.Builder.DirectoryBrowserExtensions.UseDirectoryBrowser` extension method from  ``Startup.Configure``:

目录浏览允许网站用户看到指定目录下的目录和文件列表。在 ``Startup.Configure`` 中调用 :dn:method:`~Microsoft.AspNetCore.Builder.DirectoryBrowserExtensions.UseDirectoryBrowser` 扩展方法可以开启网络应用目录浏览：

.. literalinclude:: static-files/sample/StartupBrowse.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8

And add required services by calling :dn:method:`~Microsoft.Extensions.DependencyInjection.DirectoryBrowserServiceExtensions.AddDirectoryBrowser` extension method from  ``Startup.ConfigureServices``:

并且通过从 ``Startup.ConfigureServices`` 调用 :dn:method:`~Microsoft.Extensions.DependencyInjection.DirectoryBrowserServiceExtensions.AddDirectoryBrowser` 扩展方法来增加所需服务。

.. literalinclude:: static-files/sample/StartupBrowse.cs
  :language: c#
  :start-after: >Services
  :end-before: <Services
  :dedent: 8

The code above allows directory browsing of the *wwwroot/images* folder using the URL \http://<app>/MyImages, with links to each file and folder:

这段代码允许在访问 \http://<app>/MyImages 时可浏览 *wwwroot/images* 文件夹的目录，其中包括该文件夹下的每一个文件与文件夹：

.. image:: static-files/_static/dir-browse.png

See Considerations_ on the security risks when enabling browsing.

查看关于开放访问目录时的安全隐患 Considerations_ 一文。

Note the two ``app.UseStaticFiles`` calls. The first one is required to serve the CSS, images and JavaScript in the *wwwroot* folder, and the second call for directory browsing of the *wwwroot/images* folder using the URL \http://<app>/MyImages:

注意两个 ``app.UseStaticFiles`` 调用。第一个调用请求 *wwwroot* 文件夹下的 CSS、图片和 JavaScript，第二个调用通过 \http://<app>/MyImages 请求浏览 *wwwroot/images* 文件夹的目录

.. literalinclude:: static-files/sample/StartupBrowse.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8
  :emphasize-lines: 3,5


默认文档服务
--------------------------

Setting a default home page gives site visitors a place to start when visiting your site. In order for your Web app to serve a default page without the user having to fully qualify the URI, call the ``UseDefaultFiles`` extension method from ``Startup.Configure`` as follows.

设置默认首页能给你的站点的每个访问者提供一个起始页。为使站点能提供默认页，避免用户输入完整 URI，须在 ``Startup.Configure`` 中调用 ``UseDefaultFiles`` 扩展方法：

.. literalinclude:: static-files/sample/StartupEmpty.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :emphasize-lines: 3
  :dedent: 8
 
.. note:: :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles` must be called before ``UseStaticFiles`` to serve the default file. ``UseDefaultFiles`` is a URL re-writer that doesn't actually serve the file. You must enable the static file middleware (``UseStaticFiles``) to serve the file.

.. note:: :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles` 必须在 ``UseStaticFiles`` 之前调用。``UseDefaultFiles`` 只是重写了 URL，而不是真的提供了这样一个文件。你必须开启静态文件中间件（``UseStaticFiles``）来提供这个文件。

With :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles`, requests to a folder will search for:

通过 :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles` ，请求文件夹的时候将检索以下文件：

  - default.htm
  - default.html
  - index.htm
  - index.html

The first file found from the list will be served as if the request was the fully qualified URI (although the browser URL will continue to show the URI requested).

上述列表中第一个被找到的文件将返回给用户（作为该完整 URI 的请求的应答，而此时浏览器 URL 将继续显示用户输入的 URI）。

The following code shows how to change the default file name to *mydefault.html*.

下述代码展示如何将默认文件名改为 *mydefault.html* 。

.. literalinclude:: static-files/sample/StartupDefault.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8

UseFileServer
------------------------------

:dn:method:`~Microsoft.AspNetCore.Builder.FileServerExtensions.UseFileServer` combines the functionality of :dn:method:`~Microsoft.AspNetCore.Builder.StaticFileExtensions.UseStaticFiles`, :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles`, and :dn:method:`~Microsoft.AspNetCore.Builder.DirectoryBrowserExtensions.UseDirectoryBrowser`.

:dn:method:`~Microsoft.AspNetCore.Builder.FileServerExtensions.UseFileServer` 包含了 :dn:method:`~Microsoft.AspNetCore.Builder.StaticFileExtensions.UseStaticFiles` 、 :dn:method:`~Microsoft.AspNetCore.Builder.DefaultFilesExtensions.UseDefaultFiles` 和 :dn:method:`~Microsoft.AspNetCore.Builder.DirectoryBrowserExtensions.UseDirectoryBrowser` 的功能。

The following code enables static files and the default file to be served, but does not allow directory browsing:

下面的代码启用了静态文件和默认文件，但不允许直接访问目录：

.. code-block:: c#

  app.UseFileServer();

The following code enables static files, default files and  directory browsing:

下面的代码启用了静态文件、默认文件和目录浏览功能：

.. code-block:: c#

  app.UseFileServer(enableDirectoryBrowsing: true);

See Considerations_ on the security risks when enabling browsing. As with ``UseStaticFiles``, ``UseDefaultFiles``, and ``UseDirectoryBrowser``, if you wish to serve files that exist outside the ``web root``, you instantiate and configure an :dn:class:`~Microsoft.AspNetCore.Builder.FileServerOptions` object that you pass as a parameter to ``UseFileServer``. For example, given the following directory hierarchy in your Web app:

查看直接提供目录访问时的安全风险 Considerations_ 。作为一个集合了 ``UseStaticFiles``、``UseDefaultFiles`` 和 ``UseDirectoryBrowser`` 方法于一体的方法，如果你希望提供 ``web root`` 之外存在的文件，你要实例化并配置一个 :dn:class:`~Microsoft.AspNetCore.Builder.FileServerOptions` 对象传递给 ``UseFileServer`` 的参数。比方说在你的应用中有如下层次的目录：

- wwwroot

  - css
  - images
  - ...

- MyStaticFiles

  - test.png
  - default.html

Using the hierarchy example above, you might want to enable static files, default files, and browsing for the ``MyStaticFiles`` directory. In the following code snippet, that is accomplished with a single call to :dn:class:`~Microsoft.AspNetCore.Builder.FileServerOptions`.

使用上面这个层次结构的示例，你可能希望启用静态文件、默认文件以及浏览 ``MyStaticFiles`` 目录。下面的代码片段演示了调用一次 :dn:class:`~Microsoft.AspNetCore.Builder.FileServerOptions` 来完整实现这些功能：

.. literalinclude:: static-files/sample/StartupUseFileServer.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8
  :emphasize-lines: 5-11

If ``enableDirectoryBrowsing`` is set to ``true`` you are required to call :dn:method:`~Microsoft.Extensions.DependencyInjection.DirectoryBrowserServiceExtensions.AddDirectoryBrowser` extension method from  ``Startup.ConfigureServices``:

如果在你从 ``Startup.ConfigureServices`` 请求调用 :dn:method:`~Microsoft.Extensions.DependencyInjection.DirectoryBrowserServiceExtensions.AddDirectoryBrowser` 扩展方法时将 ``enableDirectoryBrowsing`` 置为 ``true``，那么：

.. literalinclude:: static-files/sample/StartupUseFileServer.cs
  :language: c#
  :start-after: >Services
  :end-before: <Services
  :dedent: 8

Using the file hierarchy and code above:
使用的文件层次结构：

==========================================  ===================================
URI                                         Response
==========================================  ===================================
\http://<app>/StaticFiles/test.png          StaticFiles/test.png
\http://<app>/StaticFiles                   MyStaticFiles/default.html
==========================================  ===================================

If no default named files are in the *MyStaticFiles* directory, \http://<app>/StaticFiles returns the directory listing with clickable links:

如果在 *MyStaticFiles* 目录下没有默认命名的文件，则 \http://<app>/StaticFiles 将返回目录列表，其中包含可供点击的链接：

.. image:: static-files/_static/db2.PNG

.. note:: ``UseDefaultFiles`` and ``UseDirectoryBrowser`` will take the url \http://<app>/StaticFiles without the trailing slash and cause a client side redirect to \http://<app>/StaticFiles/ (adding the trailing slash). Without the trailing slash relative URLs within the documents would be incorrect.

.. note:: ``UseDefaultFiles``和``UseDirectoryBrowser`` 将会把末尾不带斜杠的 URL \http://<app>/StaticFiles 重新定向到 \http://<app>/StaticFiles/ （末尾增加了一个斜杠）。如果末尾不带斜杠，文档内相对 URL 会出错。

FileExtensionContentTypeProvider
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The :dn:class:`~Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider` class contains a  collection that maps file extensions to MIME content types. In the following sample, several file extensions are registered to known MIME types, the ".rtf" is replaced, and ".mp4" is removed.

:dn:class:`~Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider` 类内包含一个将文件扩展名映射到 MIME 内容类型的集合。在下面的例子中，多个文件扩展名注册为已知的 MIME 类型，“.rtf”被替换，“.mp4”被移除。

.. literalinclude:: static-files/sample/StartupFileExtensionContentTypeProvider.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8
  :emphasize-lines: 3-12,19

See   `MIME 内容类型 <http://www.iana.org/assignments/media-types/media-types.xhtml>`__ 。

非标准的内容类型
--------------------------

The ASP.NET static file middleware understands almost 400 known file content types. If the user requests a file of an unknown file type, the static file middleware returns a HTTP 404 (Not found) response. If directory browsing is enabled, a link to the file will be displayed, but the URI will return an HTTP 404 error.

ASP.NET 静态文件中间件能够支持超过 400 种已知文件内容类型。如果用户请求一个未知的文件类型，静态文件中间件将返回 HTTP 404（未找到）响应。如果启用目录浏览，该文件的链接将会被显示，但 URI 会返回一个 HTTP 404 错误。

The following code enables serving unknown types and will render the unknown file as an image.

下方代码把不能识别的类型和文件作为图片处理。

.. literalinclude:: static-files/sample/StartupServeUnknownFileTypes.cs
  :language: c#
  :start-after: >Configure
  :end-before: <Configure
  :dedent: 8

With the code above, a request for a file with an unknown content type will be returned as an image.

根据上面的代码，未知内容类型的文件请求将返回一张图片。

.. warning:: Enabling :dn:property:`~Microsoft.AspNetCore.Builder.StaticFileOptions.ServeUnknownFileTypes` is a security risk and using it is discouraged.  :dn:class:`~Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider`  (explained below) provides a safer alternative to serving files with non-standard extensions.

.. warning:: 开启 :dn:property:`~Microsoft.AspNetCore.Builder.StaticFileOptions.ServeUnknownFileTypes` 存在安全风险，请打消这个念头。 :dn:class:`~Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider` （下文将解释）提供了更安全的非标准扩展替代。

Considerations
^^^^^^^^^^^^^^^^

注意事项
^^^^^^^^^^^^^^^^

.. warning:: ``UseDirectoryBrowser`` and ``UseStaticFiles`` can leak secrets. We recommend that you **not** enable directory browsing in production. Be careful about which directories you enable with ``UseStaticFiles`` or ``UseDirectoryBrowser`` as the entire directory and all sub-directories will be accessible. We recommend keeping public content in its own directory such as *<content root>/wwwroot*, away from application views, configuration files, etc.

.. warning:: ``UseDirectoryBrowser``和``UseStaticFiles`` 可能会泄密。我们推荐你**不要**在生产环境开启目录浏览。要小心哪些被你开启了 ``UseStaticFiles``或``UseDirectoryBrowser`` 的目录（使得其子目录都可被访问）。我们建议将公开内容放在诸如 *<content root>/wwwroot* 这样的目录中，原理应用程序视图、配置文件等。

- The URLs for content exposed with ``UseDirectoryBrowser`` and ``UseStaticFiles`` are subject to the case sensitivity and character restrictions of their underlying file system. For example, Windows is case insensitive, but Mac and Linux are not.

- 使用 ``UseDirectoryBrowser`` 和 ``UseStaticFiles`` 暴露的文件的 URL 是否区分大小写以及字符限制受制于底层文件系统。比方说 Windows 是不区分大小写的，但 macOS 和 Linux 则区分大小写。

- ASP.NET Core applications hosted in IIS use the ASP.NET Core Module to forward all requests to the application including requests for static files. The IIS static file handler is not used because it doesn't get a chance to handle requests before they are handled by the ASP.NET Core Module.

- 托管于 IIS 的 ASP.NET Core 应用程序使用 ASP.NET Core 模块向应用程序转发所有请求，包括静态文件。IIS 静态文件处理程序（IIS Static File Handler）不会被使用，因为在 ASP.NET Core 模块处理之前它没有任何机会来处理请求。

- To remove the IIS static file handler (at the server or website level):

- 以下步骤可移除 IIS 惊叹文件处理程序（在服务器层级或网站层级）：

    - Navigate to the **Modules** feature
    - 导航到 **模块** 功能
    - Select **StaticFileModule** in the list
    - 从列表中选中 **StaticFileModule**
    - Tap **Remove** in the **Actions** sidebar
    - 在**操作**侧边栏中点击**删除**
    
.. warning:: If the IIS static file handler is enabled **and** the ASP.NET Core Module (ANCM) is not correctly configured (for example if *web.config* was not deployed), static files will be served.

.. warning:: 如果 IIS 静态文件处理程序开启**并且** ASP.NET Core 模块（ANCM）没有被正确配置（比方说 *web.config* 没有部署），（也能）将会提供静态文件。

- Code files (including c# and Razor) should be placed outside of the app project's ``web root`` (*wwwroot* by default). This creates a clean separation between your app's client side content and server side source code, which prevents server side code from being leaked.

- 代码文件（包括 C# 和 Razor）应该放在应用程序项目的 ``web root`` （默认为 *wwwroot*）之外的地方。这将确保您创建的应用程序能明确隔离客户端侧和服务器侧源代码，此举能防止服务器侧的代码被泄露。

Additional Resources
--------------------

扩展资源
--------------------

- :doc:`middleware`
- :doc:`/intro` 
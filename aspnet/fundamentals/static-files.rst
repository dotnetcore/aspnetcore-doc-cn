.. _fundamentals-static-files:

Working with Static Files 
=========================

处理静态文件
=============

作者： `Tom Archer`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

Static files, which include HTML files, CSS files, image files, and JavaScript files, are assets that the app will serve directly to clients. In this article, we'll cover the following topics as they relate to ASP.NET Core and static files.

静态文件，包括 HTML 文件、CSS 文件、图片文件以及 JavaScript 文件，会被应用程序直接提供给客户端。在本文，我们将覆盖以下有关于 ASP.NET Core 与静态文件的话题。

.. contents:: Sections:
  :local:
  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

Serving static files
--------------------

提供静态文件
--------------------

By default, static files are stored in the `webroot` of your project. The location of the webroot is defined in the project's ``hosting.json`` file where the default is `wwwroot`.

默认情况下，静态文件被保存在你项目的 `webroot` 下。网站根目录（webroot）这个位置被定义在项目的 ``hosting.json`` 文件内，默认为 `wwwroot`。

.. code-block:: json 

  "webroot": "wwwroot"

Static files can be stored in any folder under the webroot and accessed with a relative path to that root. For example, when you create a default Web application project using Visual Studio, there are several folders created within the webroot folder - ``css``, ``images``, and ``js``. In order to directly access an image in the ``images`` subfolder, the URL would look like the following:

静态文件能够被保存在网站根目录下的任意文件夹内，并通过相对根的路径来访问。比方说，当你通过 Visual Studio 创建了个默认的 Web 应用程序项目，在网站根目录下会多出几个文件夹：``css``、``images`` 以及 ``js`` 文件夹。形如下例的 URL 能够直接访问 ``images`` 目录下的图片：

  \http://<yourApp>/images/<imageFileName>

In order for static files to be served, you must configure the :doc:`middleware` to add static files to the pipeline. This specific middleware can be configured by adding a dependency on the Microsoft.AspNetCore.StaticFiles package to your project and then calling the ``UseStaticFiles`` extension method from ``Startup.Configure`` as follows:

为了能够提供静态文件，你必须配置中间件（:doc:`middleware`），把静态文件加入到管道内。这个特定的中间件能够通过下述方法来配置：在你的项目中增加 Microsoft.AspNetCore.StaticFiles 包依赖，然后从 ``Startup.Configure`` 调用 ``UseStaticFiles`` 扩展方法，如下所示：

.. code-block:: c#
  :emphasize-lines: 5

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Add static files to the request pipeline.
    app.UseStaticFiles();
    ...

Now, let's say that you have a project hierarchy where the static files you wish to serve are outside the webroot. For example,let's take a simple layout like the following:

现在，你有了一个有层次的项目——其中静态文件依你所希望的方式提供给外部。比方说下面这种简单布局：

  - wwwroot

    - css
    - images
    - ...

  - MyStaticFiles

    - test.png

In order for the user to access test.png, you can configure the static files middleware as follows:

为了让人访问到 test.png，你得这样配置静态文件中间件：

.. code-block:: c#
  :emphasize-lines: 5-9

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Add MyStaticFiles static files to the request pipeline.
    app.UseStaticFiles(new StaticFileOptions()
    {
        FileProvider = new PhysicalFileProvider(@"D:\Source\WebApplication1\src\WebApplication1\MyStaticFiles"),
        RequestPath = new PathString("/StaticFiles")
    });
    ...

At this point, if the user enters an address of ``http://<yourApp>/StaticFiles/test.png``, the ``test.png`` image will be served.

在这一点上，如果用户访问 ``http://<yourApp>/StaticFiles/test.png`` 这个地址，这张名叫 ``test.png`` 的图片就能提供给外部了。

Enabling directory browsing
---------------------------

允许直接浏览目录
---------------------------

Directory browsing allows the user of your Web app to see a list of directories and files within a specified directory (including the root). By default, this functionality is not available such that if the user attempts to display a directory within an ASP.NET Web app, the browser displays an error. To enable directory browsing for your Web app, call the ``UseDirectoryBrowser`` extension method from  ``Startup.Configure`` as follows:

目录浏览允许网站用户看到指定目录下的目录和文件列表（包括根 root）。默认情况下此功能不可用，如果用户尝试显示一个 ASP.NET Web 应用程序内的一个目录，浏览器将显示一个错误。在 ``Startup.Configure`` 中调用 ``UseDirectoryBrowser`` 扩展方法可以开启网络应用目录浏览：

.. code-block:: c#
  :emphasize-lines: 5

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Turn on directory browsing for the current directory.
    app.UseDirectoryBrowser();
    ...

The following figure illustrates the results of browsing to the Web app's ``images`` folder with directory browsing turned on:

当目录浏览被开启，访问 ``images`` 文件夹的结果如下图表所示： 

.. image:: static-files/_static/dir-browse.png

Now, let's say that you have a project hierarchy where you want the user to be able to browse a directory that is not in the webroot. For example, let's take a simple layout like the following:

现在我们可以说你已经有了一个有层次的项目——如你所愿那般用户可以浏览网站根目录之外的目录。比方说下面这种简单布局：

  - wwwroot

    - css
    - images
    - ...

  - MyStaticFiles

In order for the user to browse the ``MyStaticFiles`` directory, you can configure the static files middleware as follows:

为了让人访问到 ``MyStaticFiles`` 目录，你得这样配置静态文件中间件：

.. code-block:: c#
  :emphasize-lines: 5-9

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Add the ability for the user to browse the MyStaticFiles directory.
    app.UseDirectoryBrowser(new DirectoryBrowserOptions()
    {
        FileProvider = new PhysicalFileProvider(@"D:\Source\WebApplication1\src\WebApplication1\MyStaticFiles"),
        RequestPath = new PathString("/StaticFiles")
    });
    ...

At this point, if the user enters an address of ``http://<yourApp>/StaticFiles``, the browser will display the files in the ``MyStaticFiles`` directory.

在这一点上，如果用户访问 ``http://<yourApp>/StaticFiles`` 这个地址，浏览器将显示 ``MyStaticFiles`` 目录下的文件。

Serving a default document
--------------------------

提供默认文档
--------------------------

Setting a default home page gives site visitors a place to start when visiting your site. Without a default site users will see a blank page unless they enter a fully qualified URI to a document.  In order for your Web app to serve a default page without the user having to fully qualify the URI, call the ``UseDefaultFiles`` extension method from ``Startup.Configure`` as follows.

设置默认首页能给每个访问你站点的访问者一个起始页。如果不设这么一个默认页，用户访问站点会看到一个空白页，出给他们输入文档的完整的 URI。为使站点能提供默认页，避免用户输入完整 URI，须在 ``Startup.Configure`` 中调用 ``UseDefaultFiles`` 扩展方法：

.. code-block:: c#
  :emphasize-lines: 5-6

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Serve the default file, if present.
    app.UseDefaultFiles();
    app.UseStaticFiles();
    ...

.. note:: ``UseDefaultFiles`` must be called before ``UseStaticFiles`` or it will not serve up the default home page. You must still call ``UseStaticFiles``. ``UseDefaultFiles`` is a URL re-writer that doesn't actually serve the file. You must still specify middleware (UseStaticFiles, in this case) to serve the file.

.. note:: ``UseDefaultFiles`` 必须在 ``UseStaticFiles`` 之前调用，否则不会提供默认首页。你还是必须要调用 ``UseStaticFiles`` 的。``UseDefaultFiles`` 只是重写了 URL，而不是真正的提供了这么一个文件。你也依旧需要指定的中间件（在这个例子中是 UseStaticFiles）来提供这个文件。

If you call the ``UseDefaultFiles`` extension method and the user enters a URI of a folder, the middleware will search (in order) for one of the following files. If one of these files is found, that file will be used as if the user had entered the fully qualified URI (although the browser URL will continue to show the URI entered by the user).

如果调用 ``UseDefaultFiles`` 扩展方法，用户输入了一个文件夹的 URI，中间件将（按序）检索下列文件中的一个。如果其中一者被检索到，那么该文件就会如输入完整 URI 那般被提供给用户（虽然浏览器 URL 将继续显示用户输入的 URI）。

  - default.htm
  - default.html
  - index.htm
  - index.html

To specify a different default file from the ones listed above, instantiate a ``DefaultFilesOptions`` object and set its ``DefaultFileNames`` string list to a list of names appropriate for your app. Then, call one of the overloaded ``UseDefaultFiles`` methods passing it the ``DefaultFilesOptions`` object. The following example code removes all of the default files from the ``DefaultFileNames`` list and adds  ``mydefault.html`` as the only default file for which to search.

若要指定从上面所列的不同的默认文件，实例化 ``DefaultFilesOptions`` 对象并设置其 ``DefaultFileNames`` 字符串列表为一个适用于你的应用程序的名称列表。然后调用重载的 ``UseDefaultFiles`` 方法来传递 ``DefaultFilesOptions`` 对象。下面的代码示例演示了从 ``DefaultFileNames`` 列表中移除所有默认文件，并增加 ``mydefault.html`` 为其唯一的默认文件。

.. code-block:: c#
  :emphasize-lines: 5-9

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Serve my app-specific default file, if present.
    DefaultFilesOptions options = new DefaultFilesOptions();
    options.DefaultFileNames.Clear();
    options.DefaultFileNames.Add("mydefault.html");
    app.UseDefaultFiles(options);
    app.UseStaticFiles();
    ...

Now, if the user browses to a directory in the webroot with a file named ``mydefault.html``, that file will be served as though the user typed in the fully qualified URI.

现在，如果用户访问网站根目录，而这个目录下恰有一个名为 ``mydefault.html`` 的文件，那么该文件就会如输入完整 URI 那般被提供给用户。

But, what if you want to serve a default page from a directory that is outside the webroot directory? You could call both the ``UseStaticFiles`` and ``UseDefaultFiles`` methods passing in identical values for each method's parameters. However, it's much more convenient and recommended to call the ``UseFileServer`` method, which is covered in the next section.

但是，如果你想提供一个网络根目录之外的目录下的默认页面，你该怎么做？你可以调用 ``UseStaticFiles`` 和 ``UseDefaultFiles`` 方法为每一个方法中的参数提供相同的值。不过有更便捷更值得推荐的方法，那就是调用 ``UseFileServer`` 方法，这将在下一节中介绍。

Using the UseFileServer method
------------------------------

使用 UseFileServer 方法
------------------------------

In addition to the ``UseStaticFiles``, ``UseDefaultFiles``, and ``UseDirectoryBrowser`` extensions methods, there is also a single method - ``UseFileServer`` - that combines the functionality of all three methods. The following example code shows some common ways to use this method:

除了 ``UseStaticFiles``、``UseDefaultFiles`` 和 ``UseDirectoryBrowser`` 扩展方法之外，还有一个单独的方法——``UseFileServer``——结合了三者的功能。下面的代码实例演示了该方法的常见用法：

.. code-block:: c#

  // Enable all static file middleware (serving of static files and default files) EXCEPT directory browsing.
  app.UseFileServer();

.. code-block:: c#

  // Enables all static file middleware (serving of static files, default files, and directory browsing).
  app.UseFileServer(enableDirectoryBrowsing: true);

As with the ``UseStaticFiles``, ``UseDefaultFiles``, and ``UseDirectoryBrowser`` methods, if you wish to serve files that exist outside the webroot, you instantiate and configure an "options" object that you pass as a parameter to ``UseFileServer``. For example, let's say you have the following directory hierarchy in your Web app:

作为一个集合了 ``UseStaticFiles``、``UseDefaultFiles`` 和 ``UseDirectoryBrowser`` 方法于一体的方法，吐过你希望提供网络根目录之外存在的文件，你要实例化并配置一个「options」对象传递给 ``UseFileServer`` 的参数。比方说在你的应用中有如下层次的目录：

- wwwroot

  - css
  - images
  - ...

- MyStaticFiles

  - test.png
  - default.html

Using the hierarchy example above, you might want to enable static files, default files, and browsing for the ``MyStaticFiles`` directory. In the following code snippet, that is accomplished with a single call to ``UseFileServer``.

使用上面这个层次结构的示例，你可能希望启用静态文件、默认文件以及浏览 ``MyStaticFiles`` 目录。下面的代码片段演示了调用一次 ``UseFileServer`` 来完整实现这些功能：

.. code-block:: c#

  // Enable all static file middleware (serving of static files, default files,
  // and directory browsing) for the MyStaticFiles directory.
  app.UseFileServer(new FileServerOptions()
  {
      FileProvider = new PhysicalFileProvider(@"D:\Source\WebApplication1\src\WebApplication1\MyStaticFiles"),
      RequestPath = new PathString("/StaticFiles"),
      EnableDirectoryBrowsing = true
  });

Using the example hierarchy and code snippet from above, here's what happens if the user browses to various URIs:

使用上面的目录层次和代码片段，当用户浏览多个 URI 时会发生这些情况：

  - ``http://<yourApp>/StaticFiles/test.png`` - The ``MyStaticFiles/test.png`` file will be served to and presented by the browser.
  - ``http://<yourApp>/StaticFiles`` - Since a default file is present (``MyStaticFiles/default.html``), that file will be served. If that file didn't exist, the browser would present a list of files in the ``MyStaticFiles`` directory (because the ``FileServerOptions.EnableDirectoryBrowsing`` property is set to ``true``).

  - ``http://<yourApp>/StaticFiles/test.png`` - ``MyStaticFiles/test.png`` 文件将会被提供并呈现于浏览器之上。
  - ``http://<yourApp>/StaticFiles`` - 由于默认文件是存在的（``MyStaticFiles/default.html``），该文件会被提供。如果该文件不存在，浏览器就会显示 ``MyStaticFiles`` 目录下的文件列表（这是因为 ``FileServerOptions.EnableDirectoryBrowsing`` 属性被设置为 ``true``）。

Working with content types
--------------------------

处理内容类型
--------------------------

The ASP.NET static files middleware understands almost 400 known file content types. If the user attempts to reach a file of an unknown file type, the static file middleware will not attempt to serve the file.

ASP.NET 静态文件中间件能够理解超过 400 种已知文件内容类型。如果用户试图得到一个未知文件类型的文件，静态文件中间件不会尝试去提供这个文件。

Let's take the following directory/file hierarchy example to illustrate:

以下面这个目录/文件层次为例来说明：

- wwwroot

  - css
  - images

    - test.image

  - ...

Using this hierarchy, you could enable static file serving and directory browsing with the following:

使用这种层次结构，你可以用下面的代码来启用静态文件服务和目录浏览功能：

.. code-block:: c#
  :emphasize-lines: 5-6

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Serve static files and allow directory browsing.
    app.UseDirectoryBrowser();
    app.UseStaticFiles();

If the user browses to ``http://<yourApp>/images``, a directory listing will be displayed by the browser that includes the ``test.image`` file. However, if the user clicks on that file, they will see a 404 error - even though the file obviously exists. In order to allow the serving of unknown file types, you could set the ``StaticFileOptions.ServeUnknownFileTypes`` property to ``true`` and specify a default content type via ``StaticFileOptions.DefaultContentType``. (Refer to this `list of common MIME content types <http://www.freeformatter.com/mime-types-list.html>`_.)

如果用户浏览 ``http://<yourApp>/images``，包括 ``test.image`` 文件在内的目录列表会被显示出来。不过如果用户点击那个文件，会得到一个 404 错误——即使这个文件是存在的。为了允许提供默认文件类型，你需要设置 ``StaticFileOptions.ServeUnknownFileTypes`` 属性为 ``true``，并通过 ``StaticFileOptions.DefaultContentType`` 指定默认内容类型。（请参考 `常用 MIME 内容类型清单 <http://www.freeformatter.com/mime-types-list.html>`_.）

.. code-block:: c#
  :emphasize-lines: 5-10

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...
    // Serve static files and allow directory browsing.
    app.UseDirectoryBrowser();
    app.UseStaticFiles(new StaticFileOptions
    {
      ServeUnknownFileTypes = true,
      DefaultContentType = "image/png"
    });

At this point, if the user browses to a file whose content type is unknown, the browser will treat it as an image and render it accordingly.

如此一来，如果用户浏览到未知文件内容的文件，浏览器会将之作为图片来处理和渲染。

So far, you've seen how to specify a default content type for any file type that ASP.NET doesn't recognize. However, what if you have multiple file types that are unknown to ASP.NET? That's where the ``FileExtensionContentTypeProvider`` class comes in.

直至目前为止，你已经了解到当 ASP.NET 遇到不能识别的文件类型时，如何指定一个默认的了。不过，如果是多个未知文件类型该怎么办呢？这恰好是 ``FileExtensionContentTypeProvider`` 类所能解决的。

The ``FileExtensionContentTypeProvider`` class contains an internal collection that maps file extensions to MIME content types. To specify custom content types, simply instantiate a ``FileExtensionContentTypeProvider`` object and add a mapping to the ``FileExtensionContentTypeProvider.Mappings`` dictionary for each needed file extension/content type. In the following example, the code adds a mapping of the file extension ``.myapp`` to the MIME content type ``application/x-msdownload``.

``FileExtensionContentTypeProvider`` 类中包含了内部集合用于保存文件扩展名和 MIME 内容类型之间的映射关系。若要指定自定义的内容类型，只需要实例一个 ``FileExtensionContentTypeProvider`` 对象，在 ``FileExtensionContentTypeProvider.Mappings`` 字典中添加你所需要的「文件扩展名/内容类型」映射。在下面的例子中，代码会把 ``.myapp`` 扩展名映射到 ``application/x-msdownload`` 这个 MIME 内容类型的这组映射关系添加到字典中。

.. code-block:: c#
  :emphasize-lines: 5-13

  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    ...

    // Allow directory browsing.
    app.UseDirectoryBrowser();

    // Set up custom content types - associating file extension to MIME type
    var provider = new FileExtensionContentTypeProvider();
    provider.Mappings.Add(".myapp", "application/x-msdownload");

    // Serve static files.
    app.UseStaticFiles(new StaticFileOptions { ContentTypeProvider = provider });

    ...

Now, if the user attempts to browse to any file with an extension of ``.myapp``, the user will be prompted to download the file (or it will happen automatically depending on the browser).

现在，如果用户视图浏览任何扩展名为 ``.myapp`` 的文件，都会下载该文件（自动与否取决于浏览器的不同而不同）。

IIS Considerations
------------------

IIS 的注意事项
------------------

ASP.NET Core applications hosted in IIS use the HTTP platform handler to forward all requests to the application including requests for static files. The IIS static file handler is not used because it won’t get a chance to handle the request before it is handled by the HTTP platform handler.

托管于 IIS 的 ASP.NET Core 应用程序使用 HTTP 平台处理程序将所有请求转发到应用程序，包括静态文件的请求。IIS 静态文件处理程序在 HTTP 平台处理程序处理之前没有机会处理请求。

Best practices
--------------

最佳实践
--------------

This section includes a list of best practices for working with static files:

本节包括处理静态文件的最佳实践列表 ︰

  - Code files (including C# and Razor files) should be placed outside of the app project's webroot. This creates a clean separation between your app's static (non-compilable) content and source code.

  - 代码文件（包括 C# 和 Razor 文件）必须放在应用程序项目的网络根目录之外。这使你应用程序的静态（非可编译）内容与源代码完全隔离。

Summary
-------

总结
-------
In this article, you learned how the static files middleware component in ASP.NET Core allows you to serve static files, enable directory browsing, and serve default files. You also saw how to work with content types that ASP.NET doesn't recognize. Finally, the article explained some IIS considerations and presented some best practices for working with static files.

在本文中，你学习了如何在 ASP.NET Core 的静态文件中间件中允许为静态文件提供服务、启用目录浏览，以及提供默认文件。你也了解到了如何处理 ASP.NET 不能识别的内容类型。最后，文章列举了几个 IIS 需要注意的事项，并提出了处理静态文件的一些最佳实践。

Additional Resources
--------------------

扩展资源
--------------------

- :doc:`middleware`

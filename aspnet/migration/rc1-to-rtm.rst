从 ASP.NET 5 RC1 迁移到 ASP.NET Core 1.0
================================================

By `Cesar Blum Silveira`_, `Rachel Appel`_, `Rick Anderson`_ 

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对：

.. contents:: Sections:
  :local:
  :depth: 1

ASP.NET 5 RC1 apps were based on the .NET Execution Environment (DNX) and made use of DNX specific features. ASP.NET Core 1.0 is based on .NET Core, so you must first migrate your application to the new .NET Core project model. See `migrating from DNX to .NET Core CLI <https://docs.microsoft.com/en-us/dotnet/articles/core/migrating-from-dnx>`__ for more information.

ASP.NET 5 RC1 引用基于 .NET Execution Environment (DNX) 并使用了许多 DNX 特有功能。而 ASP.NET Core 1.0 基于 .NET Core，因此你首先得将应用程序迁移到新的 .NET Core 项目模型中。更新相关信息可以查阅 `从 DNX 迁移到 .NET Core CLI <https://docs.microsoft.com/en-us/dotnet/articles/core/migrating-from-dnx>`__ 一文。

See the following resources for a list of some of the most significant changes, announcements and migrations information:

可以通过查阅以下资源来了解新老版本间的重大变化、公告以及迁移信息：

- `ASP.NET Core RC2 significant changes <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0-rc2>`_ 
- `ASP.NET Core RC2 重大更新 <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0-rc2>`_ 
- `ASP.NET Core 1.0 significant changes <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0>`_
- `ASP.NET Core 1.0 重大更新 <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0>`_
- `Upgrading from Entity Framework RC1 to RTM <https://docs.efproject.net/en/latest/miscellaneous/rc2-rtm-upgrade.html>`_
- `迁移 Entity Framework RC1 至 RTM <https://docs.efproject.net/en/latest/miscellaneous/rc2-rtm-upgrade.html>`_
- :doc:`/migration/rc2-to-rtm`
- :doc:`/migration/rc2-to-rtm`

更新 Target Framework Monikers (TFMs)
---------------------------------------

If your app targeted ``dnx451`` or  ``dnxcore50`` in the ``frameworks`` section of *project.json*, you must make the following changes:

若你的应用针对的是 ``dnx451`` 或 ``dnxcore50``（该信息位于 *project.json* 文件的 ``frameworks`` 节点下），你需要作以下修改：

==================================== ====================================
DNX                                  .NET Core
==================================== ====================================
``dnx451``                           ``net451``
``dnxcore50``                        ``netcoreapp1.0``
==================================== ====================================

.NET Core apps must add a dependency to the ``Microsoft.NETCore.App`` package:

.NET Core 应用必须依赖 ``Microsoft.NETCore.App`` 包：

.. original - 
  {  
     "frameworks": {  
       "netcoreapp1.0": {  
         "dependencies": {  
           "Microsoft.NETCore.App": {  
             "version": "1.0.0",  
             "type": "platform"  
           }  
         }  

.. code-block:: none
  
  "dependencies": {
    "Microsoft.NETCore.App": {
      "version": "1.0.0",
      "type": "platform"
    },

命名空间与包 ID 的变化
--------------------------------

- ASP.NET 5 has been renamed to ASP.NET Core 1.0
- ASP.NET 5 重命名为 ASP.NET Core 1.0
- ASP.NET MVC and Identity are now part of ASP.NET Core
- ASP.NET MVC 和 Identity 已经成为 ASP.NET Core 的一部分 
- ASP.NET MVC 6 is now ASP.NET Core MVC
- ASP.NET MVC 6 就是现在的 ASP.NET Core MVC
- ASP.NET Identity 3 is now ASP.NET Core Identity
- ASP.NET Identity 3 就是现在的 ASP.NET Core Identity
- ASP.NET Core 1.0 package versions are ``1.0.0``
- ASP.NET Core 1.0 包版本号为 ``1.0.0``
- ASP.NET Core 1.0 tool package versions are ``1.0.0-preview2-final``
- ASP.NET Core 1.0 工具包版本号为 ``1.0.0-preview2-final``

Namespace and package name changes:

命名空间与包名的变更：

==========================================    ===================================================
ASP.NET 5 RC1                                 ASP.NET Core 1.0
==========================================    ===================================================
``Microsoft.AspNet.*``                        ``Microsoft.AspNetCore.*``
``EntityFramework.*``                         ``Microsoft.EntityFrameworkCore.*``
``Microsoft.Data.Entity.*``                   ``Microsoft.EntityFrameworkCore.*``
==========================================    ===================================================

The ``EntityFramework.Commands`` package is no longer available. The ``ef`` command is now available as a tool in the ``Microsoft.EntityFrameworkCore.Tools`` package.

``EntityFramework.Commands`` 包已不可用。``ef`` 指令现在位于 ``Microsoft.EntityFrameworkCore.Tools`` 包中。

The following packages have been renamed:

下列包已被更名：

==========================================    ===================================================
ASP.NET 5 RC1                                 ASP.NET Core 1.0
==========================================    ===================================================
EntityFramework.MicrosoftSqlServer            Microsoft.EntityFrameworkCore.SqlServer
Microsoft.AspNet.Diagnostics.Entity           Microsoft.AspNetCore.Dianostics.EntityFrameworkCore
Microsoft.AspNet.Identity.EntityFramework     Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.AspNet.Tooling.Razor                Microsoft.AspNetCore.Razor.Tools
==========================================    ===================================================

命令与工具
------------------

The ``commands`` section of  the *project.json* file is no longer supported. Use ``dotnet run`` or ``dotnet <DLL name>`` instead.

*project.json* 文件的 ``commands`` 节点已不再被支持。请转而使用 ``dornet run`` 或 ``dotnet <DLL name>``。

.NET Core CLI has introduced the concept of tools. *project.json* now supports a ``tools`` section where packages containing tools can be specified. Some important functionality for ASP.NET Core 1.0 applications has been moved to tools.

.NET Core CLI 作为一种工具的概念被引入。*project.json* 目前支持 ``tools`` 节点，在该节点内可以指定包含的工具包。值得一提的是，许多对 ASP.NET Core 1.0 而言十分重要的功能都已被归入工具之中。

See `.NET Core CLI extensibility model <https://dotnet.github.io/docs/core-concepts/core-sdk/cli/extensibility.html>`_ for more information on .NET Core CLI tools.

关于 .NET Core CLI 工具的更多信息请阅读 `.NET Core CLI extensibility model <https://dotnet.github.io/docs/core-concepts/core-sdk/cli/extensibility.html>`_ 一文。

发布到 IIS
^^^^^^^^^^^^^^^^^

IIS publishing is now provided by the ``publish-iis`` tool in the ``Microsoft.AspNetCore.Server.IISIntegration.Tools`` package. If you intend to run your app behind IIS, add the ``publish-iis`` tool to your *project.json*:

IIS 发布功能现在由 ``public-iis`` 工具提供，该工具位于 ``Microsoft.AspNetCore.Server.IISIntegration.Tools`` 包中。如果你的应用程序打算运行在 IIS 上，那么可以把 ``publish-iis`` 工具添加到 *project.json* 中：

.. code-block:: json

  {
    "tools": {
      "Microsoft.AspNetCore.Server.IISIntegration.Tools": "1.0.0-preview2-final"
    }
  }

The ``publish-iis`` tool is commonly used in the ``postpublish`` script in *project.json*:

在 *project.json* 中 ``publish-iis`` 工具常用 ``postpublish`` 脚本：

.. code-block:: json

  {
    "postpublish": [ "dotnet publish-iis --publish-folder %publish:OutputPath% --framework %publish:FullTargetFramework%" ]
  }

Entity Framework 命令
^^^^^^^^^^^^^^^^^^^^^^^^^

The ``ef`` tool is now provided in the ``Microsoft.EntityFrameworkCore.Tools`` package:

``ef`` 工具目前由 ``Microsoft.EntityFrameworkCore.Tools`` 包提供：

.. code-block:: json

  {
    "tools": {
      "Microsoft.EntityFrameworkCore.Tools": "1.0.0-preview2-final"
    }
  }

For more information, see `.NET Core CLI <https://docs.efproject.net/en/latest/cli/dotnet.html>`_.

更多信息请阅读 `.NET Core CLI <https://docs.efproject.net/en/latest/cli/dotnet.html>`_ 。

Razor 工具
^^^^^^^^^^^

Razor tooling is now provided in the ``Microsoft.AspNetCore.Razor.Tools`` package:

Razor 工具目前由 ``Microsoft.AspNetCore.Razor.Tools`` 包提供：

.. code-block:: json

  {
    "tools": {
      "Microsoft.AspNetCore.Razor.Tools": "1.0.0-preview2-final"
    }
  }


SQL 缓存工具
^^^^^^^^^^^^^^

The ``sqlservercache`` command, formerly provided by the ``Microsoft.Extensions.Caching.SqlConfig`` package, has been replaced by the ``sql-cache`` tool, available through the ``Microsoft.Extensions.Caching.SqlConfig.Tools`` package:

以前的 ``sqlservercache`` 指令由 ``Microsoft.Extensions.Caching.SqlConfig`` 包提供——现在已经发生了变化——现在 ``sqlservercache`` 指令已经被 ``sql-cache`` 工具所取代，并由 ``Microsoft.Extensions.Caching.SqlConfig.Tools`` 包来提供： 

.. code-block:: json

  {
    "tools": {
      "Microsoft.Extensions.Caching.SqlConfig.Tools": "1.0.0-preview2-final"
    }
  }

用户机密管理器
^^^^^^^^^^^^^^^^^^^^

The ``user-secret`` command, formerly provided by the ``Microsoft.Extensions.SecretManager`` package, has been replaced by the ``user-secrets`` tool, available through the ``Microsoft.Extensions.SecretManager.Tools`` package:

以前的 ``user-secret`` 指令由 ``Microsoft.Extensions.SecretManager`` 包提供，现在该命令已被 ``user-secrets`` 工具所取代，并由 ``Microsoft.Extensions.SecretManager.Tools`` 包提供：

.. code-block:: json

  {
    "tools": {
      "Microsoft.Extensions.SecretManager.Tools": "1.0.0-preview2-final"
    }
  }


文件监控
^^^^^^^^^^^^

The ``watch`` command, formerly provided by the ``Microsoft.Dnx.Watcher`` package, has been replaced by the ``watch`` tool, available through the ``Microsoft.DotNet.Watcher.Tools`` package:

以前的 ``watch``  指令由 ``Microsoft.Dnx.Watcher`` 包提供，现在该命令已被由 ``Microsoft.DotNet.Watcher.Tools`` 包的 ``watch`` 工具所替代：

.. code-block:: json

  {
    "tools": {
      "Microsoft.DotNet.Watcher.Tools": "1.0.0-preview2-final"
    }
  }

For more information on the file watcher, see **Dotnet watch** in  :doc:`/tutorials/index`.

更多关于文件监控的信息请查阅 :doc:`/tutorials/index` 的 **Dotnet 监控** 一节。

托管服务
-------

创建 Web 应用程序的托管服务
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

ASP.NET Core 1.0 apps are console apps; you must define an entry point for your app that sets up a web host and runs it. Below is an example from the startup code for one of the Web Application templates in Visual Studio:

ASP.NET Core 1.0 应用程序是控制台引用；你必须为你的引用定义一个入口点（entry point），这样才能启动并运行这个 Web 服务。下面是 Visual Studio 的 Web 应用程序模板中启动代码的例子：

.. code-block:: c#

  public class Program
  {
      public static void Main(string[] args)
      {
          var host = new WebHostBuilder()
              .UseKestrel()
              .UseContentRoot(Directory.GetCurrentDirectory())
              .UseIISIntegration()
              .UseStartup<Startup>()
              .Build();

          host.Run();
      }
  }

You must add the ``emitEntryPoint`` to the ``buildOptions`` section of your application's *project.json*:

你必须在应用程序的 *project.json* 文件的 ``buildOptions`` 节点中添加 ``emitEntryPoint``：

.. code-block:: json

  {
    "buildOptions": {
      "emitEntryPoint": true
    }
  }

类与接口的重命名
^^^^^^^^^^^^^^^^^^^^^^^^^^^

All classes and interfaces prefixed with ``WebApplication`` have been renamed to start with ``WebHost``:

所有以 ``WebApplication`` 开头的类和接口都被重命名了，新的名称以 ``WebHost`` 开头：

===========================    =========================
ASP.NET 5 RC1                  ASP.NET Core 1.0
===========================    =========================
IWebApplicationBuilder         IWebHostBuilder
WebApplicationBuilder          WebHostBuilder
IWebApplication                IWebHost
WebApplication                 WebHost
WebApplicationOptions          WebHostOptions
WebApplicationDefaults         WebHostDefaults
WebApplicationService          WebHostService
WebApplicationConfiguration    WebHostConfiguration
===========================    =========================

内容根与 web 根
^^^^^^^^^^^^^^^^^^^^^^^^^

The application base path is now called the content root.

现在应用程序的基路径被称为内容根（Content Root）。

The web root of your application is no longer specified in your *project.json* file. It is defined when setting up the web host and defaults to ``wwwroot``. Call the :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseWebRoot` extension method to specify a different web root folder. Alternatively, you can specify the web root folder in configuration and call the :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseConfiguration` extension method.

应用程序的 Web 根不再由 *project.json* 文件来指定了。现在它由网站服务启动时来配置，默认为 ``wwwroot``。可以通过调用 :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseWebRoot` 扩展方法来指定另一个文件夹作为 Web 根。另外你可以在配置中指定 Web 根文件夹，并通过调用 :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseConfiguration` 扩展方法使之生效。

生成服务器地址
^^^^^^^^^^^^^^^^^^^^^^

The server addresses that your application listens on can be specified using the :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseUrls` extension method or through configuration.

应用程序监听的服务地址是由 :dn:method:`~Microsoft.AspNetCore.Hosting.HostingAbstractionsWebHostBuilderExtensions.UseUrls` 扩展方法或通过配置来指定的。

Specifying only a port number as a binding address is no longer supported. The default binding address is \http://localhost:5000

不再支持仅指定一个端口号来绑定地址。默认的绑定地址为 \http://localhost:5000

托管服务配置
^^^^^^^^^^^^^^^^^^^^^

The ``UseDefaultHostingConfiguration`` method is no longer available. The only configuration values read by default by :dn:class:`~Microsoft.AspNetCore.Hosting.WebHostBuilder` are those specified in environment variables prefixed with ``ASPNETCORE_*``. All other configuration sources must now be added explicitly to an :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` instance. See :doc:`/fundamentals/configuration` for more information.

不再支持 ``UseDefaultHostingConfiguration`` 。由 :dn:class:`~Microsoft.AspNetCore.Hosting.WebHostBuilder` 所读取到的配置默认是那些前缀为 ``ASPNETCORE_*`` 的环境变量。其他所有的配置源都必须显式地添加到 :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` 接口中。更多信息请查看 :doc:`/fundamentals/configuration` 。

The environment key is set with the ``ASPNETCORE_ENVIRONMENT`` environment variable. ``ASPNET_ENV`` and ``Hosting:Environment`` are still supported, but generate a deprecated message warning.

环境键被设置在 ``ASPNETCORE_ENVIRONMENT`` 环境变量中。尽管 ``ASPNET_ENV`` 和 ``Hosting:Environment`` 还被支持，但会产生一个过时的警告消息。

托管服务更新
^^^^^^^^^^^^^^^^^^^^^^^

Dependency injection code that uses ``IApplicationEnvironment`` must now use :dn:iface:`~Microsoft.AspNetCore.Hosting.IHostingEnvironment`. For example, in your ``Startup`` class, change:

通过使用 ``IApplicationEnvironment`` 的依赖注入代码现在必须使用 :dn:iface:`~Microsoft.AspNetCore.Hosting.IHostingEnvironment` 。比如在你的 ``Startup`` 类中，你需要这样修改：

.. code-block:: c#

  public Startup(IApplicationEnvironment applicationEnvironment)

To:

改为：

.. code-block:: c#

  public Startup(IHostingEnvironment hostingEnvironment)

Kestrel
-------

Kestrel configuration has changed. `This GitHub announcement <https://github.com/aspnet/Announcements/issues/168>`_ outlines the changes you must make to configure Kestrel if you are not using default settings.

Kestrel 配置已变化。在 `This GitHub announcement <https://github.com/aspnet/Announcements/issues/168>`_ 中已经列出了变更大纲，如果你不打算使用默认配置来使用 Kestrel，那么你必须根据该变更大纲来配置你的 Kestrel。

控制器和 action 结果的重命名
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The following :dn:class:`~Microsoft.AspNetCore.Mvc.Controller` methods have been renamed and moved to :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerBase`:

下列 :dn:class:`~Microsoft.AspNetCore.Mvc.Controller` 方法已经被重命名，并移入 :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerBase` 中：

==================================  ==================
ASP.NET 5 RC1                       ASP.NET Core 1.0
==================================  ==================
HttpUnauthorized                    Unauthorized
HttpNotFound (and its overloads)    NotFound
HttpBadRequest (and its overloads)  BadRequest
==================================  ==================

The following action result types have also been renamed:

下列 Action 结果类型也全被重命名了：

=============================================  =============================================
ASP.NET 5 RC1                                        ASP.NET Core 1.0
=============================================  =============================================
Microsoft.AspNet.Mvc.HttpOkObjectResult        Microsoft.AspNetCore.Mvc.OkObjectResult
Microsoft.AspNet.Mvc.HttpOkResult              Microsoft.AspNetCore.Mvc.OkResult
Microsoft.AspNet.Mvc.HttpNotFoundObjectResult  Microsoft.AspNetCore.Mvc.NotFoundObjectResult
Microsoft.AspNet.Mvc.HttpNotFoundResult        Microsoft.AspNetCore.Mvc.NotFoundResult
Microsoft.AspNet.Mvc.HttpStatusCodeResult      Microsoft.AspNetCore.Mvc.StatusCodeResult
Microsoft.AspNet.Mvc.HttpUnauthorizedResult    Microsoft.AspNetCore.Mvc.UnauthorizedResult
=============================================  =============================================

ASP.NET 5 MVC 编译视图
---------------------------

To compile views, set the ``preserveCompilationContext`` option in *project.json* to preserve the compilation context, as shown here:

要编译视图的话，在 *project.json* 中设置 ``preserveCompilationContext`` 选项以便保存编译上下文，如下所示：

.. code-block:: json

  {
    "buildOptions": {
      "preserveCompilationContext": true
    }
  }

视图更新
^^^^^^^^^^^^^^^^

Views now support relative paths.

视图现在支持相对路径（relative paths）。

The Validation Summary Tag Helper ``asp-validation-summary`` attribute value has changed. Change:

Validation Summary Tag Helper 的 ``asp-validation-summary`` 特性值已经发生变化，具体为：

.. code-block:: html

  <div asp-validation-summary="ValidationSummary.All"></div>

To:

变为：

.. code-block:: html

  <div asp-validation-summary="All"></div>

ViewComponents 更新
^^^^^^^^^^^^^^^^^^^^^^^^^

- The sync APIs have been removed
- 同步 APIs 已被移除
- ``Component.Render()``, ``Component.RenderAsync()``, and ``Component.Invoke()`` have been removed
- ``Component.Render()`` 、 ``Component.RenderAsync()`` 以及 ``Component.Invoke()`` 均已被移除
- To reduce ambiguity in View Component method selection, we've modified the selection to only allow exactly one ``Invoke()`` or ``InvokeAsync()`` per View Component
- 为降低 View Component 方法选择上的歧义，我们着手进行了若干修改，具体为现在每个 View Component 都只允许使用一个 ``Invoke()`` 或 ``InvokeAsync()`` 
- ``InvokeAsync()`` now takes an anonymous object instead of separate parameters
- ``InvokeAsync()`` 现在允许接收匿名对象，而不是一个个独立的参数
- To use a view component, call ``@Component.InvokeAsync("Name of view component", <parameters>)`` from a view. The parameters will be passed to the ``InvokeAsync()`` method. The following example demonstrates the ``InvokeAsync()`` method call with two parameters:
- 可以在视图中通过调用 ``@Component.InvokeAsync("Name of view component", <parameters>)`` 来使用 view component。诸参数将传递给 ``InvokeAsync()`` 方法。下例将演示带两个参数的 ``InvokeAsync()`` 方法调用：

ASP.NET 5 RC1:

在 ASP.NET 5 RC1 中：

.. code-block:: c#

  @Component.InvokeAsync("Test", "MyName", 15)

ASP.NET Core 1.0:

在 ASP.NET Core 1.0 中：

.. code-block:: c#

  @Component.InvokeAsync("Test", new { name = "MyName", age = 15 })
  @Component.InvokeAsync("Test", new Dictionary<string, object> { 
                         ["name"] = "MyName", ["age"] = 15 })
  @Component.InvokeAsync<TestViewComponent>(new { name = "MyName", age = 15})

更新控制器发现规则
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

There are changes that simplify controller discovery:

新版本简化了控制器的发现：

The new :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` can be used to mark a class (and it's subclasses) as a controller. A class whose name doesn't end in ``Controller`` and derives from a base class that ends in ``Controller`` is no longer considered a controller. In this scenario, :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` must be applied to the derived class itself or to the base class.

新的 :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` 可用于将一个类（及其子类）标记为控制器。如果一个类的名字不以 ``Controller`` 结尾，但其基类以 ``Controller`` 结尾，那么这个类不会被视为控制器。在这种情况下，必须将 :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` 应用在这个派生类或其基类上。

A type is considered a controller if **all** the following conditions are met:

当满足以下**所有**条件时，类型才会被视作控制器：

- The type is a public, concrete, non-open generic class
- 该类型是 public 的、具体的、非开放泛型类
- :dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` is **not** applied to any type in its hierarchy
- 在其继承链中**没有**使用过 :dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` 特性
- The type name ends with ``Controller``, or :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` is applied to the type or one of its ancestors.
- 类型名以 ``Controller`` 结尾，或该类型（或其祖先）应用过 :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` 特性。

.. note:: If :dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` is applied anywhere in the type hierarchy, the discovery conventions will never consider that type or its descendants to be a controller. In other words, :dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` takes precedence over :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute`.

.. note:: 只要在该类型的任何一级使用了 :dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` 特性，那么发现约定（discovery conventions）就不会把该类型或其后代视作控制器。换而言之，:dn:class:`~Microsoft.AspNetCore.Mvc.NonControllerAttribute` 特性优先于 :dn:class:`~Microsoft.AspNetCore.Mvc.ControllerAttribute` 特性。

配置
-------------

The :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationSource` interface has been introduced to represent the configuration used to build an :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationProvider`. It is no longer possible to access the provider instances from :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder`, only the sources. This is intentional, and may cause loss of functionality as you can no longer do things like call ``Load`` on the provider instances.

:dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationSource` 接口已被引入用于代表配置，用于创建 :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationProvider` 。现在已经不能从 :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` 访问到提供程序实例，只能访问到配置源。这是故意这么设计的，尽管这样设计会导致功能损失，比如你不再能在提供程序实例上调用 ``Load`` 了。

File-based configuration providers support both relative and absolute paths to configuration files. If you want to specify file paths relative to your application's content root, you must call the :dn:method:`~Microsoft.Extensions.Configuration.FileConfigurationExtensions.SetBasePath` extension method on :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder`:

基于文件的配置提供程序同时支持配置文件的相对路径和绝对路径。如果你想指定相对于应用程序内容根（Content Root）的相对路径，你必须在 :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` 上调用 :dn:method:`~Microsoft.Extensions.Configuration.FileConfigurationExtensions.SetBasePath` 扩展方法：

.. code-block:: c#
  :emphasize-lines: 4

  public Startup(IHostingEnvironment env)
  {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json");
  }

变化时自动重载
^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``IConfigurationRoot.ReloadOnChanged`` extension method is no longer available. File-based configuration providers now provide extension methods to :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` that allow you to specify whether configuration from those providers should be reloaded when there are changes in their files. See :dn:method:`~Microsoft.Extensions.Configuration.JsonConfigurationExtensions.AddJsonFile`, :dn:method:`~Microsoft.Extensions.Configuration.XmlConfigurationExtensions.AddXmlFile` and :dn:method:`~Microsoft.Extensions.Configuration.IniConfigurationExtensions.AddIniFile` for details.

``IConfigurationRoot.ReloadOnChanged`` 扩展方法已不可用。基于文件的配置提供程序现在为 :dn:iface:`~Microsoft.Extensions.Configuration.IConfigurationBuilder` 提供了扩展方法，用来允许你指定是否在配置文件发生变化时、这些配置提供程序重新加载配置。具体可以查阅 :dn:method:`~Microsoft.Extensions.Configuration.JsonConfigurationExtensions.AddJsonFile` 、 :dn:method:`~Microsoft.Extensions.Configuration.XmlConfigurationExtensions.AddXmlFile` 以及 :dn:method:`~Microsoft.Extensions.Configuration.IniConfigurationExtensions.AddIniFile` 。

日志
-------

``LogLevel.Verbose`` has been renamed to :dn:field:`~Microsoft.Extensions.Logging.LogLevel.Trace` and is now considered less severe than :dn:field:`~Microsoft.Extensions.Logging.LogLevel.Debug`.

``LogLevel.Verbose`` 现已被重命名为 :dn:field:`~Microsoft.Extensions.Logging.LogLevel.Trace` ，且被定义为优先级低于 :dn:field:`~Microsoft.Extensions.Logging.LogLevel.Debug`。

The ``MinimumLevel`` property has been removed from :dn:iface:`~Microsoft.Extensions.Logging.ILoggerFactory`. Each logging provider now provides extension methods to :dn:iface:`~Microsoft.Extensions.Logging.ILoggerFactory` that allow specifying a minimum logging level. See :dn:method:`~Microsoft.Extensions.Logging.ConsoleLoggerExtensions.AddConsole`, :dn:method:`~Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions.AddDebug`, and :dn:method:`~Microsoft.Extensions.Logging.EventLoggerFactoryExtensions.AddEventLog` for details.

``MinimumLevel`` 属性已被从 :dn:iface:`~Microsoft.Extensions.Logging.ILoggerFactory` 中移除。每一个日志提供程序为 :dn:iface:`~Microsoft.Extensions.Logging.ILoggerFactory` 提供的扩展方法都允许指定一个最小的日志级别。具体可以查看 :dn:method:`~Microsoft.Extensions.Logging.ConsoleLoggerExtensions.AddConsole` 、 :dn:method:`~Microsoft.Extensions.Logging.DebugLoggerFactoryExtensions.AddDebug` 以及 :dn:method:`~Microsoft.Extensions.Logging.EventLoggerFactoryExtensions.AddEventLog` 。


Identity
--------

The signatures for the following methods or properties have changed:

下列方法或属性的签名发生变化：

===============================================================  ===========================================
ASP.NET 5 RC1                                                    ASP.NET Core 1.0
===============================================================  ===========================================
ExternalLoginInfo.ExternalPrincipal                              ExternalLoginInfo.Principal
User.IsSignedIn()                                                SignInManager.IsSignedIn(User)
UserManager.FindByIdAsync(HttpContext.User.GetUserId())          UserManager.GetUserAsync(HttpContext.User)
User.GetUserId()                                                 UserManager.GetUserId(User)
===============================================================  ===========================================

To use Identity in a view, add the following:

想在视图中使用 Identity，可以增加下面这段代码：

.. code-block:: c#

  @using Microsoft.AspNetCore.Identity
  @inject SignInManager<TUser> SignInManager
  @inject UserManager<TUser> UserManager

运行于 IIS
----------------

The package ``Microsoft.AspNetCore.IISPlatformHandler`` has been replaced by ``Microsoft.AspNetCore.Server.IISIntegration``.

``Microsoft.AspNetCore.IISPlatformHandler`` 包已被 ``Microsoft.AspNetCore.Server.IISIntegration`` 所取代。

HttpPlatformHandler has been replaced by the :doc:`ASP.NET Core Module (ANCM) </hosting/aspnet-core-module>`. The *web.config* file created by the *Publish to IIS tool* now configures IIS to the ANCM instead of HttpPlatformHandler to reverse-proxy requests.

HttpPlatformHandler 现在已经被 :doc:`ASP.NET Core Module (ANCM) </hosting/aspnet-core-module>` 取代。由 *Publish to IIS tool*  创建的 *web.config*  文件用于配制 IIS 的 AMCM，取代了先前 HttpPlatformHandler 的反向代理请求。

The ASP.NET Core Module must be configured in *web.config*:

ASP.NET Core Module 必须在 *web.config* 中进行配置：

.. code-block:: xml

  <configuration>
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified"/>
      </handlers>
      <aspNetCore processPath="%LAUNCHER_PATH%" arguments="%LAUNCHER_ARGS%"
                  stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout"
                  forwardWindowsAuthToken="false"/>
    </system.webServer>
  </configuration>

The *Publish to IIS tool* generates a correct *web.config*. See :doc:`/publishing/iis` for more details.

*Publish to IIS tool* 会创建一个正确的 *web.config*，具体可以查看 :doc:`/publishing/iis` 。

IIS integration middleware is now configured when creating the :dn:class:`Microsoft.AspNetCore.Hosting.WebHostBuilder`, and is no longer called in the ``Configure`` method of the ``Startup`` class:

当创建 :dn:class:`Microsoft.AspNetCore.Hosting.WebHostBuilder` 时 IIS 集成中间件（IIS integration middleware）会被配置，并且不再需要调用 ``Startup`` 类的 ``Configure`` 方法了：

.. code-block:: c#

  var host = new WebHostBuilder()
      .UseIISIntegration()
      .Build();

Web 部署的变化
^^^^^^^^^^^^^^^^^^

.. original -Delete ``<app name> - Web Deploy-publish.ps1``. This is a script generated by Visual Studio for web deploy. There is a version for ASP.NET 5 RC1 projects (which are DNX based) and a different script for ASP.NET Core 1.0 projects (which are dotnet based), and those are incompatible with each other. As such, when migrating to ASP.NET Core 1.0, you need to delete the old script and let Visual Studio generate a new one to ensure web deploy works for the migrated project.  

Delete any *<app name> - Web Deploy-publish.ps1* scripts created with Visual Studio web deploy using ASP.NET 5 RC1. The ASP.NET 5 RC1 scripts (which are DNX based) are not compatible with dotnet based scripts. Use Visual Studio to generate new web deploy scripts. 

删除所有由 Visual Studio 使用 ASP.NET 5 RC1 Web 部署所生成的 *<app name> - Web Deploy-publish.ps1* 脚本。ASP.NET 5 RC1 脚本（基于 DNX）已不被基于 dotnet 的脚本所兼容。使用 Visual Studio 来生成新的 Web 部署脚本。

applicationhost.config 变化
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. original -If ``applicationhost.config`` was created with ASP.NET 5 RC1 or an earlier release, in ASP.NET Core it will point to the wrong application folder. The ``applicationhost.config`` file will read ``wwwroot`` as the application folder and this is where IIS will look for the ``web.config`` file. But since the ``web.config`` file now goes in the ``approot``, IIS won't find the file and the user may not be able to start the appliation with IIS. 

An *applicationhost.config* file created with ASP.NET 5 RC1 will point ASP.NET Core to an invalid :ref:`content root <content-root-lbl>` location. With such a *applicationhost.config* file, ASP.NET Core will be configured with :ref:`content root <content-root-lbl>`/:ref:`web root <web-root-lbl>` as the :ref:`content root <content-root-lbl>` folder and therefore look for *web.config* in ``Content root/wwwroot``. The *web.config* file must be in the :ref:`content root <content-root-lbl>` folder. When configured like this, the app will terminate with an HTTP 500 error.

由 ASP.NET 5 RC1 创建的 *applicationhost.config* 文件将指示 ASP.NET Core 指向一个无效的 :ref:`Content root <content-root-lbl>` 位置。对于这种 *applicationhost.config* 文件，ASP.NET Core 会把 :ref:`Content root <content-root-lbl>`/:ref:`web root <web-root-lbl>` 配置为 :ref:`Content root <content-root-lbl>` 文件夹，因此会在 ``Content root/wwwroot`` 下寻找 *web.config* 。*web.config* 文件必须在 :ref:`content root <content-root-lbl>` 文件夹中。当使用这种配置时，应用将会发出一个 HTTP 500 错误信息并结束。

在 Visual Studio 中更新启动配置
-----------------------------------------

Update ``launchSettings.json`` to remove the web target and add the following:

更新 ``launchSettings.json`` 文件，移除 Web 指向并增加下面这些：

.. code-block:: json

  {
    "WebApplication1": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }

服务器垃圾回收 (GC)
------------------------------

You must turn on server garbage collection in *project.json* or *app.config* when running ASP.NET projects on the full .NET Framework:

当你的 ASP.NET 项目运行在完整的 .NET Framework （full .NET Framework）上时，你必须在 *project.json* 或 *app.config* 中开启服务器垃圾回收：

.. code-block:: json
 :emphasize-lines: 4

  {
    "runtimeOptions": {
      "configProperties": {
        "System.GC.Server": true
      }
    }
  }

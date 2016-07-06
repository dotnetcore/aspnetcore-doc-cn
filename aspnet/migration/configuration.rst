迁移配置
=======================

作者： `Steve Smith`_ and `Scott Addie`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对：

In the previous article, we began :doc:`migrating an ASP.NET MVC project to ASP.NET Core MVC <mvc>`. In this article, we migrate configuration.

在上文中，我们已着手 :doc:`将 ASP.NET MVC 项目迁移到 ASP.NET Core MVC <mvc>` ，本文我们将介绍如何迁移设置。

.. contents:: Sections:
  :local:
  :depth: 1
  
`查看或下载样例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/migration/configuration/samples>`__

设置 Configuration
-------------------

ASP.NET Core no longer uses the *Global.asax* and *web.config* files that previous versions of ASP.NET utilized. In earlier versions of ASP.NET, application startup logic was placed in an ``Application_StartUp`` method within *Global.asax*. Later, in ASP.NET MVC, a *Startup.cs* file was included in the root of the project; and, it was called when the application started. ASP.NET Core has adopted this approach completely placing all startup logic in the *Startup.cs* file.

ASP.NET Core 不再使用早前 ASP.NET 所使用的 *Global.asax* 和 *web.config* 文件。在之前的 ASP.NET 中，应用程序的启动逻辑位于 *Global.asax* 文件的 ``Application_StartUp`` 方法内。后来在 ASP.NET MVC 中，*Startup.cs* 文件被放置在项目的根路径下，当应用程序启动时就会调用它。ASP.NET Core 则将所有启动逻辑完全放置在 *Startup.cs* 文件中。

The *web.config* file has also been replaced in ASP.NET Core. Configuration itself can now be configured, as part of the application startup procedure described in *Startup.cs*. Configuration can still utilize XML files, but typically ASP.NET Core projects will place configuration values in a JSON-formatted file, such as *appsettings.json*. ASP.NET Core's configuration system can also easily access environment variables, which can provide a more secure and robust location for environment-specific values. This is especially true for secrets like connection strings and API keys that should not be checked into source control. See :doc:`/fundamentals/configuration` to learn more about configuration in ASP.NET Core.

*web.config* 文件在 ASP.NET Core 中已被取代。可以在 *Startup.cs* 中通过配置来描述应用程序启动过程。尽管仍可利用 XML 文件来配置，但通常来讲 ASP.NET Core 会把配置信息放在 JSON 文件内，比如 *appsettings.json* 文件。ASP.NET Core 的配置系统可以很便捷地访问环境变量，它可以为特定的环境提供更安全可靠的值。对于一些机密信息（如连接串以及 API Key 等）来讲这一点尤为重要，因为这些信息都不能签入源代码版本管理之中。了解更多有关 ASP.NET Core 配置信息请移步 :doc:`/fundamentals/configuration` 。

For this article, we are starting with the partially-migrated ASP.NET Core project from :doc:`the previous article <mvc>`. To setup configuration add the following constructor and property to the *Startup.cs* class located in the root of the project:

本文将基于 :doc:`上一篇文章 <mvc>` 继续迁移项目到 ASP.NET Core。找到项目根目录下的 *Startup.cs* 文件，在其中添加下面的构造函数和属性：

.. literalinclude:: configuration/samples/WebApp1/src/WebApp1/Startup.cs
  :language: c#
  :linenos:
  :lines: 15-24
  :dedent: 8
  
Note that at this point the *Startup.cs* file will not compile, as we still need to add the following ``using`` statement:

注意，此时 *Startup.cs* 文件还不能编译，我们还需要添加 ``using`` 声明：

.. code-block:: c#

  using Microsoft.Extensions.Configuration;

Add an *appsettings.json* file to the root of the project using the appropriate item template:

使用模板项在项目根目录下添加 *appsettings.json* 文件：

.. image:: configuration/_static/add-appsettings-json.png
    :width: 955px

从 web.config 迁移 Configuration 配置
----------------------------------------------

Our ASP.NET MVC project included the required database connection string in *web.config*, in the ``<connectionStrings>`` element. In our ASP.NET Core project, we are going to store this information in the *appsettings.json* file. Open *appsettings.json*, and note that it already includes the following:

在 ASP.NET MVC 项目的 `web.config` 文件中包含了所需请求的数据库连接字符串（位于 ``<connectionStrings>`` 节点内）。而在 ASP.NET Core MVC 中这些配置位于 *appsettings.json* 文件内。打开 *appsettings.json* 文件，请注意它已包含下列内容：

.. literalinclude:: configuration/samples/WebApp1/src/WebApp1/appsettings.json
  :language: json
  :emphasize-lines: 4
  :linenos:

In the highlighted line depicted above, change the name of the database from **_CHANGE_ME** to the name of your database.

请将上述代码高亮行中的数据库名称（**_CHANGE_ME**）改为你自己的数据库名。

总结
-------

ASP.NET Core places all startup logic for the application in a single file, in which the necessary services and dependencies can be defined and configured. It replaces the *web.config* file with a flexible configuration feature that can leverage a variety of file formats, such as JSON, as well as environment variables.

ASP.NET Core 将所有的启动逻辑都包含在应用程序的单个文件中，其中包括必须的服务、定义的依赖项以及配置。同时 *web.config* 也被替代为使用大量的文件格式（如 JSON）以及环境变量来灵活配置。
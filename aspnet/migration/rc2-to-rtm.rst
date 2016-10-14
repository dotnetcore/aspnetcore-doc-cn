从 ASP.NET Core RC2 迁移到 ASP.NET Core 1.0
===================================================

By `Cesar Blum Silveira`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对：

.. contents:: Sections:
  :local:
  :depth: 1

概述
--------

This migration guide covers migrating an ASP.NET Core RC2 application to ASP.NET Core 1.0.

本迁移指南主要涵盖了如何从 ASP.NET Core RC2 应用迁移到 ASP.NET Core 1.0 版本。

There weren't many significant changes to ASP.NET Core between the RC2 and 1.0 releases. For a complete list of changes, see the `ASP.NET Core 1.0 announcements <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0>`_.

从 ASP.NET Core RC2 升级到 1.0 releases 的过程中并没有过多的重大变化。查阅完整的变更清单可以移步 `ASP.NET Core 1.0 announcements <https://github.com/aspnet/announcements/issues?q=is%3Aopen+is%3Aissue+milestone%3A1.0.0>`_ 。

Install the new tools from https://dot.net/core and follow the instructions.

从 https://dot.net/core 安装新的工具，并按说明进行操作。

Update the global.json to 

更新 global.json 为

.. code-block:: javascript

  {
    "projects": [ "src", "test" ],
    "sdk": {
	"version": "1.0.0-preview2-003121"
    }
  }

工具
-----

For the tools we ship, you no longer need to use ``imports`` in *project.json*. For example:

对于我们所使用的工具，你不再需要在 *project.json* 中使用 ``imports`` 了。例：

.. code-block:: json

  {
    "tools": {
      "Microsoft.AspNetCore.Server.IISIntegration.Tools": {
        "version": "1.0.0-preview1-final",
        "imports": "portable-net45+win8+dnxcore50"
      }
    }
  }

Becomes:

变为：

.. code-block:: json

  {
    "tools": {
      "Microsoft.AspNetCore.Server.IISIntegration.Tools": "1.0.0-preview2-final"
    }
  }

托管服务
-------

The ``UseServer`` is no longer available for :dn:iface:`~Microsoft.AspNetCore.Hosting.IWebHostBuilder`. You must now use :dn:method:`~Microsoft.AspNetCore.Hosting.WebHostBuilderKestrelExtensions.UseKestrel` or :dn:method:`~Microsoft.AspNetCore.Hosting.WebHostBuilderWebListenerExtensions.UseWebListener`.

``UseServer`` 将不再支持 :dn:iface:`~Microsoft.AspNetCore.Hosting.IWebHostBuilder`，因此你必须使用 :dn:method:`~Microsoft.AspNetCore.Hosting.WebHostBuilderKestrelExtensions.UseKestrel` 或 :dn:method:`~Microsoft.AspNetCore.Hosting.WebHostBuilderWebListenerExtensions.UseWebListener` 。

ASP.NET MVC Core
----------------

The ``HtmlEncodedString`` class has been replaced by :dn:class:`~Microsoft.AspNetCore.Html.HtmlString` (contained in the  ``Microsoft.AspNetCore.Html.Abstractions`` package).

``HtmlEncodedString`` 类已经被 :dn:class:`~Microsoft.AspNetCore.Html.HtmlString` （包含于包 ``Microsoft.AspNetCore.Html.Abstractions`` 之中）所替换。

安全
--------

The :dn:class:`~Microsoft.AspNetCore.Authorization.AuthorizationHandler\<TRequirement>` class now only contains an asynchronous interface.

:dn:class:`~Microsoft.AspNetCore.Authorization.AuthorizationHandler\<TRequirement>` 类目前只包含异步接口。
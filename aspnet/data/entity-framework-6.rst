Getting Started with ASP.NET Core and Entity Framework 6
===========================================================

开始使用 ASP.NET Core 与 Entity Framework 6
===========================================================

By `Paweł Grudzień`_ and `Damien Pontifex`_

作者：`Paweł Grudzień`_ and `Damien Pontifex`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

This article will show you how to use Entity Framework 6 inside an ASP.NET Core application.

本篇文章将向你展示如何在 ASP.NET Core 应用中使用 Entity Framework 6。

.. contents:: Sections:
  :local:
  :depth: 1

Prerequisites
-------------

前提条件
---------------

Before you start, make sure that you compile against full .NET Framework in your project.json as Entity Framework 6 does not support .NET Core. If you need cross platform features you will need to upgrade to `Entity Framework Core`_.

在你开始之前，请确保你基于完整 .NET Framework 使用 project.json 来编译，因为 Entity Framework 6 不支持 .NET Core。如果你需要跨平台功能，你需要升级到 `Entity Framework Core`_。

In your project.json file specify a single target for the full .NET Framework:

.. code-block:: none

    "frameworks": {
        "net46": {}
    }

Setup connection strings and dependency injection
-------------------------------------------------

建立连接字符串与依赖注入
-------------------------

The simplest change is to explicitly get your connection string and setup dependency injection of your ``DbContext`` instance.

最简单的改变是显示地获取你的连接字符串并建立你的 ``DbContext`` 实例的依赖注入。

In your ``DbContext`` subclass, ensure you have a constructor which takes the connection string as so:

在你的 ``DbContext`` 子类，确保你有一个如下方式使用链接字符串的构造函数：

.. code-block:: c#
    :linenos:

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
    }

In the ``Startup`` class within ``ConfigureServices`` add factory method of your context with it's connection string. Context should be resolved once per scope to ensure performance and ensure reliable operation of Entity Framework.

在包含 ``ConfigureServices`` 的 ``Startup`` 类添加你的包含连接字符串的数据上下文的工厂方法。数据上下文应该在每个作用范围内实例化以确保 Entity Framework 性能与运行可靠性。

.. code-block:: c#
    :linenos:

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(() => new ApplicationDbContext(Configuration["Data:DefaultConnection:ConnectionString"]));

        // Configure remaining services
    }

Migrate configuration from config to code
-----------------------------------------

将配置从配置文件迁移到代码中
---------------------------

Entity Framework 6 allows configuration to be specified in xml (in web.config or app.config) or through code. As of ASP.NET Core, all configuration is code-based.

Entity Framework 6 允许从 xml （如 web.config 或 app.config）中识别配置。至于 ASP.NET Core ，所有配置都是基于代码的。

Code-based configuration is achieved by creating a subclass of ``System.Data.Entity.Config.DbConfiguration`` and applying ``System.Data.Entity.DbConfigurationTypeAttribute`` to your ``DbContext`` subclass.

基于代码的配置是由创建 ``System.Data.Entity.Config.DbConfiguration`` 的子类，并且在你的 ``DbContext`` 中应用  ``System.Data.Entity.DbConfigurationTypeAttribute`` 特性。

Our config file typically looked like this:

我们典型的配置文件如下面这样：

.. code-block:: xml
    :linenos:

    <entityFramework>
        <defaultConnectionFactory type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory, EntityFramework">
            <parameters>
                <parameter value="mssqllocaldb" />
            </parameters>
        </defaultConnectionFactory>
        <providers>
            <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
        </providers>
    </entityFramework>

The ``defaultConnectionFactory`` element sets the factory for connections. If this attribute is not set then the default value is ``SqlConnectionProvider``. If, on the other hand, value is provided, the given class will be used to create ``DbConnection`` with its ``CreateConnection`` method. If the given factory has no default constructor then you must add parameters that are used to construct the object.

 ``defaultConnectionFactory`` 元素设置连接的工厂。如果这个属性没有被设置，其默认值将是 ``SqlConnectionProvider``。另一方面来说，如果值是被提供的，将使用所给的类的 ``CreateConnection`` 方法创建 ``DbConnection``。。如果所给的工厂没有默认构造函数，你必须添加构造函数需要的参数对象。

.. code-block:: c#
    :linenos:

    [DbConfigurationType(typeof(CodeConfig))] // point to the class that inherit from DbConfiguration
    public class ApplicationDbContext : DbContext
    {
        [...]
    }

    public class CodeConfig : DbConfiguration
    {
        public CodeConfig()
        {
            SetProviderServices("System.Data.SqlClient",
                System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }

SQL Server, SQL Server Express and LocalDB
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

SQL Server, SQL Server Express 与 LocalDB
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

This is the default and so no explicit configuration is needed. The above ``CodeConfig`` class can be used to explicitly set the provider services and the appropriate connection string should be passed to the ``DbContext`` constructor as shown `above <#setup-connection-strings-and-dependency-injection>`_.

这是默认的，如此并不需要显示的配置。以上 ``CodeConfig`` 类可以被用来显示设置提供程序的服务，且恰当的连接字符串应该被传入 ``DbContext`` 构造函数，就像 `以上 <#setup-connection-strings-and-dependency-injection>`_所展示的那样。

Summary
-------

总述
-------

Entity Framework 6 is an object relational mapping (ORM) library, that is capable of mapping your classes to database entities with little effort. These features made it very popular so migrating large portions of code may be undesirable for many projects. This article shows how to avoid migration to focus on other new features of ASP.NET.

Entity Framework 6 是一个对象关系映射（ORM）库，其具有仅需很小的投入，就可以映射你的类到数据库实体对象的能力。这些特性使其非常受欢迎，因为对很多项目都不期望迁移大量代码。这篇文章展示如何避免迁移，而将注意力放在其它新的 ASP.NET 特性上。

Additional Resources
--------------------

其他资源
----------

- `Entity Framework - Code-Based Configuration <https://msdn.microsoft.com/en-us/data/jj680699.aspx>`_

- `Entity Framework - 基于代码的配置 <https://msdn.microsoft.com/en-us/data/jj680699.aspx>`_

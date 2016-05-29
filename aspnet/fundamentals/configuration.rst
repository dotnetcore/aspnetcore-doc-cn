.. _fundamentals-configuration:

Configuration
=============

配置
=============


作者： `Steve Smith`_, `Daniel Roth`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： 

ASP.NET Core supports a variety of different configuration options. Application configuration data can come from files using built-in support for JSON, XML, and INI formats, as well as from environment variables. You can also write your own :ref:`custom configuration provider <custom-config-providers>`.

ASP.NET Core 支持多种配置选项。应用程序配置数据内建支持读取 JSON、XML 和 INI 格式的配置文件和环境变量。你也编写自己的:ref:`自定义配置提供程序 <custom-config-providers>`。

.. contents:: Sections:
  :local:
  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/docs/tree/master/aspnet/fundamentals/configuration/sample>`__

`访问或下载样例代码 <https://github.com/aspnet/docs/tree/master/aspnet/fundamentals/configuration/sample>`__

Getting and setting configuration settings
------------------------------------------

获取和设置配置
------------------------------------------

ASP.NET Core's configuration system has been re-architected from previous versions of ASP.NET, which relied on ``System.Configuration`` and XML configuration files like ``web.config``. The new :doc:`configuration model </fundamentals/configuration>` provides streamlined access to key/value based settings that can be retrieved from a variety of providers. Applications and frameworks can then access configured settings using the new :ref:`Options pattern <options-config-objects>`

To work with settings in your ASP.NET application, it is recommended that you only instantiate an instance of ``Configuration`` in your application's ``Startup`` class. Then, use the :ref:`Options pattern <options-config-objects>` to access individual settings.

At its simplest, the ``Configuration`` class is just a collection of ``Providers``, which provide the ability to read and write name/value pairs. You must configure at least one provider in order for ``Configuration`` to function correctly. The following sample shows how to test working with ``Configuration`` as a key/value store:

.. code-block:: c#
  :linenos:

  // assumes using Microsoft.Framework.ConfigurationModel is specified
  var builder = new ConfigurationBuilder();
  builder.Add(new MemoryConfigurationProvider());
  var config = builder.Build();
  config.Set("somekey", "somevalue");

  // do some other work

  string setting2 = config["somekey"]; // also returns "somevalue"

.. note:: You must set at least one configuration provider.

.. note:: 你必须至少设置一个配置提供程序。

It's not unusual to store configuration values in a hierarchical structure, especially when using external files (e.g. JSON, XML, INI). In this case, configuration values can be retrieved using a ``:`` separated key, starting from the root of the hierarchy. For example, consider the following *appsettings.json* file:

.. _config-json:

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/appsettings.json
  :linenos:
  :language: json

The application uses configuration to configure the right connection string. Access to the ``ConnectionString`` setting is achieved through this key: ``Data:DefaultConnection:ConnectionString``.

The settings required by your application and the mechanism used to specify those settings (configuration being one example) can be decoupled using the :ref:`options pattern <options-config-objects>`. To use the options pattern you create your own settings class (probably several different classes, corresponding to different cohesive groups of settings) that you can inject into your application using an options service. You can then specify your settings using configuration or whatever mechanism you choose.

.. note:: You could store your ``Configuration`` instance as a service, but this would unnecessarily couple your application to a single configuration system and specific configuration keys. Instead, you can use the :ref:`Options pattern <options-config-objects>` to avoid these issues.

Using the built-in providers
----------------------------

使用内建提供程序
----------------------------

The configuration framework has built-in support for JSON, XML, and INI configuration files, as well as support for in-memory configuration (directly setting values in code) and the ability to pull configuration from environment variables and command line parameters. Developers are not limited to using a single configuration provider. In fact several may be set up together such that a default configuration is overridden by settings from another provider if they are present.

配置框架已内建支持 JSON、XML 和 INI 配置文件，内存配置（直接通过代码设置值），从环境变量和命令行参数中拉取配置。开发者并不受限于必须使用单个配置提供程序。事实上可以把多个配置提供程序组合在一起，就像是用从当前存在的另一个配置提供程序中获取配置值覆盖默认配置一样。

Adding support for additional configuration file providers is accomplished through extension methods. These methods can be called on a ``ConfigurationBuilder`` instance in a standalone fashion, or chained together as a fluent API, as shown.

扩展方法支持为配置添加额外的配置文件提供程序。这些方法能被独立的或链式的（如 fluent API）调用在 ``ConfigurationBuilder`` 实例之上，如下所示：

.. _custom-config:

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/Program.cs
  :linenos:
  :dedent: 12
  :language: c#
  :lines: 14-17

The order in which configuration providers are specified is important, as this establishes the precedence with which settings will be applied if they exist in multiple locations. In the example above, if the same setting exists in both *appsettings.json* and in an environment variable, the setting from the environment variable will be the one that is used. The last configuration provider specified "wins" if a setting exists in more than one location. The ASP.NET team recommends specifying environment variables last, so that the local environment can override anything set in deployed configuration files.

指定配置提供程序的顺序非常重要，这将影响它们的设置被应用的优先级（如果存在于多个位置）。上例中，如果相同的配置同时存在于 *appsettings.json* 和环境变量，则环境变量的设置将被最终使用。最后指定的配置提供程序将“获胜”（如果该设置存在于至少两处位置）。ASP.NET 团队建议最后指定环境变量，如此一来本地环境可以覆盖任何部署在配置文件中的设置。

.. note:: To override nested keys through environment variables in shells that don't support ``:`` in variable names replace them with ``__`` (double underscore).

.. note:: 如果通过环境变量重写嵌套键，请把变量中键名的 ``:`` 替换为 ``__`` （两个下划线）。

It can be useful to have environment-specific configuration files. This can be achieved using the following:

这对于指定环境的配置文件非常有用，这能通过以下代码来实现：

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :linenos:
  :dedent: 8
  :language: c#
  :lines: 19-34
  :emphasize-lines: 1,6

The ``IHostingEnvironment`` service is used to get the current environment. In the ``Development`` environment, the highlighted line of code above would look for a file named ``appsettings.Development.json`` and use its values, overriding any other values, if it's present. Learn more about :doc:`environments`.

``IHostingEnvironment`` 服务用于获取当前环境。在 ``Development`` 环境中，上例高亮行代码将寻找名为 ``appsettings.Development.json`` 的配置文件，并用其中的值覆盖当前存在的其它值。更多请参见 :doc:`environments` 。

.. warning:: You should never store passwords or other sensitive data in provider code or in plain text configuration files. You also shouldn't use production secrets in your development or test environments. Instead, such secrets should be specified outside the project tree, so they cannot be accidentally committed into the provider repository. Learn more about :doc:`environments` and managing :doc:`/security/app-secrets`.

.. warning:: 谨记，严禁把密码或其他敏感数据保存在代码或纯文本配置文件中，严谨在开发环境或测试环境中使用生产环境的机密数据（这些机密数据应当在项目树的外部被指定，这样就不会意外提交到仓库内）。移步了解更多 :doc:`environments` 和管理 :doc:`/security/app-secrets` 。

One way to leverage the order precedence of ``Configuration`` is to specify default values, which can be overridden. In this simple console application, a default value for the ``username`` setting is specified in a ``MemoryConfigurationProvider``, but this is overridden if a command line argument for ``username`` is passed to the application. You can see in the output how many configuration providers are configured at each stage of the program.

影响 ``Configuration`` 优先级顺序的一个因素是指定可被重写的默认值。在本控制台应用程序中，默认的 ``username`` 设置由 ``MemoryConfigurationProvider`` 指定，但如果命令行参数中有个 ``username`` 参数被传递给应用程序，它将被覆盖。在输出中可以看到程序的每一个步骤中有多少个配置提供程序在进行配置工作。

.. literalinclude:: configuration/sample/src/ConfigConsole/Program.cs
  :linenos:
  :language: c#

When run, the program will display the default value unless a command line parameter overrides it.

当运行时，程序将显示默认值，除非使用命令行参数重写之。

.. image:: configuration/_static/config-console.png

.. _options-config-objects:

Using Options and configuration objects
---------------------------------------

使用选项和配置对象
---------------------------------------

Using the options pattern you can easily convert any class (or POCO - Plain Old CLR Object) into a settings class. It's recommended that you create well-factored settings objects that correspond to certain features within your application, thus following the Interface Segregation Principle (ISP) (classes depend only on the configuration settings they use) as well as Separation of Concerns (settings for disparate parts of your app are managed separately, and thus are less likely to negatively impact one another).

通过使用选择模式（options pattern）你可将任何类或 POCO（Plain Old CLR Object）转换为设置类。推荐把你创建的配置根据应用程序的功能分解为多个配置对象，从而实现 ISP（Interface Segregation Principle，接口隔离原则，类只依赖于它们自己使用的配置设置）和 SoC（Separation of Concerns，关注分离，设置与应用程序相互隔离，减少彼此之间的干扰和影响）。

A simple ``MyOptions`` class is shown here:

一个简单的 ``MyOptions`` 类如下所示：

.. literalinclude:: configuration/sample/src/UsingOptions/Models/MyOptions.cs
  :linenos:
  :language: c#
  :lines: 8-12
  :dedent: 4

Options can be injected into your application using the ``IOptions<TOptions>`` service. For example, the following :doc:`controller </mvc/controllers/index>`  uses ``IOptions<MyOptions>`` to access the settings it needs to render the ``Index`` view:

通过 ``IOptions<TOptions>`` ，配置选项将被注入到应用程序中。比方说，如 :doc:`controller </mvc/controllers/index>` 使用 ``IOptions<TOptions>`` 来访问需要在 ``Index`` 视图中渲染的配置：

.. literalinclude:: configuration/sample/src/UsingOptions/Controllers/HomeController.cs
  :linenos:
  :language: c#
  :lines: 9-24
  :dedent: 4

Learn more about :doc:`dependency-injection`.

更多请浏览 :doc:`dependency-injection` 。

To setup the ``IOptions<TOption>`` service you call the ``AddOptions()`` extension method during startup in your ``ConfigureServices`` method:

为设置 ``IOptions<TOption>`` 服务，你需在启动期间在 ``ConfigureServices`` 方法内调用 ``AddOptions()`` 扩展方法。

.. literalinclude:: configuration/sample/src/UsingOptions/Startup.cs
  :linenos:
  :language: c#
  :lines: 24-27,41
  :dedent: 8

.. _options-example:

The ``Index`` view displays the configured options:

``Index`` 视图将显示配置选项：

.. image:: configuration/_static/index-view.png

You configure options using the ``Configure<TOption>`` extension method. You can configure options using a delegate or by binding your options to configuration:

配置选项使用 ``Configure<TOption>`` 扩展方法。你可以通过委托或绑定配置选项的方式来进行配置：

.. literalinclude:: configuration/sample/src/UsingOptions/Startup.cs
  :language: c#
  :linenos:
  :lines: 24-40
  :dedent: 8
  :emphasize-lines: 7,10-13

When you bind options to configuration each property in your options type is bound to a configuration key of the form ``property:subproperty:...``. For example, the ``MyOptions.Option1`` property is bound to the key ``Option1``, which is read from the ``option1`` property in *appsettings.json*. Note that configuration keys are case insensitive.

当你通过绑定选项来配置选项类型的每一个属性，实际上是绑定到每一个配置键（比如 ``property:subproperty:...``）。比方说，``MyOptions.Option1`` 属性绑定到键 ``Option1``，那么就会从 *appsettings.json* 中读取 ``option1`` 属性。注意，配置键是大小写不敏感的。

Each call to ``Configure<TOption>`` adds an ``IConfigureOptions<TOption>`` service to the service container that is used by the ``IOptions<TOption>`` service to provide the configured options to the application or framework. If you want to configure your options some other way (ex. reading settings from a data base) you can use the ``ConfigureOptions<TOptions>`` extension method to you specify a custom ``IConfigureOptions<TOption>`` service directly. 

通过调用 ``Configure<TOption>`` 将一个个 ``IConfigureOptions<TOption>`` 服务加入服务容器，是为了之后应用程序或框架能通过 ``IOptions<TOption>`` 服务来获取配置选项。若是想从其他途径（比如之前从数据库）获取配置，你可使用 ``ConfigureOptions<TOptions>`` 扩展方法直接指定经过定制的 ``IConfigureOptions<TOption>`` 服务。

You can have multiple ``IConfigureOptions<TOption>`` services for the same option type and they are all applied in order. In the :ref:`example <options-example>` above value of Option1 and Option2 are both specified in `appsettings.json`, but the value of Option1 is overridden by the configured delegate.

同一个选项类型可以有多个 ``IConfigureOptions<TOption>`` 服务，届时将按顺序应用。在上 :ref:`例 <options-example>` 中，Option1 和 Option2 都在 `appsettings.json` 中指定，但 Option1 的值最后被配置委托所覆盖。

.. _custom-config-providers:

Writing custom providers
------------------------

编写自定义提供程序
------------------------

In addition to using the built-in configuration providers, you can also write your own. To do so, you simply inherit from ``ConfigurationProvider``, and populate the ``Data`` property with the settings from your configuration provider.

除使用内建的配置提供程序，你也可自行定制。为此，你只需从 ``ConfigurationProvider`` 继承并使用来自你配置提供程序的配置来填充 ``Data`` 属性即可。

Example: Entity Framework Settings
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

例子：Entity Framework 设置
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You may wish to store some of your application's settings in a database, and access them using Entity Framework (EF). There are many ways in which you could choose to store such values, ranging from a simple table with a column for the setting name and another column for the setting value, to having separate columns for each setting value. In this example, I'm going to create a simple configuration provider that reads name-value pairs from a database using EF.

你或许希望将应用程序的配置保存在数据库中，然后通过 EntityFramework（EF）来访问它们。保存这些配置值有很多办法可以选择，比方说一张简易表格，一列表示配置名、另一列表示配置的值。在本例中，我将创建一个简易的配置提供程序，通过 EF 从数据库中读取名值对（name-value pair）。

To start off we'll define a simple ``ConfigurationValue`` entity for storing configuration values in the database:

在开始之前我们先定义一个简单的 ``ConfigurationValue`` 实体模型用来表示存储在数据库中的配置值。

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/ConfigurationValue.cs
  :linenos:
  :language: c#
  :lines: 8-12
  :dedent: 4

We also need a ``ConfigurationContext`` to store and access the configured values using EF:

然后需要一个 ``ConfigurationContext`` 用来通过 EF 存储和访问配置值

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/ConfigurationContext.cs
  :linenos:
  :language: c#
  :lines: 6-17
  :dedent: 4

Next, create the custom configuration provider by inheriting from ``ConfigurationProvider``. The configuration data is loaded by overriding the ``Load`` method, which reads in all of the configuration data from the configured database. For demonstration purposes, the configuration provider also takes care of initializing the database if it hasn't already been created and populated:

接着，通过继承 ``ConfigurationProvider`` 创建一个定制的配置提供程序。配置数据由重写的 ``Load`` 方法（该方法将从配置数据库中读取所有配置数据）所提供。由于这是演示，所以配置提供程序需要自行初始化数据库（如果该数据库尚未创建并初始化的话）。

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/EntityFrameworkConfigurationProvider.cs
  :linenos:
  :language: c#
  :lines: 10-46
  :dedent: 4

By convention we also add an ``AddEntityFramework`` extension method for adding the configuration provider:

按惯例，我们同样可以添加一个 ``AddEntityFramework`` 扩展方法来增加配置提供程序：

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/EntityFrameworkConfigurationProvider.cs
  :linenos:
  :language: c#
  :lines: 48-54
  :dedent: 4

You can see an example of how to use this custom ``ConfigurationProvider`` in your application in the following example. Create a new ``ConfigurationBuilder`` to setup your configuration providers. To add the ``EntityFrameworkConfigurationProvider`` you first need to specify the data provider and connection string. How should you configure the connection string? Using configuration of course! Add an *appsettings.json* file as a configuration provider to bootstrap setting up the ``EntityFrameworkConfigurationProvider``. By reusing the same ``ConfigurationBuilder`` any settings specified in the database will override settings specified in *appsettings.json*:

在下例中将演示如何在应用程序中使用此定制的 ``ConfigurationProvider``。创建一个新的 ``ConfigurationBuilder`` 来设置配置提供程序。指定数据提供程序和连接字符串后，添加 ``EntityFrameworkConfigurationProvider`` 配置提供程序。那你如何配置连接字符串呢？当然也是使用配置了！添加 *appsettings.json* 作为配置提供者来引导建立 ``EntityFrameworkConfigurationProvider`` 。通过重用相同的 ``ConfigurationBuilder``，在数据库中指定的任何配置都将覆盖 *appsettings.json* 中所指定的对应设置：

.. literalinclude:: configuration/sample/src/CustomConfigurationProvider/Program.cs
  :linenos:
  :language: c#
  :lines: 7-25
  :dedent: 4

Run the application to see the configured values:

运行程序，看到所配置的值。

.. image:: configuration/_static/custom-config.png

Summary
-------

总结
-------

ASP.NET Core provides a very flexible configuration model that supports a number of different file-based options, as well as command-line, in-memory, and environment variables. It works seamlessly with the options model so that you can inject strongly typed settings into your application or framework. You can create your own custom configuration providers as well, which can work with or replace the built-in providers, allowing for extreme flexibility. 

ASP.NET Core 提供了非常灵活的配置模型，支持多种配置文件类型、命令行、内存和环境变量。它能与配置模型无缝协作，因此你可为你的应用程序或框架注入强类型配置。你也可以创建自己定制的配置提供程序，用于协同或取代内置提供程序，保证了最大程序的灵活性。
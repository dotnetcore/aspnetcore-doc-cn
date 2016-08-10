:version: 1.0.0

.. _dependency-injection-controllers:

Dependency Injection and Controllers
====================================
依赖注入和控制器
====================================

作者： `Steve Smith`_
翻译： `刘浩杨 <http://github.com/liuhaoyang>`_

ASP.NET Core MVC controllers should request their dependencies explicitly via their constructors. In some instances, individual controller actions may require a service, and it may not make sense to request at the controller level. In this case, you can also choose to inject a service as a parameter on the action method.

ASP.NET Core MVC 控制器应通过它们的构造器明确的请求它们的依赖关系。在某些情况下，单个控制器的操作可能需要一个服务，在控制器级别上的请求可能没有意义。在这种情况下，你也可以选择将服务作为 action 方法的参数。

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/dependency-injection/sample>`__

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/dependency-injection/sample>`__

Dependency Injection
--------------------
依赖注入
----------

Dependency injection is a technique that follows the `Dependency Inversion Principle <http://deviq.com/dependency-inversion-principle>`_, allowing for applications to be composed of loosely coupled modules. ASP.NET Core has built-in support for :doc:`dependency injection </fundamentals/dependency-injection>`, which makes applications easier to test and maintain.

依赖注入（Dependency injection，DI）是一种如 `Dependency Inversion Principle <http://deviq.com/dependency-inversion-principle>`_ 所示的技术，允许应用程序由松散耦合的模块组成。ASP.NET Core 内置了 :doc:`dependency injection </fundamentals/dependency-injection>`，这使得应用程序更容易测试和维护。

Constructor Injection
---------------------
构造器注入
-----------

ASP.NET Core's built-in support for constructor-based dependency injection extends to MVC controllers. By simply adding a service type to your controller as a constructor parameter, ASP.NET Core will attempt to resolve that type using its built in service container. Services are typically, but not always, defined using interfaces. For example, if your application has business logic that depends on the current time, you can inject a service that retrieves the time (rather than hard-coding it), which would allow your tests to pass in implementations that use a set time.

ASP.NET Core 内置的基于构造器的依赖注入支持扩展到 MVC 控制器。通过只添加一个服务类型作为构造器参数到你的控制器中，ASP.NET Core 将会尝试使用内置的服务容器解析这个类型。服务通常是，但不总是使用接口来定义。例如，如果你的应用程序存在取决于当前时间的业务逻辑，你可以注入一个检索时间的服务（而不是对它硬编码），这将允许你的测试通过一个使用设置时间的实现。

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Interfaces/IDateTime.cs
  :language: c#

Implementing an interface like this one so that it uses the system clock at runtime is trivial:

实现这样一个接口，它在运行时使用的系统时钟是微不足道的：

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Services/SystemDateTime.cs
  :language: c#

With this in place, we can use the service in our controller. In this case, we have added some logic to the ``HomeController`` ``Index`` method to display a greeting to the user based on the time of day.

有了这些代码，我们可以在我们的控制器中使用这个服务。在这个例子中，我们在 ``HomeController`` 的 ``Index`` 方法中加入一些根据一天中的时间向用户显示问候的逻辑。

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Controllers/HomeController.cs
  :language: c#
  :emphasize-lines: 8,10,12,17-30
  :lines: 1-31,51-52

If we run the application now, we will most likely encounter an error::

如果我们现在运行应用程序，我们将可能遇到一个异常：

  An unhandled exception occurred while processing the request.

  InvalidOperationException: Unable to resolve service for type 'ControllerDI.Interfaces.IDateTime' while attempting to activate 'ControllerDI.Controllers.HomeController'.
  Microsoft.Extensions.DependencyInjection.ActivatorUtilities.GetService(IServiceProvider sp, Type type, Type requiredBy, Boolean isDefaultParameterRequired)

This error occurs when we have not configured a service in the ``ConfigureServices`` method in our ``Startup`` class. To specify that requests for ``IDateTime`` should be resolved using an instance of ``SystemDateTime``, add the highlighted line in the listing below to your ``ConfigureServices`` method:

这个错误发生时，我们没有在我们的 ``Startup`` 类的 ``ConfigureServices`` 方法中配置服务。要指定请求 ``IDateTime`` 时使用 ``SystemDateTime`` 的实例来解析，添加下面的清单中高亮的行到你的 ``ConfigureServices`` 方法中：

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Startup.cs
  :language: c#
  :emphasize-lines: 4
  :lines: 26-27,42-44
  :dedent: 8
  
.. note:: This particular service could be implemented using any of several different lifetime options (``Transient``, ``Scoped``, or ``Singleton``). See :doc:`/fundamentals/dependency-injection` to understand how each of these scope options will affect the behavior of your service.

.. note:: 这个特殊的服务可以使用任何不同的生命周期选项来实现（``Transient``, ``Scoped``, or ``Singleton``）。参考 :doc:`/fundamentals/dependency-injection` 来了解每一个作用域选项将如何影响你的服务的行为。

Once the service has been configured, running the application and navigating to the home page should display the time-based message as expected:

一旦服务被配置，运行应用程序并且导航到首页应该显示预期的基于时间的消息：

.. image:: dependency-injection/_static/server-greeting.png

.. tip:: See :doc:`/mvc/controllers/testing` to learn how to explicitly request dependencies `<http://deviq.com/explicit-dependencies-principle>`_ in controllers makes code easier to test.

.. tip:: 参考 :doc:`/mvc/controllers/testing` 学习如何在控制器中显示请求依赖关系 `<http://deviq.com/explicit-dependencies-principle>`_ 让代码更容易测试。

ASP.NET Core's built-in dependency injection supports having only a single constructor for classes requesting services. If you have more than one constructor, you may get an exception stating::

ASP.NET Core 内置的依赖注入支持用于请求服务的类型只有一个构造器。如果你有多于一个构造器，你可能会得到一个异常描述：

  An unhandled exception occurred while processing the request.

  InvalidOperationException: Multiple constructors accepting all given argument types have been found in type 'ControllerDI.Controllers.HomeController'. There should only be one applicable constructor.
  Microsoft.Extensions.DependencyInjection.ActivatorUtilities.FindApplicableConstructor(Type instanceType, Type[] argumentTypes, ConstructorInfo& matchingConstructor, Nullable`1[]& parameterMap)

As the error message states, you can correct this problem having just a single constructor. You can also :ref:`replace the default dependency injection support with a third party implementation <replacing-the-default-services-container>`, many of which support multiple constructors.

作为错误消息状态，你可以纠正只有一个构造器的问题。你也可以参考 :ref:`replace the default dependency injection support with a third party implementation <replacing-the-default-services-container>` 支持多个构造器。

Action Injection with FromServices
----------------------------------
Action 注入和 FromServices
-------------------------------

Sometimes you don't need a service for more than one action within your controller. In this case, it may make sense to inject the service as a parameter to the action method. This is done by marking the parameter with the attribute ``[FromServices]`` as shown here:

有时候在你的控制器中你不需要为超过一个 Action 使用的服务。在这种情况下，将服务作为 Action 方法的一个参数是有意义的。这是通过使用特性 ``[FromServices]`` 标记参数实现，如下所示：

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Controllers/HomeController.cs
  :language: c#
  :emphasize-lines: 1
  :lines: 33-38
  :dedent: 8
  
Accessing Settings from a Controller
------------------------------------
从控制器访问设置
-----------------

Accessing application or configuration settings from within a controller is a common pattern. This access should use the Options pattern described in :doc:`configuration </fundamentals/configuration>`. You generally should not request settings directly from your controller using dependency injection. A better approach is to request an ``IOptions<T>`` instance, where ``T`` is the configuration class you need.

在控制器中访问应用程序设置或配置设置是一个常见的模式。此访问应当使用在 :doc:`configuration </fundamentals/configuration>` 所描述的访问模式。你通常不应该从你的控制器中使用依赖注入直接请求设置。更好的方式是请求 ``IOptions<T>`` 实例， ``T`` 是你需要的配置类型。

To work with the options pattern, you need to create a class that represents the options, such as this one:

要使用选项模式，你需要创建一个表示选项的类型，如：

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Model/SampleWebSettings.cs
  :language: c#

Then you need to configure the application to use the options model and add your configuration class to the services collection in ``ConfigureServices``:

然后你需要配置应用程序使用选项模型，在 ``ConfigureServices`` 中添加你的配置类到服务集合中：

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Startup.cs
  :language: c#
  :emphasize-lines: 3-6,9,16,19
  :lines: 14-44
  :dedent: 8
  
.. note:: In the above listing, we are configuring the application to read the settings from a JSON-formatted file. You can also configure the settings entirely in code, as is shown in the commented code above. See :doc:`/fundamentals/configuration` for further configuration options.

.. note:: 在上面的清单中，我们配置应用程序程序从一个 JSON 格式的文件中读取设置。你也可以完全在代码中配置设置，像上面的代码中所显示的。参考 :doc:`/fundamentals/configuration` 更多的配置选项。

Once you've specified a strongly-typed configuration object (in this case, ``SampleWebSettings``) and added it to the services collection, you can request it from any Controller or Action method by requesting an instance of ``IOptions<T>`` (in this case, ``IOptions<SampleWebSettings>``). The following code shows how one would request the settings from a controller:

一旦你指定了一个强类型的配置对象（在这个例子中，``SampleWebSettings``），并且添加到服务集合中，你可以从任何控制器或 Action 方法中请求  ``IOptions<T>`` 的实例（在这个例子中， ``IOptions<SampleWebSettings>``）。下面的代码演示了如何从控制器中请求设置。

.. literalinclude:: dependency-injection/sample/src/ControllerDI/Controllers/SettingsController.cs
  :language: c#
  :emphasize-lines: 3,5,7
  :lines: 7-22

Following the Options pattern allows settings and configuration to be decoupled from one another, and ensures the controller is following `separation of concerns <http://deviq.com/separation-of-concerns/>`_, since it doesn't need to know how or where to find the settings information. It also makes the controller easier to unit test :doc:`/mvc/controllers/testing`, since there is no `static cling <http://deviq.com/static-cling/>`_ or direct instantiation of settings classes within the controller class.

遵循选项模式允许设置和配置互相分离，确保控制器遵循 `separation of concerns <http://deviq.com/separation-of-concerns/>`_ ，因为它不需要知道如何或者在哪里找到设置信息。由于没有 `static cling <http://deviq.com/static-cling/>`_ 或在控制器中直接实例化设置类，这也使得控制器更容易单元测试 :doc:`/mvc/controllers/testing`。

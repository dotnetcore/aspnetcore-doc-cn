.. _fundamentals-dependency-injection:

Dependency Injection
====================
依赖注入
========

原文：`Dependency Injection <https://docs.asp.net/en/latest/fundamentals/dependency-injection.html>`_

作者：`Steve Smith`_

翻译：`刘浩杨 <http://github.com/liuhaoyang>`_

校对：`许登洋(Seay) <https://github.com/SeayXu>`_

ASP.NET Core is designed from the ground up to support and leverage dependency injection. ASP.NET Core applications can leverage built-in framework services by having them injected into methods in the Startup class, and application services can be configured for injection as well. The default services container provided by ASP.NET Core provides a minimal feature set and is not intended to replace other containers.

ASP.NET Core 的底层设计支持和使用依赖注入。ASP.NET Core 应用程序可以利用内置的框架服务将它们注入到启动类的方法中，并且应用程序服务能够配置注入。由 ASP.NET Core 提供的默认服务容器提供了最小功能集并且不是要取代其他容器。

.. contents:: Sections 节：
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/dependency-injection/sample>`__

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/dependency-injection/sample>`__

What is Dependency Injection?
-----------------------------
什么是依赖注入？
---------------

Dependency injection (DI) is a technique for achieving loose coupling between objects and their collaborators, or dependencies. Rather than directly instantiating collaborators, or using static references, the objects a class needs in order to perform its actions are provided to the class in some fashion. Most often, classes will declare their dependencies via their constructor, allowing them to follow the `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle/>`_. This approach is known as "constructor injection".

依赖注入（Dependency injection，DI）是一种实现对象及其合作者或依赖项之间松散耦合的技术。将类用来执行其操作（Action）的这些对象以某种方式提供给该类，而不是直接实例化合作者或使用静态引用。通常，类会通过它们的构造函数声明其依赖关系，允许它们遵循 `显示依赖原则 (Explicit Dependencies Principle) <http://deviq.com/explicit-dependencies-principle/>`_ 。这种方法被称为 “构造函数注入（constructor injection）”。

When classes are designed with DI in mind, they are more loosely coupled because they do not have direct, hard-coded dependencies on their collaborators. This follows the `Dependency Inversion Principle <http://deviq.com/dependency-inversion-principle/>`_, which states that *"high level modules should not depend on low level modules; both should depend on abstractions."* Instead of referencing specific implementations, classes, request abstractions (typically ``interfaces``) which are provided to them when they are constructed. Extracting dependencies into interfaces and providing implementations of these interfaces as parameters is also an example of the `Strategy design pattern <http://deviq.com/strategy-design-pattern/>`_.

当类的设计使用 DI 思想，它们更加松散耦合，因为它们没有对它们的合作者直接硬编码的依赖。这遵循 `依赖倒置原则（Dependency Inversion Principle） <http://deviq.com/dependency-inversion-principle/>`_，其中指出 *"高层模块不应该依赖于低层模块；两者都应该依赖于抽象。"* 类要求在它们构造时向其提供抽象（通常是 ``interfaces`` ），而不是引用特定的实现。提取接口的依赖关系和提供这些接口的实现作为参数也是 `策略设计模式（Strategy design pattern） <http://deviq.com/strategy-design-pattern/>`_ 的一个示例。

When a system is designed to use DI, with many classes requesting their dependencies via their constructor (or properties), it's helpful to have a class dedicated to creating these classes with their associated dependencies. These classes are referred to as *containers*, or more specifically, `Inversion of Control (IoC) <http://deviq.com/inversion-of-control/>`_ containers or Dependency Injection (DI) containers. A container is essentially a factory that is responsible for providing instances of types that are requested from it. If a given type has declared that it has dependencies, and the container has been configured to provide the dependency types, it will create the dependencies as part of creating the requested instance. In this way, complex dependency graphs can be provided to classes without the need for any hard-coded object construction. In addition to creating objects with their dependencies, containers typically manage object lifetimes within the application.

当系统被设计为使用 DI ，很多类通过它们的构造函数（或属性）请求其依赖关系，有一个类被用来创建这些类及其相关的依赖关系是很有帮助的。这些类被称为 *容器（containers）* ，或者更具体地，`控制反转（Inversion of Control，IoC） <http://deviq.com/inversion-of-control/>`_ 容器或者依赖注入（Dependency injection，DI）容器。容器本质上是一个工厂，负责提供向它请求的类型实例。如果一个给定类型声明它具有依赖关系，并且容器已经被配置为提供依赖类型，它将把创建依赖关系作为创建请求实例的一部分。通过这种方式，可以向类型提供复杂的依赖关系而不需要任何硬编码的类型构造。除了创建对象的依赖关系，容器通常还会管理应用程序中对象的生命周期。

ASP.NET Core includes a simple built-in container (represented by the ``IServiceProvider`` interface) that supports constructor injection by default, and ASP.NET makes certain services available through DI. ASP.NET's container refers to the types it manages as *services*. Throughout the rest of this article, *services* will refer to types that are managed by ASP.NET Core's IoC container. You configure the built-in container's services in the ``ConfigureServices`` method in your application's ``Startup`` class.

ASP.NET Core 包含了一个默认支持构造函数注入的简单内置容器（由 ``IServiceProvider`` 接口表示），并且 ASP.NET 使某些服务可以通过 DI 获取。ASP.NET 的容器指的是它管理的类型为 *services* 。在这篇文章的其余部分， *services* 是指由ASP.NET Core的IoC容器管理的类型。你在应用程序 ``Startup`` 类的 ``ConfigureServices`` 方法中配置内置容器的服务。

.. note:: Martin Fowler has written an extensive article on `Inversion of Control Containers and the Dependency Injection Pattern <http://www.martinfowler.com/articles/injection.html>`_. Microsoft Patterns and Practices also has a great description of `Dependency Injection <https://msdn.microsoft.com/en-us/library/dn178469(v=pandp.30).aspx>`__.

.. note:: Martin Fowler 写过一篇全面的文章发表在 `Inversion of Control Containers and the Dependency Injection Pattern <http://www.martinfowler.com/articles/injection.html>`_. Microsoft 模式与实践小组（Microsoft Patterns and Practices）也有丰富的关于 `Dependency Injection <https://msdn.microsoft.com/en-us/library/dn178469(v=pandp.30).aspx>`__ 的描述.

.. note:: This article covers Dependency Injection as it applies to all ASP.NET applications. Dependency Injection within MVC controllers is covered in :doc:`/mvc/controllers/dependency-injection`.

.. note:: 本文介绍了依赖注入，因为它适用于所有的 ASP.NET 应用程序。 MVC 控制器中的依赖注入包含在 :doc:`/mvc/controllers/dependency-injection`.

Using Framework-Provided Services
---------------------------------
使用框架提供的服务
------------------

The ``ConfigureServices`` method in the ``Startup`` class is responsible for defining the services the application will use, including platform features like Entity Framework Core and ASP.NET Core MVC. Initially, the ``IServiceCollection`` provided to ``ConfigureServices`` has just a handful of services defined. Below is an example of how to add additional services to the container using a number of extensions methods like ``AddDbContext``, ``AddIdentity``, and ``AddMvc``.

``Startup`` 类的 ``ConfigureServices`` 方法负责定义应用程序将使用的服务，包括平台功能，比如 Entity Framework Core 和 ASP.NET Core MVC 。最初， ``IServiceCollection`` 只向 ``ConfigureServices`` 提供了几个服务定义。下面是如何使用一些扩展方法如 ``AddDbContext``，``AddIdentity`` 和 ``AddMvc`` 向容器中添加额外服务的一个例子。

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 39-56
  :dedent: 8
  :emphasize-lines: 5,8,12

The features and middleware provided by ASP.NET, such as MVC, follow a convention of using a single Add\ *Service*\  extension method to register all of the services required by that feature. 

ASP.NET 提供的功能和中间件，例如 MVC，遵循约定——使用单一的 Add\ *Service*\ 扩展方法来注册所有该功能所需的服务。

.. tip:: You can request certain framework-provided services within ``Startup`` methods through their parameter lists - see :doc:`startup` for more details.

.. tip:: 你可以在 ``Startup`` 的方法中通过它们的参数列表请求一些框架提供的服务 - 查看 :doc:`startup` 获取更多信息.

Of course, in addition to configuring the application to take advantage of various framework features, you can also use ``ConfigureServices`` to configure your own application services.

当然，除了使用各种框架功能配置应用程序，你也能够使用 ``ConfigureServices`` 配置你自己的应用程序服务。

Registering Your Own Services
-----------------------------
注册你自己的服务
----------------

You can register your own application services as follows. The first generic type represents the type (typically an interface) that will be requested from the container. The second generic type represents the concrete type that will be instantiated by the container and used to fulfill such requests.

你可以按照如下方式注册你自己的应用程序服务。第一个泛型类型表示将要求从容器中请求的类型（通常是接口）。第二个泛型类型表示将由容器实例化并且用于完成这些请求的具体类型。

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 53-54
  :dedent: 12

.. note:: Each ``services.Add<service>`` calls adds (and potentially configures) services. For example, ``services.AddMvc()`` adds the services MVC requires.

.. note:: 每个 ``services.Add<service>`` 调用添加（和可能配置）服务。 例如 ``services.AddMvc()`` 添加 MVC 需要的服务。

The ``AddTransient`` method is used to map abstract types to concrete services that are instantiated separately for every object that requires it. This is known as the service's *lifetime*, and additional lifetime options are described below. It is important to choose an appropriate lifetime for each of the services you register. Should a new instance of the service be provided to each class that requests it? Should one instance be used throughout a given web request? Or should a single instance be used for the lifetime of the application?

``AddTransient`` 方法用于将抽象类型映射到为每一个需要它的对象分别实例化的具体服务。这被称作为服务的  *生命周期（lifetime）*，另外的生命周期选项在下面描述。重要的是为你注册的每一个服务选择合适的生命周期。应该为每个请求的类提供一个新的服务实例？应该在一个给定的网络请求中使用一个实例？或者应该在应用程序周期中使用单例？

In the sample for this article, there is a simple controller that displays character names, called ``CharactersController``. Its ``Index`` method displays the current list of characters that have been stored in the application, and initializes the collection with a handful of characters if none exist. Note that although this application uses Entity Framework Core and the ``ApplicationDbContext`` class for its persistence, none of that is apparent in the controller. Instead, the specific data access mechanism has been abstracted behind an interface, ``ICharacterRepository``, which follows the `repository pattern <http://deviq.com/repository-pattern/>`_. An instance of ``ICharacterRepository`` is requested via the constructor and assigned to a private field, which is then used to access characters as necessary.

在这篇文章的示例中，有一个名称为 ``CharactersController`` 的简单控制器。它的 ``Index`` 方法显示已经存储在应用程序中的当前字符列表，并且初始化具有少量字符的集合如果它不存在的话。值得注意的是，虽然应用程序使用 Entity Framework Core 和 ``ApplicationDbContext`` 类作为持久化，这在控制器中都不是显而易见的。相反，具体的数据访问机制被抽象在遵循  `仓储模式（repository pattern） <http://deviq.com/repository-pattern/>`_ 的 ``ICharacterRepository`` 接口后面。 ``ICharacterRepository`` 的实例是通过构造函数请求并分配给一个私有字段，然后用来访问所需的字符。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Controllers/CharactersController.cs
  :language: c#
  :lines: 8-36
  :dedent: 4
  :emphasize-lines: 3,5-8,14,21,23-26

The `ICharacterRepository` simply defines the two methods the controller needs to work with `Character` instances.

`ICharacterRepository` 只定义了控制器需要使用 `Character` 实例的两个方法。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Interfaces/ICharacterRepository.cs
  :language: c#
  :emphasize-lines: 8-9

This interface is in turn implemented by a concrete type, ``CharacterRepository``, that is used at runtime.

这个接口在运行时使用一个具体的 ``CharacterRepository`` 类型来实现。

.. note:: The way DI is used with the ``CharacterRepository`` class is a general model you can follow for all of your application services, not just in "repositories" or data access classes.

.. note:: 在 ``CharacterRepository`` 类中使用 DI 的方式是一个你可以在你的应用程序服务遵循的通用模型，不只是在“仓储”或者数据访问类中。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Models/CharacterRepository.cs
  :language: c#
  :emphasize-lines: 9,11-14

Note that ``CharacterRepository`` requests an ``ApplicationDbContext`` in its constructor. It is not unusual for dependency injection to be used in a chained fashion like this, with each requested dependency in turn requesting its own dependencies. The container is responsible for resolving all of the dependencies in the graph and returning the fully resolved service.

注意的是 ``CharacterRepository`` 需要一个 ``ApplicationDbContext`` 在它的构造函数中。依赖注入用于这样的链式构造并不少见，每个请求依次请求它的依赖关系。容器负责解析所有的依赖关系，并返回完全解析后的服务。

.. note:: Creating the requested object, and all of the objects it requires, and all of the objects those require, is sometimes referred to as an `object graph`. Likewise, the collective set of dependencies that must be resolved is typically referred to as a `dependency tree` or `dependency graph`.

.. note:: 创建请求对象，和它需要的所有对象，以及那些对象需要的所有对象，有时称为一个 `对象图（object graph）`。同样的，必须解析依赖关系的集合通常称为 `依赖树（dependency tree）` 或者 `依赖图（dependency graph）`。


In this case, both ``ICharacterRepository`` and in turn ``ApplicationDbContext`` must be registered with the services container in ``ConfigureServices`` in ``Startup``. ``ApplicationDbContext`` is configured with the call to the extension method ``AddDbContext<T>``. The following code shows the registration of the ``CharacterRepository`` type.

在这种情况下， ``ICharacterRepository`` 和 ``ApplicationDbContext`` 都必须在 ``Startup`` 类 ``ConfigureServices`` 方法的服务容器中注册。 ``ApplicationDbContext`` 的配置调用 ``AddDbContext<T>`` 扩展方法。下面的代码展示 ``CharacterRepository`` 类型的注册。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Startup.cs
  :language: c#
  :lines: 17-33
  :emphasize-lines: 3-5,11
  :dedent: 8

Entity Framework contexts should be added to the services container using the ``Scoped`` lifetime. This is taken care of automatically if you use the helper methods as shown above. Repositories that will make use of Entity Framework should use the same lifetime.

Entity Framework 上下文应当使用 ``Scoped`` 生命周期添加到服务容器中。如果你使用上图所示的帮助方法则这是自动处理的。仓储将使 Entity Framework 使用相同的生命周期。

.. warning:: The main danger to be wary of is resolving a ``Scoped`` service from a singleton. It's likely in such a case that the service will have incorrect state when processing subsequent requests.

.. warning:: 最主要的危险是要小心从单例解析一个 ``Scoped`` 服务。在这种情况下很可能处理后续请求的时候服务会出现不正确的状态。

Service Lifetimes and Registration Options
------------------------------------------
服务生命周期和注册选项
----------------------

ASP.NET services can be configured with the following lifetimes:

ASP.NET 服务可以被配置为以下生命周期：

Transient 瞬时
  Transient lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services.
  
  瞬时（Transient）生命周期服务在它们每次请求时被创建。这一生命周期适合轻量级的，无状态的服务。
  
Scoped 作用域
  Scoped lifetime services are created once per request.
  
  作用域（Scoped）生命周期服务在每次请求被创建一次。
  
Singleton 单例
  Singleton lifetime services are created the first time they are requested (or when ``ConfigureServices`` is run if you specify an instance there) and then every subsequent request will use the same instance. If your application requires singleton behavior, allowing the services container to manage the service's lifetime is recommended instead of implementing the singleton design pattern and managing your object's lifetime in the class yourself.

  单例（Singleton）生命周期服务在它们第一次被请求时创建（或者如果你在 ``ConfigureServices`` 运行时指定一个实例）并且每个后续请求将使用相同的实例。如果你的应用程序需要单例行为，建议让服务容器管理服务的生命周期而不是在自己的类中实现单例模式和管理对象的生命周期。
  
Services can be registered with the container in several ways. We have already seen how to register a service implementation with a given type by specifying the concrete type to use. In addition, a factory can be specified, which will then be used to create the instance on demand. The third approach is to directly specify the instance of the type to use, in which case the container will never attempt to create an instance.

服务可以用多种方式在容器中注册。我们已经看到了如何通过指定具体类型用来注册一个给定类型的服务实现。除此之外，可以指定一个工厂，它将被用来创建需要的实例。第三种方式是直接指定要使用的类型的实例，在这种情况下容器将永远不会尝试创建一个实例。

To demonstrate the difference between these lifetime and registration options, consider a simple interface that represents one or more tasks as an *operation* with a unique identifier, ``OperationId``. Depending on how we configure the lifetime for this service, the container will provide either the same or different instances of the service to the requesting class. To make it clear which lifetime is being requested, we will create one type per lifetime option:

为了说明这些生命周期和注册选项之间的差异，考虑一个简单的接口将一个或多个任务表示为有一个唯一标识符 ``OperationId`` 的 *操作* 。依据我们如何配置这个服务的生命周期，容器将为请求的类提供相同或不同的服务实例。要弄清楚哪一个生命周期被请求，我们将创建每一个生命周期选项的类型：

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Interfaces/IOperation.cs
  :language: c#
  :emphasize-lines: 5,7

We implement these interfaces using a single class, ``Operation``, that accepts a ``Guid`` in its constructor, or uses a new ``Guid`` if none is provided.

我们使用 ``Operation`` 类实现这些接口。它的构造函数接收一个 ``Guid``，若未提供则生成一个新的 ``Guid``。

Next, in ``ConfigureServices``, each type is added to the container according to its named lifetime:

接下来，在 ``ConfigureServices`` 中，每一个类型根据它们命名的生命周期被添加到容器中：

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Startup.cs
  :language: c#
  :lines: 28-32
  :dedent: 12

Note that the ``IOperationSingletonInstance`` service is using a specific instance with a known ID of ``Guid.Empty`` so it will be clear when this type is in use. We have also registered an ``OperationService`` that depends on each of the other ``Operation`` types, so that it will be clear within a request whether this service is getting the same instance as the controller, or a new one, for each operation type. All this service does is expose its dependencies as properties, so they can be displayed in the view.

请注意， ``IOperationSingletonInstance`` 服务使用一个具有已知 ``Guid.Empty`` ID 的具体实例，所以该类型在使用时是明确的。我们还注册了一个依赖于其他每个 ``Operation`` 类型的 ``OperationService``，因此在一个请求中对于每个操作类型，该服务获取相同的实例或创建一个新的实例作为控制器将是明确的。所有服务通过属性暴露依赖关系，因此它们可以显示在视图中。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Services/OperationService.cs
  :language: c#

To demonstrate the object lifetimes within and between separate individual requests to the application, the sample includes an ``OperationsController`` that requests each kind of ``IOperation`` type as well as an ``OperationService``. The ``Index`` action then displays all of the controller's and service's ``OperationId`` values.

为了证明对象的生命周期在应用程序的每个单独的请求内，还是请求之间，此示例包含 ``OperationsController`` 请求每一个 ``IOperation`` 类型和 ``OperationService``。 ``Index`` action 接下来显示所有控制器和服务的 ``OperationId`` 值。

.. literalinclude:: dependency-injection/sample/DependencyInjectionSample/Controllers/OperationsController.cs
  :language: c#

Now two separate requests are made to this controller action:

现在两个独立的请求到这个 controller action：

.. image:: dependency-injection/_static/lifetimes_request1.png
.. image:: dependency-injection/_static/lifetimes_request2.png

Observe which of the ``OperationId`` values varies within a request, and between requests.

观察 ``OperationId`` 值在请求和请求之间的变化。

- *Transient* objects are always different; a new instance is provided to every controller and every service.
- *Scoped* objects are the same within a request, but different across different requests
- *Singleton* objects are the same for every object and every request (regardless of whether an instance is provided in ``ConfigureServices``)

- *瞬时（Transient）* 对象总是不同的；向每一个控制器和每一个服务提供了一个新的实例
- *作用域（Scoped）* 对象在一次请求中是相同的，但在不同请求中是不同的
- *单例（Singleton）* 对象对每个对象和每个请求是相同的（无论是否在 ``ConfigureServices`` 中提供实例）


Request Services
----------------
请求服务
--------

The services available within an ASP.NET request from ``HttpContext`` are exposed through the ``RequestServices`` collection.

来自 ``HttpContext`` 的一次 ASP.NET 请求中可用的服务通过 ``RequestServices`` 集合公开的。

.. image:: dependency-injection/_static/request-services.png

Request Services represent the services you configure and request as part of your application. When your objects specify dependencies, these are satisfied by the types found in ``RequestServices``, not ``ApplicationServices``.

请求服务将你配置的服务和请求描述为应用程序的一部分。当你的对象指定依赖关系，这些满足要求的对象通过查找 ``RequestServices`` 中对应的类型得到，而不是 ``ApplicationServices``。

Generally, you shouldn't use these properties directly, preferring instead to request the types your classes you require via your class's constructor, and letting the framework inject these dependencies. This yields classes that are easier to test (see :doc:`/testing/index`) and are more loosely coupled.

通常，你不应该直接使用这些属性，而更倾向于通过类的构造函数请求需要的类的类型，并且让框架来注入依赖关系。这将会生成更易于测试的 （查看 :doc:`/testing/index`） 和更松散耦合的类。

.. note:: Prefer requesting dependencies as constructor parameters to accessing the ``RequestServices`` collection. 

.. note:: 更倾向于请求依赖关系作为构造函数的参数来访问 ``RequestServices`` 集合。

Designing Your Services For Dependency Injection
------------------------------------------------
设计你的依赖注入服务
------------------------

You should design your services to use dependency injection to get their collaborators. This means avoiding the use of stateful static method calls (which result in a code smell known as `static cling <http://deviq.com/static-cling/>`_) and the direct instantiation of dependent classes within your services. It may help to remember the phrase, `New is Glue <http://ardalis.com/new-is-glue>`_, when choosing whether to instantiate a type or to request it via dependency injection. By following the `SOLID Principles of Object Oriented Design <http://deviq.com/solid/>`_, your classes will naturally tend to be small,  well-factored, and easily tested.

你应该设计你的依赖注入服务来获取它们的合作者。这意味着在你的服务中避免使用有状态的静态方法调用（代码被称为 `static cling <http://deviq.com/static-cling/>`_）和直接实例化依赖的类型。当选择实例化一个类型还是通过依赖注入请求它时，它可以帮助记住这句话， `New is Glue <http://ardalis.com/new-is-glue>`_。通过遵循 `SOLID Principles of Object Oriented Design <http://deviq.com/solid/>`_，你的类将倾向于比较小，已分解的及易于测试的。

What if you find that your classes tend to have way too many dependencies being injected? This is generally a sign that your class is trying to do too much, and is probably violating SRP - the `Single Responsibility Principle <http://deviq.com/single-responsibility-principle/>`_. See if you can refactor the class by moving some of its responsibilities into a new class. Keep in mind that your ``Controller`` classes should be focused on UI concerns, so business rules and data access implementation details should be kept in classes appropriate to these `separate concerns <http://deviq.com/separation-of-concerns/>`_.

如果你发现你的类往往会有太多的依赖关系被注入时该怎么办？这通常表明你的类试图做太多，并且可能违反了单一职责原则（SRP） - `Single Responsibility Principle <http://deviq.com/single-responsibility-principle/>`_。看看你是否可以通过移动一些职责到一个新的类来重构类。请记住，你的 ``Controller`` 类应该重点关注用户界面（User Interface，UI），因此业务规则和数据访问实现细节应该保存在这些适合单独关注的类中。

With regard to data access specifically, you can easily inject Entity Framework ``DbContext`` types into your controllers, assuming you've configured EF in your ``Startup`` class. However, it is best to avoid depending directly on ``DbContext`` in your UI project. Instead, depend on an abstraction (like a Repository interface), and restrict knowledge of EF (or any other specific data access technology) to the implementation of this interface. This will reduce the coupling between your application and a particular data access strategy, and will make testing your application code much easier.

关于数据访问，如果你已经在 ``Startup`` 类中配置了 EF，那么能够在你的控制器中方便的注入 Entity Framework 的 ``DbContext`` 类型。然而，最好不要在你的 UI 项目直接依赖  ``DbContext``。相反，依赖于一个抽象（比如一个仓储接口），并且限定使用 EF （或其他任何数据访问技术）来实现这个接口。这将减少应用程序和特定的数据访问策略之间的耦合，并且使你的应用程序代码更容易测试。

.. _replacing-the-default-services-container:

Replacing the default services container
----------------------------------------
替换默认的服务容器
------------------

The built-in services container is mean to serve the basic needs of the framework and most consumer applications built on it. However, developers who wish to replace the built-in container with their preferred container can easily do so. The ``ConfigureServices`` method typically returns ``void``, but if its signature is changed to return ``IServiceProvider``, a different container can be configured and returned. There are many IOC containers available for .NET. In this example, the `Autofac <http://autofac.org/>`_ package is used.

内置的服务容器的意图在于提供框架的基本需求并且大多数客户应用程序建立在它之上。然而，开发人员可以很容易地使用他们的首选容器替换默认容器。``ConfigureServices`` 方法通常返回 ``void``，但是如果改变它的签名返回 ``IServiceProvider``，可以配置并返回一个不同的容器。有很多 IOC 容器可用于 .NET。在这个例子中， `Autofac <http://autofac.org/>`_ 包被使用。

First, add the appropriate container package(s) to the dependencies property in project.json:

首先，在 project.json 的 dependencies 属性中添加适当的容器包：

.. code-block:: javascript

  "dependencies" : {
    "Autofac": "4.0.0-rc2-237",
    "Autofac.Extensions.DependencyInjection": "4.0.0-rc2-200"
  },

Next, configure the container in ``ConfigureServices`` and return an ``IServiceProvider``:

接着，在 ``ConfigureServices`` 中配置容器并返回 ``IServiceProvider``：

.. code-block:: c#
  :emphasize-lines: 1,11

  public IServiceProvider ConfigureServices(IServiceCollection services)
  {
    services.AddMvc();
    // add other framework services
    
    // Add Autofac
    var containerBuilder = new ContainerBuilder();
    containerBuilder.RegisterModule<DefaultModule>();
    containerBuilder.Populate(services);
    var container = containerBuilder.Build();
    return container.Resolve<IServiceProvider>();
  }

.. note:: When using a third-party DI container, you must change ``ConfigureServices`` so that it returns ``IServiceProvider`` instead of ``void``.

.. note:: 当使用第三方 DI 容器时，你必须更改 ``ConfigureServices`` 让它返回 ``IServiceProvider`` 而不是 ``void``。

Finally, configure Autofac as normal in ``DefaultModule``:

最后，在 ``DefaultModule`` 中配置 Autofac：

.. code-block:: c#

  public class DefaultModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<CharacterRepository>().As<ICharacterRepository>();
    }
  }

At runtime, Autofac will be used to resolve types and inject dependencies. `Learn more about using Autofac and ASP.NET Core <http://docs.autofac.org/en/latest/integration/aspnetcore.html>`_

在运行时，Autofac 将被用来解析类型和注入依赖关系。 `了解更多有关使用 Autofac 和 ASP.NET Core <http://docs.autofac.org/en/latest/integration/aspnetcore.html>`_

Recommendations
---------------
建议
-----


When working with dependency injection, keep the following recommendations in mind:

当使用依赖注入时，请记住以下建议：

- DI is for objects that have complex dependencies. Controllers, services, adapters, and repositories are all examples of objects that might be added to DI.
- Avoid storing data and configuration directly in DI. For example, a user's shopping cart shouldn't typically be added to the services container. Configuration should use the :ref:`Options Model <options-config-objects>`. Similarly, avoid "data holder" objects that only exist to allow access to some other object. It's better to request the actual item needed via DI, if possible.
- Avoid static access to services.
- Avoid service location in your application code.
- Avoid static access to ``HttpContext``.

- DI 针对具有复杂依赖关系的对象。控制器，服务，适配器和仓储都是可能被添加到 DI 的对象的例子。
- 避免直接在 DI 中存储数据和配置。例如，用户的购物车通常不应该被添加到服务容器中。配置应该使用 :ref:`Options Model <options-config-objects>`。 同样, 避免 “数据持有者” 对象只是为了允许访问其他对象而存在。如果可能的话，最好是通过 DI 获取实际的项。
- 避免静态访问服务。
- 避免在应用程序代码中服务定位。
- 避免静态访问 ``HttpContext``。

.. note:: Like all sets of recommendations, you may encounter situations where ignoring one is required. We have found exceptions to be rare -- mostly very special cases within the framework itself.

.. note:: 像所有的建议，你可能遇到必须忽视其中一个的情况。我们发现了少见的例外 -- 非常特别的情况是框架本身。

Remember, dependency injection is an *alternative* to static/global object access patterns. You will not be able to realize the benefits of DI if you mix it with static object access.

记住，依赖注入是静态/全局对象访问模式的 *另一选择*。如果你把它和静态对象访问混合的话，你将无法了解 DI 的有用之处。

Additional Resources
--------------------
附加资源
---------

- :doc:`startup`
- :doc:`/testing/index`
- `Writing Clean Code in ASP.NET Core with Dependency Injection (MSDN) <https://msdn.microsoft.com/en-us/magazine/mt703433.aspx>`_
- `Container-Managed Application Design, Prelude: Where does the Container Belong? <http://blogs.msdn.com/b/nblumhardt/archive/2008/12/27/container-managed-application-design-prelude-where-does-the-container-belong.aspx>`__
- `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle/>`_
- `Inversion of Control Containers and the Dependency Injection Pattern <http://www.martinfowler.com/articles/injection.html>`_ (Fowler)

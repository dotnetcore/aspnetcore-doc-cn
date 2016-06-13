过滤器
=======

作者： `Steve Smith`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对：

*Filters* in ASP.NET MVC allow you to run code before or after a particular stage in the execution pipeline. Filters can be configured globally, per-controller, or per-action.

ASP.NET MVC *过滤器* 可使执行管道的前后特定阶段执行代码。过滤器可以配置全局有效、仅对控制器有效或是仅对 Action 有效。

.. contents:: Sections
    :local:
    :depth: 1

`查看或下载演示代码 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/filters/sample>`_.

过滤器如何工作？
--------------------

Each filter type is executed at a different stage in the pipeline, and thus has its own set of intended scenarios. Choose what type of filter to create based on the task you need it to perform, and where in the request pipeline it executes. Filters run within the MVC Action Invocation Pipeline, sometimes referred to as the *Filter Pipeline*, which runs after MVC selects the action to execute.

不同的过滤器类型会在执行管道的不同阶段运行，因此它们各自有一套适用场景。根据你实际要解决的问题以及在请求管道中执行的位置来选择创建不同的过滤器。运行于 MVC Action 调用管道内的过滤器有时被称作为*过滤管道*，当 MVC 选择要执行哪个 Action 后就会先执行该 Action 上的过滤器。

.. image:: filters/_static/filter-pipeline-1.png

Different filter types run at different points within the pipeline. Some filters, like authorization filters, only run before the next stage in the pipeline, and take no action afterward. Other filters, like action filters, can execute both before and after other parts of the pipeline execute, as shown below.

不同过滤器在管道的不同位置运行。像授权这样的过滤器只运行在管道的靠前位置，并且其后也不会跟随 action。其它过滤器（如 action 过滤器等）可以在管道的其它部分之前或之后执行，如下所示。

.. image:: filters/_static/filter-pipeline-2.png

选择过滤器
^^^^^^^^^^^^^^^^^^

:ref:`Authorization  filters <authorization-filters>` are used to determine whether the current user is authorized for the request being made.

:ref:`授权过滤器 <authorization-filters>` 用于确定当前用户的请求是否合法。

:ref:`Resource filters <resource-filters>` are the first filter to handle a request after authorization, and the last one to touch the request as it is leaving the filter pipeline. They're especially useful to implement caching or otherwise short-circuit the filter pipeline for performance reasons.

:ref:`资源过滤器 <resource-filters>` 是授权之后第一个用来处理请求的过滤器，也是最后一个接触到请求的过滤器（因为之后就会离开过滤器管道）。在性能方面，资源过滤器在实现缓存或短路过滤器管道尤其有用。

:ref:`Action filters <action-filters>` wrap calls to individual action method calls, and can manipulate the arguments passed into an action as well as the action result returned from it.

:ref:`Action 过滤器 <action-filters>` 包装了对单个 action 方法的调用，可以将参数传递给 action 并从中获得 action result。

:ref:`Exception filters <exception-filters>` are used to apply global policies to unhandled exceptions in the MVC app.

:ref:`异常过滤器 <exception-filters>` 为 MVC 应用程序未处理异常应用全局策略。

:ref:`Result filters <result-filters>` wrap the execution of individual action results, and only run when the action method has executed successfully. They are ideal for logic that must surround view execution or formatter execution.

:ref:`结果过滤器 <result-filters>` 包装了单个 action result 的执行，当且仅当 action 方法成功执行完毕后方才运行。它们是理想的围绕视图执行或格式处理的逻辑（所在之处）。

实现
^^^^^^^^^^^^^^

All filters support both synchronous and asynchronous implementations through different interface definitions. Choose the sync or async variant depending on the kind of task you need to perform. They are interchangeable from the framework's perspective.

所有过滤器均可通过不同的接口定义来支持同步和异步实现。根据你所需执行的任务的不同来选择同步还是异步实现。从框架的角度来讲它们是可以互换的。

Synchronous filters define both an On\ *Stage*\ Executing and On\ *Stage*\ Executed method (with noted exceptions). The On\ *Stage*\ Executing method will be called before the event pipeline stage by the Stage name, and the On\ *Stage*\ Executed method will be called after the pipeline stage named by the Stage name.

同步过滤器定义了 On\ *Stage*\ Executing 方法和 On\ *Stage*\ Executed 方法（当然也有例外）。On\ *Stage*\ Executing 方法在具体事件管道阶段之前调用，而 On\ *Stage*\ Executed 方法则在之后调用（比如当 State 是 Action 时，这两个方法便是 OnActionExecuting 和 OnActionExecuted，译者注）。

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleActionFilter.cs
  :language: c#
  :emphasize-lines: 6,8,13

Asynchronous filters define a single On\ *Stage*\ ExecutionAsync method that will surround execution of the pipeline stage named by Stage. The On\ *Stage*\ ExecutionAsync method is provided a *Stage*\ ExecutionDelegate delegate which will execute the pipeline stage named by Stage when invoked and awaited.

异步过滤器定义了一个 On\ *Stage*\ ExecutionAsync 方法，可以在具体管道阶段的前后运行。On\ *Stage*\ ExecutionAsync 方法被提供了一个 *Stage*\ ExecutionDelegate 委托，当调用时该委托会执行具体管道阶段的工作，然后等待其完成。

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleAsyncActionFilter.cs
  :language: c#
  :emphasize-lines: 6,8-9

.. note:: You should only implement *either* the synchronous or the async version of a filter interface, not both. If you need to perform async work in the filter, implement the async interface. Otherwise, implement the synchronous interface. The framework will check to see if the filter implements the async interface first, and if so, it will call it. If not, it will call the synchronous interface's method(s). If you were to implement both interfaces on one class, only the async method would be called by the framework. Also, it doesn't matter whether your action is async or not, your filters can be synchronous or async independent of the action.

.. note:: *只能*实现一个过滤器接口，要么是同步版本的，要么是异步版本的，*鱼和熊掌不可兼得*。如果你需要在接口中执行异步工作，那么就去实现异步接口。不然的话，就应该实现同步版本的接口。框架会首先检查是不是实现了异步接口，如果实现了异步接口，那么将调用它。不然则调用同步接口的方法。如果一个类中实现了两个接口，那么只有异步方法会被调用。最后，不管 action 是同步的还是异步的，过滤器的同步或是异步是独立于 action 的。

过滤器作用域
^^^^^^^^^^^^^

Filters can be *scoped* at three different levels. You can add a particular filter to a particular action as an attribute. You can add a filter to all actions within a controller by applying an attribute at the controller level. Or you can register a filter globally, to be run with every MVC action.

过滤器具有三种不同级别的\ *作用域*\ 。你可以在特定的 action 上用特性（Attribute）的方式使用特定的过滤器；也可以在控制器上用特性的方式使用过滤器，这样就会将效果应用在控制器内所有的 action 上；或者注册一个全局过滤器，它将作用于整个 MVC 应用程序下的每一个 action。

Global filters are added in the ``ConfigureServices`` method in ``Startup``, when configuring MVC:

如果想要使用全局过滤器的话，在你配置 MVC 的时候在 ``Startup`` 的 ``ConfigureServices`` 方法中添加：

.. literalinclude:: filters/sample/src/FiltersSample/Startup.cs
  :language: c#
  :emphasize-lines: 5-6
  :lines: 13-22
  :dedent: 8

Filters can be added by type, or an instance can be added. If you add an instance, that instance will be used for every request. If you add a type, it will be type-activated, meaning an instance will be created for each request and any constructor dependencies will be populated by DI. Adding a filter by type is equivalent to ``filters.Add(new TypeFilterAttribute(typeof(MyFilter)))``.

It's often convenient to implement filter interfaces as *Attributes*. Filter attributes are applied to controllers and action methods. The framework includes built-in attribute-based filters that you can subclass and customize. For example, the following filter inherits from `ResultFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ResultFilterAttribute/index.html>`_, and overrides its ``OnResultExecuting`` method to add a header to the response.

.. _add-header-attribute:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/AddHeaderAttribute.cs
  :language: c#
  :emphasize-lines: 5,16

Attributes allow filters to accept arguments, as shown in the example above. You would add this attribute to a controller or action method and specify the name and value of the HTTP header you wished to add to the response:

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/SampleController.cs
  :language: c#
  :emphasize-lines: 1
  :lines: 6-12,25
  :dedent: 4

The result of the ``Index`` action is shown below - the response headers are displayed on the bottom right.

.. image:: filters/_static/add-header.png

Several of the filter interfaces have corresponding attributes that can be used as base classes for custom implementations.

Filter attributes:

- `ActionFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ActionFilterAttribute/index.html>`_
- `AuthorizationFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/AuthorizationFilterAttribute/index.html>`_
- `ExceptionFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ExceptionFilterAttribute/index.html>`_
- `ResultFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ResultFilterAttribute/index.html>`_

取消与短路
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can short-circuit the filter pipeline at any point by setting the ``Result`` property on the context parameter provided to the filter method. For instance, the following ``ShortCircuitingResourceFilter`` will prevent any other filters from running later in the pipeline, including any action filters.

通过设置传入过滤器方法的上下文参数中的  ``Result`` 属性，可以在过滤器管道的任意一点短路管道。比方说，下面的 ``ShortCircuitingResourceFilter`` 将阻止所有它之后管道内的所有过滤器，包括所有 action 过滤器。

.. _short-circuiting-resource-filter:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/ShortCircuitingResourceFilterAttribute.cs
  :language: c#
  :emphasize-lines: 12-15

In the following code, both the ``ShortCircuitingResourceFilter`` and the ``AddHeader`` filter target the ``SomeResource`` action method. However, because the ``ShortCircuitingResourceFilter`` runs first and short-circuits the rest of the pipeline, the ``AddHeader`` filter never runs for the ``SomeResource`` action. This behavior would be the same if both filters were applied at the action method level, provided the ``ShortCircuitingResourceFilter`` ran first (see :ref:`Ordering <ordering>`).

在下例中，``ShortCircuitingResourceFilter`` 和 ``AddHeader`` 过滤器都指向了名为 ``SomeResource`` 的 action 方法。然而，由于首先运行的是 ``ShortCircuitingResourceFilter``\ ，然后它短路了的剩下的管道，``SomeResource`` 上的 ``AddHeader`` 过滤器不会运行。如果这两个过滤器都以 Action 方法级别出现，它们的结果也会是一样的（只要 ``ShortCircuitingResourceFilter`` 首先运行，请查看 :ref:`Ordering <ordering>`\ ）。

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/SampleController.cs
  :language: c#
  :emphasize-lines: 1,4
  :lines: 6-8, 14-19
  :dedent: 4 

配置过滤器
-------------------

Global filters are configured within ``Startup.cs``. Attribute-based filters that do not require any dependencies can simply inherit from an existing attribute of the appropriate type for the filter in question. To create a filter *without* global scope that requires dependencies from DI, apply the ``ServiceFilterAttribute`` or ``TypeFilterAttribute`` attribute to the controller or action.

全局过滤器在 ``Startup.cs`` 中配置。基于特性的过滤器如果不需要任何依赖项的话，可以简单地继承一个与已存在的过滤器相对应的特性类型。如果要创建一个*非*全局作用域、但需要从依赖注入（DI）中获得依赖项的过滤器，在它们上面加上 ``ServiceFilterAttribute`` 或 ``TypeFilterAttribute`` 特性，这样就可用于控制器或 action 了。

依赖注入
^^^^^^^^^^^^^^^^^^^^

Filters that are implemented as attributes and added directly to controller classes or action methods cannot have constructor dependencies provided by :doc:`dependency injection </fundamentals/dependency-injection>` (DI). This is because attributes must have their constructor parameters supplied where they are applied. This is a limitation of how attributes work.

However, if your filters have dependencies you need to access from DI, there are several supported approaches. You can apply your filter to a class or action method using

- The `ServiceFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/ServiceFilterAttribute/index.html>`_ 
- The `TypeFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TypeFilterAttribute/index.html>`_ attribute
- `IFilterFactory <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/IFilterFactory/index.html>`__ implemented on your attribute

A ``TypeFilter`` will instantiate an instance, using services from DI for its dependencies. A ``ServiceFilter`` retrieves an instance of the filter from DI. The following example demonstrates using a ``ServiceFilter``:

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/HomeController.cs
  :language: c#
  :emphasize-lines: 1
  :lines: 8-12
  :dedent: 8

Using ``ServiceFilter`` without registering the filter type in ``ConfigureServices``, throws the following exception:

.. code-block:: c#

	System.InvalidOperationException: No service for type 
	'FiltersSample.Filters.AddHeaderFilterWithDI' has been registered.

To avoid this exception, you must register the ``AddHeaderFilterWithDI`` type in ``ConfigureServices``:

.. literalinclude:: filters/sample/src/FiltersSample/Startup.cs
  :language: c#
  :emphasize-lines: 1
  :lines: 20
  :dedent: 12

``ServiceFilterAttribute`` implements ``IFilterFactory``, which exposes a single method for creating an ``IFilter`` instance. In the case of ``ServiceFilterAttribute``, the ``IFilterFactory`` interface's ``CreateInstance`` method is implemented to load the specified type from the services container (DI).

``TypeFilterAttribute`` is very similar to ``ServiceFilterAttribute`` (and also implements ``IFilterFactory``), but its type is not resolved directly from the DI container. Instead, it instantiates the type using an `ObjectFactory delegate <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/DependencyInjection/ObjectFactory/index.html>`_. Because of this difference, types that are referenced using the ``TypeFilterAttribute`` do not need to be registered with the container first (but they will still have their dependencies fulfilled by the container). Also, ``TypeFilterAttribute`` can optionally accept constructor arguments for the type in question. The following example demonstrates how to pass arguments to a type using ``TypeFilterAttribute``:

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/HomeController.cs
  :language: c#
  :emphasize-lines: 1-2
  :lines: 20-25
  :dedent: 8

If you have a simple filter that doesn't require any arguments, but which has constructor dependencies that need to be filled by DI, you can inherit from ``TypeFilterAttribute``, allowing you to use your own named attribute on classes and methods (instead of ``[TypeFilter(typeof(FilterType))]``). The following filter shows how this can be implemented:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/SampleActionFilterAttribute.cs
  :language: c#
  :emphasize-lines: 1, 3, 7
  :lines: 7-38
  :dedent: 4

This filter can be applied to classes or methods using the ``[SampleActionFilter]`` syntax, instead of having to use ``[TypeFilter]`` or ``[ServiceFilter]``.

.. note:: Avoid creating and using filters purely for logging purposes, since the :doc:`built-in framework logging features </fundamentals/logging>` should already provide what you need for logging. If you're going to add logging to your filters, it should focus on business domain concerns or behavior specific to your filter, rather than MVC actions or other framework events.  

``IFilterFactory`` implements ``IFilter``. Therefore, an ``IFilterFactory`` instance can be used as an ``IFilter`` instance anywhere in the filter pipeline. When the framework prepares to invoke the filter, attempts to cast it to an ``IFilterFactory``. If that cast succeeds, the ``CreateInstance`` method is called to create the ``IFilter`` instance that will be invoked. This provides a very flexible design, since the precise filter pipeline does not need to be set explicitly when the application starts.

You can implement ``IFilterFactory`` on your own attribute implementations as another approach to creating filters:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/AddHeaderWithFactoryAttribute.cs
  :language: c#
  :emphasize-lines: 1,4-7
  :lines: 6-26
  :dedent: 4

.. _ordering:

排序
^^^^^^^^

Filters can be applied to action methods or controllers (via attribute) or added to the global filters collection. Scope also generally determines ordering. The filter closest to the action runs first; generally you get overriding behavior without having to explicitly set ordering. This is sometimes referred to as "Russian doll" nesting, as each increase in scope is wrapped around the previous scope, like a `nesting doll <https://en.wikipedia.org/wiki/Matryoshka_doll>`_.

In addition to scope, filters can override their sequence of execution by implementing `IOrderedFilter <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/IOrderedFilter/index.html>`_. This interface simply exposes an ``int`` ``Order`` property, and filters execute in ascending numeric order based on this property. All of the built-in filters, including ``TypeFilterAttribute`` and ``ServiceFilterAttribute``, implement ``IOrderedFilter``, so you can specify the order of filters when you apply the attribute to a class or method. By default, the ``Order`` property is 0 for all of the built-in filters, so scope is used as a tie-breaker and (unless ``Order`` is set to a non-zero value) is the determining factor.

Every controller that inherits from the ``Controller`` base class includes ``OnActionExecuting`` and ``OnActionExecuted`` methods. These methods wrap the filters that run for a given action, running first and last. The scope-based order, assuming no ``Order`` has been set for any filter, is:

#. The Controller ``OnActionExecuting``
#. The Global filter ``OnActionExecuting``
#. The Class filter ``OnActionExecuting``
#. The Method filter ``OnActionExecuting``
#. The Method filter ``OnActionExecuted``
#. The Class filter ``OnActionExecuted``
#. The Global filter ``OnActionExecuted``
#. The Controller ``OnActionExecuted``

.. note:: ``IOrderedFilter`` trumps scope when determining the order in which filters will run. Filters are sorted first by order, then scope is used to break ties. Order defaults to 0 if not set.

To modify the default, scope-based order, you could explicitly set the ``Order`` property of a class-level or method-level filter. For example, adding ``Order=-1`` to a method level attribute:

.. code-block:: c#

	[MyFilter(Name = "Method Level Attribute", Order=-1)]

In this case, a value of less than zero would ensure this filter ran before both the Global and Class level filters (assuming their ``Order`` property was not set).

The new order would be:

#. The Controller ``OnActionExecuting``
#. The Method filter ``OnActionExecuting``
#. The Global filter ``OnActionExecuting``
#. The Class filter ``OnActionExecuting``
#. The Class filter ``OnActionExecuted``
#. The Global filter ``OnActionExecuted``
#. The Method filter ``OnActionExecuted``
#. The Controller ``OnActionExecuted``

.. note:: The ``Controller`` class's methods always run before and after all filters. These methods are not implemented as ``IFilter`` instances and do not participate in the ``IFilter`` ordering algorithm.

.. _authorization-filters:

授权过滤
---------------------

*Authorization Filters* control access to action methods, and are the first filters to be executed within the filter pipeline. They have only a before stage, unlike most filters that support before and after methods. You should only write a custom authorization filter if you are writing your own authorization framework. Note that you should not throw exceptions within authorization filters, since nothing will handle the exception (exception filters won't handle them). Instead, issue a challenge or find another way.

*授权过滤器*\ 控制对 action 方法的访问，也是过滤器管道中第一个被执行的过滤器。它们只有一个前置阶段，不像其它大多数过滤器支持前置阶段方法和后置阶段方法。只有当你使用自己的授权框架时才需要定制授权过滤器。谨记勿在授权过滤器内抛出异常，这是因为所抛出的异常不会被处理（异常过滤器也不会处理它们）。此时记录该问题或寻求其它办法。

Learn more about :doc:`/security/authorization/index`.

更多请访问 :doc:`/security/authorization/index`\ 。

.. _resource-filters:

资源过滤
----------------

*Resource Filters* implement either the ``IResourceFilter`` or ``IAsyncResourceFilter`` interface, and their execution wraps most of the filter pipeline (only :ref:`authorization-filters` run before them - all other filters and action processing happens between their ``OnResourceExecuting`` and ``OnResourceExecuted`` methods). Resource filters are especially useful if you need to short-circuit most of the work a request is doing. Caching would be one example use case for a resource filter, since if the response is already in the cache, the filter can immediately set a result and avoid the rest of the processing for the action.

The :ref:`short circuiting resource filter <short-circuiting-resource-filter>` shown above is one example of a resource filter. A very naive cache implementation (do not use this in production) that only works with ``ContentResult`` action results is shown below:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/NaiveCacheResourceFilterAttribute.cs
  :language: c#
  :emphasize-lines: 1-2,11,16-17,27,30
  :lines: 8-41
  :dedent: 4

In ``OnResourceExecuting``, if the result is already in the static dictionary cache, the ``Result`` property is set on ``context``, and the action short-circuits and returns with the cached result. In the ``OnResourceExecuted`` method, if the current request's key isn't already in use, the current ``Result`` is stored in the cache, to be used by future requests.

Adding this filter to a class or method is shown here:

.. literalinclude:: filters/sample/src/FiltersSample/Controllers/CachedController.cs
  :language: c#
  :emphasize-lines: 1-2,6
  :lines: 7-14
  :dedent: 4
  
.. _action-filters:

Action 过滤器
--------------

*Action Filters* implement either the ``IActionFilter`` or ``IAsyncActionFilter`` interface and their execution surrounds the execution of action methods. Action filters are ideal for any logic that needs to see the results of model binding, or modify the controller or inputs to an action method. Additionally, action filters can view and directly modify the result of an action method.

*Action 过滤器*\ 要么实现 ``IActionFilter`` 接口，要么实现 ``IAsyncActionFilter`` 接口，它们可以在 action 方法执行的前后被执行。Action 过滤器非常适合放置诸如查看模型绑定结果、或是修改控制器或输入到 action 方法的逻辑。另外，action 过滤器可以查看并直接修改 action 方法的结果。

The ``OnActionExecuting`` method runs before the action method, so it can manipulate the inputs to the action by changing ``ActionExecutingContext.ActionArguments`` or manipulate the controller through ``ActionExecutingContext.Controller``. An ``OnActionExecuting`` method can short-circuit execution of the action method and subsequent action filters by setting ``ActionExecutingContext.Result``. Throwing an exception in an ``OnActionExecuting`` method will also prevent execution of the action method and subsequent filters, but will be treated as a failure instead of successful result.

``OnActionExecuting`` 方法在 action 方法执行之前运行，因此它可以通过改变 ``ActionExecutingContext.ActionArguments`` 来控制 action 的输入，或是通过 ``ActionExecutingContext.Controller`` 控制控制器（Controller）。``OnActionExecuting`` 方法可以通过设置 ``ActionExecutingContext.Result`` 来短路 action 方法的操作及其后续的过滤器。``OnActionExecuting`` 方法通过抛出异常也可以阻止 action 方法和后续过滤器的处理，但会当做失败（而不是成功）的结果来处理。

The ``OnActionExecuted`` method runs after the action method and can see and manipulate the results of the action through the ``ActionExecutedContext.Result`` property. ``ActionExecutedContext.Canceled`` will be set to true if the action execution was short-circuited by another filter. ``ActionExecutedContext.Exception`` will be set to a non-null value if the action or a subsequent action filter threw an exception. Setting ``ActionExecutedContext.Exception`` to null effectively 'handles' an exception, and ``ActionExectedContext.Result`` will then be executed as if it were returned from the action method normally.

``OnActionExecuted`` 方法在 action 方法执行之后才执行，并且可以通过 ``ActionExecutedContext.Result`` 属性查看或控制 action 的结果。如果 action 在执行时被其它过滤器短路，则 ``ActionExecutedContext.Canceled`` 将会被置为 true。如果 action 或后续的 action 过滤器抛出异常，则 ``ActionExecutedContext.Exception`` 会被设置为一个非空值。有效「处理」完异常后把 ``ActionExecutedContext.Exception`` 设置为 null，那么 ``ActionExectedContext.Result`` 会像从 action 方法正常返回值那样被处理。

For an ``IAsyncActionFilter`` the ``OnActionExecutionAsync`` combines all the possibilities of ``OnActionExecuting`` and ``OnActionExecuted``. A call to ``await next()`` on the ``ActionExecutionDelegate`` will execute any subsequent action filters and the action method, returning an ``ActionExecutedContext``. To short-circuit inside of an ``OnActionExecutionAsync``, assign ``ActionExecutingContext.Result`` to some result instance and do not call the ``ActionExectionDelegate``.

对于 ``IAsyncActionFilter`` 接口来说，它的 ``OnActionExecutionAsync`` 方法结合了 ``OnActionExecuting`` 和 ``OnActionExecuted`` 的所有能力。调用 ``await next()`` 后，``ActionExecutionDelegate`` 将会执行所有的后续 action 过滤器以及 action 方法，并返回 ``ActionExecutedContext``\ 。
如果想要在 ``OnActionExecutionAsync`` 内部短路，那么就位 ``ActionExecutingContext.Result`` 分配一个结果实例，并不要去调用 ``ActionExectionDelegate`` 即可。

.. _exception-filters:

异常过滤器
-----------------

*Exception Filters* implement either the ``IExceptionFilter`` or ``IAsyncExceptionFilter`` interface.

Exception filters handle unhandled exceptions, including those that occur during controller creation and :doc:`model binding </mvc/models/model-binding>`. They are only called when an exception occurs in the pipeline. They can provide a single location to implement common error handling policies within an app. The framework provides an abstract `ExceptionFilterAttribute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Filters/ExceptionFilterAttribute/index.html>`_ that you should be able to subclass for your needs. Exception filters are good for trapping exceptions that occur within MVC actions, but they're not as flexible as error handling middleware. Prefer middleware for the general case, and use filters only where you need to do error handling *differently* based on which MVC action was chosen.

.. tip:: One example where you might need a different form of error handling for different actions would be in an app that exposes both API endpoints and actions that return views/HTML. The API endpoints could return error information as JSON, while the view-based actions could return an error page as HTML.

Exception filters do not have two events (for before and after) - they only implement ``OnException`` (or ``OnExceptionAsync``). The ``ExceptionContext`` provided in the ``OnException`` parameter includes the ``Exception`` that occurred. If you set ``context.Exception`` to null, the effect is that you've handled the exception, so the request will proceed as if it hadn't occurred (generally returning a 200 OK status). The following filter uses a custom developer error view to display details about exceptions that occur when the application is in development:

.. literalinclude:: filters/sample/src/FiltersSample/Filters/CustomExceptionFilterAttribute.cs
  :language: c#
  :emphasize-lines: 33-34

.. _result-filters:

结果过滤器
-------------

*Result Filters* implement either the ``IResultFilter`` or ``IAsyncResultFilter`` interface and their execution surrounds the execution of action results. Result filters are only executed for successful results - when the action or action filters produce an action result. Result filters are not executed when exception filters handle an exception, unless the exception filter sets ``Exception = null``.

.. note:: The kind of result being executed depends on the action in question. An MVC action returning a view would include all razor processing as part of the ``ViewResult`` being executed. An API method might perform some serialization as part of the execution of the result. Learn more about :doc:`action results </mvc/controllers/actions>`

Result filters are ideal for any logic that needs to directly surround view execution or formatter execution. Result filters can replace or modify the action result that's responsible for producing the response.

The ``OnResultExecuting`` method runs before the action result is executed, so it can manipulate the action result through ``ResultExecutingContext.Result``. An ``OnResultExecuting`` method can short-circuit execution of the action result and subsequent result filters by setting ``ResultExecutingContext.Cancel`` to true. If short-circuited, MVC will not modify the response; you should generally write to the response object directly when short-circuiting to avoid generating an empty response. Throwing an exception in an ``OnResultExecuting`` method will also prevent execution of the action result and subsequent filters, but will be treated as a failure instead of a successful result.

The ``OnResultExecuted`` method runs after the action result has executed. At this point if no exception was thrown, the response has likely been sent to the client and cannot be changed further. ``ResultExecutedContext.Canceled`` will be set to true if the action result execution was short-circuited by another filter. ``ResultExecutedContext.Exception`` will be set to a non-null value if the action result or a subsequent result filter threw an exception. Setting ``ResultExecutedContext.Exception`` to null effectively 'handles' an exception and will prevent the exeception from being rethrown by MVC later in the pipeline. If handling an exception in a result filter, consider whether or not it's appropriate to write any data to the response. If the action result throws partway through its execution, and the headers have already been flushed to the client, there's no reliable mechanism to send a failure code.

For an ``IAsyncResultFilter`` the ``OnResultExecutionAsync`` combines all the possibilities of ``OnResultExecuting`` and ``OnResultExecuted``. A call to ``await next()`` on the ``ResultExecutionDelegate`` will execute any subsequent result filters and the action result, returning a ``ResultExecutedContext``. To short-circuit inside of an ``OnResultExecutionAsync``, set ``ResultExecutingContext.Cancel`` to true and do not call the ``ResultExectionDelegate``.

You can override the built-in ``ResultFilterAttribute`` to create result filters. The :ref:`AddHeaderAttribute <add-header-attribute>` class shown above is an example of a result filter.

.. tip:: If you need to add headers to the response, do so before the action result executes. Otherwise, the response may have been sent to the client, and it will be too late to modify it. For a result filter, this means adding the header in ``OnResultExecuting`` rather than ``OnResultExecuted``.

过滤器对比中间件
----------------------

In general, filters are meant to handle cross-cutting business and application concerns. This is often the same use case for :doc:`middleware </fundamentals/middleware>`. Filters are very similar to middleware in capability, but let you scope that behavior and insert it into a location in your app where it makes sense, such as before a view, or after model binding. Filters are a part of MVC, and have access to its context and constructs. For instance, middleware can't easily detect whether model validation on a request has generated errors, and respond accordingly, but a filter can easily do so.

一般情况下，过滤器是处理业务与应用程序的交叉关注点。它的用法很像 :doc:`中间件 </fundamentals/middleware>`\ 。从能力上来讲过滤器酷似中间件，但过滤器的作用范围很大，因此允许你将它插入到应用程序中需要使用到它的场合中，比如在视图之前或在模型绑定之后。过滤器是 MVC 的一部分，可以访问 MVC 的上下文以及构造函数。比方说，中间件不能简单地直接察觉请求中模型验证是否生成了错误并对此作出响应，而过滤器却能做到。

To experiment with filters, `download, test and modify the sample <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/filters/sample>`_.

如果想要尝试一下过滤器，\ `可以下载、测试并修改样例 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/filters/sample>`_\ 。
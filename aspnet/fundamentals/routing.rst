:version: 1.0.0-rc2

Routing
=======
作者 `Steve Smith`_

翻译： `张仁建(stoneniqiu) <http://github.com/stoneniqiu>`_

校对： 


Routing middleware is used to map requests to route handlers. Routes are configured when the application starts up, and can extract values from the URL that will be passed as arguments to route handlers. Routing functionality is also responsible for generating links that correspond to routes in ASP.NET apps.

路由中间件是用来把请求映射到处理程序。当应用程序启动起来，路由就配置好了，而且能够将URL中的值提取出来作为参数传递给处理程序。在ASP.NET 应用中，路由还负责根据路由规则来生成链接的功能。

.. contents:: 章节
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/routing/sample>`__

`访问或下载示例程序 <https://github.com/aspnet/Docs/tree/master/aspnet/fundamentals/routing/sample>`__


Routing Middleware
------------------
路由中间件
------------------

The routing :doc:`middleware <middleware>` uses *routes* to map requests to an ``IRouter`` instance. The ``IRouter`` instance chooses whether or not to handle the request, and how. The request is considered handled if its ``RouteContext.Handler`` property is set to a non-null value. If no route handler is found for a request, then the middleware calls *next* (and the next middleware in the request pipeline is invoked).

路由中间件根据路由规则把请求映射到一个 ``IRouter`` 的实例，而这个 ``IRouter`` 实例会选择是否现在处理这个请求。如果一个请求的 ``RouteContext.Handler`` 属性设置为true的话，那么就认为这个请求已经被处理了。 如果一个请求没有匹配上处理程序，那么中间件就会调用  *next*  方法（在请求管道中的下一个中间件会被调用）。


To use routing, add it to the **dependencies** in *project.json*:

在使用路由前，需要将其添加到 *project.json* 的  **依赖性** 中

.. literalinclude:: routing/sample/RoutingSample/project.json
  :dedent: 2
  :language: javascript
  :lines: 11-20
  :emphasize-lines: 7
  
Add routing to ``ConfigureServices`` in *Startup.cs*:

在 *Startup.cs* 的 ``ConfigureServices`` 中添加路由：

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :dedent: 8
  :language: c#
  :lines: 14-17
  :emphasize-lines: 3


Configuring Routing
-------------------
配置路由
--------

Routing is enabled in the ``Configure`` method in the ``Startup`` class. Create an instance of :dn:cls:`~Microsoft.AspNetCore.Routing.RouteBuilder`, passing a reference to ``IApplicationBuilder``. You can optionally provide a :dn:prop:`~Microsoft.AspNetCore.Routing.RouteBuilder.DefaultHandler` as well. Add additional routes using ``MapRoute`` and when finished call ``app.UseRouter``.

调用 ``Startup`` 类的 ``Configure`` 方法使能路由，通过引用 ``IApplicationBuilder`` 创建一个 ``RouteBuilder`` 实例，你也可以选择性的提供一个
Microsoft.AspNetCore.Routing.RouteBuilder.DefaultHandler 通过 ``MapRoute`` 方法增加额外的路由，并在结束时调用 ``app.UseRouter`` 。 

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :dedent: 8
  :lines: 20-38
  :emphasize-lines: 6-9,11,19
  
Pass ``UseRouter`` the result of the ``RouteBuilder.Build`` method.

将 ``RouteBuilder.Build`` 方法的结果传递给 ``UseRouter`` 。

.. tip:: If you are only configuring a single route, you can simply call ``app.UseRouter`` and pass in the ``IRouter`` instance you wish to use, bypassing the need to use a ``RouteBuilder``.

.. tip:: 如果你只配置了一个路由，你可以只调用 ``app.UseRouter`` 方法，并传递给你想使用的IRouter实例。而不必使用 ``RouteBuilder`` 。


The ``defaultHandler`` route handler is used as the default for the ``RouteBuilder``. Calls to ``MapRoute`` will use this handler by default. A second handler is configured within the ``HelloRouter`` instance added by the ``AddHelloRoute`` extension method. This extension methods adds a new ``Route`` to the ``RouteBuilder``, passing in an instance of ``IRouter``, a template string, and an ``IInlineConstraintResolver`` (which is responsible for enforcing any route constraints specified):

``defaultHandler`` 是作为 ``RouteBuilder`` 的默认路由处理程序。调用  ``MapRoute`` 时将默认使用这个处理程序。第二个处理程序是 ``AddHelloRoute`` 这个扩展方法增加的 ``HelloRouter`` 实例配置的。这个扩展方法增加了一个新路由到 ``RouteBuilder`` 。传递了一个 ``IRouter`` 的实例，一个模板字符串和一个 ``IInlineConstraintResolver`` (负责执行所以的路由约束)：


.. literalinclude:: routing/sample/RoutingSample/HelloExtensions.cs
  :language: c#
  :lines: 9-17
  :dedent: 8
  :emphasize-lines: 5

``HelloRouter`` is a custom ``IRouter`` implementation. ``AddHelloRoute`` adds an instance of this router to the ``RouteBuilder`` using a template string, "hello/{name:alpha}". This template will only match requests of the form "hello/{name}" where `name` is constrained to be alphabetical. Matching requests will be handled by ``HelloRouter`` (which implements the ``IRouter`` interface), which responds to requests with a simple greeting.

``HelloRouter`` 是一个自定义的 ``IRouter`` 实现， ``AddHelloRoute`` 方法使用一个模板字符串："hello/{name:alpha}"，增加了这个路由的一个实例到 ``RouteBuilder``。这个模板只匹配符合"hello/{name}"格式的请求，其种name必须是字母组成。匹配上的请求将由 ``HelloRouter`` (实现了 ``IRouter`` 接口)处理，它通过一个简单问候响应请求。



.. literalinclude:: routing/sample/RoutingSample/HelloRouter.cs
  :emphasize-lines: 8,20-23

``HelloRouter`` checks to see if ``RouteData`` includes a value for the key ``name``. If not, it immediately returns without handling the request. Likewise, it checks to see if the request begins with "/hello". Otherwise, the ``Handler`` property is set to a delegate that responds with a greeting. Setting the ``Handler`` property prevents additional routes from handling the request. The ``GetVirtualPath`` method is used for :ref:`link generation <link-generation>`.

``HelloRouter`` 会检查 ``RouteData`` 是否包含 ``name`` 的值。如果没有，就立刻返回而不作处理。同样，它会检查请求是否以"/hello"开头。否则，Handler属性会设置为一个响应问候句子的委托。设置 ``Handler`` 属性可以阻止额外的路由再去处理这个请求。 ``GetVirtualPath``  方法用来生成链接。

.. note:: Remember, it's possible for a particular route **template** to match a given request, but the associated route **handler** can still reject it, allowing a different route to handle the request.)

.. note:: 记住，一个给定的请求可以被特定的路由 **模板** 匹配，但相关的路由 **处理程序** 仍可以拒绝它。让别的路由去处理这个请求。

This route was configured to use an :ref:`inline constraint <route-constraints>`, signified by the ``:alpha`` in the name route value. This constraint limits which requests this route will handle, in this case to alphabetical values for ``name``. Thus, a request for "/hello/steve" will be handled, but a request to "/hello/123" will not (instead, in this sample the request will not match any routes and will use the "app.Run" delegate).

这个配置了的路由通过name标记为 ``:alpha`` 使用了一个内联约束。约束限定了路由要处理的请求，在本例中就是 ``name`` 的字母值。因此，一个"/hello/steve"的请求会被处理，但一个 "/hello/123" 这样的请求将不会被处理（换句话说，这个简单的路由不会匹配任何路由，而将使用 "app.Run"  这个委托）。


Template Routes
---------------
路由模板
---------------


The most common way to define routes is using ``TemplateRoute`` and route template strings. When a ``TemplateRoute`` matches, it calls its target ``IRouter`` handler. In a typical MVC app, you might use a default template route with a string like this one: 

定义路由最常用的方式是使用 ``模板路由`` 和路由模板字符串。当一个 ``模板路由`` 匹配了，它会调用它的目标 ``IRouter`` 处理程序。在一个典型的MVC应用中，你也许会使用一个默认的路由模板，像下面这个字符串：

.. image:: /fundamentals/routing/_static/default-mvc-routetemplate.png

This route template would be handled by the :dn:cls:`~Microsoft.AspNetCore.Mvc.Internal.MvcRouteHandler` ``IRouter`` instance. Tokens within curly braces (``{ }``) define `route value` parameters which will be bound if the route is matched. You can define more than one route value parameter in a route segment, but they must be separated by a literal value. For example ``{controller=Home}{action=Index}`` would not be a valid route, since there is no literal value between ``{controller}`` and ``{action}``. These route value parameters must have a name, and may have additional attributes specified.

这个路由模板会被 MvcRouteHandler ``IRouter`` 的实例处理，其中的大括号(``{ }``)定义了路由值 参数的边界。你可以在一个路由段中定义多个路由值参数，但它们必须用文字值分开，例如 ``{controller=Home}{action=Index}`` 不是一个有效路由，因为在 ``{controller}`` 和 ``{action}`` 之间没有文字值。这些路由值参数必须有一个名称，并可以有附加指定的属性。


You can use the ``*`` character as a prefix to a route value name to bind to the rest of the URI. For example, ``blog/{*slug}`` would match any URI that started with ``/blog/`` and had any value following it (which would be assigned to the ``slug`` route value).

你可以用 ``*`` 符号作为路由值名称的前缀，绑定到其余的URI。例如，``blog/{*slug}`` 将会匹配任何以 ``/blog/`` 开头的URI，且其后可跟任何值（将会分配给这个 ``slug`` 路由值）


Route value parameters may have *default values*, designated by specifying the default after the parameter name, separated by an ``=``. For example, ``controller=Home`` would define ``Home`` as the default value for ``controller``. The default value is used if no value is present in the URL for the parameter. In addition to default values, route parameters may be optional (specified by appending a ``?`` to the end of the parameter name, as in ``id?``). The difference between optional and "has default" is that a route parameter with a default value always produces a value; an optional parameter may not. Route parameters may also have constraints, which further restrict which routes the template will match.

路由值参数可以有 *默认值*，通过在参数名后指定，用 ``=`` 分开。例如， ``controller=Home`` 定义了 ``Home`` 作为 ``controller`` 的默认值，如果URL中没有参数的值，就会默认值。除了默认值，路由参数可以是可选的（通过在参数名称后面附加一个?``来定义，比如 ``id?`` ），参数可选和默认的区别就是一个有默认值的路由总会产生一个值；但可选参数也许不会有值。路由参数也可以有约束，来进一步的限制要匹配的路由模板。


The following table demonstrates some route template and their expected behavior.

下表中推荐了一些路由模板和期望行为


.. list-table:: Route Template Values
  :header-rows: 1

  * - Route Template
  * - 路由模板 
    - Example Matching URL
    - URL示例
    - Notes
    - 注意点
  * - hello
    - /hello
    - Will only match the single path '/hello'
    - 将只匹配'/hello' 路径
  * - {Page=Home}
    - /
    - Will match and set ``Page`` to ``Home``.
    - 将匹配且设置 ``Page`` 为 ``Home`` 。
  * - {Page=Home}
    - /Contact
    - Will match and set ``Page`` to ``Contact``
    - 将匹配且设置 ``Page`` 为 ``Contact``
  * - {controller}/{action}/{id?}
    - /Products/List
    - Will map to ``Products`` controller and ``List`` method; Since ``id`` was not supplied in the URL, it's ignored.
    - 会映射到 ``Products`` 控制器的 ``List`` 方法，即使URL中没有提供 ``id`` 值，会忽略掉。
  * - {controller}/{action}/{id?}
    - /Products/Details/123
    - Will map to ``Products`` controller and ``Details`` method, with ``id`` set to ``123``.
    -会映射到 ``Products``控制器的 ``Details``方法，且 ``id`` 的值为 ``123``.
  * - {controller=Home}/{action=Index}/{id?}
    - /
    - Will map to ``Home`` controller and ``Index`` method; ``id`` is ignored.
    - 会映射到 ``Home`` 控制器的 ``Index`` 方法，id 忽略掉。

.. _route-constraints:

Route Constraints
^^^^^^^^^^^^^^^^^

路由约束
^^^^^^^^^^^^^^^^^


Adding a colon ``:`` after the name allows additional inline constraints to be set on a route value parameter. Constraints with types always use the invariant culture - they assume the URL is non-localizable. Route constraints limit which URLs will match a route - URLs that do not match the constraint are ignored by the route.

给一个路由值参数设置额外的内联约束，需要在它的名称后面增加一个冒号， 同类型的约束始终使用固定区域性。它们假定URL是不需本地化的。路由约束限定了一个路由将匹配的 route - URLs 没匹配上约束的URLs将会被这个路由忽略掉。
 

.. list-table:: Inline Route Constraints
.. list-table:: 内联路由约束
  :header-rows: 1

  * - constraint
  * - 约束
    - Example
    - 示例
    - Example Match
    - 匹配示例
    - Notes
    - 注意点
  * - ``int``
    - {id:int}
    - 123
    - Matches any integer
    - 匹配任何整形
  * - ``bool``
    - {active:bool}
    - true
    - Matches ``true`` or ``false``
    -匹配 ``true`` 或 ``false``
  * - ``datetime``
    - {dob:datetime}
    - 2016-01-01
    - Matches a valid ``DateTime`` value (in the invariant culture - see `options <http://msdn.microsoft.com/en-us/library/aszyst2c(v=vs.110).aspx>`_)
    - 匹配一个合法 ``DateTime`` 值
  * - ``decimal``
    - {price:decimal}
    - 49.99
    - Matches a valid ``decimal`` value
    -匹配一个合法的 ``decimal`` 值
  * - ``double``
    - {price:double}
    - 4.234
    - Matches a valid ``double`` value
    -匹配一个合法的 ``double`` 值
  * - ``float``
    - {price:float}
    - 3.14
    - Matches a valid ``float`` value
    -匹配一个合法的 ``float`` 值
  * - ``guid``
    - {id:guid}
    - 7342570B-44E7-471C-A267-947DD2A35BF9
    - Matches a valid ``Guid`` value
    -匹配一个合法的 ``Guid`` 值
  * - ``long``
    - {ticks:long}
    - 123456789
    - Matches a valid ``long`` value
    -匹配一个合法的 ``long`` 值
  * - ``minlength(value)``
    - {username:minlength(5)}
    - steve
    - String must be at least 5 characters long.
    - 至少5个字符长
  * - ``maxlength(value)``
    - {filename:maxlength(8)}
    - somefile
    - String must be no more than 8 characters long.
    - 不能超过8个字符长度
  * - ``length(min,max)``
    - {filename:length(4,16)}
    - Somefile.txt
    - String must be at least 8 and no more than 16 characters long.
    -至少8个字符长至多16个字符长。
  * - ``min(value)``
    - {age:min(18)}
    - 19
    - Value must be at least 18.
    - 值最小是18
  * - ``max(value)``
    - {age:max(120)}
    - 91
    - Value must be no more than 120.
    - 值最大是18
  * - ``range(min,max)``
    - {age:range(18,120)}
    - 91
    - Value must be at least 18 but no more than 120.
    - 值介于18和120之间
  * - ``alpha``
    - {name:alpha}
    - Steve
    - String must consist of alphabetical characters.
    - 必须是字母型字符组成
  * - ``regex(expression)``
    - {ssn:regex(\d{3}-\d{2}-\d{4})}
    - 123-45-6789
    - String must match the provided regular expression.
    - 必须匹配正则表达式
  * - ``required``
    - {name:required}
    - Steve
    - Used to enforce that a non-parameter value is present during during URL generation.
    - 生成URL的时候用来强制提供参数值。

Inline constraints must match one of the above options, or an exception will be thrown.

内联约束必须匹配上面选项的其中之一，否则会抛出异常。

.. tip:: To constrain a parameter to a known set of possible values, you can use a regex: ``{action:regex(list|get|create)}``. This would only match the ``action`` route value to ``list``, ``get``, or ``create``. If passed into the constraints dictionary, the string "list|get|create" would be equivalent. Constraints that are passed in the constraints dictionary (not inline within a template) that don't match one of the known constraints are also treated as regular expressions.

.. tip:: 你可以使用正则表达式来约束参数是一系列可能的值。例如 ``{action:regex(list|get|create)}`` ，这将只匹配 ``action`` 的值是 ``list`` 、 ``get`` 或 ``create`` 。 如果将 "list|get|create" 传入约束字典，是等价的。传入约束字典的约束(没有内联模板)，没有匹配到已知的约束也会被视为正则表达式。

.. warning:: Avoid using constraints for **validation**, because doing so means that invalid input will result in a 404 (Not Found) instead of a 400 with an appropriate error message. Route constraints should be used to **disambiguate** between routes, not validate the inputs for a particular route.

.. warning:: 避免使用约束来做验证，这样做意味着非法输入会得到一个404(没有找到)的结果。而不是一个400的状态码加上合适的错误信息。路由约束应该用来在路由间做区分，而不是为了特定的路由做验证。


Constraints can be *chained*. You can specify that a route value is of a certain type and also must fall within a specified range, for example: ``{age:int:range(1,120)}``.  Numeric constraints like ``min``, ``max``, and ``range`` will automatically convert the value to ``long`` before being applied unless another numeric type is specified.

约束可以是 *链式* 的。你可以定义一个路由值是某种类型而且必须在一个特定范围内，例如： ``{age:int:range(1,120)}``。数字约束像 ``min``、 ``max`` 和 ``range`` 在应用之前会自动将值转为 ``long`` 型，除非定义了另外一个数据类型。

Route templates must be unambiguous, or they will be ignored. For example, ``{id?}/{foo}`` is ambiguous, because it's not clear which route value would be bound to a request for "/bar". Similarly, ``{*everything}/{plusone}`` would be ambiguous, because the first route parameter would match everything from that part of the request on, so it's not clear what the ``plusone`` parameter would match.

路由模板必须明确，否则将被忽略，例如 ``{id?}/{foo}`` 就不明确，因为不清楚哪个路由会匹配到 "/bar" 这样的请求。类似的 ``{*everything}/{plusone}`` 也不够明确，因为第一个路由参数可以匹配请求的所有部分，不清楚 ``plusone`` 参数将要匹配什么。
 

.. note:: There is a special case route for filenames, such that you can define a route value like ``files/{filename}.{ext?}``. When both ``filename`` and ``ext`` exist, both values will be populated. However, if only ``filename`` exists in the URL, the trailing period ``.`` is also optional. Thus, these would both match: ``/files/foo.txt`` and ``/files/foo``.

.. note:: 对于文件名有专门的路由案例，你可以定义一个路由像这样 ``files/{filename}.{ext?}``。当 ``filename`` 和 ``ext`` 都存在时，这两个值会被填充。然而，如果只有 ``filename`` 存在URL中，后面的扩展名是可选的。因此。``/files/foo.txt`` 和 ``/files/foo`` 都能匹配。


.. tip:: Enable :doc:`logging` to see how the built in routing implementations, such as ``TemplateRoute``, match requests.

.. tip:: 启用日志去看内置路由的是怎样实现的，比如 ``TemplateRoute`` ，匹配请求。


Route Builder Extensions
------------------------
路由生成器扩展
------------------------


Several `extension methods on RouteBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Builder/MapRouteRouteBuilderExtensions/index.html>`_ are available for convenience. The most common of these is ``MapRoute``, which allows the specification of a route given a name and template, and optionally default values, constraints, and/or :ref:`data tokens <data-tokens>`. When using these extensions, you must have specified the ``DefaultHandler`` and ``ServiceProvider`` properties of the ``RouteBuilder`` instance to which you're adding the route. These ``MapRoute`` extensions add new ``TemplateRoute`` instances to the ``RouteBuilder`` that each target the ``IRouter`` configured as the ``DefaultHandler``.

路由构建器有几个扩展方法是比较方便的，最常用的是 ``MapRoute``，用来规范路由的名称和模板，以及可选、默认值，约束，and/or :ref:data tokens <data-tokens>。当使用这些扩展的时候，你必须给 ``RouteBuilder`` 实例指定 ``DefaultHandler`` 和 ``ServiceProvider`` 属性到你正添加的路由.这些 ``MapRoute`` 的扩展增加新的 ``TemplateRoute`` 实例到 ``RouteBuilder``，每一个目标 ``IRouter`` 配置为 ``DefaultHandler``。

.. note:: ``MapRoute`` doesn't take an ``IRouter`` parameter - it only adds routes that will be handled by the ``DefaultHandler``. Since the default handler is an ``IRouter``, it may decide not to handle the request. For example, MVC is typically configured as a default handler that only handles requests that match an available controller action.

. note:: ``MapRoute`` 没有采用 ``IRouter`` 参数，它只添加将被 ``DefaultHandler`` 处理的路由。由于默认的处理程序是一个 ``IRouter`` ，它可能决定不处理请求。
例如， MVC通常配置了一个默认处理程序，它只处理能匹配到可用的控制器操作的请求。

.. _data-tokens:

Data Tokens
^^^^^^^^^^^
数据令牌
^^^^^^^^^^^

Data tokens represent data that is carried along if the route matches. They're implemented as a property bag for developer-specified data. You can use data tokens to store data you want to associate with a route, when you don't want the semantics of defaults. Data tokens have no impact on the **behavior** of the route, while defaults do. Data tokens can also be any arbitrary types, while defaults really need to be things that can be converted to/from strings.

如果路由匹配了，数据令牌代表的数据是单独携带的。他们作为开发人员指定数据的一个属性包实现。当你不需要默认值的时候,你可以用数据令牌存储一个你想的数据。数据令牌不会影响路由的行为，但默认值会。数据令牌可以是任意类型，而默认值需要是能够转成字符串的类型。


.. _link-generation:

Link Generation
---------------
链接生成
---------------


Routing is also used to generate URLs based on route definitions. This is used by helpers to generate links to known actions on MVC :doc:`controllers </mvc/controllers/index>`, but can also be used independent of MVC. Given a set of route values, and optionally a route name, you can produce a ``VirtualPathContext`` object. Using the ``VirtualPathContext`` object along with a ``RouteCollection``, you can generate a ``VirtualPath``. ``IRouter`` implementations participate in link generation through the ``GetVirtualPath`` method.

基于路由的定义，路由也可以用来生成URLs。在MVC中，被帮助类用来生成已知actions的链接。给定一系列路由值，和可选的路由名称，你可以创建一个  ``VirtualPathContext`` 对象，单独和 ``RouteCollection`` 使用 ``VirtualPathContext`` 对象，你可以生成一个 ``VirtualPath`` 。``IRouter`` 的实现通过 ``GetVirtualPath`` 方法参与链接的生成。


.. tip:: Learn more about `UrlHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Routing/UrlHelper/index.html?highlight=urlhelper>`_ and :doc:`Routing to Controller Actions </mvc/controllers/routing>`.

.. tip:: 了解更多关于 `UrlHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Routing/UrlHelper/index.html?highlight=urlhelper>` 和 `Routing to Controller Actions </mvc/controllers/routing>`。


The example below shows how to generate a link to a route given a dictionary of route values and a ``RouteCollection``.

下面的例子展示了在给定一个路由值字典和一个路由集合的情况下如何生成一个链接。

.. literalinclude:: routing/sample/RoutingSample/Startup.cs
  :language: c#
  :lines: 39-59
  :dedent: 12
  :emphasize-lines: 2-3,7,13-14,19-20

The ``VirtualPath`` generated at the end of the sample above is ``/package/create/123``.

上面示例最终生成的相对路径是``/package/create/123``。 

The second parameter to the ``VirtualPathContext`` constructor is a collection of `ambient values`. Ambient values provide convenience by limiting the number of values a developer must specify within a certain request context. The current route values of the current request are considered ambient values for link generation. For example, in an MVC application if you are in the About action of the HomeController, you don't need to specify the controller route value to link to the Index action (the ambient value of Home will be used). 

``VirtualPathContext`` 构造函数的第二个参数是一个 `ambient值` 的集合。通过限制开发人员必须在特定请求上下文中定义值的数量，环境值提供了方便。当前请求的路由值被当做生成链接的环境值。例如，在MVC应用中，如果正在Home控制器的About方法中，链接到Index方法时你不需要定义控制器路由值（Home的环境值将会被使用）。

Ambient values that don't match a parameter are ignored, and ambient values are also ignored when an explicitly-provided value overrides it, going from left to right in the URL.

没有匹配到参数的环境值将被忽略，同样有明确保留的值覆盖它，按照URL中从做到右的顺序，环境值也会被忽略。

Values that are explicitly provided but which don't match anything are added to the query string.

显示提供的但没有匹配的值将会被加到查询字符串中。

.. list-table:: Generating Links
  :header-rows: 1

  * - Matched Route
    - Ambient Values
    - Explicit Values
    - Result
  * - ``{controller}/{action}/{id?}``
    - controller="Home"
    - action="About"
    - ``/Home/About``
  * - ``{controller}/{action}/{id?}``
    - controller="Home"
    - controller="Order",action="About"
    - ``/Order/About``
  * - ``{controller}/{action}/{id?}``
    - controller="Home",color="Red"
    - action="About"
    - ``/Home/About``
  * - ``{controller}/{action}/{id?}``
    - controller="Home"
    - action="About",color="Red"
    - ``/Home/About?color=Red``

If a route has a default value that doesn't match a parameter and that value is explicitly provided, it must match the default value. For example:

如果一个路由有一个没有匹配到参数的默认值，而且这个值被提供了，那它必须匹配这个默认值。例如：

.. code-block:: c#
  
  routes.MapRoute("blog_route", "blog/{*slug}", 
    defaults: new { controller = "Blog", action = "ReadPost" });

Link generation would only generate a link for this route when the matching values for controller and action are provided.

当提供了controller和action的匹配值，才能生成这个路由的链接。 

Recommendations
---------------
建议
---------------



Routing is a powerful feature that is built into the default ASP.NET MVC project template such that most apps will be able to leverage it without having to customize its behavior. This is by design; customizing routing behavior is an advanced development approach. Keep in mind the following recommendations with regard to routing: 

路由是一个强大的特性，已经内置在默认的 ASP.NET MVC 工程模板中，因此大多数的应用不需要定制路由行为就可以使用了。就是这样设计的，定制路由行为是一个高级的开发方法。记住下面几条关于路由的建议：

  - Most apps shouldn't need custom routes. The default route will work in most cases.
  - 大多数的应用不需要定制路由.默认的路由在大多数情况下能正常工作。
  - Attribute routes should be used for all APIs.
  - 所有的API应该使用路由特性
  - Attribute routes are recommended for when you need complete control over your app's URLs.
  - 如果你需要完全控制应用的URL推荐使用路由特性。
  - Conventional routing is recommended for when **all** of your controllers/actions fit a uniform URL convention.
  - 如果所有的controllers/actions符合一个统一的约定，推荐使用传统的路由。
  - Don't use custom routes unless you understand them well and are sure you need them.
  - 除非你理解清楚而且确定需要，否则不要使用自定义路由.
  - Routes can be tricky to test and debug.
  - 路由难以测试和调试.
  - Routes should not be used as a means of securing your controllers or their action methods.
  - 路由不应该作为保护控制器和方法的一种手段.
  - Avoid building or changing route collections at runtime.
  - 避免在运行时创建和改变路由集合

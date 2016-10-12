.. Routing to Controller Actions
.. ========================================

控制器操作的路由
========================================

作者： `Ryan Nowak`_ 、 `Rick Anderson`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： 

.. ASP.NET Core MVC uses the Routing :doc:`middleware </fundamentals/middleware>` to match the URLs of incoming requests and map them to actions. Routes are defined in startup code or attributes. Routes describe how URL paths should be matched to actions. Routes are also used to generate URLs (for links) sent out in responses.

ASP.NET Core MVC 使用路由 :doc:`中间件 </fundamentals/middleware>` 来匹配传入请求的 URL 并映射到具体的操作。路由通过启动代码或者特性定义。路由描述 URL 路径应该如何匹配到操作。路由也同样用于生成相应中返回的 URL（用于链接）。

.. This document will explain the interactions between MVC and routing, and how typical MVC apps make use of routing features. See :doc:`Routing </fundamentals/routing>` for details on advanced routing.

这篇文章将解释 MVC 和路由之间的相互作用，以及典型的 MVC 应用程序如何使用路由特性。查看 :doc:`路由 </fundamentals/routing>` 获取更多高级路由信息。

.. .. contents:: Sections:
..  :local:
..  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

.. Setting up Routing Middleware
.. ------------------------------

配置路由中间件
------------------------------

.. In your `Configure` method you may see code similar to::

在你的 `Configure` 方法中也许能看到以下代码::

  app.UseMvc(routes =>
  {
     routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
  });

.. Inside the call to :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvc`, :dn:method:`~Microsoft.AspNetCore.Builder.MapRouteRouteBuilderExtensions.MapRoute` is used to create a single route, which we'll refer to as the ``default`` route. Most MVC apps will use a route with a template similar to the ``default`` route.

其中对 :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvc`， :dn:method:`~Microsoft.AspNetCore.Builder.MapRouteRouteBuilderExtensions.MapRoute` 的调用用来创建单个路由，我们称之为 ``default`` 路由。大部分 MVC 应用程序使用路由模板类似 ``default`` 路由。

.. The route template ``"{controller=Home}/{action=Index}/{id?}"`` can match a URL path like ``/Products/Details/5`` and will extract the route values ``{ controller = Products, action = Details, id = 5 }`` by tokenizing the path. MVC will attempt to locate a controller named ``ProductsController`` and run the action ``Details``::

路由模板 ``"{controller=Home}/{action=Index}/{id?}"`` 能够匹配路由比如 ``/Products/Details/5`` 并会通过标记路径提取路由值 ``{ controller = Products, action = Details, id = 5 }``。MVC 将尝试定位名为 ``ProductsController`` 的控制器并运行操作 ``Details``::

  public class ProductsController : Controller
  {
     public IActionResult Details(int id) { ... }
  }

.. Note that in this example, model binding would use the value of ``id = 5`` to set the ``id`` parameter to ``5`` when invoking this action. See the :doc:`/mvc/models/model-binding` for more details.

注意这个例子，当调用这个操作时，模型绑定会使用 ``id = 5`` 的值来将 ``id`` 参数设置为 ``5``。查看 :doc:`/mvc/models/model-binding` 获取更多信息。

.. Using the ``default`` route::

使用 ``default`` 路由::

   routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

.. The route template:

.. - ``{controller=Home}`` defines ``Home`` as the default ``controller``
.. - ``{action=Index}`` defines ``Index`` as the default ``action``
.. - ``{id?}`` defines ``id`` as optional

路由模板：

- ``{controller=Home}`` 定义 ``Home`` 作为默认的 ``controller``
- ``{action=Index}`` 定义 ``Index`` 作为默认的 ``action``
- ``{id?}`` 定义 ``id`` 为可选项

.. Default and optional route parameters do not need to be present in the URL path for a match. See the  :doc:`Routing </fundamentals/routing>` for a detailed description of route template syntax.

默认和可选路由参数不需要出现在 URL 路径，查看 :doc:`Routing </fundamentals/routing>` 获取路由模板语法的详细描述。

.. ``"{controller=Home}/{action=Index}/{id?}"`` can match the URL path ``/`` and will produce the route values ``{ controller = Home, action = Index }``. The values for ``controller`` and ``action`` make use of the default values, ``id`` does not produce a value since there is no corresponding segment in the URL path. MVC would use these route values to select the ``HomeController`` and ``Index`` action::

``"{controller=Home}/{action=Index}/{id?}"`` 可以匹配 URL 路径 ``/`` 并产生路由值 ``{ controller = Home, action = Index }``。 ``controller`` 和 ``action`` 使用默认值，因为在 URL 路径中没有响应的片段，所以 ``id`` 不会产生值。MVC会使用这些路由值选择 ``HomeController`` 和 ``Index`` 操作::

  public class HomeController : Controller
  {
    public IActionResult Index() { ... }
  }

.. Using this controller definition and route template, the ``HomeController.Index`` action would be executed for any of the following URL paths:

使用这个控制器和路由模板， ``HomeController.Index`` 操作会被以下任一 URL 路径执行：

- ``/Home/Index/17``
- ``/Home/Index``
- ``/Home``
- ``/``

.. The convenience method :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvcWithDefaultRoute`::

简便的方法 :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvcWithDefaultRoute`::

  app.UseMvcWithDefaultRoute();

.. Can be used to replace::

可以被替换为::

  app.UseMvc(routes =>
  {
     routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
  });

.. ``UseMvc`` and ``UseMvcWithDefaultRoute`` add an instance of :dn:cls:`~Microsoft.AspNetCore.Builder.RouterMiddleware` to the middleware pipeline. MVC doesn't interact directly with middleware, and uses routing to handle requests. MVC is connected to the routes through an instance of :dn:cls:`~Microsoft.AspNetCore.Mvc.Internal.MvcRouteHandler`. The code inside of ``UseMvc`` is similar to the following::

``UseMvc`` 和 ``UseMvcWithDefaultRoute`` 添加一个 :dn:cls:`~Microsoft.AspNetCore.Builder.RouterMiddleware` 的实例到中间件管道。MVC 不直接与中间件交互，使用路由来处理请求。MVC 通过 :dn:cls:`~Microsoft.AspNetCore.Mvc.Internal.MvcRouteHandler` 的实例连接到路由。``UseMvc`` 中的代码类似于下面::

   var routes = new RouteBuilder(app);

   // 添加连接到 MVC，将通过调用 MapRoute 连接。
   routes.DefaultHandler = new MvcRouteHandler(...);

   // 执行回调来注册路由。
   // routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

   // 创建路由集合并添加中间件。
   app.UseRouter(routes.Build());

..   var routes = new RouteBuilder(app);

..   // Add connection to MVC, will be hooked up by calls to MapRoute.
..   routes.DefaultHandler = new MvcRouteHandler(...);

..   // Execute callback to register routes.
..   // routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

..   // Create route collection and add the middleware.
..   app.UseRouter(routes.Build());
  
.. :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvc` does not directly define any routes, it adds a placeholder to the route collection for the ``attribute`` route. The overload ``UseMvc(Action<IRouteBuilder>)`` lets you add your own routes and also supports attribute routing.  ``UseMvc`` and all of its variations adds a placeholder for the attribute route - attribute routing is always available regardless of how you configure ``UseMvc``. :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvcWithDefaultRoute` defines a default route and supports attribute routing. The :ref:`attribute-routing-ref-label` section includes more details on attribute routing.

:dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvc` 不会直接定义任何路由，它为 ``特性`` 路由在路由集合中添加了一个占位符。``UseMvc(Action<IRouteBuilder>)`` 这个重载让你添加自己的路由并且也支持特性路由。``UseMvc`` 和它所有的重载都为特性路由添加占位符，不管你如何配置 ``UseMvc`` ，特性路由总是可用的。 :dn:method:`~Microsoft.AspNetCore.Builder.MvcApplicationBuilderExtensions.UseMvcWithDefaultRoute` 定义一个默认路由并支持特性路由。
:ref:`attribute-routing-ref-label` 章节包含了特性路由的信息。

.. Conventional routing
.. ---------------------

.. _routing-conventional-ref-label:

常规路由
---------------------

.. The ``default`` route::

``default`` 路由::

  routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

.. is an example of a *conventional routing*. We call this style *conventional routing* because it establishes a *convention* for URL paths:

是一个 *常规路由* 的例子。我们将这种风格称为 *常规路由* 因为它为 URL 路径建立了一个 *约定* ：

.. -  the first path segment maps to the controller name
.. -  the second maps to the action name.
.. -  the third segment is used for an optional ``id`` used to map to a model entity

-  第一个路径片段映射控制器名。
-  第二个片段映射操作名。
-  第三个片段是一个可选的 ``id`` 用于映射到模型实体。

.. Using this ``default`` route, the URL path ``/Products/List`` maps to the ``ProductsController.List`` action, and ``/Blog/Article/17`` maps to ``BlogController.Article``. This mapping is based on the controller and action names **only** and is not based on namespaces, source file locations, or method parameters.

使用这个 ``default`` 路由，URL 路径 ``/Products/List`` 映射到 ``ProductsController.List`` 操作，``/Blog/Article/17`` 映射到 ``BlogController.Article``。这个映射只基于控制器名和操作名，与命名空间、源文件位置或者方法参数无关。

.. .. Tip:: Using conventional routing with the default route allows you to build the application quickly without having to come up with a new URL pattern for each action you define. For an application with CRUD style actions, having consistency for the URLs across your controllers can help simplify your code and make your UI more predictable.

.. Tip:: 使用默认路由的常规路由使你可以快速构建应用程序，而不必为你定义的每一个操作想新的 URL 模式。对于 CRUD 风格操作的应用程序，保持访问控制器 URL 的一致性可以帮助简化你的代码并使你的 UI 更加可预测。

.. .. warning:: The ``id`` is defined as optional by the route template, meaning that your actions can execute without the ID provided as part of the URL. Usually what will happen if ``id`` is omitted from the URL is that it will be set to ``0`` by model binding, and as a result no entity will be found in the database matching ``id == 0``. Attribute routing can give you fine-grained control to make the ID required for some actions and not for others. By convention the documentation will include optional parameters like ``id`` when they are likely to appear in correct usage.

.. warning:: ``id`` 在路由模板中定义为可选，意味着你可以执行操作且不需要在 URL 中提供 ID。通常在 URL 中忽略 ``id`` 会通过模型绑定设置为 ``0``，并且没有实体会通过在数据库中匹配 ``id == 0`` 被找到。特性路由可以提供细粒度控制使 ID 在某些操作中必传以及其他操作中不必传。按照惯例，当可选参数可能出现在正确的用法时，文档将包括它们，比如 ``id``。

.. Multiple Routes
.. -------------------

多路由
-------------------

.. You can add multiple routes inside ``UseMvc`` by adding more calls to ``MapRoute``. Doing so allows you to define multiple conventions, or to add conventional routes that are dedicated to a specific action, such as::

你可以在 ``UseMvc`` 中通过添加 ``MapRoute`` 调用来添加多个路由。这样做让你可以定义多个约定，或者添加专用于一个特定操作的常规路由，比如::

   app.UseMvc(routes =>
   {
      routes.MapRoute("blog", "blog/{*article}",
               defaults: new { controller = "Blog", action = "Article" });
      routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
   }

.. The ``blog`` route here is a *dedicated conventional route*, meaning that it uses the conventional routing system, but is dedicated to a specific action. Since ``controller`` and ``action`` don't appear in the route template as parameters, they can only have the default values, and thus this route will always map to the action ``BlogController.Article``.

``blog`` 路由在这里是一个 *专用常规路由*，意味着它使用常规路由系统，但是专用于一个特殊的操作。由于 ``controller`` 和 ``action`` 不会作为参数出现在路由模板中，它们只能拥有默认值，因此这个路由将总是映射到操作 ``BlogController.Article``。

.. Routes in the route collection are ordered, and will be processed in the order they are added. So in this example, the ``blog`` route will be tried before the ``default`` route.

路由在路由集合中是有序的，并将按照它们添加的顺序处理。所以在这个例子中，``blog`` 路由会在 ``default`` 路由之前尝试。

.. .. note:: *Dedicated conventional routes* often use catch-all route parameters like ``{*article}`` to capture the remaining portion of the URL path. This can make a route 'too greedy' meaning that it matches URLs that you intended to be matched by other routes. Put the 'greedy' routes later in the route table to solve this.

.. note:: *专用常规路由* 通常捕捉所有参数，比如使用 ``{*article}`` 捕捉 URL 路径的剩余部分。这样使得路由 '太贪婪'，这意味着它将匹配所有你打算与其他路由规则匹配的路由。把 'greedy' 路由在路由表中置后来解决这个问题。

.. Fallback
.. ^^^^^^^^^

回退
^^^^^^^^^

.. As part of request processing, MVC will verify that the route values can be used to find a controller and action in your application. If the route values don't match an action then the route is not considered a match, and the next route will be tried. This is called *fallback*, and it's intended to simplify cases where conventional routes overlap.

作为请求处理的一部分，MVC 将验证路由值是否可以用来在你的应用程序中找到控制器和操作。如果路由值不匹配任何操作，则不会认为路由匹配成功，将会尝试下一个路由。这叫做 *回退*，它的目的是简化路由重叠的情况。

.. Disambiguating Actions
.. ^^^^^^^^^^^^^^^^^^^^^^^^

消除歧义操作
^^^^^^^^^^^^^^^^^^^^^^^^

.. When two actions match through routing, MVC must disambiguate to choose the 'best' candidate or else throw an exception. For example::

当两个操作通过路由匹配，MVC 必须消除歧义来选择‘最好的’候选，或者抛出一个异常，比如::

   public class ProductsController : Controller
   {
      public IActionResult Edit(int id) { ... }

      [HttpPost]
      public IActionResult Edit(int id, Product product) { ... }
   }

.. This controller defines two actions that would match the URL path ``/Products/Edit/17`` and route data ``{ controller = Products, action = Edit, id = 17 }``. This is a typical pattern for MVC controllers where ``Edit(int)`` shows a form to edit a product, and ``Edit(int, Product)`` processes  the posted form. To make this possible MVC would need to choose ``Edit(int, Product)`` when the request is an HTTP ``POST`` and ``Edit(int)`` when the HTTP verb is anything else.

这个控制器定义两个操作，它们都会匹配 URL 路径 ``/Products/Edit/17`` 以及路由数据是 ``{ controller = Products, action = Edit, id = 17 }``。这是 MVC 控制器中一个典型模式，其中 ``Edit(int)`` 显示编辑产品的表单，``Edit(int, Product)`` 处理提交上来的表单。为了确保这样可行，MVC 需要在请求是 HTTP ``POST`` 时选择 ``Edit(int, Product)``，并在其他 HTTP 谓词时选择 ``Edit(int)``。

.. The :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpPostAttribute` ( ``[HttpPost]`` ) is an implementation of :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` that will only allow the action to be selected when the HTTP verb is ``POST``. The presence of an ``IActionConstraint`` makes the ``Edit(int, Product)`` a 'better' match than ``Edit(int)``, so ``Edit(int, Product)`` will be tried first. See :ref:`iactionconstraint-ref-label` for details.

:dn:cls:`~Microsoft.AspNetCore.Mvc.HttpPostAttribute` ( ``[HttpPost]`` ) 是 :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` 的一个实现，它仅允许 HTTP 谓词为 ``POST`` 的请求访问操作。``IActionConstraint`` 的存在使得 ``Edit(int, Product)`` 比 ``Edit(int)`` 更好匹配，所以会先首先尝试 ``Edit(int, Product)``。查看 :ref:`iactionconstraint-ref-label` 获取更多信息。

.. You will only need to write custom ``IActionConstraint`` implementations in specialized scenarios, but it's important to understand the role of attributes like ``HttpPostAttribute``  - similar attributes are defined for other HTTP verbs. In conventional routing it's common for actions to use the same action name when they are part of a ``show form -> submit form`` workflow. The convenience of this pattern will become more apparent after reviewing the :ref:`routing-url-gen-ref-label` section.

你只会在专门的场景才需要编写自定义的 ``IActionConstraint`` 实现，但重要的是要理解特性的作用，比如 ``HttpPostAttribute`` —— 以及为其他 HTTP 谓词定义的类似的特性。在常规路由中，当操作是“现实表单 -> 提交表单”工作流时，操作使用相同的名字是很常见的。在回顾 :ref:`routing-url-gen-ref-label` 章节后，这种模式的方便将变得更加明显。

.. If multiple routes match, and MVC can't find a 'best' route, it will throw an :dn:cls:`~Microsoft.AspNetCore.Mvc.Internal.AmbiguousActionException`.

如果多个路由都匹配，并且 MVC 不能找到‘最好的’路由，将会抛出一个 :dn:cls:`~Microsoft.AspNetCore.Mvc.Internal.AmbiguousActionException` 异常。

.. Route Names
.. ^^^^^^^^^^^

.. _routing-route-name-ref-label:

路由名称
^^^^^^^^^^^

.. The strings  ``"blog"`` and ``"default"`` in the following examples are route names::

在下面例子中的 ``"blog"`` 和 ``"default"`` 字符串是路由名称::

  app.UseMvc(routes =>
  {
     routes.MapRoute("blog", "blog/{*article}",
                 defaults: new { controller = "Blog", action = "Article" });
     routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
  });

.. The route names give the route a logical name so that the named route can be used for URL generation. This greatly simplifies URL creation when the ordering of routes could make URL generation complicated. Routes names must be unique application-wide.

路由名称给予路由一个逻辑名称，以便被命名的路由可以用于 URL 的生成。这在路由命令可能使 URL 的生成变得复杂时，大大简化了 URL 的创建。路由名称在应用程序内必须唯一。

.. Route names have no impact on URL matching or handling of requests; they are used only for URL generation. :doc:`Routing </fundamentals/routing>` has more detailed information on URL generation including URL generation in MVC-specific helpers.

路由名称对 URL 匹配或者处理请求没有任何影响；它们只用于 URL 的生成。:doc:`路由 </fundamentals/routing>` 有更多关于 URL 的生成的信息，包括在具体的 MVC 帮助器中生成 URL。

.. Attribute Routing
.. -------------------------

.. _attribute-routing-ref-label:

特性路由
-------------------------

.. Attribute routing uses a set of attributes to map actions directly to route templates. In the following example, ``app.UseMvc();`` is used in the ``Configure`` method and no route is passed. The ``HomeController`` will match a set of URLs similar to what the default route ``{controller=Home}/{action=Index}/{id?}`` would match:

特性路由使用一组特性来直接将操作映射到路由模板。在下面的例子中，在 ``Configure`` 中使用 ``app.UseMvc();`` 且没有传入路由。``HomeController`` 会匹配一组类似于 ``{controller=Home}/{action=Index}/{id?}`` 的默认路由 URL：

.. code-block:: c#

  public class HomeController : Controller
  {
     [Route("")]
     [Route("Home")]
     [Route("Home/Index")]
     public IActionResult Index()
     {
        return View();
     }
     [Route("Home/About")]
     public IActionResult About()
     {
        return View();
     }
     [Route("Home/Contact")]
     public IActionResult Contact()
     {
        return View();
     }
  }

.. The ``HomeController.Index()`` action will be executed for any of the URL paths ``/``, ``/Home``, or ``/Home/Index``.

``HomeController.Index()`` 操作会被 ``/``、``/Home`` 或者 ``/Home/Index`` 中任一 URL 路径执行。

.. .. note:: This example highlights a key programming difference between attribute routing and conventional routing. Attribute routing requires more input to specify a route; the conventional default route handles routes more succinctly. However, attribute routing allows (and requires) precise control of which route templates apply to each action.

.. note:: 这个例子突出了特性路由于常规路由一个关键的不同之处。特性路由需要更多的输入来指定一个路由；常规路由处理路由更加的简洁。然而，特性路由允许（也必须）精确控制每个操作的路由模板。

.. With attribute routing the controller name and action names play **no** role in which action is selected. This example will match the same URLs as the previous example.

控制器名和操作名在特性路由中是 **不会** 影响选择哪个操作的。这个例子会匹配与上个例子相同的 URL。

.. code-block:: c#

  public class MyDemoController : Controller
  {
     [Route("")]
     [Route("Home")]
     [Route("Home/Index")]
     public IActionResult MyIndex()
     {
        return View("Index");
     }
     [Route("Home/About")]
     public IActionResult MyAbout()
     {
        return View("About");
     }
     [Route("Home/Contact")]
     public IActionResult MyContact()
     {
        return View("Contact");
     }
  }

.. .. note:: The route templates  above doesn't define route parameters for ```action``, ``area``, and ``controller``. In fact, these route parameters are not allowed in attribute routes. Since the route template is already assocated with an action, it wouldn't make sense to parse the action name from the URL.

.. note:: 上面的路由模板没有定义针对 ``action``、``area`` 以及 ``controller`` 的路由参数。实际上，这些参数不允许出现在特性路由中。因为路由模板已经关联了一个操作，解析 URL 中的操作名是没有意义的。

.. Attribute routing can also make use of the ``HTTP[Verb]`` attributes such as :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpPostAttribute`. All of these attributes can accept a route template. This example shows two actions that match the same route template:

特性路由也可以使用 ``HTTP[Verb]`` 特性，比如 :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpPostAttribute`。所有这些特性都可以接受路由模板。这个例子展示两个操作匹配同一个路由模板：

.. code-block:: c#

   [HttpGet("/products")]
   public IActionResult ListProducts()
   {
      // ...
   }

   [HttpPost("/products")]
   public IActionResult CreateProduct(...)
   {
      // ...
   }

.. For a URL path like ``/products`` the ``ProductsApi.ListProducts`` action will be executed when the HTTP verb is ``GET`` and ``ProductsApi.CreateProduct`` will be executed when the HTTP verb is ``POST``. Attribute routing first matches the URL against the set of route templates defined by route attributes. Once a route template matches,   :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` constraints are applied to determine which actions can be executed.

对于 ``/products`` 这个 URL 路径来说，``ProductsApi.ListProducts`` 操作会在 HTTP 谓词是 ``GET`` 时执行，``ProductsApi.CreateProduct`` 会在 HTTP 谓词是 ``POST`` 时执行。特性路由首先匹配路由模板集合中通过路由特性定义的 URL。一旦路由模板匹配，:dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` 约束会应用与决定执行哪个操作。

.. .. Tip:: When building a REST API, it's rare that you will want to use ``[Route(...)]`` on an action method. It's better to use the more specific ``Http*Verb*Attributes`` to be precise about what your API supports. Clients of REST APIs are expected to know what paths and HTTP verbs map to specific logical operations.

.. Tip:: 当构建一个 REST API，你几乎不会想在操作方法上使用 ``[Route(...)]``。最好是使用更加具体的 ``Http*Verb*Attributes`` 来精确的说明你的 API 支持什么。REST API 的客户端期望知道映射到具体逻辑操作上的路径和 HTTP 谓词。

.. Since an attribute route applies to a specific action, it's easy to make parameters required as part of the route template definition. In this example, ``id`` is required as part of the URL path.

由于一个特性路由应用于一个特定操作，很容易使参数作为路由模板定义中必须的一部分。在这个例子中，``id`` 是必须作为 URL 路径中一部分的。

.. code-block:: c#

   public class ProductsApiController : Controller
   {
      [HttpGet("/products/{id}", Name = "Products_List")]
      public IActionResult GetProduct(int id) { ... }
   }

.. The ``ProductsApi.GetProducts(int)`` action will be executed for a URL path like ``/products/3`` but not for a URL path like ``/products``. See :doc:`Routing </fundamentals/routing>` for a full description of route templates and related options.

``ProductsApi.GetProducts(int)`` 操作会被 URL 路径 ``/products/3`` 执行，但不会被 URL 路径 ``/products`` 执行。查看 :doc:`路由 </fundamentals/routing>` 获取路由模板以及相关选项的完整描述。

.. This route attribute also defines a *route name* of ``Products_List``. Route names can be used to generate a URL based on a specific route. Route names have no impact on the URL matching behavior of routing and are only used for URL generation. Route names must be unique application-wide.

这个路由特性同时也定义一个 ``Products_List`` 的 *路由名称*。路由名称可以用来生成基于特定路由的 URL。路由名称对路由的 URL 匹配行为没有影响，只用于 URL 的生成。路由名称必须在应用程序内唯一。

.. .. note:: Contrast this with the conventional *default route*, which defines the ``id`` parameter as optional (``{id?}``). This ability to precisely specify APIs has advantages, such as  allowing ``/products`` and ``/products/5`` to be dispatched to different actions.

.. note:: 常规的 *默认路由* 定义 ``id`` 参数作为可选项 (``{id?}``)。而特性路由的这种精确指定 API 的能力更有优势，比如把 ``/products`` 和 ``/products/5`` 分配到不同的操作。

.. Combining Routes
.. ^^^^^^^^^^^^^^^^^

.. _routing-combining-ref-label:

联合路由
^^^^^^^^^^^^^^^^^

.. To make attribute routing less repetitive, route attributes on the controller are combined with route attributes on the individual actions. Any route templates defined on the controller are prepended to route templates on the actions. Placing a route attribute on the controller makes **all** actions in the controller use attribute routing.

为了减少特性路由的重复部分， 控制器上的路由特性会和各个操作上的路由特性进行结合。任何定义在控制器上的路由模板都会作为操作路由模板的前缀。在控制器上放置一个路由特性会使 **所有** 这个控制器中的操作使用这个特性路由。

.. code-block:: c#

   [Route("products")]
   public class ProductsApiController : Controller
   {
      [HttpGet]
      public IActionResult ListProducts() { ... }

      [HttpGet("{id}")]
      public ActionResult GetProduct(int id) { ... }
   }

.. In this example the URL path ``/products`` can match ``ProductsApi.ListProducts``, and the URL path ``/products/5`` can match ``ProductsApi.GetProduct(int)``. Both of these actions only match HTTP ``GET`` because they are decorated with the :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpGetAttribute`.

在这个例子中，URL 路径 ``/products`` 会匹配 ``ProductsApi.ListProducts``，URL 路径 ``/products/5`` 会匹配 ``ProductsApi.GetProduct(int)``。两个操作都只会匹配 ``GET``，因为它们使用 :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpGetAttribute` 进行装饰。

.. Route templates applied to an action that begin with a ``/`` do not get combined with route templates applied to the controller. This example matches a set of URL paths similar to the *default route*.

应用到操作上的路由模板以 ``/`` 开头不会联合控制器上的路由模板。这个例子匹配一组类似 *默认路由* 的 URL 路径。

.. literalinclude:: routing/sample/main/Controllers/HomeController.cs
  :language: c#
  :start-after: snippet
  :end-before: #endregion

.. Ordering attribute routes
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-ordering-ref-label:

特性路由的顺序
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. In contrast to conventional routes which execute in a defined order, attribute routing builds a tree and matches all routes simultaneously. This behaves as-if the route entries were placed in an ideal ordering; the most specific routes have a chance to execute before the more general routes.

与常规路由的根据定义顺序来执行相比，特性路由构建一个树形结构同时匹配所有路由。这种行为看起来像路由条目被放置在一个理想的顺序中；最具体的路由会在一般的路由之前执行。

.. For example, a route like ``blog/search/{topic}`` is more specific than a route like ``blog/{*article}``. Logically speaking the ``blog/search/{topic}`` route 'runs' first, by default, because that's the only sensible ordering. Using conventional routing, the developer is  responsible for placing routes in the desired order.

比如，路由 ``blog/search/{topic}`` 比 ``blog/{*article}`` 更加具体。从逻辑上讲，``blog/search/{topic}`` 路由先‘运行’，因为在默认情况下这是唯一明智的排序。使用常规路由，开发者负责按所需的顺序放置路由。

.. Attribute routes can configure an order, using the ``Order`` property of all of the framework provided route attributes. Routes are processed according to an ascending sort of the ``Order`` property. The default order is ``0``. Setting a route using ``Order = -1`` will run before routes that don't set an order. Setting a route using ``Order = 1`` will run after default route ordering.

特性路由可以配置顺序，通过使用所有提供路由特性的框架中的 ``Order`` 属性。路由根据 ``Order`` 属性升序处理。默认的 ``Order`` 是 ``0``。使用 ``Order = -1`` 设置一个路由，这个路由会在没有设置 ``Order`` 的路由之前运行。使用 ``Order = 1`` 会在默认路由排序之后运行。

.. .. Tip:: Avoid depending on ``Order``. If your URL-space requires explicit order values to route correctly, then it's likely confusing to clients as well. In general attribute routing will select the correct route with URL matching. If the default order used for URL generation isn't working, using route name as an override is usually simpler than applying the ``Order`` property.

.. Tip:: 避免依赖于 ``Order``。如果你的 URL 空间需要明确的顺序值来使路由正确，那么它可能使客户端混乱。一般的特性路由会通过 URL 匹配选择正确的路由。如果 URL 的生成的默认顺序不生效，使用路由名作为重载通常比应用 ``Order`` 属性更简单。

.. Token replacement in route templates ([controller], [action], [area])
.. -----------------------------------------------------------------------

.. _routing-token-replacement-templates-ref-label:

路由模板中的标记替换（[controller]，[action]，[area]）
-----------------------------------------------------------------------

.. For convenience, attribute routes support *token replacement* by enclosing a token in square-braces (``[``, ``]``]). The tokens ``[action]``, ``[area]``, and ``[controller]`` will be replaced with the values of the action name, area name, and controller name from the action where the route is defined. In this example the actions can match URL paths as described in the comments:

为了方便，特性路由支持 *标记替换* ，通过在方括号中封闭一个标记 (``[``, ``]``])。标记 ``[action]``、 ``[area]`` 以及 ``[controller]`` 会被替换成路由中定义的操作所对应的操作名、区域名、控制器名。在这个例子中，操作可以匹配注释中描述的 URL 路径。

.. literalinclude:: routing/sample/main/Controllers/ProductsController.cs
  :language: c#
  :lines: 7-11,13-17,20-22
  :dedent: 4

.. Token replacement occurs as the last step of building the attribute routes. The above example will behave the same as the following code:

标记替换发生在构建特性路由的最后一步。上面的例子将与下面的代码相同：

.. literalinclude:: routing/sample/main/Controllers/ProductsController2.cs
  :language: c#
  :lines: 7-11,13-17,20-22
  :dedent: 4

.. Attribute routes can also be combined with inheritance. This is particularly powerful combined with token replacement.

特性路由也可以与继承相结合。下面与标记替换的集合非常强大。

.. code-block:: c#

   [Route("api/[controller]")]
   public abstract class MyBaseController : Controller { ... }

   public class ProductsController : MyBaseController
   {
      [HttpGet] // Matches '/api/Products'
      public IActionResult List() { ... }

      [HttpPost("{id}")] // Matches '/api/Products/{id}'
      public IActionResult Edit(int id) { ... }
   }

.. Token replacement also applies to route names defined by attribute routes. ``[Route("[controller]/[action]", Name="[controller]_[action]")]`` will generate a unique route name for each action.

标记替换也可以应用于在特性路由中定义路由名称。``[Route("[controller]/[action]", Name="[controller]_[action]")]`` 将为每一个操作生成一个唯一的路由名称。

.. Multiple Routes
.. ^^^^^^^^^^^^^^^^

.. _routing-multiple-routes-ref-label:

多路由
^^^^^^^^^^^^^^^^

.. Attribute routing supports defining multiple routes that reach the same action. The most common usage of this is to mimic the behavior of the *default conventional route* as shown in the following example:

特性路由支持定义多个路由指向同一个操作。最常见的使用是像下面展示一样模仿 *默认常规路由* ：

.. code-block:: c#

   [Route("[controller]")]
   public class ProductsController : Controller
   {
      [Route("")]     // Matches 'Products'
      [Route("Index")] // Matches 'Products/Index'
      public IActionResult Index()
   }

.. Putting multiple route attributes on the controller means that each one will combine with each of the route attributes on the action methods.

放置多个路由特性到控制器上意味着每一个特性都会与每一个操作方法上的路由特性进行结合。

.. code-block:: c#

   [Route("Store")]
   [Route("[controller]")]
   public class ProductsController : Controller
   {
      [HttpPost("Buy")]     // Matches 'Products/Buy' and 'Store/Buy'
      [HttpPost("Checkout")] // Matches 'Products/Checkout' and 'Store/Checkout'
      public IActionResult Buy()
   }

.. When multiple route attributes (that implement ``IActionConstraint``) are placed on an action, then each action constraint combines with the route template from the attribute that defined it.

当多个路由特性（``IActionConstraint`` 的实现）放置在一个操作上，每一个操作约束都会与特性定义的路由模板相结合。

.. code-block:: c#

   [Route("api/[controller]")]
   public class ProductsController : Controller
   {
      [HttpPut("Buy")]      // Matches PUT 'api/Products/Buy'
      [HttpPost("Checkout")] // Matches POST 'api/Products/Checkout'
      public IActionResult Buy()
   }

.. .. Tip:: While using multiple routes on actions can seem powerful, it's better to keep your application's URL space simple and well-defined. Use multiple routes on actions only where needed, for example to support existing clients.

.. Tip:: 虽然使用多个路由到操作上看起来很强大，但最好还是保持应用程序的 URL 空间简单和定义明确。使用多个路由到操作上仅仅在需要的时候，比如支持已经存在的客户端。

.. Custom route attributes using :dn:iface:`~Microsoft.AspNetCore.Mvc.Routing.IRouteTemplateProvider`
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-cust-rt-attr-irt-ref-label:

使用 :dn:iface:`~Microsoft.AspNetCore.Mvc.Routing.IRouteTemplateProvider` 自定义路由特性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. All of the route attributes provided in the framework ( ``[Route(...)]``, ``[HttpGet(...)]`` , etc.) implement the :dn:iface:`~Microsoft.AspNetCore.Mvc.Routing.IRouteTemplateProvider`
interface. MVC looks for attributes on controller classes and action methods when the app starts and uses the ones that implement ``IRouteTemplateProvider`` to build the initial set of routes.

所有框架提供的路由特性（``[Route(...)]``， ``[HttpGet(...)]`` 等等。）都实现了 :dn:iface:`~Microsoft.AspNetCore.Mvc.Routing.IRouteTemplateProvider` 接口。当应用程序启动时，MVC 查找控制器类和操作方法上实现了``IRouteTemplateProvider`` 接口的特性来构建初始路由集合。

.. You can implement ``IRouteTemplateProvider`` to define your own route attributes. Each ``IRouteTemplateProvider`` allows you to define a single route with a custom route template, order, and name:

你可以通过实现 ``IRouteTemplateProvider`` 来定义你自己的路由特性。每个 ``IRouteTemplateProvider`` 允许你定义一个包含自定义路由模板，顺序以及名称的单路由：

.. code-block:: c#

  public class MyApiControllerAttribute : Attribute, IRouteTemplateProvider
  {
     public string Template => "api/[controller]";

     public int? Order { get; set; }

     public string Name { get; set; }
  }

.. The attribute from the above example automatically sets the ``Template`` to ``"api/[controller]"`` when ``[MyApiController]`` is applied.

上面例子中，当 ``[MyApiController]``特性被应用，会自动设置 ``Template`` 为 ``"api/[controller]"``。

.. Using Application Model to customize attribute routes
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-app-model-ref-label:

使用应用程序模型来自定义特性路由
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. The *application model* is an object model created at startup with all of the metadata used by MVC to route and execute your actions. The *application model* includes all of the data gathered from route attributes (through ``IRouteTemplateProvider``). You can write *conventions* to modify the application model at startup time to customize how routing behaves. This section shows a simple example of customizing routing using application model.

*应用程序模型* 是一个在启动时创建的对象模型，它包含了所有 MVC 用来路由和执行操作的元数据。*应用程序模型* 包含从路由特性中收集的所有数据（通过 ``IRouteTemplateProvider``）。你可以在启动时编写 *约定* 修改应用程序模型来自定义路由的行为。这个章节展示了一个使用应用程序模型自定义路由的例子。

.. literalinclude:: routing/sample/main/NamespaceRoutingConvention.cs
  :language: c#

.. Mixed Routing
.. --------------

.. _routing-mixed-ref-label:

混合路由
--------------

.. MVC applications can mix the use of conventional routing and attribute routing. It's typical to use conventional routes for controllers serving HTML pages for browsers, and attribute routing for controllers serving REST APIs.

MVC 应用程序可以混合使用常规路由和特性路由。对于给浏览器处理页面的控制器，通常使用常规路由；对于提供 REST API 的控制器，通常使用特性路由。

.. Actions are either conventionally routed or attribute routed. Placing a route on the controller or the action makes it attribute routed. Actions that define attribute routes cannot be reached through the conventional routes and vice-versa. **Any** route attribute on the controller makes all actions in the controller attribute routed.

操作在常规路由或者特性路由中二选一。放置一个路由到控制器上或者操作上使操作变为特性路由。定义为特性路由的操作不能通过常规路由访问，反之亦然。放置在控制器上的 **任何** 路由特性都会使控制器中的所有操作变为特性路由。

.. .. Note:: What distinguishes the two types of routing systems is the process applied after a URL matches a route template. In conventional routing, the route values from the match are used to choose the action and controller from a lookup table of all conventional routed actions. In attribute routing, each template is already associated with an action, and no further lookup is needed.

.. Note:: 这两种路由系统的区别是通过 URL 匹配路由模板的过程。在常规路由中，匹配中的路由值被用来在所有常规路由操作的查找表中选择操作以及控制器。在特性路有中，每个模板已经关联了一个操作，进一步查找是没必要的。

.. URL Generation
.. ---------------

.. _routing-url-gen-ref-label:

URL 的生成
---------------

.. MVC applications can use routing's URL generation features to generate URL links to actions. Generating URLs eliminates hardcoding URLs, making your code more robust and maintainable. This section focuses on the URL generation features provided by MVC and will only cover basics of how URL generation works. See :doc:`Routing </fundamentals/routing>` for a detailed description of URL generation.

MVC 应用程序可以使用路由 URL 的生成特性来生成 URL 链接到操作。生成 URL 消除硬编码 URL，使你的代码健壮和易维护。这个章节关注于 MVC 提供的 URL 的生成特性，并只覆盖如何生成 URL 的基本知识。查看 :doc:`Routing </fundamentals/routing>` 获取 URL 的生成的详细描述。

.. The :dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` interface is the underlying piece of infrastructure between MVC and routing for URL generation. You'll find an instance of ``IUrlHelper`` available through the ``Url`` property in controllers, views, and view components.

:dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` 接口是 MVC 与生成 URL 的路由之间基础设施的基本块。你可以通过控制器、视图以及视图组件中的 ``Url`` 属性找到一个可用的 ``IUrlHelper`` 实例。

.. In this example, the ``IUrlHelper`` interface is used through the ``Controller.Url`` property to generate a URL to another action.

在这个例子中，``IUrlHelper`` 接口用于 ``Controller.Url`` 属性来生成一个到其他操作的 URL 。

.. literalinclude:: routing/sample/main/Controllers/UrlGenerationController.cs
  :language: none
  :start-after: snippet_1
  :end-before: #endregion

.. If the application is using the default conventional route, the value of the ``url`` variable will be the URL path string ``/UrlGeneration/Destination``. This URL path is created by routing by combining the route values from the current request (ambient values), with the values passed to ``Url.Action`` and substituting those values into the route template::

如果应用程序使用默认的常规路由，``url`` 变量的值会是 URL 路径字符串 ``/UrlGeneration/Destination``。这个 URL 路径是由将路由值与当前请求（环境值）相结合而成的路由创建，并将值传递给 ``Url.Action`` 并替换这些值到路由模板::

   ambient values: { controller = "UrlGeneration", action = "Source" }
   values passed to Url.Action: { controller = "UrlGeneration", action = "Destination" }
   route template: {controller}/{action}/{id?}

   result: /UrlGeneration/Destination

.. Each route parameter in the route template has its value substituted by matching names with the values and ambient values. A route parameter that does not have a value can use a default value if it has one, or be skipped if it is optional (as in the case of ``id`` in this example). URL generation will fail if any required route parameter doesn't have a corresponding value. If URL generation fails for a route, the next route is tried until all routes have been tried or a match is found.

路由模板中每一个路由参数的值都被匹配名字的值和环境值替换。一个路由参数如果没有值可以使用默认值，或者该参数是可选的则跳过（就像这个例子中 ``id`` 的情况）。任何必须的路由参数没有相应的值会导致 URL 的生成失败。如果一个路由中 URL的生成失败，会尝试下一个路由，直到所有路由都尝试完成或者找到匹配的路由。

.. The example of ``Url.Action`` above assumes conventional routing, but URL generation works similarly with attribute routing, though the concepts are different. With conventional routing, the route values are used to expand a template, and the route values for ``controller`` and ``action`` usually appear in that template - this works because the URLs matched by routing adhere to a *convention*. In attribute routing, the route values for ``controller`` and ``action`` are not allowed to appear in the template - they are instead used to look up which template to use.

上面 ``Url.Action`` 的例子假设是传统路由，但是 URL 的生成工作与特性路由类似，尽管概念是不同的。在路由值常规路由中，路由值被用来扩大一个模板，并且关于 ``controller`` 和 ``action`` 的路由值通常出现在那个模板中 —— 这生效了，因为路由匹配的URL 坚持了一个 *约定*。在特性路由中，关于 ``controller`` 和 ``action`` 的路由值不被允许出现在模板中 —— 它们用来查找该使用哪个模板。

.. This example uses attribute routing:

这个例子使用特性路由：

.. literalinclude:: routing/sample/main/StartupUseMvc.cs
  :language: c#
  :start-after: snippet_1
  :end-before: #endregion

.. literalinclude:: routing/sample/main/Controllers/UrlGenerationControllerAttr.cs
  :language: none
  :start-after: snippet_1
  :end-before: #endregion

.. MVC builds a lookup table of all attribute routed actions and will match the ``controller`` and ``action`` values to select the route template to use for URL generation. In the sample above,   ``custom/url/to/destination`` is generated.

MVC 构建了一个所有特性路由操作的查找表并且会匹配 ``controller`` 和 ``action`` 值选择路由模板用于 URL 的生成。在上面的例子中，``custom/url/to/destination`` 被生成了。

.. Generating URLs by action name
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

通过操作名生成 URL
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. ``Url.Action`` (:dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` . :dn:method:`~Microsoft.AspNetCore.Mvc.IUrlHelper.Action`) and all related overloads all are based on that idea that you want to specify what you're linking to by specifying a controller name and action name.

``Url.Action`` （:dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` 、 :dn:method:`~Microsoft.AspNetCore.Mvc.IUrlHelper.Action`）以及所有相关的重载都是基于通过指定控制器名和操作名来指定想要链接到的地方的点子。

.. .. note:: When using ``Url.Action``, the current route values for ``controller`` and ``action`` are specified for you - the value of ``controller`` and ``action`` are part of both *ambient values* **and** *values*. The method ``Url.Action``, always uses the current values of ``action`` and ``controller`` and will generate a URL path that routes to the current action.

.. note:: 当使用 ``Url.Action``，``controller`` 和 ``action`` 的当前路由值是为你指定的 —— ``controller`` 和 ``action`` 的值同时是 *环境值* **和** *值* 的一部分。``Url.Action`` 方法总是使用 ``controller`` 和 ``action`` 的当前值并且生成路由到当前操作的 URL 路径。

.. Routing attempts to use the values in ambient values to fill in information that you didn't provide when generating a URL. Using a route like ``{a}/{b}/{c}/{d}`` and ambient values ``{ a = Alice, b = Bob, c = Carol, d = David }``, routing has enough information to generate a URL without any additional values - since all route parameters have a value. If you added the value ``{ d = Donovan }``, the value ``{ d = David }`` would be ignored, and the generated URL path would be ``Alice/Bob/Carol/Donovan``.

路由尝试使用环境值中的值来填充信息，以至于在生成 URL 时你不需要提供信息。使用路由如 ``{a}/{b}/{c}/{d}`` 并且环境值 ``{ a = Alice, b = Bob, c = Carol, d = David }``，路由拥有足够的信息生成路由而不需要任何额外的值 —— 因为所有的路由参数都有值。如果你添加值 ``{ d = Donovan }``，那么值 ``{ d = David }`` 会被忽略，并且生成的 URL 路径会是 ``Alice/Bob/Carol/Donovan``。

.. .. warning:: URL paths are hierarchical. In the example above, if you added the value ``{ c = Cheryl }``, both of the values ``{ c = Carol, d = David }`` would be ignored. In this case we no longer have a value for ``d`` and URL generation will fail. You would need to specify the desired value of ``c`` and ``d``.  You might expect to hit this problem with the default route (``{controller}/{action}/{id?}``) - but you will rarely encounter this behavior in practice as ``Url.Action`` will always explicitly specify a ``controller`` and ``action`` value.

.. warning:: URL 路径是分层次的。在上面的例子中，如果你添加值 ``{ c = Cheryl }``，所有的值 ``{ c = Carol, d = David }`` 会被忽略。在这种情况下，我们不再有 ``d`` 的值，且 URL 生成会失败。你需要指定 ``c`` 和 ``d`` 所需的值。你可能期望用默认路由 (``{controller}/{action}/{id?}``) 来解决这个问题 —— 但是你很少会在实践中遇到这个问题，``Url.Action`` 总会明确地指定 ``controller`` 和 ``action`` 的值。

.. Longer overloads of ``Url.Action`` also take an additional *route values* object to provide values for route parameters other than ``controller`` and ``action``. You will most commonly see this used with ``id`` like ``Url.Action("Buy", "Products", new { id = 17 })``. By convention the *route values* object is usually an object of anonymous type, but it can also be an ``IDictionary<>`` or a *plain old .NET object*. Any additional route values that don't match route parameters are put in the query string.

``Url.Action`` 较长的重载也采取额外的 *路由值* 对象来提供除了 ``controller`` 和 ``action`` 意外的路由参数。你最长看到的是使用 ``id``，比如 ``Url.Action("Buy", "Products", new { id = 17 })``。按照惯例，*路由值* 通常是一个匿名类的对象，但是它也可以是一个 ``IDictionary<>`` 或者一个 *普通的 .NET 对象*。任何额外的路由值不会匹配放置在查询字符串中的路由参数。

.. literalinclude:: routing/sample/main/Controllers/TestController.cs
  :language: c#

.. .. tip:: To create an absolute URL, use an overload that accepts a ``protocol``: ``Url.Action("Buy", "Products", new { id = 17 }, protocol: Request.Scheme)``

.. tip:: 为了创建一个绝对 URL，使用一个接受 ``protocol`` 的重载： ``Url.Action("Buy", "Products", new { id = 17 }, protocol: Request.Scheme)``

.. Generating URLs by route
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-gen-urls-route-ref-label:

通过路由生成 URL
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. The code above demonstrated generating a URL by passing in the controller and action name. ``IUrlHelper`` also provides the ``Url.RouteUrl`` family of methods. These methods are similar to ``Url.Action``, but they do not copy the current values of ``action`` and ``controller`` to the route values. The most common usage is to specify a route name to use a specific route to generate the URL, generally *without* specifying a controller or action name.

上面的代码展示了通过传递控制器名和操作名创建 URL。``IUrlHelper`` 也提供 ``Url.RouteUrl`` 的系列方法。这些方法类似 ``Url.Action``，但是它们不复制 ``action`` 和 ``controller`` 的当前值到路由值。最常见的是指定一个路由名来使用具体的路由生成 URL，通常 *没有* 指定控制器名或者操作名。

.. literalinclude:: routing/sample/main/Controllers/UrlGenerationControllerRouting.cs
  :language: none
  :start-after: snippet_1
  :end-before: #endregion

.. Generating URLs in HTML
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-gen-urls-html-ref-label:

在 HTML 中生成URL
^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. :dn:iface:`~Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper` provides the :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` methods ``Html.BeginForm`` and ``Html.ActionLink`` to generate ``<form>`` and ``<a>`` elements respectively. These methods use the ``Url.Action`` method to generate a URL and they accept similar arguments. The ``Url.RouteUrl`` companions for :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` are ``Html.BeginRouteForm`` and ``Html.RouteLink`` which have similar functionality. See :doc:`/mvc/views/html-helpers` for more details.

:dn:iface:`~Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper` 提供 :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` 方法 ``Html.BeginForm`` 和 ``Html.ActionLink`` 来分别生成 ``<form>`` 和 ``<a>`` 元素。这些方法使用 ``Url.Action`` 方法来生成一个 URL 并且它们接受类似的参数。``Url.RouteUrl`` 相对于 :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper` 的是 ``Html.BeginRouteForm`` 和 ``Html.RouteLink``，它们有着类似的功能。查看 :doc:`/mvc/views/html-helpers` 获取更多信息。

.. TagHelpers generate URLs through the ``form`` TagHelper and the ``<a>`` TagHelper. Both of these use ``IUrlHelper`` for their implementation. See :doc:`/mvc/views/working-with-forms` for more information.

TagHelper 通过 ``form`` 和 ``<a>`` TagHelper 生成 URL。这些 都使用了 ``IUrlHelper`` 为它们的实现。查看 :doc:`/mvc/views/working-with-forms` 获取更多信息。

.. Inside views, the :dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` is available through the ``Url`` property for any ad-hoc URL generation not covered by the above.

内部观点，:dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` 通过 ``Url`` 属性生成任何不包含上述的特定 URL。

.. Generating URLS in Action Results
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-gen-urls-action-ref-label:

在操作结果中生成 URL
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. The examples above have shown using ``IUrlHelper`` in a controller, while the most common usage in a controller is to generate a URL as part of an action result.

上面的例子展示了在控制器中使用 ``IUrlHelper``，而在控制器中最常见的用法是生成一个 URL 作为操作结果的一部分。

.. The ``ControllerBase`` and ``Controller`` base classes provide convenience methods for action results that reference another action. One typical usage is to redirect after accepting user input.

``ControllerBase`` 和 ``Controller`` 基类针对引用其他操作的操作结果提供了方便的方法。一个典型的使用时接受用户输入后重定向。

.. code-block:: c#

   public Task<IActionResult> Edit(int id, Customer customer)
   {
       if (ModelState.IsValid)
       {
           // Update DB with new details.
           return RedirectToAction("Index");
       }
   }

.. The action results factory methods follow a similar pattern to the methods on :dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper`.

操作结果工厂方法遵循 :dn:iface:`~Microsoft.AspNetCore.Mvc.IUrlHelper` 中类似模式的方法。

.. Special case for dedicated conventional routes
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _routing-dedicated-ref-label:

专用常规路由的特殊情况
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. Conventional routing can use a special kind of route definition called a *dedicated conventional route*. In the example below, the route named ``blog`` is a dedicated conventional route.

常规路由可以使用一种特殊的路由被称作 *专用常规路由*。在下面的例子中，被命名为 ``blog`` 的路由是专用常规路由。

.. code-block:: c#

    app.UseMvc(routes =>
    {
        routes.MapRoute("blog", "blog/{*article}",
            defaults: new { controller = "Blog", action = "Article" });
        routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
    });

.. Using these route definitions, ``Url.Action("Index", "Home")`` will generate the URL path ``/`` with the ``default`` route, but why? You might guess the route values ``{ controller = Home, action = Index }`` would be enough to generate a URL using ``blog``, and the result would be ``/blog?action=Index&controller=Home``.

使用这些路由定义，``Url.Action("Index", "Home")`` 会使用默认路由生成 URL 路径 ``/``，但是为什么呢？你可能会猜路由值 ``{ controller = Home, action = Index }`` 会足以用 ``blog`` 路由来生成 URL，并且结果会是 ``/blog?action=Index&controller=Home``。

.. Dedicated conventional routes rely on a special behavior of default values that don't have a corresponding route parameter that prevents the route from being "too greedy" with URL generation. In this case the default values are ``{ controller = Blog, action = Article }``, and neither ``controller`` nor ``action`` appears as a route parameter. When routing performs URL generation, the values provided must match the default values. URL generation using ``blog`` will fail because the values ``{ controller = Home, action = Index }`` don't match ``{ controller = Blog, action = Article }``. Routing then falls back to try ``default``, which succeeds.

专用常规路由依靠默认路由的一个特殊行为，没有相应的路由参数，以防止路由生成 URL “太贪婪”。在这种情况下默认的值是 ``{ controller = Blog, action = Article }``，而不是出现在路由参数中的 ``controller`` 或者 ``action``。当路由执行 URL 的生成，提供的值必须匹配默认路由。URL 的生成使用 ``blog`` 将失败，因为值 ``{ controller = Home, action = Index }`` 不匹配 ``{ controller = Blog, action = Article }``。然后路由回退到尝试 ``default``，并成功。

.. Areas
.. ---------

.. _routing-areas-ref-label:

区域
---------

.. :doc:`/mvc/controllers/areas` are an MVC feature used to organize related functionality into a group as a separate routing-namespace (for controller actions) and folder structure (for views). Using areas allows an application to have multiple controllers with the same name - as long as they have different *areas*. Using areas creates a hierarchy for the purpose of routing by adding another route parameter, ``area`` to ``controller`` and ``action``. This section will discuss how routing interacts with areas - see :doc:`/mvc/controllers/areas` for details about how areas are used with views.

:doc:`/mvc/controllers/areas` 是一个 MVC 特点，用来组织相关的功能到一个单独的路由命名空间（针对控制器操作）的组和单独的文件夹结构中（针对视图）。使用区域允许一个应用程序拥有多个同名的路由器 —— 只要它们有不同的 *区域*。使用区域达到通过添加另一个路由参数分层的目的，``area`` 到 ``controller`` 以及 ``action``。这个章节将讨论如何路由作用于区域 —— 查看 :doc:`/mvc/controllers/areas` 获取区域如何与视图配合使用的详细信息。

The following example configures MVC to use the default conventional route and an *area route* for an area named ``Blog``:

下面的例子使用默认常规路由配置 MVC，以及一个命名为 ``Blog`` 的 *区域路由*：

.. literalinclude:: routing/sample/AreasRouting/Startup.cs
  :language: c#
  :start-after: snippet1
  :end-before: #endregion
  :dedent: 12

.. When matching a URL path like ``/Manage/Users/AddUser``, the first route will produce the route values ``{ area = Blog, controller = Users, action = AddUser }``. The ``area`` route value is produced by a default value for ``area``, in fact the route created by ``MapAreaRoute`` is equivalent to the following:

当匹配 URL 路径如 ``/Manage/Users/AddUser`` 时，第一个路由会产生路由值 ``{ area = Blog, controller = Users, action = AddUser }``。``area`` 路由值是通过 ``area`` 的默认值产生的，实际上通过 ``MapAreaRoute`` 创建路由和下面的方式是相等的：


.. literalinclude:: routing/sample/AreasRouting/Startup.cs
  :language: c#
  :start-after: snippet2
  :end-before: #endregion
  :dedent: 12

.. :dn:method:`~Microsoft.AspNetCore.Builder.MvcAreaRouteBuilderExtensions.MapAreaRoute` creates a route using both a default value and constraint for ``area`` using the provided area name, in this case ``Blog``. The default value ensures that the route always produces ``{ area = Blog, ... }``, the constraint requires the value ``{ area = Blog, ... }`` for URL generation.

:dn:method:`~Microsoft.AspNetCore.Builder.MvcAreaRouteBuilderExtensions.MapAreaRoute` 创建一个路由同时使用默认路由和 ``area`` 约束，约束使用提供的区域名，在这个例子中是 ``Blog``。默认值保证路由总是处理 ``{ area = Blog, ... }``，约束要求值 ``{ area = Blog, ... }`` 来进行 URL 的生成。

.. .. tip:: Conventional routing is order-dependent. In general, routes with areas should be placed earlier in the route table as they are more specific than routes without an area.

.. tip:: 常规路由是顺序依赖。一般来说，区域路由需要被放置在路由表的前面，因为没有比区域路由更具体的路由了。

.. Using the above example, the route values would match the following action:

使用上述例子，路由值将匹配下面操作：

.. literalinclude:: routing/sample/AreasRouting/Areas/Blog/Controllers/UsersController.cs
  :language: c#

.. The :dn:cls:`~Microsoft.AspNetCore.Mvc.AreaAttribute` is what denotes a controller as part of an area, we say that this controller is in the ``Blog`` area. Controllers without an ``[Area]`` attribute are not members of any area, and will **not** match when the ``area`` route value is provided by routing. In the following example, only the first controller listed can match the route values ``{ area = Blog, controller = Users, action = AddUser }``.

:dn:cls:`~Microsoft.AspNetCore.Mvc.AreaAttribute` 表示控制器属于一个区域的一部分，我们说，这个控制器是在 ``Blog`` 区域。控制器不带 ``[Area]`` 特性则不是任何区域的成员，并且当 ``area`` 路由值通过路由提供时 **不会** 匹配。在下面的例子中，只有第一个列出的控制器可以匹配路由值 ``{ area = Blog, controller = Users, action = AddUser }``。

.. literalinclude:: routing/sample/AreasRouting/Areas/Blog/Controllers/UsersController.cs
  :language: c#

.. literalinclude:: routing/sample/AreasRouting/Areas/Zebra/Controllers/UsersController.cs
  :language: c#

.. literalinclude:: routing/sample/AreasRouting/Controllers/UsersController.cs
  :language: c#

.. .. note:: The namespace of each controller is shown here for completeness - otherwise the controllers would have a naming conflict and generate a compiler error. Class namespaces have no effect on MVC's routing.

.. note:: 为了完整性，将每个控制器的命名空间显示到这里 —— 否则控制器将会遇到命名冲突并且声称一个编译错误。类命名空间不影响 MVC 的路由。

.. The first two controllers are members of areas, and only match when their respective area name is provided by the ``area`` route value. The third controller is not a member of any area, and can only match when no value for ``area`` is provided by routing.

前两个控制器是区域的成员，并只匹配通过 ``area`` 路由值提供的各自的区域名。第三个控制器不是任何区域的成员，只会在路由中没有 ``area`` 值时匹配。

.. .. note:: In terms of matching *no value*, the absence of the ``area`` value is the same as if the value for ``area`` were null or the empty string.

.. note:: 在匹配 *no value* 方面，缺少 ``area`` 值与 ``area`` 是 null 或者空字符串是一样的。

.. When executing an action inside an area, the route value for ``area`` will be available as an *ambient value* for routing to use for URL generation. This means that by default areas act *sticky* for URL generation as demonstrated by the following sample.

当执行一个区域内的操作时，``area`` 的路由值可作为用于录用生成 URL 的 *环境值*。这意味着默认情况下区域针对 URL 的生成有 *黏性* ，如下面例子所示。

.. literalinclude:: routing/sample/AreasRouting/Startup.cs
  :language: c#
  :start-after: snippet3
  :end-before: #endregion
  :dedent: 12

.. literalinclude:: routing/sample/AreasRouting/Areas/Duck/Controllers/UsersController.cs
  :language: c#

.. Understanding IActionConstraint
.. ---------------------------------

.. _iactionconstraint-ref-label:

理解 IActionConstraint
---------------------------------

.. .. note:: This section is a deep-dive on framework internals and how MVC chooses an action to execute. A typical application won't need a custom ``IActionConstraint``

.. note:: 这一节是框架内部的一个深潜和 MVC 如何选择操作执行。通常一个应用程序不需要自定义 ``IActionConstraint``

.. You have likely already used :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` even if you're not familiar with the interface. The ``[HttpGet]`` Attribute and similar ``[Http-VERB]`` attributes implement ``IActionConstraint`` in order to limit the execution of an action method.

你可能已经使用 :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` 即使你不熟悉这个借口。``[HttpGet]`` 特性以及类似的 ``[Http-VERB]`` 特性实现 ``IActionConstraint`` 接口以用于限制操作方法的执行。

.. code-block:: c#

   public class ProductsController : Controller
   {
       [HttpGet]
       public IActionResult Edit() { }

       public IActionResult Edit(...) { }
   }

.. Assuming the default conventional route, the URL path ``/Products/Edit`` would produce the values ``{ controller = Products, action = Edit }``, which would match **both** of the actions shown here. In ``IActionConstraint`` terminology we would say that both of these actions are considered candidates - as they both match the route data.

假设默认的常规路由，URL 路径 ``/Products/Edit`` 会产生值 ``{ controller = Products, action = Edit }``，将 **同时** 匹配这里显示的两个操作。在 ``IActionConstraint`` 的术语中，我们会说这两个操作同时被视为候选项 —— 因为它们都匹配路由数据。

.. When the :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpGetAttribute` executes, it will say that `Edit()` is a match for `GET` and is not a match for any other HTTP verb. The ``Edit(...)`` action doesn't have any constraints defined, and so will match any HTTP verb. So assuming a ``POST`` - only ``Edit(...)`` matches. But, for a ``GET`` both actions can still match - however, an action with an ``IActionConstraint`` is always considered *better* than an action without. So because ``Edit()`` has ``[HttpGet]`` it is considered more specific, and will be selected if both actions can match.

当 :dn:cls:`~Microsoft.AspNetCore.Mvc.HttpGetAttribute` 执行，它将声明 `Edit()` 匹配 `GET` 并且不匹配其他的 HTTP 谓词。``Edit(...)`` 操作没有定义任何约束，所以会匹配任何 HTTP 谓词。所以假设有一个 ``POST`` 操作 —— 只有 ``Edit(...)`` 会匹配。但是如果是 ``GET`` 两个操作都会匹配 —— 然而，一个操作使用了 ``IActionConstraint`` 总是被认为 *更好* 与没有使用的操作。所以因为 ``Edit()`` 有 ``[HttpGet]`` ，它被视为更加具体，并且在两个操作都可以匹配时被选中。

.. Conceptually, ``IActionConstraint`` is a form of *overloading*, but instead of overloading methods with the same name, it is overloading between actions that match the same URL. Attribute routing also uses ``IActionConstraint`` and can result in actions from different controllers both being considered candidates.

从概念上讲， ``IActionConstraint`` 是 *重载* 的一种形式，但不是使用相同名称的重载方法，它是匹配相同 URL 的操作的重载。特性路由也使用 ``IActionConstraint`` 并且可能导致不同控制器的操作被视为候选。

.. Implementing IActionConstraint
.. ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. _iactionconstraint-impl-ref-label:

实现 IActionConstraint
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. The simplest way to implement an :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` is to create a class derived from ``System.Attribute`` and place it on your actions and controllers. MVC will automatically discover any ``IActionConstraint`` that are applied as attributes. You can use the application model to apply constraints, and this is probably the most flexible approach as it allows you to metaprogram how they are applied.

实现 :dn:iface:`~Microsoft.AspNetCore.Mvc.ActionConstraints.IActionConstraint` 最简单的方式是创建一个类派生自 ``System.Attribute`` 并且将它放置到你的操作和控制器上。MVC 会自动发现任何作为特性被应用的 ``IActionConstraint``。你可以使用应用程序模型来应用约束，并且这可能使最灵活的方法，因为它可以允许你对它们如何被应用进行元编程。

.. In the following example a constraint chooses an action based on a *country code* from the route data. The `full sample on GitHub <https://github.com/aspnet/Entropy/blob/dev/samples/Mvc.ActionConstraintSample.Web/CountrySpecificAttribute.cs>`__.

在下面的例子，一个约束选择一个操作基于一个来自路由数据的 *country code* 。`GitHub 上完整的示例 <https://github.com/aspnet/Entropy/blob/dev/samples/Mvc.ActionConstraintSample.Web/CountrySpecificAttribute.cs>`__.

.. code-block:: c#

   public class CountrySpecificAttribute : Attribute, IActionConstraint
   {
       private readonly string _countryCode;

       public CountrySpecificAttribute(string countryCode)
       {
           _countryCode = countryCode;
       }

       public int Order
       {
           get
           {
               return 0;
           }
       }

       public bool Accept(ActionConstraintContext context)
       {
           return string.Equals(
               context.RouteContext.RouteData.Values["country"].ToString(),
               _countryCode,
               StringComparison.OrdinalIgnoreCase);
       }
   }

.. You are responsible for implementing the ``Accept`` method and choosing an 'Order' for the constraint to execute. In this case, the ``Accept`` method returns ``true`` to denote the action is a match when the ``country`` route value matches. This is different from a ``RouteValueAttribute`` in that it allows fallback to a non-attributed action. The sample shows that if you define an ``en-US`` action then a country code like ``fr-FR`` will fall back to a more generic controller that does not have ``[CountrySpecific(...)]`` applied.

你负责实现 ``Accept`` 方法并选择一个 ‘Order’ 用于约束执行。在这个例子中，``Accept`` 方法返回 ``true`` 表示当 ``country`` 路由值匹配时操作是匹配的。这和 ``RouteValueAttribute`` 不同，因为它允许回退到一个非特性操作。这个例子展示了如果你定义一个 ``en-US`` 操作，然后国家代码是 ``fr-FR`` 会回退到一个更通用的控制器，这个控制器没有应用 ``[CountrySpecific(...)]``。

.. The ``Order`` property decides which *stage* the constraint is part of. Action constraints run in groups based on the ``Order``. For example, all of the framework provided HTTP method attributes use the same ``Order`` value so that they run in the same stage. You can have as many stages as you need to implement your desired policies.

``Order`` 特性决定约束的部分是哪个阶段。操作约束基于 ``Order`` 在组中运行。比如，所有框架提供的 HTTP 方法特性使用相同 ``Order`` 值，所以他们运行在同一阶段。你可以拥有许多阶段，来实现你所需要的策略。

.. .. tip:: To decide on a value for ``Order`` think about whether or not your constraint should be applied before HTTP methods. Lower numbers run first.

.. tip:: 要决定一个 ``Order`` 的值，考虑你的约束是否需要在 HTTP 方法之前被应用。数字越低，运行越早。

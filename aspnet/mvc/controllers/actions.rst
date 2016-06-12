Controllers, Actions, and Action Results
========================================

Controllers, Actions, 和 Action Results
========================================

By `Steve Smith`_

作者： `Steve Smith`_

翻译： `姚阿勇（Dr.Yao） <https://github.com/yaoay>`_

校对：


Actions and action results are a fundamental part of how developers build apps using ASP.NET MVC.

Action 和 action result 是开发者使用 ASP.NET MVC 构建应用程序的基础部分。

.. contents:: 章节：
  :local:
  :depth: 1

What is a Controller
--------------------

In ASP.NET MVC, a `Controller` is used to define and group a set of actions. An `action` (or `action method`) is a method on a controller that handles incoming requests. Controllers provide a logical means of grouping similar actions together, allowing common sets of rules (e.g. routing, caching, authorization) to be applied collectively. Incoming requests are mapped to actions through :doc:`routing <routing>`.

什么是 Controller
-------------------

在 ASP.NET MVC 中， 控制器（ `Controller` ）用于定义和归纳一组操作（`Action` ）。操作（ 或操作方法 ）是控制器中处理入站请求的一个方法。 控制器提供了一种逻辑方式将相似的操作组织起来，允许一些通用的规则（如：路由，缓存和验证）得到共同的应用。 入站请求通过路由（ :doc:`routing <routing>` ）被映射到操作上。

In ASP.NET Core MVC, a controller can be any instantiable class that ends in "Controller" or inherits from a class that ends with "Controller". Controllers should follow the `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle>`_ and request any dependencies their actions require through their constructor using :doc:`dependency injection <dependency-injection>`.

在 ASP.NET Core MVC 中，控制器可以是以 “Controller” 结尾或者继承自以 “Controller” 结尾的类的任何可实例化类。控制器应当遵循 `显式依赖选择 <http://deviq.com/explicit-dependencies-principle>`_ 并且通过使用依赖注入在构造函数中获取他们需要的任何依赖项。

By convention, controller classes:

* Are located in the root-level "Controllers" folder
* Inherit from Microsoft.AspNet.Mvc.Controller

These two conventions are not required.

按照惯例，控制器应当：

* 放在根目录下的“Controllers”文件夹中
* 继承自 Microsoft.AspNet.Mvc.Controller

这两个惯例不是强制要求。

Within the Model-View-Controller pattern, a Controller is responsible for the initial processing of the request and instantiation of the Model. Generally, business decisions should  be performed within the Model.

在模型-视图-控制器模式中，控制器负责初始化请求以及实例化模型。通常来说，业务流程应当放在模型中执行。

.. note:: The Model should be a `Plain Old CLR Object (POCO)`, not a ``DbContext`` or database-related type.

.. note:: 模型应该是一个简单的传统 CLR 对象（ `Plain Old CLR Object (POCO)` ），而不是一个数据库上下文 ``DbContext`` 或者关系数据库类型。

The controller takes the result of the model's processing (if any), returns the proper view along with the associated view data. Learn more: :doc:`/mvc/overview` and :doc:`/tutorials/first-mvc-app/start-mvc`.

控制器取得模型的执行结果（如果有），返回正确的视图以及相关的视图数据。更多请参考： :doc:`/mvc/overview` and :doc:`/tutorials/first-mvc-app/start-mvc` 。

.. tip:: The Controller is a `UI level` abstraction. Its responsibility is to ensure incoming request data is valid and to choose which view (or result for an API) should be returned. In well-factored apps it will not directly include data access or business logic, but instead will delegate to services handling these responsibilities.

.. tip:: 控制器是一个 UI 层的抽象。它的责任在于确保入站请求的数据是有效的，然后选择应当返回哪一个视图（或者 API 的结果）。在有着良好分解的应用程序中，控制器不会直接包含数据访问或业务逻辑，而是委托给服务去处理这些任务。

 
Defining Actions
----------------
Any public method on a controller type is an action. Parameters on actions are bound to request data and validated using :doc:`model binding </mvc/models/model-binding>`.

Action 的定义
--------------

控制器上的任意公共方法都是一个 Action 。Action 上的参数通过模型绑定 :doc:`model binding </mvc/models/model-binding>` 与请求数据绑定并校验。

.. warning:: Action methods that accept parameters should verify the ``ModelState.IsValid`` property is true.

.. warning:: 接受参数的 Action 方法应该检查 ``ModelState.IsValid`` 属性是否为 True 。

Action methods should contain logic for mapping an incoming request to a business concern. Business concerns should typically be represented as services that your controller accesses through :doc:`dependency injection <dependency-injection>`. Actions then map the result of the business action to an application state.

Action 方法应当包含将传入请求映射到业务的逻辑。业务通常应该表现为由控制器通过依赖注入（ :doc:`dependency injection <dependency-injection>` ）访问的服务。

Actions can return anything, but frequently will return an instance of ``IActionResult`` (or ``Task<IActionResult>`` for async methods) that produces a response. The action method is responsible for choosing `what kind of response`; the action result `does the responding`.

Action 可以返回任何东西，但是常常会返回 ``IActionResult`` （或异步方法返回的 ``Task<IActionResult>`` ）实例以生成响应。Action 方法负责选择“什么类型的响应”；Action Result 负责“执行响应”。

Controller Helper Methods
#########################

Although not required, most developers will want to have their controllers inherit from the base ``Controller`` class. Doing so provides controllers with access to many properties and helpful methods, including the following helper methods designed to assist in returning various responses:

控制器辅助方法
###############

虽然不是必须的，但大多数开发者还是想要从 ``Controller`` 基类继承自己的控制器。从而提供了能访问很多属性和有用方法的控制器，包括下面的旨在帮助返回多种响应的辅助方法：


:doc:`View </mvc/views/index>`
  Returns a view that uses a model to render HTML. Example: ``return View(customer);``

视图（ :doc:`View </mvc/views/index>` ）
  返回一个使用模型渲染 HTML 的视图。例： ``return View(customer);``  

HTTP Status Code
  Return an HTTP status code. Example: ``return BadRequest();``

HTTP 状态代码
  返回一个 HTTP 状态代码。例： ``return BadRequest();``

Formatted Response
  Return ``Json`` or similar to format an object in a specific manner. Example: ``return Json(customer);``

格式化的响应
  返回 ``Json`` 或以特定方式格式化对象的类似格式。例： ``return Json(customer);``

Content negotiated response
  Instead of returning an object directly, an action can return a content negotiated response (using ``Ok``, ``Created``, ``CreatedAtRoute`` or ``CreatedAtAction``). Examples: ``return Ok();`` or ``return CreatedAtRoute("routename",values,newobject");``

内容协商的响应
  除了直接返回一个对象，Action 还可以返回一个内容协商的响应（使用  ``Ok``, ``Created``, ``CreatedAtRoute`` 或 ``CreatedAtAction`` ）。例：  ``return Ok();`` 或 ``return CreatedAtRoute("routename",values,newobject");``


Redirect
  Returns a redirect to another action or destination (using ``Redirect``,``LocalRedirect``,``RedirectToAction`` or ``RedirectToRoute``). Example: ``return RedirectToAction("Complete", new {id = 123});``

重定向
  返回一个指向其他 Action 或目标的重定向（使用 ``Redirect``,``LocalRedirect``,``RedirectToAction`` 或 ``RedirectToRoute`` ）. 例： ``return RedirectToAction("Complete", new {id = 123});``


In addition to the methods above, an action can also simply return an object. In this case, the object will be formatted based on the client's request. Learn more about :doc:`/mvc/models/formatting`

除了上面的方法之外，Action 还可以直接返回一个对象。在这种情况下，对象将以客户端请求的方式进行格式化。详情请参考： :doc:`/mvc/models/formatting`

Cross-Cutting Concerns
######################

In most apps, many actions will share parts of their workflow. For instance, most of an app might be available only to authenticated users, or might benefit from caching. When you want to perform some logic before or after an action method runs, you can use a `filter`. You can help keep your actions from growing too large by using :doc:`filters` to handle these cross-cutting concerns. This can help eliminate duplication within your actions, allowing them to follow the `Don't Repeat Yourself (DRY) principle <http://deviq.com/don-t-repeat-yourself/>`_.

横切关注点
###########

在大多数应用中，许多 Action 会共用部分工作流。例如，大多数应用可能只对验证过的用户开放，或者要利用缓存。当你想要在 Action 方法运行之前或之后执行一些逻辑业务时，可以使用过滤器（ `filter` ）。利用过滤器（ :doc:`filters` ）处理一些横切关注点，可以防止你的 Action 变得过于臃肿。这有助于剔除 Action 中的重复代码，使得它们可以遵循“不要重复自己”的原则 `Don't Repeat Yourself (DRY) principle <http://deviq.com/don-t-repeat-yourself/>`_ 。



In the case of authorization and authentication, you can apply the ``Authorize`` attribute to any actions that require it. Adding it to a controller will apply it to all actions within that controller. Adding this attribute will ensure the appropriate filter is applied to any request for this action. Some attributes can be applied at both controller and action levels to provide granular control over filter behavior. Learn more: :doc:`filters` and :doc:`/security/authorization/authorization-filters`.

就验证和授权而言，你可以将 ``Authorize`` 特性应用在任何一个要求授权的 Action 上。将它加在控制器上将会对该控制器里的所有的 Action 应用授权。这个特性的添加将确保每个访问此 Action 的请求都被应用了对应的过滤器。有些特性可以同时应用在控制器和 Action 上，以提供对过滤器行为更小粒度的控制。

Other examples of cross-cutting concerns in MVC apps may include:
  * :ref:`Error handling <exception-filters>`
  * :doc:`/performance/caching/response`

关于 MVC 应用程序中横切关注点的其他例子：
  * :ref:`Error handling <exception-filters>`
  * :doc:`/performance/caching/response`

.. note:: Many cross-cutting concerns can be handled using filters in MVC apps. Another option to keep in mind that is available to any ASP.NET Core app is custom :doc:`middleware </fundamentals/middleware>`.

.. note:: 在 MVC 应用程序里，很多横切关注点都可以利用过滤器来处理。还有另一种对所有 ASP.NET Core 应用程序都有效的选择需要记住，就是自定义中间件  :doc:`middleware </fundamentals/middleware>` 。

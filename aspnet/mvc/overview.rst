:version: 1.0.0


Overview of ASP.NET Core MVC
============================


ASP.NET Core MVC 概览
=======================

By `Steve Smith`_

作者：`Steve Smith`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

ASP.NET Core MVC is a rich framework for building web apps and APIs using the Model-View-Controller design pattern.

ASP.NET Core MVC 是使用模型-视图-控制器（Model-View-Controller）设计模式构建网页应用与 API 的丰富的框架。

.. contents:: Sections:
  :local:
  :depth: 1


What is the MVC pattern?
------------------------


什么是 MVC 模式？
-------------------

The Model-View-Controller (MVC) architectural pattern separates an application into three main groups of components: Models, Views, and Controllers. This pattern helps to achieve `separation of concerns <http://deviq.com/separation-of-concerns/>`_. Using this pattern, user requests are routed to a Controller which is responsible for working with the Model to perform user actions and/or retrieve results of queries. The Controller chooses the View to display to the user, and provides it with any Model data it requires.

模型-视图-控制器（MVC）架构模式将一个应用区分为三部分主要组件：模型、视图、与控制器。这种模式有助实现 `关注分离 <http://deviq.com/separation-of-concerns/>`_。使用这种模式，用户请求被路由到控制器，控制器负责与模型（Model）协作以执行用户操作和/或返回请求结果。控制器（Controller）选择视图（View），展示给用户，而给视图提供其所需要的任何模型（Model）。

The following diagram shows the three main components and which ones reference the others:

下面的图表展示了这三个主要组件以及它们间的相互引用：

.. image:: overview/_static/mvc.png

This delineation of responsibilities helps you scale the application in terms of complexity because it��s easier to code, debug, and test something (model, view, or controller) that has a single job (and follows the `Single Responsibility Principle <http://deviq.com/single-responsibility-principle/>`_). It's more difficult to update, test, and debug code that has dependencies spread across two or more of these three areas. For example, user interface logic tends to change more frequently than business logic. If presentation code and business logic are combined in a single object, you have to modify an object containing business logic every time you change the user interface. This is likely to introduce errors and require the retesting of all business logic after every minimal user interface change.

这个职责示意图帮你掌控你的应用的复杂程度，因为其更容易编码、调试、与测试一些（模型、视图、控制器）有单一功能的模块 （进一步了解 `单一职责原则 <http://deviq.com/single-responsibility-principle/>`_）。存在两者或者此三者之间的广泛依赖是非常难更新、测试、调试代码的。例如，用户界面逻辑与业务逻辑相比倾向于变化更频繁。如果表现代码与业务逻辑混杂在一个对象内，在你每次改变用户接口的时候都需要修改一个包含业务逻辑的对象。这也就更容易引入错误，并使得你在每次做一个很小的用户接口改动后都要进行完整的业务逻辑测试。

.. note:: Both the view and the controller depend on the model. However, the model depends on neither the view nor the controller. This is one the key benefits of the separation. This separation allows the model to be built and tested independent of the visual presentation.

.. note:: 视图与控制器都依赖于模型。尽管如此，模型并不依赖于视图，也不依赖于控制器。这是分离的一大优势。这样分离允许模型被创建并可以依赖于虚拟的表现中测试。

Model Responsibilities
^^^^^^^^^^^^^^^^^^^^^^

模型（Model）职责
^^^^^^^^^^^^^^^^^^^^^^

The Model in an MVC application represents the state of the application and any business logic or operations that should be performed by it. Business logic should be encapsulated in the model, along with any implementation logic for persisting the state of the application. Strongly-typed views will typically use ViewModel types specifically designed to contain the data to display on that view; the controller will create and populate these ViewModel instances from the model.

MVC 应用中的模型代表了应用的状态和业务逻辑或其可以展现的一些操作。业务逻辑应该封装在模型，连同应用持久化状态实现逻辑。强类型视图一般使用特别设计的视图模型（ViewModel）类型，它包含了视图显示需要的数据；控制器将创建并从模型填充这些视图模型。

.. note:: There are many ways to organize the model in an app that uses the MVC architectural pattern. Learn more about some `different kinds of model types <http://deviq.com/kinds-of-models/>`_.

.. note:: 有许多种方法组织 MVC 架构形式的应用中的模型。了解更多关于 `不同类型的模型 <http://deviq.com/kinds-of-models/>`_。

View Responsibilities
^^^^^^^^^^^^^^^^^^^^^

视图（View）职责
^^^^^^^^^^^^^^^^^^^^^

Views are responsible for presenting content through the user interface. They use the `Razor view engine`_ to embed .NET code in HTML markup. There should be minimal logic within views, and any logic in them should relate to presenting content. If you find the need to perform a great deal of logic in view files in order to display data from a complex model, consider using a :doc:`View Component </mvc/views/view-components>`, ViewModel, or view template to simplify the view.

视图负责在用户界面呈现内容。它们使用 `Razor 视图引擎`_ 在 HTML 标记中嵌入 .NET 代码。视图中应仅包含少量的逻辑，而这些逻辑应该是与呈现内容相关的。如果你发现需要在视图文件中完成大量的逻辑任务，以便从复杂的模型展示数据，请考虑使用 :doc:`视图组件 </mvc/views/view-components>` 、视图模型、或视图模板来简化视图。

Controller Responsibilities
^^^^^^^^^^^^^^^^^^^^^^^^^^^

控制器（Controller）职责
^^^^^^^^^^^^^^^^^^^^^^^^^^^

Controllers are the components that handle user interaction, work with the model, and ultimately select a view to render. In an MVC application, the view only displays information; the controller handles and responds to user input and interaction. In the MVC pattern, the controller is the initial entry point, and is responsible for selecting which model types to work with and which view to render (hence its name - it controls how the app responds to a given request).

控制器是承载用户交互、模型运转、并最终选择视图进行渲染的组件。在 MVC 应用中，视图只显示信息；控制器处理并对用户输入和交互做出响应。在 MVC 模式，控制器是最初的入口，负责选择同哪一个模型类型协作和选择哪一个视图用来呈现（就如其名：它控制应用对所给的请求如何做出响应）。

.. note:: Controllers should not be overly complicated by too many responsibilities. To keep controller logic from becoming overly complex, use the `Single Responsibility Principle <http://deviq.com/single-responsibility-principle/>`_ to push business logic out of the controller and into the domain model.

.. note:: 控制器不应该有太多职责而过于复杂。 为避免控制器逻辑过于复杂，请使用 `单一职责原则 <http://deviq.com/single-responsibility-principle/>`_ 将业务逻辑从控制器移到领域模型。

.. tip:: If you find that your controller actions frequently perform the same kinds of actions, you can follow the `Don't Repeat Yourself principle <http://deviq.com/don-t-repeat-yourself/>`_ by moving these common actions into `filters`_.

.. tip:: 如果你发现你的控制器方法频繁执行相同类型的方法，你可以依照 `不要让自己重复原则 <http://deviq.com/don-t-repeat-yourself/>`_ 将这些通用方法移入 `过滤器（filters）`_.


What is ASP.NET Core MVC
------------------------


什么是 ASP.NET Core MVC
------------------------

The ASP.NET Core MVC framework is a lightweight, open source, highly testable presentation framework optimized for use with ASP.NET Core.

ASP.NET Core MVC 框架是一个为使用 ASP.NET Core 优化的轻量级、开源、高度可测试的表现框架。

ASP.NET Core MVC provides a patterns-based way to build dynamic websites that enables a clean separation of concerns. It gives you full control over markup, supports TDD-friendly development and uses the latest web standards.

ASP.NET Core MVC 提供了一种基于模式的、使用干净的关注分离的方式构建动态网站。它使你能对标签完全控制，支持有好的测试驱动开发（TDD）开发方式并且使用最新的 Web 标准。


Features
--------


功能特点
---------

ASP.NET Core MVC includes the following features:

ASP.NET Core MVC 包括以下特点：

- `Routing`_
- `Model binding`_
- `Model validation`_
- `Dependency injection`_
- `Filters`_
- `Areas`_
- `Web APIs`_
- `Testability`_
- `Razor view engine`_
- `Strongly typed views`_
- `Tag Helpers`_
- `View Components`_

Routing
^^^^^^^

路由
^^^^

ASP.NET Core MVC is built on top of :doc:`ASP.NET Core's routing </fundamentals/routing>`, a powerful URL-mapping component that lets you build applications that have comprehensible and searchable URLs. This enables you to define your application's URL naming patterns that work well for search engine optimization (SEO) and for link generation, without regard for how the files on your web server are organized. You can define your routes using a convenient route template syntax that supports route value constraints, defaults and optional values.

ASP.NET Core MVC 是建立在 :doc: `ASP.NET Core 路由 </fundamentals/routing>` 上的，一项强大的 URL 映射组件，助你建立拥有可理解的、可搜索的 URL 的应用。这使得你可以定义你的应用的 URL 命名形式，使得它对搜索引擎优化（SEO）和链接生成中运行良好，而不用关心你的 WEB 服务器上的文件如何组织。你可以使用方便的路由模板语法定义你的路由，路由模板语法支持路由值约束、默认值和可选值。

*Convention-based routing* enables you to globally define the URL formats that your application accepts and how each of those formats maps to a specific action method on given controller. When an incoming request is received, the routing engine parses the URL and matches it to one of the defined URL formats, and then calls the associated controller's action method.

*基于约束的路由* 允许你全局定义你的应用支持的 URL 格式，及这些格式如何各自在给定的控制器中映射到指定的操作（Action）方法。 当接收到传入请求，路由引擎转换 URL 且匹配它至一个定义的 URL 格式模板，然后调用关联的控制器的操作方法。

.. code-block:: c#

  routes.MapRoute(name: "Default", template: "{controller=Home}/{action=Index}/{id?}");

*Attribute routing* enables you to specify routing information by decorating your controllers and actions with attributes that define your application's routes. This means that your route definitions are placed next to the controller and action with which they're associated.

*特性路由（Attribute routing）* 允许你以在控制器和方法使用添加特性的方式指定路由信息来定义你的应用的路由。这意味着你的路由定义紧邻它们所关联的控制器和方法。

.. code-block:: c#
  :emphasize-lines: 1,4

  [Route("api/[controller]")]
  public class ProductsController : Controller
  {
    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
      ...
    }
  }

Model binding
^^^^^^^^^^^^^

建立模型（Model）
^^^^^^^^^^^^^^^^

ASP.NET Core MVC :doc:`model binding </mvc/models/model-binding>` converts client request data  (form values, route data, query string parameters, HTTP headers) into objects that the controller can handle. As a result, your controller logic doesn't have to do the work of figuring out the incoming request data; it simply has the data as parameters to its action methods.

ASP.NET Core MVC :doc:`模型绑定 </mvc/models/model-binding>` 转换客户端请求数据（从值、路由数据、请求字符参数、HTTP 标头）为控制器可以处理的对象。所以，你的控制器逻辑不需要做识别出传入请求数据的工作；使得参数数据传入到操作方法简单化。

.. code-block:: C#

  public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null) { ... }

Model validation
^^^^^^^^^^^^^^^^

模型（Model）验证
^^^^^^^^^^^^^^^^^^

ASP.NET Core MVC supports :doc:`validation </mvc/models/validation>` by decorating your model object with data annotation validation attributes. The validation attributes are check on the client side before values are posted to the server, as well as on the server before the controller action is called.

ASP.NET Core MVC 支持 :doc:`校验 </mvc/models/validation>` ，以为你的模型对象添加数据批注校验特性装饰。校验特性在客户端数值传到服务器之前被检查，同时在控制器方法被调用之前也会检查。

.. code-block:: c#
  :emphasize-lines: 4-5,8-9

  using System.ComponentModel.DataAnnotations;
  public class LoginViewModel
  {
      [Required]
      [EmailAddress]
      public string Email { get; set; }
    
      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }
    
      [Display(Name = "Remember me?")]
      public bool RememberMe { get; set; }
  }

A controller action:

一个控制器方法：

.. code-block:: c#
  :emphasize-lines: 3

  public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
  {
      if (ModelState.IsValid)
      {
        // work with the model
      }
      // If we got this far, something failed, redisplay form
      return View(model);
  }

The framework will handle validating request data both on the client and on the server. Validation logic specified on model types is added to the rendered views as unobtrusive annotations and is enforced in the browser with `jQuery Validation <http://jqueryvalidation.org/>`__.

框架在客户端和服务端都将处理请求数据校验。在模型上指定的验证逻辑被添加到渲染后的视图中作为隐藏脚本，且利用 `jQuery Validation <http://jqueryvalidation.org/>`__在浏览器中被强制执行。

Dependency injection
^^^^^^^^^^^^^^^^^^^^

依赖注入
^^^^^^^^^

ASP.NET Core has built-in support for :doc:`dependency injection (DI) </fundamentals/dependency-injection>`. In ASP.NET Core MVC, :doc:`controllers </mvc/controllers/dependency-injection>` can request needed services through their constructors, allowing them to follow the `Explicit Dependencies Principle <http://deviq.com/explicit-dependencies-principle/>`_.

ASP.NET Core 内置了对 :doc:`依赖注入 (DI) </fundamentals/dependency-injection>` 的支持。在 ASP.NET Core MVC 中 :doc:`控制器 </mvc/controllers/dependency-injection>` 能通过它们的构造函数请求所需的服务，允许它们遵循 `显式依赖项原则（Explicit Dependencies Principle） <http://deviq.com/explicit-dependencies-principle/>`_。

Your app can also use :doc:`dependency injection in view files </mvc/views/dependency-injection>`, using the ``@inject`` directive:

你的应用也可以使用 :doc:`视图文件中的依赖注入 </mvc/views/dependency-injection>` ，使用 ``@inject`` 命令：

.. code-block:: html
  :emphasize-lines: 1

  @inject SomeService ServiceName
  <!DOCTYPE html>
  <html>
  <head>
    <title>@ServiceName.GetTitle</title>
  </head>
  <body>
    <h1>@ServiceName.GetTitle</h1>
  </body>
  </html>

Filters
^^^^^^^

过滤器（Filters）
^^^^^^^^^^^^^^^^^^^

:doc:`Filters </mvc/controllers/filters>` help developers encapsulate cross-cutting concerns, like exception handling or authorization. Filters enable running custom pre- and post-processing logic for action methods, and can be configured to run at certain points within the execution pipeline for a given request. Filters can be applied to controllers or actions as attributes (or can be run globally). Several filters (such as ``Authorize``) are included in the framework.

:doc:`过滤器 </mvc/controllers/filters>` 帮助开发者封装横切关注点，如异常处理或身份验证。过滤器允许运行为操作方法自定义的前期的和请求过程中的逻辑，也可以被配置为在给定请求的执行管道的特定时刻执行。过滤器可以作为特性被应用到控制器或方法（也可以全局运行）。框架包含了几项过滤器（比如 ``Authorize`` ）。

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Controllers/AccountController.cs
  :lines: 17-19
  :emphasize-lines: 1
  :language: c#

Areas
^^^^^

区域（Areas）
^^^^^^^^^^^^^

:doc:`Areas </mvc/controllers/areas>` provide a way to partition a large ASP.NET Core MVC Web app into smaller functional groupings. An area is effectively an MVC structure inside an application. In an MVC project, logical components like Model, Controller, and View are kept in different folders, and MVC uses naming conventions to create the relationship between these components. For a large app, it may be advantageous to partition the app into separate high level areas of functionality. For instance, an e-commerce app with multiple business units, such as checkout, billing, and search etc. Each of these units have their own logical component views, controllers, and models.

:doc:`Areas </mvc/controllers/areas>` 提供了一种将庞大的 ASP.NET Core MVC 网站应用分解的方法。区域是应用中一项有效的MVC结构。在 MVC 项目中，逻辑组件如 Model、控制器及视图放在不同的文件夹，MVC 使用命名规范来在这些组件间建立关系。对庞大的应用，将应用分解为单独的高级功能区域是非常有益的。例如，一个电子商务应用拥有多个业务单元，比如结算、账单、与搜索等。这些单元中的每一项都有它们各自的逻辑组件视图、控制器和模型。

Web APIs
^^^^^^^^

网络应用程序接口（Web APIs）
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

In addition to being a great platform for building web sites, ASP.NET Core MVC has great support for building Web APIs. You can build services that can reach a broad range of clients including browsers and mobile devices.

除了是一个强大的创建网站的平台，ASP.NET Core MVC 对 Web APIs 也具有强有力的支持。你可以创建服务，连接到广泛的客户端，包括各种浏览器和移动设备。

The framework includes support for HTTP content-negotiation with built-in support for :doc:`formatting data </mvc/models/formatting>` as JSON or XML. Write :doc:`custom formatters </mvc/models/custom-formatters>` to add support for your own formats.

框架内置支持 :doc:`格式化数据 </mvc/models/formatting>` 如 JSON 或 XML ，使其具备了对 HTTP 内容协商的支持。编写 :doc:`自定义格式 </mvc/models/custom-formatters>`  以支持你的自有格式。

Use link generation to enable support for hypermedia. Easily enable support for `cross-origin resource sharing (CORS) <http://www.w3.org/TR/cors/>`__ so that your Web APIs shared across multiple Web applications.

使用链接生成可以启用对超媒体的支持。简单地启用对 `跨域资源共享 (CORS) <http://www.w3.org/TR/cors/>`__ 的支持，可使得你的 Web APIs 能在多个应用间共享。

Testability
^^^^^^^^^^^

可测试性（Testablility）
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The framework's use of interfaces and dependency injection make it well-suited to unit testing, and the framework includes features (like a TestHost and InMemory provider for Entity Framework) that make :doc:`integration testing </testing/integration-testing>` quick and easy as well. Learn more about :doc:`testing controller logic </mvc/controllers/testing>`.

框架接口和依赖注入的使用，使其适合进行单元测试，且框架包含的功能（如 TestHost 和 Entity Framework 内存提供程序）使得 :doc:`集成测试 </testing/integration-testing>` 也是非常快捷和方便的。了解更多关于 :doc:`测试控制器逻辑 </mvc/controllers/testing>`。

Razor view engine
^^^^^^^^^^^^^^^^^

Razor 视图引擎
^^^^^^^^^^^^^^^

:doc:`ASP.NET Core MVC views </mvc/views/overview>` use the the :doc:`Razor view engine </mvc/views/razor>` to render views. Razor is a compact, expressive and fluid template markup language for defining views using embedded C# code. Razor is used to dynamically generate web content on the server. You can cleanly mix server code with client side content and code.

:doc:`ASP.NET Core MVC 视图 </mvc/views/overview>` 使用 :doc:`Razor 视图引擎 </mvc/views/razor>` 渲染视图。 Razor 是一种紧凑的、表达能力好且流畅的模板标记语言，用来使用嵌入的 C# 代码定义视图。Razor 被用来在服务器动态生成 Web 内容。你可以清晰地将服务端代码和客户端代码跟内容混合在一起。

.. code-block:: text

  <ul>
    @for (int i = 0; i < 5; i++) {
      <li>List item @i</li>
    }
  </ul>

Using the Razor view engine you can define :doc:`layouts </mvc/views/layout>`, :doc:`partial views </mvc/views/partial>` and replaceable sections.

使用 Razor 视图引擎你可以定义 :doc:`布局模板 </mvc/views/layout>`, :doc:`局部视图 </mvc/views/partial>` 及可替换的区块。

Strongly typed views
^^^^^^^^^^^^^^^^^^^^

强类型视图
^^^^^^^^^^

Razor views in MVC can be strongly typed based on your model. Controllers can pass a strongly typed model to views enabling your views to have type checking and IntelliSense support.

MVC 中的 Razor 视图可以强类型于你的模型。控制器可以传递强类型模型到视图，使你的视图支持类型检查和智能提示。

For example, the following view defines a model of type ``IEnumerable<Product>``:

例如，下面的视图定义了 ``IEnumerable<Product>`` 类型的模型：

.. code-block:: html

  @model IEnumerable<Product>
  <ul>
      @foreach (Product p in Model)
      {
          <li>@p.Name</li>
      }
  </ul>

Tag Helpers
^^^^^^^^^^^

标签辅助类（Tag Helper）
^^^^^^^^^^^^^^^^^^^^^^^^

:doc:`Tag Helpers </mvc/views/tag-helpers/intro>` enable server side code to participate in creating and rendering HTML elements in Razor files. You can use tag helpers to define custom tags (for example, ``<environment>``) or to modify the behavior of existing tags (for example, ``<label>``). Tag Helpers bind to specific elements based on the element name and its attributes. They provide the benefits of server-side rendering while still preserving an HTML editing experience.

:doc:`Tag Helpers </mvc/views/tag-helpers/intro>` 使得服务端代码可以在 Razor 文件中参与创建和渲染 HTML 元素。你可以使用 tag helper 定义自己的标签（比如 ``<environment>``）或更改已知标签的行为（如 ``<label>``）。 Tag Helper 依据元素名称和属性绑定到指定元素。它们为服务端渲染带来便利的同时保留了 HTML 编辑体验。

There are many built-in Tag Helpers for common tasks - such as creating forms, links, loading assets and more - and even more available in public GitHub repositories and as NuGet packages. Tag Helpers are authored in C#, and they target HTML elements based on element name, attribute name, or parent tag. For example, the built-in LinkTagHelper can be used to create a link to the ``Login`` action of the ``AccountsController``:

有很多内置的 Tag Helper 应对常用任务，比如创建表单、链接、加载资源等，并且在公共的 GitHub 仓库或作为 NuGet 包，还有更多可用的。Tag Helper 是用 C# 创作的，它们通过元素名、属性名或父标签定位 HTML 元素。例如，内置的 LinkTagHelper 可被用来创建一个链接指向到 ``AccountsController`` 的  ``Login`` 方法：

.. code-block:: html
  :emphasize-lines: 3

  <p>
      Thank you for confirming your email.
      Please <a asp-controller="Account" asp-action="Login">Click here to Log in</a>.
  </p>

The ``EnvironmentTagHelper`` can be used to include different scripts in your views (for example, raw or minified) based on the runtime environment, such as Development, Staring, or Production:

 ``EnvironmentTagHelper`` 可以用来在运行时环境包含不同的脚本到你的视图（例如：原始的或压缩的），例如 Development, Staring, 或 Production：

.. code-block:: html
  :emphasize-lines: 1,3-4,9

  <environment names="Development">
      <script src="~/lib/jquery/dist/jquery.js"></script>
  </environment>
  <environment names="Staging,Production">
      <script src="https://ajax.aspnetcdn.com/ajax/jquery/jquery-2.1.4.min.js"
              asp-fallback-src="~/lib/jquery/dist/jquery.min.js"
              asp-fallback-test="window.jQuery">
      </script>
  </environment>

Tag Helpers provide an HTML-friendly development experience and a rich IntelliSense environment for creating HTML and Razor markup. Most of the built-in Tag Helpers target existing HTML elements and provide server-side attributes for the element.

Tag Helper 提供友好的 HTML 开发体验和创建 HTML 与 Razor 标记时的丰富的智能提示。大多数内置的 Tag Helper 指向存在的 HTML 元素并且为元素提供服务端属性。

View Components
^^^^^^^^^^^^^^^

视图组件
^^^^^^^^^^^^

:doc:`View Components </mvc/views/view-components>` allow you to package rendering logic and reuse it throughout the application. They're similar to :doc:`partial views </mvc/views/partial>`, but with associated logic.

:doc:`View Components </mvc/views/view-components>` 允许你打包渲染逻辑并在应用中重用它。它们与 :doc:`局部视图 </mvc/views/partial>` 类似，但具有相关的逻辑。

Areas
======

原文：`Areas <https://docs.asp.net/en/latest/mvc/controllers/areas.html>`_

作者：`Dhananjay Kumar <https://twitter.com/debug_mode>`_  和 `Rick Anderson`_

翻译：`耿晓亮(Blue) <https://github.com/heyixiaoran>`_

校对：`许登洋(Seay) <https://github.com/SeayXu>`_

Areas 是 ASP.NET MVC 用来将相关功能组织成一组单独命名空间（路由）和文件夹结构（视图）的功能。使用 Areas 创建层次结构的路由，是通过添加另一个路由参数 ``area`` 到 ``Controller`` 和 ``action``。

Areas 提供了一种把大型 ASP.NET Core MVC Web 应用程序分为较小的功能分组的方法。Area 是应用程序内部一个有效的 MVC 结构。在 MVC 项目中，像 Model，Controller 和 View 的逻辑组件放在不同的文件夹中，MVC 用命名约定来创建这些组件间的关系。对于大型应用，它有利于把应用分割成独立高级功能的 Areas。例如，一个多业务单元的电子商务应用，如结账，计费和搜索等。每个单元都有自己的逻辑组件：视图、控制器和模型。在这种情况下，你可以用 Areas 在同一项目中物理分割业务组件。

在 ASP.NET Core MVC 项目中 Area 被定义成有自己的一套 controller，view 和 model 的较小的功能单元。

当有下列情况时应当考虑在 MVC 项目中用 Areas：

- 你的应用程序应该从逻辑上分隔成多个高级功能组件的
- 你想要分隔你的 MVC 项目，使每一个功能 area 可以独立工作

Area 特性：

- 一个 ASP.NET Core MVC 应用可以有任意数量的 area
- 每一个 area 都有自己的控制器、模型和视图
- 允许把大型 MVC 项目组织成多个高级组件以便可以独立工作
- 支持具有相同名称的多个控制器 - 只要它们有不同的 `areas`

让我们看一个例子，说明如何创建和使用 Areas。比如在一个商店应用程序里有两个不同分组的控制器和视图：Products 和 Services。下一个典型的文件夹结构，使用 MVC Area 看起来像下面：

- Project name

  - Areas

    - Products

      - Controllers

        - HomeController.cs

        - ManageController.cs

      - Views

        - Home

          - Index.cshtml

        - Manage

          - Index.cshtml

    - Services

      - Controllers

        - HomeController.cs

      - Views

        - Home

          - Index.cshtml

当 MVC 尝试在 Area 中渲染一个视图时，默认情况下，会尝试在下面位置中查找：

.. code-block:: text

  /Areas/<Area-Name>/Views/<Controller-Name>/<Action-Name>.cshtml
  /Areas/<Area-Name>/Views/Shared/<Action-Name>.cshtml
  /Views/Shared/<Action-Name>.cshtml

这些默认的位置可以通过 ``Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions`` 的 ``AreaViewLocationFormats`` 方法被修改。

例如，在下面的代码中文件夹名为 'Areas'，它被修改为 'Categories'。

.. code-block:: c#

  services.Configure<RazorViewEngineOptions>(options =>
  {
      options.AreaViewLocationFormats.Clear();
      options.AreaViewLocationFormats.Add("/Categories/{2}/Views/{1}/{0}.cshtml");
      options.AreaViewLocationFormats.Add("/Categories/{2}/Views/Shared/{0}.cshtml");
      options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
  });

需要注意的是 Views 文件夹结构是唯一需要重点考虑的并且剩余文件夹像 Controllers 和 Models 的内容并不重要。比如，根本不需要 Controllers 和 Models 文件夹。这是因为 Controllers 和 Models 的内容只是编译成一个 .dll 的代码不是作为 Views 的内容直到 view 被请求。 

一旦定义了文件夹层次结构，需要告诉 MVC 每一个相关的 area 的 controller。用 ``[Area]`` 特性修饰控制器名称。

.. code-block:: c#
  :emphasize-lines: 4

  ...
  namespace MyStore.Areas.Products.Controllers
  {
      [Area("Products")]
      public class HomeController : Controller
      {
          // GET: /Products/Home/Index
          public IActionResult Index()
          {
              return View();
          }

          // GET: /Products/Home/Create
          public IActionResult Create()
          {
              return View();
          }
      }
  }

用新创建的 areas 设置一个路由的定义。:doc:`routing` 详细介绍了如何创建路由定义, 包括使用传统路由与特性路由。在本例中，我们会用传统路由。想这样做, 只需打开 `Startup.cs` 文件并通过添加下边高亮的路由定义修改它。

.. code-block:: c#
  :emphasize-lines: 4-6

  ...
  app.UseMvc(routes =>
  {
    routes.MapRoute(name: "areaRoute",
      template: "{area:exists}/{controller=Home}/{action=Index}");

    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}");
  });

浏览 `http://<yourApp>/products`， ``Products`` area 中 ``HomeController`` 的 ``Index`` 方法将会被调用。

生成链接
---------------------

- 从一个基础 controller 的 area 中的方法生成链接到同一 controller 的另一个方法。

  当前请求路径像 ``/Products/Home/Create``

  HtmlHelper 语法：``@Html.ActionLink("Go to Product's Home Page", "Index")``

  TagHelper 语法：``<a asp-action="Index">Go to Product's Home Page</a>``

  注意这里不需要提供 'area' 和 'controller' 值因为他们在当前请求上下文中已经可用。这种值被称作 ``ambient`` 值。

- 从一个基础 controller 的 area 中的方法生成链接到不同 controller 的另一个方法。

  当前请求路径像 ``/Products/Home/Create``

  HtmlHelper 语法：``@Html.ActionLink("Go to Manage Products’ Home Page", "Index", "Manage")``

  TagHelper 语法：``<a asp-controller="Manage" asp-action="Index">Go to Manage Products’ Home Page</a>``

  注意这里用的 'area' 环境值是上面 'controller' 明确指定的。

- 从一个基础 controller 的 area 中的方法生成链接到不同 controller 和不同 area 另一个方法。

  当前请求路径像 ``/Products/Home/Create``

  HtmlHelper 语法：``@Html.ActionLink("Go to Services’ Home Page", "Index", "Home", new { area = "Services" })``

  TagHelper 语法：``<a asp-area="Services" asp-controller="Home" asp-action="Index">Go to Services’ Home Page</a>``

  注意这里没有环境值被用。

- 从一个基础 controller 的 area 中的方法生成链接到不在一个 area 中的不同 controller 的另一个方法。

  HtmlHelper 语法：``@Html.ActionLink("Go to Manage Products’ Home Page", "Index", "Home", new { area = "" })``

  TagHelper 语法：``<a asp-area="" asp-controller="Manage" asp-action="Index">Go to Manage Products’ Home Page</a>``

  因此生成链接到非 area 的基础 controller 方法，清空了这里 'area' 的环境值。

发布 Areas
---------------------

发布 areas 文件夹的所有 view，在 project.json 包含一个条目在 ``publishOptions`` 的 ``include`` 节点如下：

.. code-block:: c#

  "publishOptions": {
  "include": [
    "Areas/**/*.cshtml",
    ....
    ....
  ]
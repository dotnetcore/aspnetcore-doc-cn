Areas
======

作者：`Dhananjay Kumar <https://twitter.com/debug_mode>`_  and `Rick Anderson`_

翻译：`耿晓亮(Blue)`_

Areas 是用来将相关功能组织成一组单独命名空间（路由）和文件夹结构（视图）的 ASP.NET MVC 功能。使用 Areas 创建路由的层次结构，通过添加另一个路由参数，area，controller 和 action。

Areas 提供了一种把大型 ASP.NET Core MVC Web 应用程序划分为小功能组。Area 是应用程序内部有效的 MVC 结构。在 MVC 项目中，像 Model，Controller 和 View 的逻辑组件放在不同的文件夹中，MVC 用命名约定创建这些组件间的关系。对于大型应用，它有利于把应用分割成独立高级功能的 Areas。例如，一个多业务单元的电子商务应用，如结账，计费和搜索等。每个单元都有自己的逻辑组件 view，controller 和 model。在这种情况下，你可以用 Areas 在同一项目中物理分割业务组件。

在 ASP.NET Core MVC 项目中 Area 被定义成有自己的一套 controller，view 和 model 的更小的功能单元。

当有下列情况时应当考虑再 MVC 项目中用 Areas：

- 应用程序应该从逻辑上分隔成多个高级功能组件
- 想要分隔 MVC 程序以便每一个功能 area 可以独立工作

Area 特性：

- ASP.NET Core MVC 应用可以有任意数量的 area
- 每一个 area 都有自己的 controller，model 和 view
- 允许把大型 MVC 项目组织成多个高级组件以便可以独立工作
- 支持多个 controller 有同样的名字 - 由于有不同的 *areas*

请看下边的例子来说明 Areas 是如何创建和应用的。比如在一个商店应用程序里有两个不同分组的 controller 和 view：Products 和 Services。下面是应用 MVC areas 的典型文件夹结构：

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

当 MVC 尝试在 Area 中渲染一个 view 时，默认情况下，会尝试在下面位置中查找：

 ``/Areas/<Area-Name>/Views/<Controller-Name>/<Action-Name>.cshtml``
 ``/Areas/<Area-Name>/Views/Shared/<Action-Name>.cshtml``
 ``/Views/Shared/<Action-Name>.cshtml``

通过 ``Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions`` 的 ``AreaViewLocationFormats`` 方法可以改变默认的位置。

例如，在下面的代码中文件夹 'Areas' 变成了 'Categories'。

.. code-block:: c#

  services.Configure<RazorViewEngineOptions>(options =>
  {
      options.AreaViewLocationFormats.Clear();
      options.AreaViewLocationFormats.Add("/Categories/{2}/Views/{1}/{0}.cshtml");
      options.AreaViewLocationFormats.Add("/Categories/{2}/Views/Shared/{0}.cshtml");
      options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
  });

需要注意的是 Views 文件夹结构是唯一需要重点考虑的并且剩余文件夹像 Controllers 和 Models 的内容并不重要。比如，根本不需要 Controllers 和 Models 文件夹。这是因为 Controllers 和 Models 的内容只是编译成一个 .dll 的代码不是作为 Views 的内容直到 view 被请求。 

一旦定义了文件夹层次结构，需要告诉 MVC 每一个相关的 area 的 controller。用 ``[Area]`` 特性装饰 controller 名称。 

用新创建的 areas 设置一个路由定义。:doc:`routing` 详细介绍了如何创建路由定义, 包括使用传统路由与特性路由。在本例中，我们会用传统路由。想这样做, 只需打开 Startup.cs 文件并通过添加下边高亮的路由定义修改它。

浏览 http://<yourApp>/products，``Products`` area 中 ``HomeController`` 的 ``Index`` 方法将会被调用。

链接生成
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

  当前请求路径像 /Products/Home/Create

  HtmlHelper 语法：``@Html.ActionLink("Go to Services’ Home Page", "Index", "Home", new { area = "Services" })``

  TagHelper 语法：``<a asp-area="Services" asp-controller="Home" asp-action="Index">Go to Services’ Home Page</a>``

  注意这里没有环境值被用。

- 从一个基础 controller 的 area 中的方法生成链接到不在一个 area 中的不同 controller 的另一个方法。

  HtmlHelper 语法：``@Html.ActionLink("Go to Manage Products’ Home Page", "Index", "Home", new { area = "" })``

  TagHelper 语法：``<a asp-area="" asp-controller="Manage" asp-action="Index">Go to Manage Products’ Home Page</a>``

  因此生成链接到非 area 的基础 controller 方法，清空了这里 'area' 的环境值。

发布 Areas
---------------------

发布 areas 文件夹的所有 view，在 project.json 文件中的发布选项中包含一个如下的条目：

.. code-block:: c#

  "publishOptions": {
  "include": [
    "Areas/**/*.cshtml",
    ....
    ....
  ]
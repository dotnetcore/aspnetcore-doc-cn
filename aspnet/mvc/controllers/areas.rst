Areas
======

作者：`Dhananjay Kumar <https://twitter.com/debug_mode>`__  and `Rick Anderson`_

翻译：`耿晓亮(Blue)`

Areas 提供了一种分离大型 MVC 应用程序的 models ,views 和 controllers 语义相关分组的方式，让我们看一个示例来说明 Areas 如何创建和使用。假设你有一个存储程序包含了两个不同分组的 controllers 和 views ：Products 和 Services。

替换 Controllers 文件夹下所有的 controllers 和 Views 文件夹下所有的 views，你可以使用 Areas 将视图和控制器根据关联的领域来分组（或者用逻辑分组）。

reas provide a way to partition a large ASP.NET Core MVC Web app into smaller functional groupings. An area is effectively an MVC structure inside an application. In an MVC project, logical components like Model, Controller, and View are kept in different folders, and MVC uses naming conventions to create the relationship between these components. For a large app, it may be advantageous to partition the  app into separate high level areas of functionality. For instance, an e-commerce app with multiple business units, such as checkout, billing, and search etc. Each of these units have their own logical component views, controllers, and models. In this scenario, you can use Areas to physically partition the business components in the same project.

An area can be defined as smaller functional units in an ASP.NET Core MVC project with its own set of controllers, views, and models.

Consider using Areas in an MVC project when:

- Your application is made of multiple high-level functional components that should be logically separated
- You want to partition your MVC project so that each functional area can be worked on independently

Area features:

- An ASP.NET Core MVC app can have any number of areas
- Each area has its own controllers, models, and views
- Allows you to organize large MVC projects into multiple high-level components that can be worked on independently
- Supports multiple controllers with the same name - as long as they have different *areas*

Let's take a look at an example to illustrate how Areas are created and used. Let's say you have a store app that has two distinct groupings of controllers and views: Products and Services. A typical folder structure for that using MVC areas looks like below:

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

查看上边的目录层次结构的示例，在定义 areas 时要坚持几条准则：

- Areas 目录必须作为项目的子目录存在。
- 项目的每一个 Areas 都包含 Areas 子目录(示例中的 Products 和 Services)。
- controllers 应该在像下面的位置： ``/Areas/[area]/Controllers/[controller].cs``
- views 应该在像下面的位置： ``/Areas/[area]/Views/[controller]/[action].cshtml``

注意，如果你有一个跨 controllers 分享的 view ，它可以在下边任何一个位置：

- ``/Areas/[area]/Views/Shared/[action].cshtml``
- ``/Views/Shared/[action].cshtml``

一旦你定义了文件夹的层次结构，你需要告诉 MVC 每一个控制器相关的 areas。用 ``[Area]`` 特性修饰控制器名称。

.. code-block:: c#
  :emphasize-lines: 4

  ...
  namespace MyStore.Areas.Products.Controllers
  {
      [Area("Products")]
      public class HomeController : Controller
      {
          // GET: /<controller>/
          public IActionResult Index()
          {
              return View();
          }
      }
  }

最后一步是建立一个基于新创建的 areas 的路由定义。 :doc:`routing` 详细介绍了如何创建路由定义, 包括使用传统路由与特性路由。在本例中，我们会用传统路由。想这样做, 只需打开 *Startup.cs* 文件并修改添加下边高亮的路由定义。

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

现在，当用浏览器转到 *http://<yourApp>/products*, ``Products`` area 中 ``HomeController`` 文件的 ``Index`` 操作方法就会被调用。

Areas 之间的关联
---------------------

想要 areas 之间关联, 只需要指定通过 :doc:`Tag Helpers </mvc/views/tag-helpers/index>` 定义的 controller 的 area。

下面的代码段演示了在叫做 *Products* area 中如何连接到一个控制器操作。

.. code-block:: c#

  @Html.ActionLink("See Products Home Page", "Index", "Home", new { area = "Products" }, null)

关联到一个不是 area 部分的控制器操作，只需要移除 ``asp-route-area`` 

.. code-block:: c#

  @Html.ActionLink("Go to Home Page", "Index", "Home", new { area = "" }, null)

总结
-------
Areas 是一个非常有用的用于分组语义相关的 controllers 和共同父文件夹下的 actions 的工具。通过本文, 你学习了如何设置用于 ``Areas`` 文件夹层次结构, 如何指定 ``[Area]`` 特性表示归属指定的 area 的 controller，和如何用 areas 定义路由。
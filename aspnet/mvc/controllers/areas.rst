Areas
=====

作者：`Daniel Roth`, `Travis Boatman`

翻译：`耿晓亮(Blue)`


Areas 提供了一种分离大型 MVC 应用程序的模型 (models)，视图 (views) 和控制器 (controllers) 语义相关分组的方式，让我们看一个示例来说明 Areas 如何创建和使用。假设你有一个存储程序包含了两个不同分组的控制器 (controllers) 和视图 (views) ：Products 和 Services。

替换 Controllers 文件夹下所有的控制器 (controllers) 和 Views 文件夹下所有的视图 (views)，你可以使用 Areas 将视图和控制器根据关联的领域来分组（或者用逻辑分组）。

- Project name

  - Areas

    - Products

      - Controllers

        - HomeController.cs

      - Views

        - Home

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
- 控制器 （controllers） 应该在像下面的位置：
  ``/Areas/[area]/Controllers/[controller].cs``
- 视图 （views） 应该在像下面的位置：
  ``/Areas/[area]/Views/[controller]/[action].cshtml``

注意，如果你有一个跨控制器 （controllers） 分享的视图 （view），它可以在下边任何一个位置：

- ``/Areas/[area]/Views/Shared/[action].cshtml``
- ``/Views/Shared/[action].cshtml``

一旦你定义了文件夹的层次结构，你需要告诉MVC每一个控制器相关的 areas。用 ``[Area]`` 特性修饰控制器名称。

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

最后一步是建立一个基于新创建的 areas 的路由定义。 :文档:`路由` 详细介绍了如何创建路由定义, 包括使用传统路由与特性路由。在本例中，我们会用传统路由。想这样做, 只需打开 *Startup.cs* 文件并修改添加下边高亮的路由定义。

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

想要 areas 之间关联, 只需要指定通过 :文档:`Tag Helpers</mvc/views/tag-helpers/index>` 定义的控制器（controller）的 area。

下面的代码段演示了在叫做 *Products* area 中如何连接到一个控制器操作。

.. code-block:: c#

  @Html.ActionLink("See Products Home Page", "Index", "Home", new { area = "Products" }, null)

关联到一个不是 area 部分的控制器操作，只需要移除 ``asp-route-area`` 

.. code-block:: c#

  @Html.ActionLink("Go to Home Page", "Index", "Home", new { area = "" }, null)

总结
-------
Areas 是一个非常有用的用于分组语义相关的控制器 (controllers) 和共同父文件夹下的操作（actions）的工具。通过本文, 你学习了如何设置用于 ``Areas`` 文件夹层次结构, 如何指定 ``[Area]`` 特性表示归属指定的 area 的控制器 (controller)，和如何用 areas 定义路由。

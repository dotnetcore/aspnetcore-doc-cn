添加模型
================

Adding a model
==================================================

作者： `Rick Anderson`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `何镇汐 <https://github.com/UtilCore>`_

In this section you'll add some classes for managing movies in a database. These classes will be the "**M**\odel" part of the **M**\VC app.

在这一节里，你将添加一些类来管理数据库里的电影数据。这些类将是 **M**\VC 应用程序 "**M**\odel" 的一部分。

You’ll use a .NET Framework data-access technology known as the `Entity Framework Core <http://ef.readthedocs.org/>`__ to define and work with these data model classes. Entity Framework Core (often referred to as **EF** Core) features a development paradigm called *Code First*. You write the code first, and the database tables are created from this code. Code First allows you to create data model objects by writing simple classes. (These are also known as POCO classes, from "plain-old CLR objects.") The database is created from your classes. If you are required to create the database first, you can still follow this tutorial to learn about MVC and EF app development.

你将使用一个被称为 `Entity Framework Core <http://ef.readthedocs.org/>`__ 的 .NET 框架数据访问技术来定义和使用这些数据模型类。Entity Framework Core （通常被称为 **EF** Core） 有一个特色的开发模式被称为 *Code First* 。你先写代码，然后从这个代码创建数据库表。 Code First 允许你可以通过编写简单的类来创建数据模型对象。（也被称为 POCO 类， 来自"plain-old CLR objects." ）通过你的类创建数据库。如果你需要先创建数据库，你仍然可以按照本教程来学习 MVC 和 EF 应用程序开发。

Adding data model classes
--------------------------

添加数据模型类
--------------------------

In Solution Explorer, right click the *Models* folder > **Add** > **Class**. Name the class **Movie** and add the following properties:

在解决方案资源管理器中，右击 *Models* 文件夹 > **添加** > **类** 。类命名为 **Movie** 并且添加以下属性：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/MovieNoEF.cs
  :language: c#
  :lines: 4-16
  :dedent: 0
  :emphasize-lines: 7

In addition to the properties you'd expect to model a movie, the ``ID`` field is required by the DB for the primary key. Build the project. If you don't build the app, you'll get an error in the next section. We've finally added a **M**\odel to our **M**\VC app.

除了你希望的电影属性之外，还需要 ``ID`` 字段作为数据库主键。生成项目。如果你没有生成这个应用程序，你将在下一节中遇到错误。最终我们为我们的 **M**\VC 应用程序添加了一个 **M**\odel 。

Scaffolding a controller
-------------------------

添加控制器
-------------------------

In **Solution Explorer**, right-click the *Controllers* folder **> Add > Controller**.

在 **解决方案资源管理器** 中，右击 *Controllers* 文件夹 **> 添加 > 控制器**

.. image:: adding-model/_static/add_controller.png

In the **Add Scaffold** dialog, tap **MVC Controller with views, using Entity Framework > Add**.

在 **添加基架** 对话框中，点击 **MVC Controller with views, using Entity Framework > 添加** 。

.. image:: adding-model/_static/add_scaffold2.png

Complete the **Add Controller** dialog

- **Model class:** *Movie(MvcMovie.Models)*
- **Data context class:** *ApplicationDbContext(MvcMovie.Models)*
- **Views:**: Keep the default of each option checked
- **Controller name:** Keep the default *MoviesController*
- Tap **Add**

完成 **添加控制器** 对话框

- **模型类：** *Movie（MvcMovie.Models）*
- **数据上下文类：** *ApplicationDbContext（MvcMovie.Models）*
- **视图：** 保持默认的选项
- **控制器名称：** 保持默认的 *MoviesController*
- 点击 **添加**


.. image:: adding-model/_static/add_controller2.png

The Visual Studio scaffolding engine creates the following:

- A movies controller (*Controllers/MoviesController.cs*)
- Create, Delete, Details, Edit and Index Razor view files (*Views/Movies*)

Visual Studio 基架引擎创建的内容如下：

- 一个电影控制器 (*Controllers/MoviesController.cs*)
- Create、Delete、Details、Edit 和 Index 的 Razor 视图文件 (*Views/Movies*)

Visual Studio automatically created the `CRUD <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ (create, read, update, and delete) action methods and views for you (the automatic creation of CRUD action methods and views is known as *scaffolding*). You'll soon have a fully functional web application that lets you create, list, edit, and delete movie entries.

Visual Studio 为你自动的创建 `CRUD <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ （创建、读取、更新、以及删除）操作方法和视图（自动的创建 CRUD 操作方法和视图被称为 *搭建基架*）。很快你将拥有一个可以让你创建、查看、编辑以及删除电影条目的完整功能的 Web 应用程序。

Run the app and click on the **Mvc Movie** link. You'll get the following error:

运行这个应用程序并且点击 **Mvc Movie** 链接。你将遇到以下错误：

.. image:: adding-model/_static/pending.png

That's a great error message, we'll follow those instructions to get the database ready for our Movie app.

这是一个重要的的错误消息，我们按照这些指令来得到数据库为我们的电影应用程序做准备。

Use data migrations to create the database
--------------------------------------------

使用数据迁移来创建数据库
--------------------------

- Open a command prompt in the project directory (MvcMovie/src/MvcMovie). Follow these instructions for a quick way to open a folder in the project directory.

  - Open a file in the root of the project (for this example, use *Startup.cs*.)
  - Right click on *Startup.cs*  **> Open Containing Folder**.

- 在项目文件夹（MvcMovie/src/MvcMovie）打开命令提示符。按照以下操作说明，以一个快捷的方式打开项目目录中的文件夹

  - 在项目的根目录下打开一个文件（在这个例子中，使用 *Startup.cs* 。）
  - 右击 *Startup.cs* **> 打开所在的文件夹** 。

  .. image:: adding-model/_static/quick.png

  - Shift + right click a folder > **Open command window here**

  - Shift键 + 右击一个文件夹 > **在此处打开命令窗口**

  .. image:: adding-model/_static/folder.png

  - Run ``cd ..`` to move back up to the project directory

  - 运行 ``cd ..`` 返回上级项目目录

- Run the following commands in the command prompt:

- 在命令提示符中运行以下命令：

.. code-block:: console

  dotnet ef migrations add Initial
  dotnet ef database update

- ``dotnet`` (.NET Core) is a cross-platform implementation of .NET. You can read about it `here <http://go.microsoft.com/fwlink/?LinkId=798644>`__.
- ``dotnet ef migrations add Initial`` Runs the Entity Framework .NET Core CLI migrations command and creates the initial migration. The parameter "Initial" is arbitrary, but customary for the first (*initial*) database migration. This operation creates the *Data/Migrations/2016<date-time>_Initial.cs* file containing the migration commands to add (or drop) the `Movie` table to the database.
- ``dotnet ef database update``  Updates the database with the migration we just created.

- ``dotnet`` （.NET Core） 是 .NET 的跨平台实现。你可以在 `这里 <http://go.microsoft.com/fwlink/?LinkId=798644>`__ 了解它。
- ``dotnet ef migrations add Initial`` 运行 Entity Framework .NET Core CLI 迁移命令并创建初始化迁移。参数 "Initial" 是任意值，但是通常用这个作为第一个（*初始的*）数据库迁移。这个操作创建了一个 *Data/Migrations/2016<date-time>_Initial.cs* 文件，这个文件包含了添加（或删除） `Movie` 表到数据库的迁移命令。
- ``dotnet ef database update`` 这个命令式用我们刚刚创建的迁移来更新数据库。

Test the app
------------------

测试应用
--------

- Run the app and tap the **Mvc Movie** link
- Tap the **Create New** link and create a movie

- 运行应用程序并敲入 **Mvc Movie** 链接
- 点击 **Create New** 链接并创建电影

.. image:: adding-model/_static/movies.png

.. note:: You may not be able to enter decimal points or commas in the ``Price`` field. To support `jQuery validation <http://jqueryvalidation.org/>`__ for non-English locales that use a comma (",") for a decimal point, and non US-English date formats, you must take steps to globalize your app. See `Additional resources`_ for more information. For now, just enter whole numbers like 10.

.. note:: 你或许不能在 ``Price`` 字段输入小数点或逗号。为了使 `jQuery 验证 <http://jqueryvalidation.org/>`__ 支持非英语环境用逗号（","）表示小数点，以及非美式英语日期格式支持 `jQuery 验证 <http://jqueryvalidation.org/>`__ ，你必须采取措施使你的应用程序国际化。查看 `额外的资源`_ 获取更多的信息。现在，只能输入整数，比如10。

Tapping **Create** causes the form to be posted to the server, where the movie information is saved in a database. You are then redirected to the `/Movies` URL, where you can see the newly created movie in the listing.

点击 **Create** 使表单被提交到服务器，将电影数据保存到数据库中。然后你被重定向到 `/Movies` URL ，你可以在列表中看到新创建的电影。

.. image:: adding-model/_static/h.png

Create a couple more movie entries. Try the **Edit**, **Details**, and **Delete** links, which are all functional.

创建多于两条电影条目。尝试 **Edit** 、 **Details** 和 **Delete** 链接所有的功能。

Examining the Generated Code
---------------------------------

检查生成的代码
------------------

Open the *Controllers/MoviesController.cs* file and examine the generated ``Index`` method. A portion of the movie controller with the ``Index`` method is shown below:

打开 *Controllers/MoviesController.cs* 文件并检查生成的 ``Index`` 方法。 电影控制器的一部分和 ``Index`` 方法如下所示：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 295-307

The constructor uses :doc:`Dependency Injection  </fundamentals/dependency-injection>` to inject the database context into the controller. The database context is used in each of the `CRUD <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ methods in the controller.

构造函数使用 :doc:`依赖注入  </fundamentals/dependency-injection>` 将数据库上下文注入到控制器。数据库上下文在控制器的 `增查改删（CRUD） <https://en.wikipedia.org/wiki/Create,_read,_update_and_delete>`__ 每个方法中被使用。

A request to the Movies controller returns all the entries in the ``Movies`` table and then passes the data to the ``Index`` view.

对电影控制器的一个请求返回 ``Movies`` 表中所有的条目，然后传递数据到 ``Index`` 视图（View） 。

Strongly typed models and the @model keyword
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

强类型模型与 @model 关键字
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Earlier in this tutorial, you saw how a controller can pass data or objects to a view template using the ``ViewData`` dictionary. The ``ViewData`` dictionary is a dynamic object that provides a convenient late-bound way to pass information to a view.

在本教程的早期，你见过一个控制器如何使用 ``ViewData`` 字典传递数据或对象到一个视图模板 (View Template)。 ``ViewData`` 字典是一个动态的对象，它提供一个方便后期绑定的方式将信息传递到视图（View） 。

MVC also provides the ability to pass strongly typed objects to a view template. This strongly typed approach enables better compile-time checking of your code and richer `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ in Visual Studio (VS). The scaffolding mechanism in VS used this approach (that is, passing a strongly typed model) with the ``MoviesController`` class and view templates when it created the methods and views.

MVC 还提供将强类型对象传递到 视图模板 (View Template)。这种强类型的方式可以更好的在编译时检查你的代码，和更丰富的 `智能感知 <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ 在Visual Studio（VS）中。 当 ``MoviesController`` 创建了方法（Action）和视图（View）时， VS 会使用这种基架机制（即通过强类型模型） 。

Examine the generated ``Details`` method in the *Controllers/MoviesController.cs* file:

检查在 *Controllers/MoviesController.cs* 文件中生成的 ``Details`` 方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-45
 :dedent: 8

The ``id`` parameter is generally passed as route data, for example ``http://localhost:1234/movies/details/1`` sets:

- The controller to the ``movies`` controller (the first URL segment)
- The action to ``details`` (the second URL segment)
- The id to 1 (the last URL segment)

``id`` 参数通常作为路由数据传递，例如 ``http://localhost:1234/movies/details/1`` 这样：

- 控制器对应 ``movies`` （URL 第一个部分）
- 操作对应 ``details`` （URL 第二个部分）
- id 对应 1 （URL 最后一部分）

You could also pass in the ``id`` with a query string as follows:

你还可以向下面一样使用查询字符串 （Query String） 传递 ``id`` ：

``http://localhost:1234/movies/details?id=1``

If a Movie is found, an instance of the ``Movie`` model is passed to the ``Details`` view:

如果一部电影被找到了， ``Movie`` 模型（Model）的实例将被传递到 ``Details`` 视图（View） 。

.. code-block:: C#

  return View(movie);

.. make a copy of details - later we add Ratings to it.

Examine the contents of the *Views/Movies/Details.cshtml* file:

检查  *Views/Movies/Details.cshtml* 文件的内容：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/DetailsOriginal.cshtml
 :language: HTML
 :emphasize-lines: 1

By including a ``@model`` statement at the top of the view template file, you can specify the type of object that the view expects. When you created the movie controller, Visual Studio automatically included the following ``@model`` statement at the top of the *Details.cshtml* file:

通过在视图模板（View Template）文件顶部引入一个 ``@model`` 声明，你可以指定视图（View）所期望的对象类型。当你创建电影控制器时， Visual Studio 自动的在 *Details.cshtml* 文件顶部引入下面的 ``@model`` 声明。

.. code-block:: HTML

  @model MvcMovie.Models.Movie

This ``@model`` directive allows you to access the movie that the controller passed to the view by using a ``Model`` object that's strongly typed. For example, in the *Details.cshtml* template, the code passes each movie field to the ``DisplayNameFor`` and ``DisplayFor`` HTML Helpers with the strongly typed ``Model`` object. The ``Create`` and ``Edit`` methods and view templates also pass a ``Movie`` model object.

这个 ``@model`` 指令允许你使用一个强类型 ``Model`` 对象访问控制器（Controller）传递到视图（View）的电影对象。例如，在 *Details.cshtml* 模板中，代码通过 ``DisplayNameFor`` 和 ``DisplayFor`` HTML 帮助类（HTML Helper）用强类型 ``Model`` 对象传递每个电影字段。 ``Create`` 和 ``Edit`` 方法和视图模板（View Template）也传递一个 ``Movie`` 模型（Model） 对象。

Examine the *Index.cshtml* view template and the ``Index`` method in the Movies controller. Notice how the code creates a ``List`` object when it calls the View method. The code passes this ``Movies`` list from the ``Index`` action method to the view:

检查 *Index.cshtml* 视图模板 (View Template) 和 电影控制器里的 ``Index`` 方法。注意观察代码在调用 View 方法时是如何创建一个 ``列表 (List)`` 对象的。这段代码将 ``Movies`` 列表从 ``Index`` 操作方法传递给视图（View）：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 24-27
 :dedent: 8

When you created the movies controller, Visual Studio automatically included the following ``@model`` statement at the top of the *Index.cshtml* file:

当你创建这个电影控制器时，Visual Studio 自动的在 *Index.cshtml* 文件顶部引入下面的 ``@model`` 声明：

.. Copy Index.cshtml to IndexOriginal.cshtml

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/IndexOriginal.cshtml
 :language: HTML
 :lines: 1

The ``@model`` directive allows you to access the list of movies that the controller passed to the view by using a ``Model`` object that's strongly typed. For example, in the *Index.cshtml* template, the code loops through the movies with a ``foreach`` statement over the strongly typed ``Model`` object:

``@model`` 指令允许你访问从控制器（Controller）通过强类型 ``Model`` 对象传递到视图（View）的电影列表。例如，在 *Index.cshtml* 模板中， ``foreach`` 语句遍历代表电影列表的强类型 ``Model`` 对象。

.. Copy Index.cshtml to IndexOriginal.cshtml

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/IndexOriginal.cshtml
  :language: HTML
  :emphasize-lines: 1,31, 34,37,40,43,46-48

Because the ``Model`` object is strongly typed (as an ``IEnumerable<Movie>`` object), each item in the loop is typed as ``Movie``. Among other benefits, this means that you get compile-time checking of the code and full `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ support in the code editor:

因为 ``模型（Model） `` 对象是强类型的(作为 ``IEnumerable<Movie>`` 对象)，在循环中的每一个 item 的类型为 ``Movie`` 。除了其他好处外，这意味着你可以在编译时检查代码并且在代码编辑器里得到完整的 `智能感知 <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`__ 支持：

.. image:: adding-model/_static/ints.png

You now have a database and pages to display, edit, update and delete data. In the next tutorial, we'll work with the database.

现在你有一个数据库和用于显示、编辑、更新以及删除数据的页面。在下一个教程中，我们将学习使用数据库工作。

Additional resources
-----------------------

额外的资源
-----------------------

- :doc:`/mvc/views/tag-helpers/index`
- :doc:`/fundamentals/localization`
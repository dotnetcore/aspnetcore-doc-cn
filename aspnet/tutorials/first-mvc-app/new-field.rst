添加新的字段
================================================

作者 `Rick Anderson`_

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_  


在这个章节你将使用 `Entity Framework <http://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html>`__ Code First 迁移模型中新加的字段，从而将模型字段变更同步到数据库。

当你使用 EF Code First 模式自动创建一个数据库， Code First 模式添加到数据库的表将帮助你来跟踪数据库的数据结构是否和从它生成的模型类是同步的。 如果不同步， EF 会抛出异常。这将有助于你在开发阶段就发现错误，否则可能要到运行时才能发现这个错误了 (通过一个很隐蔽的错误信息)。

添加一个 Rating 字段到 Movie 模型
---------------------------------------------

打开 *Models/Movie.cs* 文件添加一个 ``Rating`` 属性：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/MovieDateRating.cs
  :language: c#
  :lines: 7-18
  :dedent: 4
  :emphasize-lines: 11

生成应用程序 (Ctrl+Shift+B) 。

因为你已经在 ``Movie`` 类添加了一个新的字段，你还需要更新绑定的白名单，这样这个新的属性将包括在内。 为了`` Create`` 和 ``Edit`` 行为方法包含 ``Rating`` 属性需要更新 ``[Bind]`` 属性：

.. code-block:: c#

 [Bind("ID,Title,ReleaseDate,Genre,Price,Rating")]

为了把这个字段显示出来你必须更新视图，在浏览器视图中创建或者编辑一个新的 ``Rating`` 属性。

编辑 */Views/Movies/Index.cshtml* 文件并添加一个 ``Rating`` 字段：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/IndexGenreRating.cshtml
  :language: HTML
  :emphasize-lines: 16,37
  :lines: 24-61

更新 */Views/Movies/Create.cshtml* 文件添加 ``Rating`` 字段。你可以从上一个 ``form group`` 拷贝/粘帖以便于让智能感知帮助你更新字段。智能感知参考 :doc:`Tag Helpers </mvc/views/tag-helpers/intro>`。

.. image:: new-field/_static/cr.png

应用程序无法工作直到我们更新了数据库包含新的字段。如果你现在运行程序，你将得到下面的 ``SqlException`` ：

.. image:: new-field/_static/se.png

你会看到这个错误是因为更新过 Movie 模型类与数据库中存在的 Movie 的结构是不同的。(数据库表中没有 Rating 列)

有以下几种方法解决这个错误：

#. Entity Framework 可以基于新的模型类自动删除并重建数据库结构。在开发环节的早期阶段，当你在测试数据库上积极做开发的时候，这种方式是非常方便的；它可以让你快速的同时更新模型类和数据库结构。但是，缺点是你会丢失数据库中的现有的数据 – 因此你不想在生产数据库中使用这种方法！使用初始化方法自动填充测试数据数据库往往是开发应用程序的一个有效的方式。

#. 显式修改现有数据库的结构使得它的模型类相匹配。这种方法的好处是，你可以保留你录入过的数据。您可以手动修改或通过执行一个自动创建的数据库更改脚本进行变更。

#. 采用 Code First 迁移来更新数据库结构。

对于本教程, 我们采用 Code First 迁移。

更新 ``SeedData`` 类以便于为新的的字段提供填充值。下面展示一个变更的例子，但是你想使每个 ``new Movie`` 都应用这个变更。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/SeedDataRating.cs
  :language: c#
  :lines: 25-32
  :dedent: 16
  :emphasize-lines: 6

生成解决方案，然后打开命令提示符。输入以下命令：

.. code-block:: console

  dotnet ef migrations add Rating
  dotnet ef database update

``migrations add`` 命令通知数据库迁移框架检查 ``Movie`` 模型是否当前 ``Movie`` 数据库表结构一致，如果不一致，就会创建新的必要的代码把数据库迁移到新的模型。"Rating" 名字可以是任意的，只是用于迁移文件。对于迁移操作采用有意义的名字是有帮助的。


如果在数据库中删除所有记录，数据库将会被初始化并添加 ``Rating`` 字段。你可以在浏览器或者 SSOX （貌似微软的一个 VPN 移动应用，译者注）中点击删除链接。
 
运行应用程序并验证您可以用 ``Rating`` 创建/编辑/显示电影。你还应该将 ``Rating`` 字段添加到 ``Edit`` ，``Details`` 和 ``Delete`` 视图模板中。

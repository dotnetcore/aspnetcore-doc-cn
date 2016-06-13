
检查自动生成的Detail方法和Delete方法
======================================================

作者 `Rick Anderson`_

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_

打开 Movie 控制器并查看 ``Details`` 方法:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-44
 :dedent: 8

创建这个 action 方法的 MVC 基架引擎添加了一条注释给出了会调用这个方法的 HTTP 请求。在这个例子中是一个有三个URL段的GET请求， ``Movies`` 控制器， ``Details`` 方法和 ``id`` 参数值。回顾一下 Startup 里定义的这些段。


.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 8
  :emphasize-lines: 5

代码先行（Code First）模式易于使用 ``SingleOrDefaultAsync`` 方法数据搜索。这个方法包含的一个重要安全功能，就是在代码尝试用电影做任何事之前确保查找方法已经找到了一条电影记录。例如，黑客可以把链接产生的 URL 从 *http://localhost:xxxx/Movies/Details/1*  改成类似于 *http://localhost:xxxx/Movies/Details/12345* （或者其他不代表实际电影记录的值），从而给网站带来错误。如果您不检查影片是否为空，应用程序将会抛出异常。

查看 Delete 方法和 DeleteConfirmed 的方法

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-145
 :dedent: 8

需要注意的是 ``HTTP GET Delete`` 方法不删除指定的影片，它返回一个你可以提交 (HttpPost) 删除操作的  Movie 的视图。如果在对 GET 请求的响应中执行删除操作（或者编辑，创建，或任何其他更改数据的操作）将会引入一个安全漏洞。

真正删除数据的 ``[HttpPost]`` 方法被命名为 ``DeleteConfirmed`` ，使这个 HTTP POST 方法有了唯一的签名或名称。这两个方法的签名如下：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-120,135-136,139
 :dedent: 8

公共语言运行时（CLR）需要重载方法有一个唯一的参数签名（相同的方法名，但不同的参数列表）。然而，在这里你需要两个 ``Delete`` 方法 —— 一个 GET 请求一个 POST 请求 —— 并且它们都具有相同的参数签名。（它们都接受一个整数作为参数）。

有两种方案可以解决该问题，其中一种方法是，赋予方法不同的名称。这就是基架机制在前面的例子所做的事情。然而，这引入了一个小问题： ASP.NET 利用名字将 URL 段映射到 action 方法，如果你重命名一个方法，路由通常将无法找到该方法。解决的办法就是你在例子中看到的，就是为 ``DeleteConfirmed`` 方法添加 ``ActionName（"Delete"）`` 特性。该特性为路由系统执行映射，所以一个 POST 请求的包含 /Delete/ 的 URL 会找到 ``DeleteConfirmed`` 的方法。

对于具有相同名称和参数签名的方法，另一种常见的的解决办法，是通过人为的改变 POST 方法的签名，即包含一个附加的（未使用）参数。这就是我们在前面文章中已经添加的 ``unused`` 的参数。在这里你可以对 ``[HttpPost] Delete`` 方法采用同样的解决办法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 312-322
 :dedent: 8

.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core


检查自动生成的Detail方法和Delete方法
======================================================

作者 `Rick Anderson`_

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_

打开 Movie 控制器并查看 ``Details`` 方法:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-44
 :dedent: 8

创建这个 action 方法的 MVC 脚手架引擎添加了一段备注表明 HTTP 请求会调用这个方法。在这个例子中是一个URL为3部分的GET请求， ``Movies`` 控制器， ``Details`` 方法和 ``id`` 参数值。这些部分的调用定义在 Startup 里。


.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 8
  :emphasize-lines: 5

代码先行（Code First）模式使用 ``SingleOrDefaultAsync`` 方法更易于数据搜索。内置的方法是一个重要的安全特征，即在代码尝试做任何与电影的事之前确保搜索的方法要找到一条电影的记录。例如，黑客通过修改由已生成的URL地址 *http://localhost:xxxx/Movies/Details/1*  类似于 *http://localhost:xxxx/Movies/Details/12345* 可能给网站造成错误（或者其他实际上不代表真实的电影）。如果您不检查影片是否为空，应用程序将会抛出异常。

查看 Delete 方法和 DeleteConfirmed 的方法

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-145
 :dedent: 8

需要注意的是 ``HTTP GET Delete`` 方法不删除指定的影片，它返回一个你可以提交 (HttpPost) 删除操作的  Movie 的视图，您可以执行（HttpPost）删除方法来完成删除操作。如果在一个 GET 请求的的响应中执行删除操作（类似的，执行编辑操作，创建操作，或任何其他更改数据的操作），将会带来了一个安全漏洞。

删除数据的 ``[HttpPost]`` 方法被命名为 ``DeleteConfirmed`` 通过给 HTTP POST 方法一个唯一的签名或名称。这两个方法的签名如下：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-120,135-136,139
 :dedent: 8

公共语言运行时（CLR）需要重载方法有一个唯一的参数签名（相同的方法名，但不同的参数列表）。然而，在这里，你需要两个 ``Delete`` 方法 一个 GET 请求一个 POST请求，并且它们都具有相同的签名。（它们都需要接受一个整数作为参数）。

为了解决该问题，有2种解决方案可以选择。其中一种方法是，赋予方法不同的名称。这就是脚手架机制在前面的例子所做的事情。然而，这引入了一个小问题： ASP.NET 映射 url 各部分来执行方法，如果你重命名一个方法，路由通常将无法找到该方法。解决的办法就是你在例子中看到的，就是为 ``DeleteConfirmed`` 方法添加 ``ActionName（"Delete"）`` 属性。该属性执行映射到路由系统，所以包含/ Delete / 的URL的 POST请求会找到 ``DeleteConfirmed`` 的方法。

另一种常见的解决方法是方法具有相同的名称和签名，通过人为的改变 POST 方法的签名，即包含一个额外的（未使用）参数。这就是我们在前面文章中已经添加的 ``未使用`` 的参数。你可以为 ``[HttpPost] Delete`` 方法做同样的事情：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 312-322
 :dedent: 8

.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core

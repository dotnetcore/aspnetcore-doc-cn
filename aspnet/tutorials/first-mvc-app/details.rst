
检查自动生成的Detail方法和Delete方法
======================================================

作者 `Rick Anderson`_
翻译 `kiler(谢炀)`_

打开 Movie 控制器并查看 ``Details`` 方法:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-44
 :dedent: 8

创建这个 action 方法的 MVC 脚手架引擎添加了一段备注表明 HTTP 请求会调用这个方法。在本例中 GET 请求被分为了3部分 ， ``Movies`` controller,  ``Details`` 方法 以及 ``id`` 参数值。如何解析请求的方式定义在 Startup 里面。


.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 8
  :emphasize-lines: 5

代码先行（Code First）模式使用 ``SingleOrDefaultAsync``方法更易于搜索数据。方法内置了一个重要的安全点，即在代码试图处理影片记录之前以确保检索方法找到一条影片记录。例如，黑客可以通过修改地址，由 *http://localhost:xxxx/Movies/Details/1*  修改为 *http://localhost:xxxx/Movies/Details/12345* （或者其他在实际影片库中不存在的参数值）。如果您不检查影片是否为空，应用程序将会抛出异常。
 

查看 Delete 方法和 DeleteConfirmed 的方法

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-145
 :dedent: 8

需要注意的是 ``HTTP GET Delete`` 方法不删除指定的影片，它返回一个你可以提交 (HttpPost) 删除操作的  Movie 的视图，您可以执行（HttpPost）删除方法来完成删除操作。如果在一个 GET 请求的的响应中（类似的，执行编辑操作，创建操作，或任何其他更改数据的操作）执行删除操作，将会带来了一个安全漏洞。 

删除数据的 ``[HttpPost]`` 方法，被命名为 DeleteConfirmed 。给 HTTP POST 方法一个唯一的签名或名称。这两个方法的签名如下：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-120,135-136,139
 :dedent: 8

公共语言运行库（CLR）需要重载方法有一个独特的签名（相同的方法名，但不同的参数列表）。然而，在这里，你需要两个 ``Delete`` 方法 一个 GET 请求一个 POST请求，并且它们都具有相同的签名。（它们都需要接受一个整数作为参数）。

为了解决该问题，有2种解决方案可以选择。其中一种方法是，赋予方法不同的名称。这就是脚手架机制在前面的例子所做的事情。然而，这引入了一个小问题： ASP.NET 映射 url 各部分来执行方法，如果你重命名一个方法，路由通常将无法找到该方法。解决的办法就是你看到的例子中所做的，即为DeleteConfirmed方法添加ActionName（"Delete"）属性。这将影响到路由系统，包含/ Delete / URL的 POST请求会调用 DeleteConfirmed 的方法。

另外一个解决方案是人为地改变 Post 方法的签名，使其包含 ``未使用`` 的参数。也可以对 ``[HttpPost] Delete`` 方法做同样的事:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 312-322
 :dedent: 8

.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core
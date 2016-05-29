
Examining the Details and Delete methods
======================================================
查询Details和Delete方法

By `Rick Anderson`_

作者 `Rick Anderson`_

Open the Movie controller and examine the ``Details`` method:

打开Movie控制器并且查看 ``Details`` 方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-44
 :dedent: 8

The MVC scaffolding engine that created this action method adds a comment showing a HTTP request that invokes the method. In this case it's a GET request with three URL segments, the ``Movies`` controller, the ``Details`` method and a ``id`` value. Recall these segments are defined in Startup.

MVC scaffolding引擎添加了一个注释显示，在调用一个HTTP请求方法中。这种情况下，GET请求有3个URL段， ``Movies`` 控制器， ``Details`` 方法和一个 ``id`` 值。记得这三段都是在Startup中定义的。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 8
  :emphasize-lines: 5

Code First makes it easy to search for data using the ``SingleOrDefaultAsync`` method. An important security feature built into the method is that the code verifies that the search method has found a movie before the code tries to do anything with it. For example, a hacker could introduce errors into the site by changing the URL created by the links from  *http://localhost:xxxx/Movies/Details/1* to something like  *http://localhost:xxxx/Movies/Details/12345* (or some other value that doesn't represent an actual movie). If you did not check for a null movie, the app would throw an exception.
Code First使用 ``SingleOrDefaultAsync`` 方法搜索数据比较容易。一个重要安全功能内置到了方法中。代码验证搜索方法已经找到的movie，然后再执行其他代码。例如，黑客可以在网站中通过更改 *http://localhost:xxxx/Movies/Details/1* 到 *http://localhost:xxxx/Movies/Details/1* （或者一些其它值，并不代表实际的movie）从而使得链接URL出现错误。如果您没有检测是否是空movie，应用程序就会抛出一个错误。

Examine the Delete and DeleteConfirmed methods.

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-145
 :dedent: 8

Note that the ``HTTP GET Delete`` method doesn't delete the specified movie, it returns a view of the movie where you can submit (HttpPost) the deletion. Performing a delete operation in response to a GET request (or for that matter, performing an edit operation, create operation, or any other operation that changes data) opens up a security hole.

The ``[HttpPost]`` method that deletes the data is named ``DeleteConfirmed`` to give the HTTP POST method a unique signature or name. The two method signatures are shown below:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-120,135-136,139
 :dedent: 8

The common language runtime (CLR) requires overloaded methods to have a unique parameter signature (same method name but different list of parameters). However, here you need two ``Delete`` methods -- one for GET and one for POST -- that both have the same parameter signature. (They both need to accept a single integer as a parameter.)

There are two approaches to this problem, one is to give the methods different names. That's what the scaffolding mechanism did in the preceding example. However, this introduces a small problem: ASP.NET maps segments of a URL to action methods by name, and if you rename a method, routing normally wouldn't be able to find that method. The solution is what you see in the example, which is to add the ``ActionName("Delete")`` attribute to the ``DeleteConfirmed`` method. That attribute performs mapping for the routing system so that a URL that includes /Delete/ for a POST request will find the ``DeleteConfirmed`` method.

Another common work around for methods that have identical names and signatures is to artificially change the signature of the POST method to include an extra (unused) parameter. That's what we did in a previous post when we added the ``notUsed`` parameter. You could do the same thing here for the ``[HttpPost] Delete`` method:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 312-322
 :dedent: 8

.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core
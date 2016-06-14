Adding Search    添加搜索
==================================================

翻译： `魏美娟(初见) <http://github.com/ChujianA>`_

校对： 

In this section you'll add search capability to the ``Index`` action method that lets you search movies by *genre* or *name*.
在本节中，将添加搜索功能在Index方法中，这样可以通过*gener*和*name*搜索movies.

Update the ``Index`` action method to enable search:
更新``Index``方法使它能够搜索：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 154-166
 :dedent: 8

The first line of the ``Index`` action method creates a `LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__ query to select the movies:
``Index``方法的第一行创建一个`LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__查询语句去选择(个人认为翻译成查询比较合适)movies

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 156-157
 :dedent: 12


The query is *only* defined at this point, it **has not** been run against the database.
仅仅在这里定义了查询语句，并没有在数据库里执行。

If the ``searchString`` parameter contains a string, the movies query is modified to filter on the value of the search string, using the following code:
如果``searchString``参数包含一个字符串(即searchString不为空)，movies查询语句被改为对搜索字符串的值进行过滤，用以下代码：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :lines: 159-163
  :dedent: 12
  :emphasize-lines: 3

The ``s => s.Title.Contains()`` code above is a `Lambda Expression <http://msdn.microsoft.com/en-us/library/bb397687.aspx>`__. Lambdas are used in method-based `LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__ queries as arguments to standard query operator methods such as the `Where <http://msdn.microsoft.com/en-us/library/system.linq.enumerable.where.aspx>`__ method or ``Contains`` used in the code above. LINQ queries are not executed when they are defined or when they are modified by calling a method such as ``Where``, ``Contains``  or ``OrderBy``. Instead, query execution is deferred, which means that the evaluation of an expression is delayed until its realized value is actually iterated over or the ``ToListAsync`` method is called. For more information about deferred query execution, see `Query Execution <http://msdn.microsoft.com/en-us/library/bb738633.aspx>`__.
以上``s => s.Title.Contains()`` 代码是一个`Lambda表达式 <http://msdn.microsoft.com/en-us/library/bb397687.aspx>`__.Lambdas是基于`LINQ <http://msdn.microsoft.com/en-us/library/bb397926.aspx>`__ 查询的方法，作为标准查询操作方法的参数，比如用在以上代码中的`Where <http://msdn.microsoft.com/en-us/library/system.linq.enumerable.where.aspx>`__方法或者``Contains``。当LINQ 查询被定义或者通过调用比如``Where``, ``Contains``  or ``OrderBy``方法被修改时，它并没有被执行。相反，查询执行被延迟。这就意味着表达式的赋值被延迟，直到调用遍历或者``ToListAsync``方法后，才是真实值。关于延迟查询执行的更多信息，请参考`Query Execution <http://msdn.microsoft.com/en-us/library/bb738633.aspx>`__。

.. Note:: The `Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__ method is run on the database, not the c# code above. On the database, `Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__ maps to `SQL LIKE <http://msdn.microsoft.com/en-us/library/ms179859.aspx>`__, which is case insensitive.
.. Note:: `Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__方法是在数据库中运行，并不是在以上C#代码中。在数据库中，`Contains <http://msdn.microsoft.com/en-us/library/bb155125.aspx>`__映射为`SQL LIKE <http://msdn.microsoft.com/en-us/library/ms179859.aspx>`__，这里不区分大小写


Navigate to ``/Movies/Index``. Append a query string such as ``?searchString=ghost`` to the URL. The filtered movies are displayed.
导航到``/Movies/Index``。在URL中添加一个查询字符串比如``?searchString=ghost``。显示被过滤的movies。

.. image:: search/_static/ghost.png

If you change the signature of the ``Index`` method to have a parameter named ``id``, the ``id`` parameter will match the optional ``{id}`` placeholder for the default routes set in *Startup.cs*.
如果改变``Index`` 方法中被命名为``id``的参数，在*Startup.cs*中，``id``参数将会匹配可选择的``{id}``占位符作为默认路由。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 12
  :emphasize-lines: 5

You can quickly rename the ``searchString`` parameter to ``id`` with the **rename** command. Right click on ``searchString`` **> Rename**.
用**rename**命令很快的将``searchString``参数重命名为``id``，右击``searchString`` **> Rename**。

.. image:: search/_static/rename.png

The rename targets are highlighted.
重命名对象被高亮显示。

.. image:: search/_static/rename2.png

Change the parameter to ``id`` and all occurrences of ``searchString`` change to ``id``.
将参数改成``id``，并且将所有``searchString``出现的地方都改成``id``。

.. image:: search/_static/rename3.png

The previous ``Index`` method:
之前的``Index``方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 154-166
 :emphasize-lines: 1, 6,8
 :dedent: 8

The updated ``Index`` method:
更改后的``Index``方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 173-185
 :emphasize-lines: 1, 6,8
 :dedent: 8

You can now pass the search title as route data (a URL segment) instead of as a query string value.
现在可以通过查询标题作为路由数据（一个URL部分），而不是查询字符串的值。
.. image:: search/_static/g2.png

However, you can't expect users to modify the URL every time they want to search for a movie. So now you'll add UI to help them filter movies. If you changed the signature of the ``Index`` method to test how to pass the route-bound ``ID`` parameter, change it back so that it takes a parameter named ``searchString``:
然而，不能要求用户每次搜索movie时都要更改URL，所以现在添加用户界面来过滤movie，如果要改变``Index``方法的签名来测试如何通过路由来绑定``ID``参数，改回来，将参数命名为``searchString``:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 154-166
 :emphasize-lines: 1, 6,8
 :dedent: 8

.. Index.cshtml is never referenced in the .rst files and is used only to test the code.
  Copy the relevant IndexXXX.cshtml file to Index.cshtml and test.
  Open the *Views/Movies/Index.cshtml* file, and add the ``<form>`` markup highlighted below:

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/IndexForm1.cshtml
  :language: HTML
  :lines: 1-21
  :emphasize-lines: 13-18

The HTML ``<form>`` tag uses the :doc:`Form Tag Helper </mvc/views/working-with-forms>`, so when you submit the form, the filter string is posted to the ``Index`` action of the movies controller. Save your changes and then test the filter.
HTML中的``<form>``标签使用:doc:`Form Tag Helper </mvc/views/working-with-forms>`，当提交表单时，筛选字符串将被提交到movies控制器的``Index``方法中。保存更改并测试。

.. image:: search/_static/filter.png

There's no ``[HttpPost]`` overload of the ``Index`` method as you might expect. You don't need it, because the method isn't changing the state of the app, just filtering data.
``Index``方法没有如你所期望的加载``[HttpPost]``，不需要加载它，因为这个方法没有改变应用程序的状态，仅仅是过滤数据。

You could add the following ``[HttpPost] Index`` method.
应该在下面的``[HttpPost] Index``方法中添加``[HttpPost]``。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
  :language: c#
  :lines: 212-218
  :dedent: 8
  :emphasize-lines: 1

The ``notUsed`` parameter is used to create an overload for the ``Index`` method. We'll talk about that later in the tutorial.
``notUsed``参数被用来为``Index``方法创建一个重载。在之后的教程再讨论这个。

If you add this method, the action invoker would match the ``[HttpPost] Index`` method, and the ``[HttpPost] Index`` method would run as shown in the image below.
添加这个方法，action调用将匹配``[HttpPost] Index``方法，``[HttpPost] Index``方法将运行如下图所示。

.. image:: search/_static/fo.png

However, even if you add this ``[HttpPost]`` version of the ``Index`` method, there's a limitation in how this has all been implemented. Imagine that you want to bookmark a particular search or you want to send a link to friends that they can click in order to see the same filtered list of movies. Notice that the URL for the HTTP POST request is the same as the URL for the GET request (localhost:xxxxx/Movies/Index) -- there's no search information in the URL. The search string information is sent to the server as a `form field value <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Forms/Sending_and_retrieving_form_data>`__. You can verify that with the `F12 Developer tools <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__ or the excellent `Fiddler tool <http://www.telerik.com/fiddler>`__. Start the `F12 tool <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__:
然而，即使添加``Index``方法的这个``[HttpPost]``版本，这儿有个限制，在这是如何被实现的。想象一下，给特定查询添加标签或者给朋友发送一个链接，他们就能看到相同的过滤的movies列表。注意，HTTP POST请求的URL和GET请求的URL是相同的(localhost:xxxxx/Movies/Index)--URL中 没有搜索信息。搜索字符串信息被作为`form field value <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/Forms/Sending_and_retrieving_form_data>`__表单字段值发送到服务器。可以按`F12 Developer tools <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`（F12开发者工具）验证或者比较好的`Fiddler tool <http://www.telerik.com/fiddler>`__。开始`F12 tool <https://dev.windows.com/en-us/microsoft-edge/platform/documentation/f12-devtools-guide/>`__:


Tap the **http://localhost:xxx/Movies  HTTP POST 200** line and then tap **Body  > Request Body**.
点击**http://localhost:xxx/Movies  HTTP POST 200**行，然后点击**Body  > Request Body**。

.. image:: search/_static/f12_rb.png

You can see the search parameter and :doc:`XSRF </security/anti-request-forgery>` token in the request body. Note, as mentioned in the previous tutorial, the :doc:`Form Tag Helper </mvc/views/working-with-forms>` generates an :doc:`XSRF </security/anti-request-forgery>` anti-forgery token. We're not modifying data, so we don't need to validate the token in the controller method.
在请求体可以看到搜索字符串和:doc:`XSRF </security/anti-request-forgery>`令牌。注意，在前面的教程中提到:doc:`Form Tag Helper </mvc/views/working-with-forms>`生成:doc:`XSRF </security/anti-request-forgery>`防伪标记。没有修改数据，所以不需要在控制器方法中验证令牌。

Because the search parameter is in the request body and not the URL, you can't capture that search information to bookmark or share with others. We'll fix this by specifying the request should be ``HTTP GET``. Notice how intelliSense helps us update the markup.
因为搜索参数是在请求主体里，而不是在URL中，所以不能捕捉到搜索信息给书签或者和其他人共享。通过指定请求应该是``HTTP GET``来解决这个。注意，智能提示怎样帮助我们更新标记。

.. image:: search/_static/int_m.png

.. image:: search/_static/int_get.png

Notice the distinctive font in the ``<form>`` tag. That distinctive font indicates the tag is supported by :doc:`Tag Helpers </mvc/views/tag-helpers/intro>`.
注意``<form>``标签中的独特字体，那个独特字体标识的标签是被:doc:`Tag Helpers </mvc/views/tag-helpers/intro>`支持的。

.. image:: search/_static/th_font.png


Now when you submit a search, the URL contains the search query string. Searching will also go to the ``HttpGet Index`` action method, even if you have a ``HttpPost Index`` method.
现在点击搜索，URL包含搜索查询字符串。搜索将进入``HttpGet Index``的action方法，即使有一个``HttpPost Index`` 方法。

.. image:: search/_static/search_get.png


Adding Search by Genre             添加搜索类型
------------------------           ------------

Add the following ``MovieGenreViewModel`` class to the *Models* folder:
在*Models*文件夹中添加以下``MovieGenreViewModel``类

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/MovieGenreViewModel.cs
 :language: c#

The move-genre view model will contain:
移动类型视图模型将包含：

 - a list of movies
 - movies的列表
 - a `SelectList <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/SelectList/index.html>`__ containing the list of genres. This will allow the user to select a genre from the list.
 `SelectList <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/SelectList/index.html>`__ 包含类型列表，并允许用户从列表中选择一种类型。
 
 - ``movieGenre``, which contains the selected genre
 - ``movieGenre``, 包含被选择的类型。

Replace the ``Index`` method with the following code:
用以下代码替代``Index`` 方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 223-247
 :dedent: 8


The following code is a ``LINQ`` query that retrieves all the genres from the database.
下面代码是用来从数据库中检索所有类型的``LINQ``查询。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 225-228
 :dedent: 12

The ``SelectList`` of genres is created by projecting the distinct genres (we don't want our select list to have duplicate genres).
genres（风格）为``SelectList``是通过投影不同的genres来创建的（我们不希望选择列表中有重复的genres）

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 243
 :dedent: 12


Adding search by genre to the Index view          在Index视图中添加搜索类型的模型
--------------------------------------------      --------------------------------

.. literalinclude:: start-mvc/sample/src/MvcMovie/Views/Movies/IndexFormGenre.cshtml
  :language: HTML
  :lines: 1-64
  :emphasize-lines: 1, 15-17,27,41

Test the app by searching by genre, by movie title, and by both.
通过搜索类型，movie标题或者这2个测试应用程序。
添加控制器
===================

Adding a controller
==================================================

原文 `Adding a controller <https://docs.asp.net/en/latest/tutorials/first-mvc-app/adding-controller.html>`_

作者： `Rick Anderson`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `刘怡(AlexLEWIS) <https://github.com/alexinea>`_ 、`何镇汐 <https://github.com/UtilCore>`_ 、`夏申斌 <https://github.com/xiashenbin>`_ 、`孟帅洋(书缘) <https://github.com/mengshuaiyang>`_

The Model-View-Controller (MVC) architectural pattern separates an app into three main components: the **M**\odel, the **V**\iew, and the **C**\ontroller. The MVC pattern helps you create apps that are testable and easier to maintain and update than traditional monolithic apps. MVC-based apps contain:

Model-View-Controller (MVC) 架构模式将一个应用程序分离成三个主要的组件： **M**\odel、**V**\iew 以及 **C**\ontroller 。 MVC 模式帮助你创建可测试的应用程序，比传统的单块应用程序更加容易维护和更新。基于 MVC 的应用程序包含：

- **M**\odels: Classes that represent the data of the app and that use validation logic to enforce business rules for that data. Typically, model objects retrieve and store model state in a database. In this tutorial, a ``Movie`` model retrieves movie data from a database, provides it to the view or updates it. Updated data is written to a SQL Server database.
- **V**\iews: Views are the components that display the app's user interface (UI). Generally, this UI displays the model data. 
- **C**\ontrollers: Classes that handle browser requests, retrieve model data, and then specify view templates that return a response to the browser. In an MVC app, the view only displays information; the controller handles and responds to user input and interaction. For example, the controller handles route data and query-string values, and passes these values to the model. The model might use these values to query the database.

- **M**\odels：应用程序中用来表示数据的类，并使用验证逻辑来执行该数据业务规则。通常，模型（Model）对象从数据库查询和存储 Model 状态。在本教程	``Movie`` 模型（Model）从数据库查询电影数据用来显示或更新。更新后的数据写入 SQL Server 数据库。
- **V**\iews：视图是显示用户界面（UI）的组件。通常，UI 用于显示模型（Model）数据。
- **C**\ontrollers：一种类（Class），用于处理浏览器请求，查询模型（Model）数据，以及将指定视图模板作为响应返回给浏览器。在 MVC 应用程序中，视图（View）仅仅显示信息； 控制器（Controller）处理和响应用户的输入和交互。例如， 控制器（Controller）处理路由数据和查询字符串值，然后将这些值传递给模型（Model），模型（Model）可以使用这些值去查询数据库。


The MVC pattern helps you create apps that separate the different aspects of the app (input logic, business logic, and UI logic), while providing a loose coupling between these elements. The pattern specifies where each kind of logic should be located in the app. The UI logic belongs in the view. Input logic belongs in the controller. Business logic belongs in the model. This separation helps you manage complexity when you build an app, because it enables you to work on one aspect of the implementation at a time without impacting the code of another. For example, you can work on the view code without depending on the business logic code. 

MVC 模式帮助你创建一个分离不同方面的应用程序(输入逻辑，业务逻辑，以及 UI 逻辑)，同时这些元素之间是松耦合的。该模式指定在应用程序中的每一种逻辑应该位于何处。 UI 逻辑属于视图（View）。输入逻辑属于控制器（Controller）。业务逻辑属于模型（Model）。当你构建一个应用程序时，这样的分离帮助你管理应用程序的复杂性，因为它使你编写一个方面的代码时不会影响其他(方面)的代码。比如，你可以编写视图（View）代码而不需要依赖于业务逻辑代码。

We'll be covering all these concepts in this tutorial series and show you how to use them to build a simple movie app. The following image shows the *Models*, *Views* and *Controllers* folders in the MVC project.

我们会在本系列教程中涵盖所有这些概念，并告诉你如何使用它们构建一个简单的电影应用程序。下面的图片展示了 MVC 项目中的 *Models* 、 *Views* 以及 *Controllers* 文件夹。

.. image:: adding-controller/_static/mvc1.png

- In **Solution Explorer**, right-click **Controllers > Add > New Item... > MVC Controller Class**

- 在 **解决方案资源管理器（Solution Explorer）** 中，鼠标右键点击 **Controllers > 添加（Add） > 新建项...（New Item...） > MVC 控制器类（MVC Controller Class）**

.. image:: adding-controller/_static/add_controller.png

- In the **Add New Item** dialog, enter **HelloWorldController**.

- 在 **添加新项（Add New Item）** 对话框，输入 **HelloWorldController**。

Replace the contents of *Controllers/HelloWorldController.cs* with the following: 

用下面的代码替换 *Controllers/HelloWorldController.cs* 中的内容：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 5-28
  
Every ``public`` method in a controller is callable as an HTTP endpoint. In the sample above, both methods return a string.  Note the comments preceding each method:

控制器（Controller）的每个 ``public`` 方法都可作为 HTTP 端点。在上面的例子中，两个方法都返回 string，注意它们的注释：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 10-28
  :dedent: 4
  :emphasize-lines: 4,12

The first comment states this is an `HTTP GET <http://www.w3schools.com/tags/ref_httpmethods.asp>`__ method that is invoked by appending "/HelloWorld/" to the base URL. The second comment specifies an `HTTP GET <http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html>`__ method that is invoked by appending "/HelloWorld/Welcome/" to the URL. Later on in the tutorial we'll use the scaffolding engine to generate ``HTTP POST`` methods.

第一条注释指出这是一个通过在 URL 后添加 "/HelloWorld/" 调用的 `HTTP GET <http://www.w3schools.com/tags/ref_httpmethods.asp>`__ 方法，。第二条指出这是一个通过在 URL 后添加 "/HelloWorld/Welcome/" 调用的 `HTTP GET <http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html>`__ 方法 。之后的教程我们将使用基架引擎来生成 ``HTTP POST`` 方法。

Run the app in non-debug mode (press Ctrl+F5) and append "HelloWorld" to the path in the address bar. (In the image below, http://localhost:1234/HelloWorld is used, but you'll have to replace *1234* with the port number of your app.) The ``Index`` method returns a string. You told the system to return some HTML, and it did! 

使用非调试模式(Ctrl+F5)运行应用程序，并在浏览器地址栏路径后添加 "HelloWorld" (在下面的图片中，使用了 http://localhost:1234/HelloWorld ，但是你必须用你的应用程序端口替换 *1234* )。 ``Index`` 方法返回一段字符串，系统将这段字符串转换为 HTML 返回给浏览器。

.. image:: adding-controller/_static/hell1.png

MVC invokes controller classes (and the action methods within them) depending on the incoming URL. The default :doc:`URL routing logic </mvc/controllers/routing>` used by MVC uses a format like this to determine what code to invoke:

MVC 调用的控制器（Controller）类 (以及它们的 Action 方法) 取决于传入的 URL 。MVC 的默认 :doc:`URL 路由逻辑 </mvc/controllers/routing>` 采用类似下面规则格式来决定代码的调用：

``/[Controller]/[ActionName]/[Parameters]``

You set the format for routing in the *Startup.cs* file.

你可以在 *Startup.cs* 文件中设置路由规则。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-85
  :dedent: 12
  :emphasize-lines: 5

When you run the app and don't supply any URL segments, it defaults to the "Home" controller and the "Index" method specified in the template line highlighted above. 

当你运行应用程序且不提供任何 URL 段时，它将默认访问在上面模板中高亮行指定的 "Home" Controller 中的 "Index" Action 方法。

The first URL segment determines the controller class to run. So ``localhost:xxxx/HelloWorld`` maps to the ``HelloWorldController`` class. The second part of the URL segment determines the action method on the class. So ``localhost:xxxx/HelloWorld/Index`` would cause the ``Index`` method of the ``HelloWorldController`` class to run. Notice that we only had to browse to ``localhost:xxxx/HelloWorld`` and the ``Index`` method was called by default. This is because ``Index`` is the default method that will be called on a controller if a method name is not explicitly specified. The third part of the URL segment ( ``id``) is for route data. We'll see route data later on in this tutorial.

第一个 URL 段决定运行哪个控制器（Controller）。所以 ``localhost:xxxx/HelloWorld`` 映射到 ``HelloWorldController``  类。URL 段的第二部分决定类里的 Action 方法。所以 ``localhost:xxxx/HelloWorld/Index``  将运行 ``HelloWorldController`` 中的 ``Index`` 方法。请注意，我们只需要浏览 ``localhost:xxxx/HelloWorld`` ，默认会调用 ``Index`` 方法。这是因为在没有指定方法名时， ``Index`` 是默认方法。URL 段的第三部分 ( ``id``) 是路由数据。我们之后将在本教程中了解路由数据。

Browse to ``http://localhost:xxxx/HelloWorld/Welcome``. The ``Welcome`` method runs and returns the string "This is the Welcome action method...". For this URL, the controller is ``HelloWorld`` and ``Welcome`` is the action method. We haven't used the ``[Parameters]`` part of the URL yet.

浏览 ``http://localhost:xxxx/HelloWorld/Welcome`` 。 ``Welcome`` 方法运行并返回 "This is the Welcome action method..." 。对于这个 URL ， 控制器（Controller）是 ``HelloWorld`` ， Action 方法是 ``Welcome`` 。我们还没有使用 URL 中的 ``[Parameters]`` 部分。

.. image:: adding-controller/_static/welcome.png

Let's modify the example slightly so that you can pass some parameter information  from the URL to the controller (for example, ``/HelloWorld/Welcome?name=Scott&numtimes=4``).  Change the ``Welcome`` method  to include two parameters as shown below. Note that the code uses the C# optional-parameter feature to indicate that the ``numTimes`` parameter defaults to 1 if no value is passed for that parameter.

让我们稍微修改一下例子，使我们能够通过 URL 传递一些参数信息到控制器（Controller）(例如， ``/HelloWorld/Welcome?name=Scott&numtimes=4`` )。如下所示修改 ``Welcome`` 方法使其包含两个参数。请注意，代码利用 C# 的可选参数特性指明在没有传递参数的情况下， ``numTimes`` 参数默认为1。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: none
  :lines: 51-54
  :dedent: 8

.. note:: The code above uses ``HtmlEncoder.Default.Encode`` to protect the app from malicious input (namely JavaScript). It also uses `Interpolated Strings <https://msdn.microsoft.com/en-us/library/dn961160.aspx>`__.

.. note:: 上面的代码使用 ``HtmlEncoder.Default.Encode`` 来保护应用程序免受恶意输入(即 JavaScript)。同时也使用了 `内插字符串 <https://msdn.microsoft.com/zh-cn/library/dn961160.aspx>`__。

.. note:: In Visual Studio 2015, when you are running in IIS Express without debugging (Ctl+F5), you don't need to build the app after changing the code. Just save the file, refresh your browser and you can see the changes.

.. note:: 在 Visual Studio 2015 中，当你在 IIS Express 以非调试模式 (Ctl+F5) 运行，你不需要在修改代码后生成应用程序。只需要保存文件，刷新你的浏览器就可以看到改变。

Run your app and browse to:

  ``http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4``

(Replace xxxx with your port number.) You can try different values for ``name`` and ``numtimes`` in  the URL. The MVC :doc:`model binding </mvc/models/model-binding>` system automatically maps the named parameters from  the query string in the address bar to parameters in your method. See :doc:`/mvc/models/model-binding` for more information.

运行你的应用程序并浏览：

  ``http://localhost:xxxx/HelloWorld/Welcome?name=Rick&numtimes=4``

(用你的端口替换 xxxx。) 你可以在 URL 中对 ``name`` 和 ``numtimes`` 尝试不同的值。 MVC :doc:`模型绑定 </mvc/models/model-binding>` 系统自动将地址栏里查询字符串中有名字的参数映射到你方法中的参数。查看 :doc:模型绑定 `/mvc/models/model-binding` 获得更多的信息。

.. image:: adding-controller/_static/rick4.png

In the sample above, the URL segment (``Parameters``) is not used, the ``name`` and ``numTimes`` parameters are passed as `query strings <http://en.wikipedia.org/wiki/Query_string>`__. The ``?`` (question mark) in the above URL is a separator, and the query strings follow. The ``&`` character separates query strings.

在上面的示例中， URL 段 (``Parameters``) 没有被使用， ``name`` 和 ``numTimes`` 参数作为 `查询字符串 <http://en.wikipedia.org/wiki/Query_string>`__ 被传递。 上面 URL 中的  ``?`` (问号) 是一个分隔符，后面跟查询字符串。 ``&`` 字符分割查询字符串。

Replace the ``Welcome`` method with the following code:

用下面的代码替换 ``Welcome`` 方法：

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/HelloWorldController.cs
  :language: c#
  :lines: 80-84
  :dedent: 8
  
Run the app and enter the following URL:  ``http://localhost:xxx/HelloWorld/Welcome/3?name=Rick``

运行应用程序然后输入 URL ： ``http://localhost:xxx/HelloWorld/Welcome/3?name=Rick``

.. image:: adding-controller/_static/rick_routedata.png

This time the third URL segment  matched the route parameter ``id``. The ``Welcome``  method contains a parameter  ``id`` that matched the URL template in the ``MapRoute`` method. The trailing ``?``  (in ``id?``) indicates the ``id`` parameter is optional.

这次第三个 URL 段匹配上了路由参数 ``id`` 。 ``Welcome`` 方法包含一个与 ``MapRoute`` 内的 URL 模板相匹配的 ``id`` 参数。跟随的 ``?`` (``id?``) 表示 ``id`` 参数是可选的。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-85
  :dedent: 12
  :emphasize-lines: 5
  
In these examples the controller has been doing the "VC" portion  of MVC - that is, the view and controller work. The controller is returning HTML  directly. Generally you don't want controllers returning HTML directly, since  that becomes very cumbersome to code and maintain. Instead we'll typically use a separate Razor view template file to help generate the HTML response. We'll do that in the next tutorial.

在这些例子中，控制器（Controller）一直在做 MVC 中的 "VC" 部分，就是视图（View）和控制器（Controller）部分的工作。这个控制器（Controller）直接返回 HTML 。一般来说你不想让控制器（Controller） 直接返回 HTML ，因为这让编码和维护变得非常麻烦。所以，我们通常会使用一个单独的 Razor 视图模板文件来帮助生成 HTML 响应。 我们将在下一个教程中介绍这部分。

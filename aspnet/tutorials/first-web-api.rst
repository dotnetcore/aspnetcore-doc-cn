用 ASP.NET Core MVC 创建你的第一个 Web API
=================================================

作者 `Mike Wasson`_ 和 `Rick Anderson`_
翻译 `谢炀（kiler）`_ 

HTTP协议 不仅仅提供网页服务. 它也是一个构建公开服务和数据 API 的强大平台. HTTP 协议是简单、灵活、无处不在的。 几乎你能想到的任何平台上都有HTTP支持, 所以HTTP服务能够发送到多种客户端, 包括浏览器，移动设备和传统的桌面应用程序。

在本教程中, 你将创建一个简单的Web API来管理一个"to-do"列表. 在本教程中你无需编写任何UI代码.


ASP.NET Core 已经内置了用 MVC 架构 构建 Web API 的支持。统一了两个框架使得它易于构建应用程序，包括用户界面（HTML）和 API，因为现在它们共享相同的代码库和管道。

.. note:: 如果你想把一个老的 Web API 应用程序迁移到 ASP.NET Core, 参考 :doc:`/migration/webapi`

.. contents:: 章节:
  :local:
  :depth: 1

总览
--------
这是你需要创建的 API :

=====================  ========================  ============  =============
API                    描述                      请求正文       响应正文
=====================  ========================  ============  =============
GET /api/todo          获取所有的to-do items      无            Array of to-do items
GET /api/todo/{id}     通过ID获取item             无            To-do item
POST /api/todo         添加一个新的item           To-do item    To-do item
PUT /api/todo/{id}     更新已经存在的item         To-do item    无
DELETE /api/todo/{id}  删除指定的item.            无            无
=====================  ========================  ============  =============

下面的图表展示了应用程序的基本设计.

.. image:: first-web-api/_static/architecture.png

- 不管是哪个调用 API 的客户端（浏览器，移动应用，等等）。我们不会在本教程编写客户端。
-  *model* 是一个代表你应用程序数据的类. 在本案例中, 只有一个模型 to-do 项. 模型表现为简单 C# 类型 (POCOs).
-  *controller* 是一个处理 HTTP 请求并返回 HTTP 响应的对象. 这个示例程序将只会有一个 controller.
- 为了保证教程简单我们不使用数据库. 作为替代, 我们会把 to-do 项存入内存. 但是我们依然包含了一个数据访问层（不重要的）, 用来隔离 Web API和数据层. 如果想使用数据库, 参考 :doc:`first-mvc-app/index`.

安装Fiddler
---------------

我们不创建客户端, 我们使用  `Fiddler <http://www.fiddler2.com/fiddler2/>`__  来测试 API. Fiddler 是一个 Web 调试工具可以让您撰写的 HTTP 请求进行发送并查看原始的 HTTP 响应.

创建项目
------------------

启动 Visual Studio . 从 **File** 菜单, 选择 **New** > **Project**.

选择 **ASP.NET Core Web Application** 项目模版.项目命名为 ``TodoApi`` 并且点击 **OK**.

.. image:: first-web-api/_static/new-project.png

在 **New ASP.NET Core Web Application (.NET Core) - TodoApi** 对话框中, 选择 **Web API** 模版. 点击 **OK**.

.. image:: first-web-api/_static/web-api-project.png

添加模型类
-----------------

模型表示应用程序中的数据的对象。在本示例中，唯一使用到的模型是一个 to-do 项

添加一个名为 "Models" 的目录. 在解决方案浏览器中, 右击项目. 选择 **Add** > **New Folder**. 把目录名命名为 *Models* .

.. image:: first-web-api/_static/add-folder.png

.. note:: 你可以吧模型类放到项目的任何地方, 但是*Models*是约定的默认目录.

下一步, 添加一个 ``TodoItem`` 类. 右击 *Models* 目录并选择 **Add** > **New Item**.

在 **Add New Item** 对话框中, 选择 **Class** 模版. 命名类为 ``TodoItem`` 并点击 **OK**.

.. image:: first-web-api/_static/add-class.png

将生成代码替换为:

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/TodoItem.cs
  :language: c#

添加存储类
----------------------

*repository* 类是一个封装了数据层的类, 包含了获取数据并映射到实体模型类的业务逻辑。 尽管本例中不使用数据库，但依旧值得去思考 Repository 是如何注入到我们的 Controller 的。在 *Models*  目录下创建 repository 代码。

定义一个名为 ``ITodoRepository`` 的 repository 接口. 通过类模版 (**Add New Item**  > **Class**).

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/ITodoRepository.cs
  :language: c#

接口定义了基本的 CRUD 操作.

下一步, 添加一个实现 ITodoRepository 接口的 TodoRepository 类:

.. literalinclude:: first-web-api/sample/src/TodoApi/Models/TodoRepository.cs
  :language: c#

生成应用程序确保没有任何编译错误.

注册存储库
-----------------------

定义 repository 接口, 我们可以从使用它的 MVC controller 解耦仓储类，而不是直接在在 controller 里面实例化 ``TodoRepository`` ，我们将会用 ASP.NET Core 内置功能注入 ``ITodoRepository`` ，更多请参考 :doc:`dependency injection </fundamentals/dependency-injection>`.

这种方式可以更容易地对你的 controller 进行单元测试。单元测试应该注入一个 Mock 或存根版本的 ITodoRepository。通过这样的方式测试范围可以限制在业务逻辑层而非数据访问层。

为了注入 repository 到 controller , 我们必须注册DI容器.打开 *Startup.cs* 文件. 添加以下指令:

.. code-block:: c#

  using TodoApi.Models;

在 ``ConfigureServices`` 方法中, 添加高亮方法:

.. literalinclude:: first-web-api/sample/src/TodoApi/Startup.cs
  :language: c#
  :lines: 25-31
  :emphasize-lines: 6
  :dedent: 8

添加控制器
----------------

在解决方案浏览器中, 右击 *Controllers* 目录. 选择 **Add** > **New Item** . 在 **Add New Item** 对话框中, 选择 **Web  API Controller Class** 模版. 命名为 ``TodoController``.

将生成的代码替换为如下代码:

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 1-13,67-68

这里定义了一个空的 controller 类. 下一个章节, 我们将添加代码来实现 API.

获取 to-do 列表
-------------------

为了获取 to-do 项，添加下列方法到 ``TodoController`` 类.

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 17-31
  :dedent: 8

代码实现了两个 GET 方法:

- ``GET /api/todo``
- ``GET /api/todo/{id}``

以下是 ``GetAll` 方法 HTTP 响应:

  HTTP/1.1 200 OK
  Content-Type: application/json; charset=utf-8
  Server: Microsoft-IIS/10.0
  Date: Thu, 18 Jun 2015 20:51:10 GMT
  Content-Length: 82

  [{"Key":"4f67d7c5-a2a9-4aae-b030-16003dd829ae","Name":"Item1","IsComplete":false}]

在后面的教程中，我将会告诉你如何使用 Fiddler 工具查看 HTTP 响应。

路由和 URL 路径
^^^^^^^^^^^^^^^^^^^^^

`[HttpGet] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/HttpGetAttribute/index.html>`_  标签指定这些方法均为 HTTP GET 方法。 每个方法构建的 Url 如下:

- 替换 controller 模版里面的路由标签,  ``[Route("api/[controller]")]``
- 把 "[Controller]" 替换为控制器名, 必须是带 "Controller" 后缀的小写名称. 在本示例里面控制器的名字为 "todo"  (不区分大小写). 对于这个例子, controller 的类名是 **Todo**\Controller 并且根名是 "todo". ASP.NET MVC Core 是需要区分大小写的.
- 如果 ``[HttpGet]``  标签有模版字符串, 附加到路径. 本示例没有模版字符串.

对于 ``GetById`` 方法,  "{id}" 是一个占位符. 在实际的 HTTP 请求中, 客户端会使用 ``todo`` 项的 ID 属性. 作为运行时, 当 MVC 调用 ``GetById`` , 会把 "{id}" 占位置分配到 Url 方法的 ``id`` 参数上去.

更换 "api/todo" 的启动 Url
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 
- 右击项目 **> Properties**
- 选择 **Debug** 选项卡变更 "api/todo" **Launch URL** 设置

.. image:: first-web-api/_static/launch.png

了解更多有关请求路由的信息请参考 :doc:`/mvc/controllers/routing`.

返回值
^^^^^^^^^^^^^

``GetAll`` 方法返回一个 CLR 对象. MVC 自动把对象序列化为`JSON <http://www.json.org/>`__ 并把 JSON 对象写入响应消息正文. 响应状态码为 200, 假设没有未处理异常的情况下。（未处理异常一般会被转化为 5xx 错误。）

相反, ``GetById`` 将会返回一个 ``IActionResult`` 类型, 代表一个泛型结果对象. 因为 ``GetById`` 有两个不同的返回值:

- 如果没有数据项可以匹配 ID, 方法会返回 404 错误，并最终以返回 NotFound 告终。
- 否则, 方法会返回 200 以及 JSON 响应正文. 并最终以返回 `ObjectResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/ObjectResult/index.html>`_ 告终.

使用 Fiddler 调用 API
---------------------------

这一步是可选的, 但是有助于我们查看 Web API 返回的原始 HTTP 响应.
在 Visual Studio 中, 点击 ^F5 启动项目. Visual Studio 启动浏览器并导航到 ``http://localhost:port/api/todo``,  *port* 是一个随机数. 如果你使用 Chrome, Edge 或者 Firefox 浏览器,  *todo* 数据将会被显示. 如果你使用 IE, IE 将会弹出窗口提示要求打开或者保存 *todo.json* 文件.

启动 Fiddler. 从 **File** 菜单, 取消选择 **Capture Traffic** 选项. 这个会关闭捕获 HTTP traffic.

.. image:: first-web-api/_static/fiddler1.png

选择 **Composer** 页面. 在 **Parsed** 选项卡中, 输入``http://localhost:port/api/todo``,*port* 是实际的端口号. 点击 **Execute** 发送请求.

.. image:: first-web-api/_static/fiddler2.png

结果会显示在 sessions 列表中. 响应码是200。使用 **Inspectors** 选项开来查看响应内容, 包括请求正文.

.. image:: first-web-api/_static/fiddler3.png

实现其他的CRUD操作
------------------------------------

最后一步是``Create``, ``Update``, 以及 ``Delete``方法到controller. 这些方法都是围绕着一个主题，所以我将只显示代码和标注出的主要的区别。

Create
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 33-42
  :dedent: 8

这是一个 HTTP POST 方法, 用 `[HttpPost] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/HttpPostAttribute/index.html>`_ 标签声明. `[FromBody] <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/FromBodyAttribute/index.html>`_ 标签告诉 MVC 从 HTTP 请求的正文中获取 to-do 项的值.

`CreatedAtRoute <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Controller/index.html>`_ 方法返回 201 响应,这对于在服务器上创建了新的资源的操作的 HTTP POST 方法的标准响应。 ``CreateAtRoute`` 还把 Location 头信息加入到了响应。 Location 头信息指定新创建的 todo 项的 URI。  查看 `10.2.2 201 Created <http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html>`_.

我们使用 Fiddler 来创建和发送一个请求:

#.  在 **Composer** 页面, 从下拉框选择 POST.
#.  在请求头的文本框中, 添加 ``Content-Type: application/json`` , 意思是 ``Content-Type`` 类型的头信息值为 ``application/json``. Fiddler 会自动添加 Content-Length 头信息.
#.  在请求正文的文本框, 输入以下内容: ``{"Name":"<你的 to-do 项目>"}``
#.  点击 **Execute** .

.. image:: first-web-api/_static/fiddler4.png


这是一个简单的 HTTP 会话. 试用 **Raw** 选项卡查看会话数据.

Request::

  POST http://localhost:29359/api/todo HTTP/1.1
  User-Agent: Fiddler
  Host: localhost:29359
  Content-Type: application/json
  Content-Length: 33

  {"Name":"Alphabetize paperclips"}

Response::

  HTTP/1.1 201 Created
  Content-Type: application/json; charset=utf-8
  Location: http://localhost:29359/api/Todo/8fa2154d-f862-41f8-a5e5-a9a3faba0233
  Server: Microsoft-IIS/10.0
  Date: Thu, 18 Jun 2015 20:51:55 GMT
  Content-Length: 97

  {"Key":"8fa2154d-f862-41f8-a5e5-a9a3faba0233","Name":"Alphabetize paperclips","IsComplete":false}


Update
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 44-60
  :dedent: 8

``Update`` 和创建类似 ``Create`` ,但是使用 HTTP PUT. 响应是 `204 (No Content) <http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html>`_.
根据 HTTP 规范, PUT 请求要求客户端发送整个实体更新，而不仅仅是增量。为了支持局部更新，请使用 HTTP PATCH.

.. image:: first-web-api/_static/put.png

Delete
^^^^^^

.. literalinclude:: first-web-api/sample/src/TodoApi/Controllers/TodoController.cs
  :language: c#
  :lines: 62-68
  :dedent: 8

方法返回 204 (无内容) 响应. 这意味着客户端会受到收到 204 响应即使该项目已被删除，或者根本不存在。有两种方法来处理请求删除不存在资源的问题：

- "Delete" 代表 "删除一个已存在的项", 如果不存在返回 404.
- "Delete" 代表 "确保该项不在集合中." 如果项目不在集合中, 返回 204.

无论哪种方法是合理的。如果收到 404 错误，客户端将需要处理这种情况。

.. image:: first-web-api/_static/delete.png

下一步
----------

- 关于如何为原生移动 App 创建后端, 请参考 :doc:`/mobile/native-mobile-backend`.
- 更多关于 API 部署的问题, 请参考 :doc:`发布与部署 </publishing/index>`.
- `查看或者下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/tutorials/first-web-api/sample>`__




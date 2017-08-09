作者 [Rick Anderson](https://twitter.com/RickAndMSFT) 、 [Mike Wasson](https://github.com/mikewasson)

翻译 [谢炀（Kiler)](https://github.com/kiler398/)  

<!--HTTP is not just for serving up web pages. It’s also a powerful platform for building APIs that expose services and data. HTTP is flexible and ubiquitous. Almost any platform that you can think of has an HTTP library, so HTTP services can reach a broad range of clients, including browsers, mobile devices, and traditional desktop apps.-->
HTTP协议 不仅仅提供网页服务. 它也是一个构建公开服务和数据 API 的强大平台。HTTP 协议是简单、灵活、无处不在的。 几乎你能想到的任何平台上都有HTTP支持, 所以HTTP服务能够发送到多种客户端, 包括浏览器，移动设备和传统的桌面应用程序。

<!--In this tutorial, you’ll build a web API for managing a list of "to-do" items. You won’t build a UI.-->
在本教程中, 你将创建一个简单的 Web API 来管理一个 "to-do" 列表。在本教程中你无需编写任何 UI 代码.

<!--ASP.NET Core has built-in support for MVC creating Web APIs.-->
ASP.NET Core 已经内置了用 MVC 架构 构建 Web API 的支持。

<!--There are 3 versions of this tutorial:-->
本教程有以下3个版本:

<!--* macOS: [Web API with Visual Studio for Mac](xref:tutorials/first-web-api-mac)
* Windows: [Web API with Visual Studio for Windows](xref:tutorials/first-web-api)
* macOS, Linux, Windows: [Web API with Visual Studio Code](xref:tutorials/web-api-vsc)-->
* macOS: [使用 Visual Studio Mac 版本 开发Web API](xref:tutorials/first-web-api-mac)
* Windows: [使用 Visual Studio Windows 版本 开发Web API](xref:tutorials/first-web-api)
* macOS, Linux, Windows: [使用 Visual Studio Code 开发Web API](xref:tutorials/web-api-vsc)

<!--## Overview-->
## 总览

<!--Here is the API that you’ll create:-->
这是你需要创建的 API ：

<!--|API | Description    | Request body    | Response body   |
|--- | ---- | ---- | ---- |
|GET /api/todo  | Get all to-do items | None | Array of to-do items|
|GET /api/todo/{id}  | Get an item by ID | None | To-do item|
|POST /api/todo | Add a new item | To-do item  | To-do item |
|PUT /api/todo/{id} | Update an existing item &nbsp;  | To-do item |  None |
|DELETE /api/todo/{id}  &nbsp;  &nbsp; | Delete an item &nbsp;  &nbsp;  | None  | None|-->
|API | 描述    | 请求正文    | 响应正文   |
|--- | ---- | ---- | ---- |
|GET /api/todo  | 获取所有的to-do items | 无 | Array of to-do items|
|GET /api/todo/{id}  | 通过ID获取item | 无 | To-do item|
|POST /api/todo | 添加一个新的item | To-do item  | To-do item |
|PUT /api/todo/{id} | 更新已经存在的item &nbsp;  | To-do item |  无 |
|DELETE /api/todo/{id}  &nbsp;  &nbsp; | 删除指定的item。 &nbsp;  &nbsp;  | 无  | 无|

<br>

<!--The following diagram shows the basic design of the app.-->
下面的图表展示了应用程序的基本设计.

![The client is represented by a box on the left and submits a request and receives a response from the application, a box drawn on the right. Within the application box, three boxes represent the controller, the model, and the data access layer. The request comes into the application's controller, and read/write operations occur between the controller and the data access layer. The model is serialized and returned to the client in the response.](../../tutorials/first-web-api/_static/architecture.png)

<!--* The client is whatever consumes the web API (mobile app, browser, etc). We aren’t writing a client in this tutorial. We'll use [Postman](https://www.getpostman.com/) or [curl](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man1/curl.1.html) to test the app.-->
* 不管是哪个调用 API 的客户端（浏览器，移动应用，等等）。我们不会在本教程中编写客户端。我们将使用 [Postman](https://www.getpostman.com/) 或者 [curl](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man1/curl.1.html) 来测试应用程序。

<!--* A *model* is an object that represents the data in your application. In this case, the only model is a to-do item. Models are represented as C# classes, also know as **P**lain **O**ld **C**# **O**bject (POCOs).-->
* *model* 是一个代表你应用程序数据的类. 在本案例中, 只有一个模型 to-do 项. 模型表现为简单 C# 类型 (POCOs).

<!--* A *controller* is an object that handles HTTP requests and creates the HTTP response. This app will have a single controller.-->
* *controller* 是一个处理 HTTP 请求并返回 HTTP 响应的对象. 这个示例程序将只会有一个 controller.

<!--* To keep the tutorial simple, the app doesn’t use a persistent database. Instead, it stores to-do items in an in-memory database.-->
* 为了保证教程简单我们不使用持久化数据库. 作为替代, 我们会把 to-do 项存入内存.

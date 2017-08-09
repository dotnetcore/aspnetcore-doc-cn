---
title: 在 Mac 系统中使用 Visual Studio 以及 ASP.NET Core 开发 Web API 项目 | Microsoft 文档（中文文档）
author: rick-anderson
description: 在 Mac 系统中使用 Visual Studio 以及 ASP.NET Core 开发 Web API 项目
keywords: ASP.NET Core 中文文档, WebAPI, Web API, REST, mac, macOS, HTTP, Service, HTTP Service
ms.author: riande
manager: wpickett
ms.date: 5/24/2017
ms.topic: get-started-article
ms.assetid: 830b4af5-ed14-1638-7734-764a6f13a8f6
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-web-api-mac
---

<!--# Create a Web API with ASP.NET Core MVC and Visual Studio for Mac-->
# 在 Mac 系统中使用 Visual Studio 以及 ASP.NET Core 开发 Web API 项目

<!-- WARNING: The code AND images in this doc are used by uid: tutorials/web-api-vsc, tutorials/first-web-api-mac and tutorials/first-web-api. If you change any code/images in this tutorial, update uid: tutorials/web-api-vsc -->

[!INCLUDE[template files](../includes/webApi/intro.md)]

<!--* See [Introduction to ASP.NET Core MVC on Mac or Linux](xref:tutorials/first-mvc-app-xplat/index) for an example that uses a persistent database.-->
* 参考 [Mac、Linux 平台 ASP.NET Core MVC 介绍](xref:tutorials/first-mvc-app-xplat/index) 使用的持久化数据库。

<!--## Prerequisites-->
## 环境准备

<!--Install the following:-->
按照以下组件和程序：
 
- [.NET Core SDK](https://www.microsoft.com/net/core#macos)  
- [Visual Studio for Mac](https://www.visualstudio.com/vs/visual-studio-mac/)

<!--## Create the project-->
## 创建项目

<!--From Visual Studio, select **File > New Solution**.-->
在 Visual Studio 中， 选择 **File > New Solution**。

![macOS New solution](first-web-api-mac/_static/sln.png)

<!--Select **.NET Core App >  ASP.NET Core Web API > Next**.-->
选择 **.NET Core App >  ASP.NET Core Web API > Next**。

![macOS New project dialog](first-web-api-mac/_static/1.png)

<!--Enter **TodoApi** for the **Project Name**, and then select Create.-->
在  **Project Name** 中输入 **TodoApi**， 并点击 Create。

![config dialog](first-web-api-mac/_static/2.png)

<!--### Launch the app-->
### 启动应用程序

<!--In Visual Studio, select **Run > Start With Debugging** to launch the app. Visual Studio launches a browser and navigates to `http://localhost:port`, where *port* is a randomly chosen port number. You get an HTTP 404 (Not Found) error.  Change the URL to `http://localhost:port/api/values`. The `ValuesController` data will be displayed:-->
在 Visual Studio 中， 选择 **Run > Start With Debugging** 运行应用成. Visual Studio 启动版浏览器并导航到 `http://localhost:port`， *port* 是一个随机端口好。 你会收到 HTTP 404 (Not Found) 错误信息。修改链接地址 `http://localhost:port/api/values` 并访问。  `ValuesController` 数据显示如下：

```
["value1","value2"]
```

<!--### Add support for Entity Framework Core-->
### 添加 Entity Framework Core 支持

<!--Install the [Entity Framework Core InMemory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) database provider. This database provider allows Entity Framework Core to be used with an in-memory database.-->
安装 [Entity Framework Core InMemory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) database provider。这个 database provider 允许 Entity Framework Core 使用内存数据库。

<!--* From the **Project** menu, select **Add NuGet Packages**. -->
* 在 **Project** 菜单，选择 **Add NuGet Packages**。

<!--*  Alternately, you can right-click **Dependencies**, and then select **Add Packages**.-->
  *  另外，你也可以右击 **Dependencies**， 并选择 **Add Packages**。

<!--* Enter `EntityFrameworkCore.InMemory` in the search box.
* Select `Microsoft.EntityFrameworkCore.InMemory`, and then select **Add Package**.-->
* Enter `EntityFrameworkCore.InMemory` in the search box.
* Select `Microsoft.EntityFrameworkCore.InMemory`, and then select **Add Package**.

<!--### Add a model class-->
### 添加模型类

<!--A model is an object that represents the data in your application. In this case, the only model is a to-do item.-->
模型表示应用程序中的数据的对象。在本示例中，唯一使用到的模型是一个 to-do 项

<!--Add a folder named *Models*. In Solution Explorer, right-click the project. Select **Add** > **New Folder**. Name the folder *Models*.-->
添加一个名为 "Models" 的目录. 在解决方案浏览器中, 右击项目. 选择 **Add** > **New Folder**. 把目录名命名为 *Models* 。

![new folder](first-web-api-mac/_static/folder.png)

<!--Note: You can put model classes anywhere in your project, but the *Models* folder is used by convention.-->
Note: 你可以把模型类放到项目的任何地方, 但是 *Models* 是约定的默认目录 。

<!--Add a `TodoItem` class. Right-click the *Models* folder and select **Add > New File > General > Empty Class**. Name the class `TodoItem`, and then select **New**.-->
添加一个  `TodoItem` 类. 右击 *Models* 目录并选择 **Add > New File > General > Empty Class**。命名类为 `TodoItem` 并点击 **New**。

<!--Replace the generated code with:-->
将生成代码替换为:

[!code-csharp[Main](first-web-api/sample/TodoApi/Models/TodoItem.cs)]

<!--### Create the database context-->
### 创建数据库上下文

<!--The *database context* is the main class that coordinates Entity Framework functionality for a given data model. You create this class by deriving from the `Microsoft.EntityFrameworkCore.DbContext` class.-->
*数据库上下文* 是 Entity Framework 用来协调数据模型的主要类。您可以从 `Microsoft.EntityFrameworkCore.DbContext` 类派生来创建此类。

<!--Add a `TodoContext` class to the *Models* folder.-->
在 *Models* 目录中添加 `TodoContext` 类。

[!code-csharp[Main](first-web-api/sample/TodoApi/Models/TodoContext.cs)]

[!INCLUDE[Register the database context](../includes/webApi/register_dbContext.md)]

<!--## Add a controller-->
## Add a controller

<!--In Solution Explorer, in the *Controllers* folder, add the class `TodoController`.-->
在 Solution Explorer（解决方案浏览器）中, 在 *Controllers* 目录里面， 添加 `TodoController` 类。

<!--Replace the generated code with the following (and add closing braces):-->
将生成的代码替换为以下内容（并添加关闭大括号）：

[!INCLUDE[code and get todo items](../includes/webApi/getTodoItems.md)]

<!--### Launch the app-->
### 启动程序

<!--In Visual Studio, select **Run > Start With Debugging** to launch the app. Visual Studio launches a browser and navigates to `http://localhost:port`, where *port* is a randomly chosen port number. You get an HTTP 404 (Not Found) error.  Change the URL to `http://localhost:port/api/values`. The `ValuesController` data will be displayed:-->
在 Visual Studio 中， 选择 **Run > Start With Debugging** 运行应用成. Visual Studio 启动版浏览器并导航到 `http://localhost:port`， *port* 是一个随机端口好。 你会收到 HTTP 404 (Not Found) 错误信息。修改链接地址 `http://localhost:port/api/values` 并访问。  `ValuesController` 数据显示如下：

```
["value1","value2"]
```

导航到 `http://localhost:port/api/todo` 访问 `Todo` 控制器：

```
[{"key":1,"name":"Item1","isComplete":false}]
```

<!--## Implement the other CRUD operations-->
## 实现其他的CRUD操作

<!--We'll add `Create`, `Update`, and `Delete` methods to the controller. These are variations on a theme, so I'll just show the code and highlight the main differences. Build the project after adding or changing code.-->
我们将要把 `Create`, `Update`, 以及 `Delete`方法到 controller 。这些方法都是围绕着一个主题，所以我将只列出代码以及标注出主要的区别。生成项目以后添加或者修改代码。

### Create

[!code-csharp[Main](first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=snippet_Create)]

<!--This is an HTTP POST method, indicated by the [`[HttpPost]`](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/HttpPostAttribute/index.html) attribute. The [`[FromBody]`](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/FromBodyAttribute/index.html) attribute tells MVC to get the value of the to-do item from the body of the HTTP request.-->
这是一个 HTTP POST 方法, 用 [`[HttpPost]`](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/HttpPostAttribute/index.html) 标签声明 。[`[FromBody]`](https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/FromBodyAttribute/index.html) 标签告诉 MVC 从 HTTP 请求的正文中获取 to-do 项的值 。

<!--The `CreatedAtRoute` method returns a 201 response, which is the standard response for an HTTP POST method that creates a new resource on the server. `CreatedAtRoute` also adds a Location header to the response. The Location header specifies the URI of the newly created to-do item. See [10.2.2 201 Created](http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html).-->
当通过 `CreatedAtRoute` 方法向服务器发出 HTTP POST 方法以创建新资源时，将返回标准的 201 响应。
 ``CreateAtRoute`` 还把 Location 头信息加入到了响应。 Location 头信息指定新创建的 todo 项的 URI。  查看 [10.2.2 201 Created](http://www.w3.org/Protocols/rfc2616/rfc2616-sec10.html).

<!--### Use Postman to send a Create request-->
### 使用 Postman 发送 Create 请求


<!--* Start the app (**Run > Start With Debugging**).
* Start Postman.-->
* 启动程序 (**Run > Start With Debugging**)。
* 启动 Postman。

![Postman console](first-web-api/_static/pmc.png)

<!--* Set the HTTP method to `POST`
* Select the **Body** radio button
* Select the **raw** radio button
* Set the type to JSON
* In the key-value editor, enter a Todo item such as-->
* 设置 HTTP method 为 `POST`
* 选择 **Body** 单选按钮
* 选择 **raw** 单选按钮
* 设置类型为 JSON
* 在键值对编辑器中，输入以下 Todo 数据项

```json
{
	"name":"walk dog",
	"isComplete":true
}
```

<!--* Select **Send**-->
* 选择 **Send**

<!--* Select the Headers tab in the lower pane and copy the **Location** header:-->
* 在下面的面板中选择 Headers 选项卡拷贝 **Location** 头：

![Headers tab of the Postman console](first-web-api/_static/pmget.png)

<!--You can use the Location header URI to access the resource you just created. Recall the `GetById` method created the `"GetTodo"` named route:-->
你可以使用 Location响应头(Location header URI) 来访问你刚才创建的资源。 重新调用 `GetById` 方法创建的 `"GetTodo"` 命名路由：

```csharp
[HttpGet("{id}", Name = "GetTodo")]
public IActionResult GetById(string id)
```

### Update

[!code-csharp[Main](first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=snippet_Update)]

<!--`Update` is similar to `Create`, but uses HTTP PUT. The response is [204 (No Content)](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html). According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH.-->
`Update` 类似于 `Create` ,但是使用 HTTP PUT 。响应是 [204 (No Content)](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html) 。
根据 HTTP 规范, PUT 请求要求客户端发送整个实体更新，而不仅仅是增量。为了支持局部更新，请使用 HTTP PATCH 。

```json
{
  "key": 1,
  "name": "walk dog",
  "isComplete": true
}
```

![Postman console showing 204 (No Content) response](first-web-api/_static/pmcput.png)

### Delete

[!code-csharp[Main](first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=snippet_Delete)]

<!--The response is [204 (No Content)](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html).-->
方法返回 [204 (No Content)](http://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html)。

![Postman console showing 204 (No Content) response](first-web-api/_static/pmd.png)

<!--## Next steps-->
## 后续教程

<!--* [Routing to Controller Actions](xref:mvc/controllers/routing)
* For information about deploying your API, see [Publishing and Deployment](../publishing/index.md).
* [View or download sample code](https://github.com/aspnet/Docs/tree/master/aspnetcore/tutorials/first-web-api/sample)
* [Postman](https://www.getpostman.com/)
* [Fiddler](http://www.fiddler2.com/fiddler2/)-->
* [路由到控制器和Action](xref:mvc/controllers/routing)
* 更多如何部署你的API的信息， 请参考 [发布与部署](../publishing/index.md).
* [查看或者下载示例代码](https://github.com/aspnet/Docs/tree/master/aspnetcore/tutorials/first-web-api/sample)
* [Postman](https://www.getpostman.com/)
* [Fiddler](http://www.fiddler2.com/fiddler2/)


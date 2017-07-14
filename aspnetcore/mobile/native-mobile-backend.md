---
title: 为原生移动应用程序创建后端服务 | Microsoft 文档（中文文档）
author: ardalis
description: 
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 10/14/2016
ms.topic: article
ms.assetid: 3b6a32f2-5af9-4ede-9b7f-17ab300526d0
ms.technology: aspnet
ms.prod: asp.net-core
uid: mobile/native-mobile-backend
---
<!--# Creating Backend Services for Native Mobile Applications-->
# 为原生移动应用程序创建后端服务

作者 [Steve Smith](http://ardalis.com)

翻译 [谢炀（Kiler)](https://github.com/kiler398/)  

<!--Mobile apps can easily communicate with ASP.NET Core backend services.-->
移动应用程序可以轻松的与 ASP.NET Core 后端服务通信。

<!--[View or download sample backend services code](https://github.com/aspnet/Docs/tree/master/aspnetcore/mobile/native-mobile-backend/sample)-->
[查看下载后端服务示例代码](https://github.com/aspnet/Docs/tree/master/aspnetcore/mobile/native-mobile-backend/sample)

<!--## The Sample Native Mobile App-->
## 原生移动应用程序示例

<!--This tutorial demonstrates how to create backend services using ASP.NET Core MVC to support native mobile apps. It uses the [Xamarin Forms ToDoRest app](https://developer.xamarin.com/guides/xamarin-forms/web-services/consuming/rest/) as its native client, which includes separate native clients for Android, iOS, Windows Universal, and Window Phone devices. You can follow the linked tutorial to create the native app (and install the necessary free Xamarin tools), as well as download the Xamarin sample solution. The Xamarin sample includes an ASP.NET Web API 2 services project, which this article's ASP.NET Core app replaces (with no changes required by the client).-->
本教程演示如何使用 ASP.NET Core MVC 创建后端服务来支持原生移动应用程序。 它使用 [Xamarin Forms ToDoRest 应用程序](https://developer.xamarin.com/guides/xamarin-forms/web-services/consuming/rest/)作为其原生客户端APP，其中包括用于Android，iOS， Windows通用和Window Phone设备的各种原生客户端APP。 您可以按照链接的教程创建原生客户端APP（需要安装免费 Xamarin 工具），以及下载 Xamarin 示例解决方案。 Xamarin 示例包括一个 ASP.NET Web API 2 服务项目，会被本文的ASP.NET Core 应用程序替换（客户端不需要进行修改）。

![To Do Rest application running on an Android smartphone](native-mobile-backend/_static/todo-android.png)

<!--### Features-->
### 功能

<!--The ToDoRest app supports listing, adding, deleting, and updating To-Do items. Each item has an ID, a Name, Notes, and a property indicating whether it's been Done yet.-->
ToDoRest APP 应用程序支持列表、添加、删除、以及 To-Do 数据项。每条数据包含 ID, Name, Notes, 以及一个属性定于该条数据是否完成的字段。

<!--The main view of the items, as shown above, lists each item's name and indicates if it is done with a checkmark.-->
如上所示，项目的主视图列出了每个项目的名称，并指示是否使用复选标记。

<!--Tapping the `+` icon opens an add item dialog:-->
点击 `+` 图标打开一个添加项目对话框：

![Add item dialog](native-mobile-backend/_static/todo-android-new-item.png)

<!--Tapping an item on the main list screen opens up an edit dialog where the item's Name, Notes, and Done settings can be modified, or the item can be deleted:-->
在主列表界面上点击项目将打开一个编辑对话框，可以修改项目的“名称”，“注释”和“完成”设置，或者可以删除该项目：

![Edit item dialog](native-mobile-backend/_static/todo-android-edit-item.png)

<!--This sample is configured by default to use backend services hosted at developer.xamarin.com, which allow read-only operations. To test it out yourself against the ASP.NET Core app created in the next section running on your computer, you'll need to update the app's `RestUrl` constant. Navigate to the `ToDoREST` project and open the *Constants.cs* file. Replace the `RestUrl` with a URL that includes your machine's IP address (not localhost or 127.0.0.1, since this address is used from the device emulator, not from your machine). Include the port number as well (5000). In order to test that your services work with a device, ensure you don't have an active firewall blocking access to this port.-->
本示例默认配置为使用托管在 developer.xamarin.com 上的后端服务，该服务允许只读操作。为了测试一下接下来你自己在计算机上中创建的 ASP.NET Core 应用程序，您需要更新应用程序的 `RestUrl` 常量。 导航到 `ToDoREST` 项目并打开 *Constants.cs* 文件。 将 `RestUrl` w替换为包含机器IP地址的URL（不是本地主机或127.0.0.1，因为该地址是从设备模拟器使用的，而不是来自您的机器）。 还包括端口号（5000）。 为了测试您的服务与设备的一致性，请确保您没有主动防火墙阻止对该端口的访问。

<!-- literal_block {"ids": [], "names": [], "highlight_args": {}, "backrefs": [], "dupnames": [], "linenos": false, "classes": [], "xml:space": "preserve", "language": "csharp"} -->

```csharp
// URL of REST service (Xamarin ReadOnly Service)
//public static string RestUrl = "http://developer.xamarin.com:8081/api/todoitems{0}";

// use your machine's IP address
public static string RestUrl = "http://192.168.1.207:5000/api/todoitems/{0}";
```

<!--## Creating the ASP.NET Core Project-->
## 创建的 ASP.NET Core 应用程序

<!--Create a new ASP.NET Core Web Application in Visual Studio. Choose the Web API template and No Authentication. Name the project *ToDoApi*.-->
在 Visual Studio 中创建一个新的 ASP.NET Core Web应用程序。 选择 Web API 模板，然后选择无验证。 命名项目 *ToDoApi*。

![New ASP.NET Web Application dialog with Web API project template selected](native-mobile-backend/_static/web-api-template.png)

<!--The application should respond to all requests made to port 5000. Update *Program.cs* to include `.UseUrls("http://*:5000")` to achieve this:-->
应用程序会响应所有对端口5000的请求。通过更新 *Program.cs* 包含 `.UseUrls（"http://*:5000"）` 来实现：

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Program.cs?range=10-16&highlight=3)]

<!--> [!NOTE]
> Make sure you run the application directly, rather than behind IIS Express, which ignores non-local requests by default. Run `dotnet run` from a command prompt, or choose the application name profile from the Debug Target dropdown in the Visual Studio toolbar.-->
> [!NOTE]
> 确保您是直接运行应用程序的，而不是在 IIS Express 之后，默认情况下会忽略非本地请求。 在命令提示符运行 `dotnet run` ，或从 Visual Studio 工具栏的 Debug Target 下拉列表中选择应用程序名称配置文件。

<!--Add a model class to represent To-Do items. Mark required fields using the `[Required]` attribute:-->
添加一个模型类来表示待办事项。 使用 `[Required]` 属性来标记必填的字段：

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Models/ToDoItem.cs)]

<!--The API methods require some way to work with data. Use the same `IToDoRepository` interface the original Xamarin sample uses:-->
API 方法需要一些处理数据的方法。 使用原始 Xamarin 示例使用的相同的 `IToDoRepository` 接口：

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Interfaces/IToDoRepository.cs)]

<!--For this sample, the implementation just uses a private collection of items:-->
对于本示例，实现只使用私有的项目集合：

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Services/ToDoRepository.cs)]

<!--Configure the implementation in *Startup.cs*:-->
在 *Startup.cs* 中的配置实现：

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Startup.cs?highlight=6&range=29-35)]

<!--At this point, you're ready to create the *ToDoItemsController*.-->
此时，您已准备好创建 *ToDoItemsController*。

<!--> [!TIP]
> Learn more about creating web APIs in [Building Your First Web API with ASP.NET Core MVC and Visual Studio](../tutorials/first-web-api.md).-->
> [!TIP]
> 了解更多如何创建 web APIs的信息请参考 [使用 ASP.NET Core MVC 和 Visual Studio 创建你的第一个 Web API ](../tutorials/first-web-api.md)。

<!--## Creating the Controller-->
## 创建控制器

<!--Add a new controller to the project, *ToDoItemsController*. It should inherit from Microsoft.AspNetCore.Mvc.Controller. Add a `Route` attribute to indicate that the controller will handle requests made to paths starting with `api/todoitems`. The `[controller]` token in the route is replaced by the name of the controller (omitting the `Controller` suffix), and is especially helpful for global routes. Learn more about [routing](../fundamentals/routing.md).-->
Add a new controller to the project, *ToDoItemsController*. It should inherit from Microsoft.AspNetCore.Mvc.Controller. Add a `Route` attribute to indicate that the controller will handle requests made to paths starting with `api/todoitems`. The `[controller]` token in the route is replaced by the name of the controller (omitting the `Controller` suffix), and is especially helpful for global routes. Learn more about [routing](../fundamentals/routing.md).

<!--The controller requires an `IToDoRepository` to function; request an instance of this type through the controller's constructor. At runtime, this instance will be provided using the framework's support for [dependency injection](../fundamentals/dependency-injection.md).-->
The controller requires an `IToDoRepository` to function; request an instance of this type through the controller's constructor. At runtime, this instance will be provided using the framework's support for [dependency injection](../fundamentals/dependency-injection.md).

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=1-17&highlight=9,14)]

<!--This API supports four different HTTP verbs to perform CRUD (Create, Read, Update, Delete) operations on the data source. The simplest of these is the Read operation, which corresponds to an HTTP GET request.-->
This API supports four different HTTP verbs to perform CRUD (Create, Read, Update, Delete) operations on the data source. The simplest of these is the Read operation, which corresponds to an HTTP GET request.

<!--### Reading Items-->
### 读取数据

<!--Requesting a list of items is done with a GET request to the `List` method. The `[HttpGet]` attribute on the `List` method indicates that this action should only handle GET requests. The route for this action is the route specified on the controller. You don't necessarily need to use the action name as part of the route. You just need to ensure each action has a unique and unambiguous route. Routing attributes can be applied at both the controller and method levels to build up specific routes.-->
Requesting a list of items is done with a GET request to the `List` method. The `[HttpGet]` attribute on the `List` method indicates that this action should only handle GET requests. The route for this action is the route specified on the controller. You don't necessarily need to use the action name as part of the route. You just need to ensure each action has a unique and unambiguous route. Routing attributes can be applied at both the controller and method levels to build up specific routes.

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=19-23)]

<!--The `List` method returns a 200 OK response code and all of the ToDo items, serialized as JSON.-->
The `List` method returns a 200 OK response code and all of the ToDo items, serialized as JSON.

<!--You can test your new API method using a variety of tools, such as [Postman](https://www.getpostman.com/docs/), shown here:-->
You can test your new API method using a variety of tools, such as [Postman](https://www.getpostman.com/docs/), shown here:

![Postman console showing a GET request for todoitems and the body of the response showing the JSON for three items returned](native-mobile-backend/_static/postman-get.png)

<!--### Creating Items-->
### Creating Items

<!--By convention, creating new data items is mapped to the HTTP POST verb. The `Create` method has an `[HttpPost]` attribute applied to it, and accepts an ID parameter and a `ToDoItem` instance. The HTTP verb attributes, like `[HttpPost]`, optionally accept a route template string (`{id}` in this example). This has the same effect as adding a `[Route]` attribute to the action. Since the `item` argument will be passed in the body of the POST, this parameter is decorated with the `[FromBody]` attribute.-->
By convention, creating new data items is mapped to the HTTP POST verb. The `Create` method has an `[HttpPost]` attribute applied to it, and accepts an ID parameter and a `ToDoItem` instance. The HTTP verb attributes, like `[HttpPost]`, optionally accept a route template string (`{id}` in this example). This has the same effect as adding a `[Route]` attribute to the action. Since the `item` argument will be passed in the body of the POST, this parameter is decorated with the `[FromBody]` attribute.

<!--Inside the method, the item is checked for validity and prior existence in the data store, and if no issues occur, it is added using the repository. Checking `ModelState.IsValid` performs [model validation](../mvc/models/validation.md), and should be done in every API method that accepts user input.-->
Inside the method, the item is checked for validity and prior existence in the data store, and if no issues occur, it is added using the repository. Checking `ModelState.IsValid` performs [model validation](../mvc/models/validation.md), and should be done in every API method that accepts user input.

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=25-46)]

<!--The sample uses an enum containing error codes that are passed to the mobile client:-->
The sample uses an enum containing error codes that are passed to the mobile client:

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=91-99)]

<!--Test adding new items using Postman by choosing the POST verb providing the new object in JSON format in the Body of the request. You should also add a request header specifying a `Content-Type` of `application/json`.-->
Test adding new items using Postman by choosing the POST verb providing the new object in JSON format in the Body of the request. You should also add a request header specifying a `Content-Type` of `application/json`.

![Postman console showing a POST and response](native-mobile-backend/_static/postman-post.png)

<!--The method returns the newly created item in the response.-->
The method returns the newly created item in the response.

<!--### Updating Items-->
### Updating Items

<!--Modifying records is done using HTTP PUT requests. Other than this change, the `Edit` method is almost identical to `Create`. Note that if the record isn't found, the `Edit` action will return a `NotFound` (404) response.-->
Modifying records is done using HTTP PUT requests. Other than this change, the `Edit` method is almost identical to `Create`. Note that if the record isn't found, the `Edit` action will return a `NotFound` (404) response.

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=48-69)]

<!--To test with Postman, change the verb to PUT and add the ID of the record being updated to the URL. Specify the updated object data in the Body of the request.-->
To test with Postman, change the verb to PUT and add the ID of the record being updated to the URL. Specify the updated object data in the Body of the request.

![Postman console showing a PUT and response](native-mobile-backend/_static/postman-put.png)

<!--This method returns a `NoContent` (204) response when successful, for consistency with the pre-existing API.-->
This method returns a `NoContent` (204) response when successful, for consistency with the pre-existing API.

<!--### Deleting Items-->
### Deleting Items

<!--Deleting records is accomplished by making DELETE requests to the service, and passing the ID of the item to be deleted. As with updates, requests for items that don't exist will receive `NotFound` responses. Otherwise, a successful request will get a `NoContent` (204) response.-->
Deleting records is accomplished by making DELETE requests to the service, and passing the ID of the item to be deleted. As with updates, requests for items that don't exist will receive `NotFound` responses. Otherwise, a successful request will get a `NoContent` (204) response.

[!code-csharp[Main](native-mobile-backend/sample/ToDoApi/src/ToDoApi/Controllers/ToDoItemsController.cs?range=71-88)]

<!--Note that when testing the delete functionality, nothing is required in the Body of the request.-->
Note that when testing the delete functionality, nothing is required in the Body of the request.

![Postman console showing a DELETE and response](native-mobile-backend/_static/postman-delete.png)

<!--## Common Web API Conventions-->
## Common Web API Conventions

<!--As you develop the backend services for your app, you will want to come up with a consistent set of conventions or policies for handling cross-cutting concerns. For example, in the service shown above, requests for specific records that weren't found received a `NotFound` response, rather than a `BadRequest` response. Similarly, commands made to this service that passed in model bound types always checked `ModelState.IsValid` and returned a `BadRequest` for invalid model types.-->
As you develop the backend services for your app, you will want to come up with a consistent set of conventions or policies for handling cross-cutting concerns. For example, in the service shown above, requests for specific records that weren't found received a `NotFound` response, rather than a `BadRequest` response. Similarly, commands made to this service that passed in model bound types always checked `ModelState.IsValid` and returned a `BadRequest` for invalid model types.

<!--Once you've identified a common policy for your APIs, you can usually encapsulate it in a [filter](../mvc/controllers/filters.md). Learn more about [how to encapsulate common API policies in ASP.NET Core MVC applications](https://msdn.microsoft.com/en-us/magazine/mt767699.aspx).-->
Once you've identified a common policy for your APIs, you can usually encapsulate it in a [filter](../mvc/controllers/filters.md). Learn more about [how to encapsulate common API policies in ASP.NET Core MVC applications](https://msdn.microsoft.com/en-us/magazine/mt767699.aspx).

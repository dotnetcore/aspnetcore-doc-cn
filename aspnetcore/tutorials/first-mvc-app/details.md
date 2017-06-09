---
title: 检查自动生成的Detail方法和Delete方法 | Microsoft 文档（民间汉化）
author: rick-anderson
description: 如何在一个 ASP.NET Core MVC 应用程序中编写控制器详细信息方法和视图
keywords: ASP.NET Core,
ms.author: riande
manager: wpickett
ms.date: 03/07/2017
ms.topic: get-started-article
ms.assetid: 870192b4-8d4f-45c7-8c14-83d02bc0ad79
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app/details
---
# 检查自动生成的Detail方法和Delete方法

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler](https://github.com/kiler398/) 

校对 [许登洋(Seay)](https://github.com/SeayXu) 、[姚阿勇(Dr.Yao)](https://github.com/YaoaY)

打开 Movie 控制器并查看 `Details` 方法:

[!code-csharp[Main](start-mvc/sample/MvcMovie/Controllers/MoviesController.cs?name=snippet_details)]

创建这个 action 方法的 MVC 基架引擎添加了一条注释给出了会调用这个方法的 HTTP 请求。在这个例子中是一个有三个URL段的GET请求， `Movies` 控制器， `Details`方法和 `id` 参数值。回顾一下 Startup 里定义的这些段。

[!code-csharp[Main](start-mvc/sample/MvcMovie/Startup.cs?highlight=5&name=snippet_1)]

EF 使用 `SingleOrDefaultAsync`方法更易于数据搜索。这个方法包含的一个重要的安全功能，就是在代码尝试用电影记录做任何操作之前确保查找方法已经找到了一条电影记录。例如，黑客可以把产生的链接 URL 从 `http://localhost:xxxx/Movies/Details/1`  改成类似于 `http://localhost:xxxx/Movies/Details/12345` （或者其他非实际电影记录的值），从而给网站带来错误。如果你不检查影片是否为空，应用程序将会抛出异常。

查看 `Delete` 方法和 `DeleteConfirmed` 方法

[!code-csharp[Main](start-mvc/sample/MvcMovie/Controllers/MoviesController.cs?name=snippet_delete)]

需要注意的是`HTTP GET Delete` 方法不删除指定的影片，它返回一个你可以提交 (HttpPost) 删除操作的  Movie 的视图。如果在对 GET 请求的响应中执行删除操作（或者编辑，创建，或任何其他更改数据的操作）将会引入一个安全漏洞。

真正删除数据的 `[HttpPost]` 方法被命名为 `DeleteConfirmed` ，给这个 HTTP POST 方法一个唯一的签名或名称。这两个方法的签名如下：

[!code-csharp[Main](start-mvc/sample/MvcMovie/Controllers/MoviesController.cs?name=snippet_delete2)]

[!code-csharp[Main](start-mvc/sample/MvcMovie/Controllers/MoviesController.cs?name=snippet_delete3)]


公共语言运行时（CLR）要求重载方法有一个唯一的参数签名（相同的方法名，但不同的参数列表）。然而，在这里你需要两个 `Delete` 方法 – 一个 GET 请求一个 POST 请求 – 并且它们都具有相同的参数签名。（它们都需要接收一个整数作为参数）。

有两种方案可以解决该问题，其中一种方法是，赋予方法不同的名称。这就是基架机制在前面的例子所做的事情。但是，这个方法引入了一个小问题： ASP.NET 利用名字将 URL 段映射到 action 方法，如果你重命名一个方法，路由通常将无法找到该方法。解决的办法就是你在例子中看到的，就是为 `DeleteConfirmed` 方法添加 `ActionName("Delete")` 特性。该特性为路由系统执行映射，所以一个 POST 请求的包含 /Delete/ 的 URL 会找到 `DeleteConfirmed`的方法。

对于具有相同名称和参数签名的方法，另一种常见的的解决办法是通过人为的改变 POST 方法的签名，即包含一个附加的（未使用）参数。这就是我们在前面文章中已经添加的 `notUsed` 的参数。在这里你可以对 `[HttpPost] Delete` 方法采用同样的解决办法：

```csharp
// POST: Movies/Delete/6
[ValidateAntiForgeryToken]
public async Task<IActionResult> Delete(int id, bool notUsed)
```

感谢您完成了 ASP.NET Core MVC 介绍教程的学习，我们会虚心听取您的建议，接下请学习 [MVC 以及 EF Core 入门](xref:data/ef-mvc/intro)。

>[!div class="step-by-step"]
[上一节](validation.md)

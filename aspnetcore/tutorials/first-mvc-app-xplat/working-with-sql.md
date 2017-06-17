---
title: 使用  SQLite | Microsoft 文档（中文文档）
author: rick-anderson
description: 如何在一个 ASP.NET Core MVC 应用程序中使用 SQLite
keywords: ASP.NET Core 中文文档,SQLite, SQL Server 
ms.author: riande
manager: wpickett
ms.date: 04/07/2017
ms.topic: get-started-article
ms.assetid: 1638d9b8-7c98-424d-8641-1638e23bf541
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/working-with-sql
---
# 使用  SQLite

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler)](https://github.com/kiler398/aspnetcore) 

 `MvcMovieContext` 类负责连接数据库并将 ``Movie`` 对象和数据记录进行映射。 *Startup.cs* 文件中，数据库上下文是在 ``ConfigureServices`` 方法中用 [Dependency Injection](xref:fundamentals/dependency-injection) 容器进行注册的。

[!code-csharp[Main](start-mvc/sample/MvcMovie/Startup.cs?name=snippet2&highlight=6-8)]


## SQLite

[SQLite](https://www.sqlite.org/) 官方网站的自我描述

> SQLite 是一个独立的、高可用的、嵌入式的、功能齐全的 SQL 数据库引擎。SQLite 是在世界上使用的最广泛 SQL 数据库引擎。

有很多可以下载的第三方的管理和查看 SQLite 数据库工具。 下图中的是 [DB Browser for SQLite](http://sqlitebrowser.org/)。如果你有其他熟悉 SQLite 工具，请留言介绍一下它的优点。

![SQLite DB Browser 工具显示 movie 数据](working-with-sql/_static/dbb.png)

## 填充数据库

在 *Models* 文件夹中创建一个名叫 `SeedData` 的新类。用以下代码替换生成的代码。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Models/SeedData.cs?name=snippet_1)]

注意，如果数据库中存在movies，填充初始化器返回。

```csharp
if (context.Movie.Any())
{
    return;   // DB has been seeded.
}
```

在 *Startup.cs* 文件中的 `Configure` 方法最后添加填充初始化器。

[!code-csharp[Main](start-mvc/sample/MvcMovie/Startup.cs?highlight=9&name=snippet_seed)]

### 测试一下

删除数据库中的所有记录 (这样 Seed 方法就会运行)。停止然后重新启动应用程序来填充数据库。
   
应用程序显示了填充数据。

![MVC Movie application open browser showing movie data](../../tutorials/first-mvc-app/working-with-sql/_static/m55.png)

>[!div class="step-by-step"]
[上一节 - 添加模型](adding-model.md)
[下一节 - 控制器与视图](controller-methods-views.md)

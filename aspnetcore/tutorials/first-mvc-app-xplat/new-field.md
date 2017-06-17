---
title: 添加新字段 | Microsoft 文档（中文文档）
author: rick-anderson
description: 
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 04/14/2017
ms.topic: get-started-article
ms.assetid: 1638bacf-fe7b-4b41-84b0-06a1574b7734
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/new-field
---
# 添加新字段

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler）](https://github.com/kiler398/) 

在这个教程里将会添加一个新的字段到  `Movies` 表，当我们改动数据库结构（添加一个字段）的时候，我们会删除数据库并重建一个新的库。这个工作流程在早期开发环境且没有任何生产数据要保留的情况下会工作的比较良好。

一旦你的应用程序部署了并且有生产数据希望保留了，当你修改数据库的结构的时候你就不能删除数据库了，Entity Framework 的 [Code First Migrations](http://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html) 允许你更新数据库结构并迁移数据库不丢失数据。当使用 SQL Server 数据库的时候，Migrations是一个不错的功能，但是 SQLlite 并不支持所有 migration 操作，所以只能使用一些简单的 migration，参考 [SQLite Limitations](https://docs.microsoft.com/ef/core/providers/sqlite/limitations)获取更多信息。

## 添加一个 Rating 字段到 Movie 模型

打开 *Models/Movie.cs* 文件，添加一个 `Rating` 属性：

[!code-csharp[Main](../first-mvc-app/start-mvc/sample/MvcMovie/Models/MovieDateRating.cs?highlight=11&range=7-18)]

因为你已经在 `Movie` 类添加了一个新的字段，你还需要更新绑定的白名单，这样这个新的属性将包括在内。为了 `Create` 和 `Edit` action 方法包含 `Rating` 属性需要更新`[Bind]`特性：

```csharp
[Bind("ID,Title,ReleaseDate,Genre,Price,Rating")]
   ```

为了把这个字段显示出来你必须更新视图，在浏览器视图中创建或者编辑一个新的 `Rating` 属性。

编辑 */Views/Movies/Index.cshtml* 文件并添加一个 `Rating` 字段：

[!code-HTML[Main](../first-mvc-app/start-mvc/sample/MvcMovie/Views/Movies/IndexGenreRating.cshtml?highlight=17,39&range=24-64)]

更新 */Views/Movies/Create.cshtml* 文件添加 `Rating` 字段。

应用程序无法工作，直到我们更新了数据库包含新的字段。如果你现在运行程序，你将得到下面的 `SqlException` ：

```
SqliteException: SQLite Error 1: 'no such column: m.Rating'.
```

你看到这个错误是因为更新过的 Movie 模型类与数据库中存在的 Movie 的结构是不同的。（数据库表中没有 Rating 列）
 
有以下几种方法解决这个错误：

1. Entity Framework 可以基于新的模型类自动删除并重建数据库结构。在开发环节的早期阶段，当你在测试数据库上积极做开发的时候，这种方式是非常方便的；它可以同时让你快速地更新模型类和数据库结构。但是，缺点是你会丢失数据库中的现有的数据 —— 因此你不想在生产数据库中使用这种方法！使用初始化器自动初始化数据库并填充测试数据，往往是开发应用程序的一个有效方式。

2. 显式修改现有数据库的结构使得它与模型类相匹配。这种方法的好处是，你可以保留录入过的数据。你可以手动修改或通过执行一个自动创建的数据库更改脚本进行变更。

3. 采用 Code First 迁移来更新数据库结构。

对于本教程，我们采用删除并重建数据库的方式来更新数据库结构。 在终端窗口中运行下面的命令来删除数据库：

`dotnet ef database drop`

更新 `SeedData` 类以便于为新的的字段提供填充值。下面展示一个变更的例子，你可能希望将这个变更应用到每个 `new Movie` 。

[!code-csharp[Main](../first-mvc-app/start-mvc/sample/MvcMovie/Models/SeedDataRating.cs?name=snippet1&highlight=6)]

将 `Rating` 字段添加到 `Edit`、`Details` 和 `Delete` 视图模板中。

运行应用程序并验证你可以用 `Rating` 字段 create/edit/display 电影。

>[!div class="step-by-step"]
[上一节 - 添加搜索](search.md)
[下一节 - 添加验证](validation.md)  

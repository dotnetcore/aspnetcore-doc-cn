---
title: Controller 方法与视图 | Microsoft 文档（中文文档）
author: rick-anderson
description: 学习控制器方法， 视图以及 DataAnnotations
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 04/07/2017
ms.topic: get-started-article
ms.assetid: c7313211-bb71-4adf-babe-8e72603cc0ce
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/controller-methods-views
---

# Controller 方法与视图

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler)](https://github.com/kiler398/aspnetcore) 

我们已经初步的创建了一个 movie 应用程序，但是展示并不理想。我们不希望看到 release date 字段显示时间并且 **ReleaseDate** 应该是两个单词。

![Index view: Release Date is one word (no space) and every movie release date shows a time of 12 AM](../../tutorials/first-mvc-app/working-with-sql/_static/m55.png)

打开 *Models/Movie.cs* 文件并添加下面高亮的代码行：

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Models/MovieDate.cs?name=snippet_1&highlight=2,11-12)]

编译并运行程序

<!-- include start
![MVC Movie application open browser showing movie data](../../tutorials/first-mvc-app/working-with-sql/_static/m55.png)

 -->

[!INCLUDE[adding-model](../../includes/mvc-intro/controller-methods-views.md)]

>[!div class="step-by-step"]
[上一节 - 使用 SQLite](working-with-sql.md)
[下一节 - 添加搜索](search.md)  
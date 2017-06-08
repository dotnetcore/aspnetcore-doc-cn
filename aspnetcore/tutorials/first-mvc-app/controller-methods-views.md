---
title: Controller 方法与视图 | Microsoft 文档（民间汉化）
author: rick-anderson
description: 学习控制器方法， 视图以及 DataAnnotations
keywords: ASP.NET Core,
ms.author: riande
manager: wpickett
ms.date: 03/07/2017
ms.topic: get-started-article
ms.assetid: c7313211-b271-4adf-bab8-8e72603cc0ce
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app/controller-methods-views
---

# Controller 方法与视图

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler](https://github.com/kiler398/) 

校对 [孟帅洋(书缘)](https://github.com/mengshuaiyang)、[张仁建(第二年.夏)](https://github.com/stoneniqiu)、[许登洋(Seay)](https://github.com/SeayXu) 、[姚阿勇(Dr.Yao)](https://github.com/YaoaY) 、[娄宇(Lyrics)](https://github.com/xbuilder) 

我们已经初步的创建了一个 movie 应用程序，但是展示并不理想。我们不希望看到 release date 字段显示时间并且 **ReleaseDate** 应该是两个单词。

![Index view: Release Date 是一个单次 (没有空格) 每个电影记录显示时间为 12 AM](working-with-sql/_static/m55.png)

打开 *Models/Movie.cs* 文件并添加下面高亮的代码行：

[!code-csharp[Main](start-mvc/sample/MvcMovie/Models/MovieDateWithExtraUsings.cs?name=snippet_1&highlight=13-14)]

右键点击红色波浪线代码行 **> Quick Actions**。

  ![右键菜单显示 **> Quick Actions and Refactorings**.](controller-methods-views/_static/qa.png)


点击 `using System.ComponentModel.DataAnnotations;`

  ![using System.ComponentModel.DataAnnotations 在代码顶部](controller-methods-views/_static/da.png)

  Visual studio 会自动添加 `using System.ComponentModel.DataAnnotations;`引用代码。

让我们移除多余的  `using`  引用代码。它们默认以灰色字体出现。右键点击 *Movie.cs* 文件 点击 **> Organize Usings > Remove Unnecessary Usings** 菜单。

![移除排序Usings](controller-methods-views/_static/rm.png)

更新后的代码：

[!code-csharp[Main](./start-mvc/sample/MvcMovie/Models/MovieDate.cs?name=snippet_1)]

<!-- include start -->

[!INCLUDE[adding-model](../../includes/mvc-intro/controller-methods-views.md)]

>[!div class="step-by-step"]
[上一节](working-with-sql.md)
[下一节](search.md)  

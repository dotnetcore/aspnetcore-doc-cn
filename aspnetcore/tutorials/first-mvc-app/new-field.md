---
title: 添加新字段 | Microsoft 文档（民间汉化）
author: rick-anderson
description: 
keywords: ASP.NET Core,
ms.author: riande
manager: wpickett
ms.date: 10/14/2016
ms.topic: get-started-article
ms.assetid: 16efbacf-fe7b-4b41-84b0-06a1574b95c2
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app/new-field
---
# 添加新字段

By [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [谢炀（Kiler](https://github.com/kiler398/) 

校对 [许登洋(Seay)](https://github.com/SeayXu) 、[姚阿勇(Dr.Yao)](https://github.com/YaoaY)

在这个章节你将使用 [Entity Framework](http://docs.efproject.net/en/latest/platforms/aspnetcore/new-db.html) Code First 迁移模型中新加的字段，从而将模型字段变更同步到数据库。

当你使用 EF Code First 模式自动创建一个数据库，Code First 模式添加到数据库的表将帮助你来跟踪数据库的数据结构是否和从它生成的模型类是同步的。如果不同步，EF 会抛出异常。这将有助于你在开发阶段就发现错误，否则可能要到运行时才能发现这个错误了（通过一个很隐蔽的错误信息）。

## 添加一个 Rating 字段到 Movie 模型

打开 *Models/Movie.cs* 文件，添加一个 `Rating` 属性：

[!code-csharp[Main](start-mvc/sample/MvcMovie/Models/MovieDateRating.cs?highlight=11&range=7-18)]

生成应用程序（Ctrl+Shift+B）。

因为你已经在 `Movie` 类添加了一个新的字段，你还需要更新绑定的白名单，这样这个新的属性将包括在内。为了 `Create` 和 `Edit` action 方法包含 `Rating` 属性需要更新`[Bind]`特性：

```csharp
[Bind("ID,Title,ReleaseDate,Genre,Price,Rating")]
   ```

为了把这个字段显示出来你必须更新视图，在浏览器视图中创建或者编辑一个新的 `Rating` 属性。

编辑 */Views/Movies/Index.cshtml* 文件并添加一个 `Rating` 字段：

[!code-HTML[Main](start-mvc/sample/MvcMovie/Views/Movies/IndexGenreRating.cshtml?highlight=17,39&range=24-64)]

更新 */Views/Movies/Create.cshtml* 文件添加 `Rating` 字段。你可以从上一个 "form group" 拷贝/粘帖以便于让智能感知帮助你更新字段。智能感知参考 [Tag Helpers](xref:mvc/views/tag-helpers/intro)。注意：Visual Studio 2017 RTM 版本里面你必须安装 [Razor Language Services](https://marketplace.visualstudio.com/items?itemName=ms-madsk.RazorLanguageServices) 来获取 Razor 智能提示，这个问题需要到后续更新中才能修复。

![The developer has typed the letter R for the attribute value of asp-for in the second label element of the view. An Intellisense contextual menu has appeared showing the available fields, including Rating, which is highlighted in the list automatically. When the developer clicks the field or presses Enter on the keyboard, the value will be set to Rating.](new-field/_static/cr.png)

应用程序无法工作，直到我们更新了数据库包含新的字段。如果你现在运行程序，你将得到下面的 `SqlException` ：

`SqlException: Invalid column name 'Rating'.`

你看到这个错误是因为更新过的 Movie 模型类与数据库中存在的 Movie 的结构是不同的。（数据库表中没有 Rating 列）
 
有以下几种方法解决这个错误：

1. Entity Framework 可以基于新的模型类自动删除并重建数据库结构。在开发环节的早期阶段，当你在测试数据库上积极做开发的时候，这种方式是非常方便的；它可以同时让你快速地更新模型类和数据库结构。但是，缺点是你会丢失数据库中的现有的数据 —— 因此你不想在生产数据库中使用这种方法！使用初始化器自动初始化数据库并填充测试数据，往往是开发应用程序的一个有效方式。

2. 显式修改现有数据库的结构使得它与模型类相匹配。这种方法的好处是，你可以保留录入过的数据。你可以手动修改或通过执行一个自动创建的数据库更改脚本进行变更。

3. 采用 Code First 迁移来更新数据库结构。

对于本教程，我们采用 Code First 迁移。

更新 `SeedData` 类以便于为新的的字段提供填充值。下面展示一个变更的例子，你可能希望将这个变更应用到每个 `new Movie` 。

[!code-csharp[Main](start-mvc/sample/MvcMovie/Models/SeedDataRating.cs?name=snippet1&highlight=6)]

生成解决方案，然后打开命令提示符。输入以下命令：

```console
dotnet ef migrations add Rating
dotnet ef database update
```
注意： 如果你遇到 `No executable found matching command "dotnet-ef"` 错误信息：

- 检查当前是否在项目目录 (包含 *.csproj* 文件的目录).
- 检查 *.csproj* 文件是否包含 "Microsoft.EntityFrameworkCore.Tools.DotNet" NuGet 包。
- 查看 [这篇博客](http://thedatafarm.com/data-access/no-executable-found-matching-command-dotnet-ef/) 获取解决问题的方案。

`migrations add` 命令通知数据库迁移框架检查 `Movie` 模型是否与当前 `Movie` 数据库表结构一致，如果不一致，就会创建新的必要的代码把数据库迁移到新的模型。“Rating” 名字可以是任意的，只是用于迁移文件。对于迁移操作采用有意义的名字是有帮助的。

如果在数据库中删除所有记录，数据库将会被初始化并添加 `Rating` 字段。你可以在浏览器或者 SSOX （SQL Server Object Explorer： SQL Server 对象资源浏览器）中点击删除链接。

运行应用程序并验证你可以用 `Rating` 字段 create/edit/display 电影。你还应该将 `Rating` 字段添加到 `Edit`、`Details` 和 `Delete` 视图模板中。

>[!div class="step-by-step"]
[上一节](search.md)
[下一节](validation.md)  

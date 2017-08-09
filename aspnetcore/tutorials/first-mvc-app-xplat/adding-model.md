---
title: 添加模型 | Microsoft 文档（中文文档）
author: rick-anderson
description: 如何在一个 ASP.NET Core MVC 应用程序中添加模型
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 03/30/2017
ms.topic: get-started-article
ms.assetid: 8dc28498-eeee-4666-b903-b593059e9f39
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/adding-model
---

[!INCLUDE[adding-model1](../../includes/mvc-intro/adding-model1.md)]

* 添加一个文件夹命名为 *Models*。
* 在 *Models* 文件夹中添加一个类命名为 *Movie.cs*。
* 添加下列代码到 *Models/Movie.cs* 文件：
   [!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Models/MovieNoEF.cs?name=snippet_1)]

`ID` 字段需要用来作为数据库主键

编译项目并且检查是否有错误，最后我们成功的把 **M**odel 添加到了你的 **M**VC 应用程序。

## 准备项目基架

- 添加下列高亮 NuGet 包到 *MvcMovie.csproj* 文件：
             
   [!code-csharp[Main](start-mvc/sample/MvcMovie/MvcMovie.csproj?highlight=5,17-18,21-)]

- 保存文件并在弹出 **Info** 信息 "There are unresolved dependencies" 的时候点击 **Restore**。
- 创建 *Models/MvcMovieContext.cs* 文并添加下面的 `MvcMovieContext` 类：

   [!code-csharp[Main](start-mvc/sample/MvcMovie/Models/MvcMovieContext.cs)]
   
- 打开 *Startup.cs* 文件并添加以下2个 usings 语句：

   [!code-csharp[Main](start-mvc/sample/MvcMovie/Startup.cs?name=snippet1&highlight=1,2)]

- 添加数据上下文到 *Startup.cs* 文件：

   [!code-csharp[Main](start-mvc/sample/MvcMovie/Startup.cs?name=snippet2&highlight=6-7)]

  This tells Entity Framework which model classes are included in the data model. You're defining one *entity set* of Movie objects, which will be represented in the database as a Movie table.

- 编译项目确认没有任何错误。

## 基架生成 MovieController

在项目文件打开终端窗口，运行一下命令:

```
dotnet restore
dotnet aspnet-codegenerator controller -name MoviesController -m Movie -dc MvcMovieContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries 
```

> [!NOTE]
> 如果你在运行基架命令的时候遇到错误信息， 请参考 [issue 444 in the scaffolding repository](https://github.com/aspnet/scaffolding/issues/444) 作为解决方案。

基架引擎产生以下输出：

* 一个电影控制器（Controller）（*Controllers/MoviesController.cs*）
* Create、Delete、Details、Edit 以及 Index 的 Razor 视图文件（Views/Movies）

自动创建 数据库上下文以及 [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete)（创建、读取、更新以及删除）Action 方法和视图（View）（自动创建 CRUD Action 方法和 View 视图被称为 *搭建基架（scaffolding）*）。很快你将拥有一个可以让你创建、查看、编辑以及删除电影条目的完整功能的 Web 应用程序。

### 创建数据库

你可以调用 `EnsureCreated` 方法来通知 EF Core 在数据库不存在的情况下去创建数据库。 

这个方法你只能在开发环境中显式的调用。 当应用程序第一次运行的时候它会创建符合你的数据模型的数据库， 当年你修改了数据模型， 然后删除了数据库， 下一次应用程序运行， EF Core 又会依据你的数据模型创建一个新的数据库。

这种方式在生产环境中使用中并不合适， 因为数据库中有了数据，而你又不想删除数据库导致数据丢失。 EF Core 包含一个 [Migrations](xref:data/ef-mvc/migrations) 功能来让你在数据模型变更的时候防止数据丢失，但是在本节教程中，你不会用到 Migrations 功能。 在[添加新字段](xref:tutorials/first-mvc-app-xplat/new-field)教程中，你将了解到更多关于数据模型变更的知识。

Create a *Models\DBinitialize.cs* file and add the following code:

<!-- todo - replace this with code import -->

```c#
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MvcMovie.Models
{
    public static class DBinitialize
    {
        public static void EnsureCreated(IServiceProvider serviceProvider)
        {
            var context = new MvcMovieContext(
                serviceProvider.GetRequiredService<DbContextOptions<MvcMovieContext>>());
            context.Database.EnsureCreated();
        }
    }
}
```

在 *Startup.cs* 文件的 `Configure` 方法中调用 `EnsureCreated` 方法。在方法的最后添加调用：

<!-- todo - replace this with code import -->

```c#
    app.UseMvc(routes =>
    {
        routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}");
    });

    DBinitialize.EnsureCreated(app.ApplicationServices);
}
```

[!INCLUDE[adding-model](../../includes/mvc-intro/adding-model3.md)]

这样的话，你就拥有了一个数据库，以及对应的显示，编辑，修改 和删除数据的页面。 在下一节的教程中， 来我们要处理数据库的问题了。

### 附加资源

* [Tag Helpers](xref:mvc/views/tag-helpers/intro)
* [全球化与本地化](xref:fundamentals/localization)

>[!div class="step-by-step"]
[上一节 - 添加视图](adding-view.md)
[下一节 - 处理 SQLite](working-with-sql.md)

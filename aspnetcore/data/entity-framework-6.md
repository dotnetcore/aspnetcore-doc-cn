---
title: 开始使用 ASP.NET Core 与 Entity Framework 6 | Microsoft 文档（中文文档）
author: tdykstra
description: 本文展示在 ASP.NET Core 中如何使用 Entity Framework 6。
keywords: ASP.NET Core 中文文档, Entity Framework, EF 6
ms.author: tdykstra
manager: wpickett
ms.date: 02/24/2017
ms.topic: article
ms.assetid: 016cc836-4c43-45a4-b9a7-9efaf53350df
ms.technology: aspnet
ms.prod: asp.net-core
uid: data/entity-framework-6
---
<!--# Getting started with ASP.NET Core and Entity Framework 6-->
# 开始使用 ASP.NET Core 与 Entity Framework 6

作者 [Paweł Grudzień](https://github.com/pgrudzien12)、 [Damien Pontifex](https://github.com/DamienPontifex)、 [Tom Dykstra](https://github.com/tdykstra)

翻译 [谢炀（Kiler)](https://github.com/kiler398/)  

<!--This article shows how to use Entity Framework 6 in an ASP.NET Core application.-->
本文展示在 ASP.NET Core 中如何使用 Entity Framework 6

<!--## Overview-->
## Overview

<!--To use Entity Framework 6, your project has to compile against the full .NET Framework, as Entity Framework 6 does not support .NET Core. If you need cross-platform features you will need to upgrade to [Entity Framework Core](https://docs.efproject.net).-->
为了使用 Entity Framework 6，你的项目需要基于完整 .NET Framework 编译，因为 Entity Framework 6 不支持 .NET Core。如果你需要跨平台功能，你需要升级到 [Entity Framework Core](https://docs.efproject.net)。

<!--The recommended way to use Entity Framework 6 in an ASP.NET Core application is to put the EF6 context and model classes in a class library project that targets the full framework. Add a reference to the class library from the ASP.NET Core project. See the sample [Visual Studio solution with EF6 and ASP.NET Core projects](https://github.com/aspnet/Docs/tree/master/aspnetcore/data/entity-framework-6/sample/).-->
在ASP.NET Core 应用中使用 Entity Framework 6 推荐的方式是将 EF6 上下文和模型类放在一个类库项目（ *.csproj* 项目文件）作用于整个框架。在 ASP.NET Core 项目添加对类库的引用。参考示例 [包含 ASP.NET Core 项目与 EF6 的Visual Studio解决方案](https://github.com/aspnet/Docs/tree/master/aspnetcore/data/entity-framework-6/sample/).


<!--You can't put an EF6 context in an ASP.NET Core project because .NET Core projects don't support all of the functionality that EF6 commands such as *Enable-Migrations* require.-->
向  ASP.NET Core 项目添加 EF6 上下文是不可行的，因为.NET Core 项目不支持所有的 EF6 命令功能，如 `Enable-Migrations` 。


<!--Regardless of project type in which you locate your EF6 context, only EF6 command-line tools work with an EF6 context. For example, `Scaffold-DbContext` is only available in Entity Framework Core. If you need to do reverse engineering of a database into an EF6 model, see [Code First to an Existing Database](https://msdn.microsoft.com/en-us/jj200620).-->
添加 EF6 上下文而不区分项目类型的话，则只有 EF6 命令行工具支持 EF6 上下文。例如，`Scaffold-DbContext` 只在 Entity Framework Core 中有效。如果你需要数据库逆向引擎将数据库转为 EF6 模型，参见 [已存在数据库情况的下 Code-First](https://msdn.microsoft.com/en-us/jj200620)


<!--## Reference full framework and EF6 in the ASP.NET Core project-->
## 在 ASP.NET Core 项目中引用完整框架和EF6

<!--Your ASP.NET Core project needs to reference the full .NET framework and EF6. For example, the *.csproj* file of your ASP.NET Core project will look similar to the following example (only relevant parts of the file are shown).-->
你的  ASP.NET Core  项目需要引用完整的 .NET framework 和 EF6 。例如， *.csproj* 将看起来类似于下面的示例（只展示了相关的文件部分）。

[!code-xml[](entity-framework-6/sample/MVCCore/MVCCore.csproj?range=3-9&highlight=2)]

<!--If you’re creating a new project, use the **ASP.NET Core Web Application (.NET Framework)** template.-->
如果你创建新项目，请使用 **ASP.NET Core Web Application (.NET Framework)** 模板。

<!--## Handle connection strings-->
## 处理连接字符串

<!--The EF6 command-line tools that you'll use in the EF6 class library project require a default constructor so they can instantiate the context. But you'll probably want to specify the connection string to use in the ASP.NET Core project, in which case your context constructor must have a parameter that lets you pass in the connection string. Here's an example.-->
你将要在 EF6 类库项目中使用的 EF6 命令行工具需要默认的构造函数，才能实例化上下文。但你可能想要指明在 ASP.NET Core 项目中使用的连接字符串，这种情况下，你的构造函数必需包含一个参数以能够传入连接字符串。这里有个例子。


[!code-csharp[](entity-framework-6/sample/EF6/SchoolContext.cs?name=snippet_Constructor)]

<!--Since your EF6 context doesn't have a parameterless constructor, your EF6 project has to provide an implementation of [IDbContextFactory](https://msdn.microsoft.com/library/hh506876). The EF6 command-line tools will find and use that implementation so they can instantiate the context. Here's an example.、-->
因为你的 EF6 上下文没有无参数构造函数，你的 EF6 项目需要提供 [IDbContextFactory](https://msdn.microsoft.com/library/hh506876) 的实现。EF6 命令行工具可以发现并使用这一实现，然后实例化上下文。这里有个例子。


[!code-csharp[](entity-framework-6/sample/EF6/SchoolContextFactory.cs?name=snippet_IDbContextFactory)]

<!--In this sample code, the `IDbContextFactory` implementation passes in a hard-coded connection string. This is the connection string that the command-line tools will use. You'll want to implement a strategy to ensure that the class library uses the same connection string that the calling application uses. For example, you could get the value from an environment variable in both projects.-->
这个示例代码中， `IDbContextFactory` 实现传入一个硬编码的连接字符串。这个连接字符串将被命令行工具使用。你将要实现一种策略来确保类库与调用它的应用使用相同的连接字符串。例如，你在同一个项目应该获取环境变量的值。


<!--## Set up dependency injection in the ASP.NET Core project-->
## 在 ASP.NET Core 项目中建立依赖注入

<!--In the Core project's *Startup.cs* file, set up the EF6 context for dependency injection (DI) in `ConfigureServices`. EF context objects should be scoped for a per-request lifetime.-->
在 Core 项目的 *Startup.cs* 文件中，利用 `ConfigureServices` 建立 EF6 上下文依赖注入。EF6 上下文对象作用域应该以单次请求为生命周期。

[!code-csharp[](entity-framework-6/sample/MVCCore/Startup.cs?name=snippet_ConfigureServices&highlight=5)]

<!--You can then get an instance of the context in your controllers by using DI. The code is similar to what you'd write for an EF Core context:-->
然后你可以在控制器中通过依赖注入（DI）得到上下文实例。代码与你为 EF Core 上下文所写的代码相似：

[!code-csharp[](entity-framework-6/sample/MVCCore/Controllers/StudentsController.cs?name=snippet_ContextInController)]

<!--## Sample application-->
## 示例应用程序

<!--For a working sample application, see the [sample Visual Studio solution](https://github.com/aspnet/Docs/tree/master/aspnetcore/data/entity-framework-6/sample/) that accompanies this article.-->
至于可运行的示例应用，参见  [简单的 Visual Studio 解决方案](https://github.com/aspnet/Docs/tree/master/aspnetcore/data/entity-framework-6/sample/)  ，它与本文相互映衬。

<!--This sample can be created from scratch by the following steps in Visual Studio:-->
这个示例可使用 Visual Studio 基于下面的步骤基于脚手架创建：

<!--* Create a solution.-->
* 创建解决方案。

<!--* **Add New Project > Web > ASP.NET Core Web Application (.NET Framework)**-->
* **添加新项目 > Web > ASP.NET Core Web 应用 (.NET Framework)**

<!--* **Add New Project > Windows Classic Desktop > Class Library (.NET Framework)**-->
* **添加新项目 > Windows > 类库**

<!--* In **Package Manager Console** (PMC) for both projects, run the command `Install-Package Entityframework`.-->
* 在两个项目的 **Package Manager Console** (PMC) , 执行命令  `Install-Package Entityframework`。

<!--* In the class library project, create data model classes and a context class, and an implementation of `IDbContextFactory`.-->
* 在类库项目，创建数据模型类和上下文类，和一个 `IDbContextFactory` 实现。

<!--* In PMC for the class library project, run the commands `Enable-Migrations` and `Add-Migration Initial`. If you have set the ASP.NET Core project as the startup project, add `-StartupProjectName EF6` to these commands.-->
* 在类库的 PMC 中执行 `Enable-Migrations` 和 `Add-Migration Initial`。 如果你将 ASP.NET Core 设置为启动项目, 添加 `-StartupProjectName EF6` 到这些命令中。

<!--* In the Core project, add a project reference to the class library project.-->
* 在 Core 项目中，添加一个对类库项目的引用。

<!--* In the Core project, in *Startup.cs*, register the context for DI.-->
* 在 Core 项目的 *Startup.cs*, 为依赖注入注册上下文。

<!--* In the Core project, in *appsettings.json*, add the connection string.-->
* 在 Core 项目中，添加连接字符串。

<!--* In the Core project, add a controller and view(s) to verify that you can read and write data. (Note that ASP.NET Core MVC scaffolding won't work with the EF6 context referenced from the class library.)-->
* 在 Core 项目中，添加控制器和视图，检验数据读写。（注意 ASP.NET Core MVC 脚手架不能与从类库中引用的 EF6 上下文运行。

<!--## Summary-->
## 结语

<!--This article has provided basic guidance for using Entity Framework 6 in an ASP.NET Core application.-->
这篇文章提供了在 ASP.NET Core 应用中使用 Entity Framework 6 基本的引导。

<!--## Additional Resources-->
## 附加资源

<!--* [Entity Framework - Code-Based Configuration](https://msdn.microsoft.com/en-us/data/jj680699.aspx)-->
* [Entity Framework - 基于代码的配置](https://msdn.microsoft.com/en-us/data/jj680699.aspx)


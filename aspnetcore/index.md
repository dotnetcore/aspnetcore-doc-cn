---
title: ASP.NET Core 介绍 | Microsoft 文档（民间汉化）
author: rick-anderson,江振宇(Kerry Jiang)
description: 
keywords: ASP.NET Core,
ms.author: riande
manager: wpickett
ms.date: 10/14/2016
ms.topic: article
ms.assetid: 1c501638-114a-4cd3-ad39-0a5790b4e764
ms.technology: aspnet
ms.prod: asp.net-core
uid: index
---
# ASP.NET Core 介绍


作者 [Daniel Roth](https://github.com/danroth27), [Rick Anderson](https://twitter.com/RickAndMSFT), 以及 [Shaun Luttin](https://twitter.com/dicshaunary)

翻译 [江振宇(Kerry Jiang)](http://github.com/kerryjiang)

校对 [许登洋(Seay)](https://github.com/SeayXu)、[魏美娟(初见)](http://github.com/ChujianA)、[姚阿勇(Mr.Yao)](https://github.com/YaoaY)

ASP.NET Core 是对 ASP.NET 的一次意义重大的重构。本文介绍了 ASP.NET Core 中的一些新概念，并且解释了它们如何帮助你开发现代的 Web 应用程序。

## 什么是 ASP.NET Core？


ASP.NET Core 是一个新的开源和跨平台的框架，用于构建如 Web 应用、物联网（IoT）应用和移动后端应用等连接到互联网的基于云的现代应用程序。ASP.NET Core 应用可运行于 [.NET Core](https://www.microsoft.com/net/core/platform) 和完整的 .NET Framework 之上。 构建它的目的是为那些部署在云端或者内部运行（on-premises）的应用提供一个优化的开发框架。它由最小开销的模块化的组件构成，因此在构建你的解决方案的同时可以保持灵活性。你可以在 Windows、Mac 和 Linux 上跨平台的开发和运行你的 ASP.NET Core 应用。 ASP.NET Core 开源在 [GitHub](https://github.com/aspnet/home) 上。
 
## 为什么构建 ASP.NET Core？

ASP.NET 的首个预览版作为 .NET Framework 的一部分发布于15年前。自那以后数百万的开发者用它开发和运行着众多非常棒的 Web 应用，而且在这么多年之间我们也为它增加和改进了很多的功能。

ASP.NET Core 有一些架构上的改变，这些改变会使它成为一个更为精简并且模块化的框架。ASP.NET Core 不再基于 *System.Web.dll* 。当前它基于一系列颗粒化的，并且良好构建的  [NuGet](http://www.nuget.org/) 包。这一特点能够让你通过仅仅包含需要的 NuGet 包的方法来优化你的应用。一个更小的应用程序接口通过“只为你需要的功能付出”（pay-for-what-you-use）的模型获得的好处包括更可靠的安全性、简化服务、改进性能和减少成本。

通过使用 ASP.NET Core，你可以获得以下改进：

* 统一的方式构建 web 界面 和 web APIs

* 集成 [现代的客户端开发框架](client-side/index.md) 以及开发流程

* 适用于云的，基于环境的 [配置系统](fundamentals/configuration.md)

* 内置 [依赖注入](fundamentals/dependency-injection.md) 

* 新型的轻量级的、模块化 HTTP 请求管道

* 运行于 IIS 或者自宿主（self-host）于你自己的进程的能力

* 基于支持真正的 side-by-side 应用程序版本化的 [.NET Core](https://microsoft.com/net/core) 构建

* 完全以 [NuGet](https://nuget.org) 包的形式发布

* 新的用于简化现代 web 开发的工具

* 可以在 Windows 、Mac 和 Linux 上构建和运行跨平台的 ASP.NET 应用

* 开源并且关注社区

## 使用 ASP.NET Core MVC 构建 web APIs 以及 web 界面



* 您可以构建出能够适应更广泛的客户端（包括浏览器和移动设备）的HTTP服务。内置 [多种数据格式和内容协商](mvc/models/formatting.md) 支持 。 ASP.NET Core 基于.NET Core 运行时上构建 Web API 和 RESTful 风格应用的理想平台。参考 [构建 web API](tutorials/index.md#building-web-apis)。 

* 你可以遵循 模型-视图-控制器 (MVC) 模式 来创建可测试以及设计良好的 Web应用程序。参考 [MVC](mvc/index.md) 以及 [测试](testing/index.md)。

* [Razor](http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c) 提供了一个创造性的语言来编写 [Views](mvc/views/index.md)

* [Tag Helpers](mvc/views/tag-helpers/intro.md) 能使用服务器端代码在 Razor 文件中部分创建以及呈现 HTML 元素

* [Model 绑定](mvc/models/model-binding.md) 自动把 HTTP 请求的数据映射到 action 方法参数

* [Model 验证](mvc/models/validation.md) 执行执行客户端以及服务器端验证

## 客户端开发

ASP.NET Core 设计上无缝的集成了各种客户端框架， 包括 [AngularJS](client-side/angular.md), [KnockoutJS](client-side/knockout.md) 以及 [Bootstrap](client-side/bootstrap.md)。参考 [客户端开发](client-side/index.md) 获取更多细节。

## 下一步

继续开始学习教程， 参考 [ASP.NET Core 指南](tutorials/index.md)

需要了解 ASP.NET Core 更深入的概念架构介绍，参考 [ASP.NET Core 原理](fundamentals/index.md)。

ASP.NET Core 应用程序可以使用 .NET Core 或者 .NET Framework 运行时。 更多信息请参考 [选择 .NET Core 还是 .NET Framework](https://docs.microsoft.com/dotnet/articles/standard/choosing-core-framework-server).

如果你想了解 ASP.NET Core 开发团队的进度和计划， 随时访问 [ASP.NET Community Standup](https://live.asp.net/).

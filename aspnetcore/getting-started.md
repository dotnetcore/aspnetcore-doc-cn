---
title: ASP.NET Core 入门 | Microsoft 文档（民间汉化）
author: rick-anderson
description: 
keywords: ASP.NET Core,
ms.author: riande
manager: wpickett
ms.date: 10/14/2016
ms.topic: get-started-article
ms.assetid: 73543e9d-d9d5-47d6-9664-17a9beea6cd3
ms.technology: aspnet
ms.prod: asp.net-core
uid: getting-started
---
# ASP.NET Core 入门

翻译 [娄宇(Lyrics)](http://github.com/xbuilder)

校对 [刘怡(AlexLEWIS)](http://github.com/alexinea) 

1.  安装 [.NET Core](https://microsoft.com/net/core)

2.  创建一个新的 .NET Core 项目：

    ```terminal
    mkdir aspnetcoreapp
    cd aspnetcoreapp
    dotnet new web
    ```
    
    注意: 
    - 在 macOS 或者 Linux 系统上，打开一个终端窗口。 在 Windows 系统上， 打开命令行窗口。
    - 早期版本的 .NET Core 需要一个 `t` 参数，比如   `dotnet new -t web`。 如果你运行 `dotnet new web` 出错， 安装最新版本的 [.NET Core](https://microsoft.com/net/core).  输入 `dotnet` (不需要参数) 将会显示 .NET Core 版本。

3.  还原包：

    ```terminal
    dotnet restore
    ```

4.  运行应用程序（ `dotnet run`  命令在应用程序过期（配置或代码发生变更）时重新生成它）：

    ```terminal
    dotnet run
    ```

5.  浏览 `http://localhost:5000`

## 下一步

想学习更多的入门指南， 参考 [ASP.NET Core 指南](tutorials/index.md)

想了解 ASP.NET Core 概念和架构，参看 [ASP.NET Core 介绍](index.md) 以及 [ASP.NET Core 原理](fundamentals/index.md).

ASP.NET Core 应用可以使用 .NET Core 或者 .NET Framework 运行时。更多信息，请参考 [选择 .NET Core 还是 .NET Framework](https://docs.microsoft.com/dotnet/articles/standard/choosing-core-framework-server).

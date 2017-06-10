---
title: 在 Mac 或者 Linux 上开发 ASP.NET Core MVC | Microsoft 文档（民间汉化）
author: rick-anderson
description: 在 Mac 或者上开发 ASP.NET Core MVC 
keywords: ASP.NET Core, MVC, VS Code, Visual Studio Code, Mac, Linux
ms.author: riande
manager: wpickett
ms.date: 03/07/2017
ms.topic: get-started-article
ms.assetid: 1d18b589-1638-4dc6-1638-fb0f41998d78
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/start-mvc
---
# 在 Mac 或者 Linux 上开发 ASP.NET Core MVC

作者 [Rick Anderson](https://twitter.com/RickAndMSFT)
翻译 [谢炀（Kiler](https://github.com/kiler398/) 


这篇教程将告诉你如何使用 [Visual Studio Code](https://code.visualstudio.com) (VS Code) 构建一个 ASP.NET Core MVC Web 应用程序的基础知识。本教程你假定熟悉 VS Code，参考 [Getting started with VS Code](https://code.visualstudio.com/docs) 以及 [Visual Studio Code 帮助](#visual-studio-code-help) 获取更多信息。

## 安装 VS Code 以及 .NET Core

下载并安装：
- [.NET Core](https://microsoft.com/net/core)
- [VS Code](https://code.visualstudio.com)
- VS Code [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

## 使用 dotnet 命令行创建 Web 应用程序

在终端窗口，运行一下命令：

```console
mkdir MvcMovie
cd MvcMovie
dotnet new mvc
```

在 Visual Studio Code 中打开 *MvcMovie* 文件夹并选择 *Startup.cs* 文件。

- 提示 **Warn** 信息 "Required assets to build and debug are missing from 'MvcMovie'. Add them?" 的时候选择 **Yes** 
- 提示 **Info** 信息 "There are unresolved dependencies" 选择 **Restore** 

![VS Code with Warn Required assets to build and debug are missing from 'MvcMovie'. Add them? Don't ask Again, Not Now, Yes and also Info - there are unresolved dependencies  - Restore - Close](../web-api-vsc/_static/vsc_restore.png)

点击 **Debug** (F5) 编译并运行程序。

![运行程序](../first-mvc-app/start-mvc/_static/1.png)

VS Code 启动 [Kestrel](xref:fundamentals/servers/kestrel) web 服务器来运行你的应用程序。注意地址栏显示的是 `localhost:5000` 而不是像 `example.com` 这类地址。那是因为  `localhost` 总是指向本地计算机. 那是因为 `localhost`  总是指向本地计算机。

默认模版提供了可点击的 **Home, About** 以及 **Contact** 链接。 上面的浏览器图片没有显示这些链接。根据您的浏览器的尺寸，您可能需要点击导航图标来显示他们。

![navigation icon in upper right](../first-mvc-app/start-mvc/_static/2.png)

我们将在本教程下一节中学习 MVC 并尝试写些代码。

## Visual Studio Code 帮助

- [入门](https://code.visualstudio.com/docs)
- [调试](https://code.visualstudio.com/docs/editor/debugging)
- [集成终端](https://code.visualstudio.com/docs/editor/integrated-terminal)
- [快捷键](https://code.visualstudio.com/docs/getstarted/keybindings#_keyboard-shortcuts-reference)

  - [Mac 快捷键](https://go.microsoft.com/fwlink/?linkid=832143)
  - [Linux 快捷键](https://go.microsoft.com/fwlink/?linkid=832144)
  - [Windows 快捷键](https://go.microsoft.com/fwlink/?linkid=832145)

>[!div class="step-by-step"]
[下一节 - 添加控制器](adding-controller.md)
---
title: 用 ASP.NET Core MVC 和 Visual Studio Code 创建你的第一个 Web API | Microsoft 文档（中文文档）
author: rick-anderson
description: 在  Linux, macOS, 以及 Windows平台 用 ASP.NET Core MVC 和 Visual Studio Code 创建你的第一个 Web API
keywords: ASP.NET Core 中文文档, WebAPI, Web API, REST, Mac, Linux,HTTP, Service, HTTP Service
ms.author: riande
manager: wpickett
ms.date: 5/24/2017
ms.topic: get-started-article
ms.assetid: 830b4bf5-dd14-423e-9f59-764a6f13a8f6
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/web-api-vsc
---

# 在  Linux, macOS, 以及 Windows平台 用 ASP.NET Core MVC 和 Visual Studio Code 创建你的第一个 Web API

<!-- WARNING: The code AND images in this doc are used by uid: tutorials/web-api-vsc, tutorials/first-web-api-mac and tutorials/first-web-api. If you change any code/images in this tutorial, update uid: tutorials/web-api-vsc -->

[!INCLUDE[template files](../includes/webApi/intro.md)]

<!--## Set up your development environment-->
## 设置好你的开发环境

<!--Download and install:
- [.NET Core](https://microsoft.com/net/core)
- [Visual Studio Code](https://code.visualstudio.com)
- Visual Studio Code [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)-->
下载并安装:
- [.NET Core](https://microsoft.com/net/core)
- [Visual Studio Code](https://code.visualstudio.com)
- Visual Studio Code [C# 插件](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)

<!--## Create the project-->
## 创建项目

<!--From a console, run the following commands:-->
在命令行控制台， 运行一下命令：

```console
mkdir TodoApi
cd TodoApi
dotnet new webapi
```

<!--Open the *TodoApi* folder in Visual Studio Code (VS Code) and select the *Startup.cs* file.-->
在 Visual Studio Code (VS Code) 中打开 *TodoApi* 目录并选择 *Startup.cs* 文件。

<!--- Select **Yes** to the **Warn** message "Required assets to build and debug are missing from 'TodoApi'. Add them?"
- Select **Restore** to the **Info** message "There are unresolved dependencies".-->
- 当提示 **Warn** 信息 "Required assets to build and debug are missing from 'TodoApi'. Add them?" 的时候选择  **Yes**。
- 当提示 **Info** 信息 "There are unresolved dependencies" 的时候选择  **Restore**。

<!-- uid: tutorials/first-mvc-app-xplat/start-mvc uses the pic below. If you change it, make sure it's consistent -->

![VS Code with Warn Required assets to build and debug are missing from 'TodoApi'. Add them? Don't ask Again, Not Now, Yes and also Info - there are unresolved dependencies  - Restore - Close](web-api-vsc/_static/vsc_restore.png)

<!--Press **Debug** (F5) to build and run the program. In a browser navigate to http://localhost:5000/api/values . The following is displayed:-->
按下 **Debug** (F5) 编译并运行程序。在浏览器中导航到 http://localhost:5000/api/values 。显示一下信息：

`["value1","value2"]`

<!--See [Visual Studio Code help](#visual-studio-code-help) for tips on using VS Code.-->
参考 [Visual Studio Code 帮助](#visual-studio-code-help) 了解更多关于 VS Code 的使用技巧。

<!--## Add support for Entity Framework Core-->
## 添加 Entity Framework Core 支持

<!--Edit the *TodoApi.csproj* file to install the [Entity Framework Core InMemory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) database provider. This database provider allows Entity Framework Core to be used with an in-memory database.-->
编辑 *TodoApi.csproj* 文件添加 [Entity Framework Core InMemory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/) database provider。这个 database provider 允许 Entity Framework Core 使用内存数据库。

[!code-xml[Main](web-api-vsc/sample/TodoApi/TodoApi.csproj?highlight=12)]

<!--Run `dotnet restore` to download and install the EF Core InMemory DB provider. You can run `dotnet restore` from the terminal or enter `⌘⇧P` (macOS) or `Ctrl+Shift+P` (Linux) in VS Code and then type **.NET**. Select **.NET: Restore Packages**.-->
运行 `dotnet restore` 下载并安装 EF Core InMemory DB provider。你可以在终端运行 `dotnet restore` 命令或者在 VS Code 里面输入 `⌘⇧P` (macOS) 或者 `Ctrl+Shift+P` (Linux) 并输入**.NET**。 选择 **.NET: Restore Packages**。

<!--## Add a model class-->
## 添加模型类

<!--A model is an object that represents the data in your application. In this case, the only model is a to-do item.-->
模型表示应用程序中的数据的对象。在本示例中，唯一使用到的模型是一个 to-do 项

<!--Add a folder named *Models*. You can put model classes anywhere in your project, but the *Models* folder is used by convention.-->
添加一个名为  *Models* 的目录. 你可以把模型类放到项目的任何地方, 但是 *Models* 是约定的默认目录 。

<!--Add a `TodoItem` class with the following code:-->
使用以下代码添加一个 `TodoItem` 类：

[!code-csharp[Main](first-web-api/sample/TodoApi/Models/TodoItem.cs)]

<!--## Create the database context-->
## 创建数据库上下文

<!--The *database context* is the main class that coordinates Entity Framework functionality for a given data model. You create this class by deriving from the `Microsoft.EntityFrameworkCore.DbContext` class.-->
*数据库上下文* 是 Entity Framework 用来协调数据模型的主要类。您可以从 `Microsoft.EntityFrameworkCore.DbContext` 类派生来创建此类。

<!--
Add a `TodoContext` class in the *Models* folder:-->
在 *Models* 文件夹中添加 `TodoContext` 类：

[!code-csharp[Main](first-web-api/sample/TodoApi/Models/TodoContext.cs)]

[!INCLUDE[Register the database context](../includes/webApi/register_dbContext.md)]

<!--## Add a controller-->
## 添加控制器

<!--In the *Controllers* folder, create a class named `TodoController`. Add the following code:-->
在 *Controllers* 目录中，创建名为 `TodoController` 的类。添加以下代码：

[!INCLUDE[code and get todo items](../includes/webApi/getTodoItems.md)]

<!--### Launch the app-->
### 运行程序

<!--In VS Code, press F5 to launch the app. Navigate to  http://localhost:5000/api/todo   (The `Todo` controller we just created).-->
在 VS Code 中， 按下 F5 运行程序。导航到  http://localhost:5000/api/todo   (我们刚刚创建的 `Todo` 控制)。
[!INCLUDE[last part of web API](../includes/webApi/end.md)]

<!--## Visual Studio Code help-->
## Visual Studio Code 帮助

<!--- [Getting started](https://code.visualstudio.com/docs)
- [Debugging](https://code.visualstudio.com/docs/editor/debugging)
- [Integrated terminal](https://code.visualstudio.com/docs/editor/integrated-terminal)
- [Keyboard shortcuts](https://code.visualstudio.com/docs/getstarted/keybindings#_keyboard-shortcuts-reference)-->
- [入门](https://code.visualstudio.com/docs)
- [调试](https://code.visualstudio.com/docs/editor/debugging)
- [集成终端](https://code.visualstudio.com/docs/editor/integrated-terminal)
- [键盘快捷键](https://code.visualstudio.com/docs/getstarted/keybindings#_keyboard-shortcuts-reference)

  <!--- [Mac keyboard shortcuts](https://go.microsoft.com/fwlink/?linkid=832143)
  - [Linux keyboard shortcuts](https://go.microsoft.com/fwlink/?linkid=832144)
  - [Windows keyboard shortcuts](https://go.microsoft.com/fwlink/?linkid=832145)-->
  - [Mac 键盘快捷键](https://go.microsoft.com/fwlink/?linkid=832143)
  - [Linux 键盘快捷键](https://go.microsoft.com/fwlink/?linkid=832144)
  - [Windows 键盘快捷键](https://go.microsoft.com/fwlink/?linkid=832145)

[!INCLUDE[后续章节](../includes/webApi/next.md)]



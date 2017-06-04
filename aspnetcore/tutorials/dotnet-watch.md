---
title: 使用 dotnet watch 开发 ASP.NET Core 应用程序 | Microsoft 文档（民间汉化）
author: rick-anderson
description: 展示如何使用 dotnet watch.
keywords: ASP.NET Core, 使用 dotnet watch
ms.author: riande
manager: wpickett
ms.date: 03/09/2017
ms.topic: article
ms.assetid: 563ffb3f-d369-4aa5-bf0a-7300b4e7832c
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/dotnet-watch
---
# 使用 dotnet watch 开发 ASP.NET Core 应用程序

 
作者 [Rick Anderson](https://twitter.com/RickAndMSFT) 、 [Victor Hurdugaci](https://twitter.com/victorhurdugaci)

翻译 [谢炀（Kiler)](https://github.com/kiler398/aspnetcore)  

校对 [刘怡(AlexLEWIS)](https://github.com/alexinea)、[许登洋(Seay)](https://github.com/SeayXu)

`dotnet watch` 是一个开发阶段在源文件发生变动的情况下使用 `dotnet` 命令的工具。 当代码发生变动的时候可以用来执行编译，运行测试，或者发布操作。

在本教程中，我们将使用一个现有的计算两个数字之和以及乘积的 WebApi 应用程序。示例应用程序故意包含一个错误，作为本教程的一部分我们会修复它。 

开始下载 [示例应用程序](https://github.com/aspnet/Docs/tree/master/aspnetcore/tutorials/dotnet-watch/sample)。示例程序包含两个项目，  `WebApp`（Web 应用程序）以及 `WebAppTests` （Web 应用程序配套的单元测试项目）

在命令行控制台中，进入下载示例程序的目录并且运行下述命令：

- `dotnet restore`
- `dotnet run`


控制台输出将显示如下信息，表明该应用程序正在运行并等待请求：

```console
$ dotnet run
Hosting environment: Production
Content root path: C:/Docs/aspnetcore/tutorials/dotnet-watch/sample/WebApp
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```


在 Web 浏览器中，导航到 `http://localhost:5000/api/math/sum?a=4&b=5` 页面你会看到结果 `9` 。
 
如果你导航到乘法API页面 (`http://localhost:5000/api/math/product?a=4&b=5`) 页面，你期望得到结果 `20`。但是实际上还是返回了 `9`，我们稍后会修复这个问题 。

## 在项目中添加 `dotnet watch` 

- 添加 `Microsoft.DotNet.Watcher.Tools` 到 *.csproj* 文件：
 ```xml
 <ItemGroup>
   <DotNetCliToolReference Include="Microsoft.DotNet.Watcher.Tools" Version="1.0.0" />
 </ItemGroup> 
 ```

- 运行 `dotnet restore`。

## 以 `dotnet watch` 的方式运行 `dotnet` 命令

任何与 `dotnet` 有关的命令都可以以 `dotnet watch` 这样的方式运行：例如：

| 命令 | watch 方式运行 |
| ---- | ----- |
| dotnet run | dotnet watch run |
| dotnet run -f net451 | dotnet watch run -f net451 |
| dotnet run -f net451 -- --arg1 | dotnet watch run -f net451 -- --arg1 |
| dotnet test | dotnet watch test |

在 `WebApp` 目录里面运行 `dotnet watch run` 。 控制台输出将显示如下信息，表明  `watch`（监控工作）已经开始了。

## 在 `dotnet watch` 模式修改代码

确认 `dotnet watch` 模式运行中。

修复 `MathController` 中的 `Product` 方法的 bug ，让它返回乘积结果而不是总和。

```csharp
public static int Product(int a, int b)
{
  return a * b;
} 
```
保存文件。 控制台输出将显示如下信息，表明 `dotnet watch` 检测到文件的改变并重启了应用程序。

验证 `http://localhost:5000/api/math/product?a=4&b=5` 链接返回正确的结果。

## 使用 `dotnet watch` 运行测试

- 修改 `MathController` 中的 `Product` 方法回到之前的返回总和结果并且保存文件。
- 在 windows 命令窗口，导航到 `WebAppTests` 目录。
- 运行 `dotnet restore`
- 运行 `dotnet watch test`。你会看到输出显示测试失败并且监控器在等待文件改变：

 ```console
 Total tests: 2. Passed: 1. Failed: 1. Skipped: 0.
 Test Run Failed.
  ```
- 修复 `Product` 方法代码让他返回乘积结果。保存文件。

`dotnet watch` 侦测到文件改变并且返回测试。 命令行输出显示测试通过。

## dotnet-watch GitHub 开源代码

dotnet-watch 是做为 GitHub 上的 [DotNetTools repository](https://github.com/aspnet/DotNetTools/tree/dev/src/Microsoft.DotNet.Watcher.Tools) 开源项目中的一部分。

[dotnet-watch 说明](https://github.com/aspnet/DotNetTools/blob/dev/src/Microsoft.DotNet.Watcher.Tools/README.md) 中的 [MSBuild 章节](https://github.com/aspnet/DotNetTools/blob/dev/src/Microsoft.DotNet.Watcher.Tools/README.md#msbuild) 中详细解释了如何把 dotnet-watch 配置到 MSBuild 项目文件中来监控文件的变化。 [dotnet-watch 说明](https://github.com/aspnet/DotNetTools/blob/dev/src/Microsoft.DotNet.Watcher.Tools/README.md) 一文包含了所有本教程没有提到的 dotnet-watch 信息。

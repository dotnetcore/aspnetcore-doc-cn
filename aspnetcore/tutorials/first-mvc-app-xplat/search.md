---
title: 添加搜索 | Microsoft 文档（中文文档）
author: rick-anderson
description: 如何在一个 ASP.NET Core MVC 应用程序中添加搜索功能
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 04/07/2017
ms.topic: get-started-article
ms.assetid: d69e5529-ffff-4628-855d-200206d96269
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app-xplat/search
---

[!INCLUDE[adding-model](../../includes/mvc-intro/search1.md)]

注意： SQLlite 是区分大小写的，所以你必须搜索 "Ghost" 而不是 "ghost"。

[!INCLUDE[adding-model](../../includes/mvc-intro/search2.md)]

修改 *Views\movie\Index.cshtml* Razor 视图中  `<form>` 标签指定 `method="get"`：

```html
<form asp-controller="Movies" asp-action="Index" method="get">
```

[!INCLUDE[adding-model](../../includes/mvc-intro/search3.md)]

>[!div class="step-by-step"]
[上一节 - 控制器方法与视图](controller-methods-views.md)
[下一节 - 添加新字段](new-field.md)  

---
title: 添加搜索 | Microsoft 文档（中文文档）
author: rick-anderson
description: 如何在一个 ASP.NET Core MVC 应用程序中添加搜索功能
keywords: ASP.NET Core 中文文档,
ms.author: riande
manager: wpickett
ms.date: 03/07/2017
ms.topic: get-started-article
ms.assetid: d69e5529-8ef6-4628-855d-200206d962b9
ms.technology: aspnet
ms.prod: asp.net-core
uid: tutorials/first-mvc-app/search
---

[!INCLUDE[adding-model](../../includes/mvc-intro/search1.md)]

你可以使用 **rename** 操作快速的把 `searchString` 参数重命名为 `id` ，在 `searchString` 上右击  **> Rename** 。

![Contextual menu](search/_static/rename.png)

会被重命名的代码会高亮显示。

![Code editor showing the variable highlighted throughout the Index ActionResult method](search/_static/rename2.png)

修改参数为 `id` ，其他引用到 `searchString` 的地方也会自动变更为 `id` 。

![Code editor showing the variable has been changed to id](search/_static/rename3.png)

[!INCLUDE[adding-model](../../includes/mvc-intro/search2.md)]

注意智能感知将帮我们更新标签。

![Intellisense contextual menu with method selected in the list of attributes for the form element](search/_static/int_m.png)

![Intellisense contextual menu with get selected in the list of method attribute values](search/_static/int_get.png)

请注意， `<form>` 标签中的专有标记。这种专有标记表示的标签是由 [Tag Helpers](../../mvc/views/tag-helpers/intro.md) 支持的。

![form tag with purple text](search/_static/th_font.png)

[!INCLUDE[adding-model](../../includes/mvc-intro/search3.md)]

>[!div class="step-by-step"]
[上一节](controller-methods-views.md)
[下一节](new-field.md)  

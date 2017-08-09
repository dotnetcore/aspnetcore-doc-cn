---
title: ASP.NET Core MVC 与 EF Core - 高级 - 10 of 10 | Microsoft 文档（中文文档）
author: tdykstra
description: 这篇教程则将介绍几种实用主题，它们将在你掌握利用 Entity Framework Core 开发 ASP.NET Web 应用程序的基础后非常管用。
keywords: ASP.NET Core 中文文档, Entity Framework Core, raw sql, examine sql, repository pattern, unit of work pattern, automatic change detection, existing database
ms.author: tdykstra
manager: wpickett
ms.date: 03/15/2017
ms.topic: get-started-article
ms.assetid: 92a2986a-d005-4ff6-9559-6657fd466bb7
ms.technology: aspnet
ms.prod: asp.net-core
uid: data/ef-mvc/advanced
---

# 高级主题 - EF Core 与 ASP.NET Core MVC 教程 (10 of 10)

作者 [Tom Dykstra](https://github.com/tdykstra) 和 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [刘怡(AlexLEWIS/Forerunner)](http://github.com/alexinea) 

<!--The Contoso University sample web application demonstrates how to create ASP.NET Core 1.0 MVC web applications using Entity Framework Core 1.0 and Visual Studio 2015. For information about the tutorial series, see [the first tutorial in the series](intro.md).-->
Contoso 大学 Web应用程序演示了如何使用 Entity Framework Core 1.1 以及 Visual Studio 2017 来创建 ASP.NET Core 1.1 MVC Web 应用程序。更多信息请参考 [第一节教程](intro.md).

<!--In the previous tutorial you implemented table-per-hierarchy inheritance. This tutorial introduces several topics that are useful to be aware of when you go beyond the basics of developing ASP.NET web applications that use Entity Framework Core.-->
在前一篇教程中我们实现了基于 TPH 的继承。而这篇教程则将介绍几种实用主题，它们将在你掌握利用 Entity Framework Core 开发 ASP.NET Web 应用程序的基础后非常管用。

<!--## Raw SQL Queries-->
## 原生 SQL 查询

<!--One of the advantages of using the Entity Framework is that it avoids tying your code too closely to a particular method of storing data. It does this by generating SQL queries and commands for you, which also frees you from having to write them yourself. But there are exceptional scenarios when you need to run specific SQL queries that you have manually created. For these scenarios, the Entity Framework Code First API includes methods that enable you to pass SQL commands directly to the database. You have the following options in EF Core 1.0:-->
使用 Entity Framework 的一大好处在于能避免代码与存储数据的特定方法之间的耦合。通过生成 SQL 查询和命令，我们也可以避免直接编写这些 SQL 语句。但是也有例外场景，当你需要运行特定 SQL 查询的时候，你就必须手工创建 SQL 查询了。在这些场景下，Entity Framework Code First API 包含直接传递 SQL 命令的方法。在 EF Core 1.0 中你有这些选择：

<!--* Use the `DbSet.FromSql` method for queries that return entity types. The returned objects must be of the type expected by the `DbSet` object, and they are automatically tracked by the database context unless you [关闭跟踪](crud.md#no-tracking-queries).-->
* 为返回实体类型的查询使用 `DbSet.FromSql` 方法。所返回的对象必须是 `DbSet` 对象期望获得的类型，并且它们将自动被数据库上下文跟踪，出给你 [关闭跟踪](crud.md#no-tracking-queries)。

<!--* Use the `Database.ExecuteSqlCommand` for non-query commands.-->
* 为非查询命令使用  `Database.ExecuteSqlCommand` 。

<!--If you need to run a query that returns types that aren't entities, you can use ADO.NET with the database connection provided by EF. The returned data isn't tracked by the database context, even if you use this method to retrieve entity types.-->
如果你需要运行能返回非实体类型结果的查询，你可以从 EF 中获得数据库连接，而后通过 ADO.NET 来获取。返回的数据将不会被数据库上下文跟踪，即使你检索的是实体类型。

<!--As is always true when you execute SQL commands in a web application, you must take precautions to protect your site against SQL injection attacks. One way to do that is to use parameterized queries to make sure that strings submitted by a web page can't be interpreted as SQL commands. In this tutorial you'll use parameterized queries when integrating user input into a query.-->
当你在 Web 应用程序中执行 SQL 命令时，你必须确保采取预防措施保护网站被执行 SQL 注入攻击。其中一种策略是使用参数化查询以确保通过网页提交的字符串不能被解释为 SQL 命令。在本教程中你将使用参数化查询来之兴用于输入的请求。

<!--## Call a query that returns entities-->
## 调用返回实体的请求

<!--The `DbSet<TEntity>` class provides a method that you can use to execute a query that returns an entity of type `TEntity`. To see how this works you'll change the code in the `Details` method of the Department controller.-->
`DbSet<TEntity>` c类提供了执行查询并返回  `TEntity`  实体结果的方法。你可以通过修改 Department 控制器中的 `Details` 方法来查看它的工作原理。

<!--In *DepartmentsController.cs*, in the `Details` method, replace the code that retrieves a department with a `FromSql` method call, as shown in the following highlighted code:-->
在 *DepartmentController.cs* 文件的 `Details` 方法中，将代码替换为通过调用 `FromSql` 方法检索系，如下例所示（注意代码高亮处）：

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_RawSQL&highlight=8,9,10,13)]

<!--To verify that the new code works correctly, select the **Departments** tab and then **Details** for one of the departments.-->
为了验证新代码能正常工作，请选择 **Departments** 标签，然后随意一个系的 **Details**。

![Department Details](advanced/_static/department-details.png)

## Call a query that returns other types
## 调用返回其它类型的请求

<!--Earlier you created a student statistics grid for the About page that showed the number of students for each enrollment date. You got the data from the Students entity set (`_context.Students`) and used LINQ to project the results into a list of `EnrollmentDateGroup` view model objects. Suppose you want to write the SQL itself rather than using LINQ. To do that you need to run a SQL query that returns something other than entity objects. In EF Core 1.0, one way to do that is write ADO.NET code and get the database connection from EF.-->
之前你已经在 About 页中创建了用于显示每个学生的入学时间的统计表。你从 Students 实体集（`_context.Students`）中获取数据并使用 LINQ 将结果转换为  `EnrollmentDateGroup` 视图模型对象列表。假设你想让它自己编写 SQL 语句而不是使用 LINQ，那么你需要执行一个 SQL 查询并返回一个非实体对象。在 EF Core 1.0 中，有一个办法做到这一点，就是从 EF 中取出数据库连接，然后编写 ADO.NET 代码。

<!--In *HomeController.cs*, replace the `About` method with the following code:-->
在 HomeController.cs 文件中，用 ADO.NET 代码替换 `About` 方法中的 LINQ 语句，并如下高亮处代码所示：

[!code-csharp[Main](intro/samples/cu/Controllers/HomeController.cs?name=snippet_UseRawSQL&highlight=3-32)]

<!--Add a using statement:-->
添加一个 using 语句：

[!code-csharp[Main](intro/samples/cu/Controllers/HomeController.cs?name=snippet_Usings2)]

<!--Run the About page. It displays the same data it did before.-->
运行 About 页面。它所显示的页面与更改前的别无二致。

![About page](advanced/_static/about.png)

<!--## Call an update query-->
## 调用 Update 查询

<!--Suppose Contoso University administrators want to perform global changes in the database, such as changing the number of credits for every course. If the university has a large number of courses, it would be inefficient to retrieve them all as entities and change them individually. In this section you'll implement a web page that enables the user to specify a factor by which to change the number of credits for all courses, and you'll make the change by executing a SQL UPDATE statement. The web page will look like the following illustration:-->
假设 Contoso University 的管理员需要在数据库内做全局变更，比如修改每门课程的学分。如果大学里有数量庞大的课程，那么将这些课程以实体的方式逐个检索出并挨个更新它们将是何等低效。在本节中你将实现一个页面，它能让用户为所有课程的学分变更指定一个因数，然后通过执行 SQL UPDATE 语句更新数据。这个网页看上去如下图所示：


![Update Course Credits page](advanced/_static/update-credits.png)

<!--In *CoursesContoller.cs*, add UpdateCourseCredits methods for HttpGet and HttpPost:-->
在 *CoursesContoller.cs* 中添加 HttpGet 和 HttpPost 的 UpdateCourseCredits 方法：

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_UpdateGet)]

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_UpdatePost)]

<!--When the controller processes an HttpGet request, nothing is returned in `ViewData["RowsAffected"]`, and the view displays an empty text box and a submit button, as shown in the preceding illustration.-->
当控制器处理 HttpGet 请求时， `ViewData["RowsAffected"]` 不返回任何东西，视图会显示一个空文本框和一个提交按钮，如前图所示。

<!--When the **Update** button is clicked, the HttpPost method is called, and multiplier has the value entered in the text box. The code then executes the SQL that updates courses and returns the number of affected rows to the view in `ViewData`. When the view gets a `RowsAffected` value, it displays the number of rows updated.-->
点击 **Update** 按钮，调用 HttpPost 方法，然后就会乘上文本框中输入的值。代码将执行 SQL，在视图的 `ViewData` 中更新课程数据，返回受影响行数并通过  `RowsAffected` 传递给视图上。视图从该变量获得值后将其显示于页面上。

<!--In **Solution Explorer**, right-click the *Views/Courses* folder, and then click **Add > New Item**.-->
在 **Solution Explorer** 中右键点击 *Views/Courses* 文件夹，然后点击 **Add > New Item**。

<!--In the **Add New Item** dialog, click **ASP.NET** under **Installed** in the left pane, click **MVC View Page**, and name the new view *UpdateCourseCredits.cshtml*.-->
在 **Add New Item** 对话框中点击 **ASP.NET**（在左侧栏的 **Installed** 下），点击 **MVC View Page**，将新视图命名为 *UpdateCourseCredits.cshtml*。

<!--In *Views/Courses/UpdateCourseCredits.cshtml*, replace the template code with the following code:-->
用如下代码替换 *Views/Courses/UpdateCourseCredits.cshtml* 中的模板代码：

[!code-html[Main](intro/samples/cu/Views/Courses/UpdateCourseCredits.cshtml)]

<!--Run the `UpdateCourseCredits` method by selecting the **Courses** tab, then adding "/UpdateCourseCredits" to the end of the URL in the browser's address bar (for example: `http://localhost:5813/Course/UpdateCourseCredits)`. Enter a number in the text box:-->
通过选择 **Courses** 标签页，然后在地址栏的 URL 末尾添加 "/UpdateCourseCredits"  （如 `http://localhost:5813/Course/UpdateCourseCredits)`），而后应用程序将会运行 ``UpdateCourseCredits`` 方法。在文本框中输入一个数字：


![Update Course Credits page](advanced/_static/update-credits.png)

<!--Click **Update**. You see the number of rows affected:-->
点击 **Update**。你可以看到受影响的行数：

![Update Course Credits page rows affected](advanced/_static/update-credits-rows-affected.png)

<!--Click **Back to List** to see the list of courses with the revised number of credits.-->
点击 **Back to List**，查看学分修订后的课程列表。

请注意，生产环境代码将确保更新总是有效的结果数据。 这里显示的简化版的代码可以将信用数量乘以大于5的数字（ `Credits` 属性具有 `[Range(0, 5)]` ]特性。）更新查询将工作，但无效数据可能在系统的其他部分导致意外的结果，假设学分数为5或更少。
<!--Note that production code would ensure that updates always result in valid data. The simplified code shown here could multiply the number of credits enough to result in numbers greater than 5. (The `Credits` property has a `[Range(0, 5)]` attribute.) The update query would work but the invalid data could cause unexpected results in other parts of the system that assume the number of credits is 5 or less.-->

<!--For more information about raw SQL queries, see [原生 SQL 查询](https://docs.microsoft.com/en-us/ef/core/querying/raw-sql).-->
更多有关原生 SQL 查询的资料参见 [原生 SQL 查询](https://docs.microsoft.com/en-us/ef/core/querying/raw-sql)。

<!--## Examine SQL sent to the database-->
## 检查发送到数据库的 SQL 语句

<!--Sometimes it's helpful to be able to see the actual SQL queries that are sent to the database. Built-in logging functionality for ASP.NET Core is automatically used by EF Core to write logs that contain the SQL for queries and updates. In this section you'll see some examples of SQL logging.-->
有时，可以看到发送到数据库的实际执行的 SQL 查询是非常有用的。ASP.NET Core 内建日志功能可以由 EF Core 自动使用，用于写入是包括 SQL 查询与更新的日志。本节将提供几个 SQL 日志记录的例子。

<!--Open *StudentsController.cs* and in the `Details` method set a breakpoint on the `if (student == null)` statement.-->
打开 *StudentController.cs*，在 `Details` 方法的 `if (student == null)` 语句上设置一个断点。

<!--Run the application in debug mode, and go to the Details page for a student.-->
以调试模式运行应用程序，转到某个学生的 Details 页面。

<!--Go to the **Output** window showing debug output, and you see the query:-->
转到显示调试输出的 **Output** 窗口，你会看到查询：

```
Microsoft.EntityFrameworkCore.Storage.IRelationalCommandBuilderFactory:Information: Executed DbCommand (225ms) [Parameters=[@__id_0='?'], CommandType='Text', CommandTimeout='30']
SELECT [e].[EnrollmentID], [e].[CourseID], [e].[Grade], [e].[StudentID], [c].[CourseID], [c].[Credits], [c].[DepartmentID], [c].[Title]
FROM [Enrollment] AS [e]
INNER JOIN (
    SELECT DISTINCT TOP(2) [s].[ID]
    FROM [Person] AS [s]
    WHERE ([s].[Discriminator] = N'Student') AND ([s].[ID] = @__id_0)
    ORDER BY [s].[ID]
) AS [s0] ON [e].[StudentID] = [s0].[ID]
INNER JOIN [Course] AS [c] ON [e].[CourseID] = [c].[CourseID]
ORDER BY [s0].[ID]
```

<!--You'll notice something here that might surprise you: the SQL selects up to 2 rows (`TOP(2)`). The `SingleOrDefaultAsync` method doesn't resolve to one row on the server. If the Where clause matches multiple rows, the method must return null, so EF only has to select a maximum of 2 rows, because if 3 or more match the Where clause, the result from the `SingleOrDefaultAsync` method is the same as if 2 rows match.-->
你在这里可能会吃惊：SQL 最多选选择 2 行记录（`TOP(2)`）。 `SingleOrDefaultAsync` 方法不能解析为服务器上的一行。如果 WHERE 子句命中多行记录，该方法必然返回 null，因此 EF  只能选择最多两行——这是因为不管命中两行还是更多行， `SingleOrDefaultAsync` 方法返回的结果是一样的。

<!--Note that you don't have to use debug mode and stop at a breakpoint to get logging output in the **Output** window. It's just a convenient way to stop the logging at the point you want to look at the output. If you don't do that, logging continues and you have to scroll back to find the parts you're interested in.-->
这里需要注意，你不需要使用调式模式，便能在断点处停下并将日志输出到 **Output** 窗口中。这是一种便捷的在记录日志的地方停下以便查看输出的方法。如果不这么做，日志会继续记录下去，而你必须向后滚动才能找到你所感兴趣的那部分日志。

<!--## Repository and unit of work patterns-->
## 存储库（Repository）与工作单元（UoW）模式

<!--Many developers write code to implement the repository and unit of work patterns as a wrapper around code that works with the Entity Framework. These patterns are intended to create an abstraction layer between the data access layer and the business logic layer of an application. Implementing these patterns can help insulate your application from changes in the data store and can facilitate automated unit testing or test-driven development (TDD). However, writing additional code to implement these patterns is not always the best choice for applications that use EF, for several reasons:-->
许多开发者编写代码来实现存储库（repository）和工作单元（unit of work）模式作为 Entity Framework 的封装。这类模式一般会在应用程序的数据访问层（data access layer）和业务逻辑层（business logic layer）之间创建一层抽象层（abstraction layer）。实现这类模式固然可以是应用程序与数据库存储解耦，并可促进自动化单元测试（automated unit testing）或测试驱动开发（test-driven development，TDD），但通过编写额外的代码来实现这类模式并不是 EF 金科玉律般的最佳时间，因为：

<!--* The EF context class itself insulates your code from data-store-specific code.-->
* EF 上下文类本身就将你所写的代码与数据库存储特定代码（data-store-specific code）相隔离。

<!--* The EF context class can act as a unit-of-work class for database updates that you do using EF.-->
* EF 上下文可以充当你利用 EF 更新数据库时的工作单元类。

<!--* EF includes features for implementing TDD without writing repository code.-->
* EF 包含不编写存储库代码时实现 TDD 的功能。

<!--For information about how to implement the repository and unit of work patterns, see [Entity Framework 5 系列教程](https://docs.microsoft.com/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application).-->
有关如何实现存储库与工作单元模式的资料，请参见 [Entity Framework 5 系列教程](https://docs.microsoft.com/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)。


<!--Entity Framework Core implements an in-memory database provider that can be used for testing. For more information, see [在内存中测试](https://docs.microsoft.com/ef/core/miscellaneous/testing/in-memory).-->
Entity Framework Core 实现了一种可用于测试的内存数据库提供程序。有关这方面的资料请参见 [在内存中测试](https://docs.microsoft.com/ef/core/miscellaneous/testing/in-memory)。

<!--## Automatic change detection-->
## 自动检测变更

<!--The Entity Framework determines how an entity has changed (and therefore which updates need to be sent to the database) by comparing the current values of an entity with the original values. The original values are stored when the entity is queried or attached. Some of the methods that cause automatic change detection are the following:-->
Entity Framework 确定一个实体是否发生变更（并因此需要将更新发送到数据库）的方法，是通过比较实体当前值与原始值。当实体被查询或附加时，原始植会被保存下来。引发自动变更检测的方法有以下几种：

* DbContext.SaveChanges

* DbContext.Entry

* ChangeTracker.Entries

<!--If you're tracking a large number of entities and you call one of these methods many times in a loop, you might get significant performance improvements by temporarily turning off automatic change detection using the `ChangeTracker.AutoDetectChangesEnabled` property. For example:-->
如果你正跟踪大量实体，并且在循环体中多次调用上述列举的方法，那么你可以通过 `ChangeTracker.AutoDetectChangesEnabled` 属性暂时关闭自动变更检测以获得性能上的提升。例如：

```csharp
_context.ChangeTracker.AutoDetectChangesEnabled = false;
```

<!--## Entity Framework Core source code and development plans-->
## Entity Framework Core 源码以及开发计划

<!--The source code for Entity Framework Core is available at [https://github.com/aspnet/EntityFramework](https://github.com/aspnet/EntityFramework). Besides source code, you can get nightly builds, issue tracking, feature specs, design meeting notes, [the roadmap for future development](https://github.com/aspnet/EntityFramework/wiki/Roadmap), and more. You can file bugs, and you can contribute your own enhancements to the EF source code.-->
Entty Framework Core 的源代码在 [https://github.com/aspnet/EntityFramework](https://github.com/aspnet/EntityFramework)。除了源代码，你还可以得到每日构建、问题跟踪、细节说明、设计会议笔记， [未来开发发展路线图](https://github.com/aspnet/EntityFramework/wiki/Roadmap)等。你可以提交 Bug，并且可以贡献自己的增强 EF 源代码。

<!--Although the source code is open, Entity Framework Core is fully supported as a Microsoft product. The Microsoft Entity Framework team keeps control over which contributions are accepted and tests all code changes to ensure the quality of each release.-->
虽然很多源码是开放的，Entity Framework Core 是一个完全由微软支持的产品。微软的 Entity Framework 团队保持对所有代码变更贡献的接受和测试的控制权以确保每个释出版本的质量。

<!--## Reverse engineer from existing database-->
## 从现有数据库逆向工程

<!--To reverse engineer a data model including entity classes from an existing database, use the [scaffold-dbcontext](https://docs.microsoft.com/ef/core/miscellaneous/cli/powershell#scaffold-dbcontext) command. See the [开始学习](https://docs.microsoft.com/ef/core/get-started/aspnetcore/existing-db).-->
对包含实体类型的数据模型进行反向工程，请使用 [scaffold-dbcontext](https://docs.microsoft.com/ef/core/miscellaneous/cli/powershell#scaffold-dbcontext) command 命令。具体请阅读 [开始学习](https://docs.microsoft.com/ef/core/get-started/aspnetcore/existing-db)。

<a id="dynamic-linq">

<!--## Use dynamic LINQ to simplify sort selection code-->
## 使用动态 LINQ 简化排序选择代码

<!--The [本系列的第三个教程](sort-filter-page.md) shows how to write LINQ code by hard-coding column names in a `switch` statement. With two columns to choose from, this works fine, but if you have many columns the code could get verbose. To solve that problem, you can use the `EF.Property` method to specify the name of the property as a string. To try out this approach, replace the `Index` method in the `StudentsController` with the following code.-->
[本系列的第三个教程](sort-filter-page.md) 显示了如何通过在 `switch` 语句中硬编码列名来编写 LINQ 代码。 有两列可供选择，这可以正常工作，但如果您有很多列，代码可能会更复杂。 要解决这个问题，可以使用 `EF.Property` 方法来指定属性的名称作为一个字符串。 要尝试这种方法，请使用以下代码替换 `StudentsController` 中的 `Index` 方法

[!code-csharp[Main](intro/samples/cu/Controllers/StudentsController.cs?name=snippet_DynamicLinq)]

<!--## Next steps-->
## 下一步

<!--This completes this series of tutorials on using the Entity Framework Core in an ASP.NET MVC application.-->
至此，有关在 ASP.NET MVC 应用程序中使用 Entity Framework Core 的系列教程到此结束。

<!--For more information about EF Core, see the [Entity Framework Core documentation](https://docs.microsoft.com/ef/core). A book is also available: [Entity Framework Core in Action](https://www.manning.com/books/entity-framework-core-in-action).-->
更多有关 EF Core 的资料请参见  [Entity Framework Core 文档](https://docs.microsoft.com/ef/core)。还有这本书 [Entity Framework Core in Action](https://www.manning.com/books/entity-framework-core-in-action)。

<!--For information about how to deploy your web application after you've built it, see [发布与部署](../../publishing/index.md).-->
有关部署 Web 应用程序的信息，请参见 [发布与部署](../../publishing/index.md).

<!--For information about other topics related to ASP.NET Core MVC, such as authentication and authorization, see the [ASP.NET Core 文档](https://docs.microsoft.com/aspnet/core/).-->
有关 ASP.NET Core MVC 的其他话题，比如身份认证与授权等，请参见 [ASP.NET Core 文档](https://docs.microsoft.com/aspnet/core/)。

## 致谢

<!--Tom Dykstra and Rick Anderson (twitter @RickAndMSFT) wrote this tutorial. Rowan Miller, Diego Vega, and other members of the Entity Framework team assisted with code reviews and helped debug issues that arose while we were writing code for the tutorials.-->
本教程由 Tom Dykstra 和 Rick Anderson (twitter @RickAndMSFT) 编写，由 dotNET Core Studying Group 翻译为中文。
Entity Framework 团队的 Rowan Miller、Diego Vega 以及其他诸位同僚帮助我们进行代码审查，并帮我们就本系列教程进行调试。

<!--## Common errors-->
## 常见错误  

<!--### ContosoUniversity.dll used by another process-->
### ContosoUniversity.dll 被另一个进程使用

<!--Error message:-->
错误信息：

> Cannot open '...bin\Debug\netcoreapp1.0\ContosoUniversity.dll' for writing -- 'The process cannot access the file '...\bin\Debug\netcoreapp1.0\ContosoUniversity.dll' because it is being used by another process.

<!--Solution:-->
解决方案：

<!--Stop the site in IIS Express. Go to the Windows System Tray, find IIS Express and right-click its icon, select the Contoso University site, and then click **Stop Site**.-->
在 IIS Express 中停止站点：转到 Windows System Tray（Windows 系统托盘区），找到 IIS Express 并右键单击图标，选择 Contoso University 站点，然后点击 **Stop Site**。

### Migration scaffolded with no code in Up and Down methods

<!--Possible cause:-->
导致原因：

<!--The EF CLI commands don't automatically close and save code files. If you have unsaved changes when you run the `migrations add` command, EF won't find your changes.-->
EF CLI 命令不会自动关闭并保存代码文件。如果在你运行 `migrations add` 命令时没有保存修改，EF 是不会发现你做的修改的。

<!--Solution:-->
解决方案：

<!--Run the `migrations remove` command, save your code changes and rerun the `migrations add` command.-->
运行 `migrations remove` 命令，然后保存你的代码，再运行 `migrations remove` 命令。

<!--### Errors while running database update-->
### 执行数据库更新时发生错误

<!--It's possible to get other errors when making schema changes in a database that has existing data. If you get migration errors you can't resolve, you can either change the database name in the connection string or delete the database. With a new database, there is no data to migrate, and the update-database command is much more likely to complete without errors.-->
在一个存在数据的数据库中修改架构可能会得到其他错误。如果你得到一个无法处理的迁移错误，你可以：1）修改连接字符串上的数据库名称，2）删除数据库。这样当新的数据库创建时，就不会有数据需要迁移，update-database 命令也就更易无错完成了。

<!--The simplest approach is to rename the database in *appsettings.json*. The next time you run `database update`, a new database will be created.-->
最简单的办法是在 *appsettings.json* 修改数据库的名称。下一次你运行 `database update`的时候新数据库会被创建。


<!--To delete a database in SSOX, right-click the database, click **Delete**, and then in the **Delete Database** dialog box select **Close existing connections** and click **OK**.-->
在 SSOX 中删除数据库的话，右键点击数据库，然后选择 **Delete**，接着在 **Delete Database** 对话框中选择 **Close existing connections** 并点击 **OK**。

<!--To delete a database by using the CLI, run the `database drop` CLI command:-->
通过运行 `database drop 这条 CLI 命令删除数据库：

```console
dotnet ef database drop
```

<!--### Error locating SQL Server instance-->
### 错误定位 SQL Server 实例

<!--Error Message:-->
错误信息：

> A network-related or instance-specific error occurred while establishing a connection to SQL Server. The server was not found or was not accessible. Verify that the instance name is correct and that SQL Server is configured to allow remote connections. (provider: SQL Network Interfaces, error: 26 - Error Locating Server/Instance Specified)

<!--Solution:-->
解决方案：

<!--Check the connection string. If you have manually deleted the database file, change the name of the database in the construction string to start over with a new database.-->
检查连接字符串。如果你手动删除了数据库文件，那么需要修改连接字符串中数据库的名字以便能重新开建一个新的数据库。

>[!div class="step-by-step"]
[上一节](inheritance.md)

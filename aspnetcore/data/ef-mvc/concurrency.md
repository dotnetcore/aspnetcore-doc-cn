---
title: ASP.NET Core MVC 与 EF Core - 并发 - 8 of 10 | Microsoft 文档（中文文档）
author: tdykstra
description: 本教程将指导你当多个用户同时更新同一实体时如何处理冲突。
keywords: ASP.NET Core 中文文档, Entity Framework Core, concurrency
ms.author: tdykstra
manager: wpickett
ms.date: 03/15/2017
ms.topic: get-started-article
ms.assetid: 15e79e15-bda5-441d-80c7-8032a2628605
ms.technology: aspnet
ms.prod: asp.net-core
uid: data/ef-mvc/concurrency
---

# 处理并发冲突 - EF Core 与 ASP.NET Core MVC 教程 (8 of 10)

作者 [Tom Dykstra](https://github.com/tdykstra) 、 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [刘怡(AlexLEWIS/Forerunner)](http://github.com/alexinea) 

<!--The Contoso University sample web application demonstrates how to create ASP.NET Core 1.1 MVC web applications using Entity Framework Core 1.1 and Visual Studio 2017. For information about the tutorial series, see [the first tutorial in the series](intro.md).-->
Contoso 大学 Web应用程序演示了如何使用 Entity Framework Core 1.1 以及 Visual Studio 2017 来创建 ASP.NET Core 1.1 MVC Web 应用程序。更多信息请参考 [第一节教程](intro.md).

<!--In earlier tutorials you learned how to update data. This tutorial shows how to handle conflicts when multiple users update the same entity at the same time.-->
在之前的教程中你已经学习了如何更新数据。本教程将指导你当多个用户同时更新同一实体时如何处理冲突。

<!--You'll create web pages that work with the Department entity and handle concurrency errors. The following illustrations show the Edit and Delete pages, including some messages that are displayed if a concurrency conflict occurs.-->
你将创建与 Department 实体一起使用的页面，并以此页面处理并发错误。下图展示了 Edit 和 Delete 页面，包括发生并发冲突（concurrency conflict）时显示的一些消息。

![Department Edit page](concurrency/_static/edit-error.png)

![Department Delete page](concurrency/_static/delete-error.png)

<!--## Concurrency conflicts-->
## 并发冲突

<!--A concurrency conflict occurs when one user displays an entity's data in order to edit it, and then another user updates the same entity's data before the first user's change is written to the database. If you don't enable the detection of such conflicts, whoever updates the database last overwrites the other user's changes. In many applications, this risk is acceptable: if there are few users, or few updates, or if isn't really critical if some changes are overwritten, the cost of programming for concurrency might outweigh the benefit. In that case, you don't have to configure the application to handle concurrency conflicts.-->
当某个用户显示实体并打算对其进行编辑，而另一个用户在前一个用户尚未提交更新并写入数据库时更新了相同实体的数据，此时将发生并发冲突（concurrency conflict）。如果不启用检测此类冲突，那么后一次更新数据库将覆盖其他用户的变更。在许多应用程序中，这一风险是可被接受的：他们没有多少用户，或者数据几乎没有更新，又或者对于数据更改被覆盖并不怎么重要，，那么并发编程的成本可能就高于其优势了。在那种情况下，你就不必为应用程序配置处理并发冲突。

<!--### Pessimistic concurrency (locking)-->
### 悲观并发控制（锁）

<!--If your application does need to prevent accidental data loss in concurrency scenarios, one way to do that is to use database locks. This is called pessimistic concurrency. For example, before you read a row from a database, you request a lock for read-only or for update access. If you lock a row for update access, no other users are allowed to lock the row either for read-only or update access, because they would get a copy of data that's in the process of being changed. If you lock a row for read-only access, others can also lock it for read-only access but not for update.-->
如果你的应用程序需要在并发场景下防止意外丢失数据，一种办法时使用数据库锁。这叫做悲观并发（pessimistic concurrency）。比方说在你读取一条数据库记录前，先请求一个只读锁或更新访问锁。，如果你为更新访问锁定一行记录，那么其他用户无论是只读还是更新操作都无法锁定该记录，因为他们将拿到的只是被操作的数据的副本。如果将行锁定为只读访问，那么其他用户也能将该条记录锁定为只读访问，但其他用户不能更新该条记录。

<!--Managing locks has disadvantages. It can be complex to program. It requires significant database management resources, and it can cause performance problems as the number of users of an application increases. For these reasons, not all database management systems support pessimistic concurrency. Entity Framework Core provides no built-in support for it, and this tutorial doesn't show you how to implement it.-->
锁的管理是有缺点的，它会导致程序变得复杂。它需要大量的数据库管理资源，并会随应用程序用户量的增加而产生性能问题。正因为如此，并非所有数据库管理系统都支持悲观并发。Entity Framework Core 不为其提供内建支持，本教程也不会向你介绍如何实现它。

<!--### Optimistic Concurrency-->
### 乐观并发控制

<!--The alternative to pessimistic concurrency is optimistic concurrency. Optimistic concurrency means allowing concurrency conflicts to happen, and then reacting appropriately if they do. For example, Jane visits the Department Edit page and changes the Budget amount for the English department from $350,000.00 to $0.00.-->
悲观并发的替代方案时乐观并发（optimistic concurrency）。乐观并发意味着允许并发冲突的发生，并适当做出反应。例如约翰运行 Department 的 Edit 页面，将英语系的预算金额从 $350,000.00 调整为 $0.00。

![Changing budget to 0](concurrency/_static/change-budget.png)

<!--Before Jane clicks **Save**, John visits the same page and changes the Start Date field from 9/1/2007 to 9/1/2013.-->
在约翰点击 **Save** 前，珍妮运行了相同的页面并将开始时间从 9/1/2007 改为 8/8/2013。

![Changing start date to 2013](concurrency/_static/change-date.png)

<!--Jane clicks **Save** first and sees her change when the browser returns to the Index page.-->
当约翰第一次点击 **Save** 后将返回 Index 页并看到变更的数据。

![Budget changed to zero](concurrency/_static/budget-zero.png)

<!--Then John clicks **Save** on an Edit page that still shows a budget of $350,000.00. What happens next is determined by how you handle concurrency conflicts.-->
而当珍妮在 Edit 页面上点击 **Save** 时预算仍是 $350,000.00。那么接下来所发生的，就是我们要演示如何处理并发冲突了。

<!--Some of the options include the following:-->
有这么几种方案可供选择：

<!--* You can keep track of which property a user has modified and update only the corresponding columns in the database.-->
* 你可以跟踪用户已修改的属性，并仅更新相应的列到数据库。

     <!--In the example scenario, no data would be lost, because different properties were updated by the two users. The next time someone browses the English department, they'll see both Jane's and John's changes -- a start date of 9/1/2013 and a budget of zero dollars. This method of updating can reduce the number of conflicts that could result in data loss, but it can't avoid data loss if competing changes are made to the same property of an entity. Whether the Entity Framework works this way depends on how you implement your update code. It's often not practical in a web application, because it can require that you maintain large amounts of state in order to keep track of all original property values for an entity as well as new values. Maintaining large amounts of state can affect application performance because it either requires server resources or must be included in the web page itself (for example, in hidden fields) or in a cookie.-->
     在示例场景中，不会丢失任何数据，因为两个用户更新的属性并不相同。下一次某人浏览英语系的时候他就能看到约翰和珍妮所做的修改——开始时间为 8/8/2013，预算为 0 美元。这种更新方法可以减少冲突的发生，但如果对实体的相同属性进行竞争性的更改，则不可避免地会导致数据丢失。Entity Framework 是否以此机制运行取决于其实如何实现代码的。这在 Web 应用程序中通常不实用，因为它可能会需要维持大量的状态以便能跟踪实体的所有属性的旧值和新值。维护大量的状态可能会导致程序的性能问题，因为这需要消费服务器资源，或者必须包含在网页本身（如隐藏字段）或 Cookie 中。

<!--* You can let John's change overwrite Jane's change.-->
* 你可以让珍妮所做的修改覆盖约翰的修改。

     <!--The next time someone browses the English department, they'll see 9/1/2013 and the restored $350,000.00 value. This is called a *Client Wins* or *Last in Wins* scenario. (All values from the client take precedence over what's in the data store.) As noted in the introduction to this section, if you don't do any coding for concurrency handling, this will happen automatically.-->
     下次某人浏览英语系的时候，他们将看到的是 8/8/2013 和 $350,000.00。这叫做 *Client Wins* 或 *Last in Wins* 场景（所有来自客户端的值都优先于数据库中存储的值）。如本节介绍中所述，如果你没有对并发处理进行任何编码，则会自动发生。

<!--* You can prevent John's change from being updated in the database.-->
* 你可以阻止珍妮所做的修改在数据库更新之后写入数据库。

     <!--Typically, you would display an error message, show him the current state of the data, and allow him to reapply his changes if he still wants to make them. This is called a *Store Wins* scenario. (The data-store values take precedence over the values submitted by the client.) You'll implement the Store Wins scenario in this tutorial. This method ensures that no changes are overwritten without a user being alerted to what's happening.-->
     通常来讲，你会看到一条错误消息，向她显示当前的数据状态并允许他重新应用其修改（如果她仍然想更改的话）。这叫做 *Store Wins* 场景（数据库存储的值优先于客户端提交的值）。在本教程中你将实现 Store Wins 场景。此方法确保不会覆盖任何变更，且不会向用户发出任何警告。

<!--### Detecting concurrency conflicts-->
### 检测并发冲突

<!--You can resolve conflicts by handling `DbConcurrencyException` exceptions that the Entity Framework throws. In order to know when to throw these exceptions, the Entity Framework must be able to detect conflicts. Therefore, you must configure the database and the data model appropriately. Some options for enabling conflict detection include the following:-->
你可以通过处理 Entity Framework 抛出的 `DbConcurrencyException` 异常来解决冲突。为了知道何时抛出异常，Entity Framework 必须能检测冲突。因此你必须正确配置数据库和数据模型。启用冲突检测有以下选项，包括：

<!--* In the database table, include a tracking column that can be used to determine when a row has been changed. You can then configure the Entity Framework to include that column in the Where clause of SQL Update or Delete commands.-->
* 在数据库表中，可以包括一个用于表示本行很是修改过的跟踪列。然后你可以配置 Entity Framework 将该列包含在 SQL Update 或 Delete 的 Where 子句中。

     <!--The data type of the tracking column is typically `rowversion`. The `rowversion` value is a sequential number that's incremented each time the row is updated. In an Update or Delete command, the Where clause includes the original value of the tracking column (the original row version) . If the row being updated has been changed by another user, the value in the `rowversion` column is different than the original value, so the Update or Delete statement can't find the row to update because of the Where clause. When the Entity Framework finds that no rows have been updated by the Update or Delete command (that is, when the number of affected rows is zero), it interprets that as a concurrency conflict.-->
     跟踪列的数据类型通常是 `rowversion`。 `rowversion`的值时每次更新行的时候会递增的序列号。在 Update 或 Delete 命令中，Where 子句中包含跟踪列的原始值（原始行的版本）。如果该行已被其他人更新， `rowversion`列的值就会与原始之不同，因此 Update 或 Delete 语句就不会找到需要更新的那一行（因为 Where 子句的缘故）。当 Entity Framework 发现更无可更或删无可删的时候（也就是说受影响的行数为零的时候），它将解释其为并发冲突。

<!--* Configure the Entity Framework to include the original values of every column in the table in the Where clause of Update and Delete commands.-->
* 配置 Entity Framework，在 Update 或 Delete 的 Where 子句中包含表中每一列的原始值。

     <!--As in the first option, if anything in the row has changed since the row was first read, the Where clause won't return a row to update, which the Entity Framework interprets as a concurrency conflict. For database tables that have many columns, this approach can result in very large Where clauses, and can require that you maintain large amounts of state. As noted earlier, maintaining large amounts of state can affect application performance. Therefore this approach is generally not recommended, and it isn't the method used in this tutorial.-->
     与第一个选项一样，如果第一次读取后行中任何内容发生变化，则 Where 子句将不会返回要更新的行，Entity Framework 将解释其为并发冲突。对于有许多列的数据库表来讲，这个方法可能会导致大体积的 Where 子句，并且可能会需要维护大量状态。如前所述，维护大量状态将导致应用程序的性能问题。因此此方法通常不推荐使用，它也不会在本教程中使用。

     <!--If you do want to implement this approach to concurrency, you have to mark all non-primary-key properties in the entity you want to track concurrency for by adding the `ConcurrencyCheck` attribute to them. That change enables the Entity Framework to include all columns in the SQL Where clause of Update and Delete statements.-->
     如果你想实现这种方法来应对并发，你需要标记需要跟踪并发性的实体内所有非主键属性添加 `ConcurrencyCheck` 特性。这一改变将使 Entity Framework 能将所有列包含在 Update 和 Delete 语句的 Where 子句之中。

<!--In the remainder of this tutorial you'll add a `rowversion` tracking property to the Department entity, create a controller and views, and test to verify that everything works correctly.-->
在本教程余下部分中你将会向 Department 实体添加一个 `rowversion` 跟踪属性，创建一个控制器和视图，并测试验证一下是否工作正常。

<!--## Add a tracking property to the Department entity-->
## 在 Department 实体中添加跟踪属性

<!--In *Models/Department.cs*, add a tracking property named RowVersion:-->
在 *Models/Department.cs* 中添加一个名为 RowVersion 的跟踪属性：

[!code-csharp[Main](intro/samples/cu/Models/Department.cs?name=snippet_Final&highlight=26,27)]

<!--The `Timestamp` attribute specifies that this column will be included in the Where clause of Update and Delete commands sent to the database. The attribute is called `Timestamp` because previous versions of SQL Server used a SQL `timestamp` data type before the SQL `rowversion` replaced it. The .NET type for `rowversion` is a byte array.-->
 `Timestamp` 特性指定该列将被包含在发送到数据库中的 Update 和 Delete 命令的子句中。该属性叫做 `Timestamp` 是因为 SQL Server 以前的版本所提供的  `Timestamp` 数据类型现在已被 `rowversion` 替代。 `rowversion` 的 .NET 类型是一个字节数组。

If you prefer to use the fluent API, you can use the `IsConcurrencyToken` method (in *Data/SchoolContext.cs*) to specify the tracking property, as shown in the following example:
如果你倾向于使用 fluent API，可以使用 `IsConcurrencyToken`  (在 *Data/SchoolContext.cs* 文件中) 方法来指定跟踪属性，如下例所示：

```csharp
modelBuilder.Entity<Department>()
    .Property(p => p.RowVersion).IsConcurrencyToken();
```

<!--By adding a property you changed the database model, so you need to do another migration.-->
由于你添加了一个属性，改变了数据库模型，因此你需要另一个迁移。

<!--Save your changes and build the project, and then enter the following commands in the command window:-->
保存变更并构建项目，然后在命令窗口键入下列命令：

```console
dotnet ef migrations add RowVersion
dotnet ef database update
```

<!--## Create a Departments controller and views-->
## 创建 Department 控制器和视图

<!--Scaffold a Departments controller and views as you did earlier for Students, Courses, and Instructors.-->
搭建一个 Departments 控制器和视图，如之前给 Students、Courses 以及 Instructors 创建的那样。

![Scaffold Department](concurrency/_static/add-departments-controller.png)

<!--In the *DepartmentsController.cs* file, change all four occurrences of "FirstMidName" to "FullName" so that the department administrator drop-down lists will contain the full name of the instructor rather than just the last name.-->
在 *DepartmentsController* 文件中，将出现的所有四个「FirstMidName」都改为「FullName」，一边系管理员下拉菜单包含教师的完整名称，而不是他们的姓。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_Dropdown)]

<!--## Update the Departments Index view-->
## 更新 Department Index 视图

<!--The scaffolding engine created a RowVersion column in the Index view, but that field shouldn't be displayed.-->
在 Index 视图中有基架引擎创建的 RowVersion 列。但你想要显示的是 Administrator，而不是什么 RowVersion。

<!--Replace the code in *Views/Departments/Index.cshtml* with the following code.-->
用以下代码替换 *Views/Departments/Index.cshtml* 中的代码。

[!code-html[Main](intro/samples/cu/Views/Departments/Index.cshtml?highlight=4,7,44)]

<!--This changes the heading to "Departments" deletes the RowVersion column, and shows full name instead of first name for the administrator.-->
这个改动将会把标题改为「Departments」，重新排列字段，并使用 Administrator 列代替 RowVersion 列。


<!--## Update the Edit methods in the Departments controller-->
## 在 Departments 控制器中更新 Edit 方法

<!--In both the HttpGet `Edit` method and the `Details` method, add `AsNoTracking`. In the HttpGet `Edit` method, add eager loading for the Administrator.-->
在 HttpGet 的 `Edit` 方法和 `Details` 方法中, 添加 `AsNoTracking`。 在 HttpGet 的 `Edit` 方法中,，预加载 Administrator 导航属性。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_EagerLoading&highlight=2,3)]

<!--Replace the existing code for the HttpPost `Edit` method with the following code:-->
用下列代码替换 HttpPost `Edit` 方法中已有的代码：

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_EditPost)]

<!--The code begins by trying to read the department to be updated. If the `SingleOrDefaultAsync` method returns null, the department was deleted by another user. In that case the code uses the posted form values to create a department entity so that the Edit page can be redisplayed with an error message. As an alternative, you wouldn't have to re-create the department entity if you display only an error message without redisplaying the department fields.-->
代码从尝试读取需要更新的系开始，如果 `SingleOrDefaultAsync` 方法返回 null，则说明系被另一个用户删除了。那种情形下代码使用 POST 来的值创建一个系实体，以便可以重新展示 Edit 页并显示错误消息。或者，你也可以不重新创建该系实体，只需要显示错误信息而不重新显示该系的字段。

<!--The view stores the original `RowVersion` value in a hidden field, and this method receives that value in the `rowVersion` parameter. Before you call `SaveChanges`, you have to put that original `RowVersion` property value in the `OriginalValues` collection for the entity.-->
原始的 `RowVersion` 值存放在视图的隐藏字段中，此方法在 `RowVersion` 参数中接收该值。在你调用 `SaveChanges` 之前，你必须在实体的 `OriginalValues` 集合中放置原始的 `RowVersion` 属性值

```csharp
_context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowVersion;
```

<!--Then when the Entity Framework creates a SQL UPDATE command, that command will include a WHERE clause that looks for a row that has the original `RowVersion` value. If no rows are affected by the UPDATE command (no rows have the original `RowVersion` value),  the Entity Framework throws a `DbUpdateConcurrencyException` exception.-->
然后当 Entity Framework 创建一个 SQL UPDATE 命令时，该命令包含的 WHERE 子句中就含有查询具有原始 `RowVersion` 值的行的那一部分。如果没有行受到 UPDATE 命令的影响（没有一行的 `RowVersion` 值与 WHERE 子句中指示的一致），那么 Entity Framework 将抛出 `DbUpdateConcurrencyException` 异常。

<!--The code in the catch block for that exception gets the affected Department entity that has the updated values from the `Entries` property on the exception object.-->
在该异常的 catch 代码块中，通过异常对象能获取受影响的 Department 实体，该实体的 `Entries` 属性能获得更新后的值。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?range=164)]

<!--The `Entries` collection will have just one `EntityEntry` object.  You can use that object to get the new values entered by the user and the current database values.-->
 `Entries` 集合只有一个 `EntityEntry` 对象，且该对象具有用户输入的新值。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?range=165-166)]

<!--The code adds a custom error message for each column that has database values different from what the user entered on the Edit page (only one field is shown here for brevity).-->
代码为每一个在页面（用户编辑过的 Edit 页面）上的值与数据库中的值不同的列添加一个定制错误信息（为简洁起见，此处仅显示一个字段）。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?range=174-178)]

<!--Finally, the code sets the `RowVersion` value of the `departmentToUpdate` to the new value retrieved from the database. This new `RowVersion` value will be stored in the hidden field when the Edit page is redisplayed, and the next time the user clicks **Save**, only concurrency errors that happen since the redisplay of the Edit page will be caught.-->
最后，代码将 `departmentToUpdate` 的 `RowVersion` 值设置为来自数据库的新值。当重新显示编辑页面时，新的 `RowVersion` 值将保存在重新显示后的 Edit 页的隐藏字段中，并在下一次用户点击 **Save** 时只捕获自重新显示 Edit 页面以来的并发错误。

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?range=199-200)]

<!--The `ModelState.Remove` statement is required because `ModelState` has the old `RowVersion` value. In the view, the `ModelState` value for a field takes precedence over the model property values when both are present.-->
 `ModelState.Remove`  语句是非常必须的，因为 `ModelState` 中具有旧的 `RowVersion` 值。在视图中，当两个字段都存在时，字段 `ModelState` 值优先于模型属性的值

<!--## Update the Department Edit view-->
## 更新 Department Edit 视图

<!--In *Views/Departments/Edit.cshtml*, make the following changes:-->
在 *Views/Departments/Edit.cshtml* 中作如下修改：

<!--* Remove the `<div>` element that was scaffolded for the `RowVersion` field.-->
* 移除支持 `RowVersion` 字段的 `<div>` 元素。

<!--* Add a hidden field to save the `RowVersion` property value, immediately following the hidden field for the `DepartmentID` property.-->
* 添加一个用于保存 `RowVersion` 属性值的隐藏字段，紧跟在 `DepartmentID` 属性的隐藏字段之后。

<!--* Add a "Select Administrator" option to the drop-down list.-->
* 在下拉菜单中添加「Select Administrator」选项。

[!code-html[Main](intro/samples/cu/Views/Departments/Edit.cshtml?highlight=15,41-43)]

<!--## Test concurrency conflicts in the Edit page-->
## 在 Edit 页面中测试并发冲突

<!--Run the site and click Departments to go to the Departments Index page.-->
运行站点，点击 Departments 跳转到 Departments Index 页。

<!--Right click the **Edit** hyperlink for the English department and select **Open in new tab**, then click the **Edit** hyperlink for the English department. The two browser tabs now display the same information.-->
右键点击英语系的 **Edit** 超链并选择 **Open in new tab**，然后点击英语系的 **Edit** 超链。现在两个浏览器标签页都显示了相同的页面。

<!--Change a field in the first browser tab and click **Save**.-->
在第一个浏览器标签页中修改字段，并点击 **Save**。

![Department Edit page 1 after change](concurrency/_static/edit-after-change-1.png)

<!--The browser shows the Index page with the changed value.-->
浏览器会显示更新数据后的 Index 页。

<!--Change a field in the second browser tab.-->
修改第二个浏览器标签页中的字段。

![Department Edit page 2 after change](concurrency/_static/edit-after-change-2.png)

<!--Click **Save**. You see an error message:-->
点击 **Save**。然后你能看到如下错误信息：

![Department Edit page error message](concurrency/_static/edit-error.png)

<!--Click **Save** again. The value you entered in the second browser tab is saved. You see the saved values when the Index page appears.-->
再次点击 **Save**。第二个浏览器标签页中输入的值将与你第一个浏览器标签页中修改前的原始值一道被保存起来。当显示 Index 页的时候，你可以看到保存后的值。

<!--## Update the Delete page-->
## 更新 Delete 页面

<!--For the Delete page, the Entity Framework detects concurrency conflicts caused by someone else editing the department in a similar manner. When the HttpGet `Delete` method displays the confirmation view, the view includes the original `RowVersion` value in a hidden field. That value is then available to the HttpPost `Delete` method that's called when the user confirms the deletion. When the Entity Framework creates the SQL DELETE command, it includes a WHERE clause with the original `RowVersion` value. If the command results in zero rows affected (meaning the row was changed after the Delete confirmation page was displayed), a concurrency exception is thrown, and the HttpGet `Delete` method is called with an error flag set to true in order to redisplay the confirmation page with an error message. It's also possible that zero rows were affected because the row was deleted by another user, so in that case no error message is displayed.-->
对于 Delete 页面，Entity Framework 检测由其他人以类似方法编辑系引发的并发冲突。当 HttpGet  `Delete` 方法显示确认视图，视图将原始 `RowVersion` 值包含在隐藏字段中。然后当用户确认删除时，该值可用于调用 HttpPost `Delete` 方法。当 Entity Framework 创建 SQL DELETE 命令时，它将带有包含原始 `RowVersion` 值的 Where 子句。如果该命令影响的行数（表示删除确认后页面中改变的行数）是零，则会抛出并发异常，并以一个置为 true 的错误标识符调用 HttpGet `Delete` 方法，以便能重新显示带有错误消息的确认页面。零行受到影响也有可能是该条记录被其它用户删除了，因此在这种情况下不会显示错误消息。

<!--### Update the Delete methods in the Departments controller-->
### 在 Departments 控制器中更新 Delete 方法 

<!--In *DepartmentController.cs*, replace the HttpGet `Delete` method with the following code:-->
在 *DepartmentController.cs* 中用下列代码代替 HttpGet `Delete` 方法：

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_DeleteGet&highlight=1,14-17,21-29)]

<!--The method accepts an optional parameter that indicates whether the page is being redisplayed after a concurrency error. If this flag is true and the department specified no longer exists, it was deleted by another user. In that case, the code redirects to the Index page.  If this flag is true and the Department does exist, it was changed by another user. In that case, the code sends sends an error message to the view using `ViewData`.  -->
该方法接受一个可选的参数，这个可选参数用于指示并发错误发生后是否正常重新显示页面。如果此标志是 true，则通过使用 `ViewData` 将错误消息发送到视图中。

<!--Replace the code in the HttpPost `Delete` method (named `DeleteConfirmed`) with the following code:-->
用下列代码替换 HttpPost `Delete` 方法（名为 `DeleteConfirmed`）中的代码：

[!code-csharp[Main](intro/samples/cu/Controllers/DepartmentsController.cs?name=snippet_DeletePost&highlight=3,7,14,15,16,17,18)]

<!--In the scaffolded code that you just replaced, this method accepted only a record ID:-->
在你先前所替换的基架代码中，这个方法仅接受一个记录 ID：


```csharp
public async Task<IActionResult> DeleteConfirmed(int id)
```

<!--You've changed this parameter to a Department entity instance created by the model binder. This gives EF access to the RowVersion property value in addition to the record key.-->
你已将此参数修改为模型绑定器创建的 Department 实体实例。这将允许 EF 访问除记录键以外的 RowVersion 属性值

```csharp
public async Task<IActionResult> Delete(Department department)
```

<!--You have also changed the action method name from `DeleteConfirmed` to `Delete`. The scaffolded code used the name `DeleteConfirmed` to give the HttpPost method a unique signature. (The CLR requires overloaded methods to have different method parameters.) Now that the signatures are unique, you can stick with the MVC convention and use the same name for the HttpPost and HttpGet delete methods.-->
你也把操作方法的名称从 `DeleteConfirmed` 改成了 `Delete`。基架代码中使用 `DeleteConfirmed` 这个名字给 HttpPost 方法一个独有的方法签名（CLR 要求重载方法具有不同的方法参数）。现在签名已经唯一了，你可以依据 MVC 的约定，为 HttpPost 和 HttpGet 的删除方法使用相同的方法名。

<!--If the department is already deleted, the `AnyAsync` method returns false and the application just goes back to the Index method.-->
如果 department 已被删除， `AnyAsync` 方法会返回 false，然后应用程序返回到 Index 方法。

<!--If a concurrency error is caught, the code redisplays the Delete confirmation page and provides a flag that indicates it should display a concurrency error message.-->
如果捕获到并发错误，该代码将重新显示 Delete 确认页面，并提供一个表示应当显示并发错误消息的标志。

<!--### Update the Delete view-->
### 更新 Delete 视图

<!--In *Views/Department/Delete.cshtml*, replace the scaffolded code with the following code that adds an error message field and hidden fields for the DepartmentID and RowVersion properties. The changes are highlighted.-->
用以下添加有错误信息字段和 DepartmentID 与 RowVersion 属性隐藏字段的代码替换 *Views/Department/Delete.cshtml* 中的代码。变化部分已被高亮显示。

[!code-html[Main](intro/samples/cu/Views/Departments/Delete.cshtml?highlight=9,38,43-44)]

<!--This makes the following changes:-->
它将发生如下变化：

<!--* Adds an error message between the `h2` and `h3` headings.-->
* 在 `h2` 和 `h3` 标题之间添加错误消息。

<!--* Replaces LastName with FullName in the **Administrator** field.-->
* 在 **Administrator** 字段中用 FullName 替换 LastName。

<!--* Removes the RowVersion field.-->
* 移除 RowVersion 字段。

<!--* Adds hidden fields for the `DepartmentID` and `RowVersion` properties.-->
* 为 `DepartmentID` 和 `RowVersion` 属性添加隐藏字段。

<!--Run the Departments Index page. Right click the **Delete** hyperlink for the English department and select **Open in new tab**, then in the first tab click the **Edit** hyperlink for the English department.-->
运行 Departments Index 页。右键点击英语系的 **Delete** 超链并选择 **Open in new tab**，然后再在第一个标签页中点击英语系的 **Edit** 超链。

<!--In the first window, change one of the values, and click **Save**:-->
在第一个窗口中，随便选择一个值，修改一下，然后点击 **Save**：

![Department Edit page after change before delete](concurrency/_static/edit-after-change-for-delete.png)

<!--In the second tab, click **Delete**. You see the concurrency error message, and the Department values are refreshed with what's currently in the database.-->
在第二个标签页中，点击 **Delete**。你能看到一个并发错误消息，以及 Department 的值刷新为当前数据库中的值。

![Department Delete confirmation page with concurrency error](concurrency/_static/delete-error.png)

<!--If you click **Delete** again, you're redirected to the Index page, which shows that the department has been deleted.-->
如果你再次点击 **Delete**，你会被重新定向到 Index 页，你能在 Index 上看到该系已经被删除。

<!--## Update Details and Create views-->
## 更新 Details 和 Create 视图

<!--You can optionally clean up scaffolded code in the Details and Create views.-->
你可以选择在 Details 和 Create 视图中清理基架代码。

<!--Replace the code in *Views/Departments/Details.cshtml* to delete the RowVersion column and show the full name of the Administrator.-->
修改 *Views/Departments/Details.cshtml* 中的代码，将 RowVersion 列改为 Administrator 列。

[!code-html[Main](intro/samples/cu/Views/Departments/Details.cshtml?highlight=35)]

<!--Replace the code in *Views/Departments/Create.cshtml* to add a Select option to the drop-down list.-->
修改 *Views/Departments/Create.cshtml* 中的代码，在下拉菜单中添加一个选择项。

[!code-html[Main](intro/samples/cu/Views/Departments/Create.cshtml?highlight=38-40)]

<!--## Summary-->
## 总结

<!--This completes the introduction to handling concurrency conflicts. For more information about how to handle concurrency in EF Core, see [并发冲突](https://docs.microsoft.com/en-us/ef/core/saving/concurrency). The next tutorial shows how to implement table-per-hierarchy inheritance for the Instructor and Student entities.-->
至此完成了对处理并发异常的介绍。更多有关 EF Core 中如何处理异常的资料可以阅读 [并发冲突](https://docs.microsoft.com/en-us/ef/core/saving/concurrency)。下一篇教程将介绍如何为 Stdent 和 Instructor 实体实现表层次结构继承。

>[!div class="step-by-step"]
[上一节](update-related-data.md)
[下一节](inheritance.md)  

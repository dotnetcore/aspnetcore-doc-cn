---
title: ASP.NET Core MVC 与 EF Core - 更新关联数据 - 7 of 10  | Microsoft 文档（中文文档）
author: tdykstra
description: In this tutorial you'll update related data by updating foreign key fields and navigation properties.
keywords: ASP.NET Core 中文文档, Entity Framework Core, related data, joins
ms.author: tdykstra
manager: wpickett
ms.date: 03/15/2017
ms.topic: get-started-article
ms.assetid: 67bd162b-bfb7-4750-9e7f-705228b5288c
ms.technology: aspnet
ms.prod: asp.net-core
uid: data/ef-mvc/update-related-data
---

# 更新关联数据 - EF Core 与 ASP.NET Core MVC 教程 (7 of 10)

作者 [Tom Dykstra](https://github.com/tdykstra) 、 [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译 [刘怡(AlexLEWIS/Forerunner)](http://github.com/alexinea) 

<!--The Contoso University sample web application demonstrates how to create ASP.NET Core 1.1 MVC web applications using Entity Framework Core 1.1 and Visual Studio 2017. For information about the tutorial series, see [the first tutorial in the series](intro.md).-->
Contoso 大学 Web应用程序演示了如何使用 Entity Framework Core 1.1 以及 Visual Studio 2017 来创建 ASP.NET Core 1.1 MVC Web 应用程序。更多信息请参考 [第一节教程](intro.md).

<!--In the previous tutorial you displayed related data; in this tutorial you'll update related data by updating foreign key fields and navigation properties.-->
在上一篇教程中，你学习了如何显示关联数据；在本篇教程中你讲学习如何通过更新外键字段和导航属性来更新关联数据。

<!--The following illustrations show some of the pages that you'll work with.-->
以下插图显示了您将要使用的一些页面。

![Course Edit page](update-related-data/_static/course-edit.png)

![Instructor Edit page](update-related-data/_static/instructor-edit-courses.png)

<!--## Customize the Create and Edit Pages for Courses -->
## 为 Courses 定制 Create 和 Edit 页面

<!--When a new course entity is created, it must have a relationship to an existing department. To facilitate this, the scaffolded code includes controller methods and Create and Edit views that include a drop-down list for selecting the department. The drop-down list sets the `Course.DepartmentID` foreign key property, and that's all the Entity Framework needs in order to load the `Department` navigation property with the appropriate Department entity. You'll use the scaffolded code, but change it slightly to add error handling and sort the drop-down list.-->
当创建完一个新的 Course 实体，它就必须与现有的部门有联系。为方便起见，基架代码会包含控制其方法和含有部门选择下拉菜单的 Create 与 Edit 视图。下拉菜单设置了 `Course.DepartmentID` 外键属性，这是所有 Entity Framework 需要用来加载含有 Department 实体的 `Department` 导航属性的。你会使用到这些基架代码，但需要稍微修改一下它们，比如添加错误处理以及排序下拉列表等。


<!--In *CoursesController.cs*, delete the four Create and Edit methods and replace them with the following code:-->
在 *CoursesController.cs* 中，删除这个 Create 和 Edit 方法，并用以下代码替换之：

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_CreateGet)]

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_CreatePost)]

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_EditGet)]

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_EditPost)]

<!--After the `Edit` HttpPost method, create a new method that loads department info for the drop-down list.-->
在 `Edit` 的 HttpPost 方法后面添加一个新方法用于为下拉列表加载部门信息。

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?name=snippet_Departments)]

<!--The `PopulateDepartmentsDropDownList` method gets a list of all departments sorted by name, creates a `SelectList` collection for a drop-down list, and passes the collection to the view in `ViewBag`. The method accepts the optional `selectedDepartment` parameter that allows the calling code to specify the item that will be selected when the drop-down list is rendered. The view will pass the name "DepartmentID" to the `<select>` tag helper, and the helper then knows to look in the `ViewBag` object for a `SelectList` named "DepartmentID".-->
 `PopulateDepartmentsDropDownList` 方法用于获取所有部门的列表，并依据名称进行排序，为下拉列表创建一个 `SelectList` 集合，然后将集合通过 `ViewBag` 传入视图。该方法接受一个可选的 `selectedDepartment` 参数，该参数允许调用代码指定下拉列表渲染后的可供选择的项目。视图将名称「DepartmentID」传递给 `<select>` Tag Helper，然后 Tag Helper 会到 `ViewBag` 对象中查找名为「DepartmentID」的 `SelectList` 集合。

<!--The HttpGet `Create` method calls the `PopulateDepartmentsDropDownList` method without setting the selected item, because for a new course the department is not established yet:-->
HttpGet 版本的 `Create` 方法调用 `PopulateDepartmentsDropDownList` 方法时不设置选择项，是因为对于新课程而言部门尚未建立：

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?highlight=3&name=snippet_CreateGet)]

<!--The HttpGet `Edit` method sets the selected item, based on the ID of the department that is already assigned to the course being edited:-->
HttpGet 版本的 `Edit` 方法基于已分配给正在编辑的课程的部门 ID 设置选择项：

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?highlight=15&name=snippet_EditGet)]

<!--The HttpPost methods for both `Create` and `Edit` also include code that sets the selected item when they redisplay the page after an error. This ensures that when the page is redisplayed to show the error message, whatever department was selected stays selected.-->
 `Create` 和 `Edit` 的 HttpPost 方法还包括设置选择项的代码（当出现错误后重新显示页面时会使用到）。这就确保了当页面被重新显示并展示错误信息时，所选的部门将继续保持选中状态。

<!--### Add .AsNoTracking to Details and Delete methods-->
### 为 Details 和 Delete 方法添加预加载

<!--To optimize performance of the Course Details and Delete pages, add `AsNoTracking` calls in the `Details` and HttpGet `Delete` methods.-->
要想在 Course 的 Details 页面和 Delete 页面中显示部门数据的话，当然还要在 `Details` 和 HttpGet `Delete`方法中添加  `AsNoTracking` 来优化性能。

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?highlight=10&name=snippet_Details)]

[!code-csharp[Main](intro/samples/cu/Controllers/CoursesController.cs?highlight=10&name=snippet_DeleteGet)]

<!--### Modify the Course views-->
### 修改 Course 视图

<!--In *Views/Courses/Create.cshtml*, add a "Select Department" option to the **Department** drop-down list, and change the caption for the field from **DepartmentID** to **Department**.-->
在 *Views/Courses/Create.cshtml* 中，为 **Department** 下拉列表添加「选择部门」选项，然后将字段的标题从 **DepartmentID** 改为 **Department**。

[!code-html[Main](intro/samples/cu/Views/Courses/Create.cshtml?highlight=2,4,5,6&range=35-43)]

<!--In *Views/Courses/Edit.cshtml*, make the same change for the Department field that you just did in *Create.cshtml*.-->
在 *Views/Courses/Edit.cshtml* 中也作相同的改动（如你之前在 *Create.cshtml* 中所做的改动一样）。

<!--Also in *Views/Courses/Edit.cshtml*, add a course number field before the Credits field. Because it's the primary key, it's displayed, but it can't be changed.-->
在 *Views/Courses/Edit.cshtml* 中，在 Credits 字段之前添加课程编号字段。因为它是主键，所以该字段只显示、不可修改。

[!code-html[Main](intro/samples/cu/Views/Courses/Edit.cshtml?range=15-20)]

<!--There's already a hidden field (`<input type="hidden">`) for the course number in the Edit view. Adding a `<label>` tag helper doesn't eliminate the need for the hidden field because it doesn't cause the course number to be included in the posted data when the user clicks **Save** on the **Edit** page.-->
在 Edit 视图中已经有一个银行字段（`<input type="hidden">`）用于保存课程编号。 `<label>` Tag Helper 并不能代替这个隐藏字段，因为那样的话就不能在点击 **Save** 时 POST 包含课程编号的数据。

<!--In *Views/Courses/Delete.cshtml*, add a course number field at the top and change department ID to department name.-->
在 *Views/Course/Delete.cshtml* 中，在顶部添加课程编号字段，在标题字段之前添加一个部门名称字段。

[!code-html[Main](intro/samples/cu/Views/Courses/Delete.cshtml?highlight=14-19,36)]

<!--In *Views/Course/Details.cshtml*, make the same change that you just did for *Delete.cshtml*.-->
在 *Views/Course/Details.cshtml* 中也作相同的改动（如你之前在 *Delete.cshtml* 中所做的改动一样）。

<!--### Test the Course pages-->
### 测试 Course 页面

<!--Run the **Create** page (display the Course Index page and click **Create New**) and enter data for a new course:-->
运行 **Create** 页面（显示 COurse 索引页，并点击 **Create New**），然后输入一门新课程：

![Course Create page](update-related-data/_static/course-create.png)

<!--Click **Create**. The Courses Index page is displayed with the new course added to the list. The department name in the Index page list comes from the navigation property, showing that the relationship was established correctly.-->
点击 **Create**。然后你的这门课程就被添加到 Course 的索引页的列表中了。Index 页中的部门名来自于导航属性，表明关联关系以正确建立。

<!--Run the **Edit** page (click **Edit** on a course in the Course Index page ).-->
运行 **Edit** 页面（在 Course 索引页中点击 **Edit**）。

![Course Edit page](update-related-data/_static/course-edit.png)

<!--Change data on the page and click **Save**. The Courses Index page is displayed with the updated course data.-->
修改页面中的数据然后点击 **Save**。Course 索引页就会显示更新后的课程数据。

<!--## Add an Edit Page for Instructors-->
## 给 Instructors 添加一个 Edit 页 

<!--When you edit an instructor record, you want to be able to update the instructor's office assignment. The Instructor entity has a one-to-zero-or-one relationship with the OfficeAssignment entity, which means your code has to handle the following situations:-->
当你编辑教师记录时，你希望能够更新教师的办公室分配信息。Instructor 实体与 OfficeAssignment 实体之间存在一对零或一的关系，这意味着你的代码必须处理以下几种情况：

<!--* If the user clears the office assignment and it originally had a value, delete the OfficeAssignment entity.-->
* 如果办公室分配数据最初有值，而用户将其清除，则请删除 OfficeAssignment 实体。

<!--* If the user enters an office assignment value and it originally was empty, create a new OfficeAssignment entity.-->
* 如果办公室分配数据最初为空，而用户输入了一个值，则创建一个新的 OfficeAssignment 实体。

<!--* If the user changes the value of an office assignment, change the value in an existing OfficeAssignment entity.-->
* 如果修改了办公室分配的值，那么更新已有的 OfficeAssignment 实体中的值。

<!--### Update the Instructors controller-->
### 更新 Instructors 控制器

<!--In *InstructorsController.cs*, change the code in the HttpGet `Edit` method so that it loads the Instructor entity's `OfficeAssignment` navigation property and calls `AsNoTracking`:-->
在 *InstructorsController.cs* 中，修改 HttpGet 版本的 `Edit` 方法的代码，使其能够加载 Instructor 实体的 `OfficeAssignment` 导航属性，并调用 `AsNoTracking`：

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=9,10&name=snippet_EditGetOA)]

<!--Replace the HttpPost `Edit` method with the following code to handle office assignment updates:-->
用以下代码替换 HttpPost 版本的 `Edit` 方法，用来处理办公室分配数据的更新：

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?name=snippet_EditPostOA)]

<!--The code does the following:-->
这段代码做了这些事：

<!---  Changes the method name to `EditPost` because the signature is now the same as the HttpGet `Edit` method (the `ActionName` attribute specifies that the `/Edit/` URL is still used).-->
-  讲方法名改为 `EditPost` ，因为方法签名此时与 HttpGet 版本的 `Edit` 方法一样了（此处依旧将 `ActionName` 特性指定为  `/Edit/` ）。

<!---  Gets the current Instructor entity from the database using eager loading for the `OfficeAssignment` navigation property. This is the same as what you did in the HttpGet `Edit` method.-->
-  为导航属性 `OfficeAssignment` 从数据库中预加载当前的 Instructor 实体。这和你在 HttpGet 的 `Edit` 方法中做的一样。

<!---  Updates the retrieved Instructor entity with values from the model binder. The `TryUpdateModel` overload enables you to whitelist the properties you want to include. This prevents over-posting, as explained in the [第二篇教程](crud.md).-->
-  用来自模型绑定器的值更新检索到的 Instructor 实体。重载的  `TryUpdateModel` 方法能使你将需要包含的属性列入白名单。这能防止过度发布（Over-Posting），更多解释可以查看 [第二篇教程](crud.md)。

    <!-- Snippets do not play well with <ul> [!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?range=241-244)] -->

    ```csharp
    if (await TryUpdateModelAsync<Instructor>(
        instructorToUpdate,
        "",
        i => i.FirstMidName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment))
	```
	
<!---   If the office location is blank, sets the Instructor.OfficeAssignment property to null so that the related row in the OfficeAssignment table will be deleted.-->
-   如果办公室地点是空的，那么把 Instructor.OfficeAssignment 属性置为 null，如此一来 OfficeAssignment 表中的关联行就会被删除。

    <!-- Snippets do not play well with <ul>  "intro/samples/cu/Controllers/InstructorsController.cs"} -->

    ```csharp
    if (String.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment?.Location))
    {
        instructorToUpdate.OfficeAssignment = null;
    }
    ```

<!--- Saves the changes to the database.-->
- 保存变更到数据库。

<!--### Update the Instructor Edit view-->
### 更新 Instructor Edit 视图

<!--In *Views/Instructors/Edit.cshtml*, add a new field for editing the office location, at the end before the **Save** button :-->
在 *Views/Instructors/Edit.cshtml* 中添加一个新字段用来编辑办公室位置信息，放在 **Save** 按钮之前：

[!code-html[Main](intro/samples/cu/Views/Instructors/Edit.cshtml?range=36-42)]

<!--Run the page (select the **Instructors** tab and then click **Edit** on an instructor). Change the **Office Location** and click **Save**.-->
运行页面（选择 **Instructors** 标签并在教师旁点击 **Edit**）。修改 **Office Location** 并点击 **Save**。

![Instructor Edit page](update-related-data/_static/instructor-edit-office.png)

<!--## Add Course assignments to the Instructor Edit page-->
## 将课程分配信息添加到 Instructor Edit 页面

<!--Instructors may teach any number of courses. Now you'll enhance the Instructor Edit page by adding the ability to change course assignments using a group of check boxes, as shown in the following screen shot:-->
每个教师可以教授多门课程，现在你将通过使用一组复选框更改课程分配的功能来增强 Instructor 的 Edit 页，如下截图所示：

![Instructor Edit page with courses](update-related-data/_static/instructor-edit-courses.png)

<!--The relationship between the Course and Instructor entities is many-to-many. To add and remove relationships, you add and remove entities to and from the CourseAssignments join entity set.-->
Course 和 Instructor 实体之间的关系是多对多关系。要添加和删除关系，你需要添加实体类到 InstructorCourses 连接实体集/从 InstructorCourses 连接实体集中移除。

<!--The UI that enables you to change which courses an instructor is assigned to is a group of check boxes. A check box for every course in the database is displayed, and the ones that the instructor is currently assigned to are selected. The user can select or clear check boxes to change course assignments. If the number of courses were much greater, you would probably want to use a different method of presenting the data in the view, but you'd use the same method of manipulating a join entity to create or delete relationships.-->
能让你更新教师分配给哪些课程的用户界面是一组复选框。库中每一门课程都会显示有一个复选框，教师当前分配到的课程的复选框默认选中。用户可以选中或取消选中复选框来改变课程的分配。如果课程的数量太大，你可能需要使用不同的方法在视图中呈现数据，但你可以使用相同的方法来操作连接实体以创建或删除关系。

<!--### Update the Instructors controller-->
### 更新 Instructors 控制器
 
<!--To provide data to the view for the list of check boxes, you'll use a view model class.-->
为了能给视图中的复选框列表提供数据，你必须使用视图模型类。
<!--
Create *AssignedCourseData.cs* in the *SchoolViewModels* folder and replace the existing code with the following code:-->
在 *SchoolViewModels* 文件夹中创建 *AssignedCourseData.cs* 文件，然后用下面的代码替换之：
 
[!code-csharp[Main](intro/samples/cu/Models/SchoolViewModels/AssignedCourseData.cs)]

<!--In *InstructorsController.cs*, replace the HttpGet `Edit` method with the following code. The changes are highlighted.-->
在 *InstructorsController.cs* 中，用下面的代码替换 HttpGet 版本的 ``Edit`` 方法，高亮部分为变更部分。

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=10,17,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36&name=snippet_EditGetCourses)]

<!--The code adds eager loading for the `Courses` navigation property and calls the new `PopulateAssignedCourseData` method to provide information for the check box array using the `AssignedCourseData` view model class.-->
代码为 `Courses` 导航属性添加了预加载，并调用新的 `PopulateAssignedCourseData` 方法，以为使用 `AssignedCourseData` 视图模型类的复选框数组提供信息。

<!--The code in the `PopulateAssignedCourseData` method reads through all Course entities in order to load a list of courses using the view model class. For each course, the code checks whether the course exists in the instructor's `Courses` navigation property. To create efficient lookup when checking whether a course is assigned to the instructor, the courses assigned to the instructor are put into a `HashSet` collection. The `Assigned` property  is set to true for courses the instructor is assigned to. The view will use this property to determine which check boxes must be displayed as selected. Finally, the list is passed to the view in `ViewData`.-->
 `PopulateAssignedCourseData` 方法中的代码读取所有 Course 实体，用于使用视图模型类加载课程列表。对于每一门课程，代码都会检查课程是否存在于教师的 `Courses` 导航属性之中。为了在检查课程是否被分配给教师的过程中尽可能高效，分配给教师的课程被放入一个 `HashSet` 集合中。如果课程已被分配给教师，则 `Assigned` 属性置为 true。视图将使用此属性来确定那些复选框必须显示为已选中的状态。最后，列表通过 `ViewData` 被传递给视图。

<!--Next, add the code that's executed when the user clicks **Save**. Replace the `EditPost` method with the following code, and add a new method that updates the `Courses` navigation property of the Instructor entity.-->
接下来，添加点击 **Save** 后的执行代码。用下面代码替换 `EditPost` 方法，并添加一个新方法，用于更新 Instructor 实体中的 `Courses` 导航属性。

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=3,12,13,25,39-40&name=snippet_EditPostCourses)]

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?name=snippet_UpdateCourses&highlight=1-31)]

<!--The method signature is now different from the HttpGet `Edit` method, so the method name changes from `EditPost` back to `Edit`.-->
由于方法签名现在有别于 HttpGet 版本的 `Edit` 方法，因此可以将方法名从 `EditPost` 改回 `Edit`。

<!--Since the view doesn't have a collection of Course entities, the model binder can't automatically update the `CourseAssignments` navigation property. Instead of using the model binder to update the `CourseAssignments` navigation property, you do that in the new `UpdateInstructorCourses` method. Therefore you need to exclude the `CourseAssignments` property from model binding. This doesn't require any change to the code that calls `TryUpdateModel` because you're using the whitelisting overload and `CourseAssignments` isn't in the include list.-->
由于没有 Course 实体的集合，模型绑定器不能自动更新 `CourseAssignments` 导航属性。所以你需要在新的 `UpdateInstructorCourses` 方法中实现 `CourseAssignments` 导航属性的更新。因此你需要从模型绑定器中排除 `CourseAssignments` 属性。这里不需要对调用 `TryUpdateModel` 的代码做任何变动，因为你正使用白名单重载，而  `CourseAssignments` 并不在该列表中。

<!--If no check boxes were selected, the code in `UpdateInstructorCourses` initializes the `CourseAssignments` navigation property with an empty collection and returns:-->
如果没有选中任何复选框， `UpdateInstructorCourses` 中的代码会使用空集合初始化 `CourseAssignments` 导航属性并返回：

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?name=snippet_UpdateCourses&highlight=3-7)]

<!--The code then loops through all courses in the database and checks each course against the ones currently assigned to the instructor versus the ones that were selected in the view. To facilitate efficient lookups, the latter two collections are stored in `HashSet` objects.-->
代码循环遍历数据库中的所有课程，根据当前分配给教师的课程和视图中被选中的课程检查每一门课程。为方便高效查找，后两个集合被保存在 `HashSet` 对象中。

<!--If the check box for a course was selected but the course isn't in the `Instructor.CourseAssignments` navigation property, the course is added to the collection in the navigation property.-->
如果课程复选框已选中但课程不在 `Instructor.CourseAssignments` 导航属性中，则该门课程就会被加入到导航属性集合中。

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=14-20&name=snippet_UpdateCourses)]

<!--If the check box for a course wasn't selected, but the course is in the `Instructor.CourseAssignments` navigation property, the course is removed from the navigation property.-->
如果课程复选框未选择，但该课程在 `Instructor.CourseAssignments` 导航属性中，则从导航属性中移除该课程。

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=21-29&name=snippet_UpdateCourses)]

<!--### Update the Instructor views-->
### 更新 Instructor 视图

<!--In *Views/Instructors/Edit.cshtml*, add a **Courses** field with an array of check boxes by adding the following code immediately after the `div` elements for the **Office** field and before the `div` element for the **Save** button. -->
在 *Views/Instructors/Edit.cshtml* 中，通过在 **Office** 字段的**Office** 元素之后添加以下代码，在复选框的数组中添加一个 **Courses** 字段。 `div` 元素为 **保存** 按钮。

<a id="notepad"></a>
<!--> [!NOTE] 
> Open the file in a text editor such as Notepad to make this change.  If you use Visual Studio, line breaks will be changed in a way that breaks the code.  If that happens, fix the line breaks so that they look like what you see here. The indentation doesn't have to be perfect, but the `@</tr><tr>`, `@:<td>`, `@:</td>`, and `@:</tr>` lines must each be on a single line as shown or you'll get a runtime error. After editing the file in a text editor, you can open it in Visual Studio, highlight the block of new code, and press Tab twice to line up the new code with the existing code.-->
> [!NOTE] 
> 在 Notepad 之类的文本编辑器中打开文件并做如上修改。如果你使用 Visual Studio，换行符会在某种程度上破坏代码。如果发生这种情况，请修复换行符，使其看起来如你在此处所看到的。缩进不需要很完美，但 `@</tr><tr>`、`@:<td>`、`@:</td>` 以及 `@:</tr>` 行必须放置在单独一行上，如图所示，不然会出现运行时错误。在文本编辑器中修改之后，你可以在 Visual Studio 中打开，高亮新代码块并按两次 Tab 键实使新老代码对齐。

[!code-html[Main](intro/samples/cu/Views/Instructors/Edit.cshtml?range=43-69)]

<!--This code creates an HTML table that has three columns. In each column is a check box followed by a caption that consists of the course number and title. The check boxes all have the same name ("selectedCourses"), which informs the model binder that they are to be treated as a group. The value attribute of each check box is set to the value of `CourseID`. When the page is posted, the model binder passes an array to the controller that consists of the `CourseID` values for only the check boxes which are selected.-->
此代码创建一个包含三个列的 HTML 表格。在每一列上都有一个复选框，后面跟着一门包含课程编号和课程标题的文字。复选框都具有相同的名称（name 属性，「selectedCourses」），它将告诉模型帮顶起将它们视作一组。每一个复选框的值都被设置为 `CourseID` 的值。当页面被 POST，模型绑定器将一个数组传递给控制器，该数组只包含所有选中的复选框的 `CourseID` 值。

<!--When the check boxes are initially rendered, those that are for courses assigned to the instructor have checked attributes, which selects them (displays them checked).-->
之前渲染的复选框带有 checked 属性，显示为选中的表示教师选择教授这门课程。

<!--Run the Instructor Index page, and click **Edit** on an instructor to see the **Edit** page.-->
运行 Instructor 索引页，点击某个教师胖的 **Edit** 就能看到（该教师的）**Edit** 页面。

![Instructor Edit page with courses](update-related-data/_static/instructor-edit-courses.png)

<!--Change some course assignments and click Save. The changes you make are reflected on the Index page.-->
修改一些课程分配信息，然后点击 Save。你做的更新将反映在 Index 页上。

<!--> [!NOTE] 
> The approach taken here to edit instructor course data works well when there is a limited number of courses. For collections that are much larger, a different UI and a different updating method would be required.-->
> [!NOTE] 
> 当课程数量有限时，在这里编辑教师课程数据会更好一些。对于大集合，需要用不同的 UI 和不同的更新方法。

<!--## Update the Delete page-->
## 更新 Delete 页面

<!--In *InstructorsController.cs*, delete the `DeleteConfirmed` method and insert the following code in its place.-->
在 *InstructorsController.cs* 中删除 `DeleteConfirmed` 方法，并输入以下代码。

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?highlight=5-7,9-12&name=snippet_DeleteConfirmed)]

<!--This code makes the following changes:-->
这段代码做了如下改变：

<!--* Does eager loading for the `CourseAssignments` navigation property.  You have to include this or EF won't know about related `CourseAssignment` entities and won't delete them.  To avoid needing to read them here you could configure cascade delete in the database.

* If the instructor to be deleted is assigned as administrator of any departments, removes the instructor assignment from those departments.-->
* 预加载 `CourseAssignments` 导航属性。你必须加上这段，不然 EF 不会知道关联的 `CourseAssignments` 实体，并且不会删除它们。为避免在此处读取它们，你需要到数据库中配置级联删除。

* 如果要被删除的教师已被分配为某个部门的管理员，那么就从这些部门中删除该教师的分配信息。

<!--## Add office location and courses to the Create page-->
## 将办公室地点和课程添加到 Create 页面

<!--In *InstructorController.cs*, delete the HttpGet and HttpPost `Create` methods, and then add the following code in their place:-->
在 *InstructorController.cs* 中删除 HttpGet 和 HttpPost 版本的 `Create` 方法，然后添加以下代码以代替它们：

[!code-csharp[Main](intro/samples/cu/Controllers/InstructorsController.cs?name=snippet_Create&highlight=3-5,12,14-22,29)]

<!--This code is similar to what you saw for the `Edit` methods except that initially no courses are selected. The HttpGet `Create` method calls the `PopulateAssignedCourseData` method not because there might be courses selected but in order to provide an empty collection for the `foreach` loop in the view (otherwise the view code would throw a null reference exception).-->
除了最初没有选择课程，这段代码与之前在 ``Edit`` 方法中看到的代码非常相似。HttpGet 请求的 `Create` 方法调用 `PopulateAssignedCourseData` 方法并不是因为可能会有课程被选择，而是为了给 `foreach` 循环提供一个空集合（避免在视图代码中出现空引用异常）。

<!--The HttpPost `Create` method adds each selected course to the `CourseAssignments` navigation property before it checks for validation errors and adds the new instructor to the database. Courses are added even if there are model errors so that when there are model errors (for an example, the user keyed an invalid date), and the page is redisplayed with an error message, any course selections that were made are automatically restored.-->
HttpPost 请求的 `Create` 方法在检查校验错误并将教师插入数据库之前将每个被选择的课程添加到 `CourseAssignments` 导航属性中。即使模型出现错误，也会添加课程，以便当出现错误（例如用户键入了无效日期）且页面重新显示出错误信息时，所有课程选择都将被自动还原。

<!--Notice that in order to be able to add courses to the `CourseAssignments` navigation property you have to initialize the property as an empty collection:-->
注意，为了能将课程添加到 `CourseAssignments` 导航属性中，你必须将属性初始化为一个空集合：

```csharp
instructor.CourseAssignments = new List<CourseAssignment>();
```

<!--As an alternative to doing this in controller code, you could do it in the Instructor model by changing the property getter to automatically create the collection if it doesn't exist, as shown in the following example:-->
作为在控制器代码中执行此操作的替代方法，你可以通过修改 Instructor 模型中属性 Getter 访问器，当集合不存在的时候给它自动创建一个，这样该操作就能放入 Instructor 了，如下例所示：

```csharp
private ICollection<CourseAssignment> _courseAssignments;
public ICollection<CourseAssignment> CourseAssignments
{
    get
    {
        return _courseAssignments ?? (_courseAssignments = new List<CourseAssignment>());
    }
    set
    {
        _courseAssignments = value;
    }
}
```

<!--If you modify the `CourseAssignments` property in this way, you can remove the explicit property initialization code in the controller.-->
如果你这么改 `CourseAssignments` 属性，可以删除控制器中显示属性的初始化代码。

<!--In *Views/Instructor/Create.cshtml*, add an office location text box and check boxes for courses before the Submit button. As in the case of the Edit page, this will work better if you [用 Notepad 这类文本软件来编辑](#notepad).-->
在 *Views/Instructor/Create.cshtml* 中添加一个表示「办公室位置」的文本框，并在「出租日期」字段和提交（Submit）按钮之间添加课程复选框。和在 Edit 页面中的情况一样，如果你[用 Notepad 这类文本软件来编辑](#notepad)可能会更好些。

[!code-html[Main](intro/samples/cu/Views/Instructors/Create.cshtml?range=35-69)]

<!--Test by running the **Create** page and adding an instructor. -->
测试一下看看：运行 **Create** 页面，然后添加一个教师。

<!--## Handling Transactions-->
## 处理事务

<!--As explained in the [CRUD 教程](crud.md), the Entity Framework implicitly implements transactions. For scenarios where you need more control -- for example, if you want to include operations done outside of Entity Framework in a transaction -- see [事务](https://docs.microsoft.com/ef/core/saving/transactions).-->
如在 [CRUD 教程](crud.md) 中所解释的，Entity Framework 会隐式地实现事务。对于需要更多控制的场景——比如，如果你想在事务中包含 Entity Framework 之外的操作——具体可以参见 [事务](https://docs.microsoft.com/ef/core/saving/transactions)。

<!--## Summary-->
## 总结
 
<!--You have now completed the introduction to working with related data. In the next tutorial you'll see how to handle concurrency conflicts.-->
你已完成使用关联数据的全部介绍，在下一篇教程中你将学习如何处理并发冲突。

>[!div class="step-by-step"]
[上一节](read-related-data.md)
[下一节](concurrency.md)  

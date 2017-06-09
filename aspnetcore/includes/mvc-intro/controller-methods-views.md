我们会在下一篇文章中继续发掘 [DataAnnotations](http://msdn.microsoft.com/library/system.componentmodel.dataannotations.aspx) 的内容。 [Display](https://msdn.microsoft.com/library/system.componentmodel.dataannotations.displayattribute.aspx) 标签用来指定字段的显示名（在本示例中 "Release Date" 会替代 "ReleaseDate"）。 [DataType](https://msdn.microsoft.com/library/system.componentmodel.dataannotations.datatypeattribute.aspx) 特性指定数据类型，在本示例是日期类型，所以字段中存储的时间信息不会被显示。

浏览 `Movies` 控制器并把鼠标悬停于 **Edit** 链接上可以看到目标 URL。

![在浏览窗口把鼠标悬停在编辑链接上会显示链接 http://localhost:1234/Movies/Edit/5 ](../../tutorials/first-mvc-app/controller-methods-views/_static/edit7.png)

**Edit**、**Details** 以及 **Delete** 链接是由 *Views/Movies/Index.cshtml* 文件中的 MVC Core Anchor Tag Helper 自动生成的。

[!code-HTML[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Views/Movies/IndexOriginal.cshtml?highlight=1-3&range=46-50)]

[Tag Helpers](xref:mvc/views/tag-helpers/intro) 允许服务器端代码在 Razor 文件中创建和生成 HTML 元素。在上面的代码中， `AnchorTagHelper`  通过 controller 方法以及路由ID 动态生成 HTML `href` 属性值。你可以在你熟悉的浏览器中使用 **View Source** 菜单或者使用 **F12** 工具来检查你生成的 HTML 标签。 **F12** 工具如下图。

```html
 <td>
    <a href = "/Movies/Edit/4" > Edit </ a > |
    < a href="/Movies/Details/4">Details</a> |
    <a href = "/Movies/Delete/4" > Delete </ a >
</ td >
```

在 *Startup.cs* 文件中设置回调[路由](xref:mvc/controllers/routing)格式。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Startup.cs?name=snippet_1&highlight=5)]

ASP.NET Core 会把 `http://localhost:1234/Movies/Edit/4` 转化成发送到 `Movies` controller 的 `Edit` 方法的请求并带上值为 4 的 `Id` 参数。（ Controller 方法其实就是指代 action 方法。）

[Tag Helpers](xref:mvc/views/tag-helpers/intro)  是 ASP.NET Core 中最受欢迎的新功能之一。参考 [附录资源](#additional-resources) 获取更多信息。

打开 `Movies` controller 并查看两个 `Edit` 方法，下面的代码展示了 `HTTP GET Edit` 方法。这个方法读取 movie 数据并显示 *Edit.cshtml* Razor 文件生成的编辑表单页面。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/MC1.cs?name=snippet_edit1)]

接下来的方法展示 `HTTP POST Edit` 方法，处理 POST 过来的 movie 数据：

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/MC1.cs?name=snippet_edit2)]

`[Bind]`  特性是防止 [over-posting](https://docs.microsoft.com/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost) （过度提交，客户端可能发送比期望还多的数据，比如只需要2个属性但是发送了3个属性）的一种方法。
你应该只把需要改变的属性包含到 ``[Bind]``  特性中。请参阅 [Protect your controller from over-posting](http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost) 获取更多信息。 [ViewModels](http://rachelappel.com/use-viewmodels-to-manage-data-amp-organize-code-in-asp-net-mvc-applications/) 提供了另外一种方式防止过度提交。

请注意带第二个`Edit` 方法被 `[HttpPost]` 特性所修饰。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/MC1.cs?name=snippet_edit2&highlight=4)]

`HttpPost`  标签指定这个 `Edit` 方法 *只能* 被 `POST`  请求调用。你可以把 `[HttpGet]` 标签应用到第一个 edit方法，但是，不是必须的，因为 `[HttpGet]` 是被默认使用的。

`ValidateAntiForgeryToken` 标签是用来防止伪造请求的，会在（*Views/Movies/Edit.cshtml*）视图最终呈现文件中加入反伪造标记和服务器进行配对。edit 视图生成反伪造标记请参考 [Form Tag Helper](xref:mvc/views/working-with-forms)。

[!code-HTML[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Views/Movies/Edit.cshtml?range=9)]

[Form Tag Helper](xref:mvc/views/working-with-forms) 生成一个隐藏域的防伪标记必须和 Movies controller 的e `Edit` 方法的 `[ValidateAntiForgeryToken]` 产生的防伪标记相匹配。更多信息请参考 [Anti-Request Forgery](xref:security/anti-request-forgery)。

 `HttpGet Edit` 方法获取 movie 的 `ID` 参数，通过使用 Entity Framework 的 `SingleOrDefaultAsync` 方法查找 movie，并将选中的 movie 填充到 Edit 视图。如果 movie 没有找到，返回 `NotFound` （HTTP 404）响应。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/MC1.cs?name=snippet_edit1)]

在基架系统创建 Edit 视图的时候，会检查 `Movie` 类并为它的每个属性生成代码以呈现 `<label>` 和 `<input>` 元素。下面的例子展示了 Visual Studio 基架系统生成的 Edit 视图：

[!code-HTML[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Views/Movies/EditCopy.cshtml?highlight=1)]

你会注意到为什么视图模版文件的顶部会有一行 `@model MvcMovie.Models.Movie` 声明呢？— 因为这个声明指定这个视图模版的模型期待的类型是 `Movie`。
 
基架生成的代码使用几个 Tag Helper 方法来简化 HTML 标记。 [Label Tag Helper](xref:mvc/views/working-with-forms) 用来显示字段名（"Title"、"ReleaseDate"、"Genre" 或者 "Price"）。[Input Tag Helper](xref:mvc/views/working-with-forms) 用来呈现 HTML `<input>` 元素。 [Validation Tag Helper](xref:mvc/views/working-with-forms) 显示关联到属性的错误信息。

运行应用程序并导航到 `/Movies` URL。单击 **编辑** 链接。在浏览器中查看该页面的源代码。为 `<form>` 元素生成的 HTML 如下所示。

[!code-HTML[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Views/Shared/edit_view_source.html?highlight=1,6,10,17,24,28)]

 `HTML <form>` 中的 `<input>` 元素的 `action` 属性用于设置请求发送到 `/Movies/Edit/id` URL。当点击 `Save` 按钮时表单数据会被发送到服务器。在 `</form>` 元素关闭前最后一行 `</form>` 展示了  [Form Tag Helper](xref:mvc/views/working-with-forms) 生成的 [XSRF](xref:security/anti-request-forgery) 隐藏域标识。

## 处理 POST 请求

下面的列表显示了 `[HttpPost]` 不同版本的 `Edit` 方法。

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/MC1.cs?name=snippet_edit2)]

 `[ValidateAntiForgeryToken]` 标签验证 [Form Tag Helper](xref:mvc/views/working-with-forms) 生成的存放在隐藏域中的 [XSRF](xref:security/anti-request-forgery) 反伪造标记。

[模型绑定](xref:mvc/models/model-binding) 机制以发送表单数据创建 `Movie` 对象并作为 `movie` 参数。 `ModelState.IsValid` 方法验证表单提交的数据可以用来修改（编辑或更新）一个 `Movie` 对象。如果数据有效，就可以保存。更新（编辑） movie 数据会被存到数据库通过 database context 的 `SaveChangesAsync` 方法。数据保存完毕以后，这段代码将用户重定向到 `MoviesController` 类的 `Index` 方法，这个页面显示了改动后最新的Movie集合。

表单数据被发布到服务器之前，客户端校验会检查所有字段上的验证规则。如果有任何验证错误，则显示错误消息，并且表单数据不会被发送。如果禁用了 JavaScript，将不会有客户端验证，但服务器端将检测出发送数据是无效的，表单依旧会显示出错误信息。在稍后的教程中，我们会探讨 [模型验证](xref:mvc/models/validation) 更多关于验证的细节。 *Views/Book/Edit.cshtml* 视图模版中的 [验证Tag Helper](xref:mvc/views/working-with-forms)  负责显示错误信息。

![Edit view: An exception for an incorrect Price value of abc states that The field Price must be a number. An exception for an incorrect Release Date value of xyz states Please enter a valid date.](../../tutorials/first-mvc-app/controller-methods-views/_static/val.png)

movie controller 的所有 `HttpGet` 方法都遵循类似的模式。它们获取一个对象（或者对象列表，比如 `Index`），把对象（模型）传递到视图。 `Create` 方法创建一个空的对象到 `Create` 视图。诸如 Create、Edit、Delete 等之类的会修改数据的方法都会在 `[HttpPost]` 版本的重载方法中这样做（*译者注：*执行类似于前文所述的这些操作）。在 HTTP GET 方法中修改数据有安全风险。在 `HTTP GET` 方法中修改数据同样也违反 HTTP 最佳实践以及 [REST](http://rest.elkstein.org/) 架构模式，其中规定 GET 请求不应该更改应用程序的状态。换句话说，执行 GET 操作应该是没有任何副作用，不会修改您的持久化的数据。

## 其他资源

* [全球化于本地化](xref:fundamentals/localization)
* [介绍 Tag Helpers](xref:mvc/views/tag-helpers/intro)
* [Authoring Tag Helpers](xref:mvc/views/tag-helpers/authoring)
* [Anti-Request Forgery](xref:security/anti-request-forgery)
* Protect your controller from [over-posting](http://www.asp.net/mvc/overview/getting-started/getting-started-with-ef-using-mvc/implementing-basic-crud-functionality-with-the-entity-framework-in-asp-net-mvc-application#overpost)
* [视图模型](http://rachelappel.com/use-viewmodels-to-manage-data-amp-organize-code-in-asp-net-mvc-applications/)
* [Form Tag Helper](xref:mvc/views/working-with-forms)
* [Input Tag Helper](xref:mvc/views/working-with-forms)
* [Label Tag Helper](xref:mvc/views/working-with-forms)
* [Select Tag Helper](xref:mvc/views/working-with-forms)
* [Validation Tag Helper](xref:mvc/views/working-with-forms)
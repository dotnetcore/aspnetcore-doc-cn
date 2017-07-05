[!code-csharp[Main](../../tutorials/first-web-api/sample/TodoApi/Controllers/TodoController2.cs?name=snippet_todo1)]

<!--The preceding code:-->
在前面的代码中：

<!--* Defines an empty controller class. In the next sections, we'll add methods to implement the API.
* The constructor uses [Dependency Injection](xref:fundamentals/dependency-injection) to inject the database context (`TodoContext `) into the controller. The database context is used in each of the [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) methods in the controller.
* The constructor adds an item to the in-memory database if one doesn't exist.-->
* 定义一个空控制器类。 在接下来的教程中，我们将添加 API 方法的实现。
* 控制器使用 [依赖注入](xref:fundamentals/dependency-injection) 来把数据库上下文 (`TodoContext `) 注入到控制器。数据库上下文在每个控制器的 [CRUD](https://en.wikipedia.org/wiki/Create,_read,_update_and_delete) 方法中都被用到。
* 构造函数将一个数据项添加到内存数据库（如果不存在）。

<!--## Getting to-do items-->
## 获取 to-do 列表


<!--To get to-do items, add the following methods to the `TodoController` class.-->
为了获取 to-do 项，添加下列方法到 `TodoController` 类 。

[!code-csharp[Main](../../tutorials/first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=snippet_GetAll)]

<!--These methods implement the two GET methods:-->
代码实现了两个 GET 方法:

* `GET /api/todo`
* `GET /api/todo/{id}`

<!--Here is an example HTTP response for the `GetAll` method:-->
以下是 `GetAll` 方法 HTTP 响应:

```
HTTP/1.1 200 OK
   Content-Type: application/json; charset=utf-8
   Server: Microsoft-IIS/10.0
   Date: Thu, 18 Jun 2015 20:51:10 GMT
   Content-Length: 82

   [{"Key":"1", "Name":"Item1","IsComplete":false}]
   ```

<!--Later in the tutorial I'll show how you can view the HTTP response using [Postman](https://www.getpostman.com/) or or [curl](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man1/curl.1.html).-->
在后面的教程中，我将会告诉你如何使用 [Postman](https://www.getpostman.com/) 或者 [curl](https://developer.apple.com/legacy/library/documentation/Darwin/Reference/ManPages/man1/curl.1.html)  工具查看 HTTP 响应。

<!--### Routing and URL paths-->
### 路由和 URL 路径

<!--The `[HttpGet]` attribute specifies an HTTP GET method. The URL path for each method is constructed as follows:-->
 `[HttpGet]` 标签指定这些方法均为 HTTP GET 方法。 每个方法构建的 Url 如下:

<!--* Take the template string in the controller’s route attribute:
[!code-csharp[Main](../../tutorials/first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=TodoController&highlight=3)]
* Replace "[Controller]" with the name of the controller, which is the controller class name minus the "Controller" suffix. For this sample, the controller class name is **Todo**Controller and the root name is "todo". ASP.NET Core [routing](xref:mvc/controllers/routing) is not case sensitive.
* If the `[HttpGet]` attribute has a route template (such as `[HttpGet("/products")]`, append that to the path. This sample doesn't use a template. See [Attribute routing with Http[Verb] attributes](xref:mvc/controllers/routing#attribute-routing-with-httpverb-attributes) for more information.-->
* 替换 controller 模版里面的路由标签:
[!code-csharp[Main](../../tutorials/first-web-api/sample/TodoApi/Controllers/TodoController.cs?name=TodoController&highlight=3)]
* 把 "[Controller]" 替换为控制器名, 必须是带 "Controller" 后缀的小写名称. 在本示例里面控制器的名字为 "todo"  (不区分大小写). 对于这个例子, controller 的类名是 **Todo**Controller 并且根名是 "todo". ASP.NET MVC Core [路由](xref:mvc/controllers/routing) 是需要区分大小写的 。
* 如果  `[HttpGet]`  标签有模版字符串, 附加到路径 。 本示例没有模版字符串 。参考 [使用 Http [Verb] 属性进行属性路由](xref:mvc/controllers/routing#attribute-routing-with-httpverb-attributes) 获取更多信息。

<!--In the `GetById` method:-->
在 `GetById` 方法中：

```csharp
[HttpGet("{id}", Name = "GetTodo")]
public IActionResult GetById(long id)
```

<!--`"{id}"` is a placeholder variable for the ID of the `todo` item. When `GetById` is invoked, it assigns the value of "{id}" in the URL to the method's `id` parameter.-->
在实际的 HTTP 请求中`"{id}"` 是一个占位符, 客户端在运行时会使用 `todo` 项的 ID 属性, 当  `GetById`  被调用时候 , 会把 "{id}" 占位符分配到 Url 方法的 `id` 参数上去 。

<!--`Name = "GetTodo"` creates a named route and allows you to link to this route in an HTTP Response. I'll explain it with an example later. See [Routing to Controller Actions](xref:mvc/controllers/routing) for detailed information.-->
`Name = "GetTodo"` creates a named route and allows you to link to this route in an HTTP Response. I'll explain it with an example later. See [Routing to Controller Actions](xref:mvc/controllers/routing) for detailed information.

<!--### Return values-->
### 返回值

<!--The `GetAll` method returns an `IEnumerable`. MVC automatically serializes the object to [JSON](http://www.json.org/) and writes the JSON into the body of the response message. The response code for this method is 200, assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)-->
 `GetAll` 方法返回一个  `IEnumerable` 对象 。MVC 自动把对象序列化为 [JSON](http://www.json.org/)   并把 JSON 对象写入响应消息正文. 响应状态码为 200, 假设没有未处理异常的情况下。（未处理异常一般会被转化为 5xx 错误。）

<!--In contrast, the `GetById` method returns the more general `IActionResult` type, which represents a wide range of return types. `GetById` has two different return types:-->
相反, `GetById` 将会返回一个 `IActionResult` 类型, 代表一个更加通用的结果对象. 因为 `GetById` 有两个不同的返回值:

<!--* If no item matches the requested ID, the method returns a 404 error.  This is done by returning `NotFound`.-->
* 如果没有数据项可以匹配 ID, 方法会返回 404 错误，并最终以返回 `NotFound`。

<!--* Otherwise, the method returns 200 with a JSON response body. This is done by returning an `ObjectResult`-->
* 否则, 方法会返回 200 以及 JSON 响应正文。并最终以返回 `ObjectResult`。


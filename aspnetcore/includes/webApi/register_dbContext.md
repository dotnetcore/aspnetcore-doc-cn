<!--## Register the database context-->
## 注册数据库上下文

<!--In order to inject the database context into the controller, we need to register it with the [dependency injection](xref:fundamentals/dependency-injection) container. Register the database context with the service container using the built-in support for [dependency injection](xref:fundamentals/dependency-injection). Replace the contents of the *Startup.cs* file with the following:-->
为了将数据库上下文注入到控制器，我们必须在 [依赖注入](xref:fundamentals/dependency-injection) 容器中注册它。 把数据库上下文注册服务容器使用内置 [依赖注入](xref:fundamentals/dependency-injection) 支持。 使用一下代码替换掉 *Startup.cs* 文件。

[!code-csharp[Main](../../tutorials/first-web-api/sample/TodoApi/Startup.cs?highlight=2,4,12)]

<!--The preceding code:-->
代码改动部分：

<!--* Removes the code we're not using.
* Specifies an in-memory database is injected into the service container.-->
* 移除了不需要的 using 语句。
* 指定内存数据库注入到服务器容器。

* Startup.cs : [Startup 类](../fundamentals/startup.md) - 配置请求管道的类，处理所有所有由应用程序产生的管道请求。
* Program.cs : [Program 类](../fundamentals/index.md) 应用程序的主入口点。
* firstapp.csproj : [Project 文件](https://docs.microsoft.com/en-us/dotnet/articles/core/preview3/tools/csproj) ASP.NET Core 应用程序的 MSBuild 格式项目文件。包含项目间的引用， NuGet引用以及其他项目关联项。
* appsettings.json / appsettings.Development.json : 基于环境的应用配置文件。 [参考配置](xref:fundamentals/configuration)。
* bower.json : 项目 Bower 依赖包。
* .bowerrc : 当 Bower 下载文件时用来定义组件安装位置的配置文件。
* bundleconfig.json : 设置前端 JavaScript 和 CSS 文件压缩绑定的配置文件。
* Views : 包含 Razor 视图，视图是显示应用程序用户界面的组件，用户界面会呈现模型数据。
* Controllers : 包含 MVC 控制器， 默认控制器为 *HomeController.cs*。控制器是处理请求的类。
* wwwroot : Web 应用程序根目录。

更多信息请参考 [The MVC 模式](xref:mvc/overview).

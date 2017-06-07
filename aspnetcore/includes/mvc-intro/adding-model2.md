## 添加初始化迁移并更新数据库

* 打开命令提示符并导航到项目目录. (项目目录包含 *Startup.cs* 文件)。

* 在命令提示符中运行以下脚本：

  ```console
  dotnet restore
  dotnet ef migrations add Initial
  dotnet ef database update
  ```
  
  [.NET Core](http://go.microsoft.com/fwlink/?LinkID=517853) 是跨平台的.NET实现。 包含以下命令行：

  * `dotnet restore`: 下载 *.csproj* 文件中指定的 NuGet 包。
  * `dotnet ef migrations add Initial` 运行 Entity Framework .NET Core CLI 迁移命令并创建初始化迁移。参数 "Initial" 可以是任意值，但是通常用这个作为第一个"初始的" 数据库迁移。这个操作创建了一个 *Data/Migrations/<date-time>_Initial.cs* 文件，这个文件包含了添加（或删除）*Movie* 表到数据库的迁移命令。
  * `dotnet ef database update`  用我们刚刚创建的迁移来更新数据库。

你将会在下一节教程中歇息到数据库和连接字符创。 你会在教程 [添加字段](xref:tutorials/first-mvc-app/new-field) 学习到如何变更数据模型。
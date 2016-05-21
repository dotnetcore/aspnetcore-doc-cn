Getting Started
===============

1. Install `.NET Core`_

1. 安装 `.NET Core`_

2. Create a new .NET Core project:

2. 创建一个新的 .NET Core 项目：

  .. code-block:: console
    
    mkdir aspnetcoreapp
    cd aspnetcoreapp
    dotnet new

3. Update the *project.json* file to add the Kestrel HTTP server package as a dependency:

3. 编辑 *project.json* 文件添加 Kestrel HTTP server 包引用:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/project.json
    :language: c#
    :emphasize-lines: 11

4. Restore the packages:

4. 还原包:

  .. code-block:: console
    
    dotnet restore

5. Add a *Startup.cs* file that defines the request handling logic:

5. 添加一个 *Startup.cs* 文件并定义请求处理逻辑:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/Startup.cs
    :language: c#

6. Update the code in *Program.cs* to setup and start the Web host:

6. 编辑 *Program.cs* 中的代码来设置和启动 Web 宿主:

  .. literalinclude:: getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#
    :emphasize-lines: 2,10-15

7. Run the app  (the ``dotnet run`` command will build the app when it's out of date):

7. 运行应用程序  (``dotnet run`` 命令会在应用程序过期时构建它):

  .. code-block:: console
  
    dotnet run

8. Browse to \http://localhost:5000:

8. 浏览 \http://localhost:5000:

  .. image:: getting-started/_static/running-output.png

Next steps
----------

下一步
----------

- :doc:`/tutorials/first-mvc-app/index`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`

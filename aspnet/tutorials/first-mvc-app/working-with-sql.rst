Working with SQL Server LocalDB    用本地SQL Server数据库
===============================================

翻译： `魏美娟(初见) <http://github.com/ChujianA>`_

校对： `

 The ``ApplicationDbContext`` class handles the task of connecting to the database and mapping ``Movie`` objects to database records. The database context is registered with the :doc:`Dependency Injection  </fundamentals/dependency-injection>` container in the ``ConfigureServices`` method in the *Startup.cs* file:
  ApplicationDbContext类负责链接数据库并将Movie对象和数据记录进行映射。Startup.cs文件中，数据库上下文是在ConfigureServices方法中用Dependency Injection容器进行注入的。

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 39-45
  :dedent: 8
  :emphasize-lines: 5,6

The ASP.NET Core :doc:`Configuration </fundamentals/configuration>` system reads the ``ConnectionString``. For local development, it gets the connection string from the *appsettings.json* file:
ASP.NET Core Configuration系统读取ConnectionString，为了本地开发，从appsettings.json文件中获取连接字符串。
.. literalinclude:: start-mvc/sample/src/MvcMovie/appsettings.json
  :language: javascript
  :lines: 1-6
  :emphasize-lines: 3

When you deploy the app to a test or production server, you can use an environment variable or another approach to set the connection string to a real SQL Server. See :doc:`Configuration </fundamentals/configuration>` .
 当应用程序部署到测试服务器或者生产服务器时，用环境变量或者另一种方法来设置实际数据库的连接字符串。看Configuration。

SQL Server Express LocalDB
SQL Server Express 本地数据库
--------------------------------

LocalDB is a lightweight version of the SQL Server Express Database Engine that is targeted for program development. LocalDB starts on demand and runs in user mode, so there is no complex configuration. By default, LocalDB database creates "\*.mdf" files in the *C:/Users/<user>* directory.
LocalDB是一个轻量级版本的SQL Server Express数据库引擎，这是针对程序开发。LocalDB按需启动和运行在用户模式下，并没有复杂的配置。默认情况下，LocalDB数据库创建\*.mdf文件在C:/Users/<user> 目录中。

- From the **View** menu, open **SQL Server Object Explorer** (SSOX).
从View目录中，打开SQL Server Object Explorer (SSOX).

.. image:: working-with-sql/_static/ssox.png

- Right click on the ``Movie`` table **> View Designer**
右击Movie表>View Designer

.. image:: working-with-sql/_static/design.png

.. image:: working-with-sql/_static/dv.png

Note the key icon next to ``ID``. By default, EF will make a property named ``ID`` the primary key.
注意关键图标下面的ID。默认情况下，EF将一个属性命名为ID主键
.. comment: add this when we have it for MVC 6: For more information on EF and MVC, see Tom Dykstra's excellent tutorial on MVC and EF.

- Right click on the ``Movie`` table **> View Data**
右击Movie表>View Data

.. image:: working-with-sql/_static/ssox2.png

.. image:: working-with-sql/_static/vd22.png

Seed the database
迁移数据库
--------------------------

Create a new class named ``SeedData`` in the *Models* folder. Replace the generated code with the following:
在Models文件夹中创建一个名叫SeedData的新类。用以下代码替换生成的代码。
.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :lines: 3-62

Notice if there are any movies in the DB, the seed initializer returns.
注意如果数据库上下文中存在movies，迁移初始化就返回。
.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :lines: 18-21
  :dedent: 12
  :emphasize-lines: 3

Add the seed initializer to the end of the ``Configure`` method in the *Startup.cs* file: 
Startup.cs文件中，在Configure方法的最后添加迁移初始化。
.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-88
  :dedent: 8
  :emphasize-lines: 8

Test the app
测试应用程序
- Delete all the records in the DB. You can do this with the delete links in the browser or from SSOX.
在数据库中删除所有的记录。可以在浏览器中或者删除链接或者从SSOX中删除。
- Force the app to initialize (call the methods in the ``Startup`` class) so the seed method runs. To force initialization, IIS Express must be stopped and restarted. You can do this with any of the following approaches:
强制应用程序初始化（在Startup类中调用方法），让迁移方法运行。为了初始化，IIS Express必须停止，然后重新启动。可以用下列的任何一个方法来实现：
.. comment this no longer works  - ^<Shift>F5 (Hold down the control and Shift keys and tap F5)
  - Right click the IIS Express system tray icon in the notification area and tap **Exit** or **Stop* Site*
  - .. image:: working-with-sql/_static/iisExIcon.png
  - .. image:: working-with-sql/_static/stopIIS.png
  - If you were running VS in non-debug mode, press F5 to run in debug mode
  - If you were running VS in debug mode, stop the debugger and press ^F5

.. Note:: If the database doesn't initialize, put a break point on the line ``if (context.Movie.Any())`` and start debugging.
如果是数据库没有初始化，在if (context.Movie.Any())这行设置断点，并开始调试

.. image:: working-with-sql/_static/dbg.png

The app shows the seeded data.
用用程序显示了被迁移的数据

.. image:: working-with-sql/_static/m55.png
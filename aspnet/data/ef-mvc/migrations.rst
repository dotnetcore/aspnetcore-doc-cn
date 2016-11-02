Migrations
==========

数据迁移
==========

The Contoso University sample web application demonstrates how to create ASP.NET Core 1.0 MVC web applications using Entity Framework Core 1.0 and Visual Studio 2015. For information about the tutorial series, see :doc:`the first tutorial in the series </data/ef-mvc/intro>`.

Contoso 大学演示网站展示了如何使用Visual Studio 2015 以及 Entity Framework Core 1.0 开发一个 ASP.NET Core 1.0 MVC Web 应用程序。 更多关于本系列教程的信息请参考 :doc:`系列教程序章 </data/ef-mvc/intro>`。

In this tutorial, you start using the EF Core migrations feature for managing data model changes. In later tutorials, you'll add more migrations as you change the data model.

在本教程中，你将开始使用 EF Core 数据迁移功能来管理数据模型的变动。在后续的教程中，会根据数据模型的变动添加更多的迁移。

.. contents:: Sections:
  :local:
  :depth: 1

Introduction to migrations
--------------------------

数据迁移介绍
--------------------------

When you develop a new application, your data model changes frequently, and each time the model changes, it gets out of sync with the database. You started these tutorials by configuring the Entity Framework to create the database if it doesn't exist. Then each time you change the data model -- add, remove, or change entity classes or change your DbContext class -- you can delete the database and EF creates a new one that matches the model, and seeds it with test data.

当你在开发一个新的应用时数据模型是经常变化的，每次数据模型的变动都会与数据库的实际结构产生差异。下面的教程里面，从EF创建新数据库（如果不存在）开始本节的教程。然后，每次增加、删除或者改变实体类以及DbContext类时，你可以先删除数据库，然后EF都会创建按照新要求创建一个新数据库，并且用测试数据进行初始化。
 
This method of keeping the database in sync with the data model works well until you deploy the application to production. When the application is running in production it is usually storing data that you want to keep, and you don't want to lose everything each time you make a change such as adding a new column. The EF Core Migrations feature solves this problem by enabling EF to update the database schema instead of creating  a new database.

直到应用部署到生产环境前，该方法都会很好地保持数据库与数据模型建的同步。当应用运行到生产环境中时，通常都会存储大量数据，而你通常期望保留这些数据。例如：当增加一个新列时，你不想失去任何数据。EF Core 迁移功能通过升级数据库 Schema 的方法解决该问题，而不是创建一个新的数据库。

Change the connection string
----------------------------

修改连接字符串
----------------------------

In the *appsettings.json* file, change the name of the database in the connection string to ContosoUniversity2 or some other name that you haven't used on the computer you're using.

在 *appsettings.json* 文件中, 把连接字符串中的数据库名改为 ContosoUniversity2 或者其他你本机没有使用过的数据库名。

.. literalinclude::  intro/samples/cu/appsettings2.json
  :language: json
  :end-before:  Logging
  :emphasize-lines: 6

This change sets up the project so that the first migration will create a new database. This isn't required for getting started with migrations, but you'll see later why it's a good idea.

这个修改会促使项目初始化时创建一个新数据库。我们在学习迁移时这样做其实并不需要，但是接下来你会看到这样做的好处的。

.. note:: As an alternative to changing the database name, you can delete the database. Use **SQL Server Object Explorer** (SSOX) or the ``database drop`` CLI command:

.. note:: 除了数据库改名以外， 你也可以直接删除数据库。 使用 **SQL Server Object Explorer** (SSOX) 或者 ``database drop`` CLI 命令：

  .. code-block:: none

    dotnet ef database drop -c SchoolContext

  The following section explains how to run CLI commands.

  后面的章节会展示如何使用 CLI 命令。

Create an initial migration
---------------------------

创建初始化迁移
---------------------------

Save your changes and build the project. Then open a command window and navigate to the project folder. Here's a quick way to do that:

保存并编译项目。 打开命令行窗口并跳转到项目目录。下面有个便捷的方法：

* 在 **Solution Explorer** 中， 右击项目右键菜单中选择 **Open in File Explorer** 。

  .. image:: migrations/_static/open-in-file-explorer.png
     :alt: Open File Explorer menu item

  .. image:: migrations/_static/open-in-file-explorer.png
     :alt: 打开 File Explorer 菜单

* Hold down the Shift key and right-click the project folder in File Explorer, then choose Open command window here from the context menu.

* 按住 Shift 右击文件浏览器中的项目目录, 在右键菜单中选择 Open command window here。

  .. image:: migrations/_static/open-command-window.png
     :alt: Open command window

  .. image:: migrations/_static/open-command-window.png
     :alt: 打开命令菜单

Before you enter a command, stop IIS Express for the site, or you may get an error message: "*cannot access the file ... ContosoUniversity.dll because it is being used by another process.*" To stop the site, find the IIS Express icon in the Windows System Tray, and right-click it, then click **ContosoUniversity > Stop Site**. 

键入命令前必须停止网站运行的IIS，否则将会得到错误信息： "*cannot access the file ... ContosoUniversity.dll because it is being used by another process.*" 为了停止网站， 在window系统任务栏找到 IIS Express 图标，右击点击 **ContosoUniversity > Stop Site**. 

After you have stopped IIS Express, enter the following command in the command window:

在 IIS Express 中停止站点以后， 在命令行窗口中输入下述命令：

.. code-block:: text

  dotnet ef migrations add InitialCreate -c SchoolContext

You see output like the following in the command window:

命令行窗口将会产生以下输出：

.. code-block:: text

  C:\ContosoUniversity\src\ContosoUniversity>dotnet ef migrations add InitialCreate -c SchoolContext
  Project ContosoUniversity (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.

  Done. 

  To undo this action, use 'dotnet ef migrations remove'

You have to include the ``-c SchoolContext`` parameter to specify the database context class, because the project has two context classes (the other one is for ASP.NET Identity).

你不得不包含 ``-c SchoolContext`` 参数来指定数据库 context 类， 因为项目包含两个 context 类(另一个是 ASP.NET Identity 使用的)。

Examine the Up and Down methods
-------------------------------

Examine the Up and Down methods（测试上下方法）
-------------------------------

When you executed the ``migrations add`` command, EF generated the code that will create the database from scratch. This code is in the *Migrations* folder, in the file named `<timestamp>_InitialCreate.cs`. The ``Up`` method of the ``InitialCreate`` class creates the database tables that correspond to the data model entity sets, and the ``Down`` method deletes them, as shown in the following example. 

当执行 ``migrations add`` 命令时，EF将从基架生成创建数据库的代码。这些代码位于 *Migrations* 文件夹内的 `<timestamp>_InitialCreate.cs`文件内。 ``InitialCreate`` 类的 ``Up`` 方法负责创建与数据模型实体集相关的数据库表， ``Down`` 方法负责删除这些表，具体代码如下：

.. literalinclude::  intro/samples/cu/Migrations/20160726224716_InitialCreate.cs
  :start-after: snippet_Truncate
  :end-before:  #endregion
  :language: c#
  :dedent: 4

Migrations calls the ``Up`` method to implement the data model changes for a migration. When you enter a command to roll back the update, Migrations calls the ``Down`` method.

迁移调用 ``Up`` 方法实现数据模型的变化。输入命令回滚更新时，则调用``Down``方法。

This code is for the initial migration that was created when you entered the ``migrations add InitialCreate`` command. The migration name parameter ("InitialCreate" in the example) is used for the file name and can be whatever you want. It's best to choose a word or phrase that summarizes what is being done in the migration. For example, you might name a later migration "AddDepartmentTable".

键入 ``migrations add InitialCreate`` 命令，实现初始化迁移。迁移的名称参数（本例中为 "InitialCreate" ）用于文件名，该文件可以改为任何名称。最好是选择一个表明具体用途的词语说明迁移的目的。例如，可以将后面一个迁移命名为 "AddDepartmentTable"。

If you created the initial migration when the database already exists, the database creation code is generated but it doesn't have to run because the database already matches the data model. When you deploy the app to another environment where the database doesn't exist yet, this code will run to create your database, so it's a good idea to test it first. That's why you changed the name of the database in the connection string earlier -- so that migrations can create a new one from scratch.

如果创建初始化迁移的时候数据库已经存在了，创建数据库的代码也会生成，但是并没有实际运行，因为数据库已与数据模型相匹配。当向其它环境实际部署应用时，该部分代码将运行创建数据库，所以首先做一个测试是一个好的方法。这就是早前变更连接字符串中数据库名称的原因，这样就可以迁移工作就可以从基架创建一个新的数据库。

Examine the data model snapshot
-------------------------------

Migrations also creates a "snapshot" of the current database schema in *Migrations/SchoolContextModelSnapshot.cs*. Here's what that code looks like:

迁移操作的同时也会创建一个现有数据库的 "快照" ，将数据库的结构脚本存储到 *Migrations/SchoolContextModelSnapshot.cs* 文件中。 代码如下所示：

.. literalinclude::  intro/samples/cu/Migrations/SchoolContextModelSnapshot1.cs
  :start-after: snippet_Truncate
  :end-before:  #endregion
  :language: c#
  :dedent: 4

Because this code has to reflect the database state after the latest migration,  you can't remove a migration just by deleting the file named  `<timestamp>_<migrationname>.cs`. If you delete that file, the remaining migrations will be out of sync with the database snapshot file. To delete the last migration that you added, use the `dotnet ef migrations remove <https://ef.readthedocs.io/en/latest/miscellaneous/cli/dotnet.html#dotnet-ef-migrations-remove>`__ command.

因为这些代码反映了最后一次迁移之后的数据库状态，所以不能仅通过删除  `<timestamp>_<migrationname>.cs` 文件来取消一个迁移操作。如果删除了该文件，剩余的迁移将于数据库快照文件失去同步。要删除最后一次迁移，请使用 `dotnet ef migrations remove <https://ef.readthedocs.io/en/latest/miscellaneous/cli/dotnet.html#dotnet-ef-migrations-remove>`__ 命令。

Apply the migration to the database
-----------------------------------

应用迁移到数据库
-----------------------------------

In the command window, enter the following command to create the database and tables in it.

在命令行窗口输入以下命令来创建数据库对应的表。

.. code-block:: c#

  dotnet ef database update -c SchoolContext

The output from the command is similar to the ``migrations add`` command.

命令行窗体的输出与 ``migrations add`` 命令的输出相似。

.. code-block:: text

  C:\ContosoUniversity\src\ContosoUniversity>dotnet ef database update -c SchoolContext
  Project ContosoUniversity (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.

  Done.

Use **SQL Server Object Explorer** to inspect the database as you did in the first tutorial.  You'll notice the addition of an __EFMigrationsHistory table that keeps track of which migrations have been applied to the database. View the data in that table and you'll see one entry for the first migration.

和第一个教程一样，使用 **SQL Server Object Explorer** 来查看数据库。你会注意到多了一个 __EFMigrationsHistory 表，该表保存了数据库迁移的踪迹。查看看该表的数据，你将会看到第一次迁移的入口。


.. image:: migrations/_static/migrations-table.png
   :alt: Migrations history in SSOX

Run the application to verify that everything still works the same as before.

运行应用以测试程序是否正常工作。

.. image:: migrations/_static/students-index.png
   :alt: Students Index page
 
Command line interface (CLI) vs. Package Manager Console (PMC)
--------------------------------------------------------------

命令行界面（CLI）与程序包控制台（PMC）的对比
--------------------------------------------------------------
 
The EF tooling for managing migrations is available from .NET Core CLI commands or from PowerShell cmdlets in the Visual Studio **Package Manager Console** (PMC) window. In this preview version of the tooling, the CLI commands are more stable than the PMC cmdlets, so this tutorial shows how to use the .NET Core CLI commands. 

在 .NET Core CLI 命令行和 Visual Studio  **Package Manager Console** PMC中的 PowerShell cmdlets 中都可以使用管理迁移的EF工具。在早期版本的工具中，使用 CLI 命令较后者更稳定，所以该教程展示如何使用前者。

For more information about the CLI commands, see `.NET Core CLI <https://ef.readthedocs.io/en/latest/miscellaneous/cli/dotnet.html>`__. For information about the PMC commands, see `Package Manager Console (Visual Studio) <https://ef.readthedocs.io/en/latest/miscellaneous/cli/powershell.html>`__.

更多关于 CLI 命令的信息，请参看`.NET Core CLI <https://ef.readthedocs.io/en/latest/miscellaneous/cli/dotnet.html>`__。更多关于PMC命令，请参看`Package Manager Console (Visual Studio) <https://ef.readthedocs.io/en/latest/miscellaneous/cli/powershell.html>`__。
		
Summary
-------

总结
-------

In this tutorial, you've seen how to create and apply your first migration. In the next tutorial, you'll begin looking at more advanced topics by expanding the data model. Along the way you'll create and apply additional migrations.

在本教程中，你已经看到如何创建并应用首次迁移。在后续教程中，你将开始看到更多关于数据模型更高级的主题。你将创建并应用其他的迁移。

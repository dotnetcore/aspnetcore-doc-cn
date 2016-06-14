.. _migration-identity:

迁移身份验证与识别 
=====================================

作者： `Steve Smith`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对：

In the previous article we :doc:`migrated configuration from an ASP.NET MVC project to ASP.NET Core <configuration>`. In this article, we migrate the registration, login, and user management features.

在上文中，我们已完成 :doc:`将 ASP.NET MVC 项目的配置迁移到 ASP.NET Core <configuration>` 中，本文我们将介绍如何迁移注册、登录和用户管理功能。

.. contents:: Sections:
  :local:
  :depth: 1

配置 Identity 和 Membership
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

In ASP.NET MVC, authentication and identity features are configured using ASP.NET Identity in Startup.Auth.cs and IdentityConfig.cs, located in the App_Start folder. In ASP.NET Core, these features are configured in *Startup.cs*. Before pulling in the required services and configuring them, we should add the required dependencies to the project. Open *project.json* and add ``Microsoft.AspNet.Identity.EntityFramework`` and ``Microsoft.AspNet.Identity.Cookies`` to the list of dependencies:

在 ASP.NET MVC 中身份验证和识别功能由 ASP.NET Identity 来配置（在 Startup.Auth.cs 和 IdentityConfig.cs 文件中，这两文件则位于 App_Start 目录下）。在 ASP.NET Core 中这些功能在 *Startup.cs* 中配置。在获取服务并为其配置之前，我们首先需要将所需要的依赖项添加到项目之中。打开 *project.json* 文件，在 ``dependencies`` 节点中添加 ``Microsoft.AspNet.Identity.EntityFramework`` 和 ``Microsoft.AspNet.Identity.Cookies`` ：

.. code-block:: json

  "dependencies": {
    "Microsoft.AspNet.Server.IIS": "1.0.0-beta3",
    "Microsoft.AspNet.Mvc": "6.0.0-beta3",
    "Microsoft.Framework.ConfigurationModel.Json": "1.0.0-beta3",
    "Microsoft.AspNet.Identity.EntityFramework": "3.0.0-beta3",
    "Microsoft.AspNet.Security.Cookies": "1.0.0-beta3"
  },

Now, open Startup.cs and update the ConfigureServices() method to use Entity Framework and Identity services:

随后打开 Startup.cs 并改一下 ConfigureServices() 方法，让它使用 Entity Framework 和 Identity 服务：

.. code-block:: c#

  public void ConfigureServices(IServiceCollection services)
  {
    // Add EF services to the services container.
    services.AddEntityFramework(Configuration)
      .AddSqlServer()
      .AddDbContext<ApplicationDbContext>();

    // Add Identity services to the services container.
    services.AddIdentity<ApplicationUser, IdentityRole>(Configuration)
      .AddEntityFrameworkStores<ApplicationDbContext>();

    services.AddMvc();
  }

At this point, there are two types referenced in the above code that we haven't yet migrated from the ASP.NET MVC project: ``ApplicationDbContext`` and ``ApplicationUser``. Create a new *Models* folder in the ASP.NET Core project, and add two classes to it corresponding to these types. You will find the ASP.NET MVC versions of these classes in ``/Models/IdentityModels.cs``, but we will use one file per class in the migrated project since that's more clear.

此处，我们尚不能直接从 ASP.NET MVC 项目迁移上面代码段中的两个类型引用（``ApplicationDbContext`` 以及 ``ApplicationUser``）。在 ASP.NET Core 项目中新建 *Models* 文件夹，新建两个类（名称分别对应 ``ApplicationDbContext`` 和 ``ApplicationUser``）。你会发现这些类的 ASP.NET MVC 版本在 ``/Models/IdentityModels.cs`` 文件中，不过为了清晰我们在迁移项目中为每个类单独建了一个文件。

ApplicationUser.cs:

.. code-block:: c#

  using Microsoft.AspNet.Identity;

  namespace NewMvc6Project.Models
  {
    public class ApplicationUser : IdentityUser
    {
    }
  }

ApplicationDbContext.cs:

.. code-block:: c#

  using Microsoft.AspNet.Identity.EntityFramework;
  using Microsoft.Data.Entity;

  namespace NewMvc6Project.Models
  {
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
      private static bool _created = false;
      public ApplicationDbContext()
      {
        // Create the database and schema if it doesn't exist
        // This is a temporary workaround to create database until Entity Framework database migrations 
        // are supported in ASP.NET Core
        if (!_created)
        {
          Database.AsMigrationsEnabled().ApplyMigrations();
          _created = true;
        }
      }

      protected override void OnConfiguring(DbContextOptions options)
      {
        options.UseSqlServer();
      }
    }
  }

The ASP.NET MVC Starter Web project doesn't include much customization of users, or the ApplicationDbContext. When migrating a real application, you will also need to migrate all of the custom properties and methods of your application's user and DbContext classes, as well as any other Model classes your application utilizes (for example, if your DbContext has a DbSet<Album>, you will of course need to migrate the Album class).

ASP.NET MVC 初始的 Web 项目并不包含太多使用者定制信息或 ApplicationDbContext。当迁移一个真实应用程序时，你同时需要迁移所有用户自己定制的属性和方法、DbContext 类以及其它应用程序所使用的 Model 类（假如你的 DbContext 有 DbSet<Album>，那么你就得迁移 Album 类）。

With these files in place, the Startup.cs file can be made to compile by updating its using statements:

有了这些文件，Startup.cs 文件更新一下 using 语句后就能被编译了：

.. code-block:: c#

  using Microsoft.Framework.ConfigurationModel;
  using Microsoft.AspNet.Hosting;
  using NewMvc6Project.Models;
  using Microsoft.AspNet.Identity;

Our application is now ready to support authentication and identity services - it just needs to have these features exposed to users. 

此刻我们的应用程序已经可以支持身份验证和识别服务了——它只需要将这些功能暴露给用户即可。

迁移注册与登录逻辑
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

With identity services configured for the application and data access configured using Entity Framework and SQL Server, we are now ready to add support for registration and login to the application. Recall that :ref:`earlier in the migration process <migrate-layout-file>` we commented out a reference to _LoginPartial in _Layout.cshtml. Now it's time to return to that code, uncomment it, and add in the necessary controllers and views to support login functionality.

使用 Entity Framework 和 SQL Server 为应用程序和数据访问配置身份识别服务后，就可以着手准备支持注册和登录了。回顾一下在 :ref:`之前的迁移过程 <migrate-layout-file>` 中我们注释掉了布局页 _Layout.cshtml 中的 _LoginPartial。现在是时候把这个注释去掉了，并在必要的控制器和视图中添加一些代码来实现登录功能。

Update _Layout.cshtml; uncomment the @Html.Partial line:

更新 _Layout.cshtml 文件，把 @Html.Partial 这一行注释去掉：

.. code-block:: c#

        <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
      </ul>
      @*@Html.Partial("_LoginPartial")*@
    </div>
  </div>

Now, add a new MVC View Page called _LoginPartial to the Views/Shared folder:

然后在 Views/Shared 目录下新建一个 MVC 视图页，取名 _LoginPartial：

.. image migratingauthmembership/_static/AddLoginPartial.png

Update _LoginPartial.cshtml with the following code (replace all of its contents):

把以下代码写进 _LoginPartial.cshtml 文件里（替换文件里的所有内容）：

.. code-block:: c#

  @using System.Security.Principal

  @if (User.Identity.IsAuthenticated)
  {
      using (Html.BeginForm("LogOff", "Account", FormMethod.Post, new { id = "logoutForm", @class = "navbar-right" }))
      {
          @Html.AntiForgeryToken()
          <ul class="nav navbar-nav navbar-right">
              <li>
                  @Html.ActionLink("Hello " + User.Identity.GetUserName() + "!", "Manage", "Account", routeValues: null, htmlAttributes: new { title = "Manage" })
              </li>
              <li><a href="javascript:document.getElementById('logoutForm').submit()">Log off</a></li>
          </ul>
      }
  }
  else
  {
      <ul class="nav navbar-nav navbar-right">
          <li>@Html.ActionLink("Register", "Register", "Account", routeValues: null, htmlAttributes: new { id = "registerLink" })</li>
          <li>@Html.ActionLink("Log in", "Login", "Account", routeValues: null, htmlAttributes: new { id = "loginLink" })</li>
      </ul>
  }

At this point, you should be able to refresh the site in your browser.

至此，身份验证与识别的迁移工作完成。你到浏览器中刷新站点便可看到。

总结
^^^^^^^

ASP.NET Core introduces changes to the ASP.NET Identity features. In this article, you have seen how to migrate the authentication and user management features of an ASP.NET Identity to ASP.NET Core.

ASP.NET Core 为 ASP.NET Identity 引入了一些变化。本文已向你展示了如何向 ASP.NET Core 迁移 ASP.NET Identity 的身份验证和用户管理功能。
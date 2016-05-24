Your First ASP.NET Core Application on a Mac Using Visual Studio Code
=====================================================================
用Visual Studio Code在Mac平台创建第一个ASP.NET Core Application
=====================================================================

By `Daniel Roth`_, `Steve Smith`_ and `Rick Anderson`_

This article will show you how to write your first ASP.NET Core application on a Mac.

本文将指导你如在和Mac平台下创建ASP.NET Core应用.

.. contents:: Sections:
  :local:
  :depth: 1

Setting Up Your Development Environment
---------------------------------------
配置开发环境
---------------------------------------

To setup your development machine download and install `.NET Core`_ and `Visual Studio Code`_ with the `C# extension <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__.

在开发机器上下载并且安装 `.NET Core`_ and _ 带有 `C# 扩展的 <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__ `Visual Studio Code`.

Scaffolding Applications Using Yeoman
-------------------------------------

用Yeoman创建应用程序
-------------------------------------

Follow the instruction in :doc:`/client-side/yeoman` to create an ASP.NET Core project.

按照:doc:`/client-side/yeoman`这边文章的指引来创建一个ASP.NET Core 项目.

Developing ASP.NET Applications on a Mac With Visual Studio Code
----------------------------------------------------------------
在Mac平台上用Visual Studio Code开发ASP.NET 应用
----------------------------------------------------------------

- Start **Visual Studio Code**

- 运行 **Visual Studio Code**

.. image:: your-first-mac-aspnet/_static/vscode-welcome.png

- Tap **File > Open** and navigate to your Empty ASP.NET Core app

- 打开 **File > Open** 然后选中之前创建的空ASP.NET Core应用

.. image:: your-first-mac-aspnet/_static/file-open.png

From a Terminal / bash prompt, run ``dotnet restore`` to restore the project's dependencies. Alternately, you can enter ``command shift p`` in Visual Studio Code and then type ``dot`` as shown:

通过 a Terminal 或者 bash prompt, 运行 ``dotnet restore`` 来还原项目的依赖. 或者在 Visual Studio Code 按住 ``command shift p`` 然后输入 ``dot`` ,入下图所示:

.. image:: your-first-mac-aspnet/_static/dotnet-restore.png

You can run commands directly from within Visual Studio Code, including ``dotnet restore`` and any tools referenced in the *project.json* file, as well as custom tasks defined in *.vscode/tasks.json*.

  ``dotnet restore`` 和在 *project.json* 文件中引用的所有工具, 以及在 *.vscode/tasks.json* 自定定义的任务都可以在Visual Studio Code直接运行.

This empty project template simply displays "Hello World!". Open *Startup.cs* in Visual Studio Code to see how this is configured:

这个由模板创建出来的空项目仅会显示一个"Hello World!". 在Visual Studio Code 打开 *Startup.cs* 看看它是如何配置的:

.. image:: your-first-mac-aspnet/_static/vscode-startupcs.png

If this is your first time using Visual Studio Code (or just *Code* for short), note that it provides a very streamlined, fast, clean interface for quickly working with files, while still providing tooling to make writing code extremely productive. 

Visual Studio Code提供了一个非常简单,快速和简洁的界面来快速的管理文件，并切还提供了一些工具可以用来非常高效的编写代码.如果你是第一次使用它,可以注意一下.

In the left navigation bar, there are four icons, representing four viewlets:

- Explore
- Search
- Git
- Debug


在左边的导航栏有四个图标,代表四种视图

- 资源管理器
- 搜索
- Git
- 调试

The Explore viewlet allows you to quickly navigate within the folder system, as well as easily see the files you are currently working with. It displays a badge to indicate whether any files have unsaved changes, and new folders and files can easily be created (without having to open a separate dialog window). You can easily Save All from a menu option that appears on mouse over, as well.

在资源管理器视图下你可以快速浏览文件系统,并且能很容易看到当前正在使用的文件.它会显示一个标记来表明是否有有尚未保存的更改同时可以在不打开其它对话框的情况下轻松的创建文件或者文件夹.

The Search viewlet allows you to quickly search within the folder structure, searching filenames as well as contents.

在搜索视图中可以在文档结构中对文件名和内容快速查找.

*Code* will integrate with Git if it is installed on your system. You can easily initialize a new repository, make commits, and push changes from the Git viewlet.

如果你安装了Git你的代码将会和它关联. 在Git视图中你很容易初始化一个新repository,进行提交和推送变更.

.. image:: your-first-mac-aspnet/_static/vscode-git.png

The Debug viewlet supports interactive debugging of applications.

调试视图支持应用程序的交互式调试.

Finally, Code's editor has a ton of great features. You'll notice unused using statements are underlined and can be removed automatically by using ``command .`` when the lightbulb icon appears. Classes and methods also display how many references there are in the project to them. If you're coming from Visual Studio, Code includes many of the same keyboard shortcuts, such as ``command k c`` to comment a block of code, and ``command k u`` to uncomment.

Visual Studio Code还有很多功能更. 你会发现未使用using语句会带有下划线，单出现现灯泡图标,可以通过 ``command .`` 自动删除.
类和方法还可以显示在项目中被引用了多少次. 如果你使用过 Visual Studio,你会发现有很多相似的快捷键,例如 ``command k c`` 用来注释代码, and ``command k u`` 用来去除注释.

Running Locally Using Kestrel
-----------------------------
通过 Kestrel 在本地运行
-----------------------------

The sample is configured to use :ref:`Kestrel <kestrel>` for the web server. You can see it configured in the *project.json* file, where it is specified as a dependency.

示例使用 :ref:`Kestrel <kestrel>` 作为web服务器.你可以在*project.json* 找到这个配置项，它被指定为依赖配置.

.. code-block:: json
  :emphasize-lines: 11-12
 
  {
    "version": "1.0.0-*",
    "compilationOptions": {
      "emitEntryPoint": true
    },
    "dependencies": {
      "Microsoft.NETCore.App": {
        "type": "platform",
        "version": "1.0.0-rc2-3002702"
      },
      "Microsoft.AspNetCore.Server.Kestrel": "1.0.0-rc2-final",
      "Microsoft.AspNetCore.Server.Kestrel.Https": "1.0.0-rc2-final",
      "Microsoft.Extensions.Logging.Console": "1.0.0-rc2-final"
    },
    "frameworks": {
      "netcoreapp1.0": {}
    }
  }


- Run ``dotnet run`` command to launch the app

- 运行 ``dotnet run`` 命令启动应用

- Navigate to ``localhost:5000``:

- 浏览器中访问 ``localhost:5000``:

.. image:: your-first-mac-aspnet/_static/hello-world.png

- To stop the web server enter ``Ctrl+C``.

- 通过 ``Ctrl+C`` 停止web服务器.


Publishing to Azure
-------------------

部署到 to Azure
-------------------

Once you've developed your application, you can easily use the Git integration built into Visual Studio Code to push updates to production, hosted on `Microsoft Azure <http://azure.microsoft.com>`_. 

一旦你已经部署了你的引用,你可以轻松的使用集成在Visual Studio Code中的Git将你的更新推送到基于  `Microsoft Azure <http://azure.microsoft.com>`_ 生产环境. 

Initialize Git
^^^^^^^^^^^^^^
初始化 Git
^^^^^^^^^^^^^^

Initialize Git in the folder you're working in. Tap on the Git viewlet and click the ``Initialize Git repository`` button.

在你使用的文件夹中初始化Git. 切换到Git视图 然后点击 ``Initialize Git repository`` 按钮.

.. image:: your-first-mac-aspnet/_static/vscode-git-commit.png

Add a commit message and tap enter or tap the checkmark icon to commit the staged files. 

添加提交信息然后切换到输入或者复选框来提阶段性文件. 

.. image:: your-first-mac-aspnet/_static/init-commit.png

Git is tracking changes, so if you make an update to a file, the Git viewlet will display the files that have changed since your last commit.

Git会一直跟着变更,因此你可以对文件进行更新,Git视图会显示自上次提交之后所更改的文件.

Initialize Azure Website
^^^^^^^^^^^^^^^^^^^^^^^^
初始化 Azure 网站
^^^^^^^^^^^^^^^^^^^^^^^^

You can deploy to Azure Web Apps directly using Git. 

你可以通过git将网站直接部署到Azure. 

- `Create a new Web App <https://tryappservice.azure.com/>`__ in Azure. If you don't have an Azure account, you can `create a free trial <http://azure.microsoft.com/en-us/pricing/free-trial/>`__. 

- `在Azure创一个一个Web应用 <https://tryappservice.azure.com/>`__  . 如果你没有Azure账号, `你可以免费创建一个使用账号 <http://azure.microsoft.com/en-us/pricing/free-trial/>`__. 

- Configure the Web App in Azure to support `continuous deployment using Git <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

- 在Azure将Web应用设置为 `通过Git持续部署 <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

Record the Git URL for the Web App from the Azure portal:

将此Web应用在Azure中的Git URL记录下来:

.. image:: your-first-mac-aspnet/_static/azure-portal.png

- In a Terminal window, add a remote named ``azure`` with the Git URL you noted previously.

- 打开一个终,用先前记录下的Git URL地址添一个名称为``azure`` 的远程命名.

  - ``git remote add azure https://ardalis-git@firstaspnetcoremac.scm.azurewebsites.net:443/firstaspnetcoremac.git``

- Push to master.

- 推送到master分支.

  - ``git push azure master`` to deploy. 

  .. image:: your-first-mac-aspnet/_static/git-push-azure-master.png

- Browse to the newly deployed web app. You should see ``Hello world!``

- 浏览刚刚部署上的web应用.你应该看到浏览器输出 ``Hello world!``

.. .. image:: your-first-mac-aspnet/_static/azure.png 


Additional Resources
--------------------

- `Visual Studio Code`_
- :doc:`/client-side/yeoman`
- :doc:`/fundamentals/index`

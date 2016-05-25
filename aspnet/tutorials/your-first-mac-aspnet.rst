Your First ASP.NET Core Application on a Mac Using Visual Studio Code
=====================================================================
用Visual Studio Code在Mac平台创建第一个ASP.NET Core Application
=====================================================================

By `Daniel Roth`_, `Steve Smith`_ and `Rick Anderson`_

作者 `Daniel Roth`_, `Steve Smith`_ and `Rick Anderson`_

翻译者 `赵志刚`

审稿人 `何镇汐`

This article will show you how to write your first ASP.NET Core application on a Mac.

本文指导你如何在 Mac 平台上创建你第一个 ASP.NET Core 应用程序。

.. contents:: Sections:
  :local:
  :depth: 1

Setting Up Your Development Environment
---------------------------------------
配置开发环境
---------------------------------------

To setup your development machine download and install `.NET Core`_ and `Visual Studio Code`_ with the `C# extension <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__.

在开发机器上下载并且安装 `.NET Core`_ 和带有 `C# 扩展的 <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__ `Visual Studio Code`。

Scaffolding Applications Using Yeoman
-------------------------------------

用Yeoman创建应用程序
-------------------------------------

Follow the instruction in :doc:`/client-side/yeoman` to create an ASP.NET Core project.

按照 :doc:/client-side/yeoman 一文的引导创建第一个 ASP.NET Core 项目。

Developing ASP.NET Applications on a Mac With Visual Studio Code
----------------------------------------------------------------
在Mac平台上用Visual Studio Code开发ASP.NET 应用程序
----------------------------------------------------------------

- Start **Visual Studio Code**

- 运行 **Visual Studio Code**

.. image:: your-first-mac-aspnet/_static/vscode-welcome.png

- Tap **File > Open** and navigate to your Empty ASP.NET Core app

- 打开 **File > Open**，然后选中之前创建的空ASP.NET Core应用程序

.. image:: your-first-mac-aspnet/_static/file-open.png

From a Terminal / bash prompt, run ``dotnet restore`` to restore the project's dependencies. Alternately, you can enter ``command shift p`` in Visual Studio Code and then type ``dot`` as shown:

通过终端或 Bash，利用 doenet restore 还原项目依赖。或者在 Visual Studio Code 中键入 command shift p，然后输入 dot，如下图所示：

.. image:: your-first-mac-aspnet/_static/dotnet-restore.png

You can run commands directly from within Visual Studio Code, including ``dotnet restore`` and any tools referenced in the *project.json* file, as well as custom tasks defined in *.vscode/tasks.json*.

你可在 Visual Studio Code 中直接运行指令，这些指令包括 dotnet restore 、所有在 project.json 文件中所引用的工具以及定义于 .vscode/tasks.json 中的自定义任务。

This empty project template simply displays "Hello World!". Open *Startup.cs* in Visual Studio Code to see how this is configured:

这个由模板创建出来的空项目仅会显示一个"Hello World!"。 在 Visual Studio Code 中打开 *Startup.cs* 看看它是如何配置的:

.. image:: your-first-mac-aspnet/_static/vscode-startupcs.png

If this is your first time using Visual Studio Code (or just *Code* for short), note that it provides a very streamlined, fast, clean interface for quickly working with files, while still providing tooling to make writing code extremely productive. 

若这是你第一次使用 Visual Studio Code（下文将使用简写 Code），那么你要记住它提供了一个非常简化、快速、清爽的界面来处理文件，这使得使用 VSCode 编写代码非常具有生产力。

In the left navigation bar, there are four icons, representing four viewlets:

- Explore
- Search
- Git
- Debug


在左边的导航栏有四个图标，代表四种视图

- 资源管理器
- 搜索
- Git
- 调试

The Explore viewlet allows you to quickly navigate within the folder system, as well as easily see the files you are currently working with. It displays a badge to indicate whether any files have unsaved changes, and new folders and files can easily be created (without having to open a separate dialog window). You can easily Save All from a menu option that appears on mouse over, as well.

在资源管理器视图下你可以快速浏览文件系统,并且能很容易看到当前正在使用的文件。它会显示一个标记来表明哪些文件尚未保存变更，新文件夹和文件也能轻松（通过资源管理器视图）创建（而不用单独打开一个窗体对话框）。当然，如果鼠标经过菜单选项曲，“全部保存”按钮就会出现，点击即可保存全部变更。

The Search viewlet allows you to quickly search within the folder structure, searching filenames as well as contents.

在搜索视图中可以在文档结构中对文件名和内容快速查找。

*Code* will integrate with Git if it is installed on your system. You can easily initialize a new repository, make commits, and push changes from the Git viewlet.

如果你已安装 GIT，Code 将与之关联。在Git视图中你很容易初始化一个新 repository，进行提交和推送变更.

.. image:: your-first-mac-aspnet/_static/vscode-git.png

The Debug viewlet supports interactive debugging of applications.

调试视图支持为应用程序进行交互调试。

Finally, Code's editor has a ton of great features. You'll notice unused using statements are underlined and can be removed automatically by using ``command .`` when the lightbulb icon appears. Classes and methods also display how many references there are in the project to them. If you're coming from Visual Studio, Code includes many of the same keyboard shortcuts, such as ``command k c`` to comment a block of code, and ``command k u`` to uncomment.

最后，VSCode 的编辑器还提供了一堆非常棒的特性，比如你会注意到未使用的 using 语句会带有下划线，当出现电灯图标时可使用 ``command .`` 自动移除之。类和方法同样可显示本项目中的引用次数。如果你曾使用过 Visual Studio，那么你会发现 
VSCode 中包含了许多一样的快捷键，比如注释代码用 ``command k c`` 用来注释代码, 去除注释用 ``command k u``

Running Locally Using Kestrel
-----------------------------
通过 Kestrel 在本地运行
-----------------------------

The sample is configured to use :ref:`Kestrel <kestrel>` for the web server. You can see it configured in the *project.json* file, where it is specified as a dependency.

下例 Web 服务器被配置为使用 :ref:`Kestrel <kestrel>`，可在*project.json* 文件中查看配置，它使用了依赖倒置。

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

一旦在 `Microsoft Azure <http://azure.microsoft.com>`_ 部署了你的应用程序，你便可轻松地通过 GIT 功能将推送产品的更新集成到 Visual Studio Code 中。

Initialize Git
^^^^^^^^^^^^^^
初始化 Git
^^^^^^^^^^^^^^

Initialize Git in the folder you're working in. Tap on the Git viewlet and click the ``Initialize Git repository`` button.

为你的工作文件夹初始化 GIT。切换到Git视图 然后点击 ``Initialize Git repository`` 按钮。

.. image:: your-first-mac-aspnet/_static/vscode-git-commit.png

Add a commit message and tap enter or tap the checkmark icon to commit the staged files. 

填写提交信息并点击提交，或点击复选框来提交暂存文件。

.. image:: your-first-mac-aspnet/_static/init-commit.png

Git is tracking changes, so if you make an update to a file, the Git viewlet will display the files that have changed since your last commit.

GIT 会跟踪变更，如果更新了文件，GIT 视图能比较并显示出最后一题提交本次是变更后的文件的差异。

Initialize Azure Website
^^^^^^^^^^^^^^^^^^^^^^^^
初始化 Azure 网站
^^^^^^^^^^^^^^^^^^^^^^^^

You can deploy to Azure Web Apps directly using Git. 

你可以通过git将应用程序直接部署到Azure。

- `Create a new Web App <https://tryappservice.azure.com/>`__ in Azure. If you don't have an Azure account, you can `create a free trial <http://azure.microsoft.com/en-us/pricing/free-trial/>`__. 

- `在 Azure 创建一个 Web 应用程序 <https://tryappservice.azure.com/>`__  。如果你没有Azure账号, `你可以免费创建一个试用账号 <http://azure.microsoft.com/en-us/pricing/free-trial/>`__。

- Configure the Web App in Azure to support `continuous deployment using Git <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

- 在Azure将Web应用程序设置为 `通过Git持续部署 <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

Record the Git URL for the Web App from the Azure portal:

将此Web应用程序在Azure中的Git URL记录下来:

.. image:: your-first-mac-aspnet/_static/azure-portal.png

- In a Terminal window, add a remote named ``azure`` with the Git URL you noted previously.

- 在终端窗口中，用之前记下的 Git URL 远程新建一个名为 azure 的仓库。

  - ``git remote add azure https://ardalis-git@firstaspnetcoremac.scm.azurewebsites.net:443/firstaspnetcoremac.git``

- Push to master.

- 推送到master分支。

  - ``git push azure master`` to deploy. 
  
  - 使用``git push azure master`` 部署。

  .. image:: your-first-mac-aspnet/_static/git-push-azure-master.png

- Browse to the newly deployed web app. You should see ``Hello world!``

- 浏览刚部署的 Web 应用程序，你应该看到浏览器输出 ``Hello world!``

.. .. image:: your-first-mac-aspnet/_static/azure.png 


Additional Resources
--------------------

扩展阅读
--------------------

- `Visual Studio Code`_
- :doc:`/client-side/yeoman`
- :doc:`/fundamentals/index`

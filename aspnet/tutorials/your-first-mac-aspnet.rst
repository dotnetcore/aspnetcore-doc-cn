用 Visual Studio Code 在 macOS 上创建首个 ASP.NET Core 应用程序
=====================================================================

Your First ASP.NET Core Application on a Mac Using Visual Studio Code
=====================================================================

作者 `Daniel Roth`_ 、`Steve Smith`_ 以及 `Rick Anderson`_、`Shayne Boyer`_

翻译 `赵志刚 <https://github.com/rdzzg>`_ 

校对 `何镇汐 <https://github.com/UtilCore>`_ 、 `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

This article will show you how to write your first ASP.NET Core application on a Mac.

本节将展示如何在 macOS 平台上创建首个 ASP.NET Core 应用程序。

.. contents:: Sections:
  :local:
  :depth: 1

Setting Up Your Development Environment
---------------------------------------

配置开发环境
---------------------------------------

To setup your development machine download and install `.NET Core`_ and `Visual Studio Code`_ with the `C# extension <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__. Node.js and npm is also required. If not already installed visit `nodejs.org <https://nodejs.org/en/download/package-manager/#osx>`_. 

在开发机中下载并安装 `.NET Core`_ 、和 `Visual Studio Code` 及 `C# 扩展 <https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp>`__ （在 VS Code 中通过命令 ``ext install csharp`` 安装，译者注）。

Scaffolding Applications Using Yeoman
-------------------------------------

用 Yeoman 创建应用程序
-------------------------------------

Follow the instruction in :doc:`/client-side/yeoman` to create an ASP.NET Core project.

按照 :doc:`/client-side/yeoman` 一文的引导创建第一个 ASP.NET Core 项目。

Developing ASP.NET Applications on a Mac With Visual Studio Code
----------------------------------------------------------------

在 macOS 上使用 Visual Studio Code 开发 ASP.NET 应用程序
----------------------------------------------------------------

- Start **Visual Studio Code**

- 运行 **Visual Studio Code**

.. image:: your-first-mac-aspnet/_static/vscode-welcome.png

- Tap **File > Open** and navigate to your Empty ASP.NET Core app

- 打开 **File > Open** ，导航到先前所创建的空 ASP.NET Core 应用程序

.. image:: your-first-mac-aspnet/_static/file-open.png

From a Terminal / bash prompt, run ``dotnet restore`` to restore the project's dependencies. Alternately, you can enter ``command shift p`` in Visual Studio Code and then type ``dot`` as shown:

通过终端 / Bash 提示符，执行 ``dotnet restore`` 还原项目依赖（在终端命令行中切换至项目所在目录，而后运行 ``dotnet restore`` ，译者注）。或者在 Visual Studio Code 中键入 ``command shift p`` （也可用 ``F1`` 代替，译者注），然后输入 ``dot``，如下图所示：

.. image:: your-first-mac-aspnet/_static/dotnet-restore.png

You can run commands directly from within Visual Studio Code, including ``dotnet restore`` and any tools referenced in the *project.json* file, as well as custom tasks defined in *.vscode/tasks.json*.

你可以在 Visual Studio Code 中直接运行指令，这些指令包括 ``dotnet restore`` 、*project.json* 文件中所引用的所有工具以及定义于 *.vscode/tasks.json* 中的自定义任务。

This empty project template simply displays "Hello World!". Open *Startup.cs* in Visual Studio Code to see how this is configured:

这个由模板创建出来的空项目仅会显示一个“Hello World!”。在 Visual Studio Code 中打开 *Startup.cs* 看看它是如何配置的:

.. image:: your-first-mac-aspnet/_static/vscode-startupcs.png

If this is your first time using Visual Studio Code (or just *Code* for short), note that it provides a very streamlined, fast, clean interface for quickly working with files, while still providing tooling to make writing code extremely productive. 

若这是你第一次使用 Visual Studio Code（下文将使用简写 *Code*），那么你要记住它提供了一个非常简化、快速、清爽的界面来处理文件，这使得使用 VSCode 编写代码非常具有生产力。

In the left navigation bar, there are four icons, representing four viewlets:

- Explore
- Search
- Git
- Debug

左侧导航栏中的四个图标分别代表四种功能

- 资源管理器
- 搜索
- Git
- 调试

The Explore viewlet allows you to quickly navigate within the folder system, as well as easily see the files you are currently working with. It displays a badge to indicate whether any files have unsaved changes, and new folders and files can easily be created (without having to open a separate dialog window). You can easily Save All from a menu option that appears on mouse over, as well.

在资源管理器视图下你可以快速浏览文件系统，并且能很容易看到当前正在使用的文件。它会使用一个符号来标识哪些文件尚未保存变更，同时创建新文件夹和文件也很容易（通过资源管理器视图）。当然如果鼠标经过菜单项，「全部保存」按钮就会出现，点击即可保存全部变更。

The Search viewlet allows you to quickly search within the folder structure, searching filenames as well as contents.

搜索视图允许你在目录结构中快速搜索文件名及内容。

*Code* will integrate with Git if it is installed on your system. You can easily initialize a new repository, make commits, and push changes from the Git viewlet.

如果你已安装 GIT，VSCode 将集成它。在 Git 视图中，你可以轻松初始化一个新的版本库，进行提交和推送变更。

.. image:: your-first-mac-aspnet/_static/vscode-git.png

The Debug viewlet supports interactive debugging of applications.

调试视图支持为应用程序进行交互调试。

Finally, Code's editor has a ton of great features. You'll notice unused using statements are underlined and can be removed automatically by using ``command .`` when the lightbulb icon appears. Classes and methods also display how many references there are in the project to them. If you're coming from Visual Studio, Code includes many of the same keyboard shortcuts, such as ``command k c`` to comment a block of code, and ``command k u`` to uncomment.

最后，VSCode 的编辑器还提供了一些非常棒的特性，比如你会注意到未使用的 using 语句会带有下划线，当出现电灯图标时可使用 ``command .`` 自动移除之。类和方法同样可显示本项目中的引用次数。如果你曾使用过 Visual Studio，那么你会发现 VSCode 中包含了许多一样的快捷键，比如用 ``command k c`` 注释代码，用 ``command k u`` 去除注释。

Running Locally Using Kestrel
-----------------------------

通过 Kestrel 在本地运行
-----------------------------

The sample is configured to use :ref:`Kestrel <kestrel>` for the web server. You can see it configured in the *project.json* file, where it is specified as a dependency.

本示例配置使用 :ref:`Kestrel <kestrel>` Web 服务器，可在 *project.json* 文件的 ``dependencies`` 节点中看到该项配置。

.. code-block:: json
  :emphasize-lines: 14
 
  {
    "dependencies": {
      "Microsoft.NETCore.App": {
        "version": "1.0.0",
        "type": "platform"
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

- 浏览器中访问 ``localhost:5000``：

.. image:: your-first-mac-aspnet/_static/hello-world.png

- To stop the web server enter ``Ctrl+C``.

- 通过 ``Ctrl+C`` 停止 web 服务器。


Publishing to Azure
-------------------

部署到 Azure
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

GIT 会跟踪变更，所以如果你更新了文件，Git视图将显示上次提交之后修改过的文件。

Initialize Azure Website
^^^^^^^^^^^^^^^^^^^^^^^^

初始化 Azure 网站
^^^^^^^^^^^^^^^^^^^^^^^^

You can deploy to Azure Web Apps directly using Git. 

你可以通过 git 将应用程序直接部署到Azure。

- `Create a new Web App <https://tryappservice.azure.com/>`__ in Azure. If you don't have an Azure account, you can `create a free trial <http://azure.microsoft.com/en-us/pricing/free-trial/>`__. 

- `在 Azure 创建一个 Web 应用程序 <https://tryappservice.azure.com/>`__ 。如果你没有 Azure 账号，`你可以免费创建一个试用账号 <http://azure.microsoft.com/en-us/pricing/free-trial/>`__ 。

- Configure the Web App in Azure to support `continuous deployment using Git <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__.

- 配置 Azure Web 应用程序支持使用 `Git 持续部署 <http://azure.microsoft.com/en-us/documentation/articles/web-sites-publish-source-control/>`__ 。

Record the Git URL for the Web App from the Azure portal:

将此 Web 应用程序在 Azure 中的 Git URL 记录下来:

.. image:: your-first-mac-aspnet/_static/azure-portal.png

- In a Terminal window, add a remote named ``azure`` with the Git URL you noted previously.

- 在终端窗口中，用之前记下的 Git URL 新建一个名为 ``azure`` 的远程主机。

  - ``git remote add azure https://ardalis-git@firstaspnetcoremac.scm.azurewebsites.net:443/firstaspnetcoremac.git``

- Push to master.

- 推送到 master 分支。

  - ``git push azure master`` to deploy. 
  
  - 部署： ``git push azure master`` 。

  .. image:: your-first-mac-aspnet/_static/git-push-azure-master.png

- Browse to the newly deployed web app. You should see ``Hello world!``

- 浏览刚才部署的 Web 应用程序，你应该看到输出结果： ``Hello world!`` 。

.. image:: your-first-mac-aspnet/_static/deployment.png 


Additional Resources
--------------------

扩展资源
--------------------

- `Visual Studio Code`_
- :doc:`/client-side/yeoman`
- :doc:`/fundamentals/index`

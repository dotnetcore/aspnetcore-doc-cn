.. _dotnet-watch:

Developing ASP.NET Core applications using dotnet watch
=======================================================

使用 dotnet watch 开发 ASP.NET Core 应用程序
=======================================================

By `Victor Hurdugaci`_

作者 `Victor Hurdugaci`_ 

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_ 

Introduction
------------

介绍
------------

``dotnet watch`` is a development time tool that runs a ``dotnet`` command when source files change. It can be used to compile, run tests, or publish when code changes.

``dotnet watch`` 是一个开发阶段在源文件发生变动的情况下使用 ``dotnet`` 命令的工具。 当代码发生变动的时候可以用来执行编译，运行测试，或者发布操作。

In this tutorial we'll use an existing WebApi application that calculates the sum and product of two numbers to demonstrate the use cases of ``dotnet watch``. The sample application contains an intentional bug that we'll fix as part of this tutorial.

在本教程中，我们将使用一个现有的计算两个数字之和以及乘积的 WebAPI 应用程序来演示如何使用 ``dotnet watch`` 。示例应用程序故意包含一个错误，作为本教程的一部分我们会修复它。

Getting started
---------------

开始入门
---------------

Start by downloading `the sample application <https://github.com/aspnet/Docs/tree/dev/aspnet/tutorials/dotnet-watch/sample>`__. It contains two projects, ``WebApp`` (a web application) and ``WebAppTests`` (unit tests for the web application)

开始下载 `示例程序 <https://github.com/aspnet/Docs/tree/dev/aspnet/tutorials/dotnet-watch/sample>`__。示例程序包含两个项目， ``WebApp`` （Web 应用程序）以及 ``WebAppTests`` （Web 应用程序配套的单元测试项目）

In a console, open the folder where you downloaded the sample application and run:

在命令行控制台中，进入下载示例程序的目录并且运行下述命令：

1.  ``dotnet restore``
2.  ``cd WebApp``
3.  ``dotnet run``

The console output will show messages similar to the ones below, indicating that the application is now running and waiting for requests:

控制台输出将显示如下信息，指示该应用程序正在运行并等待请求：

.. code-block:: bash

  $ dotnet run
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0

  Compilation succeeded.
    0 Warning(s)
    0 Error(s)

  Time elapsed 00:00:02.6049991

  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

In a web browser, navigate to ``http://localhost:5000/api/math/sum?a=4&b=5`` and you should see the result ``9``.

在 Web 浏览器中，导航到 ``http://localhost:5000/api/math/sum?a=4&b=5`` 页面你会看到结果 ``9`` 。

If you navigate to ``http://localhost:5000/api/math/product?a=4&b=5`` instead, you'd expect to get the result ``20``. Instead, you get ``9`` again.

如果你导航到 ``http://localhost:5000/api/math/product?a=4&b=5`` 页面，你期望得到结果 ``20``。但是实际上还是返回了 ``9`` 。

We'll fix that.

我们会修复这个问题的。

Adding ``dotnet watch`` to a project
------------------------------------

项目添加 ``dotnet watch``
------------------------------------

1. Add ``Microsoft.DotNet.Watcher.Tools`` to the ``tools`` section of the *WebApp/project.json* file as in the example below:

1. 按照下面例子的方式在 *WebApp/project.json* 文件的 ``tools`` 配置节中添加 ``Microsoft.DotNet.Watcher.Tools`` 引用：

.. literalinclude:: dotnet-watch/sample/WebAppTests/project.json
   :language: javascript
   :lines: 21-23
   :emphasize-lines: 2
   :dedent: 2

2. Run ``dotnet restore``.

2. 运行 ``dotnet restore``。

The console output will show messages similar to the ones below:

控制台输出将显示如下信息：

.. code-block:: bash

  log  : Restoring packages for /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/project.json...
  log  : Restoring packages for tool 'Microsoft.DotNet.Watcher.Tools' in /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/project.json...
  log  : Installing Microsoft.DotNet.Watcher.Core 1.0.0-preview2-final.
  log  : Installing Microsoft.DotNet.Watcher.Tools 1.0.0-preview2-final.

Running ``dotnet`` commands using ``dotnet watch``
--------------------------------------------------

使用 ``dotnet watch`` 运行 ``dotnet`` 命令
--------------------------------------------------

Any ``dotnet`` command can be run with  ``dotnet watch``:  For example:

任何与 ``dotnet`` 有关的命令都可以以 ``dotnet watch`` 这样的方式运行：例如：

========================================= ======================================
命令                                      带上 watch 的命令Command
========================================= ======================================
``dotnet run``                            ``dotnet watch run``
``dotnet run -f net451``                  ``dotnet watch run -f net451``
``dotnet run -f net451 -- --arg1``        ``dotnet watch run -f net451 -- --arg1``
``dotnet test``                           ``dotnet watch test``
========================================= ======================================

To run ``WebApp`` using the watcher, run ``dotnet watch run`` in the ``WebApp`` folder. The console output will show messages similar to the ones below, indicating that ``dotnet watch`` is now watching code files:

为了让 ``WebApp`` 在 watcher 模式下运行，在 ``WebApp`` 目录里面运行 ``dotnet watch run`` 命令。 控制台输出将显示如下信息，限制 ``dotnet watch`` 现在正在监控代码文件：

.. code-block:: bash

  user$ dotnet watch run
  [DotNetWatcher] info: Running dotnet with the following arguments: run
  [DotNetWatcher] info: dotnet process id: 39746
  Project WebApp (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

Making changes with ``dotnet watch``
------------------------------------

在 ``dotnet watch`` 模式进行修改
------------------------------------

Make sure ``dotnet watch`` is running.

确认 ``dotnet watch`` 模式运行中。

Let's fix the bug that we discovered when we tried to compute the product of two number.

让我们来修复上面发现的那个两个数相乘结果错误。

Open *WebApp/Controllers/MathController.cs*.

打开文件 *WebApp/Controllers/MathController.cs*。

We've intentionally introduced a bug in the code.

我们故意在代码中引入了错误。

.. literalinclude:: dotnet-watch/sample/WebApp/Controllers/MathController.cs
   :language: c#
   :lines: 12-17
   :emphasize-lines: 5
   :dedent: 4

Fix the code by replacing ``a + b`` with ``a * b``.

通过把代码 ``a + b`` 替换为 ``a * b`` 修复错误。

Save the file. The console output will show messages similar to the ones below, indicating that ``dotnet watch`` detected a file change and restarted the application.

保存文件。 控制台输出将显示如下信息，指示 ``dotnet watch`` 检测到文件的改变并重启了应用程序。

.. code-block:: bash

  [DotNetWatcher] info: File changed: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/Controllers/MathController.cs
  [DotNetWatcher] info: Running dotnet with the following arguments: run
  [DotNetWatcher] info: dotnet process id: 39940
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:03.3312829

  Hosting environment: Production
  Content root path: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp
  Now listening on: http://localhost:5000
  Application started. Press Ctrl+C to shut down.

Verify ``http://localhost:5000/api/math/product?a=4&b=5`` returns the correct result.

验证 ``http://localhost:5000/api/math/product?a=4&b=5`` 链接返回正确的结果。

Running tests using ``dotnet watch``
------------------------------------

使用 ``dotnet watch`` 运行测试
------------------------------------

The file watcher can run other ``dotnet`` commands like ``test`` or ``publish``.

文件监控也能运行其他 ``dotnet`` 命令例如 ``test`` 或者 ``publish``。

1. Open the ``WebAppTests`` folder that already has ``dotnet watch`` in *project.json*.
2. Run ``dotnet watch test``.

1. 打开 ``WebAppTests`` 目录，确认 *project.json* 文件中已经包含了 ``dotnet watch`` 。
2. 运行 ``dotnet watch test`` 命令。

If you previously fixed the bug in the ``MathController`` then you'll see an output similar to the one below, otherwise you'll see a test failure:

如果你之前在 ``MathController`` 中修复了错误你会看到控制台输出显示如下信息，否则你会看到测试失败的信息：

.. code-block:: bash

  WebAppTests user$ dotnet watch test
  [DotNetWatcher] info: Running dotnet with the following arguments: test
  [DotNetWatcher] info: dotnet process id: 40193
  Project WebApp (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  Project WebAppTests (.NETCoreApp,Version=v1.0) was previously compiled. Skipping compilation.
  xUnit.net .NET CLI test runner (64-bit .NET Core osx.10.11-x64)
    Discovering: WebAppTests
    Discovered:  WebAppTests
    Starting:    WebAppTests
    Finished:    WebAppTests
  === TEST EXECUTION SUMMARY ===
     WebAppTests  Total: 2, Errors: 0, Failed: 0, Skipped: 0, Time: 0.259s
  SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
  [DotNetWatcher] info: dotnet exit code: 0
  [DotNetWatcher] info: Waiting for a file to change before restarting dotnet...

一旦所有的测试运行起来了，监控器会指示他在下一次重新启动 ``dotnet test`` 前会等待一个文件的变更。

3. Open the controller file in *WebApp/Controllers/MathController.cs* and change some code. If you haven't fixed the product bug, do it now. Save the file.

3. 打开控制器 *WebApp/Controllers/MathController.cs* 文件并且修改代码。如果你没有修复乘法错误，马上修改。并保存。

``dotnet watch`` will detect the file change and rerun the tests. The console output will show messages similar to the one below:

``dotnet watch`` 将会检测到文件变更并且重新运行测试。 控制台输出将显示如下信息：

.. code-block:: bash

  [DotNetWatcher] info: File changed: /Users/user/dev/aspnet/Docs/aspnet/tutorials/dotnet-watch/sample/WebApp/Controllers/MathController.cs
  [DotNetWatcher] info: Running dotnet with the following arguments: test
  [DotNetWatcher] info: dotnet process id: 40233
  Project WebApp (.NETCoreApp,Version=v1.0) will be compiled because inputs were modified
  Compiling WebApp for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:03.2127590
  Project WebAppTests (.NETCoreApp,Version=v1.0) will be compiled because dependencies changed
  Compiling WebAppTests for .NETCoreApp,Version=v1.0
  Compilation succeeded.
    0 Warning(s)
    0 Error(s)
  Time elapsed 00:00:02.1204052

  xUnit.net .NET CLI test runner (64-bit .NET Core osx.10.11-x64)
    Discovering: WebAppTests
    Discovered:  WebAppTests
    Starting:    WebAppTests
    Finished:    WebAppTests
  === TEST EXECUTION SUMMARY ===
     WebAppTests  Total: 2, Errors: 0, Failed: 0, Skipped: 0, Time: 0.260s
  SUMMARY: Total: 1 targets, Passed: 1, Failed: 0.
  [DotNetWatcher] info: dotnet exit code: 0
  [DotNetWatcher] info: Waiting for a file to change before restarting dotnet...

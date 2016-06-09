单元测试
============

作者 `Steve Smith`_ 翻译：王健

ASP.NET Core在设计时考虑到可测试性，让你的应用程序创建单元测试比以往任何时候都更容易。本文简要介绍了单元测试（以及他们从其他类型的测试的区别），并演示了如何添加测试项目到您的解决方案，然后使用命令行或Visual Studio运行单元测试。

.. contents:: Sections:
  :local:
  :depth: 1

`查看或下载代码 <https://github.com/aspnet/Docs/tree/master/aspnet/testing/unit-testing/sample>`__

测试入门
----------------------------

拥有一套自动化测试是保证软件应用程序做它的作者想要它做的最好的方法之一。软件应用程序有很多中测试的种类，  如集成测试doc:`integration tests <integration-testing>`、web 测试，压力测试等等。级别最低的单元测试，它是测试独立的软件组件或者方法。单元测试应该只测试开发者控制范围内的代码，而不应该测试基础设施问题，如数据库，文件系统或网络资源。单元测试可以用测试驱动开发`Test Driven Development (TDD) <http://deviq.com/test-driven-development/>`_来编写，或者可以将单元测试加入到已经存在的代码中来增加它的正确性。在这两种情况下，单元测试应该短小，命名规范，运行迅速，因为理想情况下在提交更改到项目的共享代码库之前您会希望能运行数百个单元测试。

.. 注意:: 开发者总是为给测试类和方法起一个好名字而绞尽脑汁。作为一个引子，ASP.NET 产品团队遵循如下`约定<https://github.com/aspnet/Home/wiki/Engineering-guidelines#unit-tests-and-functional-tests>`__


当编写单元测试时，要小心你会不经意引进对基础设施的依赖。这些往往使测试更慢和更脆弱，因而应预留给集成测试。您可以通过以下的`显式依赖原则<http://deviq.com/explicit-dependencies-principle/>`和依赖注入:doc:`/fundamentals/dependency-injection`在您的应用程序中避免这些隐藏的依赖。您还可以在集成测试的一个独立的项目中进行单元测试，并确保您的单位测试项目没有对基础设施包的引用或依赖关系。


创建测试项目
----------------------

测试项目仅仅是引用了一个类库的测试运行器和被测试的项目（也被称为System Under Test 或 SUT 系统）。在您的SUT项目中组织您的测试项目到一个独立的文件夹，是一个不错的选择，和ASP.NET Core项目推荐的惯例是这样的::

  global.json
  PrimeWeb.sln
  src/
    PrimeWeb/
      project.json
      Startup.cs
      Services/
        PrimeService.cs
  test/
    PrimeWeb.UnitTests/
      project.json
      Services/
        PrimeService_IsPrimeShould.cs

有一个与所测试的项目名称的文件夹/目录是很重要的（在上面项目中是primeweb），因为文件系统是用来找到你的项目的。

配置Test project.json文件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

测试项目的*project.json* 文件中应该添加使用过的测试框架依赖项和SUT项目。例如，想要使用`xUnit test framework <http://xunit.github.io/>`,您应该像以下方式来配置依赖项。

.. literalinclude:: unit-testing/sample/test/PrimeWeb.UnitTests/project.json
  :language: json
  :lines: 20-24
  :linenos:
  :dedent: 2


至于其他的测试框架对.NET Core 的支持，我们将在这里提供链接。我们仅简单使用xUnit作为众多不同的可用于.NET开发的测试框架的一个例子。

除添加依赖项之外，我们希望能使用``dotnet test``来运行测试。为了实现这个，需要在*project.json*中添加以下命令节：

.. literalinclude:: unit-testing/sample/test/PrimeWeb.UnitTests/project.json
  :language: json
  :lines: 25-27
  :linenos:
  :dedent: 2

运行测试
-------------

在运行您运行测试之前，你需要写一些测试。在这个demo中，我已经创建了一个检查是否是素数的简单服务。下面是其中一个测试：

.. literalinclude:: unit-testing/sample/test/PrimeWeb.UnitTests/Services/PrimeService_IsPrimeShould.cs
  :language: c#
  :lines: 18-27
  :linenos:
  :dedent: 8

您可以根据您的使用偏好在命令行或使用Visual Studio运行测试。

Visual Studio
^^^^^^^^^^^^^

要在Visual Studio运行测试，首先打开测试资源管理器选项卡，然后生成解决方案，发现所有可用的测试。一旦你这样做的话，你应该可以看到所有的测试资源管理器窗口的测试。点击运行所有运行测试并查看结果。

.. image:: unit-testing/_static/test-explorer.png

如果您单击左上角的图标，Visual Studio会为您在申请工作的每一个生成后运行的测试，提供即时反馈。

命令行
^^^^^^^^^^^^

运行命令行测试，进入到你的单元测试项目文件夹。然后运行：

  dotnet test

您应该可以看到类似以下的内容：

.. image:: unit-testing/_static/dnx-test.png

dotnet watch
^^^^^^^^^^^^

你可以使用``DOTNET watch``工具自动执行命令,只要该文件夹的内容更改。这可以用来当文件被保存在项目中自动运行测试。注意，即使从测试项目文件夹中运行时，它也会检测到SUT项目和测试项目变化中的变化。

要使用``DOTNET watch``，只需运行它，并传递给``dotnet``命令参数。在这个例子中：

  dotnet watch test

当``DOTNET watch``运行，您可以更新您的测试和（或）应用程序，在保存后您可以看到测试重新运行，如下图所示。
.. image:: unit-testing/_static/dnx-watch.png

自动化测试的一个主要好处是提供快速反馈测试，降低了引入bug和发现bug之间的时间。随着连续运行测试，无论是使用``DOTNET watch``或Visual Studio，当引入了打破了有关程序应该如何运行的现有预期的行为,开发人员几乎可以立即发现.

. tip:: 查看sample_看到完整的测试集和服务行为。你可以运行Web应用程序和浏览``/checkprime?5``测试数据是否是素数。你可以通过:doc:`integration-testing`来了解更多关于测试和重构这个检验素数的web程序。

额外的资源
--------------------

- :doc:`integration-testing`
- :doc:`/fundamentals/dependency-injection`

.. _sample: https://github.com/aspnet/docs/tree/master/aspnet/testing/unit-testing/sample

:version: 1.0.0-rc1

Testing Controller Logic
========================

测试控制器逻辑
========================

By `Steve Smith`_

作者： `Steve Smith`_ 

翻译：`姚阿勇（Dr.Yao） <https://github.com/YaoaY>`_

Controllers in ASP.NET MVC apps should be small and focused on user-interface concerns. Large controllers that deal with non-UI concerns are more difficult to test and maintain.

ASP.NET MVC 应用程序的控制器应当小巧并专注于用户界面。涉及了非 UI 事务的大控制器更难于测试和维护。

.. contents:: Sections
	:local:
	:depth: 1
	
`View or download sample from GitHub <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/testing/sample>`_

`在 GitHub 上查看或下载示例 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/testing/sample>`_

Why Test Controllers
--------------------

为什么要测试控制器
--------------------

Controllers are a central part of any ASP.NET Core MVC application. As such, you should have confidence they behave as intended for your app. Automated tests can provide you with this confidence and can detect errors before they reach production. It's important to avoid placing unnecessary responsibilities within your controllers and ensure your tests focus only on controller responsibilities.

控制器是所有 ASP.NET Core MVC 应用程序的核心部分。因此，你应当确保它们的行为符合应用的预期。 自动化测试可以为你提供这样的保障并能够在进入生产环境之前将错误检测出来。重要的一点是，避免将非必要的职责加入你的控制器并且确保测试只关注在控制器的职责上。

Controller logic should be minimal and not be focused on business logic or infrastructure concerns (for example, data access). Test controller logic, not the framework. Test how the controller *behaves* based on valid or invalid inputs. Test controller responses based on the result of the business operation it performs.

控制器的逻辑应当最小化并且不要去关心业务逻辑或基础事务（如，数据访问）。要测试控制器的逻辑，而不是框架。根据有效或无效的输入去测试控制器的 *行为* 如何。根据其执行业务操作的返回值去测试控制器的响应。

Typical controller responsibilities:

典型的控制器职责：

- Verify ``ModelState.IsValid``
- Return an error response if ``ModelState`` is invalid
- Retrieve a business entity from persistence
- Perform an action on the business entity
- Save the business entity to persistence
- Return an appropriate ``IActionResult``

- 验证 ``ModelState.IsValid``
- 如果 ``ModelState`` 无效则返回一个错误响应
- 从持久层获取一个业务实体
- 在业务实体上执行一个操作
- 将业务实体保存到持久层
- 返回一个合适的 ``IActionResult``

Unit Testing
------------

单元测试
------------

`Unit testing`_ involves testing a part of an app in isolation from its infrastructure and dependencies. When unit testing controller logic, only the contents of a single action is tested, not the behavior of its dependencies or of the framework itself. As you unit test your controller actions, make sure you focus only on its behavior. A controller unit test avoids things like :doc:`filters <filters>`, :doc:`routing </fundamentals/routing>`, or :doc:`model binding </mvc/models/model-binding>`. By focusing on testing just one thing, unit tests are generally simple to write and quick to run. A well-written set of unit tests can be run frequently without much overhead. However, unit tests do not detect issues in the interaction between components, which is the purpose of :ref:`integration testing <integration-testing>`.

`单元测试`_ 包括对应用中独立于基础结构和依赖项之外的某一部分的测试。对控制器逻辑进行单元测试的时候，只测试一个操作的内容，而不测试其依赖项或框架本身的行为。就是说对你的控制器操作进行测试时，要确保只聚焦于操作本身的行为。控制器单元测试避开诸如 :doc:`过滤器 <filters>`， :doc:`路由 </fundamentals/routing>`, or :doc:`模型绑定 </mvc/models/model-binding>` 这些内容。由于只专注于测试某一项内容，单元测试通常编写简单而运行快捷。一组编写良好的单元测试可以无需过多开销地频繁运行。然而，单元测试并不检测组件之间交互的问题，那是 :ref:`集成测试 <integration-testing>` 的目的。



If you've writing custom filters, routes, etc, you should unit test them, but not as part of your tests on a particular controller action. They should be tested in isolation.

如果你在编写自定义的过滤器，路由，诸如此类，你应该对它们进行单元测试，但不是作为某个控制器操作测试的一部分。它们应该单独进行测试。

.. tip:: `Create and run unit tests with Visual Studio <https://www.visualstudio.com/en-us/get-started/code/create-and-run-unit-tests-vs>`__.

.. tip:: `使用 Visual Studio 创建并运行单元测试 <https://www.visualstudio.com/en-us/get-started/code/create-and-run-unit-tests-vs>`__.

To demonstrate unit testing, review the following controller. It displays a list of brainstorming sessions and allows new brainstorming sessions to be created with a POST:

为演示单元测试，请查看下面的控制器。它显示一个头脑风暴讨论会的列表，并且可以用 POST 请求创建新的头脑风暴讨论会：

.. literalinclude:: testing/sample/TestingControllersSample/src/TestingControllersSample/Controllers/HomeController.cs
  :language: c#
  :emphasize-lines: 12,16,21,42-43

The controller is following the `explicit dependencies principle <http://deviq.com/explicit-dependencies-principle/>`_, expecting dependency injection to provide it with an instance of ``IBrainstormSessionRepository``. This makes it fairly easy to test using a mock object framework, like `Moq <https://www.nuget.org/packages/Moq/>`_. The ``HTTP GET Index`` method has no looping or branching and only calls one method. To test this ``Index`` method, we need to verify that a ``ViewResult`` is returned, with a ``ViewModel`` from the repository's ``List`` method.

这个控制器遵循 `显式依赖原则 <http://deviq.com/explicit-dependencies-principle/>`_，期望依赖注入为其提供一个 ``IBrainstormSessionRepository`` 的实例。这样就非常容易用一个 Mock 对象框架来进行测试，比如 `Moq <https://www.nuget.org/packages/Moq/>`_ 。``HTTP GET Index`` 方法没有循环或分支，只是调用了一个方法。要测试这个 ``Index`` 方法，我们需要验证是否返回了一个 ``ViewResult`` ，其中包含一个来自存储库的 ``List`` 方法的 ``ViewModel`` 。 

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/UnitTests/HomeControllerTests.cs
  :language: c#
  :emphasize-lines: 17-18

The ``HTTP POST Index`` method (shown below) should verify:

``HTPP POST Index`` 方法（下面所示）应当验证：

- The action method returns a ``ViewResult`` with the appropriate data when ``ModelState.IsValid`` is ``false``
- The ``Add`` method on the repository is called and a ``RedirectToActionResult`` is returned with the correct arguments when ``ModelState.IsValid`` is true.

- 当 ``ModelState.IsValid`` 为 ``false`` 时，操作方法返回一个包含适当数据的 ``ViewResult``。
- 当 ``ModelState.IsValid`` 为 ``true`` 时，存储库的 ``Add`` 方法被调用，然后返回一个包含正确变量内容的 ``RedirectToActionResult`` 。

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/UnitTests/HomeControllerTests.cs
  :language: c#
  :lines: 29-57
  :dedent: 4
  :emphasize-lines: 1-2,10-12,15-16,19,24-25,28

The first test confirms when ``ModelState`` is not valid, the same ``ViewResult`` is returned as for a ``GET`` request. Note that the test doesn't attempt to pass in an invalid model. That wouldn't work anyway since model binding isn't running - we're just calling the method directly. However, we're not trying to test model binding - we're only testing what our code in the action method does. The simplest approach is to add an error to ``ModelState``.

第一个测试确定当 ``ModelState`` 无效时，返回一个与 ``GET`` 请求一样的 ``ViewResult`` 。注意，测试不会尝试传递一个无效模型进去。那样是没有作用的，因为模型绑定并没有运行 - 我们只是直接调用了操作方法。然而，我们并不想去测试模型绑定 —— 我们只是在测试操作方法里的代码行为。最简单的方法就是在 ``ModelState`` 中添加一个错误。

The second test verifies that when ``ModelState`` is valid, a new ``BrainstormSession`` is added (via the repository), and the method returns a ``RedirectToActionResult`` with the expected properties. Mocked calls that aren't called are normally ignored, but calling ``Verifiable`` at the end of the setup call allows it to be verified in the test. This is done with the call to ``mockRepo.Verify``.

第二个测试验证当 ``ModelState`` 有效时，新的 ``BrainstormSession`` 被添加（通过存储库），并且该方法返回一个带有预期属性值的 ``RedirectToActionResult`` 。未被执行到的 mock 调用通常就被忽略了，但是在设定过程的最后调用 ``Verifiable`` 则允许其在测试中被验证。这是通过调用 ``mockRepo.Verify`` 实现的。

.. note:: The Moq library used in this sample makes it easy to mix verifiable, or "strict", mocks with non-verifiable mocks (also called "loose" mocks or stubs). Learn more about `customizing Mock behavior with Moq <https://github.com/Moq/moq4/wiki/Quickstart#customizing-mock-behavior>`_.

.. note:: 这个例子中所采用的 Moq 库便于将可验证的，或者说 “严格的”，与不可验证的 mock （也称为 “宽松的” mock 或 stub）混合进行 mock 。了解更多关于 `使用 Moq 自定义 Mock 行为 <https://github.com/Moq/moq4/wiki/Quickstart#customizing-mock-behavior>`_ 。

Another controller in the app displays information related to a particular brainstorming session. It includes some logic to deal with invalid id values:

应用程序里的另外一个控制器显示指定头脑风暴讨论会的相关信息。它包含一些处理无效 id 值的逻辑：

.. literalinclude:: testing/sample/TestingControllersSample/src/TestingControllersSample/Controllers/SessionController.cs
  :language: c#
  :emphasize-lines: 16,20,25,33

The controller action has three cases to test, one for each ``return`` statement:

这个控制器操作有三种情况要测试，每条 ``return`` 语句一种：

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/UnitTests/SessionControllerTests.cs
  :language: c#
  :emphasize-lines: 16,26,39

The app exposes functionality as a web API (a list of ideas associated with a brainstorming session and a method for adding new ideas to a session):

这个应用程序以 Web API （一个头脑风暴讨论会的意见列表以及一个给讨论会添加新意见的方法）的形式公开功能：

.. _ideas-controller:

.. literalinclude:: testing/sample/TestingControllersSample/src/TestingControllersSample/Api/IdeasController.cs
  :language: c#
  :emphasize-lines: 20-22,27,29-36,39-41,45,50,60

The ``ForSession`` method returns a list of ``IdeaDTO`` types, with property names camel cased to match JavaScript conventions. Avoid returning your business domain entities directly via API calls, since frequently they include more data than the API client requires, and they unnecessarily couple your app's internal domain model with the API you expose externally. Mapping between domain entities and the types you will return over the wire can be done manually (using a LINQ ``Select`` as shown here) or using a library like `AutoMapper <https://github.com/AutoMapper/AutoMapper>`_

``ForSession`` 方法返回一个 ``IdeaDTO`` 类型的列表，该类型有着符合 JavaScript 惯例的驼峰命名法的属性名。从而避免直接通过 API 调用返回你业务领域的实体，因为通常它们都包含了 API 客户端并不需要的更多数据，而且它们将你的应用程序的内部领域模型与外部公开的 API 不必要地耦合起来。可以手动将业务领域实体与你想要返回的类型连接映射起来（使用这里展示的 LINQ ``Select``），或者使用诸如 `AutoMapper <https://github.com/AutoMapper/AutoMapper>`_ 的类库。

The unit tests for the ``Create`` and ``ForSession`` API methods:

``Create`` 和 ``ForSession`` API 方法的单元测试：

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/UnitTests/ApiIdeasControllerTests.cs
  :language: c#
  :emphasize-lines: 16-17,26-27,37-38,65-66,76-77

As stated previously, to test the behavior of the method when ``ModelState`` is invalid, add a model error to the controller as part of the test. Don't try to test model validation or model binding in your unit tests - just test your action method's behavior when confronted with a particular ``ModelState`` value.

如前所述，要测试这个方法在 ``ModelState`` 无效时的行为，可以将一个模型错误作为测试的一部分添加到控制器。不要在单元测试尝试测试模型验证或者模型绑定 —— 仅仅测试应对特定 ``ModelState`` 值的时候，你的操作方法的行为。

The second test depends on the repository returning null, so the mock repository is configured to return null. There's no need to create a test database (in memory or otherwise) and construct a query that will return this result - it can be done in a single line as shown.

第二项测试需要存储库返回 null ，因此将模拟的存储库配置为返回 null 。没有必要去创建一个测试数据库（内存中的或其他的）并构建一条能返回这个结果的查询 —— 就像展示的那样，一行代码就可以了。

The last test verifies that the repository's ``Update`` method is called. As we did previously, the mock is called with ``Verifiable`` and then the mocked repository's ``Verify`` method is called to confirm the verifiable method was executed. It's not a unit test responsibility to ensure that the ``Update`` method saved the data; that can be done with an integration test.

最后一项测试验证存储库的 ``Update`` 方法是否被调用。像我们之前做过的那样，在调用 mock 时调用了 ``Verifiable`` ，然后模拟存储库的 ``Verify`` 方法被调用，用以确认可验证的方法已被执行。确保 ``Update`` 保存了数据并不是单元测试的职责；那是集成测试做的事。

.. _integration-testing:

Integration Testing
-------------------

集成测试
-------------------

:doc:`Integration testing </testing/integration-testing>` is done to ensure separate modules within your app work correctly together. Generally, anything you can test with a unit test, you can also test with an integration test, but the reverse isn't true. However, integration tests tend to be much slower than unit tests. Thus, it's best to test whatever you can with unit tests, and use integration tests for scenarios that involve multiple collaborators.

:doc:`集成测试 </testing/integration-testing>` 是为了确保你应用程序里各独立模块能够正确地一起工作。通常，能进行单元测试的东西，都能进行集成测试，但反之则不行。不过，集成测试往往比集成测试慢得多。因此，最好尽量采用单元测试，在涉及到多方合作的情况下再进行集成测试。

Although they may still be useful, mock objects are rarely used in integration tests. In unit testing, mock objects are an effective way to control how collaborators outside of the unit being tested should behave for the purposes of the test. In an integration test, real collaborators are used to confirm the whole subsystem works together correctly.

尽管 mock 对象仍然有用，但在集成测试中很少用到它们。在单元测试中，mock 对象是一种有效的方式，根据测试目的去控制测试单元外的合作者应当有怎样的行为。在集成测试中，则采用真实的合作者来确定整个子系统能够正确地一起工作。

Application State
^^^^^^^^^^^^^^^^^

应用程序状态
^^^^^^^^^^^^^^^^^

One important consideration when performing integration testing is how to set your app's state. Tests need to run independent of one another, and so each test should start with the app in a known state. If your app doesn't use a database or have any persistence, this may not be an issue. However, most real-world apps persist their state to some kind of data store, so any modifications made by one test could impact another test unless the data store is reset. Using the built-in ``TestServer``, it's very straightforward to host ASP.NET Core apps within our integration tests, but that doesn't necessarily grant access to the data it will use. If you're using an actual database, one approach is to have the app connect to a test database, which your tests can access and ensure is reset to a known state before each test executes.

在执行集成测试的时候，一个重要的考虑因素就是如何设置你的应用程序的状态。各个测试需要独立地运行，所以每个测试都应该在已知状态下随应用程序启动。如果你的应用没有使用数据库或者任何持久层，这可能不是个问题。然而，大多数真实的应用程序都会将它们的状态持久化到某种数据存储中，所以某个测试对其有任何改动都可能影响到其他测试，除非重置了数据存储。使用内置的 ``TestServer`` ，它可以直接托管我们集成测试中的 ASP.NET Core 应用程序，但又无须对我们将使用的数据授权访问。如果你正在使用真实的数据库，一种方法是让应用程序连接到测试数据库，你的测试可以访问它并且确保在每个测试执行之前会重置到一个已知的状态。

In this sample application, I'm using Entity Framework Core's InMemoryDatabase support, so I can't just connect to it from my test project. Instead, I expose an ``InitializeDatabase`` method from the app's ``Startup`` class, which I call when the app starts up if it's in the ``Development`` environment. My integration tests automatically benefit from this as long as they set the environment to ``Development``. I don't have to worry about resetting the database, since the InMemoryDatabase is reset each time the app restarts.

在这个示例应用程序里，我采用了 Entity Framework Core 的 InMemoryDatabase 支持，因此我可以直接把我的测试项目连接到它。实际上，我在应用程序的 ``Startup`` 类里公开了一个 ``InitializeDatabase`` 方法，我可以在开发（ ``Development`` ）环境中启动应用程序的时候调用这个方法。我的集成测试只要把环境设置为 ``Development`` ，就能自动从中受益。我不需要担心重置数据库，因为 InMemoryDatabase 会在应用程序每次重启的时候重置。

The ``Startup`` class:

.. literalinclude:: testing/sample/TestingControllersSample/src/TestingControllersSample/Startup.cs
  :language: c#
  :emphasize-lines: 19-20,38-39,47,56

You'll see the ``GetTestSession`` method used frequently in the integration tests below.

在下面的集成测试中，你会看到 ``GetTestSession`` 方法被频繁使用。

Accessing Views
^^^^^^^^^^^^^^^
访问视图
^^^^^^^^^^^^^^^

Each integration test class configures the ``TestServer`` that will run the ASP.NET Core app. By default, ``TestServer`` hosts the web app in the folder where it's running - in this case, the test project folder. Thus, when you attempt to test controller actions that return ``ViewResult``, you may see this error:

每一个集成测试类都会配置 ``TestServer`` 来运行 ASP.NET Core 应用程序。默认情况下，``TestServer`` 在其运行的目录下承载 Web 应用程序 —— 在本例中，就是测试项目文件夹。因此，当你尝试测试返回 ``ViewResult`` 的控制器操作的时候，你会看见这样的错误：

.. code-block:: none

  The view 'Index' was not found. The following locations were searched:
  (list of locations)

.. code-block:: none

  未找到视图 “Index”。已搜索以下位置：
  （位置列表）

To correct this issue, you need to configure the server to use the ``ApplicationBasePath`` and ``ApplicationName`` of the web project. This is done in the call to ``UseServices`` in the integration test class shown:

要修正这个问题，你需要配置服务器使其采用 Web 项目的 ``ApplicationBasePath`` 和 ``ApplicationName`` 。这在所示的集成测试类中调用 ``UseServices`` 完成的：

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/IntegrationTests/HomeControllerTests.cs
  :language: c#
  :emphasize-lines: 20,22-32,36-37,42

In the test above, the ``responseString`` gets the actual rendered HTML from the View, which can be inspected to confirm it contains expected results.

在上面的测试中，``responseString`` 从视图获取真实渲染的 HTML ，可以用来检查确认其中是否包含期望的结果。

API Methods
^^^^^^^^^^^

API 方法
^^^^^^^^^^^

If your app exposes web APIs, it's a good idea to have automated tests confirm they execute as expected. The built-in ``TestServer`` makes it easy to test web APIs. If your API methods are using model binding, you should always check ``ModelState.IsValid``, and integration tests are the right place to confirm that your model validation is working properly. 

如果你的应用程序有公开的 Web API，采用自动化测试来确保它们按期望执行是个好主意。内置的 ``TestServer`` 便于测试 Web API。如果你的 API 方法使用了模型绑定，那么你应该始终检查 ``ModelState.IsValid`` ，另外确认你的模型验证工作是否正常应当在集成测试里进行。

The following set of tests target the ``Create`` method in the :ref:`IdeasController <ideas-controller>` class shown above:

.. literalinclude:: testing/sample/TestingControllersSample/tests/TestingControllersSample.Tests/IntegrationTests/ApiIdeasControllerTests.cs
  :language: c#
  :lines: 37-142

下面一组测试针对上文所示的 :ref:`IdeasController <ideas-controller>` 里的 ``Create`` 方法：

Unlike integration tests of actions that returns HTML views, web API methods that return results can usually be deserialized as strongly typed objects, as the last test above shows. In this case, the test deserializes the result to a ``BrainstormSession`` instance, and confirms that the idea was correctly added to its collection of ideas.

不同于对返回 HTML 视图的操作的集成测试，有返回值的 Web API 方法通常能够反序列化为强类型对象，就像上面所示的最后一个测试。在此例中，该测试将返回值反序列化为一个 ``BrainstormSession`` 实例，然后再确认意见是否被正确添加到了意见集合里。

You'll find additional examples of integration tests in this article's `sample project <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/testing/sample>`_.

你可以在 `sample project <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/controllers/testing/sample>`_ 这篇文章里找到更多的集成测试示例。

.. _Unit testing: https://docs.microsoft.com/en-us/dotnet/articles/core/testing/unit-testing-with-dotnet-test

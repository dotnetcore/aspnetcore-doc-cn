集成测试
===================

作者： `Steve Smith`_ 翻译：王健


集成测试确保应用程序的组件组装在一起时正常工作。 ASP.NET Core支持使用单元测试框架和可用于处理没有网络开销请求的内置测试的网络主机集成测试。

.. contents:: Sections:
  :local:
  :depth: 1

`查看或下载代码 <https://github.com/aspnet/docs/tree/master/aspnet/testing/integration-testing/sample>`__

集成测试介绍
-----------------------------------
 
集成测试验证应用程序不同的部位是否正确地组装在一起。不像单元测试 :doc:`unit-testing`，集成测试经常涉及到应用基础设施，如数据库，文件系统，网络资源或网页的请求和响应。单元测试用赝品或模拟对象代替这些问题，但集成测试的目的是为了确认该系统与这些系统的预期运行一致。

集成测试，因为它们执行较大的代码段，并且它们依赖于基础结构组件，往往要比单元测试慢几个数量级。因此，限制你写多少集成测试，特别是如果你可以测试与单元测试相同的行为，是一个不错的选择。

.. tip:: 如果某些行为可以使用一个单元测试或集成测试进行测试，优先单元测试，因为这几乎总是会更快的。你可能有几十或几百个单元测试有许多不同的输入，而只是一个集成测试覆盖了最重要的屈指可数的场景。

Testing the logic within your own methods is usually the domain of unit tests. Testing how your application works within its framework (e.g. ASP.NET) or with a database is where integration tests come into play. It doesn't take too many integration tests to confirm that you're able to write a row to and then read a row from the database. You don't need to test every possible permutation of your data access code - you only need to test enough to give you confidence that your application is working properly.

Integration Testing ASP.NET
---------------------------

To get set up to run integration tests, you'll need to create a test project, refer to your ASP.NET web project, and install a test runner. This process is described in the :doc:`unit-testing` documentation, along with more detailed instructions on running tests and recommendations for naming your tests and test classes.

.. tip:: Separate your unit tests and your integration tests using different projects. This helps ensure you don't accidentally introduce infrastructure concerns into your unit tests, and lets you easily choose to run all tests, or just one set or the other.

The Test Host
^^^^^^^^^^^^^

ASP.NET includes a test host that can be added to integration test projects and used to host ASP.NET applications, serving test requests without the need for a real web host. The provided sample includes an integration test project which has been configured to use `xUnit`_ and the Test Host, as you can see from this excerpt from its *project.json* file:

.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/project.json
  :linenos:
  :language: javascript
  :lines: 21-26
  :dedent: 2
  :emphasize-lines: 5

Once the Microsoft.AspNet.TestHost package is included in the project, you will be able to create and configure a TestServer in your tests. The following test shows how to verify that a request made to the root of a site returns "Hello World!" and should run successfully against the default ASP.NET Empty Web template created by Visual Studio.

.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/PrimeWebDefaultRequestShould.cs
  :linenos:
  :language: c#
  :lines: 10-32
  :dedent: 8
  :emphasize-lines: 6-7

These tests are using the Arrange-Act-Assert pattern, but in this case all of the Arrange step is done in the constructor, which creates an instance of ``TestServer``. There are several different ways to configure a ``TestServer`` when you create it; in this example we are passing in the ``Configure`` method from our system under test (SUT)'s ``Startup`` class. This method will be used to configure the request pipeline of the ``TestServer`` identically to how the SUT server would be configured.

In the Act portion of the test, a request is made to the ``TestServer`` instance for the "/" path, and the response is read back into a string. This string is then compared with the expected string of "Hello World!". If they match, the test passes, otherwise it fails.

Now we can add a few additional integration tests to confirm that the prime checking functionality works via the web application:

.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/PrimeWebCheckPrimeShould.cs
  :linenos:
  :language: c#
  :lines: 10-68
  :dedent: 4
  :emphasize-lines: 8-9

Note that we're not really trying to test the correctness of our prime number checker with these tests, but rather that the web application is doing what we expect. We already have unit test coverage that gives us confidence in ``PrimeService``, as you can see here:

.. image:: integration-testing/_static/test-explorer.png

.. note:: You can learn more about the unit tests in the :doc:`unit-testing` article.

Now that we have a set of passing tests, it's a good time to think about whether we're happy with the current way in which we've designed our application. If we see any `code smells <http://deviq.com/code-smells/>`_, now may be a good time to refactor the application to improve its design.

Refactoring to use Middleware
-----------------------------

Refactoring is the process of changing an application's code to improve its design without changing its behavior. It should ideally be done when there is a suite of passing tests, since these help ensure the system's behavior remains the same before and after the changes. Looking at the way in which the prime checking logic is implemented in our web application, we see:

.. code-block:: c#
  :linenos:
  :emphasize-lines: 13-33

    public void Configure(IApplicationBuilder app,
        IHostingEnvironment env)
    {
        // Add the platform handler to the request pipeline.
        app.UseIISPlatformHandler();
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.Run(async (context) =>
        {
            if (context.Request.Path.Value.Contains("checkprime"))
            {
                int numberToCheck;
                try
                {
                    numberToCheck = int.Parse(context.Request.QueryString.Value.Replace("?",""));
                    var primeService = new PrimeService();
                    if (primeService.IsPrime(numberToCheck))
                    {
                        await context.Response.WriteAsync(numberToCheck + " is prime!");
                    }
                    else
                    {
                        await context.Response.WriteAsync(numberToCheck + " is NOT prime!");
                    }
                }
                catch
                {
                    await context.Response.WriteAsync("Pass in a number to check in the form /checkprime?5");
                }
            }
            else
            {
                await context.Response.WriteAsync("Hello World!");
            }
        });
    }

This code works, but it's far from how we would like to implement this kind of functionality in an ASP.NET application, even as simple a one as this is. Imagine what the ``Configure`` method would look like if we needed to add this much code to it every time we added another URL endpoint! 

One option we can consider is adding :doc:`MVC </mvc/index>` to the application, and creating a controller to handle the prime checking. However, assuming we don't currently need any other MVC functionality, that's a bit overkill. 

We can, however, take advantage of ASP.NET Core :doc:`middleware </fundamentals/middleware>`, which will help us encapsulate the prime checking logic in its own class and achieve better `separation of concerns <http://deviq.com/separation-of-concerns/>`_ within the ``Configure`` method.

We want to allow the path the middleware uses to be specified as a parameter, so the middleware class expects a ``RequestDelegate`` and a ``PrimeCheckerOptions`` instance in its constructor. If the path of the request doesn't match what this middleware is configured to expect, we simply call the next middleware in the chain and do nothing further. The rest of the implementation code that was in ``Configure`` is now in the ``Invoke`` method.

.. note:: Since our middleware depends on the ``PrimeService`` service, we are also requesting an instance of this service via the constructor. The framework will provide this service via :doc:`/fundamentals/dependency-injection`, assuming it has been configured (e.g. in ``ConfigureServices``).

.. literalinclude:: integration-testing/sample/src/PrimeWeb/Middleware/PrimeCheckerMiddleware.cs
  :linenos:
  :language: c#
  :emphasize-lines: 39-62

.. note:: Since this middleware acts as an endpoint in the request delegate chain when its path matches, there is no call to ``_next.Invoke`` in the case where this middleware handles the request.

With this middleware in place and some helpful extension methods created to make configuring it easier, the refactored ``Configure`` method looks like this:

.. literalinclude:: integration-testing/sample/src/PrimeWeb/Startup.cs
  :linenos:
  :language: c#
  :lines: 18-34
  :dedent: 8
  :emphasize-lines: 11

Following this refactoring, we are confident that the web application still works as before, since our integration tests are all passing.

.. tip:: It's a good idea to commit your changes to source control after you complete a refactoring and your tests all pass. If you're practicing Test Driven Development, `consider adding Commit to your Red-Green-Refactor cycle <http://ardalis.com/rgrc-is-the-new-red-green-refactor-for-test-first-development>`_.

Summary
-------

Integration testing provides a higher level of verification than unit testing. It tests application infrastructure and how different parts of an application work together. ASP.NET Core is very testable, and ships with a ``TestServer`` that makes wiring up integration tests for web server endpoints very easy.

Additional Resources
--------------------

- :doc:`unit-testing`
- :doc:`/fundamentals/middleware`

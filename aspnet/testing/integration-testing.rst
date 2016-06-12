集成测试
========

作者： `Steve Smith`_ 

翻译：`王健 <https://github.com/wjhgzx>`_

集成测试确保应用程序的组件组装在一起时正常工作。 ASP.NET Core支持使用单元测试框架和可用于处理没有网络开销请求的内置测试的网络主机集成测试。

.. contents:: Sections:
  :local:
  :depth: 1

`查看或下载代码 <https://github.com/aspnet/docs/tree/master/aspnet/testing/integration-testing/sample>`__

集成测试介绍
------------
 
集成测试验证应用程序不同的部位是否正确地组装在一起。不像单元测试 :doc:`unit-testing` ，集成测试经常涉及到应用基础设施，如数据库，文件系统，网络资源或网页的请求和响应。单元测试用赝品或模拟对象代替这些问题，但集成测试的目的是为了确认该系统与这些系统的预期运行一致。

集成测试，因为它们执行较大的代码段，并且它们依赖于基础结构组件，往往要比单元测试慢几个数量级。因此，限制你写多少集成测试，特别是如果你可以测试与单元测试相同的行为，是一个不错的选择。

.. tip:: 如果某些行为可以使用一个单元测试或集成测试进行测试，优先单元测试，因为这几乎总是会更快的。你可能有几十或几百个单元测试有许多不同的输入，而只是一个集成测试覆盖了最重要的屈指可数的场景。

在您自己的方法中测试逻辑通常是单元测试的范畴。测试您的应用程序在它的框架内（例如ASP.NET），或是与一个数据库是否正常运行，是集成测试的工作。它并不需要太多的集成测试，以确认你能写一行，然后从数据库中读取一行。你并不需要测试的数据访问代码每一个可能的排列——您仅需要充足的测试来给您信心认为您的应用程序能够运行良好。

ASP.NET 集成测试
----------------

要建立运行集成测试，你需要创建一个测试项目，请参考ASP.NET的Web项目，并安装测试器。此过程在：:doc:`unit-testing`中有更详细的说明，为您命名您的测试和测试类提供了建议。 

.. top:: 单独的单元测试和集成测试使用不同的项目。这有助于确保您不小心将基础设施问题引入到您的单元测试中，让您轻松选择运行所有的测试，或是一组或其他。

测试宿主
--------

ASP.NET包括可添加到集成测试项目的测试宿主和用于托管ASP.NET应用程序，用于处理测试请求，而不需要一个真实的虚拟宿主。所提供的示例包括被配置为使用 `xUnit`_ 的集成测试项目和测试主机，您可以从 *project.json*  文件中进行查看。


.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/project.json
  :linenos:
  :language: javascript
  :lines: 21-26
  :dedent: 2
  :emphasize-lines: 5

当Microsoft.AspNet.TestHost包被包含在项目中，您将能够在您的测试中创建和配置TESTSERVER。下面的测试演示了如何验证一个对网站的根节点提出了请求并返回的“Hello World！”，并且应该利用Visual Studio中创建的默认ASP.NET空Web模板中成功运行。

.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/PrimeWebDefaultRequestShould.cs
  :linenos:
  :language: c#
  :lines: 10-32
  :dedent: 8
  :emphasize-lines: 6-7


这些测试使用安排-执行-断言的模型，但是在这种情况下，所有的安排步骤都在构造器中完成了，它创建了一个 ``TestServer`` 的实例。当您创建 ``TestServer`` 时，有好几种不同的方式来配置它；在这个示例中，我们从被测试的系统（SUT）的 ``Startup`` 类中的 ``Configure`` 方法进行设置。这种方法可用于配置TestServer请求管道，与如何配置SUT服务器相同。

在测试的行动部分，发起一个对 ``TestServer`` 实例的“/”路径的请求，并且响应读回字符串。这个字符串将与预期的字符串"Hello World!"进行对比。如果匹配，测试通过，否则测试失败。

现在我们可以添加一些附加的集成测试，来确认通过web应用程序的素数检测功能性工作：

.. literalinclude:: integration-testing/sample/test/PrimeWeb.IntegrationTests/PrimeWebCheckPrimeShould.cs
  :linenos:
  :language: c#
  :lines: 10-68
  :dedent: 4
  :emphasize-lines: 8-9

需要注意的是，我们并不是想使用这些测试用例来测试质数检查程序的正确性，而是确认Web应用程序在我们期待的事情。我们已经有对 ``PrimeService`` 充满信心的单元测试覆盖率，您可以在这里看到：

.. image:: integration-testing/_static/test-explorer.png

.. Note:: 您可以从 :doc:`unit-testing`的文章中了解更多关于单元测试的内容。

现在，我们有一组通过的测试，是一个好的机会来考虑我们是否对设计应用程序的方案感到满意了。如果我们发现任何 `代码异味 <http://deviq.com/code-smells/>`_，这将是一个重构应用程序来改善设计的好时机。

使用中间件重构
-------------

重构是改变一个应用程序的代码，以提高其设计而不改变其行为的过程。当有一套通过的测试，重构将理想的进行，因为这些有助于确保系统的行为在重构之前和之后保持不变。看看素数检测逻辑在我们的web应用程序中的实现方式，我们发现：

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


这段代码能正确运行，但远远不是我们想在ASP.NET应用中实现这种功能的方式，即使和这段代码一样简单。想象一下，如果我们在每次添加另一个URL终结点时，我们需要在它的代码中添加那么多代码，``Configure`` 方法会是什么样子呢！


一个选择是，可以考虑在应用程序中添加 :doc:`MVC </mvc/index>`，并创建一个控制器来处理素数检测。然而，假设我们目前不需要任何其它MVC的功能，这是一个有点矫枉过正。

然而，我们可以利用ASP.NET Core  :doc:`中间件 </fundamentals/middleware>` 的优势，可以帮助我们在它自己的类中封装素数检测的逻辑，并且在 ``Configure`` 方法中实现更好的 `关注点分离 <http://deviq.com/separation-of-concerns/>`_ 。

我们想让中间件使用的路径被指定为一个参数，所以中间件类在他的构造方法中预留了一个 ``RequestDelegate`` 和一个 ``PrimeCheckerOptions`` 实例。如果请求的路径与中间件期望的配置不匹配，我们只需要调用链表中的下一个中间件，并不做进一步处理。其余的在 ``Configure`` 中的实现代码，现在在 ``Invoke`` 方法中了。

.. tip:: 由于我们的中间件取决于 ``PrimeService`` 服务，我们也通过构造函数请求该服务的实例。该框架通过依赖注入来提供这项服务，查看 :doc:`/fundamentals/dependency-injection` ，假设已经进行了配置(例如在 ``ConfigureServices`` 中)。

.. literalinclude:: integration-testing/sample/src/PrimeWeb/Middleware/PrimeCheckerMiddleware.cs
  :linenos:
  :language: c#
  :emphasize-lines: 39-62

.. tip:: 由于这个中间件作为请求委托链的一个endpoint,当它的路径匹配时，在这种情况下这个中间件处理请求时并没有调用 ``_next.Invoke`` 

有了合适的中间件和一写有用的扩展方法，使配置更加容易。重构过的 ``Configure`` 方法看起来像这样：

.. literalinclude:: integration-testing/sample/src/PrimeWeb/Startup.cs
  :linenos:
  :language: c#
  :lines: 18-34
  :dedent: 8
  :emphasize-lines: 11

在这重构之后，我们有信心Web应用程序仍然像之前一样工作，因为我们的集成测试都是通过的。

.. tip:: 当您完成重构并且所有测试都通过后，提交您的变更到源代码管理中，是一个好的主意。如果您正尝试测试驱动开发，`考虑提交代码到你的Red-Green-Refacotr循环中 <http://ardalis.com/rgrc-is-the-new-red-green-refactor-for-test-first-development>`_。

总结
-------

集成测试提供了比单元测试更高层次的验证。它测试应用程序的基础设施和应用程序的不同部分如何一起工作。 ASP.NET Core 有很大可测试性，并附带了 ``TestServer`` 这使得为Web服务器endpoint连布置集成测试变得非常简单。

附加的资源
---------

- :doc: `unit-testing`
- :doc: `/fundamentals/middleware`

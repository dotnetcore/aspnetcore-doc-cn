Injecting Services Into Views
=============================

注入服务到视图中
================

作者： `Steve Smith`_

翻译： `Dr.Yao <https://github.com/yaoay>`_

ASP.NET Core supports :doc:`dependency injection </fundamentals/dependency-injection>` into views. This can be useful for view-specific services, such as localization or data required only for populating view elements. You should try to maintain `separation of concerns <http://deviq.com/separation-of-concerns>`_ between your controllers and views. Most of the data your views display should be passed in from the controller.

ASP.NET Core 支持在视图中使用 :doc:`依赖注入 </fundamentals/dependency-injection>` 。对于针对视图的服务非常有用，比如本地化或者仅用于填充视图元素的数据。你应该尝试保持控制器和视图间的关注点分离（ `separation of concerns <http://deviq.com/separation-of-concerns>`_ ）。你的视图所显示的大部分数据都应该从控制器传入。

.. contents:: 章节：
  :local:
  :depth: 1

`查看或下载示例源码 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/dependency-injection/sample>`__

A Simple Example
----------------
You can inject a service into a view using the ``@inject`` directive. You can think of ``@inject`` as adding a property to your view, and populating the property using DI.

一个简单的示例
---------------

你可以使用 ``@inject`` 直接将服务注入视图中。你可以把 ``@inject`` 看作是给你的视图增加一个属性，然后使用依赖注入填充属性。

``@inject`` 的语法：
  ``@inject <type> <name>``

An example of ``@inject`` in action:

一个使用 ``@inject`` 的例子：

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Views/Todo/Index.cshtml
  :linenos:
  :language: c#
  :emphasize-lines: 4-5,15-17

This view displays a list of ``ToDoItem`` instances, along with a summary showing overall statistics. The summary is populated from the injected ``StatisticsService``. This service is registered for dependency injection in ``ConfigureServices`` in *Startup.cs*:

这个视图显示了一个 ``ToDoItem`` 实例的列表，加上一个总体统计概览。概览信息是由注入的 ``StatisticsService`` 填入的。这个服务是在 *Startup.cs* 中的 ``ConfigureServices`` 里被注册到依赖注入的。

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Startup.cs
  :linenos:
  :lines: 15-22
  :language: c#
  :emphasize-lines: 5-6
  :dedent: 8
  
The ``StatisticsService`` performs some calculations on the set of ``ToDoItem`` instances, which it accesses via a repository:

``StatisticsService`` 通过仓库（ ``Repository`` ）对 ``ToDoItem`` 数据集执行一些计算。  

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Model/Services/StatisticsService.cs
  :linenos:
  :language: c#
  :emphasize-lines: 16,21,27

The sample repository uses an in-memory collection. The implementation shown above (which operates on all of the data in memory) is not recommended for large, remotely accessed data sets.

示例仓库使用的是 in-memory 集合。上面的实现方法（所有对内存数据的操作）不推荐用于大型、远程存储的数据集。

The sample displays data from the model bound to the view and the service injected into the view:

这个示例通过绑定到视图的模型以及注入到视图的服务来显示数据：

.. image:: dependency-injection/_static/screenshot.png

Populating Lookup Data
----------------------
View injection can be useful to populate options in UI elements, such as dropdown lists. Consider a user profile form that includes options for specifying gender, state, and other preferences. Rendering such a form using a standard MVC approach would require the controller to request data access services for each of these sets of options, and then populate a model or ``ViewBag`` with each set of options to be bound.

填充查阅数据
-------------

视图注入有助于填充 UI 元素的可选项，例如下拉列表。设想一个包括性别、地区以及其他可选项的用户资料表格。如果通过标准的MVC方式渲染这个表格，则需要控制器为每一种选项请求数据访问服务，然后再为绑定的每一种选项填充一个模型或 ``ViewBag``。对于针对视图的服务非常有用，比如本地化或者仅用于填充视图元素的数据。 

An alternative approach injects services directly into the view to obtain the options. This minimizes the amount of code required by the controller, moving this view element construction logic into the view itself. The controller action to display a profile editing form only needs to pass the form the profile instance:

另一种方法则直接将服务注入到视图中从而获取这些可选项数据。这种方法将控制器需要的代码量减少到了最少，把构造视图元素的逻辑移到视图本身中去。用来显示资料编辑表格的控制器 ``action`` 只需要把资料实例传给表格就可以了（而不需要传各种选项数据，译注）：



.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Controllers/ProfileController.cs
  :linenos:
  :language: c#
  :emphasize-lines: 9,19

The HTML form used to update these preferences includes dropdown lists for three of the properties:

用来编辑这些选项的 HTML 表格包含了其中三个下拉列表：

.. image:: dependency-injection/_static/updateprofile.png

These lists are populated by a service that has been injected into the view:

这些列表是由传入视图的一个服务填充的：

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Views/Profile/Index.cshtml
  :linenos:
  :language: c#
  :emphasize-lines: 4,16-17,21-22,26-27

The ``ProfileOptionsService`` is a UI-level service designed to provide just the data needed for this form:

``ProfileOptionsService`` 是一个被设计用来仅为这个表格提供所需数据的 UI 层的服务：

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Model/Services/ProfileOptionsService.cs
  :linenos:
  :language: c#
  :emphasize-lines: 7,13,24

.. tip:: Don't forget to register types you will request through dependency injection in the  ``ConfigureServices`` method in *Startup.cs*.

.. tip:: 不要忘记在 *Startup.cs* 的 ``ConfigureServices`` 方法中把你想要通过依赖注入请求的服务注册一下。

Overriding Services
-------------------
In addition to injecting new services, this technique can also be used to override previously injected services on a page. The figure below shows all of the fields available on the page used in the first example:

重写服务
---------
除了注入新服务之外，这种技术也可以用来重写之前已经在页面上注入的服务。下图显示了第一个例子的页面上可用的所有字段：

.. image:: dependency-injection/_static/razor-fields.png

As you can see, the default fields include ``Html``, ``Component``, and ``Url`` (as well as the ``StatsService`` that we injected). If for instance you wanted to replace the default HTML Helpers with your own, you could easily do so using ``@inject``:

如你所见，默认的字段有 ``Html`` ， ``Component`` ， 和 ``Url`` （同样还有我们注入的 ``StatsService``）。假如你想要把默认的 HTML Helpers 替换成你自己的，你可以利用 ``@inject`` 轻松实现：

.. literalinclude:: dependency-injection/sample/src/ViewInjectSample/Views/Helper/Index.cshtml
  :linenos:
  :language: html
  :emphasize-lines: 5,13

If you want to extend existing services, you can simply use this technique while inheriting from or wrapping the existing implementation with your own.

如果你想要扩展已有服务，你只需要在使用这种替换技术的同时，让你自己的服务继承或包装已有实现。

See Also
--------
* Simon Timms Blog: `Getting Lookup Data Into Your View <http://blog.simontimms.com/2015/06/09/getting-lookup-data-into-you-view/>`_

参考
-----
* Simon Timms 的博文: `Getting Lookup Data Into Your View <http://blog.simontimms.com/2015/06/09/getting-lookup-data-into-you-view/>`_

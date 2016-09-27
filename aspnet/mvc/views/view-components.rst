:version: 1.0.0-rc1

.. View Components
================

视图组件
================

作者： `Rick Anderson`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `高嵩 <https://github.com/jack2gs>`_  

.. contents:: 章节:
  :local:
  :depth: 1

.. `View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/view-components/sample>`__

`查看或下载示例代码 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/view-components/sample>`__

.. Introducing view components
---------------------------

介绍视图组件
---------------------------

.. New to ASP.NET Core MVC, view components are similar to partial views, but they are much more powerful. View components don’t use model binding, and only depend on the data you provide when calling into it. A view component:

.. - Renders a chunk rather than a whole response
.. - Includes the same separation-of-concerns and testability benefits found between a controller and view
.. - Can have parameters and business logic
.. - Is typically invoked from a layout page

视图组件是 ASP.NET Core MVC 中的新特性，与局部视图相似，但是它们更加的强大。视图组件不使用模型绑定，只取决于调用它时所提供的数据。视图组件有以下特点：

- 渲染一个块，而不是整个响应
- 在控制器和视图之间同样包含了关注点分离和可测试性带来的好处
- 可以拥有参数和业务逻辑
- 通常从布局页调用

.. View Components are intended anywhere you have reusable rendering logic that is too complex for a partial view, such as: 

.. - Dynamic navigation menus
.. - Tag cloud (where it queries the database)
.. - Login panel
.. - Shopping cart
.. - Recently published articles
.. - Sidebar content on a typical blog 
.. - A login panel that would be rendered on every page and show either the links to log out or log in, depending on the log in state of the user

视图组件可以用在任何需要重复逻辑且对局部视图来说过于复杂的情况，比如：

- 动态导航菜单
- 标签云 (需要从数据库查询时)
- 登录面板
- 购物车
- 最近发表的文章
- 一个典型博客的侧边栏内容 
- 会在所有页面渲染的登录面板，根据用户登录状态显示登录或者注销

.. A `view component <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__ consists of two parts, the class (typically derived from  ``ViewComponent``) and the result it returns (typically a view). Like controllers, a view component can be a POCO, but most developers will want to take advantage of the methods and properties available by deriving from ``ViewComponent``.

一个 `视图组件 <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__ 包含两个部分，类（通常派生自 ``ViewComponent`` ）和它返回的结果（通常是一个视图）。类似控制器，视图组件可以是 POCO 类型，但是大部分开发者想要使用派生自 ``ViewComponent`` 的方法和属性。

.. Creating a view component
---------------------------

创建视图组件
---------------------------

.. This section contains the high level requirements to create a view component. Later in the article we'll examine each step in detail and create a view component.

这个章节包含创建视图组件的高级需求。在稍后的文章中，我们将详细地检查每一个步骤，并创建一个视图组件。

.. The view component class
^^^^^^^^^^^^^^^^^^^^^^^^^

视图组件类
^^^^^^^^^^^^^^^^^^^^^^^^^

.. A view component class can be created by any of the following:

.. - Deriving from `ViewComponent`
.. - Decorating a class with the ``[ViewComponent]`` attribute, or deriving from a class with the ``[ViewComponent]`` attribute
.. - Creating a class where the name ends with the suffix *ViewComponent*

一个视图组件类可以由以下任何一个方式创建：

- 派生自 `ViewComponent`
- 使用 ``[ViewComponent]`` 特性装饰一个类，或者这个类的派生类。
- 创建一个类，并以 *ViewComponent* 作为后缀。

.. Like controllers, view components must be public, non-nested, and non-abstract classes. The view component name is the class name with the "ViewComponent" suffix removed. It can also be explicitly specified using the `ViewComponentAttribute.Name`_ property.

如同控制器一样，视图组件必须是公开的，非嵌套的，以及非抽象的类。视图组件名称是类名并去掉“ViewComponent”后缀。也可以通过 `ViewComponentAttribute.Name`_ 属性进行明确的指定。

.. A view component class:

.. - Fully supports constructor :doc:`dependency injection </fundamentals/dependency-injection>`
.. - Does not take part in the controller lifecycle, which means you can't use :doc:`filters </mvc/controllers/filters>` in a view component

一个视图组件类：

- 完全支持构造函数 :doc:`依赖注入 </fundamentals/dependency-injection>`
- 不参与控制器生命周期，意味着你不能在视图组件中使用 :doc:`过滤器 </mvc/controllers/filters>`

.. View component methods
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

视图组件方法
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. A view component defines its logic in an ``InvokeAsync`` method that returns an `IViewComponentResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/IViewComponentResult/index.html>`__. Parameters come directly from invocation of the view component, not from model binding. A view component never directly handles a request. Typically a view component initializes a model and passes it to a view by calling the `View <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__ method. In summary, view component methods:

.. - Define an `InvokeAsync`` method that returns an ``IViewComponentResult``
.. - Typically initializes a model and passes it to a view by calling the `ViewComponent <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__  `View <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewResult/index.html>`__ method
.. - Parameters come from the calling method, not HTTP, there is no model binding
.. - Are not reachable directly as an HTTP endpoint, they are invoked from your code (usually in a view). A view component never handles a request
.. - Are overloaded on the signature rather than any details from the current HTTP request

视图组件在 ``InvokeAsync`` 方法中中定义逻辑，并返回 `IViewComponentResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/IViewComponentResult/index.html>`__。参数直接来自视图组件的调用，而不是来自模型绑定。视图组件从来不直接处理请求。通常视图组件初始化模型并通过调用 `View <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__ 方法传递它到视图。总之，视图组件方法有以下特点：

- 定义一个 ``InvokeAsync`` 方法并返回 ``IViewComponentResult``
- 通常初始化模型并通过调用  `ViewComponent <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html>`__  `View <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewResult/index.html>`__ 方法传递它到视图
- 参数来自调用方法，而不是 HTTP，没有模型绑定
- 不可直接作为 HTTP 终结点，它们从你的代码中调用（通常在视图中）。视图组件从不处理请求
- 重载的签名，而不是当前 HTTP 请求中的任何细节

.. View search path
^^^^^^^^^^^^^^^^^^

视图搜索路径
^^^^^^^^^^^^^^^^^^

.. The runtime searches for the view in the following paths:

..  - Views/<controller_name>/Components/<view_component_name>/<view_name>
..  - Views/Shared/Components/<view_component_name>/<view_name>

运行时对视图的搜索路径如下：

  - Views/<controller_name>/Components/<view_component_name>/<view_name>
  - Views/Shared/Components/<view_component_name>/<view_name>
  
.. The default view name for a view component is *Default*, which means your view file will typically be named *Default.cshtml*. You can specify a different view name when creating the view component result or when calling the ``View`` method.

视图组件默认的视图名是 *Default*，意味着通常你的视图文件会命名为 *Default.cshtml*。当你创建视图组件结果或者调用 ``View`` 方法的时候，你可以指定不同的视图名。
 
.. We recommend you name the view file *Default.cshtml* and use the *Views/Shared/Components/<view_component_name>/<view_name>* path. The ``PriorityList`` view component used in this sample uses *Views/Shared/Components/PriorityList/Default.cshtml* for the view component view.

我们建议你命名视图文件为 *Default.cshtml* 并且使用 *Views/Shared/Components/<view_component_name>/<view_name>* 路径。在这个例子中使用的 ``PriorityList`` 视图组件使用了 *Views/Shared/Components/PriorityList/Default.cshtml* 这个路径。

.. Invoking a view component
-------------------------

调用视图组件
-------------------------

.. To use the view component, call ``@Component.InvokeAsync("Name of view component", <anonymous type containing parameters>)`` from a view. The parameters will be passed to the ``InvokeAsync`` method.  The ``PriorityList`` view component developed in the article is invoked from the *Views/Todo/Index.cshtml* view file. In the following, the ``InvokeAsync`` method is called with two parameters:

要使用视图组件，从视图中调用 ``@Component.InvokeAsync("视图组件名", <匿名类型参数>)``。参数将传递给 ``InvokeAsync`` 方法。在文章中开发的 ``PriorityList`` 视图组件被 *Views/Todo/Index.cshtml* 视图文件调用。在下面，使用了两个参数调用 ``InvokeAsync`` 方法：

.. code-block:: HTML

   @await Component.InvokeAsync("PriorityList", new { maxPriority = 2, isDone = false }) 

.. Invoking a view component directly from a controller
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

从控制器直接调用视图组件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. View components are typically invoked from a view, but you can invoke them directly from a controller method. While view components do not define endpoints like controllers, you can easily implement a controller action that returns the content of a `ViewComponentResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponentResult/index.html>`__. 

视图组件通常从视图中调用，但是你也可以从控制器方法中直接调用。当视图组件没有像控制器一样定义终结点时，你可以简单实现一个控制器的 Action ，并使用一个 `ViewComponentResult <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponentResult/index.html>`__ 作为返回内容。

.. In this example, the view component is called directly from the controller:

在这例子中，视图组件通过控制器直接调用：

.. literalinclude:: view-components/sample/ViewCompFinal/Controllers/ToDoController.cs
 :language: c#
 :lines: 23-26
 :dedent: 6
 
.. Walkthrough: Creating a simple view component
----------------------------------------------

演练：创建一个简单的视图组件
----------------------------------------------

.. `Download <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/view-components/sample>`__, build and test the starter code. It's a simple project with a ``Todo`` controller that displays a list of *Todo* items.

`下载 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/view-components/sample>`__，生成并测试启动代码。这是一个简单的项目，使用一个 ``Todo`` 控制器来显示 *Todo* 项列表。

.. image:: view-components/_static/2dos.png

 
.. Add a ViewComponent class
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

添加一个视图组件类
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. Create a *ViewComponents* folder and add the following ``PriorityListViewComponent`` class.

创建一个 *ViewComponents* 文件夹并添加下面的 ``PriorityListViewComponent`` 类。

.. literalinclude:: view-components/sample/ViewCompFinal/ViewComponents/PriorityListViewComponent1.cs
 :language: c#
 :lines: 3-33

.. Notes on the code: 

.. - View component classes can be contained in **any** folder in the project.
.. - Because the class name ``PriorityListViewComponent`` ends with the suffix **ViewComponent**, the runtime will use the string "PriorityList" when referencing the class component from a view. I'll explain that in more detail later. 
.. - The ``[ViewComponent]`` attribute can change the name used to reference a view component. For example, we could have named the class ``XYZ``,  and  applied the  ``ViewComponent`` attribute:

..   code-block:: c#   
    [ViewComponent(Name = "PriorityList")]
    public class XYZ : ViewComponent

.. - The ``[ViewComponent]`` attribute above tells the view component selector to use the name ``PriorityList`` when looking for the views associated with the component, and to use the string "PriorityList" when referencing the class component from a view. I'll explain that in more detail later. 
.. - The component uses :doc:`dependency injection </fundamentals/dependency-injection>` to make the data context available. 
.. - ``InvokeAsync`` exposes a method which can be called from a view, and it can take an arbitrary number of arguments. 
.. - The ``InvokeAsync`` method returns the set of ``ToDo`` items that are not completed and have priority lower than or equal to ``maxPriority``.

代码注释：

- 视图组件类可以被放在项目中 **任何** 文件夹内。
- 因为类命名为 ``PriorityListViewComponent``，以 **ViewComponent** 作为后缀结束，在运行时会从视图中使用 "PriorityList" 字符串来引用组件类。我会在后面详细解释。
- ``[ViewComponent]`` 特性可以改变被用来引用视图组件的名字。比如，我们可以命名类为 ``XYZ``，然后应用 ``ViewComponent`` 特性：

  .. code-block:: c#
    
    [ViewComponent(Name = "PriorityList")]
    public class XYZ : ViewComponent

- 上面的 ``[ViewComponent]`` 特性告知视图组件选择器在寻找与组件相关的视图时使用名字 ``PriorityList``，并且在从视图中引用组件类时使用 "PriorityList" 字符串。我会在后面详细解释。 
- 组件使用 :doc:`依赖注入 </fundamentals/dependency-injection>` 使得数据上下文可用。 
- ``InvokeAsync`` 暴露一个可以在视图中调用的方法，并且它可以接受任意数量的参数。 
- ``InvokeAsync`` 方法返回没有完成并优先级小于等于 ``maxPriority`` 的 ``ToDo`` 项的集合。

.. Create the view component Razor view
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

创建视图组件 Razor 视图
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

.. #. Create the *Views/Shared/Components* folder. This folder **must** be named *Components*.
.. #. Create the *Views/Shared/Components/PriorityList* folder. This folder name must match the name of the view component class, or the name of the class minus the suffix (if we followed convention and used the *ViewComponent* suffix in the class name). If you used the ``ViewComponent`` attribute, the class name would need to match the attribute designation.
.. #. Create a *Views/Shared/Components/PriorityList/Default.cshtml* Razor view. 

#. 创建 *Views/Shared/Components* 文件夹。这个文件夹 **必须** 命名为 *Components*。
#. 创建 *Views/Shared/Components/PriorityList* 文件夹。这个文件夹必须和视图组件类名字匹配，或者是类名去掉后缀（如果我们遵循了约定并且使用 *ViewComponent* 作为类名后缀）。如果你使用 ``ViewComponent`` 特性，类名需要匹配特性中指定的名字。
#. 创建一个 *Views/Shared/Components/PriorityList/Default.cshtml* Razor 视图。 

.. literalinclude:: view-components/sample/ViewCompFinal/Views/Shared/Components/PriorityList/Default1.cshtml
 :language: html 

.. The Razor view takes a list of ``TodoItem`` and displays them. If the view component ``InvokeAsync`` method doesn't pass the name of the view (as in our sample), *Default* is used for the view name by convention. Later in the tutorial, I'll show you how to pass the name of the view. To override the default styling for a specific controller, add a view to the controller specific view folder (for example *Views/Todo/Components/PriorityList/Default.cshtml)*.

Razor 视图取一组 ``TodoItem`` 并显示它们。如果视图组件的 ``InvokeAsync`` 方法没有传递视图名（就像我们例子中），按照约定会使用 *Default* 作为视图名。在教程的后面部分，我会告诉你如何传递视图的名称。为特定控制器重写默认的样式，添加一个视图到特定控制器的视图文件夹（比如 *Views/Todo/Components/PriorityList/Default.cshtml*）。

.. If the view component was controller specific, you could add it to the controller specific folder (*Views/Todo/Components/PriorityList/Default.cshtml*)

如果视图组件是特定控制器的，你可以添加到特定控制器文件夹（*Views/Todo/Components/PriorityList/Default.cshtml*）。

.. 4. Add a ``div`` containing a call to the priority list component to the bottom of the *Views/Todo/index.cshtml* file:

4. 在 *Views/Todo/index.cshtml* 文件底部添加一个 ``div`` 包含调用 PriorityList 组件：

.. literalinclude:: view-components/sample/ViewCompFinal/Views/Todo/IndexFirst.cshtml
  :language: html
  :lines: 34-

.. The markup ``@Component.InvokeAsync`` shows the syntax for calling view components. The first argument is the name of the component we want to invoke or call. Subsequent parameters are passed to the component. ``InvokeAsync`` can take an arbitrary number of arguments. 

标记 ``@Component.InvokeAsync`` 展示了调用视图组件的语法。第一个参数是我们想要调用的组件名。随后的参数传递给组件。``InvokeAsync`` 可以接受任意数量的参数。

.. The following image shows the priority items: 

下面的图片显示了 Priority 项：

.. image:: view-components/_static/pi.png

.. You can also call the view component directly from the controller:

你也可以从控制器直接调用视图组件：

.. literalinclude:: view-components/sample/ViewCompFinal/Controllers/ToDoController.cs
  :language: c#
  :lines: 23-26
  :dedent: 6

.. Specifying a view name
^^^^^^^^^^^^^^^^^^^^^^^^^

指定视图名
^^^^^^^^^^^^^^^^^^^^^^^^^

.. A complex view component might need to specify a non-default view under some conditions. The following code shows how to specify the "PVC" view  from the ``InvokeAsync`` method. Update the ``InvokeAsync`` method in the ``PriorityListViewComponent`` class.

一个复杂的视图组件在某些条件下可能需要指定非默认的视图。下面的代码展示如何从 ``InvokeAsync`` 方法中指定 "PVC" 视图。更新 ``PriorityListViewComponent`` 类中的 ``InvokeAsync`` 方法。

.. literalinclude:: view-components/sample/ViewCompFinal/ViewComponents/PriorityListViewComponentFinal.cs
  :language: c#  
  :lines: 28-39
  :dedent: 8
  :emphasize-lines: 4-9

.. Copy the *Views/Shared/Components/PriorityList/Default.cshtml* file to a view named *Views/Shared/Components/PriorityList/PVC.cshtml*. Add a heading to indicate the PVC view is being used. 

复制 *Views/Shared/Components/PriorityList/Default.cshtml* 文件到一个视图中并命名为 *Views/Shared/Components/PriorityList/PVC.cshtml*。添加一个标题到 PVC 视图来表明正在使用此视图。

.. literalinclude:: view-components/sample/ViewCompFinal/Views/Shared/Components/PriorityList/PVC.cshtml
  :language: html  
  :emphasize-lines: 3
  
.. Update *Views/TodoList/Index.cshtml*

更新 *Views/TodoList/Index.cshtml*

.. literalinclude:: view-components/sample/ViewCompFinal/Views/Todo/IndexFinal.cshtml
  :language: html  
  :lines: 32-

.. Run the app and verify PVC view.

运行应用程序并验证 PVC 视图。

.. image:: view-components/_static/pvc.png

.. If the PVC view is not rendered, verify you are calling the view component with a priority of 4 or higher.

如果 PVC 视图没有渲染，请验证你是否使用 4 或者更高 priority 参数来调用视图组件。

.. Examine the view path
^^^^^^^^^^^^^^^^^^^^^^^^^

.. #. Change the priority parameter to three or less so the priority view is not returned. 
.. #. Temporarily rename the *Views/Todo/Components/PriorityList/Default.cshtml* to *Temp.cshtml*.
.. #. Test the app, you'll get the following error::

..    An unhandled exception occurred while processing the request.

..    InvalidOperationException: The view 'Components/PriorityList/Default' 
       was not found. The following locations were searched: 
       /Views/ToDo/Components/PriorityList/Default.cshtml 
       /Views/Shared/Components/PriorityList/Default.cshtml.
    Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful()

.. 4. Copy  *Views/Shared/Components/PriorityList/Default.cshtml to *Views/Todo/Components/PriorityList/Default.cshtml*.
.. #. Add some markup to the *Todo* view component view to indicate the view is from the *Todo* folder.
.. #. Test the **non-shared** component view.
    
.. .. image:: view-components/_static/shared.png

检查视图路径
^^^^^^^^^^^^^^^^^^^^^^^^^

#. 改变 priority 参数到 3 或者更低，使得不返回优先级视图。 
#. 暂时重命名 *Views/Todo/Components/PriorityList/Default.cshtml* 为 *Temp.cshtml*。
#. 测试应用程序，你将得到以下错误：

    An unhandled exception occurred while processing the request.

    InvalidOperationException: The view 'Components/PriorityList/Default' 
       was not found. The following locations were searched: 
       /Views/ToDo/Components/PriorityList/Default.cshtml 
       /Views/Shared/Components/PriorityList/Default.cshtml.
    Microsoft.AspNetCore.Mvc.ViewEngines.ViewEngineResult.EnsureSuccessful()

4. 复制 *Views/Shared/Components/PriorityList/Default.cshtml* 到 *Views/Todo/Components/PriorityList/Default.cshtml*。
#. 添加一些标记到 *Todo* 视图组件视图来表明视图是来自 *Todo* 文件夹。
#. 测试 **非共享** 组件视图。
    
.. image:: view-components/_static/shared.png

.. Avoiding magic strings
^^^^^^^^^^^^^^^^^^^^^^^^^^

避免魔法字符串
^^^^^^^^^^^^^^^^^^^^^^^^^^

.. If you want compile time safety you can replace the hard coded view component name with the class name. Create the view component without the "ViewComponent" suffix:

如果你想编译时安全你可以用类名替换硬编码视图组件名。创建视图组件不以 "ViewComponent" 作为后缀：

.. literalinclude:: view-components/sample/ViewCompFinal/ViewComponents/PriorityList.cs
  :language: c#  
  :lines: 4-34
  :emphasize-lines: 10,14

.. Add a ``using`` statement to your Razor view file and use the ``nameof`` operator:

添加一个 ``using`` 语句到你的 Razor 视图文件并使用 ``nameof`` 操作符：

.. literalinclude:: view-components/sample/ViewCompFinal/Views/Todo/IndexNameof.cshtml
  :language: html  
  :lines: 1-6, 33-


.. Additional Resources
----------------------

附加的资源
----------------------

- :doc:`dependency-injection`
- ViewComponent_

.. _ViewComponent: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponent/index.html
.. _ViewComponentAttribute.Name: https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/ViewComponentAttribute/index.html#prop-Microsoft.AspNetCore.Mvc.ViewComponentAttribute.Name

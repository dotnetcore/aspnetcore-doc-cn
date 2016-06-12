布局视图
========

Layout
=======

作者： `Steve Smith`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： 

Views frequently share visual and programmatic elements. In this article, you'll learn how to use common layouts, share directives, and run common code before rendering views in your ASP.NET app.

视图(View)经常共享视觉元素和编程元素。在本篇文章中，你将学习如何在你的 ASP.NET 应用程序中使用通用布局视图、共享指令以及在渲染视图前运行通用代码。

.. contents:: Sections
  :local:
  :depth: 1


What is a Layout
----------------

什么是布局视图
----------------

Most web apps have a common layout that provides the user with a consistent experience as they navigate from page to page. The layout typically includes common user interface elements such as the app header, navigation or menu elements, and footer. 

大部分 Web 应用程序在用户切换页面时，使用通用布局提供了一致的用户体验。通用布局通常包含页眉、导航栏（或菜单）以及页脚等通用 UI 元素。

.. image:: layout/_static/page-layout.png

Common HTML structures such as scripts and stylesheets are also frequently used by many pages within an app. All of these shared elements may be defined in a *layout* file, which can then be referenced by any view used within the app. Layouts reduce duplicate code in views, helping them follow the `Don't Repeat Yourself (DRY) principle <http://deviq.com/don-t-repeat-yourself/>`_.

在一个应用程序中，诸如脚本(scripts)和样式表(stylesheets)这样的通用 HTML 结构也频繁的被许多页面使用。所有的这些共享元素可以在 *layout* 文件中定义，这样应用程序中的任何视图都可以使用它们。布局视图减少了视图中的重复代码，帮助我们遵循 `Don't Repeat Yourself (DRY) 原则 <http://deviq.com/don-t-repeat-yourself/>`_ 。

By convention, the default layout for an ASP.NET app is named ``_Layout.cshtml``. The Visual Studio ASP.NET MVC project template includes this layout file in the ``Views/Shared`` folder:

按照惯例，ASP.NET 应用程序的默认布局文件命名为 ``_Layout.cshtml``。Visual Studio ASP.NET MVC 项目模板包含这个布局文件，位置在 ``Views/Shared`` 文件夹：

.. image:: layout/_static/web-project-views.png

This layout defines a top level template for views in the app. Apps do not require a layout, and apps can define more than one layout, with different views specifying different layouts.

An example ``_Layout.cshtml``:

这个布局为应用程序定义了一个高级视图模板。布局对应用程序来说不是必须的，应用程序也可以定义多个模板供不同的视图使用。

一个例子 ``_Layout.cshtml`` ：

.. literalinclude:: ../../../common/samples/WebApplication1/src/WebApplication1/Views/Shared/_Layout.cshtml
  :language: html
  :emphasize-lines: 42,66

Specifying a Layout
-------------------

指定布局
--------

Razor views have a ``Layout`` property. Individual views specify a layout by defining this property:

Razor 视图拥有一个 ``Layout`` 属性。各个视图可以通过定义这个属性来指定布局：

.. literalinclude:: ../../../common/samples/WebApplication1/src/WebApplication1/Views/_ViewStart.cshtml
  :language: html
  :emphasize-lines: 2

The layout specified can use a full path (example: ``/Views/Shared/_Layout.cshtml``) or a partial name (example: ``_Layout``). When a partial name is provided, the Razor view engine will search for the layout file using its standard discovery process. The controller-associated folder is searched first, followed by the ``Shared`` folder. This discovery process is identical to the one used to discover :doc:`partial views <partial>`.

指定布局时可以用完整路径（例如： ``/Views/Shared/_Layout.cshtml`` ）或者部分名称（例如： ``_Layout`` ）。当使用部分名称时，Razor 视图引擎将使用它的标准发现流程搜索布局文件。首先 Controller 相关的文件夹，其次是 ``Shared`` 文件夹。这个发现流程和 :doc:`部分视图 <partial>` 的是完全相同的。

Every layout must call ``RenderBody``. Wherever the call to ``RenderBody`` is placed, the contents of the view will be rendered.

每个布局视图必须调用 ``RenderBody`` 方法。在哪里调用 ``RenderBody`` ，视图内容就会在那里被渲染。 

Sections
^^^^^^^^

A layout can optionally reference one or more *sections*, by calling ``RenderSection``. Sections provide a way to organize where certain page elements should be placed. Each call to ``RenderSection`` can specify whether that section is required or optional. Individual views specify the content to be rendered within a section using the ``@section`` Razor syntax. If a view defines a section, it must be rendered (or an error will occur).

布局视图可以通过调用 ``RenderSection`` 方法来引用一个或多个 *sections* （布局视图不是必须引用 Section）。Section 提供了组织某些页面元素放置的方法。每一个 ``RenderSection`` 调用都可以指定 Secton 是必须还是可选的。个别视图使用 ``@section`` 指定被渲染的内容。如果一个视图定义了一个 Section，它必须被渲染（否则将会发生错误）。（译者注：这里的必须被渲染指必须通过 ``RenderSection`` 方法调用。）

An example ``@section`` definition in a view:

一个在视图中定义 ``@section`` 的例子：

.. code-block:: html

	@section Scripts {
	  <script type="text/javascript" src="/scripts/main.js"></script>
	}

In the code above, validation scripts are added to the ``scripts`` section on a view that includes a form. Other views in the same application might not require any additional scripts, and so wouldn't need to define a scripts section.

在上面的代码中，将验证脚本添加到一个包含 Form 表单的视图中的 ``scripts`` Section 中。其它在同一个应用程序的视图也许不需要任何额外的脚本，所以不需要定义 ``scripts`` Section。

Sections only flow from views. They cannot be referenced from partials, view components, or other parts of the view system.

Section 只能在视图之间相互调用，而不能在局部视图，视图组件，或视图系统的其他部分中引用。

.. _viewimports:

Importing Shared Directives
---------------------------

导入共享指令
----------------

Views can use Razor directives to do many things, such as specifying namespaces or performing :doc:`dependency injection <dependency-injection>`. Directives used by many views may be specified in a ``_ViewImports.cshtml`` file.  The ``_ViewImports`` file supports the following directives:

视图可以使用 Razor 指令做许多事，比如指定命名空间或者进行 :doc:`依赖注入 <dependency-injection>` 。指令可以指定在一个 ``_ViewImports.cshtml`` 文件中并被多个视图使用。 ``_ViewImports`` 文件支持以下指令：

- addTagHelper
- removeTagHelper
- tagHelperPrefix
- using
- model
- inherits
- inject

The file does not support other Razor features, such as functions and section definitions.

这个文件不支持其他 Razor 特性，比如 functions 和 section 的定义等等。

A sample ``_ViewImports.cshtml`` file:

一个 ``_ViewImports.cshtml`` 文件的例子：

.. literalinclude:: ../../../common/samples/WebApplication1/src/WebApplication1/Views/_ViewImports.cshtml
  :language: html

The ``_ViewImports.cshtml`` file for an ASP.NET Core MVC app is typically placed in the ``Views`` folder root. A ``_ViewImports.cshtml`` file can be placed within a controller-associated view folder, in which case it will only be applied to views within that folder. ``_ViewImports`` files are run first at the root level, and then for a ``_ViewImports`` file specified in the controller-associated folder, so settings specified at the root level may be overridden at the folder level.

在ASP.NET Core MVC 应用程序中， ``_ViewImports.cshtml`` 通常被放置在 ``Views`` 文件夹根目录下。在运行顺序上，首先运行在根目录下的 ``_ViewImports`` 文件，然后运行 Controller 相关文件夹下的 ``_ViewImports``文件，所以在根目录中 ``_ViewImports`` 文件里指定的设定可能会被覆盖掉。

For example, if a root level ``_ViewImports.cshtml`` file specifies ``@model`` and ``@addTagHelper``, and another ``_ViewImports.cshtml`` file in the controller-associated folder of the view specifies a different ``@model`` and adds another ``@addTagHelper``, the view will have access to both tag helpers and will use the latter ``@model``.

举个例子，如果根目录中 ``_ViewImports.cshtml`` 文件指定了 ``@model`` 和 ``@addTagHelper``，另外一个 Controller 相关文件夹下的 ``_ViewImports.cshtml`` 文件指定一个不同的 ``@model`` 并添加另外一个 ``@addTagHelper`` ，视图将可访问两种 TagHelper 并使用后者指定的 ``@model`` 。

If multiple ``_ViewImports.cshtml`` files are run for a view, combined behavior of the directives included in the ``ViewImports.cshtml`` files will be as follows:

- addTagHelper, remoteTagHelper: all run, in order
- tagHelperPrefix: the closest one to the view overrides any others
- model: the closest one to the view overrides any others
- inherits: the closest one to the view overrides any others
- using: all are included; duplicates are ignored
- inject: for each property, the closest one to the view overrides any others with the same property name

如果一个视图中有多个 ``_ViewImports.cshtml`` 文件被运行，多个 ``ViewImports.cshtml`` 文件中指令的组合行为如下：

- addTagHelper、remoteTagHelper：按照顺序全部运行
- tagHelperPrefix：离视图最近的一个覆盖其他的
- model：离视图最近的一个覆盖其他的
- inherits：离视图最近的一个覆盖其他的
- using：全部包含; 重复的忽略
- inject：对每一个属性而言（通过属性名区分），离视图最近的一个覆盖其他的

.. _viewstart:

Running Code Before Each View
-----------------------------

在视图之前运行代码
--------------------

If you have code you need to run before every view, this should be placed in the ``_ViewStart.cshtml`` file. By convention, the ``_ViewStart.cshtml`` file is located in the root of the ``Views`` folder. The statements listed in ``_ViewStart.cshtml`` are run before every full view (not layouts, and not partial views). Like :ref:`ViewImports.cshtml <viewimports>`, ``_ViewStart.cshtml`` is hierarchical. If a ``_ViewStart.cshtml`` file is defined in the controller-associated view folder, it will be run after the one defined in the root of the ``Views`` folder 	.

A sample ``_ViewStart.cshtml`` file:

如果你有代码需要在任何视图之前运行，这些代码应该放在 ``_ViewStart.cshtml`` 文件中。按照约定， ``_ViewStart.cshtml`` 文件位于 ``Views`` 文件夹的根目录。 ``_ViewStart.cshtml`` 中列出的语句会在所有完整的视图（不包含布局视图和部分视图）之前运行。比如 :ref:`ViewImports.cshtml <viewimports>`，``_ViewStart.cshtml`` 也有优先级分层。如果一个 ``_ViewStart.cshtml`` 文件定义在 Controller 相关的视图文件夹内，它将比 ``Views`` 文件夹根目录下的 ``_ViewStart.cshtml`` 文件更晚运行（如果根目录下有这个文件的话）。

一个 ``_ViewStart.cshtml`` 文件例子：

.. literalinclude::  /../common/samples/WebApplication1/src/WebApplication1/Views/_ViewStart.cshtml
  :language: html

The file above specifies that all views will use the ``_Layout.cshtml`` layout.

上面的文件指定了所有的视图将使用 ``_Layout.cshtml`` 布局视图。

.. note:: Neither ``_ViewStart.cshtml`` nor ``_ViewImports.cshtml`` are placed in the ``/Views/Shared`` folder. The app-level versions of these files should be placed directly in the ``/Views`` folder.

.. note:: 无论 ``_ViewStart.cshtml`` 还是 ``_ViewImports.cshtml`` 都不能放在 ``/Views/Shared`` 文件夹下。这些文件是应用程序级别的，应该直接放在 ``/Views`` 文件夹下。

Introduction to Tag Helpers
=========================================

Tag Helpers 介绍
==================
By `Rick Anderson`_ 

翻译： `刘浩杨 <http://github.com/liuhaoyang>`_

.. _issue: https://github.com/aspnet/Docs/issues/125

    - `What are Tag Helpers?`_
    - `What Tag Helpers provide`_ 
    - `Managing Tag Helper scope`_
    - `IntelliSense support for Tag Helpers`_
    - `Tag Helpers compared to HTML Helpers`_ 
    - `Tag Helpers compared to Web Server Controls`_
    - `Customizing the Tag Helper element font`_
    - `Additional Resources`_

What are Tag Helpers?
------------------------------------
什么是 Tag Helpers ？
-----------------------

Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. For example, the built-in `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/ImageTagHelper/index.html>`__ can append a version number to the image name. Whenever the image changes, the server generates a new unique version for the image, so clients are guaranteed to get the current image (instead of a stale cached image). There are many built-in Tag Helpers for common tasks - such as creating forms, links, loading assets and more - and even more available in public GitHub repositories and as NuGet packages.
Tag Helpers are authored in C#, and they target HTML elements based on element name, attribute name, or parent tag. For example, the built-in `LabelTagHelper  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/LabelTagHelper/index.html>`__ can target the HTML ``<label>`` element when the ``LabelTagHelper`` attributes are applied. 
If you're familiar with `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__, Tag Helpers reduce the explicit transitions between HTML and C# in Razor views. `Tag Helpers compared to HTML Helpers`_ explains the differences in more detail.

在 Razor 文件中，Tag Helpers 能够让服务端代码参与创建和渲染 HTML 元素。例如，内置的 `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ImageTagHelper/index.html>`__ 能够在图像名称后面追加版本号。每当图像变化时，服务器为图像生成一个新的唯一的版本，所以保证客户端得到当前图像（而不是旧的缓存图像）。对于常见任务有许多内置的 Tag Helpers - 比如创建表单，链接，加载资产以及更多 - 在公共的 Github 存储库和以 NuGet 包的方式更容易获得。在 C# 里编写 Tag Helpers，它们的目标是基于元素名称，特性名称或者父标签的 HTML 元素。例如，当 ``LabelTagHelper`` 特性被应用时，内置的  `LabelTagHelper  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/LabelTagHelper/index.html>`__ 能够作用于 HTML ``<label>`` 元素。如果你熟悉 `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__，Tag Helpers 在 Razor 视图中减少 HTML 和 C# 之间的显示转换。 `Tag Helpers compared to HTML Helpers`_ 解释了更详细的差异。

What Tag Helpers provide
------------------------------------
Tag Helpers 提供了什么
-------------------------

**An HTML-friendly development experience**
 For the most part, Razor markup using Tag Helpers looks like standard HTML. Front-end designers conversant with HTML/CSS/JavaScript can edit Razor without learning C# Razor syntax.

**一种 HTML-friendly 的开发体验**
 在大多数情况下，Razor 标记使用 Tag Helpers 看起来更像标准的 HTML。熟悉 HTML/CSS/JavaScript 的前端设计师在没有学习 C# Razor 语法的情况下能够编辑 Razor 。

**A rich IntelliSense environment for creating HTML and Razor markup**
 This is in sharp contrast to HTML Helpers, the previous approach to server-side creation of markup in Razor views. `Tag Helpers compared to HTML Helpers`_ explains the differences in more detail. `IntelliSense support for Tag Helpers`_ explains the IntelliSense environment. Even developers experienced with Razor C# syntax are more productive using Tag Helpers than writing C# Razor markup. 

**一个丰富的智能感知环境来创建 HTML 和 Razor 标记**
 这和 HTML Helpers 有明显的对比，前一种方法在服务端创建 Razor 视图中的标记。 `Tag Helpers compared to HTML Helpers`_ 讲解了更详细的差异。`IntelliSense support for Tag Helpers`_ 讲解了智能感知环境。
 
**A way to make you more productive and able to produce more robust, reliable, and maintainable code using information only available on the server**
 For example, historically the mantra on updating images was to change the name of the image when you change the image. Images should be aggressively cached for performance reasons, and unless you change the name of an image, you risk clients getting a stale copy. Historically, after an image was edited, the name had to be changed and each reference to the image in the web app needed to be updated. Not only is this very labor intensive, it's also error prone (you could miss a reference, accidentally enter the wrong string, etc.) The built-in `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/ImageTagHelper/index.html>`__ can do this for you automatically. The ``ImageTagHelper`` can append a version number to the image name, so whenever the image changes, the server automatically generates a new unique version for the image. Clients are guaranteed to get the current image. This robustness and labor savings comes essentially free by using the ``ImageTagHelper``.  

**一种让你使用仅在服务器上可用的信息来更有效并且能够生成更强大，可靠和可维护代码的方式。**
 例如，在之前当你更改图像的时候，更新图像的原则是更改图像的名称。出于性能原因应该主动缓存图像，除非你改变图像的名称，你的客户端有得到一份过期的副本的风险。在之前，一个图像被编辑后，它的名称必须改变并且在网络应用程序中图像的每一个引用都需要更新。这不仅是体力活，同时也容易出错（你可能漏掉一个引用，意外的输入错误字符串等）。内置的 `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ImageTagHelper/index.html>`__ 能够自动为你做这件事情。``ImageTagHelper`` 能够在图像名称后追加一个版本号，每当图像变化时，服务器为图像自动生成一个新的唯一的版本。客户端被保证得到当前的图像。通过使用 ``ImageTagHelper`` 这种健壮性和节省劳力基本上是无偿的。

Most of the built-in Tag Helpers target existing HTML elements and provide server-side attributes for the element. For example, the ``<input>`` element used in many of the views in the *Views/Account* folder contains the ``asp-for`` attribute, which extracts the name of the specified model property into the rendered HTML. The following Razor markup:

大多数内置的 Tag Helpers 指向现有的 HTML 元素并且为这些元素提供服务端特性。例如：在 *Views/Account* 文件夹下的许多视图中使用的 ``<input>`` 元素包含了 ``asp-for`` 属性，提取指定模型的属性名称到呈现的 HTML 中。Razor 标记如下：

.. code-block:: html

   <label asp-for="Email"></label>

Generates the following HTML:

生成以下的 HTML :

.. code-block:: html

   <label for="Email">Email</label>
   
The ``asp-for`` attribute is made available by the ``For`` property in the ``LabelTagHelper``. See :doc:`authoring` for more information.

``asp-for`` 特性由在 ``LabelTagHelper`` 中的 ``For`` 属性提供。查看 :doc:`authoring` 获取更多信息。

Managing Tag Helper scope
-----------------------------
管理 Tag Helper 范围
----------------------

Tag Helpers scope is controlled by a combination of ``@addTagHelper``, ``@removeTagHelper``, and the "!" opt-out character.
Tag Helpers 的范围由 ``@addTagHelper`` 和  ``@removeTagHelper`` 进行控制，并且 "!" 为退出字符。

.. _add-helper-label:

``@addTagHelper`` makes Tag Helpers available
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

``@addTagHelper`` 使 Tag Helpers 可用
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

If you create a new ASP.NET Core web app named *AuthoringTagHelpers* (with no authentication), the following *Views/_ViewImports.cshtml* file will be added to your project:

如果你创建一个新的 ASP.NET Core web 应用命名为 *AuthoringTagHelpers* （无身份认证），下面的 *Views/_ViewImports.cshtml* 文件将被添加到你的项目：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImportsCopy.cshtml
   :language: html
   :emphasize-lines: 2
   :lines: 1-2

The ``@addTagHelper`` directive makes Tag Helpers available to the view. In this case, the view file is *Views/_ViewImports.cshtml*, which by default is inherited by all view files in the *Views* folder and sub-directories; making Tag Helpers available. The code above uses the wildcard syntax ("*") to specify that all Tag Helpers in the specified assembly (*Microsoft.AspNetCore.Mvc.TagHelpers*) will be available to every view file in the *Views* directory or sub-directory. The first parameter after ``@addTagHelper`` specifies the Tag Helpers to load (we are using "\*" for all Tag Helpers), and the second parameter "Microsoft.AspNetCore.Mvc.TagHelpers" specifies the assembly containing the Tag Helpers. *Microsoft.AspNetCore.Mvc.TagHelpers* is the assembly for the built-in ASP.NET Core Tag Helpers.

``@addTagHelper`` 指示使 Tag Helpers 在视图中可用。在这种情况下，视图文件是 *Views/_ViewImports.cshtml* ，默认继承所有的视图文件在 *Views* 和子目录；使 Tag Helpers 可用。上面的代码使用通配符 ("*") 来指定在特定程序集（*Microsoft.AspNetCore.Mvc.TagHelpers*）中的所有的 Tag Helpers 在每一个 *Views* 目录和子目录中的视图文件中可用。 ``@addTagHelper`` 后面的第一个参数指定要加载的 Tag Helpers （我们使用 “*” 对于所有 Tag Helpers），第二个参数 “Microsoft.AspNetCore.Mvc.TagHelpers” 指定包含 Tag Helpers 的程序集。 *Microsoft.AspNetCore.Mvc.TagHelpers* 是内置的 ASP.NET Core Tag Helpers 程序集。

To expose all of the Tag Helpers in this project (which creates an assembly named *AuthoringTagHelpers*), you would use the following:

暴露这个项目中的所有 Tag Helpers （创建一个名称为 *AuthoringTagHelpers* 的程序集），你可以像下面一样使用：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImportsCopy.cshtml
   :language: html
   :emphasize-lines: 3

If your project contains an ``EmailTagHelper`` with the default namespace (``AuthoringTagHelpers.TagHelpers.EmailTagHelper``), you can provide the fully qualified name (FQN) of the Tag Helper:

如果你的项目包含一个使用默认命名空间(``AuthoringTagHelpers.TagHelpers.EmailTagHelper``)的 ``EmailTagHelper`` ，你可以对 Tag Helper 提供完全限定名（FQN）：

.. FQN syntax

.. code-block:: html
   :emphasize-lines: 3
   
    @using AuthoringTagHelpers
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    @addTagHelper "AuthoringTagHelpers.TagHelpers.EmailTagHelper, AuthoringTagHelpers"

To add a Tag Helper to a view using an FQN, you first add the FQN (``AuthoringTagHelpers.TagHelpers.EmailTagHelper``), and then the assembly name (*AuthoringTagHelpers*). Most developers prefer to use the  "\*" wildcard syntax. The wildcard syntax allows you to insert the wildcard character "\*" as the suffix in an FQN. For example, any of the following directives will bring in the ``EmailTagHelper``:

使用 FQN 在视图中添加一个 Tag Helper ，你首先添加 FQN （``AuthoringTagHelpers.TagHelpers.EmailTagHelper``），然后是程序集名称（*AuthoringTagHelpers*）。大多数开发者喜欢使用 "\*" 通配符语法。通配符语法允许你在 FQN 中插入通配符 "\*" 作为后缀。例如，下列指令将在 ``EmailTagHelper`` 中引入：

.. code-block:: c#

    @addTagHelper "AuthoringTagHelpers.TagHelpers.E*, AuthoringTagHelpers"
    @addTagHelper "AuthoringTagHelpers.TagHelpers.Email*, AuthoringTagHelpers"

As mentioned previously, adding the ``@addTagHelper`` directive to the *Views/_ViewImports.cshtml* file makes the Tag Helper available to all view files in the *Views* directory and sub-directories. You can use the ``@addTagHelper`` directive in specific view files if you want to opt-in to exposing the Tag Helper to only those views.

像前面说的，添加 ``@addTagHelper`` 指令到 *Views/_ViewImports.cshtml* 文件使 Tag Helper 可用于 *Views* 目录和子目录中的所有视图文件。如果你想选择在特定的视图文件中暴露 Tag Helper ，你可以在这些视图文件中使用 ``@addTagHelper`` 指令。

.. _remove-razor-directives-label:

``@removeTagHelper`` removes Tag Helpers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
``@removeTagHelper`` 删除 Tag Helpers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``@removeTagHelper`` has the same two parameters as ``@addTagHelper``, and it removes a Tag Helper that was previously added. For example, ``@removeTagHelper`` applied to a specific view removes the specified Tag Helper from the view. Using ``@removeTagHelper`` in a *Views/Folder/_ViewImports.cshtml* file removes the specified Tag Helper from all of the views in *Folder*.

``@removeTagHelper`` 具有和 ``@addTagHelper`` 相同的两个参数，并且它删除之前添加的一个 Tag Helper 。例如： ``@removeTagHelper`` 应用于从特定的视图中移除特定的 Tag Helper 。在 *Views/Folder/_ViewImports.cshtml* 中使用 ``@removeTagHelper`` 删除 *Folder* 中所有视图中特定的 Tag Helper 。


Controlling Tag Helper scope with the *_ViewImports.cshtml* file
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
在 *_ViewImports.cshtml* 中控制 Tag Helper 的范围
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can add a *_ViewImports.cshtml* to any view folder, and the view engine adds the directives from that *_ViewImports.cshtml* file to those contained in the *Views/_ViewImports.cshtml* file. If you added an empty *Views/Home/_ViewImports.cshtml* file for the *Home* views, there would be no change because the *_ViewImports.cshtml* file is additive. Any ``@addTagHelper`` directives you add to the *Views/Home/_ViewImports.cshtml* file (that are not in the default *Views/_ViewImports.cshtml* file) would expose those Tag Helpers to views only in the *Home* folder.

你可以在任何视图文件夹中添加一个 *_ViewImports.cshtml* ，并且视图引擎添加 *_ViewImports.cshtml* 文件中的指令到包含它们的 *Views/_ViewImports.cshtml* 文件中。如果你为 *Home* 视图添加一个空 *Views/Home/_ViewImports.cshtml* 文件，它们不会有任何变化因为 *_ViewImports.cshtml* 文件是附加的。你添加到 *Views/Home/_ViewImports.cshtml* 文件（不是默认的 *Views/_ViewImports.cshtml* 文件）的任何 ``@addTagHelper`` 指令将会只在 *Home* 文件夹中公开这些 Tag Helpers 。

Opting out of individual elements
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
选择退出个别元素
^^^^^^^^^^^^^^^^^^

You can disable a Tag Helper at the element level with the Tag Helper opt-out character ("!"). For example, ``Email`` validation is disabled in the ``<span>`` with the Tag Helper opt-out character:

你可以在元素级别禁用带有退出符（"!"）的标签助手。例如：在 ``<span>`` 中带有退出字符的 ``Email`` 验证被禁用：

.. code-block:: c#

    <!span asp-validation-for="Email" class="text-danger"></!span>

You must apply the Tag Helper opt-out character to the opening and closing tag. (The Visual Studio editor automatically adds the opt-out character to the closing tag when you add one to the opening tag). After you add the opt-out character, the element and Tag Helper attributes are no longer displayed in a distinctive font.

你必须使用 Tag Helper 退出字符来打开和关闭标签。（当你添加一个打开标签时， Visual Studio 编辑器自动添加退出字符来关闭标签）。在你添加退出字符之后，元素和 Tag Helper 特性将不再显示在一个单独的字体中。

.. _prefix-razor-directives-label:

Using ``@tagHelperPrefix`` to make Tag Helper usage explicit
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
使用 ``@tagHelperPrefix`` 让 Tag Helper 用法明确
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``@tagHelperPrefix`` directive allows you to specify a tag prefix string to enable Tag Helper support and to make Tag Helper usage explicit. In the code image below, the Tag Helper prefix is set to ``th:``, so only those elements using the prefix ``th:`` support Tag Helpers (Tag Helper-enabled elements have a distinctive font). The ``<label>`` and ``<input>`` elements have the Tag Helper prefix and are Tag Helper-enabled, while the ``<span>`` element does not.

``@tagHelperPrefix`` 指令允许你指定一个标签前缀来启用 Tag Helper 支持和使 Tag Helper 用法明确。在下面的代码图片中， Tag Helper 前缀设置为``th:``，因此只有那些使用前缀 ``th:`` 的元素支持 Tag Helpers （Tag Helper可用的元素有独特的字体）。 ``<label>`` 和 ``<input>`` 元素使用 Tag Helper 前缀并且 Tag Helper 可用， ``<span>`` 元素不能使用 Tag Helper。

.. image:: intro/_static/thp.png 

.. comment for next version:Note: Quotes are optional with the ``@tagHelperPrefix`` directive. The following two directives are equivalent: 
.. comment code-block:: html
   @tagHelperPrefix th:
   @tagHelperPrefix "th:"

The same hierarchy rules that apply to ``@addTagHelper`` also apply to ``@tagHelperPrefix``.

同一层次的规则适用于 ``@addTagHelper`` 也适用于 ``@tagHelperPrefix``。

IntelliSense support for Tag Helpers
----------------------------------------
Tag Helpers 智能感知支持
---------------------------

When you create a new ASP.NET web app in Visual Studio, it adds "Microsoft.AspNetCore.Razor.Tools" to the *project.json* file. This is the package that adds Tag Helper tooling. 

当你在 Visual Studio 中创建一个新的 ASP.NET web 应用，在 *project.json* 文件中添加 "Microsoft.AspNetCore.Razor.Tools" 。这是添加 Tag Helper 工具的包。

Consider writing an HTML ``<label>`` element. As soon as you enter ``<l`` in the Visual Studio editor, IntelliSense displays matching elements:

考虑写一个 HTML ``<label>`` 元素。在 Visual Studio 编辑器中你一进入 ``<l`` ，智能感知显示匹配的元素：

.. image:: intro/_static/label.png 

Not only do you get HTML help, but the icon (the "@" symbol with "<>" under it).

你得到的不仅仅是 HTML 的帮助，而且图标（ "@" 和 "<>" 符合在下面）。

.. image:: intro/_static/tagSym.png 

identifies the element as targeted by Tag Helpers. Pure HTML elements (such as the ``fieldset``) display the "<>"icon. 

有针对性的通过 Tag Helpers 标识元素。纯 HTML 元素（如 ``fieldse）显示 "<>" 图标。

A pure HTML ``<label>`` tag displays the HTML tag (with the default Visual Studio color theme) in a brown font, the attributes in red, and the attribute values in blue.

一个纯 HTML  ``<label>`` 标签使用棕色字体显示 HTML 标签（默认的 Visual Studio 颜色主题），特性使用红色，特性值使用蓝色。

.. image:: intro/_static/LableHtmlTag.png 

After you enter ``<label``, IntelliSense lists the available HTML/CSS attributes and the Tag Helper-targeted attributes:

在你输入 ``<label`` 后，智能感知列出可用的 HTML/CSS 特性和 Tag Helper 目标特性：

.. image:: intro/_static/labelattr.png

IntelliSense statement completion allows you to enter the tab key to complete the statement with the selected value:

智能感知声明完成允许你输入 tab 键来完成所选值的语句：

.. image:: intro/_static/stmtcomplete.png

As soon as a Tag Helper attribute is entered, the tag and attribute fonts change. Using the default Visual Studio "Blue" or "Light" color theme, the font is bold purple. If you're using the "Dark" theme the font is bold teal. The images in this document were taken using the default theme.

一旦你输入了一个 Tag Helper 特性，标签和属性的字体改变。使用 Visual Studio 默认的 "Blue" 或 "Light" 颜色主题，字体是醒目的紫色。如果你使用 "Dark" 主题，字体是醒目的蓝绿色。在这个文档中的图片使用的是默认的主题。

.. image:: intro/_static/labelaspfor2.png

You can enter the Visual Studio *CompleteWord* shortcut (Ctrl +spacebar is the `default <https://msdn.microsoft.com/en-us/library/da5kh0wa.aspx>`__) inside the double quotes (""), and you are now in C#, just like you would be in a C# class. IntelliSense displays all the methods and properties on the page model. The methods and properties are available because the property type is ``ModelExpression``. In the image below, I'm editing the ``Register`` view, so the ``RegisterViewModel`` is available.

你可以在双引号（""）里输入 Visual Studio 的 *CompleteWord* 快捷键（ `默认的 <https://msdn.microsoft.com/en-us/library/da5kh0wa.aspx>`__ 是 Ctrl +spacebar ），你现在在 C# 中，就像你在一个 C# 类中。智能感知显示页面模型的所有方法和属性。方法和属性能被使用因为属性类型是 ``ModelExpression``。在下面的图片中，我编辑 ``Register`` 视图，所以 ``RegisterViewModel`` 是可用的。

.. image:: intro/_static/intellemail.png

IntelliSense lists the properties and methods available to the model on the page. The rich IntelliSense environment helps you select the CSS class:

智能感知列出模型在页面上可用的属性和方法。丰富的智能感知环境帮助你选择 CSS class：

.. image:: intro/_static/iclass.png

.. image:: intro/_static/intel3.png

Tag Helpers compared to HTML Helpers
---------------------------------------------
Tag Helpers 和 HTML Helpers 比较
-------------------------------------

Tag Helpers attach to HTML elements in Razor views, while `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__ are invoked as methods interspersed with HTML in Razor views. Consider the following Razor markup, which creates an HTML label with the CSS class "caption":

HTML elements 在 Razor 视图中附加到 HTML 元素，而 `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__ 在 Razor 视图中作为穿插到 HTML 的方法被调用。考虑下面的 Razor 标记，它创建一个带有 "caption" CSS class的HTML label 标签：

.. code-block:: html

    @Html.Label("FirstName", "First Name:", new {@class="caption"})

The at (``@``) symbol tells Razor this is the start of code. The next two parameters ("FirstName" and "First Name:") are strings, so `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`_ can't help. The last argument:

at (``@``) 符号告诉 Razor 这是代码的开始。接下来的连个参数（"FirstName" 和 "First Name:"）是字符串，所以 `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`_ 不能帮助。最后的参数：

.. code-block:: html

  new {@class="caption"}
  
Is an anonymous object used to represent attributes. Because **class** is a reserved keyword in C#, you use the ``@`` symbol to force C# to interpret "@class=" as a symbol (property name). To a front-end designer (someone familiar with HTML/CSS/JavaScript and other client technologies but not familiar with C# and Razor), most of the line is foreign. The entire line must be authored with no help from IntelliSense.

是一个用于表示特性的匿名对象。因为 **class** 是一个 C# 的保留关键字，使用 ``@`` 符号强制 C# 解释 "@class=" 作为一个符号（属性名称）。一个前端设计师（一些人熟悉 HTML/CSS/JavaScript 和其他客户端技术但是不熟悉 C# 和 Razor），大部分的路线是不相关的。整行必须在没有智能感知的帮助下编写。
  
Using the ``LabelTagHelper``, the same markup can be written as:

使用 ``LabelTagHelper``，相同的标记可以被写为：

.. image:: intro/_static/label2.png 

With the Tag Helper version, as soon as you enter ``<l`` in the Visual Studio editor, IntelliSense displays matching elements:

使用 Tag Helper 的版本，一旦你在 Visual Studio 编辑器输入 ``<l``，智能感知显示匹配的元素：

.. image:: intro/_static/label.png 

IntelliSense helps you write the entire line. The ``LabelTagHelper`` also defaults to setting the content of the ``asp-for`` attribute value ("FirstName") to "First Name"; It converts camel-cased properties to a sentence composed of the property name with a space where each new upper-case letter occurs. In the following markup:

智能感知帮助你写整行代码。 ``LabelTagHelper`` 也默认设置 ``asp-for`` 特性值（"FirstName"）的内容到 "First Name"；它转换驼峰名称属性到每一个首字母大写的属性名称组成的句子。在下面的标记中：

.. image:: intro/_static/label2.png 

generates:

生成：
 
 .. code-block:: html

    <label class="caption" for="FirstName">First Name</label>

The  camel-cased to sentence-cased content is not used if you add content to the ``<label>``. For example:

如果你想添加内容到 ``<label>`` 中，camel-cased 到 sentence-cased 的内容不被使用。例如：

.. image:: intro/_static/1stName.png

generates:

生成：

 .. code-block:: html
 
  <label class="caption" for="FirstName">Name First</label>

The following code image shows the Form portion of the *Views/Account/Register.cshtml* Razor view generated from the legacy ASP.NET 4.5.x MVC template included with Visual Studio 2015.

下面的代码图片展示了从传统的包含在 Visual Studio 2015 中的 ASP.NET 4.5.x MVC 的模版生成的 *Views/Account/Register.cshtml* Razor 视图的表单部分。

.. image:: intro/_static/regCS.png 

The Visual Studio editor displays C# code with a grey background. For example, the ``AntiForgeryToken`` HTML Helper:

Visual Studio 编辑器使用灰色背景显示 C# 代码。例如， ``AntiForgeryToken`` HTML Helper：

.. code-block:: html

    @Html.AntiForgeryToken()
 
is displayed with a grey background. Most of the markup in the Register view is C#. Compare that to the equivalent approach using Tag Helpers:

被灰色背景显示。在 Register 视图中大部分标记是 C#。与使用 Tag Helpers 的等效方法比较：
 
.. image:: intro/_static/regTH.png 

The markup is much cleaner and easier to read, edit, and maintain than the HTML Helpers approach. The C# code is reduced to the minimum that the server needs to know about. The Visual Studio editor displays markup targeted by a Tag Helper in a distinctive font. 

和 HTML Helpers 方法相比，这些标记干净的多并且更容易阅读，编辑和维护。C# 代码减少到服务器需要知道的最小值。 Visual Studio 编辑器通过一个独特的字体显示标记的目标。

Consider the *Email* group:

.. literalinclude:: intro/sample/Register.cshtml
   :language: c#
   :lines: 12-18
   :dedent: 4

Each of the "asp-" attributes has a value of "Email", but "Email" is not a string. In this context, "Email" is the C# model expression property for the ``RegisterViewModel``. 

每一个 "asp-" 特性都有一个 "Email" 值，但是 "Email" 不是字符串。在这个上下文， "Email" 是对于 ``RegisterViewModel`` 的 C# 模型表达式属性。


The Visual Studio editor helps you write **all** of the markup in the Tag Helper approach of the register form, while Visual Studio provides no help for most of the code in the HTML Helpers approach. `IntelliSense support for Tag Helpers`_ goes into detail on working with Tag Helpers in the Visual Studio editor.

 Visual Studio 编辑器帮助你编写在 Tag Helper 注册表单中方法的 **all** 标记， Visual Studio 没有提供帮助给 HTML Helpers 方法的代码。 `IntelliSense support for Tag Helpers`_ 详细介绍 Tag Helpers 在 Visual Studio 编辑器中的工作。

Tag Helpers compared to Web Server Controls
-----------------------------------------------
Tag Helpers 和 Web 服务器控件比较
------------------------------------

- Tag Helpers don't own the element they're associated with; they simply participate in the rendering of the element and content. ASP.NET `Web Server controls <https://msdn.microsoft.com/en-us/library/7698y1f0.aspx>`__ are declared and invoked on a page.

-  Tag Helpers 不拥有它们所关联的元素，它们只简单的参与元素和内容的渲染。ASP.NET `Web Server controls <https://msdn.microsoft.com/en-us/library/7698y1f0.aspx>`__ 声明并且在页面上调用。

- `Web Server controls <https://msdn.microsoft.com/en-us/library/zsyt68f1.aspx>`__ have a non-trivial lifecycle that can make developing and debugging difficult. 

- Web Server controls allow you to add functionality to the client Document Object Model (DOM) elements by using a client control. Tag Helpers have no DOM. 

- Web 服务器控件允许你给通过客户端控制的客户端文档对象模型（ocument Object Model ，DOM）添加功能。Tag Helpers 不具有 DOM。

- Web Server controls include automatic browser detection. Tag Helpers have no knowledge of the browser.

- Web 服务器包含自动的浏览器检测。 Tag Helpers 不能识别浏览器。

- Multiple Tag Helpers can act on the same element (see `Avoiding Tag Helper conflicts <http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html#avoiding-tag-helper-conflicts>`__ ) while you typically can't compose Web Server controls.

- 多个 Tag Helpers 可以作用在相同的元素，而你通常不能构成 Web 服务器控件。

- Tag Helpers can modify the tag and content of HTML elements that they're scoped to, but don't directly modify anything else on a page. Web Server controls have a less specific scope and can perform actions that affect other parts of your page; enabling unintended side effects. 

- Tag Helpers 可以修改在它们范围内的标签和 HTML 元素的内容，但是不直接修改页面上的任何内容。 Web 服务器控件有一个较小的特定范围，可以执行影响页面其他部分的操作，从而造成非预期的副作用。

- Web Server controls use type converters to convert strings into objects. With Tag Helpers, you work natively in C#, so you don't need to do type conversion. 

- Web 服务器控件使用类型转换器（type converters）转换字符串到对象。使用 Tag Helpers，你本身就使用 C# 工作，所以你不需要做类型转换。

- Web Server controls use `System.ComponentModel <https://msdn.microsoft.com/en-us/library/system.componentmodel%28v=vs.110%29.aspx>`__ to implement the run-time and design-time behavior of components and controls. ``System.ComponentModel`` includes the base classes and interfaces for implementing attributes and type converters, binding to data sources, and licensing components. Contrast that to Tag Helpers, which typically derive from ``TagHelper``, and the ``TagHelper`` base class exposes only two methods, ``Process`` and ``ProcessAsync``.

- Web 服务器控件使用 `System.ComponentModel <https://msdn.microsoft.com/en-us/library/system.componentmodel%28v=vs.110%29.aspx>`__ 实现组件和控件的运行时和设计时行为。 ``System.ComponentModel`` 包括用于实现属性和类型转换器的基类和接口，绑定到数据源和授权组件。和 Tag Helpers 对比，通常来自 ``TagHelper``，并且 ``TagHelper`` 基类只公开两个方法 ``Process`` 和 ``ProcessAsync``。

Customizing the Tag Helper element font
---------------------------------------------
自定义 Tag Helper 元素字体
------------------------------

You can customize the font and colorization from **Tools** > **Options** > **Environment** > **Fonts and Colors**:

你可以在 **工具** > **选项** > **环境** > **字体和颜色** 中自定义字体和颜色：

.. image:: intro/_static/fontoptions2.png
   
Additional Resources
----------------------
附加资源
----------------------

- :doc:`/mvc/views/tag-helpers/authoring`
- :doc:`Working with Forms (Tag Helpers) </mvc/views/tag-helpers/index>`
- `TagHelperSamples on GitHub <https://github.com/dpaquette/TagHelperSamples>`__ contains Tag Helper samples for working with `Bootstrap <http://getbootstrap.com/>`__. 
- `TagHelperSamples on GitHub <https://github.com/dpaquette/TagHelperSamples>`__ 包含 Tag Helper 样本和 `Bootstrap <http://getbootstrap.com/>`__  工作. 

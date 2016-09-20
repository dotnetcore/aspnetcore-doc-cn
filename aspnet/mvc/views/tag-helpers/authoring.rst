Authoring Tag Helpers
===========================================

自定义标签辅助类（Tag Helpers）
===========================================

原文：`Authoring Tag Helpers <https://docs.asp.net/en/latest/mvc/views/tag-helpers/authoring.html>`_

作者：`Rick Anderson`_

翻译：`张海龙(jiechen) <http://github.com/ijiechen>`_

校对：`许登洋(Seay) <https://github.com/SeayXu>`_

.. contents:: Sections:
  :local:
  :depth: 1

`View or download sample code <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/tag-helpers/authoring/sample>`__

`示例代码查看与下载 <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/tag-helpers/authoring/sample>`__

Getting started with Tag Helpers
------------------------------------

从 Tag Helper 讲起
------------------------------------

This tutorial provides an introduction to programming Tag Helpers. :doc:`intro` describes the benefits that Tag Helpers provide.

本篇教程是对 Tag Helper 编程作以介绍。 :doc:`intro` 描述了 Tag Helper 的优势。

A tag helper is any class that implements the ``ITagHelper`` interface. However, when you author a tag helper, you generally derive from ``TagHelper``, doing so gives you access to the ``Process`` method. We will introduce the ``TagHelper`` methods and properties as we use them in this tutorial.

Tag Helper 是任何实现 ``ITagHelper`` 接口的类（Class）。然而，当你编写一个 Tag Helper，你通常是从 ``TagHelper`` 开始，这样做让你可以访问 ``Process`` 方法。我们将介绍 ``TagHelper`` 方法和属性，如同我们将在本教程使用它们的。

#. Create a new ASP.NET Core project called **AuthoringTagHelpers**. You won't need authentication for this project.

#. 创建一个命名为 **AuthoringTagHelpers** 的新 ASP.NET Core 项目。对该项目你不需要添加身份验证。

#. Create a folder to hold the Tag Helpers called *TagHelpers*. The *TagHelpers* folder is *not* required, but it is a reasonable convention. Now let's get started writing some simple tag helpers.

#. 创建一个用来放置 Tag Helper 的 *TagHelpers* 文件夹。 *TagHelpers* 文件夹是 *非* 必需的，但它是一个合理的惯例。现在让我们来开始编写一些简单的 Tag Helper。

Starting the email Tag Helper
--------------------------------

编写 email Tag Helper
-------------------------------

In this section we will write a tag helper that updates an email tag. For example:

这一节我们将写一个 Tag Helper ，用来更新 email 标签。例如：

.. code-block:: html

  <email>Support</email>

The server will use our email tag helper to convert that markup into the following:

服务端将使用我们的 email Tag Helper 来生成以下标记：

.. code-block:: html

  <a href="mailto:Support@contoso.com">Support@contoso.com</a>

That is, an anchor tag that makes this an email link. You might want to do this if you are writing a blog engine and need it to send email for marketing, support, and other contacts, all to the same domain.

也就是，一个锚标签转为了一个 email 链接。如果你在写一个博客引擎，并且需要它为市场、支持、其他联系人发送邮件到相同的域，你可能想要这样做。

#. Add the following ``EmailTagHelper`` class to the *TagHelpers* folder.

#. 添加下面的 ``EmailTagHelper`` 类到 *TagHelpers* 文件夹。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1EmailTagHelperCopy.cs
  :language: c#

**Notes:**

**说明：**

- Tag helpers use a naming convention that targets elements of the root class name (minus the *TagHelper* portion of the class name). In this example, the root name of **Email**\TagHelper is *email*, so the ``<email>`` tag will be targeted. This naming convention should work for most tag helpers, later on I'll show how to override it.

- Tag helper 使用以目标元素名作为根类名（除去类名中 *TagHelper* 部分）的命名约定。在这个例子中， **Email**\TagHelper 的根名称是 *email* ，因此 ``<email>`` 标签将是目标标签。这个命名约定适用于大多数 tag helper ，稍后我将展示如何对它重写。

- The ``EmailTagHelper`` class derives from ``TagHelper``. The ``TagHelper`` class provides the rich methods and properties we will examine in this tutorial.

-  ``EmailTagHelper`` 类派生自  ``TagHelper`` 。 ``TagHelper`` 类提供了我们即将在本文探究的丰富的方法和属性。

- The  overridden ``Process`` method controls what the tag helper does when executed. The ``TagHelper`` class also provides an asynchronous version (``ProcessAsync``) with the same parameters.

- 重写 ``Process`` 方法可以控制 Tag Helper 在执行过程中的行为。 ``TagHelper`` 类同样提供了相同参数的异步版本（``ProcessAsync``）。

- The context parameter to ``Process`` (and ``ProcessAsync``) contains information associated with the execution of the current HTML tag.

-  ``Process`` （或 ``ProcessAsync``）的上下文参数包含了与当前 HTML 标签执行的相关信息。

- The output parameter to ``Process`` (and ``ProcessAsync``) contains a stateful HTML element representative of the original source used to generate an HTML tag and content.

-  ``Process`` （或 ``ProcessAsync``）的输出参数包含了用来生成 HTML 标签和内容的源代码的静态 HTML 元素呈现。

- Our class name has a suffix of **TagHelper**, which is *not* required, but it's considered a best practice convention. You could declare the class as:

- 我们的类名后缀为 **TagHelper** ，是 *非* 必需的，但它被认为是最佳惯例约定。你可以定义类，如：

.. code-block:: c#

  public class Email : TagHelper

2. To make the ``EmailTagHelper`` class available to all our Razor views, we will add the ``addTagHelper`` directive to the *Views/_ViewImports.cshtml* file:

2. 为使 ``EmailTagHelper`` 类在我们所有 Razor 视图中可用，我们将把 ``addTagHelper`` 指令添加到 *Views/_ViewImports.cshtml* 文件：

.. wildcard syntax

.. 通配符

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImportsCopy.cshtml
  :language: html
  :emphasize-lines: 2,3

The code above uses the wildcard syntax to specify all the tag helpers in our assembly will be available. The first string after ``@addTagHelper`` specifies the tag helper to load (we are using "\*" for all tag helpers), and the second string "AuthoringTagHelpers" specifies the assembly the tag helper is in. Also, note that the second line brings in the ASP.NET Core MVC tag helpers using the wildcard syntax (those helpers are discussed in :doc:`intro`.) It's the ``@addTagHelper`` directive that makes the tag helper available to the Razor view. Alternatively, you can provide the fully qualified name (FQN) of a tag helper as shown below:

以上代码我们使用了通配符表明所有的 tag helper 都将在我们的程序集中启用。 ``@addTagHelper`` 之后的第一个字符串指明了要加载的 tag helper（我们使用 "\*" 代表所有 tag helper ），第二个字符串 "AuthoringTagHelpers" 指明了此 tag helper 所在的程序集。除此之外要注意的是，使用通配符的第二行，引入了 ASP.NET Core MVC 的 tag helper（这些辅助类在 :doc:`intro` 中已经讨论过）。是 ``@addTagHelper`` 命令使 tag helper 在 Razor 视图中起作用的。你还可以提供如下所示的 tag helper 的全名（FQN）：

.. FQN syntax

.. 完全限定名

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImports.cshtml
  :language: html
  :lines: 1-3
  :emphasize-lines: 3

To add a tag helper to a view using a FQN, you first add the FQN (``AuthoringTagHelpers.TagHelpers.EmailTagHelper``), and then the assembly name (*AuthoringTagHelpers*). Most developers will prefer to use the wildcard syntax. :doc:`intro` goes into detail on tag helper adding, removing, hierarchy, and wildcard syntax.

使用 FQN 给视图添加 tag helper，首先你要添加 FQN（``AuthoringTagHelpers.TagHelpers.EmailTagHelper``），然后是程序集名称（*AuthoringTagHelpers*）。多数开发人员喜欢用通配符。:doc:`intro` 详细了解 tag helper 的添加、删除、层次结构和通配符。

3. Update the markup in the *Views/Home/Contact.cshtml* file with these changes:

3. 更新 *Views/Home/Contact.cshtml* 文件中下列变化对应的标签。

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/Contact.cshtml
  :language: html
  :emphasize-lines: 15-16
  :lines: 1-17

4. Run the app and use your favorite browser to view the HTML source so you can verify that the email tags are replaced with anchor markup (For example, ``<a>Support</a>``). *Support* and *Marketing* are rendered as a links, but they don't have an ``href`` attribute to make them functional. We'll fix that in the next section.

4. 运行应用并使用你喜欢的浏览器查看 HTML 代码，你可以校验 email 标签都被替换成了链接标签（例如： ``<a>Support</a>``），*Support* 和 *Marketing* 被渲染为链接，但是，它们没有一个 ``href`` 属性能使其正常运行。我们将在下一节修复它。

**Note:** Like `HTML tags and attributes <http://www.w3.org/TR/html-markup/documents.html#case-insensitivity>`__, tags, class names and attributes in Razor, and C# are not case-sensitive.

**说明：** 比如 `HTML 标签与属性 <http://www.w3.org/TR/html-markup/documents.html#case-insensitivity>`__，Razor 与 C# 中的标签、类名及属性是不区分大小写的。

A working email Tag Helper
----------------------------------

一个可工作的 email Tag Helper
---------------------------

In this section, we will update the ``EmailTagHelper`` so that it will create a valid anchor tag for email. We'll update our tag helper to take information from a Razor view (in the form of a ``mail-to`` attribute) and use that in generating the anchor.

在这一节中，我们将更新 ``EmailTagHelper`` 使其可以为 email 创建一个有效的锚链接标签。我们将修改我们的 tag helper 使其在 Razor 视图中附加信息（以 ``mail-to`` 属性的形式）并使用它生成链接。

Update the ``EmailTagHelper`` class with the following:

参照以下代码更新 ``EmailTagHelper`` ：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/EmailTagHelperMailTo.cs
  :lines: 5-24
  :dedent: 4
  :language: c#

**Notes:**

**说明：**

-  Pascal-cased class and property names for tag helpers are translated into their `lower kebab case <http://stackoverflow.com/questions/11273282/whats-the-name-for-dash-separated-case/12273101#12273101>`__. Therefore, to use the ``MailTo`` attribute, you'll use ``<email mail-to="value"/>`` equivalent.

-  以 Pascal 形式命名 tag helper 的类名及属性名会被翻译成它们的 `小写 kebab 形式 <http://stackoverflow.com/questions/11273282/whats-the-name-for-dash-separated-case/12273101#12273101>`__。因此，你使用 ``MailTo`` 属性，与使用 ``<email mail-to="value"/>`` 是等价的。

- The last line sets the completed content for our minimally functional tag helper.

- 最后一行设置了我们 tag helper 完成的最小化功能的内容。

- The following line shows the syntax for adding attributes:

- 以下代码展示添加属性的语法：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/EmailTagHelperMailTo.cs
  :lines: 14-21
  :dedent: 8
  :emphasize-lines: 6
  :language: c#

That approach works for the attribute "href" as long as it doesn't currently exist in the attributes collection. You can also use the ``output.Attributes.Add`` method to add a tag helper attribute to the end of the collection of tag attributes.

虽然当前 “href” 在属性集中不存在，但离实现已经很接近了。你同样可以使用 ``output.Attributes.Add`` 方法在标签属性集的最后添加一个 tag helper 属性。

3. Update the markup in the *Views/Home/Contact.cshtml* file with these changes:

3. 依照以下改动修改 *Views/Home/Contact.cshtml* 文件标记：

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/ContactCopy.cshtml
  :language: html
  :emphasize-lines: 15-16
  :lines: 1-17

4. Run the app and verify that it generates the correct links.

4. 运行应用可验证它生成了正确的链接。

**Note:** If you were to write the email tag self-closing (``<email mail-to="Rick" />``), the final output would also be self-closing. To enable the ability to write the tag with only a start tag (``<email mail-to="Rick">``) you must decorate the class with the following:

**说明：** 如果你写的是自闭合的 email 标签（``<email mail-to="Rick" />``），最终的输出也将是自闭合的。为了启用写入仅是一个开始标签的功能（ ``<email mail-to="Rick">`` ），你必须如下设置类：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/EmailTagHelperMailVoid.cs
  :lines: 6
  :dedent: 4
  :language: c#

With a self-closing email tag helper, the output would be ``<a href="mailto:Rick@contoso.com" />``. Self-closing anchor tags are not valid HTML, so you wouldn't want to create one, but you might want to create a tag helper that is self-closing. Tag helpers set the type of the ``TagMode`` property after reading a tag.

使用自闭合的 email tag helper，输出将是 ``<a href="mailto:Rick@contoso.com" />``。自闭合链接标签是无效的 HTML，因此你不应该创建，但你可能想要创建自闭合的 tag helper。Tag helper 是在读取 tag 后设置 ``TagMode`` 属性的。

.. In this section we will update the ``EmailTagHelper`` so that it gets the target ``mail-to`` from the content. Need to revert the contact.cshtml file back.

.. 在这一节我们将更新 ``EmailTagHelper`` 使其从内容取到 ``mail-to`` 目标。需要撤销 contact.cshtml 文件更改。


An asynchronous email helper
_____________________________________

异步 email helper
----------------------

In this section we'll write an asynchronous email helper.

这一节我们将编写一个异步 email helper。

#. Replace the ``EmailTagHelper`` class with the following code:

#. 用以下代码替换 ``EmailTagHelper`` 类：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/EmailTagHelper.cs
  :lines: 6-17
  :dedent: 4
  :language: c#

**Notes:**

**说明：**

- This version uses the asynchronous ``ProcessAsync`` method. The asynchronous ``GetChildContentAsync`` returns a ``Task`` containing the ``TagHelperContent``.

- 这个版本使用异步的 ``ProcessAsync`` 方法。异步的 ``GetChildContentAsync`` 返回 ``Task`` ，其包含了 ``TagHelperContent``。

- We use the ``output`` parameter to get contents of the HTML element.

- 我们使用 ``output`` 参数取得 HTML 元素内容。

2. Make the following change to the *Views/Home/Contact.cshtml* file so the tag helper can get the target email.

2. 对 *Views/Home/Contact.cshtml* 文件做以下更改使 tag helper 取得目标 email。

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/Contact.cshtml
  :language: html
  :emphasize-lines: 15-16
  :lines: 1-17

3. Run the app and verify that it generates valid email links.

3. 运行应用并验证生成了有效的 email 链接。

The bold Tag Helper
---------------------------

粗体（Bold） Tag helper
---------------------------

#. Add the following ``BoldTagHelper`` class to the *TagHelpers* folder.

#. 添加以下 ``BoldTagHelper`` 类到 *TagHelpers* 文件夹。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/BoldTagHelper.cs
  :language: c#

**Notes:**

**说明：**

- The ``[HtmlTargetElement]`` attribute passes an attribute parameter that specifies that any HTML element that contains an HTML attribute named "bold" will match, and the ``Process`` override method in the class will run. In our sample, the ``Process``  method removes the "bold" attribute and surrounds the containing markup with ``<strong></strong>``.

-  ``[HtmlTargetElement]`` 属性传递一个属性参数，指定为任何 HTML 元素包含名为 “bold” 的 HTML 属性，并且类中重写的 ``Process`` 方法将被执行。在我们的示例中， ``Process`` 方法删除了 “bold” 属性且以 ``<strong></strong>`` 标记包含其中内容。

-  Because we don't want to replace the existing tag content, we must write the opening ``<strong>`` tag with the ``PreContent.SetHtmlContent`` method and the closing ``</strong>`` tag with the ``PostContent.SetHtmlContent`` method.

- 因为我们不想替换已有标签内容，我们必须用 ``PreContent.SetHtmlContent`` 方法写 ``<strong>`` 开始标签并用 ``PostContent.SetHtmlContent`` 方法写 ``</strong>`` 闭合标签。

2. Modify the *About.cshtml* view to contain a ``bold`` attribute value. The completed code is shown below.

2. 修改 *About.cshtml* 视图，添加一个 ``bold`` 属性值。完整代码如下。

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/AboutBoldOnly.cshtml
  :language: html
  :lines: 1-9
  :emphasize-lines: 7

3. Run the app. You can use your favorite browser to inspect the source and verify that the markup has changed as promised.

3. 运行程序。你可以用你喜欢的浏览器审查源代码，会发现标记已被如愿改变。

The ``[HtmlTargetElement]`` attribute above only targets HTML markup that provides an attribute name of "bold". The ``<bold>`` element was not modified by the tag helper.

上面 ``[HtmlTargetElement]`` 属性只指向具有属性名为 "bold" 的 HTML 标记， ``<bold>`` 元素不会被 tag helper 修改。

4. Comment out the ``[HtmlTargetElement]`` attribute line and it will default to targeting ``<bold>`` tags, that is, HTML markup of the form ``<bold>``. Remember, the default naming convention will match the class name **Bold**\TagHelper to ``<bold>`` tags.

4. 注释掉 ``[HtmlTargetElement]`` 属性行，其目标将为 ``<bold>`` 标签，也就是 HTML 形式的标记 ``<bold>`` 。请记得，默认的名称转换将从匹配类名 **Bold**\TagHelper 变为匹配 ``<bold>`` 标签。

5. Run the app and verify that the ``<bold>`` tag is processed by the tag helper.

5. 运行程序可验证 ``<bold>`` 标签已被 tag helper 处理了。

Decorating a class with multiple ``[HtmlTargetElement]`` attributes results in a logical-OR of the targets. For example, using the code below, a bold tag or a bold attribute will match.

对一个类配置多个 ``[HtmlTargetElement]`` 特性的结果将是对目标作逻辑或判断。例如，使用下列代码，bold 标签或 bold 属性将被匹配。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/zBoldTagHelperCopy.cs
  :language: c#
  :lines: 5-6

When multiple attributes are added to the same statement, the runtime treats them as a logical-AND. For example, in the code below, an HTML element must be named "bold" with an attribute named "bold" ( <bold bold /> ) to match.

当在同一个声明中使用多个属性时，运行时将视为逻辑与关系。例如，使用如下代码，HTML 元素必须命名为 "bold" 并具有 "bold" 属性方能匹配。

.. code-block:: c#

  [HtmlTargetElement("bold", Attributes = "bold")]

You can also use the ``[HtmlTargetElement]`` to change the name of the targeted element. For example if you wanted the ``BoldTagHelper`` to target ``<MyBold>`` tags, you would use the following attribute:

你同样可以使用 ``[HtmlTargetElement]`` 来改变目标元素名称。例如，如果你想要使 ``BoldTagHelper`` 指向目标 ``<MyBold>`` 标签，你应该使用以下属性：

.. code-block:: c#

  [HtmlTargetElement("MyBold")]

Web site information Tag Helper
------------------------------------

网站信息 Tag Helper
---------------------

#. Add a *Models* folder.

#. 添加一个 *Models* 文件夹。

#. Add the following ``WebsiteContext`` class to the *Models* folder:

#. 添加下面的 ``WebsiteContext`` 类到 *Models* 文件夹：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Models/WebsiteContext.cs
  :language: c#

3. Add the following ``WebsiteInformationTagHelper`` class to the *TagHelpers* folder.

3. 添加下面的 ``WebsiteInformationTagHelper`` 类到 *TagHelpers* 文件夹。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/WebsiteInformationTagHelper.cs
  :language: c#

**Notes:**

**说明：**

- As mentioned previously, tag helpers translates Pascal-cased C# class names and properties for tag helpers into `lower kebab case <http://c2.com/cgi/wiki?KebabCase>`__. Therefore, to use the ``WebsiteInformationTagHelper`` in Razor, you'll write ``<website-information />``.

- 如前文所述，tag helper 将 tag helper 的 C# 类名和属性 Pascal 形式转换为 `小写 kebab 形式 <http://c2.com/cgi/wiki?KebabCase>`__。尽管如此，在 Razor 中使用 ``WebsiteInformationTagHelper`` 你将能输出 ``<website-information />``。

- We are not explicitly identifying the target element with the ``[HtmlTargetElement]`` attribute, so the default of ``website-information`` will be targeted. If you applied the following attribute (note it's not kebab case but matches the class name):

- 我们并非明确要使用 ``[HtmlTargetElement]`` 属性指定目标元素，因此， ``website-information`` 的默认方式将被作为目标。如果你使用下面的属性（注意它不是 kebab 形式而是匹配类名）：

.. code-block:: c#

  [HtmlTargetElement("WebsiteInformation")]

The lower kebab case tag ``<website-information />`` would not match. If you want use the ``[HtmlTargetElement]`` attribute, you would use kebab case as shown below:

小写的 kebab 标签 ``<website-information />`` 不会被匹配。如果你要使用 ``[HtmlTargetElement]`` 属性，你应该使用如下所示的 kebab 形式：

.. code-block:: c#

  [HtmlTargetElement("Website-Information")]

- Elements that are self-closing have no content. For this example, the Razor markup will use a self-closing tag, but the tag helper will be creating a `section <http://www.w3.org/TR/html5/sections.html#the-section-element>`__ element (which is not self-closing and we are writing content inside the ``section`` element). Therefore, we need to set ``TagMode`` to ``StartTagAndEndTag`` to write output. Alternatively, you can comment out the line setting ``TagMode`` and write markup with a closing tag. (Example markup is provided later in this tutorial.)

- 自闭合元素没有内容。在这个例子，Razor 标记将使用自闭合标签，但 tag helper 将创建一个 `section <http://www.w3.org/TR/html5/sections.html#the-section-element>`__ 元素（是指非闭合的并且我们在 ``section`` 元素内部输出内容的元素）。因此，我们需要设置 ``TagMode`` 为 ``StartTagAndEndTag`` 来输出。换言之，你可以注释掉 ``TagMode`` 设置行，并用闭合标签书写标记。（示例标记在本教程下文中提供）

- The ``$`` (dollar sign) in the following line uses an `interpolated string <https://msdn.microsoft.com/en-us/library/Dn961160.aspx>`__:

-  下面代码行中的 ``$`` （美元符号） 使用 `interpolated string <https://msdn.microsoft.com/en-us/library/Dn961160.aspx>`__：

.. code-block:: c#

  $@"<ul><li><strong>Version:</strong> {Info.Version}</li>

5. Add the following markup to the *About.cshtml* view. The highlighted markup displays the web site information.

5. 在 *About.cshtml* 视图添加下列标记。高亮的标记显示了网站信息。

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/About.cshtml
  :language: html
  :emphasize-lines: 1,12-20
  :lines: 1-20

**Note:** In the Razor markup shown below:

**说明：** 在 Razor 标记中如下：

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/About.cshtml
  :language: html
  :lines: 13-17

Razor knows the ``info`` attribute is a class, not a string, and you want to write C# code. Any non-string tag helper attribute should be written without the ``@`` character.

Razor 知道 ``info`` 属性是一个类名，不是字符串，你需要写 C# 代码。一些非字符 tag helper 属性不应该写 ``@`` 字符。

6. Run the app, and navigate to the About view to see the web site information.

6. 运行应用，导航到关于视图查看网站信息。

**Note:**

**说明：**

- You can use the following markup with a closing tag and remove the line with ``TagMode.StartTagAndEndTag`` in the tag helper:

- 你可以使用下面的有闭标签的标记，并移除 tag helper 中有 ``TagMode.StartTagAndEndTag`` 的代码行：

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/AboutNotSelfClosing.cshtml
  :language: html
  :lines: 13-18

Condition Tag Helper
---------------------------------

条件 Tag Helper
-------------------

The condition tag helper renders output when passed a true value.

条件 tag helper 在传值为真的时候渲染输出。

#. Add the following ``ConditionTagHelper`` class to the *TagHelpers* folder.

#. 添加下面的 ``ConditionTagHelper`` 类到 *TagHelpers* 文件夹。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/ConditionTagHelper.cs
  :language: c#

2. Replace the contents of the *Views/Home/Index.cshtml* file with the following markup:

2. 使用下面的标记替换 *Views/Home/Index.cshtml* 文件中的内容：

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/Index.cshtml
  :language: html

3. Replace the ``Index`` method in the ``Home`` controller with the following code:

3. 用下面的代码替换 ``Home`` 控制器中的 ``Index`` 方法：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Controllers/HomeController.cs
  :language: c#
  :lines: 9-18
  :dedent: 6

4. Run the app and browse to the home page. The markup in the conditional ``div`` will not be rendered. Append the query string ``?approved=true`` to the URL (for example, \http://localhost:1235/Home/Index?approved=true). The approved is set to true and the conditional markup will be displayed.

4. 运行应用打开首页。在有条件的 ``div`` 中的标记不会被渲染。在URL请求字符串后添加 ``?approved=true`` （例如： \http://localhost:1235/Home/Index?approved=true）。approved 被设置 true，有条件的标记将被显示。

**Note:** We use the `nameof <https://msdn.microsoft.com/en-us/library/dn986596.aspx>`_ operator to specify the attribute to target rather than specifying a string as we did with the bold tag helper:

**说明：** 我们使用 `nameof <https://msdn.microsoft.com/en-us/library/dn986596.aspx>`_ 运算符来把属性识别为目标，而非像我们用 bold tag helper 所做的指定字符串。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/zConditionTagHelperCopy.cs
  :language: c#
  :lines: 5-18
  :emphasize-lines: 1,2,5
  :dedent: 3

The `nameof <https://msdn.microsoft.com/en-us/library/dn986596.aspx>`_ operator will protect the code should it ever be refactored (we might want to change the name to RedCondition).

`nameof <https://msdn.microsoft.com/en-us/library/dn986596.aspx>`_ 运算符可以在代码被重构的时候保护代码（我们可能想将名称改为 RedCondition）。

Avoiding Tag Helper conflicts
______________________________

避免 Tag Helper 冲突
______________________________

In this section, we will write a pair of auto-linking tag helpers. The first will replace markup containing a URL starting with HTTP to an HTML anchor tag containing the same URL (and thus yielding a link to the URL). The second will do the same for a URL starting with WWW.

在这一节，我们将写一对自动链接的 tag helper。首先将替换包含以 HTTP 为首的链接的标记为包含相同 URL（从而产生一个指向 URL 的链接）的 HTML 锚标签。其次将对以 www 为首的 URL 做同样的操作。

Because these two helpers are closely related and we may refactor them in the future, we'll keep them in the same file.

因为这两个 Helper 密切相关，我们未来将会重构它们，我们将它们放在同一文件。

#. Add the following ``AutoLinker`` class to the *TagHelpers* folder.

#. 添加下面的 ``AutoLinker`` 类到 *TagHelpers* 文件夹。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1AutoLinker.cs
  :language: c#
  :lines: 7-20
  :dedent: 4

**Notes:** The ``AutoLinkerHttpTagHelper`` class targets ``p`` elements and uses `Regex <https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.aspx>`__ to create the anchor.

**说明：**  ``AutoLinkerHttpTagHelper`` 类指向 ``p`` 元素且使用 `正则 <https://msdn.microsoft.com/en-us/library/system.text.regularexpressions.regex.aspx>`__ 来创建锚。

2. Add the following markup to the end of the *Views/Home/Contact.cshtml* file:

2. 添加下面的标记到 *Views/Home/Contact.cshtml* 文件末尾：

.. literalinclude::  authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/Home/Contact.cshtml
  :language: html
  :lines: 1-19
  :emphasize-lines: 19

3. Run the app and verify that the tag helper renders the anchor correctly.

3. 运行程序并验证 tag helper 正确渲染了锚链接。

4. Update the ``AutoLinker`` class to include the ``AutoLinkerWwwTagHelper`` which will convert www text to an anchor tag that also contains the original www text. The updated code is highlighted below:

4. 更新 ``AutoLinker`` 类，添加 ``AutoLinkerWwwTagHelper`` ，它将转换 www 文字为同样包含原始 www 文字的链接标签。修改的代码是下面高亮部分：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1AutoLinker.cs
  :language: c#
  :lines: 7-34
  :emphasize-lines: 15-34
  :dedent: 4

5. Run the app. Notice the www text is rendered as a link but the HTTP text is not. If you put a break point in both classes, you can see that the HTTP tag helper class runs first. Later in the tutorial we'll see how to control the order that tag helpers run in. The problem is that the tag helper output is cached, and when the WWW tag helper is run, it overwrites the cached output from the HTTP tag helper. We'll fix that with the following code:

5. 运行应用。注意 www 文字被渲染为一条链接，但 HTTP 文字却没有。如果你在两个类中打断点，你可以发现 HTTP tag helper 类先运行。在稍后的教程中我们将看到如何控制其中 tag helper 执行顺序。问题在于 tag helper 输出是被缓存的，而当 WWW tag helper 在运行的时候，它覆盖了来自 HTTP tag helper 的输出缓存。我们将使用下面的代码来修复它：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1AutoLinkerCopy.cs
  :language: c#
  :lines: 8-38
  :emphasize-lines: 5,6,10,21,22,26

**Note:** In the first edition of the auto-linking tag helpers, we got the content of the target with the following code:

**说明：** 在第一个 auto-linking tag helper 版本中，我们使用下面的代码取得目标的内容：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1AutoLinker.cs
  :language: c#
  :lines: 12
  :dedent: 12

That is, we call ``GetChildContentAsync`` using the ``TagHelperOutput`` passed into the ``ProcessAsync`` method. As mentioned previously, because the output is cached, the last tag helper to run wins. We fixed that problem with the following code:

也就是，我们使用 ``TagHelperOutput`` 调用 ``GetChildContentAsync`` 传入了 ``ProcessAsync`` 方法。如前面提到的，因为输出是缓存的，最终运行的 tag helper 成功。我们使用下面的代码来修复这个问题：

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z2AutoLinkerCopy.cs
  :language: c#
  :lines: 18-19
  :dedent: 12

The code above checks to see if the content has been modified, and if it has, it gets the content from the output buffer.

上面的代码检查可见内容是否已被改变，如果已经存在，则从输出缓冲中获取内容。

7. Run the app and verify that the two links work as expected. While it might appear our auto linker tag helper is correct and complete, it has a subtle problem. If the WWW tag helper runs first, the www links will not be correct. Update the code by adding the ``Order`` overload to control the order that the tag runs in. The ``Order`` property determines the execution order relative to other tag helpers targeting the same element. The default order value is zero and instances with lower values are executed first.

7. 运行应用可验证两个链接如愿执行。在表现出我们的自动链接 tag helper 是完全正确的同时，它还有个小问题。如果 www tag helper 首先运行，www 链接不正常了。添加 ``Order`` 重载修改代码来控制其中 tag 的运行的顺序。``Order`` 属性决定指向同一目标元素的相关 tag helper 的执行顺序。顺序默认值为 0 ，越小的值被优先执行。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z2AutoLinkerCopy.cs
  :language: c#
  :lines: 8-15
  :emphasize-lines: 5-8
  :dedent: 4

The above code will guarantee that the HTTP tag helper runs before the WWW tag helper. Change ``Order`` to ``MaxValue`` and verify that the markup generated for the  WWW tag is incorrect.

以上代码将授权 HTTP tag helper 在 WWW tag helper 之前执行。将 ``Order`` 改为  ``最大值`` 可验证为 WWW 标签生成的标记不正确。

Inspecting and retrieving child content
----------------------------------------

审查并检索子集内容
---------------------

The tag-helpers provide several properties to retrieve content.

tag-helper 提供了多种属性来检索内容。

- The result of ``GetChildContentAsync`` can be appended to ``output.Content``.

-  ``GetChildContentAsync`` 的结果可被附加到 ``output.Content``。

- You can inspect the result of ``GetChildContentAsync`` with ``GetContent``.

- 你可以使用 ``GetContent`` 审查 ``GetChildContentAsync`` 的结果。

- If you modify ``output.Content``, the TagHelper body will not be executed or rendered unless you call ``GetChildContentAsync`` as in our auto-linker sample:

- 如果你修改 ``output.Content``，TagHelper 内容将不被执行或渲染，除非你像在我们的 auto-linker 示例中调用 ``GetChildContentAsync`` 。

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/TagHelpers/z1AutoLinkerCopy.cs
  :language: c#
  :lines: 8-21
  :emphasize-lines: 5,6,10
  :dedent: 4

- Multiple calls to ``GetChildContentAsync`` will return the same value and will not re-execute the ``TagHelper`` body unless you pass in a false parameter indicating  not use the cached result.

- 多次调用 ``GetChildContentAsync`` 将返回相同的值，而不是重复执行 ``TagHelper`` 主体，除非你传入一个 false 参数指示不使用缓存结果。
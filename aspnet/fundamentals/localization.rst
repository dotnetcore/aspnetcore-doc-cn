Globalization and localization   
==============================

全球化与本地化  
==============================

原文：`Globalization and localization <https://docs.asp.net/en/latest/fundamentals/localization.html>`_

作者：`Rick Anderson`_, `Damien Bowden`_, `Bart Calixto`_, `Nadeem Afana`_  

翻译：`谢炀(Kiler) <https://github.com/kiler398/>`_

校对：`许登洋(Seay) <https://github.com/SeayXu>`_


Creating a multilingual website with ASP.NET Core will allow your site to reach a wider audience. ASP.NET Core provides services and middleware for localizing into different languages and cultures.

使用 ASP.NET Core 创建一个多语言版本的网站有助于你吸引到更多的用户，ASP.NET Core 提供服务和中间件来支持本地化语言和文化。

Internationalization involves `Globalization <https://msdn.microsoft.com/en-us/library/aa292081(v=vs.71).aspx>`__ and `Localization <https://msdn.microsoft.com/en-us/library/aa292137(v=vs.71).aspx>`__. Globalization is the process of designing apps that support different cultures. Globalization adds support for input, display, and output of a defined set of language scripts that relate to specific geographic areas.

国际化涉及 `全球化 <https://msdn.microsoft.com/en-us/library/aa292081(v=vs.71).aspx>`__ 和 `本地化 <https://msdn.microsoft.com/en-us/library/aa292137(v=vs.71).aspx>`__。全球化是为了应用程序支持不同文化而设计的。全球化增加了对特定地理区域的语言文字的输入、显示和输出的支持。

Localization is the process of adapting a globalized app, which you have already processed for localizability, to a particular culture/locale.  For more information see **Globalization and localization terms** near the end of this document.

本地化是将已经完成了本地化分析处理的的全球化应用程序，针对特定的文化/区域设定做更改的程序。欲了解更多信息，请参阅文档末尾 **Globalization and localization terms** 。

App localization involves the following:

#. Make the app's content localizable
#. Provide localized resources for the languages and cultures you support
#. Implement a strategy to select the language/culture for each request

应用程序本地化包含以下内容：

#. 让应用程序的内容本地化。
#. 为不同的文化和语言提供本地化资源包。
#. 在每个请求中实现语言/文化切换策略。

.. contents:: Sections:
  :local:
  :depth: 1

Make the app's content localizable
--------------------------------------

让应用程序的内容本地化
--------------------------------------

Introduced in ASP.NET Core, :dn:iface:`~Microsoft.Extensions.Localization.IStringLocalizer` and :dn:iface:`~Microsoft.Extensions.Localization.IStringLocalizer\<T>` were architected to improve productivity when developing localized apps. ``IStringLocalizer`` uses the `ResourceManager <https://msdn.microsoft.com/en-us/library/system.resources.resourcemanager(v=vs.110).aspx>`__ and `ResourceReader <https://msdn.microsoft.com/en-us/library/system.resources.resourcereader(v=vs.110).aspx>`__ to provide culture-specific resources at run time. The simple interface has an indexer and an ``IEnumerable`` for returning localized strings. ``IStringLocalizer`` doesn't require you to store the default language strings in a resource file. You can develop an app targeted for localization and not need to create resource files early in development. The code below shows how to wrap the string "About Title" for localization.

在 ASP.NET Core 中， :dn:iface:`~Microsoft.Extensions.Localization.IStringLocalizer` 以及 :dn:iface:`~Microsoft.Extensions.Localization.IStringLocalizer\<T>` 在开发本地化应用程序时被架构为提高生产力的手段。 ``IStringLocalizer`` 使用 `ResourceManager <https://msdn.microsoft.com/en-us/library/system.resources.resourcemanager(v=vs.110).aspx>`__ and `ResourceReader <https://msdn.microsoft.com/en-us/library/system.resources.resourcereader(v=vs.110).aspx>`__ 在运行时提供指定文化的资源文件。``IStringLocalizer`` 是一个实现了 ``IEnumerable`` 的简单接口并且拥有索引器来来返回本地化的字符串。``IStringLocalizer`` 并不需要你把默认语言字符串存储在资源文件中。你可以针对某个特定的语言开发应用程序，而不是需要在开发早期创建资源文件。下面的代码演示了如何包装字符串 “About Title” 本地化。

.. literalinclude:: localization/sample/Controllers/AboutController.cs
  :language: c#

In the code above, the ``IStringLocalizer<T>`` implementation comes from :doc:`/fundamentals/dependency-injection`. If the localized value of "About Title" is not found, then the indexer key is returned, that is, the string "About Title". You can leave the default language literal strings in the app and wrap them in the localizer, so that you can focus on developing the app. You develop your app with your default language and prepare it for the localization step without first creating a default resource file. Alternatively, you can use the traditional approach and provide a key to retrieve the default language string. For many developers the new workflow of not having a default language *.resx* file and simply wrapping the string literals can reduce the overhead of localizing an app. Other developers will prefer the traditional work flow as it can make it easier to work with longer string literals and make it easier to update localized strings.

在上面的代码中， ``IStringLocalizer<T>`` 实现了 :doc:`/fundamentals/dependency-injection` 。如果没有发现 "About Title" 的本地化值，则索引的键值被返回，即是字符串 "About Title" 。您可以在应用程序中保留默认语言文字字符串，然后再使用 localizer 包装他们，这样你就可以专注于开发应用程序。使用默认语言开发应用并为进行本地化的步骤做准备，同时无需事先创建一个默认的资源文件。另外，您也可以使用传统的方法，一键恢复默认语言的字符串。对于大部分开发者来说新的工作流程无需一个默认语言的 *.resx*  文件，并且简单地包装字符串可以减少本地化的应用程序的工作量。其他开发者会选择传统的工作流程，因为它可以更容易地与长字符串文字工作，并使其更易于更新本地化字符串。

Use the `IHtmlLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer-TResource/index.html>`__ implementation for resources that contain HTML. ``IHtmlLocalizer`` HTML encodes arguments that are formatted in the resource string, but not the resource string. In the sample highlighted below, only the value of ``name`` parameter is HTML encoded.

使用 `IHtmlLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer-TResource/index.html>`__  来处理包含 HTML 的资源文件， ``IHtmlLocalizer`` 对格式化过的资源字符串参数进行编码，而不是对原始资源字符串。 下面例子中的高亮代码一般你仅仅只希望本地化文本而非 HTML，只有 ``name`` 参数的值被 HTML 编码。

.. literalinclude:: localization/sample/Controllers/BookController.cs
  :language: c#
  :lines: 1-23
  :emphasize-lines: 3,5,20 

:Note: You generally want to only localize text and not HTML.

:Note: 一般你仅仅只希望本地化文本而非 HTML。

At the lowest level, you can get ``IStringLocalizerFactory`` out of :doc:`/fundamentals/dependency-injection`:

在最低层次, 你可以通过 :doc:`/fundamentals/dependency-injection` 获取 ``IStringLocalizerFactory``：

.. literalinclude:: localization/sample/Controllers/TestController.cs
  :language: c#
  :lines: 9-26
  :emphasize-lines: 6-11 
  :dedent: 3

The code above demonstrates each of the two factory create methods.

上面的代码演示了两个不同的工厂创建方法。

You can partition your localized strings by controller, area, or have just one container. In the sample app, a dummy class named ``SharedResource`` is used for shared resources.

你可以通过控制器、区域划分本地化字符串，或者都包含在一个容器中。在示例应用程序中，名为 ``SharedResource`` 的 dummy 类被用于共享资源。

.. literalinclude:: localization/sample/Resources/SharedResource.cs
  :language: c#

Some developers use the ``Startup`` class to contain global or shared strings.  In the sample below, the ``InfoController`` and the ``SharedResource`` localizers are used:

一些开发人员使用 ``Startup`` 类来包含全局或共享字符串。在下面的例子中，展示了 ``InfoController`` 以及 ``SharedResource`` 本地化使用：

.. literalinclude:: localization/sample/Controllers/InfoController.cs
  :language: c#
  :lines: 9-26
  :dedent: 3

View localization
--------------------

视图本地化
--------------------

The `IViewLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IViewLocalizer/index.html>`__ service provides localized strings for a `view <http://docs.asp.net/projects/mvc/en/latest/views/index.html>`_. The `ViewLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/ViewLocalizer/index.html>`__ class implements this interface and finds the resource location from the view file path. The following code shows how to use the default implementation of ``IViewLocalizer``:

 `IViewLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IViewLocalizer/index.html>`__ 服务为视图提供本地化字符串。 `ViewLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Localization/ViewLocalizer/index.html>`__ 
 类实现了这个接口，并且根据视图文件的路径来查找资源。下面的代码演示了如何使用 ``IViewLocalizer`` 的默认实现：

.. literalinclude:: localization/sample/Views/Home/About.cshtml
  :language: HTML
  
The default implementation of ``IViewLocalizer`` finds the resource file based on the view's file name. There is no option to use a global shared resource file. ``ViewLocalizer`` implements the localizer using `IHtmlLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer/index.html>`__, so Razor doesn't HTML encode the localized string. You can parameterize resource strings and ``IViewLocalizer`` will HTML encode the parameters, but not the resource string. Consider the following Razor markup:

 ``IViewLocalizer`` 的默认实现基于视图的文件名称来查找资源文件。没有使用全局共享的资源文件的可选项。 ``ViewLocalizer`` 使用 `IHtmlLocalizer <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer/index.html>`__ 实现本地化，
 所以 Razor 模版不会 HTML 编码本地化字符串。你可以使用参数传递资源字符串并且 ``IViewLocalizer`` 会HTML 编码参数，而不是资源字符串。参考下面的 Razor 标签代码：

.. code-block:: HTML

  @Localizer["<i>Hello</i> <b>{0}!</b>", UserManager.GetUserName(User)]
 
A French resource file could contain the following:

========================  ===============================  
Key                       Value    
========================  ===============================  
<i>Hello</i> <b>{0}!</b>  <i>Bonjour</i> <b>{0}!</b>  
========================  ===============================

法语资源文件会包含下述内容：

========================  ===============================  
键                        值   
========================  ===============================  
<i>Hello</i> <b>{0}!</b>  <i>Bonjour</i> <b>{0}!</b>  
========================  ===============================

The rendered view would contain the HTML markup from the resource file.

渲染视图将包含资源文件中的 HTML 标签。

:Note: You generally want to only localize text and not HTML.

:Note: 你仅仅只想本地化文本而非HTML。

To use a shared resource file in a view, inject `IHtmlLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer-TResource/index.html>`__:

为了在视图中试用共享资源文件，需要注入  `IHtmlLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/Localization/IHtmlLocalizer-TResource/index.html>`__:

.. literalinclude:: localization/sample/Views/Test/About.cshtml
  :language: HTML
  :emphasize-lines: 5,12

DataAnnotations localization
------------------------------------

DataAnnotations 本地化
------------------------------------

DataAnnotations error messages are localized with `IStringLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Localization/IStringLocalizer-T/index.html>`__. Using the option ``ResourcesPath = "Resources"``, the error messages in ``RegisterViewModel`` can be stored in either of the following paths:

使用 `IStringLocalizer<T> <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/Extensions/Localization/IStringLocalizer-T/index.html>`__ 本地化DataAnnotations错误信息。 使用选项 ``ResourcesPath = "Resources"``,  ``RegisterViewModel`` 中的错误信息会存储到以下路径中:

- Resources/ViewModels.Account.RegisterViewModel.fr.resx
- Resources/ViewModels/Account/RegisterViewModel.fr.resx
  
.. literalinclude:: localization/sample/ViewModels/Account/RegisterViewModel.cs
  :language: c#
  :lines: 9-26
  :dedent: 3
  
The runtime doesn't look up localized strings for non-validation attributes. In the code above, "Email" (from ``[Display(Name = "Email")]``) will not be localized.

运行时不支持从非验证属性中查找本地化字符串。在上面的代码里，“Email” (来自 ``[Display(Name = "Email")]``) ）将不会被本地化。

Provide localized resources for the languages and cultures you support
------------------------------------------------------------------------

为你的语言和文化提供本地化资源支持
------------------------------------------------------------------------

SupportedCultures and SupportedUICultures
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

SupportedCultures（文化支持） 以及 SupportedUICultures（UI文化支持）
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

ASP.NET Core allows you to specify two culture values, ``SupportedCultures`` and ``SupportedUICultures``. The `CultureInfo <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo(v=vs.110).aspx>`__ object for ``SupportedCultures`` determines the results of culture-dependent functions, such as date, time, number, and currency formatting. ``SupportedCultures`` also determines the sorting order of text, casing conventions, and string comparisons. See `CultureInfo.CurrentCulture <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.currentculture%28v=vs.110%29.aspx>`__ for more info on how the server gets the Culture. The ``SupportedUICultures`` determines which translates strings (from *.resx* files) are looked up by the `ResourceManager <https://msdn.microsoft.com/en-us/library/system.resources.resourcemanager(v=vs.110).aspx>`__. The ``ResourceManager`` simply looks up culture-specific strings that is determined by ``CurrentUICulture``. Every thread in .NET has ``CurrentCulture`` and ``CurrentUICulture`` objects. ASP.NET Core inspects these values when rendering culture-dependent functions. For example, if the current thread's culture is set to "en-US" (English, United States), ``DateTime.Now.ToLongDateString()`` displays "Thursday, February 18, 2016", but if ``CurrentCulture`` is set to "es-ES" (Spanish, Spain) the output will be "jueves, 18 de febrero de 2016".

ASP.NET Core 允许你指定两个文化值， ``SupportedCultures`` 以及 ``SupportedUICultures``。``SupportedCultures`` 的  `CultureInfo <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo(v=vs.110).aspx>`__  对象决定了和文化相关的函数，如日期，时间，数字和货币格式的结果。 ``SupportedCultures`` 同时决定了如何文字排序，大小写转换以及字符串比较。参考 `CultureInfo.CurrentCulture <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.currentculture%28v=vs.110%29.aspx>`__  获取更多关于服务器如何获取文化的信息。``SupportedUICultures`` 决定如何通过 `ResourceManager <https://msdn.microsoft.com/en-us/library/system.resources.resourcemanager(v=vs.110).aspx>`__  查找翻译字符串（从 *.resx* 文件）。 `ResourceManager` 只是通过 ``CurrentUICulture`` 简单的查找指定文化的字符串。.NET 的每个线程都会拥有 ``CurrentCulture`` 和 ``CurrentUICulture`` 对象。当 ASP.NET Core 在渲染与文化相关的函数的时候会检视这些对象值。例如，如果当前线程的区域性设置为 "en-US" （英语，美国）， ``DateTime.Now.ToLongDateString() "Thursday, February 18, 2016"`` ，但如果CurrentCulture设置为 "es-ES" （西班牙语，西班牙），输出将会是 "jueves, 18 de febrero de 2016"。

.. contents:: Sections:
  :local:
  :depth: 1
  
.. note:: Currently, resource files are not read when the project is run from Visual Studio. See `this issue <https://github.com/aspnet/dnx/issues/3047>`_ for more information. Until the issue with Visual Studio is addressed, you can test the project by running it from the command line.

.. note:: 当前，当项目在 Visual Studio 中运行的时候资源文件是不可读的。更多信息请参见 `这个问题 <https://github.com/aspnet/dnx/issues/3047>`_  。在 Visual Studio 的这个问题得到解决前，您可以通过命令行来测试运行项目。

Working with resource files
-----------------------------

使用资源文件
-----------------------------

A resource file is a useful mechanism for separating localizable strings from code. Translated strings for the non-default language are isolated *.resx* resource files. For example, you might want to create Spanish resource file named *Welcome.es.resx* containing translated strings. "es" is the language code for Spanish. To create this resource file in Visual Studio:

资源文件是一种从代码中分离本地化字符串的有效机制。非默认语言翻译字符串被隔离到 *.resx* 资源文件中。例如，您可能希望创建一个名为 *Welcome.es.resx* 的西班牙语资源文件来包含翻译字符串。 "es" 是西班牙语的语言编码。在Visual Studio中，这样创建资源文件：

#. In **Solution Explorer**, right click on the folder which will contain the resource file > **Add** > **New Item**.

#. 在 **Solution Explorer** 中， 右击包含资源文件的目录 > **Add** > **New Item** 。

.. image:: localization/_static/newi.png

2. In the **Search installed templates** box, enter "resource" and name the file.

2. 在 **Search installed templates** 对话框中， 输入 "resource" 并且命名文件。

.. image:: localization/_static/res.png

3. Enter the key value (native string) in the **Name** column and the translated string in the **Value** column.

3.  在 **Name** 列输入键值 (本地字符串) 在 **Value** 列输入翻译值。

.. image:: localization/_static/hola.png

Visual Studio shows the *Welcome.es.resx* file.

Visual Studio 展示出 *Welcome.es.resx* 文件。

.. image:: localization/_static/se.png

Generating resource files with Visual Studio
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

使用 Visual Studio 创建资源文件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

If you create a resource file in Visual Studio without a culture in the file name (for example, *Welcome.resx*), Visual Studio will create a C# class with a property for each string. That's usually not what you want with ASP.NET Core; you typically don't have a default *.resx* resource file (A *.resx* file without the culture name). We suggest you create the *.resx* file with a culture name (for example *Welcome.fr.resx*). When you create a *.resx* file with a culture name, Visual Studio will not generate the class file. We anticipate that many developers will **not** create a default language resource file. 

如果你在Visual Studio中创建一个资源文件，而且文件名中不存在文化信息（例如， *Welcome.resx*），Visual Studio 将会为之创建一个C#类并且为每个字符串创建一个字段。这通常不是你想要的 ASP.NET Core 的方式；你通常不会有一个默认的.resx资源文件（文件名不包含文化信息的 *.resx* 文件）。我们建议您创建带有文化名称的.resx（例如 * Welcome.fr.resx* ）文件。当你创建一个与文化信息关联的  *.resx* 时，Visual Studio 不会产生类文件。按照我们预期大部分开发者商 **不会** 创建默认语言资源文件。

Adding Other Cultures  
^^^^^^^^^^^^^^^^^^^^^^^^^

添加其他文化  
^^^^^^^^^^^^^^^^^^^^^^^^^^
 
Each language and culture combination (other than the default language) requires a unique resource file. You can create resource files for different cultures and locales by creating new resource files in which the ISO language codes are part of the file name (for example, **en-us**, **fr-ca**, and **en-gb**). These ISO codes are placed between the file name and the .resx file name extension, as in *Welcome.es-MX.resx* (Spanish/Mexico). To specify a culturally neutral language, you would eliminate the country code, such as *Welcome.fr.resx* for the French language. 

每一种语言和文化的结合（除了默认语言外）需要一个独特的资源文件。您可以为不同的文化和语言环境创建新的资源文件，ISO 语言代码作为文件名的一部分（例如， **en-us** 、 **fr-ca** 以及 **en-gb**）。这些 ISO 语言代码放置在文件名和 .resx 文件扩展名之间，如 *Welcome.es-MX.resx* （西班牙语/墨西哥）。要指定文化中性语言，你可以消除国家代码，如 *Welcome.fr.resx* 为法语。

Implement a strategy to select the language/culture for each request
---------------------------------------------------------------------

为每个请求提供语言文化选择功能的实施策略
---------------------------------------------------------------------

Configuring localization
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

配置本地化
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Localization is configured in the ``ConfigureServices`` method:

本地化在 ``ConfigureServices`` 方法中配置：

.. literalinclude:: localization/sample/Startup.cs
  :language: c#
  :lines: 44-45,55-61
  :dedent: 6
  :emphasize-lines: 4,7,8

- ``AddLocalization`` Adds the localization services to the services container. The code above also sets the resources path to "Resources".
- ``AddViewLocalization`` Adds support for localized view files. In this sample view localization is based on the view file suffix. For example "fr" in the *Index.fr.cshtml* file.
- ``AddDataAnnotationsLocalization`` Adds support for localized ``DataAnnotations`` validation messages through ``IStringLocalizer`` abstractions.
  
- ``AddLocalization`` 在服务容器中添加本地化服务。 上述代码同时把资源文件路径设置到 "Resources" 。
- ``AddViewLocalization`` 添加本地化视图文件支持。 在这个示例中视图本地化是基于视图文件后缀的。例如 ：*Index.fr.cshtml* 中的 "fr" 。
- ``AddDataAnnotationsLocalization`` 增加了通过 ``IStringLocalizer`` 来抽象支持本地化 ``DataAnnotations`` 验证消息。  
  
Localization middleware
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

本地化中间件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The current culture on a request is set in the localization :doc:`/fundamentals/middleware`. The localization middleware is enabled in the ``Configure`` method of *Startup.cs* file.

在请求中的当前的文化是在本地化  :doc:`/fundamentals/middleware`  中设置的。本地化中间件在 *Startup.cs* 文件的 ``Configure`` 方法中启用。

.. literalinclude:: localization/sample/Startup.cs
  :language: c#
  :lines: 107, 136-159
  :dedent: 6

:dn:method:`~Microsoft.AspNetCore.Builder.ApplicationBuilderExtensions.UseRequestLocalization` initializes a :dn:class:`~Microsoft.AspNetCore.Builder.RequestLocalizationOptions` object. On every request the list of `RequestCultureProvider <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/RequestCultureProvider/index.html>`__ in the `RequestLocalizationOptions <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/RequestLocalizationOptions/index.html>`__ is enumerated and the first provider that can successfully determine the request culture is used. The default providers come from the ``RequestLocalizationOptions`` class:

:dn:method:`~Microsoft.AspNetCore.Builder.ApplicationBuilderExtensions.UseRequestLocalization` 初始化 :dn:class:`~Microsoft.AspNetCore.Builder.RequestLocalizationOptions` 对象。在每次请求里 `RequestLocalizationOptions <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Localization/RequestLocalizationOptions/index.html>`__  的 `RequestCultureProvider <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/RequestCultureProvider/index.html>`__ 列表会被遍历，第一个provider 会被使用来判断请求使用的文化。默认的 provider 来自 ``RequestLocalizationOptions`` 类：

#. `QueryStringRequestCultureProvider <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/QueryStringRequestCultureProvider/index.html>`__
#. `CookieRequestCultureProvider <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/CookieRequestCultureProvider/index.html>`__
#. `AcceptLanguageHeaderRequestCultureProvider <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Localization/AcceptLanguageHeaderRequestCultureProvider/index.html>`__

The default list goes from most specific to least specific. Later in the article we'll see how you can change the order and even add a custom culture provider. If none of the providers can determine the request culture, the ``DefaultRequestCulture`` is used.
  
默认列表从最具体的到最不具体的。后面的文章中我会告诉你如何更改顺序，甚至添加自定义的本地化 provider。如果没有非空的 provider， ``DefaultRequestCulture`` 被使用。
    
QueryStringRequestCultureProvider
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Some apps will use a query string to set the `culture and UI culture <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx#Current>`__. For apps that use the cookie or Accept-Language header approach, adding a query string to the URL is useful for debugging and testing code. By default, the ``QueryStringRequestCultureProvider`` is registered as the first localization provider in the ``RequestCultureProvider`` list. You pass the query string parameters ``culture`` and ``ui-culture``. The following example sets the specific culture (language and region) to Spanish/Mexico:

有些应用程序会使用一个查询字符串来设置 `区域性和 UI 区域性  <https://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx#Current>`__。对于使用 cookie 或者 Accept-Language 头的方法的应用程序，在 URL 上增加查询字符串有助于调试和测试代码。除非你修改 ``RequestCultureProvider`` 列表，否则查询字符串参数永远是用来指定本地化 provider 的。你可在查询字符串参数中传递 ``culture`` 以及 ``ui-culture`` 参数。下面的例子指定了具体的区域性（语言和区域）设置为西班牙语/墨西哥：

  \http://localhost:5000/?culture=es-MX&ui-culture=es-MX

If you only pass in one of the two (``culture`` or ``ui-culture``), the query string provider will set both values using the one you passed in. For example, setting just the culture will set both the ``Culture`` and the ``UICulture``:

如果你仅仅使用（``culture`` 或者 ``ui-culture``）中的一个参数进行传递，查询字符串 provider 将使用你传递一个值来设置这两个参数。例如，仅设置culture，将会同样设置 ``Culture`` 和 ``UICulture``：

  \http://localhost:5000/?culture=es-MX

CookieRequestCultureProvider
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Production apps will often provide a mechanism to set the culture with the ASP.NET Core culture cookie. Use the :dn:method:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue`  method to create a cookie.

生产环境的应用程序通常会提供一种机制，把区域性信息设置到 ASP.NET Core 区域性 cookie 之上。使用 :dn:method:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.MakeCookieValue`  方法创建一个 cookie.

The :dn:cls:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider` :dn:field:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName`  returns the default cookie name used to track the user’s preferred culture information. The default cookie  name is ".AspNetCore.Culture".

The cookie format is ``c=%LANGCODE%|uic=%LANGCODE%``, where ``c`` is ``Culture`` and ``uic`` is ``UICulture``, for example:

c='en-UK'\|uic='en-US'

If you only specify one of culture info and UI culture, the specified culture will be used for both culture info and UI culture.

:dn:cls:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider` 的 :dn:field:`~Microsoft.AspNetCore.Localization.CookieRequestCultureProvider.DefaultCookieName` 返回用于跟踪用户的首选区域性信息默认的 Cookie 名称。默认的 Cookie 名称是 “.AspNetCore.Culture”。

cookie 的格式是 ``c=%LANGCODE%|uic=%LANGCODE%``, ``c`` 为区域信息 and ``uic`` 为 UI 区域信息,例如：

c='en-UK'\|uic='en-US'

如果仅指定culture 或 UI culture中的一个，指定的区域性信息将同时用于 culture和 UI culture。

The Accept-Language HTTP header
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

HTTP Accept-Language HTTP 头信息
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The `Accept-Language header <https://www.w3.org/International/questions/qa-accept-lang-locales>`__ is settable in most browsers and was originally intended to specify the user's language. This setting indicates what the browser has been set to send or has inherited from the underlying operating system. The Accept-Language HTTP header from a browser request is not an infallible way to detect the user's preferred language (see `Setting language preferences in a browser <https://www.w3.org/International/questions/qa-lang-priorities.en.php>`__). A production app should include a way for a user to customize their choice of culture.

大多数浏览器支持设置 `Accept-Language header <https://www.w3.org/International/questions/qa-accept-lang-locales>`__ 头信息，这个设置最初的目的是为了指定用户的语言。指示什么类型的浏览器已被设置且发送或已经从底层操作系统继承。从浏览器请求的 Accept-Language HTTP 标头来检测用户的首选语言容易产生错误（请参见 `在浏览器中设置语言首选项 <https://www.w3.org/International/questions/qa-lang-priorities.en.php>`__）。生产环境中应用程序应该包括一种方法让用户自己选择的区域性信息。

Setting the Accept-Language HTTP header in IE 
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

在 IE 浏览器中设置 Accept-Language HTTP 头信息
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

#. From the gear icon, tap **Internet Options**.
#. Tap **Languages**.

#. 在齿轮图标菜单中, 点击 **Internet Options**。
#. 点击 **Languages**。

.. image:: localization/_static/lang.png

3. Tap **Set Language Preferences**.
#. Tap **Add a language**.
#. Add the language.
#. Tap the language, then tap **Move Up**.

3. 点击 **Set Language Preferences**。
#. 点击 **Add a language**。
#. 添加语言。
#. 点击语言, 然后点击 **Move Up**。

Using a custom provider
^^^^^^^^^^^^^^^^^^^^^^^^^^^^

使用自定义 provider
^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Suppose you want to let your customers store their language and culture in your databases. You could write a provider to look up these values for the user. The following code shows how to add a custom provider:

假设你想要在数据库里面存储客户的语言和文化信息。你可以写一个 provider 来查找这些用户的值。下面的代码演示如何添加自定义provider：

.. code-block:: c#

    services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("fr")
        };

        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;

        options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
        {
           // My custom request culture logic
          return new ProviderCultureResult("en");
        }));
    });

Use ``RequestLocalizationOptions`` to add or remove localization providers. 

使用 ``RequestLocalizationOptions`` 添加或者删除本地化 providers . 

Resource file naming
---------------------

资源文件命名
---------------------

Resources are named for the type of their class minus the default namespace (which is also the name of the assembly). For example, a French resource in the ``LocalizationWebsite.Web`` project for the class ``LocalizationWebsite.Web.Startup`` would be named *Startup.fr.resx*. The class ``LocalizationWebsite.Web.Controllers.HomeController`` would be *Controllers.HomeController.fr.resx*. If for some reason your targeted class is not in the base namespace you will need the full type name. For example, in the sample project a type ``ExtraNamespace.Tools`` would be *ExtraNamespace.Tools.fr.resx*.

资源被命名为资源文件的类名减去默认命名空间（一般是应用程序集名称）。例如 ``LocalizationWebsite.Web`` 项目  ``LocalizationWebsite.Web.Startup`` 类的法语的资源将被命名为 *Startup.fr.resx*。 ``LocalizationWebsite.Web.Controllers.HomeController``  类则是 *Controllers.HomeController.fr.resx*。如果出于某种原因，你的目标类是不是在默认的命名空间，您将需要完整的类型名称。 例如，在示例项目类型 ``ExtraNamespace.Tools`` 类会使用 *ExtraNamespace.Tools.fr.resx* 。

In the sample project, the ``ConfigureServices`` method sets the ``ResourcesPath`` to "Resources", so the project relative path for the home controller's French resource file is *Resources/Controllers.HomeController.fr.resx*. Alternatively, you can use folders to organize resource files. For the home controller, the path would be *Resources/Controllers/HomeController.fr.resx*. If you don't use the ``ResourcesPath`` option, the *.resx* file would go in the project base directory. The resource file for ``HomeController`` would be named *Controllers.HomeController.fr.resx*. The choice of using the dot or path naming convention depends on how you want to organize your resource files.

在示例项目中， ``ConfigureServices`` 方法将 ``ResourcesPath`` 设置为 "Resources"，所以对于 home controller 的法语资源文件的项目相对路径是Resources/Controllers.HomeController.fr.resx。另外，您也可以使用文件夹来组织资源文件。对于 home controller ，路径将是 *Resources/Controllers/HomeController.fr.resx*。如果不使用 ``ResourcesPath`` 可选项 ， *.resx* 文件会包含在项目的根目录。 ``HomeController`` 的资源文件将被命名为 *Controllers.HomeController.fr.resx*。选择使用点或路径命名约定的选择取决于你想如何组织你的资源文件。 

+-----------------------------------------------+--------------------+
|Resource name                                  | Dot or path naming |  
+===============================================+====================+
|*Resources/Controllers.HomeController.fr.resx* | Dot                |
+-----------------------------------------------+--------------------+  
|*Resources/Controllers/HomeController.fr.resx* | Path               |
+-----------------------------------------------+--------------------+ 

+-----------------------------------------------+--------------------+
| 资源文件                                      | 点或路径           |  
+===============================================+====================+
|*Resources/Controllers.HomeController.fr.resx* | 点                 |
+-----------------------------------------------+--------------------+  
|*Resources/Controllers/HomeController.fr.resx* | 路径               |
+-----------------------------------------------+--------------------+ 


Resource files using ``@inject IViewLocalizer`` in Razor views follow a similar pattern. The resource file for a view can be named using either dot naming or path naming. Razor view resource files mimic the path of their associated view file. Assuming we set the ``ResourcesPath`` to "Resources", the French resource file associated with the *Views/Book/About.cshtml* view could be either of the following:

资源文件在 Razor 视图中使用类似 ``@inject IViewLocalizer`` 模式来调用。对于视图的资源文件可以使用点命名或路径命名的方式进行命名。Razor 视图资源文件名参照其关联视图文件路径。假设我们设置 ``ResourcesPath`` 为 "Resources"，关联视图 *Views/Book/About.cshtml* 的法语资源文件将会如下所示：

- Resources/Views/Home/About.fr.resx
- Resources/Views.Home.About.fr.resx

If you don't use the ``ResourcesPath`` option, the *.resx* file for a view would be located in the same folder as the view.

如果你不设置 ``ResourcesPath`` 选项，视图的 *.resx* 文件将与视图文件位于同一文件夹内。

If you remove the ".fr" culture designator AND you have the culture set to French (via cookie or other mechanism), the default resource file is read and strings are localized. The Resource manager designates a default or fallback resource, when nothing meets your requested culture you're served the \*.resx file without a culture designator. If you want to just return the key when missing a resource for the requested culture you must not have a default resource file.

如果您删除了 “.fr” 区域性标志但是你又把当前区域性信息设置为法语（通过 Cookie 或其他机制），默认的资源文件将会被读取出来用以字符串本地化。当在你的服务器上找不到对应你的请求区域性信息的资源文件的时候，资源管理器会指定指定默认或备份资源。如果你想在缺少资源请求的文化时能返回键值，你不能有一个默认的资源文件。

Setting the culture programmatically
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

编程方式设置文化
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

This sample **Localization.StarterWeb** project on `GitHub <https://github.com/aspnet/entropy>`__ contains UI to set the ``Culture``. The *Views/Shared/_SelectLanguagePartial.cshtml* file allows you to select the culture from the list of supported cultures:

`GitHub <https://github.com/aspnet/entropy>`__ 上的示例项目  **Localization.StarterWeb** 包含用户界面来设置 ``Culture``。*Views/Shared/_SelectLanguagePartial.cshtml* 文件允许您从支持的区域性列表中选择区域：

.. literalinclude:: localization/sample/Views/Shared/_SelectLanguagePartial.cshtml
  :language: none
  
The *Views/Shared/_SelectLanguagePartial.cshtml* file is added to the ``footer`` section of the layout file so it will be available to all views:

 *Views/Shared/_SelectLanguagePartial.cshtml* 文件添加到布局文件的 ``footer`` 区域，因此将提供给所有视图使用：

.. literalinclude:: localization/sample/Views/Shared/_Layout.cshtml
  :language: HTML
  :lines: 48-61
  :dedent: 6
    
The ``SetLanguage`` method sets the culture cookie.

``SetLanguage`` 方法设置文化 cookie。

.. literalinclude:: localization/sample/Controllers/HomeController.cs
  :language: c#
  :lines: 57-67
  :dedent: 6
  
You can't simply plug in the *_SelectLanguagePartial.cshtml* to sample code for this project. The **Localization.StarterWeb** project on `GitHub <https://github.com/aspnet/entropy>`__ has code to flow the
``RequestLocalizationOptions`` to a Razor partial through the :doc:`/fundamentals/dependency-injection` container.

你不能简单的把 *_SelectLanguagePartial.cshtml* 应用到示例代码项目。在 `GitHub <https://github.com/aspnet/entropy>`__ 上 **Localization.StarterWeb** 项目有相关代码通过依赖注入容器把 ``RequestLocalizationOptions`` 注入到 Razor 局部模版。 

Globalization and localization terms
---------------------------------------

本地化全球化术语
---------------------------------------

The process of localizing your app also requires a basic understanding of relevant character sets commonly used in modern software development and an understanding of the issues associated with them. Although all computers store text as numbers (codes), different systems store the same text using different numbers. The localization process refers to translating the app user interface (UI) for a specific culture/locale. 

本地化您的应用程序的过程也需要对现代软件开发中常用的相关字符集的有一个基本的了解，并熟悉与之相关的问题。尽管所有的计算机把文本存储为数字（编码），不同的系统使用不同的数字存储相同的文本。本地化进程是指代通过指定的文化/区域设置来翻译应用程序的用户界面（UI）。

`Localizability <https://msdn.microsoft.com/en-us/library/aa292135(v=vs.71).aspx>`__ is an intermediate process for verifying that a globalized app is ready for localization.

`Localizability <https://msdn.microsoft.com/en-us/library/aa292135(v=vs.71).aspx>`__ 是用于验证一个全球化的应用程序已经准备好本地化的一个即时流程。

The `RFC 4646 <https://www.ietf.org/rfc/rfc4646.txt>`__ format for the culture name is "<languagecode2>-<country/regioncode2>", where <languagecode2> is the language code and <country/regioncode2> is the subculture code. For example, ``es-CL`` for Spanish (Chile), ``en-US`` for English (United States), and ``en-AU`` for English (Australia). `RFC 4646 <https://www.ietf.org/rfc/rfc4646.txt>`__ is a combination of an ISO 639 two-letter lowercase culture code associated with a language and an ISO 3166 two-letter uppercase subculture code associated with a country or region.  See `Language Culture Name <https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx>`__.

区域性名称的 `RFC 4646 <https://www.ietf.org/rfc/rfc4646.txt>`__ 格式为 "<languagecode2>-<country/regioncode2>" ，其中 <languagecode2> 是语言代码， <country/regioncode2> 是子文化代码。例如， ``es-CL`` 西班牙语（智利）， ``en-US`` 是指 英语（美国）， ``en-AU`` 则是英语（澳大利亚）。 `RFC 4646 <https://www.ietf.org/rfc/rfc4646.txt>`__ 是用与语言相关的ISO 639双字母小写区域性代码和一个与国家或地区相关的ISO3166双字母大写子代码组合。详见 `Language Culture Name <https://msdn.microsoft.com/en-us/library/ee825488(v=cs.20).aspx>`__。

Internationalization is often abbreviated to "I18N". The abbreviation takes the first and last letters and the number of letters between them, so 18 stands for the number of letters between the first "I" and the last "N". The same applies to Globalization (G11N), and Localization (L10N).

国际化通常缩写为 "I18N"。缩写采取首字母和尾字母以及它们之间的字母数，所以 18 代表首字母 "I" 和尾字母 "N" 之间的字母数。这同样适用于全球化（G11N）和本地化（L10N）。

Terms:

- Globalization (G11N): The process of making an app support different languages and regions.
- Localization (L10N): The process of customizing an app for a given language and region.
- Internationalization (I18N): Describes both globalization and localization.
- Culture: It is a language and, optionally, a region.
- Neutral culture: A culture that has a specified language, but not a region. (for example "en", "es")
- Specific culture: A culture that has a specified language and region. (for example "en-US", "en-GB", "es-CL")
- Locale: A locale is the same as a culture. 

术语：

- Globalization（全球化） (G11N)：让你的应用程序支持多种语言和区域设置。
- Localization（本地化） (L10N)：让你的应用程序支持某一种特定语言/区域设置。
- Internationalization（国际化） (I18N)：是全球化和本地化的结合。
- Culture（文化）：指代语言和可选地区。
- Neutral culture（非特定区域文化）：指代某种语言，不包含区域。(如 "en", "es")
- Specific culture（特定区域文化）：指代某种语言和区域的组合。(如 "en-US", "en-GB", "es-CL")
- Locale（区域设置）：区域设置和文件是相同的。

Additional Resources
---------------------

- `Localization.StarterWeb project <https://github.com/aspnet/entropy>`__ used in the article.
- `Resource Files in Visual Studio <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#VSResFiles>`__
- `Resources in .resx Files <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#ResourcesFiles>`__

附录资源
---------------------

- 文中使用的的 `Localization.StarterWeb 项目 <https://github.com/aspnet/entropy>`__ 
- `VS中的资源文件 <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#VSResFiles>`__
- `.resx文件中的资源 <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#ResourcesFiles>`__ 文中使用的的 `Localization.StarterWeb 项目 <https://github.com/aspnet/entropy>`__ 
- `VS中的资源文件 <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#VSResFiles>`__
- `.resx文件中的资源 <https://msdn.microsoft.com/en-us/library/xbx3z216(v=vs.110).aspx#ResourcesFiles>`__
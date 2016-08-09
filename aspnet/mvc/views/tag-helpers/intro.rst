Introduction to Tag Helpers
=========================================

By `Rick Anderson`_ 

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

Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. For example, the built-in `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/ImageTagHelper/index.html>`__ can append a version number to the image name. Whenever the image changes, the server generates a new unique version for the image, so clients are guaranteed to get the current image (instead of a stale cached image). There are many built-in Tag Helpers for common tasks - such as creating forms, links, loading assets and more - and even more available in public GitHub repositories and as NuGet packages.
Tag Helpers are authored in C#, and they target HTML elements based on element name, attribute name, or parent tag. For example, the built-in `LabelTagHelper  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/LabelTagHelper/index.html>`__ can target the HTML ``<label>`` element when the ``LabelTagHelper`` attributes are applied. 
If you're familiar with `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__, Tag Helpers reduce the explicit transitions between HTML and C# in Razor views. `Tag Helpers compared to HTML Helpers`_ explains the differences in more detail.

What Tag Helpers provide
------------------------------------

**An HTML-friendly development experience**
 For the most part, Razor markup using Tag Helpers looks like standard HTML. Front-end designers conversant with HTML/CSS/JavaScript can edit Razor without learning C# Razor syntax.

**A rich IntelliSense environment for creating HTML and Razor markup**
 This is in sharp contrast to HTML Helpers, the previous approach to server-side creation of markup in Razor views. `Tag Helpers compared to HTML Helpers`_ explains the differences in more detail. `IntelliSense support for Tag Helpers`_ explains the IntelliSense environment. Even developers experienced with Razor C# syntax are more productive using Tag Helpers than writing C# Razor markup. 
 
**A way to make you more productive and able to produce more robust, reliable, and maintainable code using information only available on the server**
 For example, historically the mantra on updating images was to change the name of the image when you change the image. Images should be aggressively cached for performance reasons, and unless you change the name of an image, you risk clients getting a stale copy. Historically, after an image was edited, the name had to be changed and each reference to the image in the web app needed to be updated. Not only is this very labor intensive, it's also error prone (you could miss a reference, accidentally enter the wrong string, etc.) The built-in `ImageTagHelper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNetCore/Mvc/TagHelpers/ImageTagHelper/index.html>`__ can do this for you automatically. The ``ImageTagHelper`` can append a version number to the image name, so whenever the image changes, the server automatically generates a new unique version for the image. Clients are guaranteed to get the current image. This robustness and labor savings comes essentially free by using the ``ImageTagHelper``.  

Most of the built-in Tag Helpers target existing HTML elements and provide server-side attributes for the element. For example, the ``<input>`` element used in many of the views in the *Views/Account* folder contains the ``asp-for`` attribute, which extracts the name of the specified model property into the rendered HTML. The following Razor markup:

.. code-block:: html

   <label asp-for="Email"></label>

Generates the following HTML:

.. code-block:: html

   <label for="Email">Email</label>
   
The ``asp-for`` attribute is made available by the ``For`` property in the ``LabelTagHelper``. See :doc:`authoring` for more information.

Managing Tag Helper scope
-----------------------------

Tag Helpers scope is controlled by a combination of ``@addTagHelper``, ``@removeTagHelper``, and the "!" opt-out character.

.. _add-helper-label:

``@addTagHelper`` makes Tag Helpers available
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

If you create a new ASP.NET Core web app named *AuthoringTagHelpers* (with no authentication), the following *Views/_ViewImports.cshtml* file will be added to your project:

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImportsCopy.cshtml
   :language: html
   :emphasize-lines: 2
   :lines: 1-2

The ``@addTagHelper`` directive makes Tag Helpers available to the view. In this case, the view file is *Views/_ViewImports.cshtml*, which by default is inherited by all view files in the *Views* folder and sub-directories; making Tag Helpers available. The code above uses the wildcard syntax ("*") to specify that all Tag Helpers in the specified assembly (*Microsoft.AspNetCore.Mvc.TagHelpers*) will be available to every view file in the *Views* directory or sub-directory. The first parameter after ``@addTagHelper`` specifies the Tag Helpers to load (we are using "\*" for all Tag Helpers), and the second parameter "Microsoft.AspNetCore.Mvc.TagHelpers" specifies the assembly containing the Tag Helpers. *Microsoft.AspNetCore.Mvc.TagHelpers* is the assembly for the built-in ASP.NET Core Tag Helpers.
   
To expose all of the Tag Helpers in this project (which creates an assembly named *AuthoringTagHelpers*), you would use the following:

.. literalinclude:: authoring/sample/AuthoringTagHelpers/src/AuthoringTagHelpers/Views/_ViewImportsCopy.cshtml
   :language: html
   :emphasize-lines: 3

If your project contains an ``EmailTagHelper`` with the default namespace (``AuthoringTagHelpers.TagHelpers.EmailTagHelper``), you can provide the fully qualified name (FQN) of the Tag Helper:

.. FQN syntax

.. code-block:: html
   :emphasize-lines: 3
   
    @using AuthoringTagHelpers
    @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
    @addTagHelper "AuthoringTagHelpers.TagHelpers.EmailTagHelper, AuthoringTagHelpers"

To add a Tag Helper to a view using an FQN, you first add the FQN (``AuthoringTagHelpers.TagHelpers.EmailTagHelper``), and then the assembly name (*AuthoringTagHelpers*). Most developers prefer to use the  "\*" wildcard syntax. The wildcard syntax allows you to insert the wildcard character "\*" as the suffix in an FQN. For example, any of the following directives will bring in the ``EmailTagHelper``:

.. code-block:: c#

    @addTagHelper "AuthoringTagHelpers.TagHelpers.E*, AuthoringTagHelpers"
    @addTagHelper "AuthoringTagHelpers.TagHelpers.Email*, AuthoringTagHelpers"

As mentioned previously, adding the ``@addTagHelper`` directive to the *Views/_ViewImports.cshtml* file makes the Tag Helper available to all view files in the *Views* directory and sub-directories. You can use the ``@addTagHelper`` directive in specific view files if you want to opt-in to exposing the Tag Helper to only those views.

.. _remove-razor-directives-label:

``@removeTagHelper`` removes Tag Helpers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``@removeTagHelper`` has the same two parameters as ``@addTagHelper``, and it removes a Tag Helper that was previously added. For example, ``@removeTagHelper`` applied to a specific view removes the specified Tag Helper from the view. Using ``@removeTagHelper`` in a *Views/Folder/_ViewImports.cshtml* file removes the specified Tag Helper from all of the views in *Folder*.

Controlling Tag Helper scope with the *_ViewImports.cshtml* file
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can add a *_ViewImports.cshtml* to any view folder, and the view engine adds the directives from that *_ViewImports.cshtml* file to those contained in the *Views/_ViewImports.cshtml* file. If you added an empty *Views/Home/_ViewImports.cshtml* file for the *Home* views, there would be no change because the *_ViewImports.cshtml* file is additive. Any ``@addTagHelper`` directives you add to the *Views/Home/_ViewImports.cshtml* file (that are not in the default *Views/_ViewImports.cshtml* file) would expose those Tag Helpers to views only in the *Home* folder.

Opting out of individual elements
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can disable a Tag Helper at the element level with the Tag Helper opt-out character ("!"). For example, ``Email`` validation is disabled in the ``<span>`` with the Tag Helper opt-out character:

.. code-block:: c#

    <!span asp-validation-for="Email" class="text-danger"></!span>

You must apply the Tag Helper opt-out character to the opening and closing tag. (The Visual Studio editor automatically adds the opt-out character to the closing tag when you add one to the opening tag). After you add the opt-out character, the element and Tag Helper attributes are no longer displayed in a distinctive font.

.. _prefix-razor-directives-label:

Using ``@tagHelperPrefix`` to make Tag Helper usage explicit
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``@tagHelperPrefix`` directive allows you to specify a tag prefix string to enable Tag Helper support and to make Tag Helper usage explicit. In the code image below, the Tag Helper prefix is set to ``th:``, so only those elements using the prefix ``th:`` support Tag Helpers (Tag Helper-enabled elements have a distinctive font). The ``<label>`` and ``<input>`` elements have the Tag Helper prefix and are Tag Helper-enabled, while the ``<span>`` element does not.

.. image:: intro/_static/thp.png 

.. comment for next version:Note: Quotes are optional with the ``@tagHelperPrefix`` directive. The following two directives are equivalent: 
.. comment code-block:: html
   @tagHelperPrefix th:
   @tagHelperPrefix "th:"

The same hierarchy rules that apply to ``@addTagHelper`` also apply to ``@tagHelperPrefix``.

IntelliSense support for Tag Helpers
----------------------------------------

When you create a new ASP.NET web app in Visual Studio, it adds "Microsoft.AspNetCore.Razor.Tools" to the *project.json* file. This is the package that adds Tag Helper tooling. 

Consider writing an HTML ``<label>`` element. As soon as you enter ``<l`` in the Visual Studio editor, IntelliSense displays matching elements:

.. image:: intro/_static/label.png 

Not only do you get HTML help, but the icon (the "@" symbol with "<>" under it).

.. image:: intro/_static/tagSym.png 

identifies the element as targeted by Tag Helpers. Pure HTML elements (such as the ``fieldset``) display the "<>"icon. 

A pure HTML ``<label>`` tag displays the HTML tag (with the default Visual Studio color theme) in a brown font, the attributes in red, and the attribute values in blue.

.. image:: intro/_static/LableHtmlTag.png 

After you enter ``<label``, IntelliSense lists the available HTML/CSS attributes and the Tag Helper-targeted attributes:

.. image:: intro/_static/labelattr.png

IntelliSense statement completion allows you to enter the tab key to complete the statement with the selected value:

.. image:: intro/_static/stmtcomplete.png

As soon as a Tag Helper attribute is entered, the tag and attribute fonts change. Using the default Visual Studio "Blue" or "Light" color theme, the font is bold purple. If you're using the "Dark" theme the font is bold teal. The images in this document were taken using the default theme.

.. image:: intro/_static/labelaspfor2.png

You can enter the Visual Studio *CompleteWord* shortcut (Ctrl +spacebar is the `default <https://msdn.microsoft.com/en-us/library/da5kh0wa.aspx>`__) inside the double quotes (""), and you are now in C#, just like you would be in a C# class. IntelliSense displays all the methods and properties on the page model. The methods and properties are available because the property type is ``ModelExpression``. In the image below, I'm editing the ``Register`` view, so the ``RegisterViewModel`` is available.

.. image:: intro/_static/intellemail.png

IntelliSense lists the properties and methods available to the model on the page. The rich IntelliSense environment helps you select the CSS class:

.. image:: intro/_static/iclass.png

.. image:: intro/_static/intel3.png

Tag Helpers compared to HTML Helpers
---------------------------------------------

Tag Helpers attach to HTML elements in Razor views, while `HTML Helpers <http://stephenwalther.com/archive/2009/03/03/chapter-6-understanding-html-helpers>`__ are invoked as methods interspersed with HTML in Razor views. Consider the following Razor markup, which creates an HTML label with the CSS class "caption":

.. code-block:: html

    @Html.Label("FirstName", "First Name:", new {@class="caption"})

The at (``@``) symbol tells Razor this is the start of code. The next two parameters ("FirstName" and "First Name:") are strings, so `IntelliSense <https://msdn.microsoft.com/en-us/library/hcw1s69b.aspx>`_ can't help. The last argument:

.. code-block:: html

  new {@class="caption"}
  
Is an anonymous object used to represent attributes. Because **class** is a reserved keyword in C#, you use the ``@`` symbol to force C# to interpret "@class=" as a symbol (property name). To a front-end designer (someone familiar with HTML/CSS/JavaScript and other client technologies but not familiar with C# and Razor), most of the line is foreign. The entire line must be authored with no help from IntelliSense.
  
Using the ``LabelTagHelper``, the same markup can be written as:

.. image:: intro/_static/label2.png 

With the Tag Helper version, as soon as you enter ``<l`` in the Visual Studio editor, IntelliSense displays matching elements:

.. image:: intro/_static/label.png 

IntelliSense helps you write the entire line. The ``LabelTagHelper`` also defaults to setting the content of the ``asp-for`` attribute value ("FirstName") to "First Name"; It converts camel-cased properties to a sentence composed of the property name with a space where each new upper-case letter occurs. In the following markup:

.. image:: intro/_static/label2.png 

generates:
 
 .. code-block:: html

    <label class="caption" for="FirstName">First Name</label>

The  camel-cased to sentence-cased content is not used if you add content to the ``<label>``. For example:

.. image:: intro/_static/1stName.png

generates:

 .. code-block:: html
 
  <label class="caption" for="FirstName">Name First</label>

The following code image shows the Form portion of the *Views/Account/Register.cshtml* Razor view generated from the legacy ASP.NET 4.5.x MVC template included with Visual Studio 2015.

.. image:: intro/_static/regCS.png 

The Visual Studio editor displays C# code with a grey background. For example, the ``AntiForgeryToken`` HTML Helper:

.. code-block:: html

    @Html.AntiForgeryToken()
 
is displayed with a grey background. Most of the markup in the Register view is C#. Compare that to the equivalent approach using Tag Helpers:
 
.. image:: intro/_static/regTH.png 

The markup is much cleaner and easier to read, edit, and maintain than the HTML Helpers approach. The C# code is reduced to the minimum that the server needs to know about. The Visual Studio editor displays markup targeted by a Tag Helper in a distinctive font. 

Consider the *Email* group:

.. literalinclude:: intro/sample/Register.cshtml
   :language: c#
   :lines: 12-18
   :dedent: 4

Each of the "asp-" attributes has a value of "Email", but "Email" is not a string. In this context, "Email" is the C# model expression property for the ``RegisterViewModel``. 


The Visual Studio editor helps you write **all** of the markup in the Tag Helper approach of the register form, while Visual Studio provides no help for most of the code in the HTML Helpers approach. `IntelliSense support for Tag Helpers`_ goes into detail on working with Tag Helpers in the Visual Studio editor.

Tag Helpers compared to Web Server Controls
-----------------------------------------------

- Tag Helpers don't own the element they're associated with; they simply participate in the rendering of the element and content. ASP.NET `Web Server controls <https://msdn.microsoft.com/en-us/library/7698y1f0.aspx>`__ are declared and invoked on a page.

- `Web Server controls <https://msdn.microsoft.com/en-us/library/zsyt68f1.aspx>`__ have a non-trivial lifecycle that can make developing and debugging difficult. 

- Web Server controls allow you to add functionality to the client Document Object Model (DOM) elements by using a client control. Tag Helpers have no DOM. 

- Web Server controls include automatic browser detection. Tag Helpers have no knowledge of the browser.

- Multiple Tag Helpers can act on the same element (see `Avoiding Tag Helper conflicts <http://mvc.readthedocs.org/en/latest/views/tag-helpers/authoring.html#avoiding-tag-helper-conflicts>`__ ) while you typically can't compose Web Server controls.

- Tag Helpers can modify the tag and content of HTML elements that they're scoped to, but don't directly modify anything else on a page. Web Server controls have a less specific scope and can perform actions that affect other parts of your page; enabling unintended side effects. 


- Web Server controls use type converters to convert strings into objects. With Tag Helpers, you work natively in C#, so you don't need to do type conversion. 

- Web Server controls use `System.ComponentModel <https://msdn.microsoft.com/en-us/library/system.componentmodel%28v=vs.110%29.aspx>`__ to implement the run-time and design-time behavior of components and controls. ``System.ComponentModel`` includes the base classes and interfaces for implementing attributes and type converters, binding to data sources, and licensing components. Contrast that to Tag Helpers, which typically derive from ``TagHelper``, and the ``TagHelper`` base class exposes only two methods, ``Process`` and ``ProcessAsync``.

Customizing the Tag Helper element font
---------------------------------------------

You can customize the font and colorization from **Tools** > **Options** > **Environment** > **Fonts and Colors**:

.. image:: intro/_static/fontoptions2.png


   
Additional Resources
----------------------

- :doc:`/mvc/views/tag-helpers/authoring`
- :doc:`Working with Forms (Tag Helpers) </mvc/views/tag-helpers/index>`
- `TagHelperSamples on GitHub <https://github.com/dpaquette/TagHelperSamples>`__ contains Tag Helper samples for working with `Bootstrap <http://getbootstrap.com/>`__. 


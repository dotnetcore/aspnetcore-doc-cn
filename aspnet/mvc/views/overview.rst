Views Overview
==============

By `Steve Smith`_

ASP.NET MVC Core controllers can return formatted results using *views*.

.. contents:: Sections
  :local:
  :depth: 1

What are Views?
---------------

In the Model-View-Controller (MVC) pattern, the *view* encapsulates the presentation details of the user's interaction with the app. Views are HTML templates with embedded code that generate content to send to the client. Views use :doc:`Razor syntax <razor>`, which allows code to interact with HTML with minimal code or ceremony.

ASP.NET Core MVC views are *.cshtml* files stored by default in a *Views* folder within the application. Typically, each controller will have its own folder, in which are views for specific controller actions. 

.. image:: overview/_static/views_solution_explorer.png
  :height: 100
  :width: 200

In addition to action-specific views, :doc:`partial views <partial>`, :doc:`layouts, and other special view files <layout>` can be used to help reduce repetition and allow for reuse within the app's views.

Benefits of Using Views
-----------------------

Views provide `separation of concerns <http://deviq.com/separation-of-concerns/>`_ within an MVC app, encapsulating user interface level markup separately from business logic. ASP.NET MVC views use :doc:`Razor syntax <razor>` to make switching between HTML markup and server side logic painless. Common, repetitive aspects of the app's user interface can easily be reused between views using :doc:`layout and shared directives <layout>` or :doc:`partial views <partial>`.

Creating a View
---------------

Views that are specific to a controller are created in the *Views/[ControllerName]* folder. Views that are shared among controllers are placed in the */Views/Shared* folder. Name the view file the same as its associated controller action, and add the *.cshtml* file extension. For example, to create a view for the *About* action on the *Home* controller, you would create the *About.cshtml* file in the */Views/Home* folder.

A sample view file (*About.cshtml*):

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Views/Home/About.cshtml
   :language: html

*Razor* code is denoted by the ``@`` symbol. C# statements are run within Razor code blocks set off by curly braces (``{`` ``}``), such as the assignment of "About" to the ``ViewData["Title"]`` element shown above. Razor can be used to display values within HTML by simply referencing the value with the ``@`` symbol, as shown within the ``<h2>`` and ``<h3>`` elements above.

This view focuses on just the portion of the output for which it is responsible. The rest of the page's layout, and other common aspects of the view, are specified elsewhere. Learn more about :doc:`layout and shared view logic <layout>`.

How do Controllers Specify Views?
---------------------------------

Views are typically returned from actions as a :dn:cls:`~Microsoft.AspNetCore.Mvc.ViewResult`. Your action method can create and return a ``ViewResult`` directly, but more commonly if your controller inherits from :dn:cls:`~Microsoft.AspNetCore.Mvc.Controller`, you'll simply use the ``View`` helper method, as this example demonstrates:

*HomeController.cs*

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Controllers/HomeController.cs
  :language: c#
  :lines: 16-21
  :emphasize-lines: 5
  :dedent: 4
  
The ``View`` helper method has several overloads to make returning views easier for app developers. You can optionally specify a view to return, as well as a model object to pass to the view.

When this action returns, the *About.cshtml* view shown above is rendered:

.. image:: overview/_static/about-page.png

View Discovery
^^^^^^^^^^^^^^

When an action returns a view, a process called *view discovery* takes place. This process determines which view file will be used. Unless a specific view file is specified, the runtime looks for a controller-specific view first, then looks for matching view name in the *Shared* folder.

When an action returns the ``View`` method, like so ``return View();``, the action name is used as the view name. For example, if this were called from an action method named "Index", it would be equivalent to passing in a view name of "Index". A view name can be explicitly passed to the method (``return View("SomeView");``). In both of these cases, view discovery searches for a matching view file in:

  1. Views/<ControllerName>/<ViewName>.cshtml
  2. Views/Shared/<ViewName>.cshtml
  
.. tip:: We recommend following the convention of simply returning ``View()`` from actions when possible, as it results in more flexible, easier to refactor code.

A view file path can be provided, instead of a view name. In this case, the *.cshtml* extension must be specified as part of the file path. The path should be relative to the application root (and can optionally start with "/" or "~/"). For example: ``return View("Views/Home/About.cshtml");``

.. note:: :doc:`Partial views <partial>` and :doc:`view components <view-components>` use similar (but not identical) discovery mechanisms.

.. note:: You can customize the default convention regarding where views are located within the app by using a custom :dn:iface:`~Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander`.

.. tip:: View names may be case sensitive depending on the underlying file system. For compatibility across operating systems, always match case between controller and action names and associated view folders and filenames.

Passing Data to Views
---------------------

You can pass data to views using several mechanisms. The most robust approach is to specify a *model* type in the view (commonly referred to as a *viewmodel*, to distinguish it from business domain model types), and then pass an instance of this type to the view from the action. We recommend you use a model or view model to pass data to a view. This allows the view to take advantage of strong type checking. You can specify a model for a view using the ``@model`` directive:

.. code-block:: html
    :emphasize-lines: 1

    @model WebApplication1.ViewModels.Address
    <h2>Contact</h2>
    <address>
        @Model.Street<br />
        @Model.City, @Model.State @Model.PostalCode<br />
        <abbr title="Phone">P:</abbr>
        425.555.0100
    </address>

Once a model has been specified for a view, the instance sent to the view can be accessed in a strongly-typed manner using ``@Model`` as shown above. To provide an instance of the model type to the view, the controller passes it as a parameter:

.. code-block:: c#
    :emphasize-lines: 13
   
    public IActionResult Contact()
    {
        ViewData["Message"] = "Your contact page.";

        var viewModel = new Address()
        {
            Name = "Microsoft",
            Street = "One Microsoft Way",
            City = "Redmond",
            State = "WA",
            PostalCode = "98052-6399"
        };
        return View(viewModel);
    }

There are no restrictions on the types that can be provided to a view as a model. We recommend passing Plain Old CLR Object (POCO) view models with little or no behavior, so that business logic can be encapsulated elsewhere in the app. An example of this approach is the *Address* viewmodel used in the example above:

.. code-block:: c#
    :emphasize-lines: 13
   
    namespace WebApplication1.ViewModels
    {
        public class Address
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
        }
    }

.. note:: Nothing prevents you from using the same classes as your business model types and your display model types. However, keeping them separate allows your views to vary independently from your domain or persistence model, and can offer some security benefits as well (for models that users will send to the app using :doc:`model binding </mvc/models/model-binding>`).

Loosely Typed Data
^^^^^^^^^^^^^^^^^^

In addition to strongly typed views, all views have access to a loosely typed collection of data. This same collection can be referenced through either the ``ViewData`` or ``ViewBag`` properties on controllers and views. The ``ViewBag`` property is a wrapper around ``ViewData`` that provides a dynamic view over that collection. It is not a separate collection.

``ViewData`` is a dictionary object accessed through ``string`` keys. You can store and retrieve objects in it, and you'll need to cast them to a specific type when you extract them. You can use ``ViewData`` to pass data from a controller to views, as well as within views (and partial views and layouts). String data can be stored and used directly, without the need for a cast.

Set some values for ``ViewData`` in an action:

.. code-block:: c#

    public IActionResult SomeAction()
    {
        ViewData["Greeting"] = "Hello";
        ViewData["Address"]  = new Address()
        {
            Name = "Steve",
            Street = "123 Main St",
            City = "Hudson",
            State = "OH",
            PostalCode = "44236"
        };
        
        return View();
    }

Work with the data in a view:

.. code-block:: html
    :emphasize-lines: 3,6
   
    @{
        // Requires cast
        var address = ViewData["Address"] as Address;
    }

    @ViewData["Greeting"] World!

    <address>
        @address.Name<br />
        @address.Street<br />
        @address.City, @address.State @address.PostalCode
    </address>

The ``ViewBag`` objects provides dynamic access to the objects stored in ``ViewData``. This can be more convenient to work with, since it doesn't require casting. The same example as above, using ``ViewBag`` instead of a strongly typed ``address`` instance in the view:

.. code-block:: html
    :emphasize-lines: 1,4-6

    @ViewBag.Greeting World!

    <address>
        @ViewBag.Address.Name<br />
        @ViewBag.Address.Street<br />
        @ViewBag.Address.City, @ViewBag.Address.State @ViewBag.Address.PostalCode
    </address>

.. note:: Since both refer to the same underlying ``ViewData`` collection, you can mix and match between ``ViewData`` and ``ViewBag`` when reading and writing values, if convenient.

Dynamic Views
^^^^^^^^^^^^^

Views that do not declare a model type but have a model instance passed to them can reference this instance dynamically. For example, if an instance of ``Address`` is passed to a view that doesn't declare an ``@model``, the view would still be able to refer to the instance's properties dynamically as shown:

.. code-block:: html
    :emphasize-lines: 13,16-18
    
    <address>
        @Model.Street<br />
        @Model.City, @Model.State @Model.PostalCode<br />
        <abbr title="Phone">P:</abbr>
        425.555.0100
    </address>

This feature can offer some flexibility, but does not offer any compilation protection or IntelliSense. If the property doesn't exist, the page will fail at runtime.

More View Features
------------------

:doc:`Tag helpers <tag-helpers/intro>` make it easy to add server-side behavior to existing HTML tags, avoiding the need to use custom code or helpers within views. Tag helpers are applied as attributes to HTML elements, which are ignored by editors that aren't familiar with them, allowing view markup to be edited and rendered in a variety of tools. Tag helpers have many uses, and in particular can make :doc:`working with forms <working-with-forms>` much easier.

Generating custom HTML markup can be achieved with many built-in :doc:`HTML Helpers <html-helpers>`, and more complex UI logic (potentially with its own data requirements) can be encapsulated in :doc:`view-components`. View components provide the same separation of concerns that controllers and views offer, and can eliminate the need for actions and views to deal with data used by common UI elements.

Like many other aspects of ASP.NET Core, views support :doc:`dependency injection </fundamentals/dependency-injection>`, allowing services to be :doc:`injected into views <dependency-injection>`.

Model Binding 模型绑定
======================

作者： `Rachel Appel`_ 

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `凌军州 <#>`_ 、`何镇汐 <#>`_

.. contents:: Sections 章节:
  :local:
  :depth: 1

Introduction to model binding 
-----------------------------

模型绑定介绍
------------

Model binding in MVC maps data from HTTP requests to action method parameters. The parameters may be simple types such as strings, integers, or floats, or they may be complex types. This is a great feature of MVC because mapping incoming data to a counterpart is an often repeated scenario, regardless of size or complexity of the data. MVC solves this problem by abstracting binding away so developers don't have to keep rewriting a slightly different version of that same code in every app. Writing your own text to type converter code is tedious, and error prone. 

MVC 中的模型绑定从 HTTP 请求参数中将数据映射到 Action 方法里。这些参数可以是 string， interger， float 这样的简单类型，也可以是复杂类型。这是 MVC 的一个非常棒功能，因为无论传入数据的大小或复杂性如何，映射传入数据到对应项是一个经常重复的情况。 MVC 通过抽象绑定解决了这个问题，所以开发者们不必继续在每一个应用中反复编写同样的代码。自己编写文本到类型的转换代码是冗长并且容易出错的。

How model binding works
-----------------------

模型绑定是如何工作的
--------------------

When MVC receives an HTTP request, it routes it to a specific action method of a controller. It determines which action method to run based on what is in the route data, then it binds values from the HTTP request to that action method's parameters. For example, consider the following URL:

当 MVC 收到一个 HTTP 请求，它将其路由到一个 Controller 下特定的 Action 。它基于路由数据来决定运行哪个 Action 方法，然后将值从 HTTP 请求绑定到 Action 方法的参数中。例如，考虑以下URL：


`http://contoso.com/movies/edit/2`

Since the route template looks like this, ``{controller=Home}/{action=Index}/{id?}``, ``movies/edit/2`` routes to the ``Movies`` controller, and its ``Edit`` action method. It also accepts an optional parameter called ``id``. The code for the action method should look something like this: 

因为路由模板看起来像这样，``{controller=Home}/{action=Index}/{id?}``， ``movies/edit/2`` 路由到 ``Movies`` Controller 中的 ``Edit`` Action 方法。同时接受到一个可选参数 ``id`` 。 Action 方法代码应该看起来像这样：

.. code-block:: c#
  :linenos:
   
  public IActionResult Edit(int? id)
   
.. note:: The strings in the URL route are not case sensitive. 

.. note:: URL 中的字符串不区分大小写。

MVC will try to bind request data to the action parameters by name. MVC will look for values for each parameter using the parameter name and the names of its public settable properties. In the above example, the only action parameter is named ``id``, which MVC binds to the value with the same name in the route values. In addition to route values MVC will bind data from various parts of the request and it does so in a set order. Below is a list of the data sources in the order that model binding looks through them:

MVC 尝试通过参数名将请求数据绑定到 Action 的参数上。 MVC 将查询所有的参数名( HTTP 请求中的)和可写属性名称相同的(不区分大小写)。在上面的例子中，只有一个参数命名为 ``id`` ， MVC 将路由值中名称相同的值绑定过去。除了路由数据之外， MVC 会以一种固定的顺序从 HTTP 请求中的其他部分绑定数据。下面是模型绑定的数据源列表的绑定顺序：
 
#. ``Form values``: These are form values that go in the HTTP request using the POST method. (including jQuery POST requests).
#. ``Route values``: The set of route values provided by `routing <https://docs.asp.net/projects/mvc/en/latest/controllers/routing.html>`_. 
#. ``Query strings``: The query string part of the URI.

分割线----------

#. ``Form values``: 这是通过 HTTP POST 请求发送的表单数据(包括 jQuery POST 请求)。
#. ``Route values``: 路由数据集由 `路由 <https://docs.asp.net/projects/mvc/en/latest/controllers/routing.html>`_ 提供。
#. ``Query strings``: URI 的查询字符串的一部分。

.. note:: Form values, route data, and query strings are all stored as name-value pairs.

.. note:: 表单值，路由数据，以及查询字符串都以键值对的形式存储。

Since model binding asked for a key named ``id`` and there is nothing named ``id`` in the form values, it moved on to the route values looking for that key. In our example, it's a match. Binding happens, and the value is converted to the integer 2. The same request using Edit(string id) would convert to the string "2". 

因为模型绑定要找一个命名为 ``id`` 的键，但是在表单数据里没有命名为 ``id`` 的键，所以接下来将在路由数据中找寻这个键。在我们的例子中，从路由数据中找到后并将值转换成 interger 类型的值 2 进行绑定。相同的请求定义为 Edit(string id) 将转换成 string 类型的值 "2" 。

So far the example uses simple types. In MVC simple types are any .NET primitive type or type with a string type converter. If the action method's parameter were a class such as the ``Movie`` type, which contains both simple and complex types as properties, MVC's model binding will still handle it nicely. It uses reflection and recursion to traverse the properties of complex types looking for matches. Model binding looks for the pattern parameter_name.property_name to bind values to properties. If it doesn't find matching values of this form, it will attempt to bind using just the property name. For those types such as ``Collection`` types, model binding looks for matches to `parameter_name[index]` or just `[index]`. Model binding treats  ``Dictionary`` types similarly, asking for `parameter_name[key]` or just `[key]`, as long as they keys are simple types. Keys that are supported match the field names HTML and tag helpers generated for the same model type. This enables round-tripping values so that the form fields remain filled with the user's input for their convenience, for example, when bound data from a create or edit did not pass validation.

到目前为止的例子使用的都是简单类型。在 MVC 中简单类型是任何 .NET 原始类型或者带字符串的类型的转换器。如果 Action 方法的参数是一个类，比如说 ``Movie`` 类型，这个类包含简单类型和复杂类型的属性， MVC 的模型绑定仍然可以很好的处理它。它使用反射和递归遍历复杂类型寻找匹配的属性。模型绑定寻找 `parameter_name.parameter_name` 的规律去绑定值到属性上。如果没有从表单中找到匹配的值，将尝试只通过 `property_name` 进行绑定。对于那些 ``集合(Collection)`` 类型，模型绑定会去匹配 `parameter_name[index]` 或者只是 `[index]` 。模型绑定对待 ``字典(Dictionary)`` 类型也是一样，寻找 `parameter_name[key]` 或只是 `[key]` ，前提是他们的 Key 是简单类型。 Key 支持匹配 HTML 和 Tag Helpers 为相同的模型类型生成的字段名。当创建或者编辑的绑定数据未通过验证的时候，回传值使得用户输入的表单字段仍然保留，方便了用户(不必重新输入全部数据)。

In order for binding to happen the class must have a public default constructor and member to be bound must be public writable properties. When model binding happens the class will only be instantiated using the public default constructor, then the properties can be set.

为了绑定发生，这个类必须有一个 public 的默认构造函数，并且被绑定的成员必须是 public 并且可写的属性。当模型绑定发生的时候只会通过默认的构造函数去实例化类型，然后设置属性的值。

When a parameter is bound, model binding stops looking for values with that name and it moves on to bind the next parameter. If binding fails, MVC does not throw an error. You can query for model state errors by checking the ``ModelState.IsValid`` property. 

当一个参数被绑定，模型绑定停止继续查找这个参数名并开始绑定下一个参数。如果绑定失败， MVC 不会抛出异常。你可以查询模型状态异常通过检查 ``ModelState.IsValid`` 属性。

.. Note:: Each entry in the controller's ``ModelState`` property is a ``ModelStateEntry`` containing an ``Errors property``. It's rarely necessary to query this collection yourself. Use ``ModelState.IsValid`` instead. 

.. Note:: Controller里的 ``ModelState`` 属性中的每个 Entry 都是一个包含了 ``Errors 属性`` 的 ``ModelStateEntry`` 。 你基本不需要去查询这个集合.使用 ``ModelState.IsValid`` 来替代它。

Additionally, there are some special data types that MVC must consider when performing model binding:

此外，还有一些特殊的数据类型在 MVC 执行模型绑定的时候需要考虑：

- ``IFormFile``, ``IEnumerable<IFormFile>``: One or more uploaded files that are part of the HTTP request.
- ``CancelationToken``: Used to cancel activity in asynchronous controllers.

- ``IFormFile``, ``IEnumerable<IFormFile>``: 一个或多个通过 HTTP 请求上传的文件。
- ``CancelationToken``: 用于在异步 Controller 中取消活动。

These types can be bound to action parameters or to properties on a class type.

这些类型可以被绑定到 Action 参数或者一个类的属性中

Once model binding is complete, `validation <https://docs.asp.net/projects/mvc/en/latest/models/validation.html>`_ occurs. Default model binding works great for the vast majority of development scenarios. It is also extensible so if you have unique needs you can customize the built-in behavior.  

一旦模型绑定完成。就会进行 `验证 <https://docs.asp.net/projects/mvc/en/latest/models/validation.html>`_ 。默认的模型绑定适合绝大多数开发场景。它也是可扩展的，所以如果您有独特的需求，您可以自定义内置的行为。

Customize model binding behavior with attributes
------------------------------------------------

通过 Attributes 自定义模型绑定行为
----------------------------------

MVC contains several attributes that you can use to direct its default model binding behavior to a different source. For example, you can specify whether binding is required for a property, or if it should never happen at all by using the ``[BindRequired]`` or ``[BindNever]`` attributes. Alternatively, you can override the default data source, and specify the model binder's data source. Below is a list of model binding attributes:

MVC 包含几种让你可以指定与默认绑定源不同行为的 Attribute 。比如，你可以通过使用 ``[BindRequired]`` 或者 ``[BindNever]`` Attribute 指定一个属性是否需要绑定，或者它是否应该不发生。另外你可以替换默认的数据源，指定模型绑定器(Model Binder)的数据源。下面的是模型绑定 Attribute 的列表：

- ``[BindRequired]``: This attribute adds a model state error if binding cannot occur.
- ``[BindNever]``: Tells the model binder to never bind to this parameter.
- ``[FromHeader]``, ``[FromQuery]``, ``[FromRoute]``, ``[FromForm]``: Use these to specify the exact binding source you want to apply.
- ``[FromServices]``: This attribute uses :doc:`dependency injection </fundamentals/dependency-injection>` to bind parameters from services.
- ``[FromBody]``: Use the configured formatters to bind data from the request body. The formatter is selected based on content type of the request.
- ``[ModelBinder]``: Used to override the default model binder, binding source and name.

- ``[BindRequired]``: 这个 Attribute 表示如果这个绑定不能发生，将添加一个模型状态错误(Model State Error) 。
- ``[BindNever]``: 告诉模型绑定器(Model Binder)这个参数不进行绑定。
- ``[FromHeader]``, ``[FromQuery]``, ``[FromRoute]``, ``[FromForm]``: 通过这些来指定期望的绑定源。
- ``[FromServices]``: 这个 Attribute 使用 :doc:`dependency injection </fundamentals/dependency-injection>` 通过服务来绑定参数。
- ``[FromBody]``: 使用配置好的格式化器来 从 HTTP 请求 Body 中绑定数据。格式化器的选择基于 HTTP 请求的 Content-Type
- ``[ModelBinder]``: 用来替换默认的模型绑定器(Model Binder)，绑定源和名字。

Attributes are very helpful tools when you need to override the default behavior of model binding.
当你需要替换模型绑定的默认行为时， Attribute 是非常有用的工具。

Binding formatted data from the request body
--------------------------------------------

从 Http Request 的 body 中绑定格式化数据
----------------------------------------

Request data can come in a variety of formats including JSON, XML and many others. When you use the [FromBody] attribute to indicate that you want to bind a parameter to data in the request body, MVC uses a configured set of formatters to handle the request data based on its content type. By default MVC includes a ``JsonInputFormatter`` class for handling JSON data, but you can add additional formatters for handling XML and other custom formats. 

HTTP 请求数据能够支持各种各样的格式，包括 JSON 、 XML 以及许多其它的格式。当你使用 [FromBody] 特性的时候表示你想要从 HTTP 请求的 Body 中绑定参数， MVC 使用一个格式化器的配置集来处理与 HTTP 请求的 Content-Type 对应的请求数据。默认情况下 MVC 包含一个 ``JsonInputFormatter`` 类用来处理 JSON 数据，但是你可以添加额外的格式化器来处理 XML 或者其它自定义格式。

.. Note:: The ``JsonInputFormatter`` is the default formatter and it is based off of `Json.NET <http://www.newtonsoft.com/json>`_.

.. Note:: ``JsonInputFormatter`` 是默认的格式化器，它是基于 `Json.NET <http://www.newtonsoft.com/json>`_.

ASP.NET selects input formatters based on the `Content-Type <https://www.w3.org/Protocols/rfc1341/4_Content-Type.html>`_ header and the type of the parameter, unless there is an attribute applied to it specifying otherwise. If you'd like to use XML or another format you must configure it in the `Startup.cs` file, but you may first have to obtain a reference to ``Microsoft.AspNet.Mvc.Formatters.Xml`` using NuGet. Your startup code should look something like this:

ASP.NET 选择输入格式化器基于 `Content-Type <https://www.w3.org/Protocols/rfc1341/4_Content-Type.html>`_ Header 以及参数的类型，除非这里有一个 Attribute 去指定其它的。如果你更愿意去使用 XML 或者其他格式，你必须在 `Startup.cs` 文件中进行配置，但是也许你首先必须通过 NuGet 引用 ``Microsoft.AspNet.Mvc.Formatters.Xml`` 。你的启动代码看起来应该像这样：


.. code-block:: c#
  :linenos:
   
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddMvc()
         .AddXmlSerializerFormatters();
  }

Code in the `Startup.cs` file contains a ``ConfigureServices`` method with a ``services`` argument you can use to build up services for your ASP.NET app. In the sample, we are adding an XML formatter as a service that MVC will provide for this app. The ``options`` argument passed into the ``AddMvc`` method allows you to add and manage filters, formatters, and other system options from MVC upon app startup. Then apply the ``Consumes`` attribute to controller classes or action methods to work with the format you want. 

`Startup.cs` 文件中的代码包含了一个带有 ``services`` 参数的 ``ConfigureServices`` 方法，你可以使用它来为你的 ASP.NET 应用构建服务。在示例中，我们添加一个 XML 格式化器作为一个在此应用中 MVC 能够提供的的服务。 ``options`` 参数传入 ``AddMvc`` 方法允许你去添加和管理过滤器( Filter )，格式化器( Formatter ),以及其它 MVC 的系统选项从应用中启动。然后应用 ``各种各样的`` Attribute 到 Controller 类或者 Action 方法上去实现你预期的效果。

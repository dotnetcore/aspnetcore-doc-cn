Model Validation 模型验证
=========================
作者： `Rachel Appel <http://github.com/rachelappel>`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `孟帅洋 <#>`_

In this article:

在这篇文章中：

.. contents:: Sections 节
  :local:
  :depth: 1

Introduction to model validation
--------------------------------

介绍模型验证
------------

Before an app stores data in a database, the app must validate the data. Data must be checked for potential security threats, verified that it is appropriately formatted by type and size, and it must conform to your rules. Validation is necessary although it can be redundant and tedious to implement. In MVC, validation happens on both the client and server. 

在一个应用程序将数据存储到数据库之前，这个应用程序必须验证数据。数据必须检查潜在的安全隐患，验证类型和大小是正确并且符合你所制定的规则。尽管验证的实现可能会是冗余和繁琐的，却是有必要的。在 MVC 中，验证发生在客户端和服务器端。

Fortunately, .NET has abstracted validation into validation attributes. These attributes contain validation code, thereby reducing the amount of code you must write. 

幸运地是， .Net 有一些拥有抽象验证的验证 Attribute 。这些 Attribute 包含验证代码，从而减少你必须写的代码量。

Validation Attributes
---------------------

验证 Attribute
--------------

Validation attributes are a way to configure model validation so it's similar conceptually to validation on fields in database tables. This includes constraints such as assigning data types or required fields. Other types of validation include applying patterns to data to enforce business rules, such as a credit card, phone number, or email address. Validation attributes make enforcing these requirements much simpler and easier to use.

验证 Attribute 是一种配置模型验证的方法，类似在数据库表中验证字段的概念。它包含了指定数据类型或者必填字段等约束。其它类型的验证包括将强制的业务规则应用到数据验证，比如验一个信用卡号，一个手机号码，或者一个 Email 地址。 验证 Attribute 使这些要求更简单，更容易使用。

Below is an annotated ``Movie`` model from an app that stores information about movies and TV shows. Most of the properties are required and several string properties have length requirements. Additionally, there is a numeric range restriction in place for the ``Price`` property from 0 to $999.99, along with a custom validation attribute.

下面是一个存储了电影和电视节目信息的应用程序中被注解的 ``Movie`` 模型。大部分属性是必填的，几个字符串类型的属性有长度限制。此外，在 ``Price`` 属性上通过自定义验证 Attribute 实现了 0 到 $999.99 的数字范围限制。

.. literalinclude:: validation/sample/Movie.cs
   :language: c#
   :lines: 6-31
   :dedent: 4

Simply reading through the model reveals the rules about data for this app, making it easier to maintain the code. Below are several popular built-in validation attributes:

简单地通过阅读模型了解了这个应用程序的数据规则，(这种编码方式)让维护代码变得更简单。一下是几个常用的内置验证 Attribute ：

- ``[CreditCard]``: Validates the property has a credit card format.
- ``[Compare]``: Validates two properties in a model match. 
- ``[EmailAddress]``: Validates the property has an email format.
- ``[Phone]``: Validates the property has a telephone format.
- ``[Range]``: Validates the property value falls within the given range.
- ``[RegularExpression]``: Validates that the data matches the specified regular expression.
- ``[Required]``: Makes a property required.
- ``[StringLength]``: Validates that a string property has at most the given maximum length.
- ``[Url]``: Validates the property has a URL format.

- ``[CreditCard]``: 验证属性是信号卡号格式。
- ``[Compare]``: 验证模型中的两个属性匹配。 
- ``[EmailAddress]``: 验证属性是 Email 格式。
- ``[Phone]``: 验证属性是 电话号码 格式。
- ``[Range]``: 验证属性在指定的范围内。
- ``[RegularExpression]``: 验证数据匹配指定的正则表达式。
- ``[Required]``: 使属性成为必填。
- ``[StringLength]``: 验证字符串类型属性的最大长度。
- ``[Url]``: 验证属性是 URL 格式。

MVC supports any attribute that derives from ``ValidationAttribute`` for validation purposes. Many useful validation attributes can be found in the `System.ComponentModel.DataAnnotations <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations(v=vs.110).aspx>`_ namespace.

MVC 支持任何为了验证目的而从 ``ValidationAttribute`` 继承的 Attribute 。需要有用的验证 Attribute 可以在 `System.ComponentModel.DataAnnotations <https://msdn.microsoft.com/zh-cn/library/system.componentmodel.dataannotations(v=vs.110).aspx>`_ 命名空间下找到。

There may be instances where you need more features than built-in attributes provide. For those times, you can create custom validation attributes by deriving from ``ValidationAttribute`` or changing your model to implement ``IValidatableObject``.

可能在某些情况下，你需要使用比内置 Attribute 更多的验证功能。在那时，你可以通过创建继承自 ``ValidationAttribute`` 的自定义验证 Attribute 或者修改你的模型去实现 ``IValidatableObject`` 接口。

Model State
-----------

模型状态
--------

Model state represents validation errors in submitted HTML form values.

模型状态表示在 HTML 表单提交值的一系列验证错误。

MVC will continue validating fields until reaches the maximum number of errors (200 by default). You can configure this number by inserting the following code into the ``ConfigureServices`` method in the ``Startup.cs`` file:

MVC 将持续验证字段直到错误数达到最大值(默认200)。你可以通过在 ``Startup.cs`` 文件下的 ``ConfigureServices`` 方法中插入以下代码来配置这个最大值：

.. literalinclude:: validation/sample/Startup.cs
   :language: c#
   :lines: 27
   :dedent: 12

Handling Model State Errors
---------------------------

处理模型状态异常
---------------------------

Model validation occurs prior to each controller action being invoked, and it is the action method’s responsibility to inspect ModelState.IsValid and react appropriately. In many cases, the appropriate reaction is to return some kind of error response, ideally detailing the reason why model validation failed.

模型验证发生在每个控制器（Controller）的行为（Action）被调用之前，而检查 ModelState.IsValid 和做出适当的反应是行为（Action）方法的职责。在许多情况下，适当的反映是返回某种错误响应，理想情况下详细介绍了模型验证失败的原因。

Some apps will choose to follow a standard convention for dealing with model validation errors, in which case a filter may be an appropriate place to implement such a policy. You should test how your actions behave with valid and invalid model states.

一些应用程序将选择遵循一个标准的惯例来处理模型验证错误，在这种情况下，过滤器可能是一个适当的方式来实现这种策略。你需要分别用有效和无效的模型状态来测试 Action 的行为。


Manual validation
-----------------

手动验证
--------

After model binding and validation are complete, you may want to repeat parts of it. For example, a user may have entered text in a field expecting an integer, or you may need to compute a value for a model's property. 

当模型绑定和验证完成后，你也许想重复其中的部分操作。例如，用户可能输入了一个被期望为 integer 类型的字段的文本，或者你需要为模型中的一个属性计算一个值。

You may need to run validation manually. To do so, call the ``TryValidateModel`` method, as shown here: 

你需要手动去执行验证。像这样，调用 ``TryValidateModel`` 方法：

.. literalinclude:: validation/sample/MoviesController.cs
   :language: c#
   :lines: 52
   :dedent: 12
   
Custom validation
-----------------

自定义验证
----------

Validation attributes work for most validation needs. However, some validation rules are specific to your business, as they're not just generic data validation such as ensuring a field is required or that it conforms to a range of values. For these scenarios, custom validation attributes are a great solution. Creating your own custom validation attributes in MVC is easy. Just inherit from the ``ValidationAttribute``, and override the ``IsValid`` method. The ``IsValid`` method accepts two parameters, the first is an object named `value` and the second is a ``ValidationContext`` object named `validationContext`. `Value` refers to the actual value from the field that your custom validator is validating.

验证 Attribute 满足大多数的验证需求。然而你的业务存在一些特殊的验证规则，它们不仅仅是通用的数据验证，如确保字段必填或者符合一个值的范围之类的。对于这些情况，自定义验证 Attribute 是一个不错的解决方案。在 MVC 中创建你自己的自定义验证 Attribute 是非常容易的。只需要继承 ``ValidationAttribute`` 并且重写 ``IsValid`` 方法。 ``IsValid`` 方法接受两个参数，第一个是命名为 `value` 的 object 对象，第二个参数是一个命名为 `validationContext` 的 ``ValidationContext`` 对象。 `Value` 指的是你的自定义验证器验证的字段的值。

In the following sample, a business rule states that users may not set the genre to `Classic` for a movie released after 1960. The ``[ClassicMovie]`` attribute checks the genre first, and if it is a classic, then it checks the release date to see that it is later than 1960. If it is released after 1960, validation fails. The attribute accepts an integer parameter representing the year that you can use to validate data. You can capture the value of the parameter in the attribute's constructor, as shown here:

在下面的示例中，一个业务规则规定，用户可能不会将在1960年之后发布的电影的 `Genre` 设置为 `Classic`。``[ClassicMovie]`` Attribute 首先检查 `Genre` ，如果它是 `Genre.Classic` ，接下来检查电影发布日期是否晚于1960年。如果发布晚于1960年，验证失败。这个 Attribute 接受一个 integer 类型的参数作为验证数据的年份。你可以在这个 Attribute 的构造函数中对这个值进行赋值，如同这里显示的：
					 
.. literalinclude:: validation/sample/ClassicMovieAttribute.cs
   :language: c#
   :lines: 9-28
   :dedent: 4
   
The ``movie`` variable above represents a ``Movie`` object that contains the data from the form submission to validate. In this case, the validation code checks the date and genre in the ``IsValid`` method of the ``ClassicMovieAttribute`` class as per the rules. Upon successful validation ``IsValid`` returns a ``ValidationResult.Success`` code, and when validation fails, a ``ValidationResult`` with an error message. When a user modifies the ``Genre`` field and submits the form, the ``IsValid`` method of the ``ClassicMovieAttribute`` will verify whether the movie is a classic. Like any built-in attribute, apply the ``ClassicMovieAttribute`` to a property such as ``ReleaseDate`` to ensure validation happens, as shown in the previous code sample. Since the example works only with ``Movie`` types, a better option is to use ``IValidatableObject`` as shown in the following paragraph.

上面的 ``movie`` 变量代表一个包含了表单提交数据并等待验证的 ``Movie`` 的对象。在这个例子中，``ClassicMovieAttribute`` 类的 ``IsValid`` 方法按照规定检查了日期和分类( Genre )。当验证成功， ``IsValid`` 方法返回一个 ``ValidationResult.Success`` 枚举码；当验证失败，返回一个带有错误消息的 ``ValidationResult`` 。当用户修改了 ``Genre`` 字段并且提交表单， ``ClassicMovieAttribute`` 中的 ``IsValid`` 方法将验证电影是否是经典( Classic )。如同其他内置的 Attribute 一样，应用 ``ClassicMovieAttribute`` 到比如 ``ReleaseDate`` 这个属性上来确保验证发生，如果之前例子中的演示代码一样。因为这个例子仅对 ``Movie`` 类型有效，一个更好的选择使用下面段落介绍的 ``IValidatableObject``。

Alternatively, this same code could be placed in the model by implementing the ``Validate`` method on the ``IValidatableObject`` interface. While custom validation attributes work well for validating individual properties, implementing ``IValidatableObject`` can be used to implement class-level validation as seen here.

另外，相同的代码可以放在模型里，通过去实现 ``IValidatableObject`` 接口中的 ``Validate`` 方法。当自定义验证 Attribute 能够很好的验证各个属性时，实现 ``IValidatableObject`` 接口可以用来实现类等级(Class-Level)的验证，如下。

.. literalinclude:: validation/sample/MovieIValidatable.cs
   :language: c#
   :lines: 33-41
   :dedent: 8
  
Client side validation
----------------------

客户端验证
----------

Client side validation is a great convenience for users. It saves time they would otherwise spend waiting for a round trip to the server. In business terms, even a few fractions of seconds multiplied hundreds of times each day adds up to be a lot of time, expense, and frustration. Straightforward and immediate validation enables users to work more efficiently and produce better quality input and output. 

客户端验证为客户带了极大的便利。它可以节省时间而不用花费一个来回时间等待服务器的验证结果。在业务角度来看，一天中哪怕是几秒乘以数百次，都会增加很多工作时间、开支以及挫败感。直接和即时的验证，使用户能够更有效地工作，得到质量更好的投入和产出。

You must have a view with the proper JavaScript script references in place for client side validation to work as you see here. 

你必须适当的引用 JavaScript 脚本来进行客户端验证，如下。

.. literalinclude:: validation/sample/Views/Shared/_Layout.cshtml
   :language: html
   :lines: 37
   :dedent: 4
.. literalinclude:: validation/sample/Views/Shared/_ValidationScriptsPartial.cshtml
   :language: html

MVC uses validation attributes in addition to type metadata from model properties to validate data and display any error messages using JavaScript. When you use MVC to render form elements from a model using `Tag Helpers <https://docs.asp.net/en/latest/mvc/views/tag-helpers/index.html>`_ or `HTML helpers <https://docs.asp.net/en/latest/mvc/views/html-helpers.html>`_ it will add HTML 5 `data- attributes <http://w3c.github.io/html/dom.html#embedding-custom-non-visible-data-with-the-data-attributes>`_ in the form elements that need validation, as shown below. MVC generates the ``data-`` attributes for both built-in and custom attributes. You can display validation errors on the client using the relevant tag helpers as shown here:

除了模型属性的类型元数据外，MVC还是用验证 Attribute 通过 JavaScript 验证数据并展示所有错误信息。当你使用 MVC 去渲染使用 `Tag Helpers <https://docs.asp.net/en/latest/mvc/views/tag-helpers/index.html>`_ 或者 `HTML helpers <https://docs.asp.net/en/latest/mvc/views/html-helpers.html>`_ 的表单数据之时，它将在需要验证的表单元素中添加 HTML 5 `data- attributes <http://w3c.github.io/html/dom.html#embedding-custom-non-visible-data-with-the-data-attributes>`_ ，如同下面看到的。 MVC 对所有内置验证 Attribute 和自定义验证 Attribute 生成 ``data-`` 特性。你可以通过相关的 Tag Helper 在客户端显示验证错误，如同这里展示的：

.. literalinclude:: validation/sample/Views/Movies/Create.cshtml
   :language: html
   :lines: 19-25
   :dedent: 8
   :emphasize-lines: 4-5

The tag helpers above render the HTML below. Notice that the ``data-`` attributes in the HTML output correspond to the validation attributes for the ``ReleaseDate`` property. The ``data-val-required`` attribute below contains an error message to display if the user doesn't fill in the release date field, and that message displays in the accompanying ``<span>`` element.

上面的 Tag Helper 渲染的 HTML 如下。 注意输出的 HTML 中 ``data-`` 特性对应 ``ReleaseDate`` 属性的验证 Attribute。下面的 ``data-val-required`` 特性包含一个用于展示的错误消息，如果用户没有填写 ReleaseDate 字段，错误消息将随着 ``<span>`` 元素一起显示。

.. code-block:: html
  :emphasize-lines: 8-12

  <form action="/movies/Create" method="post">
    <div class="form-horizontal">
      <h4>Movie</h4>
      <div class="text-danger"></div>
      <div class="form-group">
        <label class="col-md-2 control-label" for="ReleaseDate">ReleaseDate</label>
        <div class="col-md-10">
          <input class="form-control" type="datetime"
          data-val="true" data-val-required="The ReleaseDate field is required."
          id="ReleaseDate" name="ReleaseDate" value="" />
          <span class="text-danger field-validation-valid"
          data-valmsg-for="ReleaseDate" data-valmsg-replace="true"></span>
        </div>
      </div>
      </div>
  </form>
            
Client-side validation prevents submission until the form is valid. The Submit button runs JavaScript that either submits the form or displays error messages. 

客户端验证防止表单提交直到有效为止。无论提交表单还是显示错误消息，提交按钮都会执行 JavaScript 代码。

MVC determines type attribute values based on the .NET data type of a property, possibly overridden using ``[DataType]`` attributes. The base ``[DataType]`` attribute does no real server-side validation. Browsers choose their own error messages and display those errors however they wish, however the jQuery Validation Unobtrusive package can override the messages and display them consistently with others. This happens most obviously when users apply ``[DataType]`` subclasses such as ``[EmailAddress]``.

MVC 基于 .NET 属性的数据类型决定类型特性值，可以使用 ``[DataType]`` Attribute 来覆盖。基础的 ``[DataType]`` Attribute 并不是真正的服务端认证。浏览器选择它们自己的错误消息并按照它们希望的那样显示这些错误，然而 jQuery Validation Unobtrusive 包可以重写这些消息并且让他们显示方式保持一致。当用户应用 ``[DataType]`` 的子类比如 ``[EmailAddress]`` 的时候，这种情况最明显。


IClientModelValidator
---------------------

客户端模型验证器
----------------

You may create client side logic for your custom attribute, and `unobtrusive validation <http://jqueryvalidation.org/documentation/>`_ will execute it on the client for you automatically as part of validation. The first step is to control what data- attributes are added by implementing the ``IClientModelValidator`` interface as shown here:  

你也许会为你的自定义 Attribute 创建客户端逻辑，`unobtrusive validation <http://jqueryvalidation.org/documentation/>`_ 会在客户端将它作为验证的一部分自动执行。第一步
是向下面一样，通过实现 ``IClientModelValidator`` 接口来控制那些被添加的 data- 特性：

.. literalinclude:: validation/sample/ClassicMovieAttribute.cs
   :language: c#
   :lines: 30-42
   :dedent: 8
 
Attributes that implement this interface can add HTML attributes to generated fields. Examining the output for the ``ReleaseDate`` element reveals HTML that is similar to the previous example, except now there is a ``data-val-classicmovie`` attribute that was defined in the ``AddValidation`` method of ``IClientModelValidator``.

Attribute 实现这个接口后可以添加 HTML 特性到生成的字段。检查输出的 HTML 中的 ``ReleaseDate`` 元素，和上一个例子差不多，除了通过 ``IClientModelValidator`` 接口的 ``AddValidation`` 方法定义了一个 ``data-val-classicmovie`` 特性。

.. code-block:: html

  <input class="form-control" type="datetime"
  data-val="true"
  data-val-classicmovie="Classic movies must have a release year earlier than 1960"
  data-val-classicmovie-year="1960"
  data-val-required="The ReleaseDate field is required."
  id="ReleaseDate" name="ReleaseDate" value="" />

Unobtrusive validation uses the data in the ``data-`` attributes to display error messages. However, jQuery doesn't know about rules or messages until you add them to jQuery's ``validator`` object. This is shown in the example below that adds a method named ``classicmovie`` containing custom client validation code to the jQuery ``validator`` object. 

Unobtrusive validation 使用 ``data-`` 特性中的数据来显示错误消息。然而 JQuery 在你添加 JQuery 的 ``validator`` 对象之前是不知道规则和消息的。在显示在下面的例子中将一个包含自定义客户端验证代码的命名为 ``classicmovie`` 的方法添加到 JQuery 的 ``validator`` 对象中。

.. literalinclude:: validation/sample/Views/Movies/Create.cshtml
   :language: javascript
   :lines: 71-93
   :dedent: 4

Now jQuery has the information to execute the custom JavaScript validation as well as the error message to display if that validation code returns false. 

现在 JQuery 拥有执行自定义 JavaScript 验证以及当验证代码返回 false 时用来显示的错误消息的信息了。

Remote validation
-----------------

远程验证
--------

Remote validation is a great feature to use when you need to validate data on the client against data on the server. For example, your app may need to verify whether an email or user name is already in use, and it must query a large amount of data to do so. Downloading large sets of data for validating one or a few fields consumes too many resources. It may also expose sensitive information. An alternative is to make a round-trip request to validate a field.

当你需要在客户端上使用服务器上的数据进行验证的时候，远程验证是一个很棒的功能。比如，你的应用程序也许需要验证一个 Email 或者用户名是否已经被使用，这样做必须查询大量的数据。为了验证一个或几个字段下载大量的数据，消耗了过多的资源。并且可能会暴露敏感信息。另一个办法是使用回传请求来验证字段。

You can implement remote validation in a two step process. First, you must annotate your model with the ``[Remote]`` attribute. The ``[Remote]`` attribute accepts multiple overloads you can use to direct client side JavaScript to the appropriate code to call. The example points to the ``VerifyEmail`` action method of the ``Users`` controller. 

你可以用两个步骤实现远程验证。首先，你需要用 ``[Remote]`` Attribute 注解你的模型。``[Remote]`` Attribute 接受多个重载可以直接使用客户端 JavaScript 到适当的代码来调用。下面的例子指向 ``Users`` Controller 的 ``VerifyEmail`` Action 。

.. literalinclude:: validation/sample/User.cs
   :language: c#
   :lines: 5-9
   :dedent: 4
 
The second step is putting the validation code in the corresponding action method as defined in the ``[Remote]`` attribute. It returns a ``JsonResult`` that the client side can use to proceed or pause and display an error if needed.

第二步是将验证代码放到 ``[Remote]`` Attribute 中定义的相应 Action 方法中。Action 方法返回一个 ``JsonResult`` ，如果需要，客户端可以用来继续或者暂停并显示错误。
 
.. literalinclude:: validation/sample/UsersController.cs
   :language: none
   :lines: 19-28
   :dedent: 8
 
Now when users enter an email, JavaScript in the view makes a remote call to see if that email has been taken, and if so, then displays the error message. Otherwise, the user can submit the form as usual. 

现在当用户输入一个 Email ，View 中的 JavaScript 进行远程调用来检查 Email 是否被占用，如果被占用就显示错误消息。否则，用户可以和往常一样提交表单。

创建复杂数据模型
=============================

翻译： `刘怡(AlexLEWIS/Forerunner) <http://github.com/alexinea>`_

校对：

The Contoso University sample web application demonstrates how to create ASP.NET Core 1.0 MVC web applications using Entity Framework Core 1.0 and Visual Studio 2015. For information about the tutorial series, see :doc:`the first tutorial in the series </data/ef-mvc/intro>`.

在 Contoso University 示例 Web 应用程序中我们将演示如何使用 Entity Framework Core 1.0 和 Visual Studio 2015 来创建 ASP.NET Core 1.0 MVC Web 应用程序。

In the previous tutorials you worked with a simple data model that was composed of three entities. In this tutorial you'll add more entities and relationships and you'll customize the data model by specifying formatting, validation, and database mapping rules.

在前几篇教程中我们已经接触到由三个实体组成的简单数据模型。本教程中会增加更多的实体，你将接触到它们之间的关系、定制指定格式的数据模型、验证与数据库映射规则。

When you're finished, the entity classes will make up the completed data model that's shown in the following illustration:

当你完成这些之后，实体类将组成完整的数据模型，如下图所示：

.. image:: complex-data-model/_static/diagram.png
   :alt: Entity diagram

.. contents:: Sections:
  :local:
  :depth: 1

通过特性定制数据模型
--------------------------------------------

In this section you'll see how to customize the data model by using attributes that specify formatting, validation, and database mapping rules. Then in several of the following sections you'll create the complete School data model by adding attributes to the classes you already created and creating new classes for the remaining entity types in the model.

本节中你能学到如何通过利用特性（Attributes）指定格式、验证和数据库映射规则来定制数据模型。然后在后续几节中，你将通过给既有的或新创建的类增加特性的方式创建完整的 School 数据模型。

DataType 特性
^^^^^^^^^^^^^^^^^^^^^^

For student enrollment dates, all of the web pages currently display the time along with the date, although all you care about for this field is the date. By using data annotation attributes, you can make one code change that will fix the display format in every view that shows the data. To see an example of how to do that, you'll add an attribute to the ``EnrollmentDate`` property in the ``Student`` class.

对于学生入学日期，所有的网页都将时间和日期放在一起显示，尽管你只是考虑这个字段是一个日期。通过使用数据标注特性（data annotation attributes），你可以通过一段代码来改变所有视图中所展示的数据格式。看完如何使用数据标注特性的例子之后，你可以在 ``Student`` 类的 ``EnrollmentDate`` 的属性上增加一个特性了。

In *Models/Student.cs*, add a ``using`` statement for the ``System.ComponentModel.DataAnnotations`` namespace and add ``DataType`` and ``DisplayFormat`` attributes to the ``EnrollmentDate`` property, as shown in the following example:

在 *Model/Student.cs* 中用 ``using`` 声明添加 ``System.ComponentModel.DataAnnotations`` 命名空间，然后给 ``EnrollmentDate`` 属性添加 ``DataType`` 特性和 ``DisplayFormat`` 特性，如下例所示：

.. literalinclude::  intro/samples/cu/Models/Student.cs
  :language: c#
  :start-after: snippet_DataType
  :end-before:  #endregion
  :emphasize-lines: 3, 12-13

The ``DataType`` attribute is used to specify a data type that is more specific than the database intrinsic type. In this case we only want to keep track of the date, not the date and time. The  ``DataType`` Enumeration provides for many data types, such as Date, Time, PhoneNumber, Currency, EmailAddress, and more. The ``DataType`` attribute can also enable the application to automatically provide type-specific features. For example, a ``mailto:`` link can be created for ``DataType.EmailAddress``, and a date selector can be provided for ``DataType.Date`` in browsers that support HTML5. The ``DataType`` attribute emits HTML 5 ``data-`` (pronounced data dash) attributes that HTML 5 browsers can understand. The ``DataType`` attributes do not provide any validation. 

``DataType`` 特性用于指定比数据库内部类型更为具体的数据类型。在本例中我们只是想跟踪日期，并不关心学生的入学时间。``DataType`` 枚举提供了不少数据类型，例如日期（Date）、时间（Time）、电话号码（PhoneNumber）、货币（Currency）、电子邮件地址（EmailAddress）等。``DataType`` 特性还可以让应用程序自动提供一些类型特有的功能，例如 ``mailto:`` 链接会因为创建了 ``DataType.EmailAddress`` 特性而自动出现，日期选择器会因为使用了 ``DataType.Date`` 特性再出现在浏览器中，而且是支持 HTML5 的。``DataType`` 特性能生成支持 HTML5 的浏览器能理解的带有 ``data-`` 前缀的 HTML5 属性。不过 ``DataType`` 特性不提供任何验证功能。

``DataType.Date`` does not specify the format of the date that is displayed. By default, the data field is displayed according to the default formats based on the server's CultureInfo.

``DataType.Date`` 并不指定显示日期的格式。默认情况下数据字段的默认格式显示是基于服务器的 CultureInfo。

The ``DisplayFormat`` attribute is used to explicitly specify the date format:

``DisplayFormat`` 特性用于显式指定日期格式：

.. code-block:: c#

 [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

The ``ApplyFormatInEditMode`` setting specifies that the formatting should also be applied when the value is displayed in a text box for editing. (You might not want that for some fields -- for example, for currency values, you might not want the currency symbol in the text box for editing.)

``ApplyFormatInEditMode`` 设置指定了在编辑模式时值在文本框中所显示的格式（对于某些字段你可能并不希望如此，比方说对于货币值，你可能并不希望在文本框中修改货币符号）。

You can use the ``DisplayFormat`` attribute by itself, but it's generally a good idea to use the ``DataType`` attribute also. The ``DataType`` attribute conveys the semantics of the data as opposed to how to render it on a screen, and provides the following benefits that you don't get with ``DisplayFormat``:

你可以使用 ``DisplayFormat`` 特性，也可以使用 ``DataType`` 特性。``DataType`` 特性传递的是数据的语义，而不是如何使其显式于屏幕上，而且还提供了你使用 ``DisplayFormat`` 时享受不到的好处：

* The browser can enable HTML5 features (for example to show a calendar control, the locale-appropriate currency symbol, email links, some client-side input validation, etc.).
* 浏览器可以启用 HTML5 特性（比如显示日历控件、适配区域设置的货币单位、邮件链接以及一些客户端输入验证功能等）。
* By default, the browser will render data using the correct format based on your locale.
* 默认情况下，浏览器使用基于你当前区域设置的正确的格式来渲染数据。

For more information, see the `<input> tag helper documentation <https://docs.asp.net/en/latest/mvc/views/working-with-forms.html#the-input-tag-helper>`__.

更多信息请查看 `<input> tag helper 文档 <https://docs.asp.net/en/latest/mvc/views/working-with-forms.html#the-input-tag-helper>`__。

Run the Students Index page again and notice that times are no longer displayed for the enrollment dates. The same will be true for any view that uses the Student model.

再次运行 Student 索引页，注意入学日期这一栏不再显示时间了，这同样会是用于所有使用 Student 模型的任何视图。

.. image:: complex-data-model/_static/dates-no-times.png
   :alt: Students index page showing dates without times

StringLength 特性
^^^^^^^^^^^^^^^^^^^^^^^^^^

You can also specify data validation rules and validation error messages using attributes. The ``StringLength`` attribute sets the maximum length  in the database and provides client side and server side validation for ASP.NET MVC. You can also specify the minimum string length in this attribute, but the minimum value has no impact on the database schema.

你也可以通过特性指定数据验证规则和验证错误消息。``StringLength`` 特性可以设置数据在数据库中的最大长度，并为 ASP.NET MVC 提供客户端和服务端验证。你还可以通过这个特性指定最小字符串长度，但最小长度对于数据库架构来说没有任何影响。

Suppose you want to ensure that users don't enter more than 50 characters for a name. To add this limitation, add ``StringLength`` attributes to the ``LastName`` and ``FirstMidName`` properties, as shown in the following example:

假设你希望确保用户输入的名字不超过 50 个字符，那么可以通过在 ``LastName`` 和 ``FirstMidName`` 属性上添加 ``StringLength`` 特性来实现这个限制，如下所示：

.. literalinclude::  intro/samples/cu/Models/Student.cs
  :language: c#
  :start-after: snippet_StringLength
  :end-before:  #endregion
  :emphasize-lines: 10, 12
  
The ``StringLength`` attribute won't prevent a user from entering white space for a name. You can use the ``RegularExpression`` attribute to apply restrictions to the input. For example the following code requires the first character to be upper case and the remaining characters to be alphabetical:

``StringLength`` 特性并不会阻止用户在名字中输入空格。你需要使用 ``RegularExpression`` 特性来限制输入的内容。比如下例中的代码要求输入的字符必须都是字母，且首字母必须大写：

.. code-block:: none

  [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]

The ``MaxLength`` attribute provides functionality similar to the ``StringLength`` attribute but doesn't provide client side validation.

``MaxLength`` 特性提供了类似于 ``StringLength`` 特性的功能，但不提供客户端验证。

The database model has now changed in a way that requires a change in the database schema. You'll use migrations to update the schema without losing any data that you may have added to the database by using the application UI.

由于数据库模型已经发生变化，所以也需要相应的改编数据库的架构。你需要使用迁移（migration）来升级数据库架构，这样一来就不会丢由应用程序界面添加到库中的任何数据。

Save your changes and build the project. Then open the command window in the project folder and enter the following commands:

保存并生成项目，然后打开项目文件夹的命令窗口并输入如下命令：

.. code-block:: none

  dotnet ef migrations add MaxLengthOnNames -c SchoolContext
  dotnet ef database update -c SchoolContext

The ``migrations add`` command creates a file named `<timeStamp>_MaxLengthOnNames.cs`. This file contains code in the ``Up`` method that will update the database to match the current data model. The ``database update`` command ran that code.

``migrations add`` 命令会创建一个名为 `<timeStamp>_MaxLengthOnNames.cs` 的文件。这个文件中的 ``Up`` 方法将更新数据库使其匹配当前数据模型。使用 ``database update`` 命令来运行这段代码。

The timestamp prefixed to the migrations file name is used by Entity Framework to order the migrations. You can create multiple migrations before running the update-database command, and then all of the migrations are applied in the order in which they were created.

迁移文件的时间戳前缀用于让 Entity Framework 执行迁移时排序。你可以在运行升级数据库命令之前创建多个迁移，然后所有迁移会按它们创建的顺序逐个应用。

Run the Create page, and enter either name longer than 50 characters. When you click Create, client side validation shows an error message.

运行 Create 页面，然后输入名称长度超过 50 个字符的内容。当你点击 Create 按钮时，客户端验证将显示错误消息：

.. image:: complex-data-model/_static/string-length-errors.png
   :alt: Students index page showing string length errors

Column 特性
^^^^^^^^^^^^^^^^^^^^

You can also use attributes to control how your classes and properties are mapped to the database. Suppose you had used the name ``FirstMidName`` for the first-name field because the field might also contain a middle name. But you want the database column to be named ``FirstName``, because users who will be writing ad-hoc queries against the database are accustomed to that name. To make this mapping, you can use the ``Column`` attribute.

你也可以使用特性来控制「类和属性」与「数据库」之间的映射关系。假设你为 first-name 字段准备了个名为 ``FirstMidName`` 属性（因为该字段可能会包含中间名），但你希望你的数据库列的名字是 ``FirstName``，因为用写即席查询（*ad-hoc queries，用户根据自己的需求灵活选择查询条件，系统根据用户的选择生成相应的查询，译者注*）语句的用户可能更习惯于这个名字。那么就可以用 ``Column`` 特性做一个映射（Mapping）即可。

The ``Column`` attribute specifies that when the database is created, the column of the ``Student`` table that maps to the ``FirstMidName`` property will be named ``FirstName``. In other words, when your code refers to ``Student.FirstMidName``, the data will come from or be updated in the ``FirstName`` column of the ``Student`` table. If you don't specify column names, they are given the same name as the property name.

``Column`` 特性指定了在创建数据库时，``FirstMidName`` 属性将映射为 ``Student`` 表的 ``FirstName`` 字段。换而言之，当你的代码引用 ``Student.FirstMidName`` 时，数据将来自或更新到 ``Student`` 表的 ``FirstName`` 字段。如果不指定列名，那么列名将与属性名同名。

In the *Student.cs* file, add a ``using`` statement for ``System.ComponentModel.DataAnnotations.Schema`` and add the column name attribute to the ``FirstMidName`` property, as shown in the following highlighted code:

在 *Student.cs* 文件中利用 ``using`` 声明添加 ``System.ComponentModel.DataAnnotations.Schema`` 命名空间并在 ``FirstMidName`` 属性上添加列名特性，如下所示：

.. literalinclude::  intro/samples/cu/Models/Student.cs
  :language: c#
  :start-after: snippet_Column
  :end-before:  #endregion
  :emphasize-lines: 4, 14

Save your changes and build the project.

保存变更并生成项目。

The addition of the ``Column`` attribute changes the model backing the ``SchoolContext``, so it won't match the database. 

模型在添加 ``Column`` 特性后发生了改变，因此 ``SchoolContext`` 不再匹配数据库。

Save your changes and build the project. Then open the command window in the project folder and enter the following commands to create another migration:

保存变更并生成项目，然后打开项目文件夹的命令窗口并输入以下命令来创建另一个迁移：

.. code-block:: none

  dotnet ef migrations add ColummFirstName -c SchoolContext
  dotnet ef database update -c SchoolContext

In **SQL Server Object Explorer**, open the Student table designer by double-clicking the **Student** table.

在 **SQL Server Object Explorer** 中双击 **Student** 表打开 Student 表设计器。

.. image:: complex-data-model/_static/ssox-after-migration.png
   :alt: Students table in SSOX after migrations

Before you applied the first two migrations, the name columns were of type nvarchar(MAX). They are now nvarchar(50) and the column name has changed from FirstMidName to FirstName.

在你应用前两个迁移之前，名字一列的类型是 nvarchar(MAX)。它们现在变成了 nvarchar(50)，同时列名也从 FirstMidName 变成了 FirstName。

.. note:: If you try to compile before you finish creating all of the entity classes in the following sections, you might get compiler errors.

.. note:: 如果尚未创建完下面章节中的实体类便进行编译，你可能会收到编译器错误。

完成 Student 实体的修改
--------------------------------------

.. image:: complex-data-model/_static/student-entity.png
   :alt: Student entity

In *Models/Student.cs*, replace the code you added earlier with the following code. The changes are highlighted.

在 *Models/Student.cs* 中，把你早期添加的代码改为以下这段代码，注意两段代码之间的区别。

.. literalinclude::  intro/samples/cu/Models/Student.cs
  :language: c#
  :start-after: snippet_BeforeInheritance
  :end-before:  #endregion
  :emphasize-lines: 11, 13, 15, 18, 22, 25-31

Table 特性
^^^^^^^^^^^^^^^^^^^

As you saw in the first tutorial, by default tables are named after the ``DbSet`` property name.  The property name is for a collection, so it is typically plural ("Students"), but many developers and DBAs prefer to use the singular form ("Student") for table names. This attribute specifies the name that EF will use for the table in the database that stores Student entities.

如你所见，在第一个教程中，默认情况下表由 ``DbSet`` 后面的属性名所决定的。属性名是一个集合，所以通常情况下是复数形式（比如「Students」），但许多开发人员以及 DBA 们更倾向于使用单数形式（「Student」）作为表名。本特性便可以指定 EF 将存储学生实体所使用的 Student 名称作为数据库中表的名称。

Required 特性
^^^^^^^^^^^^^^^^^^^^^^
The ``Required`` attribute makes the name properties required fields. The ``Required`` attribute is not needed for non-nullable types such as value types (DateTime, int, double, float, etc.). Types that can't be null are automatically treated as required fields. 

``Required`` 特性让名字属性称为必填字段。``Required`` 特性对于非可空类型来说并非必要，比如对于值类型（DateTime、int、double、float 等）就不是必须的。类型不可为 null 的都会被作为必填字段自动处理。

You could remove the ``Required`` attribute and replace it with a minimum length parameter for the ``StringLength`` attribute:

你可以移除 ``Required`` 特性并用 ``StringLength`` 特性的最小长度参数来代替：

.. code-block:: c#

  [Display(Name = "Last Name")]
  [StringLength(50, MinimumLength=1)]
  public string LastName { get; set; }

Display 特性
^^^^^^^^^^^^^^^^^^^^^

The ``Display`` attribute specifies that the caption for the text boxes should be "First Name", "Last Name", "Full Name", and "Enrollment Date" instead of the property name in each instance (which has no space dividing the words).

``Display`` 特性指定文本框的标题是「First Name」、「Full Name」以及「Enrollment Date」，而不是每个实例中各属性的名称（实例中属性名没有空格来分隔单词）。

FullName 计算属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

``FullName`` is a calculated property that returns a value that's created by concatenating two other properties. Therefore it has only a get accessor, and no ``FullName`` column will be generated in the database. 

``FullName`` 是一个计算属性，它返回两个属性值连接后的结果。因此它只有一个 Getter 访问器，在数据库中也不会生成 FullName 列。

创建 Instructor 实体
----------------------------

.. image:: complex-data-model/_static/instructor-entity.png
   :alt: Instructor entity

Create *Models/Instructor.cs*, replacing the template code with the following code:

创建 *Models/Instructor.cs* 文件，用以下代码代替模板代码：

.. literalinclude::  intro/samples/cu/Models/Instructor.cs
  :start-after: snippet_BeforeInheritance
  :end-before: #endregion
  :language: c#

Notice that several properties are the same in the Student and Instructor entities. In the :doc:`Implementing Inheritance </data/ef-mvc/inheritance>` tutorial later in this series, you'll refactor this code to eliminate the redundancy.

请注意，在 Student 和 Instructor 这两个实体中有多个属性是相同的。在本系列教程的后续教程中有一篇 :doc:`实现继承 </data/ef-mvc/inheritance>`，你将在那篇教程中学习到如何重构这段代码以消除冗余。

You can put multiple attributes on one line, so you could also write the ``HireDate`` attributes as follows:

你可以把多个特性放在同一行上，所以 ``HireDate`` 特性也可以这么写：

.. code-block:: c#

  [DataType(DataType.Date),Display(Name = "Hire Date"),DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

Courses 和 OfficeAssignment 导航属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``Courses`` and ``OfficeAssignment`` properties are navigation properties. 

``Courses`` 和 ``OfficeAssignment`` 属性是都导航属性。

An instructor can teach any number of courses, so ``Courses`` is defined as a collection. 

教师可以教授任何数量的课程，所以 ``Courses`` 被定义为一个集合。

.. code-block:: c#

  public ICollection<InstructorCourse> Courses { get; set; }

If a navigation property can hold multiple entities, its type must be a list in which entries can be added, deleted, and updated.  You can specify ``ICollection<T>`` or a type such as ``List<T>`` or ``HashSet<T>``. If you specify ``ICollection<T>``, EF creates a ``HashSet<T>`` collection by default.

如若导航属性可以连接多个实体，那么这个类型就必然是一个列表，可以对实体进行增删改。你可以指定其类型为 ``ICollection<T>``，或者类似 ``List<T>`` 或 ``HashSet<T>`` 这样的类型。如果你把类型指定为 ``ICollection<T>``，EF 则将默认创建 ``HashSet<T>`` 集合。

The reason why these are ``InstructorCourse`` entities is explained below in the section about many-to-many relationships. 

之所以是 ``InstructorCourse`` 实体的原因将在下文多对多关系一节中加以解释。

Contoso University business rules state that an instructor can only have at most one office, so the ``OfficeAssignment`` property holds a single OfficeAssignment entity (which may be null if no office is assigned).

Contoso University 的业务规则规定，教师最多只能有一个办公室，因此 ``OfficeAssignment`` 属性连接单个 OfficeAssignment 实体（如果没有办公室，该属性可为 null）。

.. code-block:: c#

  public virtual OfficeAssignment OfficeAssignment { get; set; }

创建 OfficeAssignment 实体
----------------------------------

.. image:: complex-data-model/_static/officeassignment-entity.png
   :alt: OfficeAssignment entity

Create *Models/OfficeAssignment.cs* with the following code:

根据以下代码创建 *Models/OfficeAssignment.cs*：

.. literalinclude::  intro/samples/cu/Models/OfficeAssignment.cs
  :language: c#

Key 特性
^^^^^^^^^^^^^^^^^

There's a one-to-zero-or-one relationship  between the Instructor and the OfficeAssignment entities. An office assignment only exists in relation to the instructor it's assigned to, and therefore its primary key is also its foreign key to the Instructor entity. But the Entity Framework can't automatically recognize InstructorID as the primary key of this entity because its name doesn't follow the ID or classnameID naming convention. Therefore, the ``Key`` attribute is used to identify it as the key:

Instructor 实体和 OfficeAssignment 实体之间的关系是一对零或一（one-to-zero-or-one）。办公室的分配仅与分配给它的教师有关，所以对于 Instructor 实体来说它的主键即其外键。但 Entity Framework 不会自动把 InstructorID 识别为该实体的主键，因为其名称并不符合名称约定的「ID」或「类名ID」。所以用 ``Key`` 特性来标识该实体的主键：

.. code-block:: c#

  [Key]
  [ForeignKey("Instructor")]
  public int InstructorID { get; set; }

You can also use the ``Key`` attribute if the entity does have its own primary key but you want to name the property something other than classnameID or ID. 

还有一种使用 ``Key`` 特性的场合，就是当你的实体已经有了自己的主键，但你的实体内还存在其他的属性名为「类型ID」或「ID」的。

By default EF treats the key as non-database-generated because the column is for an identifying relationship.

默认情况下 EF 会将主键视作「非数据库生成（non-database-generated）」，而是将该列用于标识关系。

ForeignKey 特性
^^^^^^^^^^^^^^^^^^^^^^^^

When there is a one-to-zero-or-one relationship or a one-to-one relationship between two entities (such as between OfficeAssignment and Instructor), EF might not be able to work out which end of the relationship is the principal and which end is dependent.  One-to-one relationships have a reference navigation property in each class to the other class. The ``ForeignKey`` attribute can be applied to the dependent class to establish the relationship.

当两实体间存在一对零或一（one-to-zero-or-one）关系或一对一（one-to-one）关系（例如 OfficeAssignment 和 Instructor 之间）时，EF 可能会无法确定关系的哪一端是主体、哪一端是依赖。一对一关系在每个类中都会有一个引用导航属性指向另一个类。``ForeignKey`` 特性在建立关系时可应用于依赖类上。

Instructor 导航属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The Instructor entity has a nullable ``OfficeAssignment`` navigation property (because an instructor might not have an office assignment), and the OfficeAssignment entity has a non-nullable ``Instructor`` navigation property (because an office assignment can't exist without an instructor -- ``InstructorID`` is non-nullable). When an Instructor entity has a related OfficeAssignment entity, each entity will have a reference to the other one in its navigation property.

Instructor 实体有一个可空的 ``OfficeAssignment`` 导航属性（因为有的教师可能没有被分配办公室），OfficeAssignment 实体有一个非可空（non-nullable）的 ``Instructor`` 导航属性（因为被分配的办公室必须有一个教师——``InstructorID`` 是不可为空的）。当 Instructor 实体具有一个与之相关的 OfficeAssignment 实体时，每个实体都会通过其内的导航属性引用另一个。

You could put a ``[Required]`` attribute on the Instructor navigation property to specify that there must be a related instructor, but you don't have to do that because the ``InstructorID`` foreign key (which is also the key to this table) is non-nullable.

你可以在 Instructor 实体的导航属性上放一个 ``[Required]`` 特性用来明确表示此处必须有一个关联的教师，但其实不必这么做，因为外键 ``InstructorID``（同时也是这张表的主键）不可为空。

修改 Course 实体
------------------------

.. image:: complex-data-model/_static/course-entity.png
   :alt: Course entity

In *Models/Course.cs*, replace the code you added earlier with the following code:

用下面这段代码代替你之前在 *Models/Course.cs* 中所增加的：

.. literalinclude::  intro/samples/cu/Models/Course.cs
  :language: c#
  :start-after: snippet_Final
  :end-before:  #endregion
  :emphasize-lines: 2,3,10,16,23

The course entity has a foreign key property ``DepartmentID`` which points to the related Department entity and it has a ``Department`` navigation property. 

Course 实体有一个名为 ``DepartmentID`` 的外键属性，指向与之相关的 Department 实体，并具有一个 ``Department`` 导航属性。

The Entity Framework doesn't require you to add a foreign key property to your data model when you have a navigation property for a related entity.  EF automatically creates foreign keys in the database wherever they are needed and creates `shadow properties <https://ef.readthedocs.io/en/latest/modeling/shadow-properties.html>`__ for them. But having the foreign key in the data model can make updates simpler and more efficient. For example, when you fetch a course entity to edit, the  Department entity is null if you don't load it, so when you update the course entity, you would have to first fetch the Department entity. When the foreign key property ``DepartmentID`` is included in the data model, you don't need to fetch the Department entity before you update.

当你的关联实体内存在导航属性时，Entity Framework 并不强制要求你在数据模型中添加外键属性。EF 会为你自动在数据库中创建外键——无论它们是否需要——并为它们创建 `影子属性（shadow properties） <https://ef.readthedocs.io/en/latest/modeling/shadow-properties.html>`__ 。但在数据模型中使用外键可以使更新更简单高效。比方说当你获取到要编辑的 Course 实体时，如果不去刻意加载的话 Department 实体就是 null，所以当你更新 Course 实体时，你首先需要获取 Department 实体。当你的数据模型中包含了外键属性 ``DepartmentID``，一切就简单了，在你更新数据前你根本不需要去获取 Department 实体。

DatabaseGenerated 特性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The ``DatabaseGenerated`` attribute with the ``None`` parameter on the ``CourseID`` property specifies that primary key values are provided by the user rather than generated by the database.

在 ``CourseID`` 属性上，带有 ``None`` 参数的 ``DatabaseGenerated`` 特性指定的主键值由用户提供，而不是有数据库生成。

.. code-block:: c#

  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  [Display(Name = "Number")]
  public int CourseID { get; set; }

By default, the Entity Framework assumes that primary key values are generated by the database. That's what you want in most scenarios. However, for Course entities, you'll use a user-specified course number such as a 1000 series for one department, a 2000 series for another department, and so on.

默认情况下，Entity Framework 会假设主键值由数据库生成。大多数情况下这也是你所需要的，但对于 Course 实体来讲，你可能会需要由用户来指定课程编号，比如 A 系的编号为 1000 系列的，B 系的编号则是 2000 系列的等等。

The ``DatabaseGenerated`` attribute can also be used to generate default values, as in the case of database columns used to record the date a row was created or updated.  For more information, see `Generated Properties <https://ef.readthedocs.io/en/latest/modeling/generated-properties.html>`__.

``DatabaseGenerated`` 特性也可以用于生成默认值，比如用于记录创建或更新一行记录的时间等。更多信息可以查看 `属性生成 <https://ef.readthedocs.io/en/latest/modeling/generated-properties.html>`__。

外键与导航属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The foreign key properties and navigation properties in the Course entity reflect the following relationships:

在 Course 实体中的外键属性和导航属性反映以下关系：

A course is assigned to one department, so there's a ``DepartmentID`` foreign key and a ``Department`` navigation property for the reasons mentioned above.

每门课程都被分配到一个系下，基于这一点在 Course 实体中存在 ``DepartmentID`` 外键和 ``Department`` 导航属性。

.. code-block:: c#

  public int DepartmentID { get; set; }
  public Department Department { get; set; }

A course can have any number of students enrolled in it, so the ``Enrollments`` navigation property is a collection:

每门课程都可以有任意数量的学生来注册，故 ``Enrollments`` 导航属性是一个集合：

.. code-block:: c#

  public ICollection<Enrollment> Enrollments { get; set; }

A course may be taught by multiple instructors, so the ``Instructors`` navigation property is a collection:

每门课程都可以有多个教师来教授，所以 ``Instructors`` 导航属性也是一个集合：

.. code-block:: c#

  public ICollection<Instructor> Instructors { get; set; }

创建 Department 实体
----------------------------

.. image:: complex-data-model/_static/department-entity.png
   :alt: Department entity 

Create *Models/Department.cs* with the following code:

用下列代码创建 *Models/Department.cs* 文件：

.. literalinclude::  intro/samples/cu/Models/Department.cs
  :language: c#
  :start-after: snippet_Begin
  :end-before:  #endregion

Column 特性
^^^^^^^^^^^^^^^^^^^^

Earlier you used the ``Column`` attribute to change column name mapping. In the code for the Department entity, the ``Column`` attribute is being used to change SQL data type mapping so that the column will be defined using the SQL Server money type in the database:

之前你使用 ``Column`` 特性来改变列名映射。在 Department 实体代码中，``Column`` 特性被用于更改 SQL 数据类型映射，因此此处的列将在数据库中被定义为使用 SQL Server money 类型：

.. code-block:: c#

  [Column(TypeName="money")]
  public decimal Budget { get; set; }

Column mapping is generally not required, because the Entity Framework chooses the appropriate SQL Server data type based on the CLR type that you define for the property. The CLR ``decimal`` type maps to a SQL Server ``decimal`` type. But in this case you know that the column will be holding currency amounts, and the money data type is more appropriate for that.

列映射通常并不是必须的，因为 Entity Framework 会根据你为属性定义的 CLR 类型选择合适的 SQL Server 数据类型。CLR 的 ``decimal`` 类型会被映射为 SQL Server 的 ``decimal`` 类型。但是在本例中你需要知道该列需要保存的是货币金额，所以在此处使用 SQL Server 的 money 类型更为妥当。

外键与导航属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The foreign key and navigation properties reflect the following relationships:

实体中的外键属性和导航属性反映以下关系：

A department may or may not have an administrator, and an administrator is always an instructor. Therefore the ``InstructorID`` property is included as the foreign key to the Instructor entity, and a question mark is added after the ``int`` type designation to mark the property as nullable. The navigation property is named ``Administrator`` but holds an Instructor entity:

每个系可以有也可以没有管理员，同时管理员又总是教师。因此 ``InstructorID`` 属性被包含在 Instructor 实体中并作为外键，同时 ``int`` 类型后面添加一个问号，这表示该属性被标记为可空。该导航属性命名为 ``Administrator``，但实际上保存的是 Instructor 实体。

.. code-block:: c#

  public int? InstructorID { get; set; }
  public virtual Instructor Administrator { get; set; }

A department may have many courses, so there's a Courses navigation property:

每个系都可以有很多门课程，所以存在这么一个 Courses 导航属性：

.. code-block:: c#

  public ICollection<Course> Courses { get; set; }

.. note::  By convention, the Entity Framework enables cascade delete for non-nullable foreign keys and for many-to-many relationships. This can result in circular cascade delete rules, which will cause an exception when you try to add a migration. For example, if you didn't define the Department.InstructorID property as nullable, EF would configure a cascade delete rule to delete the instructor when you delete the department, which is not what you want to have happen. If your business rules required the ``InstructorID`` property to be non-nullable, you would have to use the following fluent API statement to disable cascade delete on the relationship:

.. note:: 根据约定，Entity Framework 会对非可空外键和多对多关系启用级联删除。当你尝试添加迁移的时候将引发一场，这是循环级联删除规则所导致的结果。举例来说，如果你没有定义 Department.InstructorID 属性为可空，那么 EF 将配置一个级联删除规则，当你删除系的时候会同时删除教师，这并不是你所希望发生的。如果你的业务规则需要 ``InstructorID`` 属性为非可空，那么你就需要使用 Fluent API 语句来禁用级联删除了：

  .. code-block:: c# 

    modelBuilder.Entity<Department>()
      .HasOne(d => d.Administrator)
      .WithMany()
      .OnDelete(DeleteBehavior.Restrict)

修改 Enrollment 实体
----------------------------

.. image:: complex-data-model/_static/enrollment-entity.png
   :alt: Enrollment entity 

In *Models/Enrollment.cs*, replace the code you added earlier with the following code:

用下列代码取代早前在 *Models/Enrollment.cs* 中添加的代码：

.. literalinclude::  intro/samples/cu/Models/Enrollment.cs
  :language: c#
  :start-after: snippet_Final
  :end-before:  #endregion
  :emphasize-lines: 1-2,16

外键与导航属性
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

The foreign key properties and navigation properties reflect the following relationships:

实体中的外键属性和导航属性反映以下关系：

An enrollment record is for a single course, so there's a ``CourseID`` foreign key property and a ``Course`` navigation property:

学生注册课程的记录是针对单门课程的，所以需要一个 ``CourseID`` 外键属性和 ``Course`` 导航属性：

.. code-block:: c#

  public int CourseID { get; set; }
  public Course Course { get; set; }

An enrollment record is for a single student, so there's a ``StudentID`` foreign key property and a ``Student`` navigation property:

学生注册课程的记录又是针对单个学生的，所以同时你也需要一个 ``StudentID`` 外键属性和 ``Student`` 导航属性：

.. code-block:: c#

  public int StudentID { get; set; }
  public Student Student { get; set; }

多对多关系
--------------------------

There's a many-to-many relationship between the Student and Course entities, and the Enrollment entity functions as a many-to-many join table *with payload* in the database. "With payload" means that the Enrollment table contains additional data besides foreign keys for the joined tables (in this case, a primary key and a Grade property).

在 Student 和 Course 实体之间存在多对多（many-to-many）关系，这正是 Enrollment 实体在其中起到的作用——在数据库中 Enrollment 实体是 *带有载荷* 的多对多连接表。「带有载荷」的意思是说 Enrollment 表包含了除用于连接表的外键（在本例中，外键是主键和 Grade 属性）之外的额外数据。

The following illustration shows what these relationships look like in an entity diagram. (This diagram was generated using the Entity Framework Power Tools for EF 6.x; creating the diagram isn't part of the tutorial, it's just being used here as an illustration.)

下图展示了在实体图表中这些关系所展示的样子（这张图表由 Entity Framework Power Tools for EF 6.x 所生成；本教程不包含如何创建这张图表，此处仅作示例）。

.. image:: complex-data-model/_static/student-course.png
   :alt: Student-Course many to many relationship

Each relationship line has a 1 at one end and an asterisk (*) at the other, indicating a one-to-many relationship.

每一根关系线（relationship line）一端有个「1」另一端有个「*」，表示一对多（one-to-many）关系。

If the Enrollment table didn't include grade information, it would only need to contain the two foreign keys CourseID and StudentID. In that case, it would be a many-to-many join table without payload (or a pure join table) in the database. The Instructor and Course entities have that kind of many-to-many relationship, and your next step is to create an entity class to function as a join table without payload. 

如果 Enrollment 表并不包含年级信息，那么就只需要包含两个外键（CourseID 和 StudentID 即可）。那样的话它将是不带有载荷的多对多连接表（或者说它是一张纯粹的连接表）。Instructor 和 Course 实体具有这种多对多关系，你下一步的工作就是创建一个不带有载荷的实体类来连接它们。

CourseAssignment 实体
---------------------------

A join table is required in the database for the Instructor-to-Courses many-to-many relationship, and ``CourseAssignment`` is the entity that represents that table.

为了在数据库中体现 Instructor 和 Courses 之间的多对多关系，我们需要一张连接表，用 ``CourseAssignment`` 实体用来表示那张连接表。

.. image:: complex-data-model/_static/courseassignment-entity.png
   :alt: CourseAssignment entity

Create *Models/CourseAssignment.cs* with the following code:

用下面这段代码创建 *Models/CourseAssignment.cs* 文件：

.. literalinclude::  intro/samples/cu/Models/CourseAssignment.cs
  :language: c#

复合主键
^^^^^^^^^^^^^

Since the foreign keys are not nullable and together uniquely identify each row of the table, there is no need for a separate primary key. The `InstructorID` and `CourseID` properties should function as a composite primary key. The only way to identify composite primary keys to EF is by using the *fluent API* (it can't be done by using attributes). You'll see how to configure the composite primary key in the next section.

由于本例中外键是不可为空且唯一标识表中的每一行，所以没有必要再单独设计一个主键。`InstructorID` 和 `CourseID` 属性可以作为一个复合主键。在 EF 中只有一种办法让 EF 识别复合主键，那就是使用 *fluent API*（这一点使用特性是不能实现的）。下节中你将了解如何配置复合主键。

The composite key ensures that while you can have multiple rows for one course, and multiple rows for one instructor, you can't have multiple rows for the same instructor and course. The ``Enrollment`` join entity defines its own primary key, so duplicates of this sort are possible. To prevent such duplicates, you could add a unique index on the foreign key fields, or configure ``Enrollment`` with a primary composite key similar to ``CourseAssignment``. For more information, see `Indexes <https://docs.efproject.net/en/latest/modeling/indexes.html>`__.

复合主键能够保证这么一点：当你同一门课程和同一个教师都有多行记录时，不可能存在多行同一门课程与同一个教师在同一行记录中的情况出现。``Enrollment`` 连接实体定义自己的主键，因此这种重复的情况时可能出现的。为了避免这种情况发生，可以在外键字段上添加一个唯一索引，或者给 ``Enrollment`` 实体配置复合索引，类似于 ``CourseAssignment``。更多信息可以查看 `索引 <https://docs.efproject.net/en/latest/modeling/indexes.html>`__。

连接实体名称
^^^^^^^^^^^^^^^^^

It's common to name a join entity ``EntityName1EntityName2``, which in this case would be ``CourseInstructor``. However, we recommend that you choose a name that describes the relationship. Data models start out simple and grow, with no-payload joins frequently getting payloads later. If you start with a descriptive entity name, you won't have to change the name later. 

通常来说用于连接的实体会被命名为类似于 ``EntityName1EntityName2`` 这样的名次风格，在本例中可以命名为 ``CourseInstructor``。不过我们推荐你选择一个能描述两者关系的名称。数据模型一开始会很简单，然后慢慢发展，从没有负载连接发展到后期频繁地获取负载。如果你一开始就用具有描述性的名字来给实体命名，那么后面就不再需要更名了。

升级数据库上下文  
---------------------------

Add the following highlighted code to the *Data/SchoolContext.cs*:

添加下面第 15-18 行以及 25-31 行的代码到 *Data/SchoolContext.cs* 中：

.. literalinclude::  intro/samples/cu/Data/SchoolContext.cs
  :language: c#
  :start-after: snippet_BeforeInheritance
  :end-before:  #endregion
  :emphasize-lines: 15-18,25-31

This code adds the new entities and configures the CourseAssignment entity's composite primary key.

这段代码添加了新实体并配置了 CourseAssignment 实体的复合主键。

替代特性的 Fluent API
------------------------------------

The code in the ``OnModelCreating`` method of the ``DbContext`` class uses the *fluent API* to configure EF behavior. The API is called "fluent" because it's often used by stringing a series of method calls together into a single statement, as in this example from the `EF Core documentation <http://ef.readthedocs.io/en/latest/modeling/index.html#methods-of-configuration>`__:

``DbContext`` 的 ``OnModelCreating`` 方法中的代码通过使用 *fluent API* 配置 EF 行为。这种 API 之所以被称作「fluent」，是因为其经常被用于将一系列的方法调用串在一起，组成一个单一的语句，就如 `EF Core 文档 <http://ef.readthedocs.io/en/latest/modeling/index.html#methods-of-configuration>`__ 中的例子那般：

.. code-block:: c#

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
      modelBuilder.Entity<Blog>()
          .Property(b => b.Url)
          .IsRequired();
  }

In this tutorial you're using the fluent API only for database mapping that you can't do with attributes. However, you can also use the fluent API to specify most of the formatting, validation, and mapping rules that you can do by using attributes. Some attributes such as ``MinimumLength`` can't be applied with the fluent API. As mentioned previously, ``MinimumLength`` doesn't change the schema, it only applies a client and server side validation rule.

本教程中你所使用的 fluent API 只用来映射数据库（因为这个工作特性无法完成）。然而你也可以使用 fluent API 来指定之前用特性来指定的定义格式、验证、映射规则等。有些特性（如 ``MinimumLength``）不能用在 fluent API 中。如前所述，``MinimumLength`` 不会改变数据库架构，它只会应用在客户端和服务器端的验证规则上。

Some developers prefer to use the fluent API exclusively so that they can keep their entity classes "clean." You can mix attributes and fluent API if you want, and there are a few customizations that can only be done by using fluent API, but in general the recommended practice is to choose one of these two approaches and use that consistently as much as possible. If you do use both, note that wherever there is a conflict, Fluent API overrides attributes.

一些开发者喜欢使用 fluent API，因为认为这样做能保持他们的实体「整洁」。你可以混合使用特性和 fluent API，只要你愿意，当然也有一些定制配置只能用 fluent API 来完成，但一般情况下推荐你尽量选择其中一种来使用。如果你两个都用，那么要注意两者之前的冲突，牢记 Fluent API 配置会重写特性配置。

For more information about attributes vs. fluent API, see `Methods of configuration <https://ef.readthedocs.io/en/latest/modeling/index.html#methods-of-configuration>`__.

更多关于特性与 fluent API 之间的对比，请参考 `配置方法 <https://ef.readthedocs.io/en/latest/modeling/index.html#methods-of-configuration>`__。

显示关系的实体图表
------------------------------------

The following illustration shows the diagram that the Entity Framework Power Tools create for the completed School model.

下图展示了 Entity Framework Power Tools 所创建的完整的 School 模型。

.. image:: complex-data-model/_static/diagram.png
   :alt: Entity diagram

Besides the one-to-many relationship lines (1 to \*), you can see here the one-to-zero-or-one relationship line (1 to 0..1) between the Instructor and OfficeAssignment entities and the zero-or-one-to-many relationship line (0..1 to \*) between the Instructor and Department entities.

除了一对多关系线（1 对 \*），你能发现在 Instructor 和 OfficeAssignment 实体之间有一对零或一关系线（1 对 0..1），在 Instructor 和 Department 实体之间有零或一对多关系线（0..1 对 \*）的存在。

用测试数据初始化并填充数据库
--------------------------------

Replace the code in the *Data/DbInitializer.cs* file with the following code in order to provide seed data for the new entities you've created.

用下面这段代码代替 *Data/DbInitializer.cs* 文件中的代码，为你新创建的实体提供初始填充数据。

.. literalinclude::  intro/samples/cu/Data/DbInitializer.cs
  :language: c#
  :start-after: snippet_Final
  :end-before:  #endregion

As you saw in the first tutorial, most of this code simply creates new entity objects and loads sample data into properties as required for testing. Notice how the many-to-many relationships are handled: the code creates relationships by creating entities in the ``Enrollments`` and ``CourseInstructor`` join entity sets.

如你在第一篇教程中所见，大多数代码只是简单地创建新实体对象并简单地将数据加载到测试所需的属性中。注意多对多关系是如何处理的：代码通过创建 ``Enrollments`` 和 ``CourseInstructor`` 的连接实体集，以此来创建彼此间的关系。

添加迁移
---------------

Save your changes and build the project. Then open the command window in the project folder and enter the ``migrations add`` command (don't do the update-database command yet):

保存变更并生成项目。打开项目文件夹的命令窗口，输入 ``migrations add`` 命令（先不要使用 update-database 命令）：

.. code-block:: none

  dotnet ef migrations add ComplexDataModel -c SchoolContext

You get a warning about possible data loss.

你得到一个警告，显示可能会有数据丢失。

.. code-block:: none

  C:\ContosoUniversity\src\ContosoUniversity>dotnet ef migrations add ComplexDataModel -c SchoolContext
  Project ContosoUniversity (.NETCoreApp,Version=v1.0) will be compiled because Input items removed from last build
  Compiling ContosoUniversity for .NETCoreApp,Version=v1.0
  Compilation succeeded.
      0 Warning(s)
      0 Error(s)
  Time elapsed 00:00:02.9907258

  An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy.
  
  Done.

  To undo this action, use 'dotnet ef migrations remove'

If you tried to run the ``database update`` command at this point (don't do it yet), you would get the following error:

如果你此时尝试运行 ``database update`` 命令（记住先不要这么做），那么你会得到以下错误：

  The ALTER TABLE statement conflicted with the FOREIGN KEY constraint "FK_dbo.Course_dbo.Department_DepartmentID". The conflict occurred in database "ContosoUniversity", table "dbo.Department", column 'DepartmentID'.

Sometimes when you execute migrations with existing data, you need to insert stub data into the database to satisfy foreign key constraints. The generated code in the ``Up`` method adds a non-nullable DepartmentID foreign key to the Course table. If there are already rows in the Course table when the code runs, the ``AddColumn`` operation fails because SQL Server doesn't know what value to put in the column that can't be null. For this tutorial you'll run the migration on a new database, but in a production application you'd have to make the migration handle existing data, so the following directions show an example of how to do that.

有时当你使用现有的数据执行迁移时，你需要将存根数据茶如到数据库中以满足外键约束。在 ``Up`` 方法中生成的代码在 Course 表中添加了一个不可为空的 DepartmentID 外键。如果当代码运行时 Course 中已经存在数据，那么 ``AddColumn`` 操作就会失败，因为 SQL Ser贝尔 不知道在那个不可为空的字段中放什么值。在本教程中，你的迁移将在新数据库上运行，但对于生产环境上的应用程序来说你必须迁移现有数据，所以下面将通过例子来介绍如何迁移。

To make this migration work with existing data you have to change the code to give the new column a default value, and create a stub department named "Temp" to act as the default department. As a result, existing Course rows will all be related to the "Temp" department after the ``Up`` method runs.

为了使迁移与现有数据配合，你必须给新添加的列一个默认值，并创建一个名为「Temp」的存根系作为默认系。因此当 ``Up`` 方法运行后 Course 将与「Temp」系表关联。

Open the `<timestamp>_ComplexDataModel.cs` file. Comment out the line of code that adds the DepartmentID column to the Course table, and add before it the following code (the commented lines are also shown):

打开 `<timestamp>_ComplexDataModel.cs` 文件。在 Course 表中注释掉添加 DepartmentID 列的哪行代码，然后在它之前添加如下代码：

.. literalinclude::  intro/samples/cu/Migrations/20160727184013_ComplexDataModel.cs
  :language: c#
  :start-after: snippet_DefaultDepartment
  :end-before:  #endregion
  :dedent: 12

In a production application, you would write code or scripts to add Department rows and relate Course rows to the new Department rows. You would then no longer need the "Temp" department or the default value on the Course.DepartmentID column.

在生产环境的应用程序中，你可以编写代码或脚本来添加 Department 行，然后将 Course 行与新的 Department 行进行关联。接着你就不在需要「Temp」系或在 Course.DepartmentID 列中使用默认值了。

Save your changes and build the project.

保存变更并生成项目。

修改连接字符串并升级数据库
----------------------------------------------------

You now have new code in the ``DbInitializer`` class that adds seed data for the new entities to an empty database. To make EF create a new empty database, change the name of the database in the connection string in *appsettings.json* to ContosoUniversity3 or some other name that you haven't used on the computer you're using.

此时你在 ``DbInitializer`` 类中有了一段新代码，这段代码为空数据库的新实体提供初始化种子数据。为使 EF 创建新数空白数据库，需要在 *appsettings.json* 中改变数据库的连接字符串名称，改成 ContosoUniversity3 或者其它名字，只要这个名字你尚未使用即可。

.. literalinclude::  intro/samples/cu/appsettings.json
  :language: json
  :end-before:  Logging
  :emphasize-lines: 6

Save your change to *appsettings.json*.

保存 *appsettings.json*。

.. note:: As an alternative to changing the database name, you can delete the database. Use **SQL Server Object Explorer** (SSOX) or the ``database drop`` CLI command:

.. note:: 作为更改数据库名称的替代方法，你可以删除数据库。使用 **SQL Serber ObjectExplorer**（SSOX）或使用 ``database drop`` CLI 命令：

  .. code-block:: none

    dotnet ef database drop -c SchoolContext

After you have changed the database name or deleted the database, run the ``database update`` command in the command window to execute the migrations.

数据库更名或删除数据库后，在命令窗口中运行 ``database update`` 命令来执行迁移。

.. code-block:: none

  dotnet ef database update -c SchoolContext

Run the app to cause the ``DbInitializer.Initialize`` method to run and populate the new database.

运行应用程序，使 ``DbInitializer.Initialize`` 方法运行并填充新数据库。

Open the database in SSOX as you did earlier, and expand the **Tables** node to see that all of the tables have been created. (If you still have SSOX open from the earlier time, click the Refresh button.)

像之前那样在 SSOX 中打开数据库，然后展开 **Tables** 节点，查看是不是创建了所有表（如果你之前已经打开了 SSOX，先点击刷新按钮）。

.. image:: complex-data-model/_static/ssox-tables.png
   :alt: Tables in SSOX

Run the application to trigger the initializer code that seeds the database.

运行应用程序以便触发包含数据库种子数据（Seeds）的初始化器代码。

Right-click the **CourseInstructors** table and select **View Data** to verify that it has data in it.

右键点击 **CourseInstructors** 表，选择 **View Data** 来验证数据是否存入其中。

.. image:: complex-data-model/_static/ssox-ci-data.png
   :alt: CourseInstructor data in SSOX

总结
-------

You now have a more complex data model and corresponding database. In the following tutorial, you'll learn more about how to access related data.

现在你就有了一个更复杂的数据模型和对应的数据库了。在接下来的教程中，你将学习更多有关如何访问相关数据的知识。
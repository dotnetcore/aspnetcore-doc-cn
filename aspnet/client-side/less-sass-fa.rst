用 Less、Sass 和 Font Awesome 设计应用
======================================================

By `Steve Smith`_

翻译： `刘怡(AlexLEWIS/Forerunner) <http://github.com/alexinea>`_

校对：

Users of web applications have increasingly high expectations when it comes to style and overall experience. Modern web applications frequently leverage rich tools and frameworks for defining and managing their look and feel in a consistent manner. Frameworks like `Bootstrap`_ can go a long way toward defining a common set of styles and layout options for the web sites. However, most non-trivial sites also benefit from being able to effectively define and maintain styles and cascading style sheet (CSS) files, as well as having easy access to non-image icons that help make the site's interface more intuitive. That's where languages and tools that support `Less`_ and `Sass`_, and libraries like `Font Awesome`_, come in.

随着 Web 应用的使用者们对于风格样式和整体体验的愈发挑剔，现代 Web 应用程序（愈来愈）经常地使用丰富的工具和框架定义与管理外观和体验的一致性。如 `Bootstrap`_ 这样的框架于网站的通用样式集与布局一道大有益处。不过对于大多数重要站点同样可从中得益——更有效地定义和维护样式和级联样式表（CSS）文件、更易访问的非图片的图标——这使得网站的界面更加直观。这正是支持 `Less`_、`Sass`_ 以及 `Font Awesome`_ 的语言和工具所起的作用了。

.. _Bootstrap : http://getbootstrap.com/
.. _Less : http://lesscss.org/
.. _Sass : http://sass-lang.com/
.. _`Font Awesome` : http://fortawesome.github.io/Font-Awesome/

.. contents:: Sections:
  :local:
  :depth: 1

CSS 预处理语言
--------------------------

Languages that are compiled into other languages, in order to improve the experience of working with the underlying language, are referred to as pre-processors. There are two popular pre-processors for CSS: Less and Sass.  These pre-processors add features to CSS, such as support for variables and nested rules, which improve the maintainability of large, complex stylesheets. CSS as a language is very basic, lacking support even for something as simple as variables, and this tends to make CSS files repetitive and bloated. Adding real programming language features via preprocessors can help reduce duplication and provide better organization of styling rules. Visual Studio provides built-in support for both Less and Sass, as well as extensions that can further improve the development experience when working with these languages.

为提高与底层语言工作的体验，将由预处理器（pre-processors）编译为另一种语言的语言，称为预处理语言。对 CSS 而言有两款比较流行的预处理器：Less 和 Sass。这些预处理器会将诸如支持变量和嵌套规则等功能添加到 CSS 中，从而极大提高大型复杂样式的可维护性。CSS 是一种非常基本的语言，缺乏支持最基本的一些变量，这往往使 CSS 文件变得重复、臃肿。通过预处理器实现真正的编程语言功能可以减少重复并为更好地组织样式规则。Visual Studio 内建支持 Less 和 Sass，以及在使用这些语言时能进一步提高开发体验的扩展。

As a quick example of how preprocessors can improve readability and maintainability of style information, consider this CSS:

下例为您演示为何预处理器能提高样式信息的可读性和可维护性，先来看这段 CSS：

.. code-block:: css

  .header {
     color: black;
     font-weight: bold;
     font-size: 18px;
     font-family: Helvetica, Arial, sans-serif;
  }

  .small-header {
     color: black;
     font-weight: bold;
     font-size: 14px;
     font-family: Helvetica, Arial, sans-serif;
  }

Using Less, this can be rewritten to eliminate all of the duplication, using a mixin (so named because it allows you to "mix in" properties from one class or rule-set into another):

当使用 Less 时，为了消除所有重复信息，将一个类（class）或规则集（rule-set）写进另一个之中。上面这段 CSS 可以改写成下面这段：

.. code-block:: css
  :emphasize-lines: 9

  .header {
     color: black;
     font-weight: bold;
     font-size: 18px;
     font-family: Helvetica, Arial, sans-serif;
  }

  .small-header {
     .header;
     font-size: 14px;
  }

Visual Studio adds a great deal of built-in support for Less and Sass. You can also add support for earlier versions of Visual Studio by installing the `Web Essentials extension <http://vswebessentials.com/>`_.

Visual Studio 中增加了对 Less 和 Sass 的内建支持。你也可以通过安装 `Web Essentials extension <http://vswebessentials.com/>`_ 为早期版本的 Visual Studio 提供支持。

Less
----

The Less CSS pre-processor runs using Node.js. You can quickly install it using the Node Package Manager (NPM), with:

Less CSS 预处理器运行于 Node.js 之上。你可以通过 NPM（Node Package Manager）便捷地安装之：

.. code-block:: console

  npm install -g less

If you're using Visual Studio, you can get started with Less by adding one or more Less files to your project, and then configuring Gulp (or Grunt) to process them at compile-time. Add a Styles folder to your project, and then add a new Less file called main.less to this folder.

如果你使用的是 Visual Studio，你可以在你的项目中添加一个或多个 Less 文件，然后配置 Gulp（或 Grunt）使其在编译时（compile-time）处理这些 Less 文件。在项目中增加一个样式文件夹，然后在其中添加名曰 main.less 的 Less 文件。

.. image:: less-sass-fa/_static/add-less-file.png

Once added, your folder structure should look something like this:

添加后，文件夹结构如下所示：

.. image:: less-sass-fa/_static/folder-structure.png

Now we can add some basic styling to the file, which will be compiled into CSS and deployed to the wwwroot folder by Gulp.

然后我们在文件中添加一些基础样式，这些样式将会被编译为 CSS 并由 Gulp 部署到 wwwroot 中。

Modify main.less to include the following content, which creates a simple color palette from a single base color.

在 main.less 中增加以下内容：从单一基色中创建一个简单的调色板。

.. code-block:: none

  @base: #663333;
  @background: spin(@base, 180);
  @lighter: lighten(spin(@base, 5), 10%);
  @lighter2: lighten(spin(@base, 10), 20%);
  @darker: darken(spin(@base, -5), 10%);
  @darker2: darken(spin(@base, -10), 20%);

  body {
    background-color:@background;
  }
  .baseColor  {color:@base}
  .bgLight    {color:@lighter}
  .bgLight2   {color:@lighter2}
  .bgDark     {color:@darker}
  .bgDark2    {color:@darker2}

``@base`` and the other @-prefixed items are variables. Each of them represents a color. Except for ``@base``, they are set using color functions: lighten, darken, and spin. Lighten and darken do pretty much what you would expect; spin adjusts the hue of a color by a number of degrees (around the color wheel). The less processor is smart enough to ignore variables that aren't used, so to demonstrate how these variables work, we need to use them somewhere. The classes ``.baseColor``, etc. will demonstrate the calculated values of each of the variables in the CSS file that is produced.

``@base`` 和其他以 @ 符号作为前缀的项目都是变量。每个变量都表示一种颜色。除了 ``@base``，其他的都是用了颜色函数：变浅（lighten）、变深（darken）以及旋转（spin）。变浅和变深的效果如你所预料的那般；旋转是指在色轮（color wheel）上调整色调的角度。Less 处理器能够智能识别并忽略那些未被使用的变量，因此在演示这些变量如何工作时，我们需要在某些地方使用这些变量。``.baseColor`` 等类（class）将演示在所生成的 CSS 文件中为每一个变量计算其值。

开始学习
^^^^^^^^^^^^^^^

If you don't already have one in your project, add a new Gulp configuration file. Make sure package.json includes gulp in its ``devDependencies``, and add "gulp-less":

如果你项目中还没有 Gulp 配置文件，请先新建一个。确保 gulp 包含在 package.json 文件的 ``devDependencies`` 节点中，并在其中添加“gulp-less”：

.. code-block:: javascript
  :emphasize-lines: 3

  "devDependencies": {
      "gulp": "3.8.11",
      "gulp-less": "3.0.2",
      "rimraf": "2.3.2"
    }

Save your changes to the package.json file, and you should see that the all of the files referenced can be found in the Dependencies folder under NPM. If not, right-click on the NPM folder and select "Restore Packages."

保存 package.json 文件，然后你应该能看到所有引用的文件都可以在 NPM 下的 Dependencies 文件夹中被找到。如果没有，请右键点击 NPM 文件夹并选择“Restore Packages”。

Now open gulpfile.js. Add a variable at the top to represent less:

打开 gulpfile.js 文件，添加一个变量在里面，并指定为 less：

.. code-block:: javascript
  :emphasize-lines: 4

  var gulp = require("gulp"),
            rimraf = require("rimraf"),
            fs = require("fs"),
            less = require("gulp-less");

add another variable to allow you to access project properties:

添加另一个变量，用于访问项目属性：

.. code-block:: javascript

  var project = require('./project.json');

Next, add a task to run less, using the syntax shown here:

然后用如下所示的语法添加一个 less 运行任务：

.. code-block:: javascript

  gulp.task("less", function () {
    return gulp.src('Styles/main.less')
      .pipe(less())
      .pipe(gulp.dest(project.webroot + '/css'));
  });

Open the Task Runner Explorer (view>Other Windows > Task Runner Explorer). Among the tasks, you should see a new task named ``less``. Run it, and you should have output similar to what is shown here:

打开 Task Runner Explorer（view > Other Windows > Task Runner Explorer）。在这些任务重，你应该能看到名为 ``less`` 的新任务。运行之，然后你应该能看到输出结果类似下面这般所示：

.. image:: less-sass-fa/_static/less-task-runner.png

Now refresh your Solution Explorer and inspect the contents of the wwwroot/css folder. You should find a new file, main.css, there:

刷新 Solution Explorer 并检查 wwwroot/css 文件夹的内容。你应该能发现一个名叫 main.css 的新文件在那儿：

.. image:: less-sass-fa/_static/main-css-created.png

Open main.css and you should see something like the following:

打开 main.css 文件，其内容类似下面这段：

.. code-block:: css

  body {
    background-color: #336666;
  }
  .baseColor {
    color: #663333;
  }
  .bgLight {
    color: #884a44;
  }
  .bgLight2 {
    color: #aa6355;
  }
  .bgDark {
    color: #442225;
  }
  .bgDark2 {
    color: #221114;
  }

Add a simple HTML page to the wwwroot folder and reference main.css to see the color palette in action.

在 wwwroot 文件夹中添加一个简单的 HTML 页面并引用 main.css 文件用来查看调色板。

.. code-block:: html

  <!DOCTYPE html>
  <html>
  <head>
    <meta charset="utf-8" />
    <link href="css/main.css" rel="stylesheet" />
    <title></title>
  </head>
  <body>
    <div>
      <div class="baseColor">BaseColor</div>
      <div class="bgLight">Light</div>
      <div class="bgLight2">Light2</div>
      <div class="bgDark">Dark</div>
      <div class="bgDark2">Dark2</div>
    </div>
  </body>
  </html>

You can see that the 180 degree spin on ``@base`` used to produce ``@background`` resulted in the color wheel opposing color of ``@base``:

你可以发现基于 ``@base`` 旋转 180 度的颜色生成了与 ``@base`` 颜色相反的 ``@background``：

.. image:: less-sass-fa/_static/less-test-screenshot.png

Less also provides support for nested rules, as well as nested media queries. For example, defining nested hierarchies like menus can result in verbose CSS rules like these:

Less 永阳提供了对于嵌套规则的支持，以及嵌套媒介的查询。例如，如定义菜单这样的嵌套层次结构会导致这样冗长的 CSS 规则：

.. code-block:: css

  nav {
    height: 40px;
    width: 100%;
  }
  nav li {
    height: 38px;
    width: 100px;
  }
  nav li a:link {
    color: #000;
    text-decoration: none;
  }
  nav li a:visited {
    text-decoration: none;
    color: #CC3333;
  }
  nav li a:hover {
    text-decoration: underline;
    font-weight: bold;
  }
  nav li a:active {
    text-decoration: underline;
  }


Ideally all of the related style rules will be placed together within the CSS file, but in practice there is nothing enforcing this rule except convention and perhaps block comments.

理想情况下所有样式相关的规则都会被放置于 CSS 文件内，但实际情况并非强制要求如此，除非一些约定成俗的规则或者可能会出现的块状注释。

Defining these same rules using Less looks like this:

使用 Less 定义这些想用的规则会看起来想这个样子：

.. code-block:: none

  nav {
    height: 40px;
    width: 100%;
    li {
      height: 38px;
      width: 100px;
      a {
        color: #000;
        &:link { text-decoration:none}
        &:visited { color: #CC3333; text-decoration:none}
        &:hover { text-decoration:underline; font-weight:bold}
        &:active {text-decoration:underline}
      }
    }
  }

Note that in this case, all of the subordinate elements of ``nav`` are contained within its scope. There is no longer any repetition of parent elements (``nav``, ``li``, ``a``), and the total line count has dropped as well (though some of that is a result of putting values on the same lines in the second example). It can be very helpful, organizationally, to see all of the rules for a given UI element within an explicitly bounded scope, in this case set off from the rest of the file by curly braces.

注意，在该例中，``nav`` 的所有从属元素都被包含在其范围内。父元素（``nac``、``li``、``a``）不需要多次重复，总行数也有所下降（尽管其中的一部分是把值放在第二个例子的相同行的结果所致）。在组织上这非常管用，能够在一个给定的 UI 元素的有限范围内看到所有的规则，这个范围通过大括号来划定。

The ``&`` syntax is a Less selector feature, with & representing the current selector parent. So, within the a {...} block, ``&`` represents an ``a`` tag, and thus ``&:link`` is equivalent to ``a:link``.

``&`` 语法是 Less 选择器功能，当使用 & 时就表示当前选择器的父节点。因此在 a {...} 块中使用了 & 表示 ``a`` 标签，``&:link`` 就相当于 ``a:link`` 了。

Media queries, extremely useful in creating responsive designs, can also contribute heavily to repetition and complexity in CSS. Less allows media queries to be nested within classes, so that the entire class definition doesn't need to be repeated within different top-level ``@media`` elements. For example, this CSS for a responsive menu:

媒介查询，在创建响应式设计的过程中格外有用，当然也会导致 CSS 文件重复且复杂度大增。Less 允许媒介查询嵌套于类（class）中，因此类就不需要在不同的顶级 ``@media`` 元素中重复被定义了。例如这么一个响应式的菜单的 CSS 样式：

.. code-block:: css

  .navigation {
    margin-top: 30%;
    width: 100%;
  }
  @media screen and (min-width: 40em) {
    .navigation {
      margin: 0;
    }
  }
  @media screen and (min-width: 62em) {
    .navigation {
      width: 960px;
      margin: 0;
    }
  }

This can be better defined in Less as:

如果在 Less 中就能更好地定义：

.. code-block:: none

  .navigation {
    margin-top: 30%;
    width: 100%;
    @media screen and (min-width: 40em) {
      margin: 0;
    }
    @media screen and (min-width: 62em) {
      width: 960px;
      margin: 0;
    }
  }

Another feature of Less that we have already seen is its support for mathematical operations, allowing style attributes to be constructed from pre-defined variables. This makes updating related styles much easier, since the base variable can be modified and all dependent values change automatically.

Less 的另一项功能我们也已经看到了，就是它支持数学运算，允许样式属性通过预定义变量来生成。这使得更新关联样式变得极为轻松，因为只需改变基础变量，基于它的所有值都能自动改变。

CSS files, especially for large sites (and especially if media queries are being used), tend to get quite large over time, making working with them unwieldy. Less files can be defined separately, then pulled together using ``@import`` directives. Less can also be used to import individual CSS files, as well, if desired.

特别是对于大型站点（尤其是使用了媒介查询的大型站点）的 CSS 文件，往往会因为随着时间的推移其维护工作变得越发笨拙。Less 文件可以单独定义，然后通过使用 ``@import`` 指令将它们拉到一起。如果需要的话，Less 也可用于导入至单独的 CSS 文件。

*Mixins* can accept parameters, and Less supports conditional logic in the form of mixin guards, which provide a declarative way to define when certain mixins take effect. A common use for mixin guards is to adjust colors based on how light or dark the source color is. Given a mixin that accepts a parameter for color, a mixin guard can be used to modify the mixin based on that color:

*混合写法（Mixins）*可以接受参数，Less 支持混合 Guards 形式的条件逻辑，混合 Guards 提供了当某个混合生效时声明定义的办法。混合 Guards 的通常用法是基于原色进行明暗调整。给定一个接受颜色参数的混合（mixin），混合 Guard 通常就用于基于该色彩进行颜色修改：

.. code-block:: css

  .box (@color) when (lightness(@color) >= 50%) {
    background-color: #000;
  }
  .box (@color) when (lightness(@color) < 50%) {
    background-color: #FFF;
  }
  .box (@color) {
    color: @color;
  }

  .feature {
    .box (@base);
  }

Given our current ``@base`` value of ``#663333``, this Less script will produce the following CSS:

当前 ``@base`` 的值返回为 ``#663333``，这段 Less 脚本将生成如下 CSS：

.. code-block:: css

  .feature {
    background-color: #FFF;
    color: #663333;
  }

Less provides a number of additional features, but this should give you some idea of the power of this preprocessing language.

Less 提供了很多其他功能，但上面这些信息已经足以助你这门预处理语言的能力有所了解了。

Sass
----

Sass is similar to Less, providing support for many of the same features, but with slightly different syntax. It is built using Ruby, rather than JavaScript, and so has different setup requirements. The original Sass language did not use curly braces or semicolons, but instead defined scope using white space and indentation. In version 3 of Sass, a new syntax was introduced, **SCSS** ("Sassy CSS"). SCSS is similar to CSS in that it ignores indentation levels and whitespace, and instead uses semicolons and curly braces.

Sass 和 Less 很像，提供了许多相同功能的支持，但语法上略有不同。Sass 使用 Ruby 构建，而不是 JavaScript，因此有不同的安装需求。原先的 Sass 语言没有使用大括号或分号，而是通过空格和缩进来定义作用域。Sass 的第三个版本引入了新语法，被称为 **SCSS**（“Sassy CSS”）。SCSS 像 CSS 那样忽略空格和缩进，转而使用大括号和分号。

To install Sass, typically you would first install Ruby (pre-installed on Mac), and then run:

安装 Sass，通畅来说你首选需要安装 Ruby（macOS 上已经预装好了），然后运行：

.. code-block:: console

  gem install sass

However, assuming you're running Visual Studio, you can get started with Sass in much the same way as you would with Less. Open package.json and add the "gulp-sass" package to ``devDependencies``:

不过如果你运行的是 Visual Studio，你完全可以用与上文 Less 相似的方法开始写 Sass。打开 package.json 然后在 ``devDependencies`` 节点中增加 ``gulp-sass`` 包：

.. code-block:: javascript

  "devDependencies": {
    "gulp": "3.8.11",
    "gulp-less": "3.0.2",
    "gulp-sass": "1.3.3",
    "rimraf": "2.3.2"
  }

.. note While it is possible to have both Less and Sass side-by-side in the same project, typically you only use one or the other. Less is shown here because we're working from the same project we started at the beginning of this article.

.. note 虽然同一个项目可以同时使用 Less 和 Sass，但一般而言你要么用这个，要么用另一个。Less 在上面显示，只不过是因为这篇文章中我们使用了同一个项目，先介绍 Less 的用法、再介绍 Sass 的用法罢了。

Next, modify gulpfile.js to add a sass variable and a task to compile your Sass files and place the results in the wwwroot folder:

下一步，修改 gulpfile.js 文件，增加 Sass 变量，增加编译 Sass 文件的任务，把编译结果放置在 wwwroot 文件夹中：

.. code-block:: javascript

  var gulp = require("gulp"),
    rimraf = require("rimraf"),
    fs = require("fs"),
    less = require("gulp-less"),
    sass = require("gulp-sass");

  // other content removed

  gulp.task("sass", function () {
    return gulp.src('Styles/main2.scss')
      .pipe(sass())
      .pipe(gulp.dest(project.webroot + '/css'));
  });

Now you can add the Sass file main2.scss to the Styles folder in the root of the project:

然后你在项目根下的样式文件夹中新建名字为 main2.scss 的 Sass 文件：

.. image:: less-sass-fa/_static/add-scss-file.png

Open main2.scss and add the following:

打开 main2.scss，然后添加下面内容：

.. code-block:: none

  $base: #CC0000;
  body {
    background-color: $base;
  }

Save all of your files. Now in Task Runner Explorer, you should see a sass task. Run it, refresh solution explorer, and look in the /wwwroot/css folder. There should be a main2.css file, with these contents:

保存文件。然后在 Task Runner Explorer 中你可以看到一个 sass 任务。运行这个任务，刷新 solution explorer，查看 /wwwroot/css 文件夹。那里应该会有一个 main2.css 文件，其内容为：

.. code-block:: css

  body {
    background-color: #CC0000; }

Sass supports nesting in much the same was that Less does, providing similar benefits. Files can be split up by function and included using the ``@import`` directive:

Sass 所支持的嵌套和 Less 的大致相同，提供了相似的好处。根据功能可以将样式文件切为若干个文件，然后通过 ``@import`` 指令包含在一起：

.. code-block:: css

  @import 'anotherfile';


Sass supports mixins as well, using the ``@mixin`` keyword to define them and @include to include them, as in this example from `sass-lang.com <http://sass-lang.com>`_:

Sass 也支持混合（mixins），通过关键词 ``@mixin`` 来定义并通过关键词 ``@include`` 来包含它们，下例引用自 `sass-lang.com <http://sass-lang.com>`_：

.. code-block:: css

  @mixin border-radius($radius) {
    -webkit-border-radius: $radius;
     -moz-border-radius: $radius;
      -ms-border-radius: $radius;
        border-radius: $radius;
  }

  .box { @include border-radius(10px); }


In addition to mixins, Sass also supports the concept of inheritance, allowing one class to extend another. It's conceptually similar to a mixin, but results in less CSS code. It's accomplished using the ``@extend`` keyword. First, let's see how we might use mixins, and the resulting CSS code. Add the following to your main2.scss file:

除了混合（Mixins），Sass 支持继承概念，允许类扩展自另一个。这在概念上类似于混合，但可以生成更少的 CSS 代码。它通过使用关键词 ``@extend`` 来实现。首先让我们看看，如果使用混合（Mixins），会得到什么样的 CSS 代码。添加下面这些代码到 main2.scss 文件中：

.. code-block:: css
  :emphasize-lines: 8,13

  @mixin alert {
    border: 1px solid black;
    padding: 5px;
    color: #333333;
  }

  .success {
    @include alert;
    border-color: green;
  }

  .error {
    @include alert;
    color: red;
    border-color: red;
    font-weight:bold;
  }


Examine the output in main2.css after running the sass task in Task Runner Explorer:

到 Task Runner Explorer 运行 sass 任务后，检查输出的 main2.css 文件：

.. code-block:: css
  :emphasize-lines: 2-4,9-11

  .success {
    border: 1px solid black;
    padding: 5px;
    color: #333333;
    border-color: green;
   }

  .error {
    border: 1px solid black;
    padding: 5px;
    color: #333333;
    color: red;
    border-color: red;
    font-weight: bold;
  }

Notice that all of the common properties of the alert mixin are repeated in each class. The mixin did a good job of helping use eliminate duplication at development time, but it's still creating CSS with a lot of duplication in it, resulting in larger than necessary CSS files - a potential performance issue. It would be great if we could follow the `Don't Repeat Yourself (DRY) Principle <http://deviq.com/don-t-repeat-yourself/>`_ at both development time and runtime.

注意，所有通过混合引入的 alert 属性在每一个类中都重复了。混合确实可以在开发过程中消除重复工作，但它依旧会产生出现重复 CSS 代码的样式文件，通常所生成的 CSS 文件比所需要的更大——这是一个潜在的性能问题。如果我们遵循`不要使自己重复原则（Don't Repeat Yourself (DRY) Principle） <http://deviq.com/don-t-repeat-yourself/>`_，那么这个问题就会变得巨大。

Now replace the alert mixin with a ``.alert`` class, and change ``@include`` to ``@extend`` (remembering to extend ``.alert``, not ``alert``):

现在修改一下，用 ``.alert`` 类来混合，把 ``@include`` 改为 ``@extend``（注意是扩展 ``.alert``，而不是 ``alert``）：

.. code-block:: css
  :emphasize-lines: 8,13

  .alert {
    border: 1px solid black;
    padding: 5px;
    color: #333333;
  }

  .success {
    @extend .alert;
    border-color: green;
  }

  .error {
    @extend .alert;
    color: red;
    border-color: red;
    font-weight:bold;
  }


Run Sass once more, and examine the resulting CSS:

再一次运行 Sass，然后检查生成的 CSS：

.. code-block:: css

  .alert, .success, .error {
    border: 1px solid black;
    padding: 5px;
    color: #333333; }

  .success {
    border-color: green; }

  .error {
    color: red;
    border-color: red;
    font-weight: bold; }

Now the properties are defined only as many times as needed, and better CSS is generated.

现在属性都按需定义了，并生成了更好的 CSS 样式。

Sass also includes functions and conditional logic operations, similar to Less. In fact, the two languages' capabilities are very similar.

Sass 同样包含其他很多功能与逻辑条件操作，类似 Less。事实上这两种语言连能力都非常相似。

使用 Less 还是 Sass？
-------------

There is still no consensus as to whether it's generally better to use Less or Sass (or even whether to prefer the original Sass or the newer SCSS syntax within Sass). A recent poll conducted on twitter of mostly ASP.NET developers found that the majority preferred to use Less, by about a 2-to-1 margin. Probably the most important decision is to **use one of these tools**, as opposed to just hand-coding your CSS files. Once you've made that decision, both Less and Sass are good choices.

对于使用 Less 还是 Sass 其实依旧没有达成共识（甚至对于原生 Sass 还是基于 Sass 的 SCSS 之间有没有定论）。一项在 twitter 上展开的调查结果显示，对于大部分 ASP.NET 开发者来说，大约有三分之二的人会选择使用 Less。也许最关键的问题只不过在于 **无论 Less 还是 Sass，只是工具罢了，选择其一即可**，使用 Less/Sass 也不过是为了避免直接手工编码 CSS 文件而已。所以如果你同意这句话，不管是 Less 还是 Sass 都是你的最佳选择。

Font Awesome
------------

In addition to CSS pre-compilers, another great resource for styling modern web applications is Font Awesome. Font Awesome is a toolkit that provides over 500 scalable vector icons that can be freely used in your web applications. It was originally designed to work with Bootstrap, but has no dependency on that framework, or on any JavaScript libraries.

除了 CSS 预编译器，还有另一个妙不可言的可用于设计现代 Web 应用程序的资源，那就是 Font Awesome。Font Awesome 是一个提供了超过 500 个可扩展矢量图标的工具包，你可以在你的 Web 应用程序中自由使用它们。它最初被设计用于 Bootstrap，不过并不依赖于这个框架，也不依赖于任何 JavaScript 库。

The easiest way to get started with Font Awesome is to add a reference to it, using its public content delivery network (CDN) location:

使用 Font Awesome 的最简办法是使用公共内容分发网络（CDN）位置引用它：

.. code-block:: html

  <link rel="stylesheet"
  href="//maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css">

Of course, you can also quickly add it to your Visual Studio project by adding it to the "dependencies" in bower.json:

当然，你也可以方便地在 Visual Studio 中增加，只需要在 bower.json 文件的 “dependencies” 节点中加上 ``Font-Awesome``：

.. code-block:: javascript
  :emphasize-lines: 11

  {
    "name": "ASP.NET",
    "private": true,
    "dependencies": {
      "bootstrap": "3.0.0",
      "jquery": "1.10.2",
      "jquery-validation": "1.11.1",
      "jquery-validation-unobtrusive": "3.2.2",
      "hammer.js": "2.0.4",
      "bootstrap-touch-carousel": "0.8.0",
      "Font-Awesome": "4.3.0"
    }
  }


Then, to get the stylesheet added to the wwwroot folder, modify gulpfile.js as follows:

然后在 wwwroot 文件夹中增加样式，如下这般修改 gulpfile.js 文件：

.. code-block:: javascript
  :emphasize-lines: 10

  gulp.task("copy", ["clean"], function () {
    var bower = {
      "angular": "angular/angular*.{js,map}",
      "bootstrap": "bootstrap/dist/**/*.{js,map,css,ttf,svg,woff,eot}",
      "bootstrap-touch-carousel": "bootstrap-touch-carousel/dist/**/*.{js,css}",
      "hammer.js": "hammer.js/hammer*.{js,map}",
      "jquery": "jquery/jquery*.{js,map}",
      "jquery-validation": "jquery-validation/jquery.validate.js",
      "jquery-validation-unobtrusive": "jquery-validation-unobtrusive/jquery.validate.unobtrusive.js",
      "font-awesome": "Font-Awesome/**/*.{css,otf,eot,svg,ttf,woff,wof2}"
    };

    for (var destinationDir in bower) {
      gulp.src(paths.bower + bower[destinationDir])
        .pipe(gulp.dest(paths.lib + destinationDir));
    }
  });

Once this is in place (and saved), running the 'copy' task in Task Runner Explorer should copy the font awesome fonts and css files to ``/lib/font-awesome``.

完成后保存，然后在 Task Runner Explorer 中运行 'copy' 任务，font awesome 字体和 css 文件就会被复制到 ``/lib/font-awesome`` 中。

Once you have a reference to it on a page, you can add icons to your application by simply applying Font Awesome classes, typically prefixed with "fa-", to your inline HTML elements (such as ``<span>`` or ``<i>``).  As a very simple example, you can add icons to simple lists and menus using code like this:

如果你需要在页面中引用它，只需要简单地应用 Font Awesome 类就能在应用程序中使用这些图标了——通常来说就是在你的内联（inline）的 HTML 元素（如 ``<span>`` 或 ``<li>``）使用以 ``fa-`` 作为前缀的那些类即可。下面是一个简单例子，将图标添加到了列表和菜单中：

.. code-block:: html
  :emphasize-lines: 6,9-11

  <!DOCTYPE html>
  <html>
  <head>
    <meta charset="utf-8" />
    <title></title>
    <link href="lib/font-awesome/css/font-awesome.css" rel="stylesheet" />
  </head>
  <body>
    <ul class="fa-ul">
      <li><i class="fa fa-li fa-home"></i> Home</li>
      <li><i class="fa fa-li fa-cog"></i> Settings</li>
    </ul>
  </body>
  </html>

This produces the following in the browser - note the icon beside each item:

这样能在浏览器中生成如下的效果（注意每一项旁边的图标）：

.. image:: less-sass-fa/_static/list-icons-screenshot.png


You can view a complete list of the available icons here:

你可以通过下方链接查看完整的可用图标的清单：

http://fortawesome.github.io/Font-Awesome/icons/

总结
-------

Modern web applications increasingly demand responsive, fluid designs that are clean, intuitive, and easy to use from a variety of devices. Managing the complexity of the CSS stylesheets required to achieve these goals is best done using a pre-processor like Less or Sass. In addition, toolkits like Font Awesome quickly provide well-known icons to textual navigation menus and buttons, improving the overall user experience of your application.

现代 Web 应用程序对于响应式页面、简洁直观的设计以及跨设备多端使用的要求与日俱增。要实现这些目标，CSS 样式文件最好使用预处理器来维护和管理，就像 Less 或 Sass 那样。另外，例如 Font Awesome 这样的工具为文本导航菜单和按钮提供了一致性的图标，大大提升了应用的整体体验。
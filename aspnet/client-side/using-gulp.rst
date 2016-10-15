.. _using-gulp:

使用 Gulp
==========

By `Erik Reitan`_, `Scott Addie`_, `Daniel Roth`_ and `Shayne Boyer`_ 

翻译： `刘怡(AlexLEWIS/Forerunner) <http://github.com/alexinea>`_

校对：

In a typical modern web application, the build process might:

在典型的现代 Web 应用程序中，构建过程可能是：

- Bundle and minify JavaScript and CSS files.
- Run tools to call the bundling and minification tasks before each build.
- Compile LESS or SASS files to CSS.
- Compile CoffeeScript or TypeScript files to JavaScript.

- 绑定与最小化 JavaScript 与 CSS 文件。
- 每次构建前均运行工具调用绑定与最小化任务。
- 将 LESS 或 SASS 文件编译为 CSS 文件。
- 将 CoffeeScript 或 TypeScript 文件编译为 JavaScript 文件。

A *task runner* is a tool which automates these routine development tasks and more. Visual Studio provides built-in support for two popular JavaScript-based task runners: `Gulp <http://gulpjs.com>`__ and `Grunt <http://gruntjs.com/>`_.

*Task Runner* 是一个自动化处理这些日常开发任务的工具。Visual Studio 提供了对两种流行的给予 JavaScript 的 Task Runner 的内建支持：`Gulp <http://gulpjs.com>`__ 和 `Grunt <http://gruntjs.com/>`_。

.. contents:: Sections:
  :local:
  :depth: 1

Gulp 概述
----------------

Gulp is a JavaScript-based streaming build toolkit for client-side code. It is commonly used to stream client-side files through a series of processes when a specific event is triggered in a build environment. Some advantages of using Gulp include the automation of common development tasks, the simplification of repetitive tasks, and a decrease in overall development time. For instance, Gulp can be used to automate :doc:`bundling and minification <bundling-and-minification>` or the cleansing of a development environment before a new build.

Gulp 是基于 JavaScript 的客户端代码构建工具。它通常用于当当在构建环境中某个指定事件被触发时通过一系列过程来处理客户端文件。使用 Gulp 的有点包括日常开发任务、重复任务的自动化，简化并减少整体开发时间。举例来讲，Gulp 可用于自动化:doc:`捆绑与最小化 <bundling-and-minification>` 或清理之前构建生成的开发环境。

A set of Gulp tasks is defined in *gulpfile.js*. The following JavaScript, includes Gulp modules and specifies file paths to be referenced within the forthcoming tasks:

一组 Gulp 任务需要在 *gulpfile.js* 中定义。下面的 JavaScript 代码包含了 Gulp 模块，并为即将来临的任务指定了引用的文件

.. code-block:: javascript

  /// <binding Clean='clean' />
  "use strict";
  
  var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

  var paths = {
    webroot: "./wwwroot/"
  };

  paths.js = paths.webroot + "js/**/*.js";
  paths.minJs = paths.webroot + "js/**/*.min.js";
  paths.css = paths.webroot + "css/**/*.css";
  paths.minCss = paths.webroot + "css/**/*.min.css";
  paths.concatJsDest = paths.webroot + "js/site.min.js";
  paths.concatCssDest = paths.webroot + "css/site.min.css";

The above code specifies which Node modules are required. The ``require`` function imports each module so that the dependent tasks can utilize their features. Each of the imported modules is assigned to a variable. The modules can be located either by name or path. In this example, the modules named ``gulp``, ``rimraf``, ``gulp-concat``, ``gulp-cssmin``, and ``gulp-uglify`` are retrieved by name. Additionally, a series of paths are created so that the locations of CSS and JavaScript files can be reused and referenced within the tasks. The following table provides descriptions of the modules included in *gulpfile.js*.

上面代码指定了需要哪些 Node 模块。``require`` 函数将每个模块导入，这样依赖任务就可以利用它们的功能。每个被导入的模块都被分配给一个变量。模块可以通过名称或路径来定位。在本例中，名为 ``gulp``、``rimraf``、``gulp-concat``、``gulp-cssmin`` 以及 ``gulp-uglify`` 的模块都通过名称来检索得到。此外还创建了一组路径，以便 CSS 和 JavaScript 的路径可被重用并在任务中引用。下表提供了在 *gulpfile.js* 中所包含的模块的描述。

=============  =========================================================================================================================================================== 
模块名    描述
=============  =========================================================================================================================================================== 
gulp           Gulp 流生成系统，更多信息请阅读 `gulp <https://www.npmjs.com/package/gulp>`__。
rimraf         Node 下的删除模块，更多信息请阅读 `rimraf <https://www.npmjs.com/package/rimraf>`_。
gulp-concat    基于操作系统的换行符连接文件的模块，更多信息请阅读 `gulp-concat <https://www.npmjs.com/package/gulp-concat>`_。
gulp-cssmin    最小化 CSS 文件的模块，更多信息请阅读 `gulp-cssmin <https://www.npmjs.com/package/gulp-cssmin>`_。
gulp-uglify    使用 `UglifyJS <https://www.npmjs.com/package/gulp-cssmin>`_ 工具最小化 *.js* 文件的模块，更多请阅读 `gulp-uglify <https://www.npmjs.com/package/gulp-uglify>`_。
=============  ===========================================================================================================================================================

Once the requisite modules are imported, the tasks can be specified. Here there are six tasks registered, represented by the following code:

当导入必须的模块之后，就可指定任务。此处有留个注册任务，由以下代码表示：

.. code-block:: javascript
  :emphasize-lines: 1,5,9,11,18,25

  gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
  });

  gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
  });

  gulp.task("clean", ["clean:js", "clean:css"]);

  gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
      .pipe(concat(paths.concatJsDest))
      .pipe(uglify())
      .pipe(gulp.dest("."));
  });

  gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
      .pipe(concat(paths.concatCssDest))
      .pipe(cssmin())
      .pipe(gulp.dest("."));
  });

  gulp.task("min", ["min:js", "min:css"]);

The following table provides an explanation of the tasks specified in the code above:

下表提供了关于任务指定代码的一些解释：

=============  ========================================================================  
任务名称        描述  
=============  ========================================================================  
clean:js       使用 rimraf 模块移除最小化版本的 `site.js` 文件的任务。
clean:css      使用 rimraf 模块移除最小化版本的 `site.css` 文件的任务。
clean          依次调用 ``clean:js`` 和 ``clean:css`` 的任务。
min:js         最小化并连接所有在 `js` 文件夹中的 *.js* 文件（除 *.min.js* 文件外）的任务。
min:css        最小化并连接所有在 `css` 文件夹中的 *.css* 文件（除 *.min.css* 文件外）的任务。
min            依次调用 ``min:js`` 和 ``min:css`` 的任务。
=============  ========================================================================

运行默认任务
---------------------

If you haven’t already created a new Web app, create a new ASP.NET Web Application project in Visual Studio.

如果你尚未创建新的 Web 应用程序，请在 Visual Studio 中创建新的 ASP.NET Web Application 项目。

1. Add a new JavaScript file to your Project and name it *gulpfile.js*, copy the following code.

1. 在项目中添加一个 JavaScript 文件，并命名为 *gulpfile.js*，将以下代码复制进去。

.. code-block:: javascript

  /// <binding Clean='clean' />
  "use strict";
  
  var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

  var paths = {
    webroot: "./wwwroot/"
  };

  paths.js = paths.webroot + "js/**/*.js";
  paths.minJs = paths.webroot + "js/**/*.min.js";
  paths.css = paths.webroot + "css/**/*.css";
  paths.minCss = paths.webroot + "css/**/*.min.css";
  paths.concatJsDest = paths.webroot + "js/site.min.js";
  paths.concatCssDest = paths.webroot + "css/site.min.css";

  gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
  });

  gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
  });

  gulp.task("clean", ["clean:js", "clean:css"]);

  gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
      .pipe(concat(paths.concatJsDest))
      .pipe(uglify())
      .pipe(gulp.dest("."));
  });

  gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
      .pipe(concat(paths.concatCssDest))
      .pipe(cssmin())
      .pipe(gulp.dest("."));
  });

  gulp.task("min", ["min:js", "min:css"]);

2. Open the *project.json* file (add if not there) and add the following.

2. 打开 *project.json* 文件（如果没有，添加一个）并添加以下代码。

.. code-block:: javascript

  {
    "devDependencies": {
      "gulp": "3.8.11",
      "gulp-concat": "2.5.2",
      "gulp-cssmin": "0.1.7",
      "gulp-uglify": "1.2.0",
      "rimraf": "2.2.8"
    }
  }

3. In **Solution Explorer**, right-click *gulpfile.js*, and select **Task Runner Explorer**. 

3. 在 **Solution Explorer** 中右键单击 *gulpfile.js*，选择 **Task Runner Explorer**。

  .. image:: using-gulp/_static/02-SolutionExplorer-TaskRunnerExplorer.png

  **Task Runner Explorer** shows the list of Gulp tasks. In the default ASP.NET Core Web Application template in Visual Studio, there are six tasks included from *gulpfile.js*.

  **Task RUnner Explorer** 显示了 Gulp 任务列表。在 Visual Studio 的默认 ASP.NET Core Web Application 模板中有六个任务被包含在 *gulpfile.js* 里。

  .. image:: using-gulp/_static/03-TaskRunnerExplorer.png 

4. Underneath **Tasks** in **Task Runner Explorer**, right-click **clean**, and select **Run** from the pop-up menu.

4. 在 **Task Runner Explorer** 下面的 **Tasks**，右键点击 **clean**，然后从弹出菜单中选择 **Run**

  .. image:: using-gulp/_static/04-TaskRunner-clean.png 

**Task Runner Explorer** will create a new tab named **clean** and execute the related clean task as it is defined in *gulpfile.js*.

**Task Runner Explorer** 将创建名为 **clean** 的新标签并按 *gulpfile.js* 所定义的执行相关的清理任务。

5. Right-click the **clean** task, then select **Bindings** > **Before Build**.

5. 右键点击 **clean** 任务，并选择 **Bindings** > **Before Build**。

  .. image:: using-gulp/_static/05-TaskRunner-BeforeBuild.png 

  The **Before Build** binding option allows the clean task to run automatically before each build of the project.

  **Before Build** 绑定选项允许清理任务自动在每次构建项目前执行。

It's worth noting that the bindings you set up with **Task Runner Explorer** are **not** stored in the *project.json*.  Rather they are stored in the form of a comment at the top of your *gulpfile.js*.  It is possible (as demonstrated in the default project templates) to have gulp tasks kicked off by the *scripts* section of your *project.json*.  **Task Runner Explorer** is a way you can configure tasks to run using Visual Studio.  If you are using a different editor (for example, Visual Studio Code) then using the *project.json* will probably be the most straightforward way to bring together the various stages (prebuild, build, etc.)  and the running of gulp tasks. 

值得注意的是你使用 **Task Runner Explorer** 设置的绑定并**不**会保存在 *project.json* 中。相反，它们会以注释的形式保存在 *gulpfile.js* 文件的顶部。有可能（如演示中的默认项目模板）由 *project.json* 的 *script* 节点启动 gulp 任务。**Task Runner Explorer** 是一种可以配置任务以使其运行于 Visual Studio 的方法。如果你是用不同的编辑器（比如 Visual Studio Code）那么使用 *project.json* 可能是把各个阶段（预构建、构建等）和 gulp 任务的运行汇集在一起的最简单的方法。

.. note:: *project.json* stages are not triggered when building in Visual Studio by default.  If you want to ensure that they are set this option in the Visual Studio project properties: Build tab -> Produce outputs on build.  This will add a *ProduceOutputsOnBuild* element to your *.xproj* file which will cause Visual studio to trigger the *project.json* stages when building.

.. note:: 默认情况下在 Visual Studio 构建时不会触发 *project.json* 阶段。如果你想确保触发该阶段，设置此选项：Build tab -> Produce outputs on build。这会使你 *.xproj* 文件中添加一个 *ProduceOutputsOnBuild* 元素，该元素将引起 Visual Studio 在构建时触发 *project.json* 阶段。

定义与运行新任务
-------------------------------

To define a new Gulp task, modify *gulpfile.js*.

为了定义一个新的 Gulp 任务，需要修改 *gulpfile.js* 文件。
 
1. Add the following JavaScript to the end of *gulpfile.js*:

1. 在 *gulpfile.js* 文件末尾处添加以下 JavaScript 代码：

.. code-block:: javascript

  gulp.task("first", function () {
    console.log('first task! <-----');
  });
  
This task is named ``first``, and it simply displays a string. 

该任务命名为 ``first``，只是为了简单地显示一个字符串。

2. Save *gulpfile.js*.
2. 保存 *gulpfile.js*。
3. In **Solution Explorer**, right-click *gulpfile.js*, and select *Task Runner Explorer*. 
3. 在 **Solution Explorer** 中右键点击 *gulpfile.js* 并选择 *Task Runner Explorer*。
4. In **Task Runner Explorer**, right-click **first**, and select **Run**.
4. 在 **Task Runner Explorer** 中右键点击 **first** 并选择 **Run**。

  .. image:: using-gulp/_static/06-TaskRunner-First.png 

  You’ll see that the output text is displayed. If you are interested in examples based on a common scenario, see Gulp Recipes.

  你将看到输出文本被显示出来。如果你对基于一般场景的例子感兴趣，可以取去看一下 Gulp Recipes。

定义与运行一系列任务
--------------------------------------
When you run multiple tasks, the tasks run concurrently by default. However, if you need to run tasks in a specific order, you must specify when each task is complete, as well as which tasks depend on the completion of another task. 

当你运行多个任务，默认情况下任务会并发运行。然而，如果你需要为任务的运行指定一个顺序，那么你就必须指定每个任务何时完成，以及那些任务取决于另一个任务的完成。

1. To define a series of tasks to run in order, replace the ``first`` task that you added above in *gulpfile.js* with the following:

1. 为定义一系列按顺序运行的任务，用以下代码替换在 *gulpfile.js* 中添加的 ``first`` 任务的内容：

.. code-block:: javascript

  gulp.task("series:first", function () {
    console.log('first task! <-----');
  });

  gulp.task("series:second", ["series:first"], function () {
    console.log('second task! <-----');
  });

  gulp.task("series", ["series:first", "series:second"], function () {});

You now have three tasks: ``series:first``, ``series:second``, and ``series``. The ``series:second`` task includes a second parameter which specifies an array of tasks to be run and completed before the ``series:second`` task will run.  As specified in the code above, only the ``series:first`` task must be completed before the ``series:second`` task will run.

你目前有三个任务：``series:first``、``series:second`` 以及 ``series``。``series:second`` 任务包含第二个参数，它指定在 ``series:second`` 任务运行之前要运行并完成的任务列表。如上代码中所指定的，只有 ``series:first`` 运行完成后，``series:second`` 任务才可以运行。

2. Save *gulpfile.js*.
2. 保存 *gulpfile.js*。
3. In **Solution Explorer**, right-click *gulpfile.js* and select **Task Runner Explorer** if it isn’t already open. 
3. 如果 **Task Runner Explorer** 尚未打开，从 **Solution Explorer** 中右键点击 *gulpfile.js* 并选择打开之。
4. In **Task Runner Explorer**, right-click **series** and select **Run**.
4. 在 **Task Runner Explorer** 中右键点击 **series** 并选择 **Run**。

  .. image:: using-gulp/_static/07-TaskRunner-Series.png 
 
IntelliSense
------------

IntelliSense provides code completion, parameter descriptions, and other features to boost productivity and to decrease errors. Gulp tasks are written in JavaScript; therefore, IntelliSense can provide assistance while developing. As you work with JavaScript, IntelliSense lists the objects, functions, properties, and parameters that are available based on your current context. Select a coding option from the pop-up list provided by IntelliSense to complete the code.

IntelliSense 提供完成代码、参数描述和其他很多功能，用以提高生产力并减少错误的发生。Gulp 任务使用 JavaScript 编写；因此，IntelliSense 可以在开发阶段提供辅助。当你在编写 JavaScript 时，IntelliSense 可以根据当前的上下文列出可用的对象、函数、属性以及参数。从 IntelliSense 提供的弹出列表中选择一个编码选项以完成代码。

  .. image:: using-gulp/_static/08-IntelliSense.png 

  For more information about IntelliSense, see `JavaScript IntelliSense <https://msdn.microsoft.com/en-us/library/bb385682.aspx>`_.

  有关 IntelliSense 的更多信息请查看 `JavaScript IntelliSense <https://msdn.microsoft.com/en-us/library/bb385682.aspx>`_。

开发环境、准生产环境和生产环境
-------------------------------------------------

When Gulp is used to optimize client-side files for staging and production, the processed files are saved to a local staging and production location. The *_Layout.cshtml* file uses the **environment** tag helper to provide two different versions of CSS files. One version of CSS files is for development and the other version is optimized for both staging and production. In Visual Studio 2015, when you change the **Hosting:Environment** environment variable to ``Production``, Visual Studio will build the Web app and link to the minimized CSS files. The following markup shows the **environment** tag helpers containing link tags to the ``Development`` CSS files and the minified ``Staging, Production`` CSS files.

当把 Gulp 用于为预生产环境和生产环境优化客户端文件时，已处理的文件将被保存到本地的预生产环境和生产环境位置。*_Layout.cshtml* 文件使用 **environment** Tag Helper 来提供两种版本的 CSS 文件。一种版本的 CSS 文件用于开发，另一个版本针对预生产环境和生产环境进行了优化。在 Visual Studio 2015 中，当你把 **Hosting:Environment** 环境变量改为 ``Production`` 之后，Visual Studio 将构建 Web 应用程序并链接到最小化的 CSS 文件。以下标记显示了 **environment** Tag Helper 包含了针对 ``Development`` 的 link 标记以及针对 ``Staging, Production`` 的最小化 CSS 文件。

.. code-block:: html

  <environment names="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.css" />
    <link rel="stylesheet" href="~/css/site.css" />
  </environment>
  <environment names="Staging,Production">
    <link rel="stylesheet" href="https://ajax.aspnetcdn.com/ajax/bootstrap/3.3.5/css/bootstrap.min.css"
        asp-fallback-href="~/lib/bootstrap/dist/css/bootstrap.min.css"
        asp-fallback-test-class="sr-only" asp-fallback-test-property="position" asp-fallback-test-value="absolute" />
    <link rel="stylesheet" href="~/css/site.min.css" asp-append-version="true" />
  </environment>

切换环境
------------------------------

To switch between compiling for different environments, modify the **Hosting:Environment** environment variable's value.

要在不同环境的编译之间切换，需要修改 **Hosting:Environment** 环境变量的值。

1. In **Task Runner Explorer**, verify that the **min** task has been set to run **Before Build**.
1. 在 **Task Runner Explorer** 中验证 **min** 任务是否已经设置为**在构建前**运行。
2. In **Solution Explorer**, right-click the project name and select **Properties**.
2. 在 **Solution Explorer** 中右键点击项目名并选择 **Properties**。

  The property sheet for the Web app is displayed.

  Web 引用属性表已显示

3. Click the **Debug** tab.
3. 点击 **Debug** 标签。
4. Set the value of the **Hosting:Environment** environment variable to ``Production``.
4. 将 **Hosting:Environment** 环境变量的值设置为 ``Production``。
5. Press **F5** to run the application in a browser.
5. 点击 **F5** 在浏览器中运行应用程序。
6. In the browser window, right-click the page and select **View Source** to view the HTML for the page.
6. 在浏览器窗口中右键点击页面，并选择 **查看源代码**，查看页面的 HTML。

  Notice that the stylesheet links point to the minified CSS files.

  请注意，样式表链接指向缩小后的 CSS 文件。

7. Close the browser to stop the Web app.
7. 关闭浏览器并停止 Web 应用程序
8. In Visual Studio, return to the property sheet for the Web app and change the **Hosting:Environment** environment variable back to ``Development``.
8. 在 Visual Studio 中，返回到 Web 应用程序属性表，并将 **Hosting:Environment** 环境变量改回 ``Development``。
9. Press **F5** to run the application in a browser again.
9. 点击 **F5** 在浏览器中再次运行应用程序。
10. In the browser window, right-click the page and select **View Source** to see the HTML for the page.
10. 在浏览器窗口中右键点击页面，并选择 **查看源代码**，查看页面的 HTML。

  Notice that the stylesheet links point to the unminified versions of the CSS files.

  请注意此时样式表链接指向的是未经缩小的 CSS 文件。

For more information related to environments in ASP.NET Core, see :doc:`/fundamentals/environments`.

更多有关 APP.NET Core 环境的信息请查阅 :doc:`/fundamentals/environments`。

Task 与 Module 详情
-----------------------

A Gulp task is registered with a function name.  You can specify dependencies if other tasks must run before the current task. Additional functions allow you to run and watch the Gulp tasks, as well as set the source (`src`) and destination (`dest`) of the files being modified. The following are the primary Gulp API functions:

Gulp 任务通过函数名注册。如果有其它任务在当前任务之前裕兴，那么可以指定依赖关系。附加函数允许你运行并监视 Gulp 任务，以及设置被修改文件的源（`src`）和目标（`dest`）。以下是主要的 Gulp API 函数：

===============  ==========================================  =================================================================================================================  
Gulp 功能         语法                                        描述
===============  ==========================================  =================================================================================================================  
task             ``gulp.task(name[, deps], fn) { }``         ``task`` 函数能创建一个任务。``name`` 参数定义了任务的名称。``deps`` 参数包含一组该任务运行前必须完成的其它任务。`fn` 参数表示任务完成后执行的回调函数。
watch            ``gulp.watch(glob [, opts], tasks) { }``    ``watch`` 函数负责监视文件，当文件发生变化时运行任务。``glob`` 参数是 ``string`` 或 ``array`` 类型，它决定了要监视哪些文件。``opts`` 参数提供了额外的文件监视选项。
src              ``gulp.src(globs[, options]) { }``          ``src`` 函数提供与 ``glob`` 值匹配的文件。``glob`` 参数是 ``string`` 或 ``array`` 类型，它决定了要读取哪些文件。``options`` 参数提供了额外的文件选项。
dest             ``gulp.dest(path[, options]) { }``          ``dest`` 函数定义了可写入文件的位置。``path`` 参数是字符串或函数，用于确定目标文件夹。``options`` 参数是一个指向输出文件夹选项的对象。
===============  ==========================================  =================================================================================================================  

For additional Gulp API reference information, see `Gulp Docs API <https://github.com/gulpjs/gulp/blob/master/docs/API.md>`_. 

更多有关 Gulp API 的参考信息请阅读 `Gulp Docs API <https://github.com/gulpjs/gulp/blob/master/docs/API.md>`_ 。

Gulp Recipes
------------
The Gulp community provides Gulp `recipes <https://github.com/gulpjs/gulp/blob/master/docs/recipes/README.md>`_. These recipes consist of Gulp tasks to address common scenarios.

Gulp 社区提供了 Gulp `Recipes <https://github.com/gulpjs/gulp/blob/master/docs/recipes/README.md>`_。这些 recipes 包含了 Gulp 任务以应对常见情况。

总结
-------
Gulp is a JavaScript-based streaming build toolkit that can be used for bundling and minification. Visual Studio automatically installs Gulp along with a set of Gulp plugins. Gulp is maintained on `GitHub <https://github.com/gulpjs/gulp>`_. For additional information about Gulp, see the `Gulp Documentation <https://github.com/gulpjs/gulp/blob/master/docs/README.md>`_ on GitHub.

Gulp 是基于 JavaScript 的流构建工具，可用于捆绑与最小化文件。Visual Studio 自动安装了 Gulp 和一些 Gulp 插件。Gulp 维护在 `GitHub <https://github.com/gulpjs/gulp>`_ 上。更多有关 Gulp 的信息可以到 gitHub 上查看 `Gulp 文档 <https://github.com/gulpjs/gulp/blob/master/docs/README.md>`_。

同时可以参考
--------

- :doc:`bundling-and-minification`
- :doc:`using-grunt`

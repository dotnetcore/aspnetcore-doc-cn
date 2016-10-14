使用 Grunt
===========

`Noel Rice`_

翻译： `刘怡(AlexLEWIS/Forerunner) <http://github.com/alexinea>`_

校对：

Grunt is a JavaScript task runner that automates script minification, TypeScript compilation, code quality "lint" tools, CSS pre-processors, and just about any repetitive chore that needs doing to support client development. Grunt is fully supported in Visual Studio, though the ASP.NET project templates use Gulp by default (see :doc:`using-gulp`).

Grunt 是一款 JavaScript 任务运行器，它支持客户端开发的自动化压缩脚本、编译 TypeScript、Lint 代码质量工具、CSS 预处理器以及其他需要做的重复性工作。尽管 ASP.NET 项目模板默认使用 Gulp（具体请查看 :doc:`using-gulp`），但 Grunt 在 Visual Studio 中还是被完整支持的。

.. contents:: Sections:
  :local:
  :depth: 1

This example uses an empty ASP.NET Core project as its starting point, to show how to automate the client build process from scratch.

本文中使用的例子将基于一个空的 ASP.NET Core 项目作为出发点，来演示客户端构建过程中如何从头开始就实现自动化。

The finished example cleans the target deployment directory, combines JavaScript files, checks code quality, condenses JavaScript file content and deploys to the root of your web application. We will use the following packages: 

本例将完成清理目标部署目录、连接 JavaScript 文件、检查代码质量、压缩 JavaScript 文件内容以及部署到 Web 应用程序根目录中。我们将使用以下软件包：

- **grunt**: The Grunt task runner package.
- **grunt**：Grunt 任务运行器包。
- **grunt-contrib-clean**: A plugin that removes files or directories.
- **grunt-contrib-clean**：用于清理文件或目录的插件。
- **grunt-contrib-jshint**: A plugin that reviews JavaScript code quality.
- **grunt-contrib-jshint**：用于检查 JavaScript 代码质量的插件。
- **grunt-contrib-concat**: A plugin that joins files into a single file.
- **grunt-contrib-concat**：用于连接多个文件到单个文件的插件。
- **grunt-contrib-uglify**: A plugin that minifies JavaScript to reduce size.
- **grunt-contrib-uglify**：用于压缩 JavaScript 文件的插件。
- **grunt-contrib-watch**: A plugin that watches file activity.
- **grunt-contrib-watch**：用于监视文件活动的插件。

准备工作
-------------------------

To begin, set up a new empty web application and add TypeScript example files. TypeScript files are automatically compiled into JavaScript using default Visual Studio settings and will be our raw material to process using Grunt.

首先设置一个新的空 Web 应用程序并添加 TypeScript 示例文件。TypeScript 文件使用默认的 Visual Studio 设置，自动编译为 JavaScript 文件，以供后续 Grunt 处理时使用。

1. In Visual Studio, create a new ``ASP.NET Web Application``.
1. 进入 Visual Studio 创建新的 ``ASP.NET Web Application``。
2. In the **New ASP.NET Project** dialog, select the ASP.NET Core **Empty** template and click the OK button.
2. 在 **New ASP.NET Project** 对话框中选择 ASP.NET Core **空**模板并点击 OK。
3. In the Solution Explorer, review the project structure. The ``\src`` folder includes empty ``wwwroot`` and ``Dependencies`` nodes.
3. 在 Solution Explorer 中查看项目结构。``\src`` 文件夹包含空的 ``wwwroot`` 和 ``Dependencies`` 节点。

.. image:: using-grunt/_static/grunt-solution-explorer.png

4. Add a new folder named ``TypeScript`` to your project directory.
4. 在项目目录下添加 ``TypeScript`` 文件夹。
5. Before adding any files, let’s make sure that Visual Studio has the option 'compile on save' for TypeScript files checked. *Tools > Options > Text Editor > Typescript > Project*
5. 在添加文件前，先确保 Visual Studio 对 TypeScript 文件开启「Compile on save」选项。位置是 *Tools > Options > Test Editor > TypeScript > Projects*

.. image:: using-grunt/_static/typescript-options.png

6. Right-click the ``TypeScript`` directory and select **Add > New Item** from the context menu. Select the **JavaScript file** item and name the file **Tastes.ts** (note the \*.ts extension). Copy the line of TypeScript code below into the file (when you save, a new Tastes.js file will appear with the JavaScript source).

6. 右键点击 ``TypeScript`` 目录并在上下文菜单中选择 **Add > New Item**。选择 **JavaScript 文件**并将文件名命名为 **Testes.ts**（注意文件扩展名是 \*.ts）。复制下面的 TypeScript 代码到文件中（当你保存时，JavaScript  文件 Testes.js 会出现）。

.. code-block:: javascript

  enum Tastes { Sweet, Sour, Salty, Bitter }

7. Add a second file to the **TypeScript** directory and name it ``Food.ts``. Copy the code below into the file.

7. 添加第二个文件到 **TypeScript** 目录下并命名为 ``Food.ts``。复制下面代码到该文件中。

.. code-block:: javascript

  class Food {
    constructor(name: string, calories: number) {
      this._name = name;
      this._calories = calories; 
    }

    private _name: string;
    get Name() {
      return this._name;
    }

    private _calories: number;
    get Calories() {
      return this._calories;
    }

    private _taste: Tastes;
    get Taste(): Tastes { return this._taste }
    set Taste(value: Tastes) {
      this._taste = value;
    }
  }

配置 NPM
---------------

Next, configure NPM to download grunt and grunt-tasks.

接下来配置 NPM 下载 grunt 和 grunt-tasks。

1. In the Solution Explorer, right-click the project and select **Add > New Item** from the context menu. Select the **NPM configuration file** item, leave the default name, ``package.json``, and click the **Add** button.

1. 在 Solution Explorer 中右键单击项目并在上下文菜单中选择 **Add > New Item**。选择 **NPM configuration file** 项，保留默认文件名 ``package.json``，点击 **Add** 按钮。

2. In the package.json file, inside the ``devDependencies`` object braces, enter "grunt". Select ``grunt`` from the Intellisense list and press the Enter key. Visual Studio will quote the grunt package name, and add a colon. To the right of the colon, select the latest stable version of the package from the top of the Intellisense list (press ``Ctrl-Space`` if Intellisense does not appear).

2. 在 package.json 文件的 ``devDependencies``对象括号内输入「grunt」。从智能感知列表中选择 ``grunt``，然后点击回车键。Visual Studio 将引用 grunt 宝名称，并会添加冒号。在冒号的右边，从智能感知菜单的最顶部选择最新的稳定版本的软件包（如果智能感知列表没有出现，按 ``Ctrl-Space``）。

.. image:: using-grunt/_static/devdependencies-grunt.png

.. note:: NPM uses `semantic versioning <http://semver.org/>`_ to organize dependencies. Semantic versioning, also known as SemVer, identifies packages with the numbering scheme <major>.<minor>.<patch>. Intellisense simplifies semantic versioning by showing only a few common choices. The top item in the Intellisense list (0.4.5 in the example above) is considered the latest stable version of the package. The caret (^) symbol matches the most recent major version and the tilde (~) matches the most recent minor version. See the `NPM semver version parser reference <https://www.npmjs.com/package/semver>`_ as a guide to the full expressivity that SemVer provides.

.. note:: NPM 使用 `语义化版本（semantic versioning） <http://semver.org/>`_ 来组织依赖关系。语义化版本，也叫做 SemVer，用数字化的 <主版本 major>.<小版本 minor>.<patch> 版本架构来识别包。智能感知列表的第一项（上例中的 0.4.5）通常被认为是软件包的最新稳定版本；（latest stable version）。插入符号（^）表示匹配最新的主版本，波浪号（~）表示匹配最近的小版本。完整指南请参见 `NPM 语义化版本解析参考 <https://www.npmjs.com/package/semver>`_。

3. Add more dependencies to load grunt-contrib* packages for *clean, jshint, concat, uglify and watch* as shown in the example below. The versions do not need to match the example.

3. 添更多依赖一来，为 *clean、jshint、concat、uglify 以及 watch* 加载 grunt-contrib 包，如下例所示。版本不需要与例中的版本号匹配。 

.. code-block:: javascript

  "devDependencies": {
      "grunt": "0.4.5",
      "grunt-contrib-clean": "0.6.0",
      "grunt-contrib-jshint": "0.11.0",
      "grunt-contrib-concat": "0.5.1",
      "grunt-contrib-uglify": "0.8.0",
      "grunt-contrib-watch": "0.6.1"
  }

4. Save the ``package.json`` file.

4. 保存 ``package.json`` 文件。

The packages for each devDependencies item will download, along with any files that each package requires. You can find the package files in the ``node_modules`` directory by enabling the **Show All Files** button in the Solution Explorer.  

将下载每个 devDependencies 项的包及其所需的所有文件。在 Solution Explorer 中开启 ** Show All Files** 按钮后就可以在 ``node_modules`` 目录下找到所有的包文件。

.. image:: using-grunt/_static/node-modules.png

.. note:: If you need to, you can manually restore dependencies in Solution Explorer by right-clicking on ``Dependencies\NPM`` and selecting the **Restore Packages** menu option.

.. note:: 如果你需要，你可以在 Solution Explorer 中右键点击 ``Dependencies\NPM`` 并选择 **Restore Packages** 手工恢复依赖关系。

.. image:: using-grunt/_static/restore-packages.png


配置 Grunt
-----------------

Grunt is configured using a manifest named ``Gruntfile.js`` that defines, loads and registers tasks that can be run manually or configured to run automatically based on events in Visual Studio.

Grunt 使用名为 ``Gruntfile.js`` 的清单配置，任务的定义、加载以及注册可以手工运行或配置为基于 Visual Studio 事件的自动运行任务。

1. Right-click the project and select **Add > New Item**. Select the **Grunt Configuration file** option, leave the default name, ``Gruntfile.js``, and click the **Add** button. 

1. 右键单击项目并选择 **Add > New Item**。选择 **Grunt COnfiguration file** 选项，保留默认名称 ``Gruntfile.js`` 并点击 **Add** 按钮。

The initial code includes a module definition and the ``grunt.initConfig()`` method. The ``initConfig()`` is used to set options for each package, and the remainder of the module will load and register tasks.

初始代码包括模块定义和 ``grunt.initConfig()`` 方法。``initConfig()`` 方法为每个包设置选项，雨下的模块将加载并注册任务。

.. code-block:: javascript

  module.exports = function (grunt) {
    grunt.initConfig({
    });
  }; 

2.  Inside the ``initConfig()`` method, add options for the ``clean`` task as shown in the example Gruntfile.js below. The clean task accepts an array of directory strings. This task removes files from wwwroot/lib and removes the entire /temp directory.

2. 在 ``initConfig()`` 方法中，为 ``clean`` 任务增加选项，如下例 Gruntfile.js 文件所示。clean 任务接收目录字符串数组。该任务将在 wwwroot/lib 和 中删除文件，并在 /temp 目录中删除实体。

.. code-block:: javascript

  module.exports = function (grunt) {
    grunt.initConfig({
      clean: ["wwwroot/lib/*", "temp/"],
    });
  };

3. Below the initConfig() method, add a call to ``grunt.loadNpmTasks()``. This will make the task runnable from Visual Studio.

3. 在 initConfig() 方法下满添加对 ``grunt.loadNpmTasks()`` 的调用。这将使任务可在 Visual Studio 中运行。

.. code-block:: javascript

  grunt.loadNpmTasks("grunt-contrib-clean");

4. Save Gruntfile.js. The file should look something like the screenshot below. 

4. 保存 Gruntfile.js 文件。该文件看上去如下图所示。

.. image:: using-grunt/_static/gruntfile-js-initial.png

5. Right-click Gruntfile.js and select **Task Runner Explorer** from the context menu. The Task Runner Explorer window will open.

5. 右键点击 Gruntfile.js 并选择 **Task Runner Explorer**。打开 Task Runner Explorer 窗体。

.. image:: using-grunt/_static/task-runner-explorer-menu.png

6. Verify that ``clean`` shows under **Tasks** in the Task Runner Explorer.

6. 验证 Task Runner Explorer 中的 **Tasks** 下是否有 ``clean``。

.. image:: using-grunt/_static/task-runner-explorer-tasks.png

7. Right-click the clean task and select **Run** from the context menu. A command window displays progress of the task.

7. 右键单击 clean 任务并选择 **Run**，命令窗口随即显示该任务的过程。

.. image:: using-grunt/_static/task-runner-explorer-run-clean.png

.. note:: There are no files or directories to clean yet. If you like, you can manually create them in the Solution Explorer and then run the clean task as a test. 

.. note:: 尚未清理文件或目录。如果你想清理，你可以手动在 Solution Explorer 中创建它们并运行 clean 任务来测试一番。

8. In the initConfig() method, add an entry for ``concat`` using the code below. 

8. 在 initConfig() 方法中使用下列代码为 ``concat`` 添加入口。

The ``src`` property array lists files to combine, in the order that they should be combined. The ``dest`` property assigns the path to the combined file that is produced.

``src`` 属性列出了要组合的文件的列表，他们将按顺序组合。``dest`` 属性指定组合后的文件存放的路径。

.. code-block:: javascript

  concat: {
    all: {
      src: ['TypeScript/Tastes.js', 'TypeScript/Food.js'],
      dest: 'temp/combined.js'
    }
  }, 

.. note:: The ``all`` property in the code above is the name of a target. Targets are used in some Grunt tasks to allow multiple build environments. You can view the built-in targets using Intellisense or assign your own.

.. note:: 代码中 ``all`` 属性是目标名称（name of target）。目标（Target）在一些 Grunt 任务中用于允许多个多个构建环境。你可以通过使用智能感知来查看内建的目标（built-in targets），或者分配你自己的目标。

9. Add the ``jshint`` task using the code below. 

9. 使用下列代码添加 ``jshint`` 任务。

The jshint code-quality utility is run against every JavaScript file found in the temp directory.

将对临时目录中找到的每一个 JavaScript 文件运行 jshint 代码质量工具。

.. code-block:: javascript

  jshint: {
    files: ['temp/*.js'],
    options: {
      '-W069': false,
    }
  },

.. note:: The option "-W069" is an error produced by jshint when JavaScript uses bracket syntax to assign a property instead of dot notation, i.e. ``Tastes["Sweet"]`` instead of ``Tastes.Sweet``. The option turns off the warning to allow the rest of the process to continue.

.. note:: 选项「-W0069」是由 jshint 产生的错误，当 JavaScript 使用括号语法分配属性而不是使用点符号（比如用 ``Tasted["Sweet"]`` 取代 ``Tasted.Sweet``）。该选项关闭警告以允许继续进行剩下的过程。

10. Add the ``uglify`` task using the code below. 

10. 使用以下代码增加 ``uglify`` 任务。

The task minifies the combined.js file found in the temp directory and creates the result file in wwwroot/lib following the standard naming convention <file name>.min.js.

该任务将使发现于临时目录下的 combined.js 文件最小化，并按约定的 <file name>.min.js 标准命名格式将结果文件创建到 wwwroot/lib 中。

.. code-block:: javascript

  uglify: {
    all: {
      src: ['temp/combined.js'],
      dest: 'wwwroot/lib/combined.min.js'
    }
  },

11. Under the call grunt.loadNpmTasks() that loads grunt-contrib-clean, include the same call for jshint, concat and uglify using the code below.

11. 在加载 grunt-contrib-clean 的 grunt.loadNpmTasks() 调用之下，使用如下代码包含对 jshint、concat 以及 uglify 的相同调用。

.. code-block:: javascript

  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-uglify');

12. Save ``Gruntfile.js``. The file should look something like the example below.

12. 保存 ``Gruntfile.js``。文件看上去类似下图所示。

.. image:: using-grunt/_static/gruntfile-js-complete.png
 
13. Notice that the Task Runner Explorer Tasks list includes ``clean``, ``concat``, ``jshint`` and ``uglify`` tasks. Run each task in order and observe the results in Solution Explorer. Each task should run without errors.

13. 请注意 Task Runner Explorer 的 Tasks 列表包含 ``clean``、``concat``、``jshint`` 和 ``uglify`` 任务。按顺序运行每个任务，并在 Solution Explorer 中观察结果。每个任务应该都不会运行报错。

.. image:: using-grunt/_static/task-runner-explorer-run-each-task.png

The concat task creates a new combined.js file and places it into the temp directory. The jshint task simply runs and doesn’t produce output. The uglify task creates a new combined.min.js file and places it into wwwroot/lib. On completion, the solution should look something like the screenshot below:

concat 任务创建新的 combind.js 文件并将其放置于临时目录中。jshint 任务只是简单运行且不会生成任何输出。uflify 任务创建一个新的 combind.min.js 文件并将之放入 wwwroot/lib。完成后，解决方案看上去将如下图所示：

.. image:: using-grunt/_static/solution-explorer-after-all-tasks.png

.. note:: For more information on the options for each package, visit https://www.npmjs.com/ and lookup the package name in the search box on the main page. For example, you can look up the grunt-contrib-clean package to get a documentation link that explains all of its parameters.

.. note:: 更多关于每个包的选项信息，请访问 https://www.npmjs.com/ 并在主页上搜索包名。例如你可以查找 grunt-contrib-clean 包以获取解释其所有参数的文档链接。

万事就绪
^^^^^^^^^^^^^^^^

Use the Grunt ``registerTask()`` method to run a series of tasks in a particular sequence. For example, to run the example steps above in the order clean -> concat -> jshint -> uglify, add the code below to the module. The code should be added to the same level as the loadNpmTasks() calls, outside initConfig.

使用 Grunt ``registerTask()`` 方法来以一定的顺序运行一组任务。比如按顺序运行上例中的步骤：clean -> concat -> jshint -> uglify，将以下代码添加到模块中。这段代码会在 initConfig 外添加与 loadNpmTasks() 同级别的调用。

.. code-block:: javascript

  grunt.registerTask("all", ['clean', 'concat', 'jshint', 'uglify']);

The new task shows up in Task Runner Explorer under Alias Tasks. You can right-click and run it just as you would other tasks. The ``all`` task will run ``clean``, ``concat``, ``jshint`` and ``uglify``, in order. 

新任务在 Task Runner Explorer 中显示，并显示在 Alias 任务下面。请右键单击并运行（和运行别的任务一样）。``all`` 任务将按顺序运行 ``clean``、``concat``、``jshint`` 以及 ``uglify``。

.. image:: using-grunt/_static/alias-tasks.png

变更监视
--------------------

A ``watch`` task keeps an eye on files and directories. The watch triggers tasks automatically if it detects changes. Add the code below to initConfig to watch for changes to \*.js files in the TypeScript directory. If a JavaScript file is changed, ``watch`` will run the ``all`` task.

``watch`` 任务用于监视文件和目录。如果检测到改变，watch 会自动触发任务。将下列代码添加到 initConfig 中以监视 TypeScript 目录中的 \*.js 文件变更。

.. code-block:: javascript

  watch: {
    files: ["TypeScript/*.js"],
    tasks: ["all"]
  }

Add a call to ``loadNpmTasks()`` to show the ``watch`` task in Task Runner Explorer. 

添加对 ``loadNpmTasks()`` 的调用，用于在 Task Runner Explorer 面板中显示 ``watch`` 任务。

.. code-block:: javascript

  grunt.loadNpmTasks('grunt-contrib-watch');

Right-click the watch task in Task Runner Explorer and select Run from the context menu. The command window that shows the watch task running will display a "Waiting…" message. Open one of the TypeScript files, add a space, and then save the file. This will trigger the watch task and trigger the other tasks to run in order. The screenshot below shows a sample run.

在 Task Runner Explorer 面板中右键点击 watch 任务并在上下文菜单中选择 Run。命令窗口将显示 watch 任务运行的消息：「Waiting...」。打开某一个 TypeScript 文件，添加一个空格，然后保存。接着就会触发 watch 任务，并触发其他任务按顺序运行。下方截图展示了一个示例运行。

.. image:: using-grunt/_static/watch-running.png

绑定到 Visual Studio 事件
-------------------------------

Unless you want to manually start your tasks every time you work in Visual Studio, you can bind tasks to **Before Build**, **After Build**, **Clean**, and **Project Open** events. 

除非你想每次都从 Visual Studio 中手动启动任务，不然你可以将任务绑定到 **Before Build**、**After Build**、**Clean** 以及 **Project Open** 事件上。

Let’s bind ``watch`` so that it runs every time Visual Studio opens. In Task Runner Explorer, right-click the watch task and select **Bindings > Project Open** from the context menu. 

我们绑定 ``watch``，这样一来它就能在 Visual Studio 启动时自动运行。在 Task Runner Explorer 中，右键单击 watch 任务并在上下文菜单中选择 **Bindings > Project Open**。

.. image:: using-grunt/_static/bindings-project-open.png

Unload and reload the project. When the project loads again, the watch task will start running automatically.

卸载并重新加载项目。当项目重新加载，watch 任务将自动开始运行。

总结
-------

Grunt is a powerful task runner that can be used to automate most client-build tasks. Grunt leverages NPM to deliver its packages, and features tooling integration with Visual Studio. Visual Studio's Task Runner Explorer detects changes to configuration files and provides a convenient interface to run tasks, view running tasks, and bind tasks to Visual Studio events.

Grunt 是一个强大的任务运行器，它能被用于执行大多数客户端构建的自动化任务。Grunt 利用 NPM 来交付包，并与 Visual Studio 工具集成。Visual Studio 的 Task Runner Explorer 能检测配置文件的改变并提供方便的界面来运行任务，查看运行中的任务以及绑定任务到 Visuao Studio 事件。

相关阅读
--------

  - :doc:`using-gulp`


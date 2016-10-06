Bundling（打包） 和 Minification（压缩）
==================================================

By `Rick Anderson`_, `Erik Reitan`_ and `Daniel Roth`_

Bundling（打包） 和 Minification（压缩）是 ASP.NET 里面用来提升你的Web应用程序页面加载性能的技术手段. Bundling（打包）可以把多个文件合成到一个文件. Minification（压缩）主要工作是压缩脚本和 CSS 的体积, 使得其易于加载. 当着两个技术同时使用是, Bundling（打包） 和 Minification（压缩） 通过减少对服务器的请求的数量和减少所请求的资源文件(例如 CSS 和 JavaScript 脚本)的大小来改善负载时的性能 .

下面的文章解释了试用 Bundling（打包） 和 Minification（压缩）的好处, 包括如何在 ASP.NET Core 应用程序中启用该功能。

.. contents:: 章节:
  :local:
  :depth: 1

总览
--------

在ASP.NET Core 应用程序中, 可以在设计时使用第三方工具打包和压缩客户端资源, 压缩文件在应用程序部署前被创建。 部署前打包和压缩有助于减少服务器负载. 但是, 必须意识到设计时打包和压缩增加编译工作的复杂性和而且仅仅只能对静态文件生效。


打包和压缩主要是改善页面第一次请求请求加载时间。 一旦Web页面被请求, 浏览器会缓存资源文件 (JavaScript, CSS and 图片) 所有当再次请求同一个页面、获取同一个网站请求同样的资源文件的时候打包和压缩无法获取更快的加载速度. 如果你没有正确的设置你资源文件的过期头信息, 并且你也没有使用压缩和打包,几天以后浏览器的刷新机制会把资源文件设置为过期，并且浏览器会重新请求资源文件. 在这种情况下, Bundling（打包） 和 Minification（压缩）可以在首次访问资源之后依然能够提升性能.

Bundling（打包）
----------------

Bundling（打包）是一个可以轻松的把多个文件合并成一个文件功能。 因为Bundling（打包）可以将多个文件合并, 这可以有效降低查看显示页面资源（例如 Web 页面）所需要的请求数量。你可以创建 CSS, JavaScript 或者其他文件的打包，更少的文件意味着更少的浏览器到服务器或者应用程序服务的所需的 HTTP 请求。这将有效的提升页面首次加载性能。

Bundling（打包）功能通过使用 `gulp-concat <https://www.npmjs.com/package/gulp-concat>`__ 插件完成，该插件通过 Node 包管理器 (`npm <https://www.npmjs.com/>`__) 安装。在你的 *package.json* 文件的 ``devDependencies`` 配置节中添加 ``gulp-concat`` 包.在解决方案管理器里面的 **Dependencies** 菜单里右击 **npm** 节点选择 **Open package.json** 来在 Visual Studio 里面编辑你的项目里面的 *package.json* 文件:

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/package.json
  :language: json
  :emphasize-lines: 7

运行 ``npm install`` 安装指定的包 . 只要 *package.json* 文件发生变动 Visual Studio 会自动安装 npm 包。

在 *gulpfile.js* 文件中导入 ``gulp-concat`` 模块:

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/gulpfile.js
  :language: js
  :lines: 4-8
  :emphasize-lines: 3

使用 `globbing <http://www.tldp.org/LDP/abs/html/globbingref.html>`__ 模式来制定你所需要打包或者压缩的文件:

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/gulpfile.js
  :language: js
  :lines: 12-19

你可以定义 gulp 任务在目标文件上 运行 ``concat`` 来输出文件到根目录 :

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/gulpfile.js
  :language: js
  :lines: 31-43
  :emphasize-lines: 3, 10

The `gulp.src <https://github.com/gulpjs/gulp/blob/master/docs/API.md#gulpsrcglobs-options>`__ function emits a stream of files that can be `piped <http://nodejs.org/api/stream.html#stream_readable_pipe_destination_options>`__ to gulp plugins. An array of globs specifies the files to emit using `node-glob syntax <https://github.com/isaacs/node-glob>`__. The glob beginning with ``!`` excludes matching files from the glob results up to that point.

Minification（压缩）
------------------------


压缩执行各种代码优化来减少请求所需的资源（如CSS，图像，JavaScript文件）的文件大小。常用的优化方式是移除不必要的空格和注释，以及缩短变量名到一个字符。

请看下面的 JavaScript 函数:

.. code-block:: javascript

  AddAltToImg = function (imageTagAndImageID, imageContext) {
    ///<signature>
    ///<summary> Adds an alt tab to the image
    // </summary>
    //<param name="imgElement" type="String">The image selector.</param>
    //<param name="ContextForImage" type="String">The image context.</param>
    ///</signature>
    var imageElement = $(imageTagAndImageID, imageContext);
    imageElement.attr('alt', imageElement.attr('id').replace(/ID/, ''));
  }

经过压缩, 函数被缩减到以下的代码 :

.. code-block:: javascript

  AddAltToImg=function(t,a){var r=$(t,a);r.attr("alt",r.attr("id").replace(/ID/,""))};

除了移除不必要的空格和注释, 下面的参数和变量名被重命名（缩短）:

==================  =======
原始                重命名
==================  =======
imageTagAndImageID  t
imageContext        a
imageElement        r
==================  =======

你可以使用 `gulp-uglify <https://www.npmjs.com/package/gulp-uglify>`__ 插件来压缩的你的 JavaScript 脚本. 对于 CSS 可以使用 `gulp-cssmin <https://www.npmjs.com/package/gulp-cssmin>`__ 插件. 在此之前使用 npm 安装以下包 :

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/package.json
  :language: json
  :emphasize-lines: 8-9

把 ``gulp-uglify`` 以及 ``gulp-cssmin`` 模块导入到你的 *gulpfile.js* 文件:

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/gulpfile.js
  :language: js
  :lines: 4-8
  :emphasize-lines: 4, 5

添加 ``uglify`` 来压缩打包你的 JavaScript 文件、 ``cssmin`` 用来压缩打包 CSS 文件.

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/gulpfile.js
  :language: js
  :lines: 31-43
  :emphasize-lines: 4, 11

在命令行中试用压缩打包任务可以用 gulp (``gulp min``)，或者在 Visual Studio 的 **Task Runner Explorer** 中执行各种 gulp 任务。要使用 **Task Runner Explorer** 在解决方案资源管理器中选择 *gulpfile.js* 文件并点击 **Tools > Task Runner Explorer**:

.. image:: bundling-and-minification/_static/task-runner-explorer.png

.. note:: 当你的项目在被 Build 的时候 gulp 压缩打包任务不会自动运行需要手工操作。

压缩和打包的效果
-----------------------------------


下表列出在一个简单的页面上使用了压缩打包技术后资源文件访问变化情况:

==================  ==========  ============  ============
操作                启用 B/M     未启用 B/M    变化
==================  ==========  ============  ============
File Requests       7           18            157%
KB Transferred      156         264.68        70%
Load Time (MS)      885         2360          167%
==================  ==========  ============  ============

在指定的浏览器中头文件发送的字节数显著的减少了. 加载时间有了巨大的提升,但是这个例子仅仅是本地运行的. 如果是在网络环境下对资源文件压缩打包会获取更好的性能。

控制压缩和打包
-------------------------------------

一般情况下，你只需要在生产环境中对您的应用程序的资源文件进行压缩打包。在开发环境中，应该试用原始文件，以便您的应用程序更容易调试。

您可以在页面中使用 environment tag-helper (更多参考 :doc:`/mvc/views/tag-helpers/index`) 来指定哪些脚本和 CSS 文件.  environment tag helper 将会在指定的环境呈现对应的内容 . 如何指定当前环境请参考 :doc:`/fundamentals/environments` .


在 ``开发`` 环境中运行时，以下 environment tag 将呈现未处理的CSS文件：

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/Views/Shared/_Layout.cshtml
  :language: html
  :linenos:
  :lines: 8-11
  :dedent: 4
  :emphasize-lines: 3


在 ``生产`` or ``迭代`` 环境运行时，这个 environment tag 只会显示压缩打包的 CSS 文件：

.. literalinclude:: bundling-and-minification/samples/WebApplication1/src/WebApplication1/Views/Shared/_Layout.cshtml
  :language: html
  :linenos:
  :lines: 12-17
  :dedent: 4
  :emphasize-lines: 5

参考资源
----------------
- :doc:`using-gulp`
- :doc:`using-grunt`
- :doc:`/fundamentals/environments`
- :doc:`/mvc/views/tag-helpers/index`

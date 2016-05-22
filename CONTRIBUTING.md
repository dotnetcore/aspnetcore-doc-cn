<!--# Contributing #-->

# 贡献 #

翻译：[刘怡(AlexLEWIS)](http://github.com/alexinea)

校对：[刘浩扬(Landiro)](https://github.com/liuhaoyang)

<!--Information on contributing to this repo is in the [Contributing Guide](https://github.com/aspnet/Home/blob/dev/CONTRIBUTING.md) in the Home repo.-->

关于向本仓库贡献的详细阐述位于 aspnet 组织 Home 仓库的 [贡献指南（Contributing Guide）](https://github.com/aspnet/Home/blob/dev/CONTRIBUTING.md) 中。

<!--The documentation is built using [Sphinx](http://sphinx-doc.org) and [reStructuredText](http://sphinx-doc.org/rest.html), and then hosted by [ReadTheDocs](http://aspnet.readthedocs.org).-->

文档基于 [Sphinx](http://sphinx-doc.org) 和 [reStructuredText](http://sphinx-doc.org/rest.html) 生成，并托管于 [ReadTheDocs](http://aspnet.readthedocs.org)。

<!--## Video: Getting Started ##-->
## 视频：入门 ##

<!--[Watch a video](http://ardalis.com/contributing-to-asp-net-5-documentation) showing how to get started building the documentation locally.-->

[请观看本视频](http://ardalis.com/contributing-to-asp-net-5-documentation)，视频展示了如何在本地生成文档。

<!--## Building the Docs ##-->
## 生成文档 ##

<!--Once you have cloned the Docs to your local machine, the following instructions will walk you through installing the tools necessary to build and test.-->

一旦你将 Docs 项目克隆（clone）到本地计算机，按照下面步骤安装必要的工具，你就能生成文档并测试。

<!--
1. [Download python](https://www.python.org/downloads/) version 2.7.10 or higher (Version 3.4 is recommended).
2. If you are installing on Windows, ensure both the Python install directory and the Python scripts directory have been added to your `PATH` environment variable. For example, if you install Python into the c:\python34 directory, you would add `c:\python34;c:\python34\scripts` to your `PATH` environment variable.
3. Install Sphinx by opening a command prompt and running the following Python command. (Note that this operation might take a few minutes to complete.)
4. By default, when you install Sphinx, it will install the ReadTheDocs custom theme automatically. If you need to update the installed version of this theme, you should run:
5. Install the Sphinx .NET domain:
6. Navigate to one of the main project subdirectories in the Docs repo - such as `mvc`, `aspnet`, or `webhooks`.
7. Run ``make`` (make.bat on Windows, Makefile on Mac/Linux)
8. Once make completes, the generated docs will be in the .../docs/<project>/_build/html directory. Simply open the `index.html` file in your browser to see the built docs for that project.
-->


1. [下载 python](https://www.python.org/downloads/) 2.7.10+（推荐安装 3.4）。

2. 如果你是在 Windows 上安装，请同时确认 Python 的安装目录和 Python scripts 目录已被添加到 `PATH` 环境变量中。比如你把 Python 安装到了 C:\python34 目录，那么你需要把 `c:\python34;c:\python34\scripts` 添加到 `PATH` 环境变量中。

3. 通过命令行提示安装 Sphinx ，需要在 Python 命令下运行：（注，此操作需花费几分钟）

    ```pip install sphinx```
	
4. 默认情况下，当你安装 Sphinx 时会自动安装 ReadTheDocs 风格的自定义主题。如果你要更新已安装的该主题版本，可以这样：

    ```pip install -U sphinx_rtd_theme```
	
5. 安装 Sphinx .NET domain：

    ```pip install sphinxcontrib-dotnetdomain```
	
6. 导航到 Docs 仓库主项目的子目录中的一个，比如 `mvc`、`aspnet` 或 `webhooks`。

7. 运行 ``make``（Windows 上的 make.bat，Mac/Linux 上的 Makefile）

    ```make html```
	
8. 当 make 完成后，生成的文档将位于 .../docs/&lt;project&gt;/_build/html 目录下。直接用浏览器打开 `index.html` 就能看到所生成的该项目的文档了。

<!--## Use autobuild to easily view site changes locally ##-->
## 使用自动生成，轻松在本地浏览 ##

<!--
You can also install [sphinx-autobuild](https://github.com/GaretJax/sphinx-autobuild) which will run a local web server and automatically refresh whenever changes to the source files are detected. To do so:
-->

安装 [sphinx-autobuild](https://github.com/GaretJax/sphinx-autobuild) 后，将在本地运行一个 web 服务器，当源文件发生变化时将自动更新，只需要这么做：
   
<!--
1. Install sphinx-autobuild
2. Navigate to one of the main project subdirectories in the Docs repo - such as `mvc`, `aspnet`, or `webhooks`.
3. Run ``make`` (make.bat on Windows, Makefile on Mac/Linux)
4. Browse to `http://127.0.0.1:8000` to see the locally built documentation. 
5. Hit `^C` to stop the local server.
-->

1. 安装 sphinx-autobuild

    ```pip install sphinx-autobuild```
	
2. 导航到 Docs 仓库主项目的子目录中的一个，比如 `mvc`、`aspnet` 或 `webhooks`。

3. 运行 ``make``（Windows 上的 make.bat，Mac/Linux 上的 Makefile）

    ```make livehtml```
	
4. 访问 `http://127.0.0.1:8000` 阅读本地生成的文档。

5. 敲击 `^C` 关闭本地服务器。

<!--## Adding Content ##-->
## 增加内容 ##

<!--
Before adding content, submit an issue with a suggestion for your proposed article. Provide detail on what the article would discuss, and how it would relate to existing documentation.
-->

在添加内容之前，把你打算写的文章的一些想法以问题（issue）的形式提交上去，里面提供文章将要讨论的详细内容，以及它将如何与现有的文档相关联。

<!--
Also, please review the following style guides:
-->

同样的，请回顾一下这些风格指南：

<!--
- [Sphinx Style Guide](http://documentation-style-guide-sphinx.readthedocs.org/en/latest/style-guide.html)
- [ASP.NET Docs Style Guide](http://docs.asp.net/en/latest/contribute/style-guide.html)
-->

- [Sphinx 风格指南](http://documentation-style-guide-sphinx.readthedocs.org/en/latest/style-guide.html)
- [ASP.NET 文档风格指南](http://docs.asp.net/en/latest/contribute/style-guide.html)

<!--
Articles should be organized into logical groups or sections. Each section should be given a named folder (e.g. /yourfirst). That section contains the rst files for all articles in the section. For images and other static resources, create a subfolder that matches the name of the article. Within this subfolder, create a ``sample`` folder for code samples and a  ``_static`` folder for images and other static content.
-->

文章应该在逻辑上被组织成若干个组或节，每一个节都需要指定一个具名文件夹（比如 /yourfirst）。该节包含了所有文章的 rst 格式文件。对于图片和其它静态资源而言，创建一个子文件夹（名字与文章的名字一致）。在这个子文件夹中，创建一个 ``smaple`` 文件夹来放代码样例，以及一个 ``_static`` 文件夹来放图片和其它静态内容。

<!--### Example Structure ###-->
### 结构举例 ###

	docs
		/client-side
			/angular
				/_static
					controllers.png
					events.png
					...
				/sample
					(sample code)
			/bootstrap
				/_static
					about-page.png
					...
			angular.rst
			bootstrap.rst

<!--
**Note:** Sphinx will automatically fix duplicate image names, such as the about-page.png files shown above. There is no need to try to ensure uniqueness of static files beyond an individual article.
-->

**注意：** Sphinx 会自动填充同名图片，比如上文所示的 about-page.png 文件。这表明我们不再需要去刻意检查每个文章的静态资源是否唯一的了。

<!--
Author information should be placed in the _authors folder following the example of steve-smith.rst. Place photos in the photos folder - size them to be no more than 125px wide or tall.
-->

作者信息需要放在 _authors 文件夹下，具体参考 steve-smith.rst。把照片都放进 photos 文件夹，并将照片的尺寸控制下长宽均小于 125 像素以内。

<!--## Process for Contributing ##-->
## 贡献的步骤##

<!--
**Step 1:** Open an Issue describing the article you wish to write and how it relates to existing content. Get approval to write your article.
-->

**第一步：** 新建一个 Issue，给我们描述一下你想写的文章以及它与现有内容之间的关联。如果我们同意了，你就可以去写这篇文章了。

<!--**Step 2:** Fork the `/aspnet/docs` repo.-->

**第二步：** fork `/aspnet/docs` 仓库。

<!--**Step 3:** Create a `branch` for your article.-->

**第三步：** 给你的文章创建一个`分支（branch）`

<!--
**Step 4:** Write your article, placing the article in its own folder and any needed images in a _static folder located in the same folder as the article. Be sure to follow the [ASP.NET Docs Style Guide](http://docs.asp.net/en/latest/contribute/style-guide.html). If you have code samples, place them in a folder within the `/samples/` folder.
-->

**第四步：** 开始编写。把文章放在对应的文件夹下，所有的图片放在文件夹下的 _static 子文件夹中。确保你已遵循了 [ASP.NET 文档风格指南](http://docs.asp.net/en/latest/contribute/style-guide.html) 的要求。如果你还附有代码样例，则把样例放在 `/samples/` 文件夹内。

<!--
**Step 5:** Submit a Pull Request from your branch to `aspnet/docs/master`.
-->

**第五步：** 从你的分支提交 Pull Request 到 `aspnet/docs/master`。

<!--
**Step 6:** Discuss the Pull Request with the ASP.NET team; make any requested updates to your branch. When they are ready to accept the PR, they will add a :shipit: (`:shipit:`) comment.
-->

**第六步：** 针对提交的信息与 ASP.NET 团队展开有关 Pull Request（PR） 的讨论。若是他们决定接受你的请求，则会给你个 :shipit: (`:shipit:`) 评论。

<!--
**Step 7:** The last step before your Pull Request is accepted is to [squash all commits](http://stackoverflow.com/questions/14534397/squash-all-my-commits-into-one-for-github-pull-request) into a single commit message. Do this in your branch, using the `rebase` git command. For example, if you want to squash the last 4 commits into a single commit, you would use:
-->

**第七步：** 接受 Pull Request 前的最后步骤是 [合并所有提交](http://stackoverflow.com/questions/14534397/squash-all-my-commits-into-one-for-github-pull-request)。这一操作在你的分支项目中完成，使用 `rebase` 指令。举个例子，如果你想把最后四个提交合并为一个提交，你可以这么做：

	git rebase -i HEAD~4


<!--
The `-i` option stands for "interactive" and should open a text editor showing the last N commits, preceded with "pick ".  Change all but the first instance of "pick " to "squash " and save the file and exit the editor. A more detailed answer is [available here](http://stackoverflow.com/a/6934882).
-->

`-i` 参数表示「interactive」，它会用记事本软件显示最后 N 条以「pick」开头的提交。把除了第一个以外的所有「pick」改为「squash」，保存文件并退出编辑器（[更多信息请访问这里](http://stackoverflow.com/a/6934882)）。


<!--## Common Pitfalls ##-->
##常见错误##

<!--Below are some common pitfalls you should try to avoid:-->

下列错误你当竭力避免：

<!--
- Don't forget to submit an issue before starting work on an article
- Don't forget to create a separate branch before working on your article
- Don't update or `merge` your branch after you submit your pull request
- Don't forget to squash your commits once your pull request is ready to be accepted
- If updating code samples in `/samples/`, be sure any line number references in your article remain correct
-->

- 写文章之前别忘了先提交一个 Issue；
- 别忘记先建立独立的分支项目，再写文章；
- 当你提交了 pull request 后，不要再去更新或 `merge` 你的分支；
- 一旦 pull request 被接受，及时跟进合并提交；
- 如果你更新了 `/samples/` 下的代码样例，一定要再三确保你在文章中所引用的行号是正确无误的。

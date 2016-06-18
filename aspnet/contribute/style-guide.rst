.. _style-guide:

ASP.NET Docs Style Guide 
========================

ASP.NET 文档风格指南
======================

作者： `Steve Smith`_

翻译： `刘怡(AlexLEWIS) <http://github.com/alexinea>`_

校对： `孟帅洋(书缘) <https://github.com/mengshuaiyang>`_

This document provides an overview of how articles published on `docs.asp.net <http://docs.asp.net>`_ should be formatted. You can actually use this file, itself, as a template when contributing articles.

本文档提供了有关在 `docs.asp.net <http://docs.asp.net>`_ 发布文章所应遵循的格式的概览。实际上你贡献文章时，你可直接把本文件作为模板使用。

.. contents:: Sections:
  :local:
  :depth: 1

.. contents:: 章节:
  :local:
  :depth: 1

Article Structure
-----------------

文章结构
-----------------

Articles should be submitted as individual text files with a **.rst** extension. Authors should be sure they are familiar with the `Sphinx Style Guide <http://documentation-style-guide-sphinx.readthedocs.org/en/latest/style-guide.html>`_, but where there are disagreements, this document takes precedence. The article should begin with its title on line 1, followed by a line of === characters. Next, the author should be displayed with a link to an author specific page (ex. the author's GitHub user page, Twitter page, etc.). 

文章须以单独文本的形式被提交，其文件扩展名为 **.rst**。作者首先要熟悉一下 `Sphinx 风格指南 <http://documentation-style-guide-sphinx.readthedocs.org/en/latest/style-guide.html>`_，但如果出现冲突歧义的，以本文档为准。文章开头第一行是标题，然后是一行 === 字符，接着显示作者信息并链接到作者页（比如作者的 GigHub 用户也、Twitter 页等）。

Articles should typically begin with a brief abstract describing what will be covered, followed by a bulleted list of topics, if appropriate. If the article has associated sample files, a link to the samples should be included following this bulleted list. 

文章通常以一段文章摘要作为开头。如果合适的话，紧随其后的是主题清单（list of topics）。如果文章有关联的样例文件，主题清单内的内容会被替换为该样例文件的链接。

Articles should typically include a Summary section at the end, and optionally additional sections like Next Steps or Additional Resources. These should not be included in the bulleted list of topics, however.

文章结尾处需要包含一个总结（Summary），另外也可以包含几个可选的节，比如下一步（Next Steps）或扩展资源（Additional Resources）。不过这不会包含在所生成的主题清单内。

Headings
^^^^^^^^

标题
^^^^^^^^

Typically articles will use at most 3 levels of headings. The title of the document is the highest level heading and must appear on lines 1-2 of the document. The title is designated by a row of === characters.

文章通常使用至多三级标题，文档的标题是最高级别的标题，它必须出现在文档的第一行和第二行。标题通过一行 === 符号来指定。

Section headings should correspond to the bulleted list of topics set out after the article abstract. `Article Structure`, above, is an example of a section heading. A section heading should appear on its own line, followed by a line consisting of --- characters.

节标题对应于文章摘要下方的主题列表。上面的 `文章结构` 就是一个节标题的例子。节标题必须独占一行，然后紧跟着一行 --- 字符。

Subsection headings can be used to organize content within a section. `Headings`, above, is an example of a subsection heading. A subsection heading should appear on its own line, followed by a line of ^^^ characters.

分段标题可以用于组织一节内的内容。上面的 `标题` 就是个分段标题的例子。分段标题也必须独占一行，然后紧跟着一行 ^^^ 字符。


.. code-block:: rst

  文章标题 Title (H1)
  ===================

  节标题 Section heading (H2)
  -----------------------------

  分段标题 Subsection heading (H3)
  ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

For section headings, only the first word should be capitalized:

对节标题来说，只有首字母是大写的（当然，这种情况一般只用于英文版的文档——译者注）：

- Use this heading style
- Do Not Use This Style 

More on sections and headings in ReStructuredText:
http://sphinx-doc.org/rest.html#sections

更多关于 ReStructuredText 节和标题的资料：
http://sphinx-doc.org/rest.html#sections

ReStructuredText Syntax
-----------------------

ReStructuredText 语法
-----------------------

The following ReStructuredText elements are commonly used in ASP.NET documentation articles. Note that **indentation and blank lines are significant!**

下列 ReStructuredText 元素被大量用在 ASP.NET 文档的文章内，特别注意，**缩进和空行都是有意义的！**

Inline Markup
^^^^^^^^^^^^^

内嵌标记
^^^^^^^^^^^^^

Surround text with:

放置在文本的两侧：

- One asterisk for \*emphasis\* (*italics*)
- Two asterisks for \**strong emphasis\** (**bold**)
- Two backticks for \``code samples``\ (an ``<html>`` element)

- 一个星号，形如 \*emphasis\* (*倾斜*)
- 两个星号，形如 \**strong emphasis\** (**加粗**)
- 两个反引号，形如 \``code samples``\ (比如一个 ``<html>`` 元素)

.. note:: Inline markup cannot be nested, nor can surrounded content start or end with whitespace (``* foo*`` is wrong).

.. note:: 内嵌标记不能嵌套，也不能放置在开头结尾包含空格的内容两边（比如 ``* foo*`` 就是错误的）。

Escaping is done using the ``\`` backslash.

转义使用 ``\`` 反斜杠。

Format specific items using these rules:

具体的格式化规则：

- *Italics* (surround with \*)
  - Files, folders, paths (for long items, split onto their own line)
  - New terms
  - URLs (unless rendered as links, which is the default)
- **Strong** (surround with \**)
  - UI elements
- ``Code Elements`` (surround with \``)
  - Classes and members
  - Command-line commands
  - Database table and column names
  - Language keywords

- *斜体* （用 \* 包裹）
  - 文件、文件夹、路径（如果比较长，独占一行）
  - 新术语
  - URLs（除非默认地作为链接来显示）
- **加重** （用 \** 包裹）
  - UI 元素
- ``代码元素`` （用 \`` 包裹）
  - 类和成员
  - 命令行（Command-line）命令
  - 数据库表和字段名
  - 编程语言的关键词

Links
^^^^^

链接
^^^^^

Links should use HTTPS when possible. Inline hyperlinks are formatted like this:

链接尽可能的使用 HTTPS。内联式超链的格式如下：

.. code-block:: rst

  Learn more about `ASP.NET <http://www.asp.net>`_.
  欲知详情，请前往 `ASP.NET <http://www.asp.net>`_。

Learn more about `ASP.NET <http://www.asp.net>`_.

欲知详情，请前往 `ASP.NET <http://www.asp.net>`_。

Surround the link text with backticks. Within the backticks, place the target in angle brackets, and ensure there is a space between the end of the link text and the opening angle bracket. Follow the closing backtick with an underscore.

链接文本用反引号包裹，里面的目标地址使用角括号包裹。请确保链接文本和开门角括号之间有一个空格，在关门反引号后面有个下划线。

In addition to URLs, documents and document sections can also be linked by name:

除了 URLs，文档和文档的节也可以通过名称来链接：

.. code-block:: rst

  For example, here is a link to the `Inline Markup`_ section, above.
  打个比方，此处可以前往上面的 `Inline Markup`_ 一节。

For example, here is a link to the `Inline Markup`_ section, above.

打个比方，此处可以前往上面的 `Inline Markup`_ 一节。

Any element that is rendered as a link should not have any additional formatting or styling.

任何显示为链接的元素都不能附带有其他的格式或样式。

Lists
^^^^^

列表
^^^^^

Lists can be started with a ``-`` or ``*`` character:

列表启于 ``-`` 或 ``*`` 字符：

.. code-block:: rst

  - This is one item
  - This is a second item

.. code-block:: rst

  - 这是第一点
  - 这是第二点

Numbered lists can start with a number, or they can be auto numbered by starting each item with the \# character. Please use the \# syntax.

编号列表启于数值，如果每一项的开头使用 \# 字符则能够给他们自动编号。请使用 \# 语法。

.. code-block:: rst

  1. Numbered list item one.(don't use numbers)
  2. Numbered list item two.(don't use numbers)

  #. Auto-numbered one.
  #. Auto-numbered two.


.. code-block:: rst

  1. 这是第一点（请不要使用数值）
  2. 这是第二点（请不要使用数值）

  #. 这是自动编号的第一点。
  #. 这是自动编号的第二点。

Source Code
^^^^^^^^^^^

源代码
^^^^^^^^^^^

Source code is very commonly included in these articles. Images should never be used to display source code. Prefer ``literalinclude`` for most code samples. Reserve ``code-block`` for small snippets that are not included in the sample project. A ``code-block`` can be declared as shown below, including spaces, blank lines, and indentation:

源代码经常被包含在文章中，禁止使用图片来显示源代码。大多数代码样例请使用 ``literalinclude``，把 ``code-block`` 保留给简单的独立代码段。``code-block`` 声明了代码在下方显示、空一行并且使用缩进：

.. code-block:: rst

  .. code-block:: c#

  public void Foo()
  {
    // Foo all the things!
  }

This results in:

其结果为：

.. code-block:: c#

  public void Foo()
  {
    // Foo all the things!
  }

The code block ends when you begin a new paragraph without indentation. `Sphinx supports quite a few different languages <http://pygments.org/docs/lexers/>`_. Some common language strings that are available include:

代码块止于不带缩进的新段落。`Sphinx 支持多种编程语言 <http://pygments.org/docs/lexers/>`_。一些常用的且可被识别的语言包括：

- ``c#``
- ``javascript``
- ``html``

.. _Captions: 

.. _标题定位: 

Line numbers should only be used while editing to assist in find the line numbers to emphasize. Code blocks also support line numbers and emphasizing or highlighting certain lines:

在编辑时可以通过使用行号来强调指定行。代码块同样支持一组行号以便突出和高亮某几行：

.. code-block:: rst

  .. code-block:: c#
    :linenos:
    :emphasize-lines: 3

    public void Foo()
    {
      // Foo all the things!
    }

This results in:

其结果为：

.. code-block:: c#
  :linenos:
  :emphasize-lines: 3

  public void Foo()
  {
    // Foo all the things!
  }

.. note:: Once the ``emphasize-lines`` is determined, remove ``:linenos:``. When updating a doc, remove all occurrences of ``:linenos:``.

.. note:: 当 ``emphasize-lines`` 时确定的，那就移除 ``:linenos:``。当更新文档时，移除所有出现的 ``:linenos:``。

.. note:: ``caption`` and ``name`` will result in a code-block not being displayed due to our builds using a Sphinx version prior to version 1.3. If you don't see a code block displayed above this note, it's most likely because the version of Sphinx is < 1.3.

.. note:: 如果生成文档所用的 Sphinx 的版本早于 1.3，那么 ``caption`` 和 ``name`` 就不会输出为代码段了。如果你看不到注释上面的代码段，这极有可能是因为 Sphinx 版本低于 1.3。


Images
^^^^^^

图片
^^^^^^

Images such as screen shots and explanatory figures or diagrams should be placed in a ``_static`` folder within a folder named the same as the article file. References to images should therefore always be made using relative references, e.g. ``article-name/style-guide/_static/asp-net.png``. Note that images should always be saved as all lower-case file names, using hyphens to separate words, if necessary.

诸如截图、图示或图表等图片需要被放在 ``_static`` 文件夹下，这个文件夹位于一个与文章文件同名的父级文件夹内。同时要求使用相对路径来引用图片，比如 ``article-name/style-guide/_static/asp-net.png``。注意，图片应当被保存以全小写的文件名（如果必要，要求使用连字符（hyphens）来间隔各个单词）。

.. note:: Do not use images for code. Use ``code-block`` or ``literalinclude`` instead.

.. note:: 不要用图片作为代码。请使用 ``code-block`` 或 ``literalinclude`` 来代替。

To include an image in an article, use the ``.. image`` directive:

可以使用 ``.. image`` 指令引用一张图：

.. code-block:: rst

  .. image:: style-guide/_static/asp-net.png

.. note:: No quotes are needed around the file name.

.. note:: 文件名周围不需要包裹引号。

Here's an example using the above syntax:

这是上面语法的一个例子：

.. image:: style-guide/_static/asp-net.png

Images are responsively sized according to the browser viewport when using this directive. Currently the maximum width supported by the http://docs.asp.net theme is 697px.

使用本指令后，图片将根据浏览器视图自适应大小。目前 http://docs.asp.net 的最宽尺寸支持到 697px。

Notes
^^^^^

注释
^^^^^

To add a note callout, like the ones shown in this document, use the ``.. note::`` directive.

使用 ``.. note::`` 指令可以添加向下面这样的注意标注：

.. code-block:: rst

  .. note:: This is a note.

This results in:

.. note:: This is a note.

Including External Source Files
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

包含外部源文件
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

One nice feature of ReStructuredText is its ability to reference external files. This allows actual sample source files to be referenced from documentation articles, reducing the chances of the documentation content getting out of sync with the actual, working code sample (assuming the code sample works, of course). However, if documentation articles are referencing samples by filename and line number, it is important that the documentation articles be reviewed whenever changes are made to the source code, otherwise these references may be broken or point to the wrong line number. For this reason, it is recommended that samples be specific to individual articles, so that updates to the sample will only affect a single article (at most, an article series could reference a common sample). Samples should therefore be placed in a subfolder named the same as the article file, in a ``sample`` folder (e.g. ``/article-name/sample/``).

ReStructuredText 有个非常赞的地方，是它支持引用外部文件。这意味着文章可以引用实际的样例源文件，降低文档内容中出现实际有效的代码样例与实际内容不同步的情况。不过，如果文章通过文件名和行号来引用样例，那么下面这个问题就非常重要了：如果源代码发生了变化，就要去逐篇文章的检查，否则引用就会被破坏或是指向了错误的行。基于这个原因，推荐每个样例都只对应特定的单独文章，在更新样例时也就只会影响到这一篇文章（最多一个文章系列引用一个公共样例）。因此样本应该被放在一个与文章文件同名的文件内，作为名为 ``sample`` 的子文件夹中（比如，``/article-name/sample/``）。


External file references can specify a language, emphasize certain lines, display line numbers (recommended), similar to `Source Code`_. Remember that these line number references may need to be updated if the source file is changed.

外部文件引用能够指定语言、突出某行、显示行号（推荐），类似于 `Source Code`_。牢记，这些行号可能因为源文件的变化而变化。

.. code-block:: rst

  .. literalinclude:: style-guide/_static/startup.cs
    :language: c#
    :emphasize-lines: 19,25-27
    :linenos:

.. literalinclude:: style-guide/_static/startup.cs
  :language: c#
  :emphasize-lines: 19,25-27
  :linenos:

You can also include just a section of a larger file, if desired:

当然也可以只引用大文件中的一部分，如果这样写：

.. code-block:: rst
  :emphasize-lines: 3

  .. literalinclude:: style-guide/_static/startup.cs
    :language: c#
    :lines: 1,4,20-
    :linenos:

This would include the first and fourth line, and then line 20 through the end of the file.

就只会引用第 1 行和第 4 行、以及从第 20 行开始到文件结束的代码了。

Literal includes also support `Captions`_ and names, as with ``code-block`` elements. If the ``caption`` is left blank, the file name will be used as the caption. Note that captions and names are available with Sphinx 1.3, which the ReadTheDocs theme used by this system is not yet updated to support.

文本包含同样支持标题 `标题定位`_ 和名称，就和 ``code-block`` 元素一样。如果 ``caption`` 留空，则会使用文件名替换之。注意，标题和名称都被 Sphinx 1.3 所支持，但这些暂时还不被 ReadTheDocs 主题所支持。

Format code to eliminate or minimize horizontal scroll bars.

排版代码，让每行都尽可能短。

Tables
^^^^^^

表格
^^^^^^

Tables should never render with horizontal scroll bars. Tables can be constructed using grid-like "ASCII Art" style text. In general they should only be used where it makes sense to present some tabular data. Rather than include all of the syntax options here, you will find a detailed reference at `<http://docutils.sourceforge.net/docs/ref/rst/restructuredtext.html#grid-tables>`_.

表格不会渲染水平滚动条，你可使用类栅格化的「ASCII Art」风格文本来构造它，一般来说只是用有意义的表列数据。你能在 `<http://docutils.sourceforge.net/docs/ref/rst/restructuredtext.html#grid-tables>`_ 阅读完整参考。


UI navigation
^^^^^^^^^^^^^

UI 导航
^^^^^^^^^^^^^

When documenting how a user should navigate a series of menus, use the ``:menuselection:`` directive:

若想告告诉读者如何导航到多级菜单，可使用 ``:menuselection:`` 指令：

.. code-block:: rst

  :menuselection:`Windows --> Views --> Other...`

This will result in :menuselection:`Windows --> Views --> Other...`.

它的结果将是 :menuselection:`Windows --> Views --> Other...`。

Additional Reading 
------------------

扩展阅读
---------

Learn more about Sphinx and ReStructuredText:

更多有关 Sphinx 和 ReStructuredText 的资料可以访问：

- `Sphinx documentation <http://sphinx-doc.org/contents.html>`_
- `RST Quick Reference <http://docutils.sourceforge.net/docs/user/rst/quickref.html>`_

- `Sphinx 文档 <http://sphinx-doc.org/contents.html>`_
- `RST 快速参考 <http://docutils.sourceforge.net/docs/user/rst/quickref.html>`_

Summary 
---------

总结
---------

This style guide is intended to help contributors quickly create new articles for `docs.asp.net <http://docs.asp.net>`_. It includes the most common RST syntax elements that are used, as well as overall document organization guidance. If you discover mistakes or gaps in this guide, please `submit an issue <https://github.com/aspnet/docs/issues>`_.


本指南的目的是帮助广大贡献者能为 `docs.asp.net <http://docs.asp.net>`_ 快速创建新文章。本指南包含了大部分常用的 RST 语法元素以及完整的文档组织指导。若你在阅读本指南时发现错误，请`通过 issue 提交给我们 <https://github.com/aspnet/docs/issues>`_ 。

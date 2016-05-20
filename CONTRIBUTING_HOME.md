<!--# How to contribute 如何贡献 #-->
# 如何贡献

翻译：[刘怡(AlexLEWIS)](http://github.com/alexinea)

校对：

<!--One of the easiest ways to contribute is to participate in discussions and discuss issues. You can also contribute by submitting pull requests with code changes.-->

列位，最简单的贡献之策是参与话题或问题（issues）的讨论，当然你也可以修改代码并一并通过 pull request 提交给我们。

<!--## General feedback and discussions?-->
## 普通反馈与讨论
<!--Please start a discussion on the [Home repo issue tracker](https://github.com/aspnet/Home/issues).-->

可以在 Asp.Net 团队的 [Home 仓库问题跟踪](https://github.com/aspnet/Home/issues) 频道中发起讨论。

<!--## Bugs and feature requests? BUGs 与细节请求-->
## BUGs 与细节请求
<!--For non-security related bugs please log a new issue in the appropriate GitHub repo. Here are some of the most common repos:-->

对于非安全相关的 BUG，请移步至相关 Github 仓库中记录该新问题。下面是几个常用仓库：

* [DependencyInjection](https://github.com/aspnet/DependencyInjection)
* [Docs](https://github.com/aspnet/Docs)
* [EntityFramework](https://github.com/aspnet/EntityFramework)
* [Identity](https://github.com/aspnet/Identity)
* [MVC](https://github.com/aspnet/Mvc)
* [Razor](https://github.com/aspnet/Razor)
* [Templates](https://github.com/aspnet/Templates)
* [Tooling](https://github.com/aspnet/Tooling)

<!--Or browse the full list of repos in the [aspnet](https://github.com/aspnet/) organization.-->

或前往 [aspnet](https://github.com/aspnet/) 组织浏览完整的仓库清单。

<!--## Reporting security issues and bugs##-->
## 报告安全性问题与 BUGs ##

<!--Security issues and bugs should be reported privately, via email, to the Microsoft Security Response Center (MSRC)  secure@microsoft.com. You should receive a response within 24 hours. If for some reason you do not, please follow up via email to ensure we received your original message. Further information, including the MSRC PGP key, can be found in the [Security TechCenter](https://technet.microsoft.com/en-us/security/ff852094.aspx).-->

安全性问题和 BUG 需要通过邮件私下报告给微软安全响应中心（Microsoft Security Response Center，MSRC），他们的邮件地址是：secure@microsoft.com。你会在 24 小时内得到响应。如果因故未能得到响应，请通过邮件跟进以确认我们是否已收到你的原始信件。更多信息（包括 MSRC PGP key）可在 [安全技术中心（Security TechCenter）](https://technet.microsoft.com/zh-cn/security/ff852094.aspx) 中获得。

<!--## Other discussions-->
## 其他讨论
<!--Our team members also monitor several other discussion forums:-->

我们团队同时也密切关注下列几个论坛：
<!--
* [ASP.NET Core forum](https://forums.asp.net/1255.aspx/1?ASP+NET+5)
* [StackOverflow](https://stackoverflow.com/) with the [`asp.net-core`](https://stackoverflow.com/questions/tagged/asp.net-core), [`asp.net-core-mvc`](https://stackoverflow.com/questions/tagged/asp.net-core-mvc), or [`entity-framework-core`](https://stackoverflow.com/questions/tagged/entity-framework-core) tags.
* [JabbR chat room](https://jabbr.net/#/rooms/AspNetCore) for real-time discussions with the community and the people who work on the project
-->

* [ASP.NET Core 论坛](https://forums.asp.net/1255.aspx/1?ASP+NET+5)
* [StackOverflow](https://stackoverflow.com/) 中带有 [`asp.net-core`](https://stackoverflow.com/questions/tagged/asp.net-core), [`asp.net-core-mvc`](https://stackoverflow.com/questions/tagged/asp.net-core-mvc) 或 [`entity-framework-core`](https://stackoverflow.com/questions/tagged/entity-framework-core) 标签的频道.
* [JabbR chat room](https://jabbr.net/#/rooms/AspNetCore) 用于与社区和项目处理者进行即时讨论。


<!--## Filing issues-->
## 提问题的智慧
<!--When filing issues, please use our [bug filing templates](https://github.com/aspnet/Home/wiki/Functional-bug-template).-->
请用我们的 [BUG 模板](https://github.com/aspnet/Home/wiki/Functional-bug-template) 提交问题。

<!--The best way to get your bug fixed is to be as detailed as you can be about the problem.-->
为了修正你所提交的 BUG ，你最好尽可能详细地提供关于这个问题的一切信息。

<!--Providing a minimal project with steps to reproduce the problem is ideal.-->
如果能提供一个小项目来重现这个问题则更佳。

<!--Here are questions you can answer before you file a bug to make sure you're not missing any important information.-->
在你发送 BUG 前先回答下面几个问题，以确保你没有遗漏任何重要信息。

<!--
1. Did you read the [documentation](https://github.com/aspnet/home/wiki)?
2. Did you include the snippet of broken code in the issue?
3. What are the *EXACT* steps to reproduce this problem?
4. What package versions are you using (you can see these in the `project.json` file)?
5. What operating system are you using?
6. What version of IIS are you using?
-->

1. 是否已阅读 [文档](https://github.com/aspnet/home/wiki)？
2. 是否在所提交的问题中包含了代码片段？
3. 能重现该问题的**明确步骤**。
4. 你当前所使用的 package 的版本号（这些信息位于 `project.json` 文件内）。
5. 你当前所使用的操作系统。
6. 你当前所使用的 IIS 版本号。

<!--GitHub supports [markdown](https://help.github.com/articles/github-flavored-markdown/), so when filing bugs make sure you check the formatting before clicking submit.-->

Github 支持 [markdown](https://help.github.com/articles/github-flavored-markdown/) 格式，所以在点击提交 BUG 之前请务必检查你的格式。


<!--## Contributing code and content-->
## 贡献代码和内容

<!--You will need to sign a [Contributor License Agreement](https://cla2.dotnetfoundation.org/) before submitting your pull request. To complete the Contributor License Agreement (CLA), you will need to submit a request via the form and then electronically sign the Contributor License Agreement when you receive the email containing the link to the document. This needs to only be done once for any .NET Foundation OSS project.-->

在你提交 pull request 之前需先登录 [贡献者许可协议（Contributor License Agreement，CLA）](https://cla2.dotnetfoundation.org/)。为完成 CLA，你得提交申请单，然后你会收到一封包含 CLA 文档链接的邮件，点击前往并作电子签名。所有 .NET 开源软件项目都只需完成这一次。

<!--Make sure you can build the code. Familiarize yourself with the project workflow and our coding conventions. If you don't know what a pull request is read this article: https://help.github.com/articles/using-pull-requests.-->

请确保能生成你的代码，熟悉项目流程和编码规范。如果你不知道何谓 pull request 请先阅读这篇文章：https://help.github.com/articles/using-pull-requests。

<!--Before submitting a feature or substantial code contribution please discuss it with the team and ensure it follows the product roadmap. You might also read these two blogs posts on contributing code: [Open Source Contribution Etiquette](http://tirania.org/blog/archive/2010/Dec-31.html) by Miguel de Icaza and [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) by Ilya Grigorik. Note that all code submissions will be rigorously reviewed and tested by the ASP.NET and Entity Framework teams, and only those that meet an extremely high bar for both quality and design/roadmap appropriateness will be merged into the source.-->

在提交功能点或大量代码贡献之前，先与团队讨论一下以确保其遵循产品路线图。你或可先去阅读下面这两篇关于代码贡献的博文：Miguel de Icaza 的 [《开源贡献之道》（Open Source Contribution Etiquette）](http://tirania.org/blog/archive/2010/Dec-31.html) 和 Ilya Grigorik 的 [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/)。注意，所有代码提交都将被严格审查并经由 ASP.NET 和 Entity Framework 团队测试。只有那些质量极高且合乎我等规划的代码才会被合并到我们的源中。


<!--Here's a few things you should always do when making changes to the code base:-->
在你更新代码前请牢记下面几件事情：

<!--**Engineering guidelines**-->
**工程指导原则**

<!--The coding, style, and general engineering guidelines are published on the [Engineering guidelines](https://github.com/aspnet/Home/wiki/Engineering-guidelines) page.-->

代码、风格与通用工程指导原则皆已发表在 [Engineering guidelines](https://github.com/aspnet/Home/wiki/Engineering-guidelines) 页中。

<!--**Commit/Pull Request Format**-->
**提交/拉取请求格式**

```
Summary of the changes (Less than 80 chars)
 - Detail 1
 - Detail 2

Addresses #bugnumber (in this specific format)
```

<!--**Tests**-->
**测试**

<!--
-  Tests need to be provided for every bug/feature that is completed.
-  Tests only need to be present for issues that need to be verified by QA (e.g. not tasks)
-  If there is a scenario that is far too hard to test there does not need to be a test for it.
  - "Too hard" is determined by the team as a whole.
-->

- 测试需要提供每一个所提交的 BUG/细节；
- 如果当前问题已被 QA 所证实则只需测试；
- 如果处在一个非常难以测试的场景中，则无需测试之。
    + 「非常难」这一定义取决于整个团队而言。

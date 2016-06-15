通过使用 Visual Studio 连接服务添加 Azure 存储空间
=====

作者：`Patrick Sheahan`、`Tom Archer`、`Kemp Brown`

翻译：`耿晓亮(Blue)`

概述
---------------------

用 Visual Studio 2015， 通过添加连接服务对话框，你可以连接任何 C# 的云服务，.NET 后端移动服务，ASP.NET 网站或服务，ASP.NET 5 服务或者 Azure WebJob 服务。连接服务功能添加了所有需要引用和连接代码，并适当的修改了你的配置文件。对话框也附带了告诉你后续步骤开始 blob 存储，队列和表的文档。

支持的项目类型
---------------------

你可以用连接服务对话框来连接下面项目类型的 Azure 存储。

 - ASP.NET 网站项目

 - ASP.NET 5 项目

 - .NET 云服务网络角色和工作者角色项目

 - .NET 移动服务项目

 - Azure WebJob 项目

通过连接服务对话框连接到 Azure 存储
---------------------

1. 确认你有一个 Azure 账号。如果你没有 Azure 账号，你可以注册一个 `免费试用 <https://azure.microsoft.com/zh-cn/pricing/free-trial/>`_。一旦你有了 Azure 账号，你就可以创建存储账号，移动服务和配置 Azure 有效的目录。

2. 在 Visual Studio 中打开你的项目，在解决方案管理器里 **References** 节点上打开右键菜单，选择 **添加链接服务**。

   .. image :: https://github.com/Azure/azure-content/blob/master/articles/media/vs-azure-tools-connected-services-storage/IC796702.png

3. 在 **添加连接服务** 对话框中，选择 **Azure 存储**，然后选择 **配置** 按钮。如果你没有登录会提示让你登录。

   .. image :: https://github.com/Azure/azure-content/blob/master/articles/media/vs-azure-tools-connected-services-storage/IC796703.png

4. 在 **Azure** 存储对话框中，选择一个存在的存储账号并选择 **添加**。如果你需要创建一个新的存储账号，转到下一步，否则跳过步骤6. 

   .. image :: https://github.com/Azure/azure-content/blob/master/articles/media/vs-azure-tools-connected-services-storage/IC796704.png

5. 创建一个新的存储账号：

    i. 在 Azure 存储对话框中选择 **创建新存储账号** 按钮。

   ii. 填写 **创建存储账号** 对话框然后选择 **创建** 按钮。

       .. image :: https://github.com/Azure/azure-content/blob/master/articles/media/vs-azure-tools-connected-services-storage/create-storage-account.png

       当你返回 **Azure存储** 对话框是。列表中会出现新的存储账号。
    
  iii. 在列表中选择新的存储账号并选择 **添加**。

6. WebJob 项目的服务引用节点下会出现存储连接服务。

   .. image :: https://github.com/Azure/azure-content/blob/master/articles/media/vs-azure-tools-connected-services-storage/IC796705.png

7. 回到起始页，查看并找出你的项目是如何被修改的。当你添加一个链接服务后起始页就会在浏览器里出现。你可以查看接下来的建议步骤和代码示例，或者切换到生成的页面查看你的项目都引用了什么，你的代码和配置文件是如何被修改的。

如何修改你的项目
---------------------

当你完成对话框后。Visual Studio 添加了引用并且修改了某些配置文件。根据项目类型会有一些特殊的修改

 - ASP.NET 项目，参见 `发生了什么— ASP.NET 项目 <https://azure.microsoft.com/zh-cn/pricing/free-trial/>`_。

 - ASP.NET 5 项目，参见 `发生了什么— ASP.NET 5 项目 <https://azure.microsoft.com/zh-cn/documentation/articles/vs-storage-aspnet5-getting-started-blobs/>`_。

 - 云服务项目（网络角色和工作者角色），参见 `发生了什么— Cloud Service 项目 <https://azure.microsoft.com/zh-cn/documentation/articles/vs-storage-cloud-services-getting-started-blobs/>`_。

 - WebJob 项目，参见 `发生了什么— WebJob 项目 <https://github.com/Azure/azure-content/blob/master/articles/storage/vs-storage-webjobs-what-happened.md>`_。

后续步骤
---------------------

1. 使用入门代码示例作为参考，创建你想要的存储类型，然后开始编写代码来访问你的存储账号！

2. 提问和获取帮助

    - `MSDN 论坛: Azure 存储 <https://social.msdn.microsoft.com/forums/azure/home?forum=windowsazuredata>`_

    - `Azure 存储团队博客 <http://blogs.msdn.com/b/windowsazurestorage/>`_

    - `azure.microsoft.com 上的存储板块 <https://azure.microsoft.com/services/storage/>`_

    - `azure.microsoft.com 上的存储文档 <https://azure.microsoft.com/documentation/services/storage/>`_
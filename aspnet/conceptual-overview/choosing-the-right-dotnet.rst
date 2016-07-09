为你的服务器选择合适版本的.NET的框架
=============================================

原文 `Choosing the Right .NET For You on the Server <https://docs.asp.net/en/latest/fundamentals/choosing-the-right-dotnet.html>`_

作者：`Daniel Roth`_ 

翻译：`王健 <https://github.com/wjhgzx>`_

校对：`谢炀(Kiler) <https://github.com/kiler398/>`_、`何镇汐 <https://github.com/UtilCore>`_、`许登洋(Seay) <https://github.com/SeayXu>`_、`孟帅洋(书缘) <https://github.com/mengshuaiyang>`_

ASP.NET Core基于 `.NET Core`_ 项目模型，它支持构建能够运行在 Windows、Mac和 Linux 上的跨平台应用程序。当您构建一个 .Net Core 项目的时候，您可以选择一种 .NET框架来构建您的应用程序，.NET Framework (CLR)、 .NET Core (CoreCLR) 或者 `Mono <http://mono-project.com>`_ ？应该选择哪一种 .NET框架，我们来看下每一种的优缺点吧。

.NET Framework
--------------

.NET Framework 是三个.NET框架中最知名和最成熟的一个。.NET Framework 是承载于Windows系统平台的全功能成熟框架。.NET Framework 的生态系统已经非常成熟，并且已经被使用超过了10年。.NET Framework 如今已经大量用于生产环境并为您创建的应用程序和类库提供最高级别的兼容性。

.NET Framework 只能运行在Windows系统上。它是一个拥有大量 API 并且发布周期漫长的框架。虽然 .NET Framework 的代码可供参考 ，但它不是一个活跃的开源项目。

.NET Core
---------

.NET Core是一个模块化的运行时和类库实现，包括.NET Framework的一个子集。 .NET Core支持Windows、Mac以及Linux系统。.NET Core是由一组被称为 “CoreFX” 的库，和一个被称为 “CoreCLR” 的小的并被优化过的运行时。 .NET Core是开源的，所以你可以跟踪它的项目进度，并在 `GitHub <https://github.com/dotnet>`_ 上贡献代码。

CoreCLR 运行时 (Microsoft.CoreCLR) 和 CoreFX 库通过 `NuGet`_ 进行分发。.NET Core 被构建成组件化的库集合，因此，您可以在您的应用程序中根据需要，仅使用需要的 API 。您也可以在更受限的环境中运行基于.NET Core 的应用（如 `Windows Server Nano <http://blogs.technet.com/b/windowsserver/archive/2015/04/08/microsoft-announces-nano-server-for-modern-apps-and-cloud.aspx>`_）。

API 进行了更新，使其更好地组件化。这意味着现有的在 .NET Framework 中创建的库通常需要重新编译来使其运行在 .NET Core 中。.NET Core 的生态系统相对来说比较新，但是在流行的 .Net 组件包的支持下发展迅速，如 JSON.NET，AutoFac，xUnit.net 等等。

基于 .NET Core 的开发，可以使你在单一平台上开发的程序运行在多个平台上。

Mono
----

`Mono <http://mono-project.com>`_ 是主要用于构造非 Windows 平台的 .NET Framework 的一个端口。 Mono 是开源的，跨平台的。它也具有类似 .NET Framework 的API，所以如今很多现有的托管库在 Mono 上运行。 当.NET Core 在跨平台发展成熟的过程中，Mono 是跨平台开发的良好试验场。

总结
-------

.NET Core 项目模型使 .NET 开发可用于比以往更多的场景。通过 .NET Core 您可以在已有的可用的 .NET 平台上针对性的构建应用程序。选择哪一种的 .NET框架 将取决于您的使用场景、时间表、功能需求和兼容性需求。

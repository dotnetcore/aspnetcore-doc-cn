为您的服务器选择合适版本的.NET的框架
=============================================

作者：`Daniel Roth`_  翻译：王健

ASP.NET Core基于`.NET Core`_ 项目模型，它支持构建能运行在Windows、Mac和 Linux上的跨平台应用程序。当您构建一个.Net Core项目，您也可以在.NET Framework (CLR), .NET Core (CoreCLR) 或者`Mono <http://mono-project.com>`_ 中来选择一种.NET风格来构建您的应用程序，.NET Framework (CLR), .NET Core (CoreCLR) 或者 Mono？您应该选择哪一种.NET风格，来看看每一种的优缺点吧。

.NET Framework
--------------

.NET Framework 是三种.NET框架中最知名和最成熟的一种。.NET Framework是和Windows系统一起装载的成熟的全功能框架。.NET Framework的生态系统已经非常成熟，并且已经被使用超过10年。.NET Framework（生产就绪），并且为您的现有应用程序和库提供最高级别的兼容性。

.NET Framework 只能运行在Windows系统上。它是一个暴露了大量API并且发布周期缓慢的整体框架。虽然.NET Framework的代码 `可供参考<http://referencesource.microsoft.com/>`_，但它不是一个活跃开源的项目。

.NET Core
---------

.NET Core是一个模块化的运行时和库实现，包括.NET Framework的一个子集。 .NET Core支持Windows，Mac和Linux。 .NET Core是由一组被称为“CoreFX”的库，和一个被称为“CoreCLR”的优化过的小的运行时。 .NET Core是开源的，所以你可以跟踪它的进展，并在`GitHub <https://github.com/dotnet>`_ 上贡献代码。

CoreCLR运行时(Microsoft.CoreCLR) 和CoreFX库通过`NuGet`进行分发。.NET Core被构建成组件化的库集合，因此，您可以在您的应用程序中限制API表面积，使其仅使用您需要的部分。您也可以在更受限的环境中运行基于.NET Core的应用（如 `Windows Server Nano <http://blogs.technet.com/b/windowsserver/archive/2015/04/08/microsoft-announces-nano-server-for-modern-apps-and-cloud.aspx>`_）。

API进行了更新,使其更好地组件化。这意味着现有的在.NET Framework中创建的库通常需要重新编译来使其运行在.NET Core中。.NET Core的生态系统相对来说比较新，但是在流行的.Net组件包的支持下发展迅速，如JSON.NET, AutoFac, xUnit.net等等。

在.NET Core上开发，您可以针对单一一致的平台，也可以运行在多个平台上。

Mono
----

`Mono <http://mono-project.com>`_是主要用于建造非Windows平台的.NET Framework的一个端口。 Mono是开源的，跨平台的。它也具有类似.NET框架的API，所以如今很多现有的托管库在Mono上运行。 Mono是跨平台发展的良好试验场,.NET Core在跨平台支持上已经成熟。

总结
-------

.NET Core项目模型使.NET开发可用于比以往更多的场景。通过.NET Core您可以在已有的可用的.NET平台上针对性构建应用程序。选择哪一种风格的.NET将取决于您的特定场景、时间限制、功能需求和兼容性需求。
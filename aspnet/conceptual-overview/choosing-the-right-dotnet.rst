为您的服务器选择合适版本的.NET的框架
=============================================

作者：`Daniel Roth`_  
翻译：`王健 <https://github.com/wjhgzx>`_

ASP.NET Core基于 `.NET Core`_ 项目模型，它支持构建能运行在Windows、Mac和 Linux上的跨平台应用程序。当您构建一个.Net Core项目，您也可以在.NET Framework (CLR)，.NET Core (CoreCLR) 或者 `Mono <http://mono-project.com>`_ 中来选择一种.NET版本来构建您的应用程序，.NET Framework (CLR), .NET Core (CoreCLR) 或者 Mono？您应该选择哪一种.NET版本，来看看每一种的优缺点吧。

.NET Framework
--------------

.NET Framework 是三种.NET框架中最知名和最成熟的一种。.NET Framework是和Windows系统一起装载的成熟的全功能框架。.NET Framework的生态系统已经非常成熟，并且已经被使用超过10年。.NET Framework 为您创建的应用程序和类库提供最高级别的兼容性。

.NET Framework 只能运行在Windows系统上。它是一个拥有大量API并且发布周期漫长的框架。虽然.NET Framework的代码可供参考 ，但它不是一个活跃的开源项目。

.NET Core
---------

.NET Core是一个模块化的运行时和类库实现，包括.NET Framework的一个子集。 .NET Core支持Windows，Mac和Linux。.NET Core是由一组被称为“CoreFX”的库，和一个被称为“CoreCLR”的优化过的小的运行时。 .NET Core是开源的，所以你可以跟踪它的进展，并在 `GitHub <https://github.com/dotnet>`_ 上贡献代码。

CoreCLR运行时(Microsoft.CoreCLR) 和CoreFX库通过 `NuGet`_ 进行分发。.NET Core被构建成组件化的库集合，因此，您可以在您的应用程序中根据需要，仅使用需要的API。您也可以在更受限的环境中运行基于.NET Core的应用（如 `Windows Server Nano <http://blogs.technet.com/b/windowsserver/archive/2015/04/08/microsoft-announces-nano-server-for-modern-apps-and-cloud.aspx>`_）。

API进行了更新，使其更好地组件化。这意味着现有的在.NET Framework中创建的库通常需要重新编译来使其运行在.NET Core中。.NET Core的生态系统相对来说比较新，但是在流行的.Net组件包的支持下发展迅速，如JSON.NET, AutoFac, xUnit.net等等。

基于 .NET Core的开发，可以使你在单一平台上开发的程序运行在多个平台上。

Mono
----

`Mono <http://mono-project.com>`_ 是主要用于建造非Windows平台的.NET Framework的一个端口。 Mono是开源的，跨平台的。它也具有类似.NET Framework的API，，所以如今很多现有的托管库在Mono上运行。 当.NET Core 在跨平台发展成熟的过程中，Mono是跨平台开发的良好试验场。

总结
-------

.NET Core项目模型使.NET开发可用于比以往更多的场景。通过.NET Core您可以在已有的可用的.NET平台上针对性构建应用程序。选择哪一种风格的.NET将取决于您的特定场景、时间限制、功能需求和兼容性需求。
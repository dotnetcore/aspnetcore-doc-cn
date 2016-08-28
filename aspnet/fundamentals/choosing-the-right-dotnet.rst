Choosing the Right .NET For You on the Server
=============================================

为你的服务器选择合适版本的.NET的框架
=============================================

By `Daniel Roth`_

作者：`Daniel Roth`_ 

翻译：`王健 <https://github.com/wjhgzx>`_

校对：`谢炀(Kiler) <https://github.com/kiler398/>`_、`何镇汐 <https://github.com/UtilCore>`_、`许登洋(Seay) <https://github.com/SeayXu>`_、`孟帅洋(书缘) <https://github.com/mengshuaiyang>`_

ASP.NET Core is based on the `.NET Core`_ project model, which supports building applications that can run cross-platform on Windows, Mac and Linux. When building a .NET Core project you also have a choice of which .NET flavor to target your application at: .NET Framework (CLR), .NET Core (CoreCLR) or `Mono <http://mono-project.com>`_. Which .NET flavor should you choose? Let's look at the pros and cons of each one.

ASP.NET Core基于 `.NET Core`_ 项目模型，它支持构建能够运行在 Windows、Mac和 Linux 上的跨平台应用程序。当您构建一个 .Net Core 项目的时候，您可以选择一种 .NET框架来构建您的应用程序，.NET Framework (CLR)、 .NET Core (CoreCLR) 或者 `Mono <http://mono-project.com>`_ ？应该选择哪一种 .NET框架，我们来看下每一种的优缺点吧。

.NET Framework
--------------

.NET Framework
--------------

The .NET Framework is the most well known and mature of the three options. The .NET Framework is a mature and fully featured framework that ships with Windows. The .NET Framework ecosystem is well established and has been around for well over a decade. The .NET Framework is production ready today and provides the highest level of compatibility for your existing applications and libraries.

.NET Framework 是三个.NET框架中最知名和最成熟的。.NET Framework 是承载于Windows系统平台的全功能成熟框架。.NET Framework 的生态系统已经非常成熟，并且已经被使用超过了10年。.NET Framework 如今已经大量用于生产环境并为您创建的应用程序和类库提供最高级别的兼容性。

The .NET Framework runs on Windows only. It is also a monolithic component with a large API surface area and a slower release cycle. While the code for the .NET Framework is `available for reference <http://referencesource.microsoft.com/>`_ it is not an active open source project.

.NET Framework 只能运行在Windows系统上。它是一个拥有大量 API 并且发布周期漫长的框架。虽然 .NET Framework 的代码可供参考 ，但它不是一个活跃的开源项目。

.NET Core
---------

.NET Core
---------

.NET Core is a modular runtime and library implementation that includes a subset of the .NET Framework. .NET Core is supported on Windows, Mac and Linux. .NET Core consists of a set of libraries, called "CoreFX", and a small, optimized runtime, called "CoreCLR". .NET Core is open-source, so you can follow progress on the project and contribute to it on `GitHub <https://github.com/dotnet>`_.

.NET Core是一个模块化的运行时和类库实现，包括.NET Framework的一个子集。 .NET Core支持Windows、Mac以及Linux系统。.NET Core是由一组被称为 “CoreFX” 的库，和一个被称为 “CoreCLR” 的小的并被优化过的运行时。 .NET Core是开源的，所以你可以跟踪它的项目进度，并在 `GitHub <https://github.com/dotnet>`_ 上贡献代码。

The CoreCLR runtime (Microsoft.CoreCLR) and CoreFX libraries are distributed via `NuGet`_. Because .NET Core has been built as a componentized set of libraries you can limit the API surface area your application uses to just the pieces you need. You can also run .NET Core based applications on much more constrained environments (ex. :doc:`/tutorials/nano-server`).

CoreCLR 运行时 (Microsoft.CoreCLR) 和 CoreFX 库通过 `NuGet`_ 进行分发。.NET Core 被构建成组件化的库集合，因此，您可以在您的应用程序中根据需要，仅使用需要的 API 。您也可以在更受限的环境中运行基于.NET Core 的应用（如 :doc:`/tutorials/nano-server`）。

The API factoring in .NET Core was updated to enable better componentization. This means that existing libraries built for the .NET Framework generally need to be recompiled to run on .NET Core. The .NET Core ecosystem is relatively new, but it is rapidly growing with the support of popular .NET packages like JSON.NET, AutoFac, xUnit.net and many others.

API 进行了更新，使其更好地组件化。这意味着现有的在 .NET Framework 中创建的库通常需要重新编译来使其运行在 .NET Core 中。.NET Core 的生态系统相对来说比较新，但是在流行的 .Net 组件包的支持下发展迅速，如 JSON.NET，AutoFac，xUnit.net 等等。

Developing on .NET Core allows you to target a single consistent platform that can run on multiple platforms. 

基于 .NET Core 的开发，可以使你在单一平台上开发的程序运行在多个平台上。

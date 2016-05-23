Choosing the Right .NET For You on the Server
为您的服务器选择合适版本的.NET（的框架）

By `Daniel Roth`
作者：`Daniel Roth`  翻译：王健

ASP.NET Core is based on the .NET Core project model, which supports building applications that can run cross-platform on Windows, Mac and Linux. When building a .NET Core project you also have a choice of which .NET flavor to target your application at: .NET Framework (CLR), .NET Core (CoreCLR) or Mono. Which .NET flavor should you choose? Let's look at the pros and cons of each one.

ASP.NET Core基于.Net Core项目模型，它支持构建能运行在Windows、Mac和 Linux上的跨平台应用程序。当您构建一个.Net Core项目，您也可以在.NET Framework (CLR), .NET Core (CoreCLR) 或者 Mono中来选择一种.NET风格来构建您的应用程序，.NET Framework (CLR), .NET Core (CoreCLR) 或者 Mono？您应该选择哪一种.NET风格，来看看每一种的优缺点吧。


.NET Framework

The .NET Framework is the most well known and mature of the three options. The .NET Framework is a mature and fully featured framework that ships with Windows. The .NET Framework ecosystem is well established and has been around for well over a decade. The .NET Framework is production ready today and provides the highest level of compatibility for your existing applications and libraries.

.NET Framework 是三种.NET框架中最知名和最成熟的一种。.NET Framework是和Windows系统一起装载的成熟的全功能框架。.NET Framework的生态系统已经非常成熟，并且已经被使用超过10年。.NET Framework（生产就绪），并且为您的现有应用程序和库提供最高级别的兼容性。

The .NET Framework runs on Windows only. It is also a monolithic component with a large API surface area and a slower release cycle. While the code for the .NET Framework is available for reference it is not an active open source project.

.NET Framework 只能运行在Windows系统上。它是一个包含大量API表面积并且发布周期缓慢的整体框架。虽然.NET Framework的代码可供参考，但它不是一个活跃的开源项目。

.NET Core

.NET Core is a modular runtime and library implementation that includes a subset of the .NET Framework. .NET Core is supported on Windows, Mac and Linux. .NET Core consists of a set of libraries, called "CoreFX", and a small, optimized runtime, called "CoreCLR". .NET Core is open-source, so you can follow progress on the project and contribute to it on GitHub.

.NET Core是一个模块化的运行时和库实现，包括.NET Framework的一个子集。 .NET Core支持Windows，Mac和Linux。 .NET Core是由一组被称为“CoreFX”的库，和一个被称为“CoreCLR”的优化过的小的运行时。 .NET Core是开源的，所以你可以跟踪它的进展，并在GitHub上贡献代码。

The CoreCLR runtime (Microsoft.CoreCLR) and CoreFX libraries are distributed via `NuGet`_. Because .NET Core has been built as a componentized set of libraries you can limit the API surface area your application uses to just the pieces you need. You can also run .NET Core based applications on much more constrained environments (ex. Windows Server Nano).

CoreCLR运行时(Microsoft.CoreCLR) 和CoreFX库通过`NuGet`进行分发。.NET Core被构建成组件化的库集合，因此，您可以在您的应用程序中限制API表面积，使其仅使用您需要的部分。您也可以在更受限的环境中运行基于.NET Core的应用（如 Windows Server Nano）。

The API factoring in .NET Core was updated to enable better componentization. This means that existing libraries built for the .NET Framework generally need to be recompiled to run on .NET Core. The .NET Core ecosystem is relatively new, but it is rapidly growing with the support of popular .NET packages like JSON.NET, AutoFac, xUnit.net and many others.

API进行了更新,使其更好地组件化。这意味着现有的在.NET Framework中创建的库通常需要重新编译来使其运行在.NET Core中。.NET Core的生态系统相对来说比较新，但是在流行的.Net组件包的支持下发展迅速，如JSON.NET, AutoFac, xUnit.net等等。

Developing on .NET Core allows you to target a single consistent platform that can run on multiple platforms.

在.NET Core上开发，您可以针对单一一致的平台，也可以运行在多个平台上。


Mono

Mono is a port of the .NET Framework built primarily for non-Windows platforms. Mono is open source and cross-platform. It also shares a similar API factoring to the .NET Framework, so many existing managed libraries work on Mono today. Mono is a good proving ground for cross-platform development while cross-platform support in .NET Core matures.

Mono是主要用于建造非Windows平台的.NET Framework的一个端口。 Mono是开源的，跨平台的。它也具有类似.NET框架的API，所以如今很多现有的托管库在Mono上运行。 Mono是跨平台发展的良好试验场,.NET Core在跨平台支持上已经成熟。

Moni

Summary
总结

The .NET Core project model makes .NET development available for more scenarios than ever before. With .NET Core you have the option to target your application at existing available .NET platforms. Which .NET flavor you pick will depend on your specific scenarios, timelines, feature requirements and compatibility requirements.

.NET Core项目模型使.NET开发可用于比以往更多的场景。通过.NET Core您可以在已有的可用的.NET平台上针对性构建应用程序。选择哪一种风格的.NET将取决于您的特定场景、时间限制、功能需求和兼容性需求。
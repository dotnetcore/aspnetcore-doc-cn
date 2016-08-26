.. _directory-structure:

Directory Structure
===================

目录结构
===================

By `Luke Latham`_

作者： `Luke Latham`_

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_ 


In ASP.NET Core, the application directory, *publish*, is comprised of application files, config files, static assets, packages, and the runtime (for self-contained apps). This is the same directory structure as previous versions of ASP.NET, where the entire application lives inside the web root directory.

在 ASP.NET Core中，应用程序目录，*publish*，是由应用程序文件，配置文件，静态资源，包和运行时（对于独立应用程序）。这是和之前的 ASP.NET 相同的目录结构，整个应用程序存放于Web根目录中。

+----------------+------------------------------------------------+
| 应用类型       | 目录结构                                        |
+================+================================================+
| 便携软件        | - publish*                                     |
|                |                                                |
|                |   - logs* (如果在发布选项中包含)                 |
|                |   - refs*                                      |
|                |   - runtimes*                                  |
|                |   - Views*  (如果在发布选项中包含)               |
|                |   - wwwroot*  (如果在发布选项中包含)             |
|                |   - .dll files                                 |
|                |   - myapp.deps.json                            |
|                |   - myapp.dll                                  |
|                |   - myapp.pdb                                  |
|                |   - myapp.runtimeconfig.json                   |
|                |   - web.config (如果在发布选项中包含)            |
+----------------+------------------------------------------------+
| 独立软件        | - publish*                                     |
|                |                                                |
|                |   - logs*  (如果在发布选项中包含)                |
|                |   - refs*                                      |
|                |   - Views*  (如果在发布选项中包含)               |
|                |   - wwwroot*  (如果在发布选项中包含)             |
|                |   - .dll files                                 |
|                |   - myapp.deps.json                            |
|                |   - myapp.exe                                  |
|                |   - myapp.pdb                                  |
|                |   - myapp.runtimeconfig.json                   |
|                |   - web.config  (如果在发布选项中包含)           |
+----------------+------------------------------------------------+

\* Indicates a directory

The contents of the *publish* directory represent the *content root path*, also called the *application base path*, of the deployment. Whatever name is given to the *publish* directory in the deployment, its location serves as the server's physical path to the hosted application. The *wwwroot* directory, if present, only contains static assets. The *logs* directory may be included in the deployment by creating it in the project and adding it to **publishOptions** of *project.json* or by physically creating the directory on the server.

*publish* 目录的内容代表  *content root path* ，也称为部署的 *application base path* 。在部署的时候无论 *publish* 目录被命名为什么，其路径总会别作为托管应用程序的服务器的物理路径。*wwwroot* 目录如果存在的话，仅仅包含静态的资源。*logs* 目录可以通过创建项目的时候包含在项目中，也可以将其添加到 *project.json* 的 **publishOptions**之中，或者直接在服务器上物理创建。

The deployment directory requires Read/Execute permissions, while the *logs* directory requires Read/Write permissions. Additional directories where assets will be written require Read/Write permissions.

部署目录需要读/执行权限， *logs* 目录需要读/写权限。其他目录如果需要写入资源则需要读/写权限。

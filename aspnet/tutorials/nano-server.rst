.. _nano-server:

在 Nano Server 上运行ASP.NET Core
=================================

ASP.NET Core on Nano Server
===========================

作者： `Sourabh Shirhatti`_

翻译： `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对： `刘怡(AlexLEWIS) <https://github.com/alexinea>`_

.. attention:: This tutorial uses a pre-release version of the Nano Server installation option of Windows Server Technical Preview 4. You may use the software in the virtual hard disk image only to internally demonstrate and evaluate it. You may not use the software in a live operating environment. Please see https://go.microsoft.com/fwlink/?LinkId=624232 for specific information about the end date for the preview.

.. attention:: 本教程使用 Windows Server Technical Preview 4 中的预发行版本的 Nano Server 安装选项。 你可以使用虚拟硬盘镜只用来内部演示和评估它。不能将该软件在生产环境中使用。请看 https://go.microsoft.com/fwlink/?LinkId=624232 查看具体的预览截止日期信息。

In this tutorial, you'll take an existing ASP.NET Core app and deploy it to a Nano Server instance running IIS.

在本教程中，你将使用一个现有的 ASP.NET Core 应用程序并将其部署在一个 Nano Server 实例的 IIS 上。

.. contents:: Sections:
  :local:
  :depth: 1

Introduction
------------

介绍
------

Windows Server 2016 Technical Preview offers a new installation option: Nano Server. Nano Server is a remotely administered server operating system optimized for private clouds and datacenters. It takes up far less disk space, sets up significantly faster, and requires far fewer updates and restarts than Windows Server. You can learn more about Nano Server from the `official docs <https://msdn.microsoft.com/en-us/library/mt126167.aspx>`_.

Windows Server 2016 Technical Preview 提供了一个新的安装选项： Nano Server 。 Nano Server 是一个针对私有云和数据中心进行优化的远程管理服务器操作系统。它比 Windows Server 占用更少的磁盘空间，更快的安装速度，更少的更新并重启。你可以通过 `官方文档 <https://msdn.microsoft.com/en-us/library/mt126167.aspx>`_ 了解更多关于 Nano Server 的内容。

In this tutorial, we will be using the pre-built `Virtual Hard Disk (VHD) for Nano Server <https://msdn.microsoft.com/en-us/virtualization/windowscontainers/nano_eula>`_  from Windows Server Technical Preview 4. This pre-built VHD already includes the Reverse Forwarders and IIS packages which are required for this tutorial.

在本教程中，我们将使用在 Windows Server Technical Preview 4 中预创建的 `Virtual Hard Disk (VHD) for Nano Server <https://msdn.microsoft.com/en-us/virtualization/windowscontainers/nano_eula>`_ 。这个预创建的 VHD 已经包含了在本教程中我们需要的反向代理和 IIS 包。

Before proceeding with this tutorial, you will need the :doc:`published </publishing/index>` output of an existing ASP.NET Core application. Ensure your application is built to run in a **64-bit** process.

在进行本教程之前，你需要 :doc:`发布 </publishing/index>` 一个存在的 ASP.NET Core 应用程序。确保你的程序是构建在 **64 位** 进程中运行的。

Setting up the Nano Server Instance
-----------------------------------

设置 Nano Server 实例
-----------------------------------

`Create a new Virtual Machine using Hyper-V <https://technet.microsoft.com/en-us/library/hh846766.aspx>`_ on your development machine using the previously downloaded VHD. The machine will require you to set an administator password before logging on. At the VM console, press F11 to set the password before the first logon.

在你的开发机上 `通过 Hyper-V 创建一个新的虚拟机 <https://technet.microsoft.com/en-us/library/hh846766.aspx>`_ 并使用之前下载的 VHD 。这个虚拟机需要你在登录前设置一个管理员密码。第一次登录前，在虚拟机(VM)控制台按 F11 设置密码。

After setting the local password, you will manage Nano Server using PowerShell remoting.

在设置本地密码之后，你将通过 PowerShell Remoting 管理 Nano Server 。

Connecting to your Nano Server Instance using PowerShell Remoting
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

通过 PowerShell Remoting 连接你的 Nano Server 实例
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Open an elevated PowerShell window to add your remote Nano Server instance to your ``TrustedHosts`` list.

打开一个提升过权限的 PowerShell 窗口来添加你的远程 Nano Server 实例到你的 ``受信任的主机(TrustedHosts)`` 列表

.. code:: ps1

  $ip = "10.83.181.14" # replace with the correct IP address
  Set-Item WSMan:\localhost\Client\TrustedHosts "$ip" -Concatenate -Force

Once you have added your Nano Server instance to your ``TrustedHosts``, you can connect to it using PowerShell remoting

一旦你添加了 Nano Server 实例到你的 ``受信任的主机(TrustedHosts)`` 列表，你就可以用 PowerShell Remoting 连接它了。

.. code:: ps1

  $ip = "10.83.181.14" # replace with the correct IP address
  $s = New-PSSession -ComputerName $ip -Credential ~\Administrator
  Enter-PSSession $s

If you have successfully connected then your prompt will look like this ``[10.83.181.14]: PS C:\Users\Administrator\Documents>``

如果你成功连接，你的命令提示符将看起来像这样 ``[10.83.181.14]: PS C:\Users\Administrator\Documents>``

Installing the HttpPlatformHandler Module
-----------------------------------------

安装 HttpPlatformHandler 组件
-----------------------------------------

The :ref:`HttpPlatformHandler <http-platformhandler>` is an IIS 7.5+ module which is responsible for process management of HTTP listeners and to proxy requests to processes that it manages. At the moment, the process to install the HttpPlatformHandler Module for IIS is manual. You will need to install the latest 64-bit version of the `HttpPlatformHandler <http://www.iis.net/downloads/microsoft/HttpPlatformHandler>`_ on a regular (not Nano) machine. After installing you will need to copy the following files:

:ref:`HttpPlatformHandler <http-platformhandler>` 是一个适用于 IIS 7.5 及以上版本的组件，它用来负责 HTTP 监听器的过程管理和代理请求的过程管理。 目前需要手动在 IIS 上安装 HttpPlatformHandler 组件。你需要在你的常规机(不是 Nano Server) 上安装最新的 64 位版本的 `HttpPlatformHandler <http://www.iis.net/downloads/microsoft/HttpPlatformHandler>`_ 。安装之后你需要复制以下文件：

* *%windir%\\System32\\inetsrv\\HttpPlatformHandler.dll*
* *%windir%\\System32\\inetsrv\\config\\schema\\httpplatform_schema.xml*

On the Nano machine you’ll need to copy those two files to their respective locations.

在 Nano 主机中你需要复制这两个文件到他们各自的位置。

.. code:: ps1

  Copy-Item .\HttpPlatformHandler.dll c:\Windows\System32\inetsrv
  Copy-Item .\httpplatform_schema.xml c:\Windows\System32\inetsrv\config\schema

Enabling the HttpPlatformHandler
--------------------------------

启用 HttpPlatformHandler
--------------------------------

You can execute the following PowerShell script in a remote PowerShell session to enable the HttpPlatformHandler module on the Nano server.

你可以在 PowerShell 远程会话中执行以下 PowerShell 脚本来在 Nano Server 上启用 HttpPlatformHandler 组件。

.. note:: This script runs on a clean system, but is not meant to be idempotent. If you run this multiple times it will add multiple entries. If you end up in a bad state, you can find backups of the *applicationHost.config* file at *%systemdrive%\inetpub\history*.

.. note:: 这份脚本可以运行在干净的系统里，但是不能运行多次。如果你运行这份脚本多次，它将会添加多个条目。如果你最终处于糟糕的状态，你可以在 *%systemdrive%\inetpub\history* 中找到 *applicationHost.config* 文件的备份。

.. literalinclude:: nano-server/enable-platformhandler.ps1
  :language: ps1

Manually Editing *applicationHost.config*
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

手动编辑 *applicationHost.config*
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can skip this section if you already ran the PowerShell script above. Though is not recommended, you can alternatively enable the HttpPlatformHandler by manually editing the *applicationHost.config* file.

你如果你已经运行过上面的 PowerShell脚本，可以跳过这一节。你可以选择通过手动编辑 *applicationHost.config* 文件的方式来启用 HttpPlatformHandler ，尽管不推荐这样做。

Open up *C:\\Windows\\System32\\inetsrv\\config\\applicationHost.config*

打开 *C:\\Windows\\System32\\inetsrv\\config\\applicationHost.config*

Under ``<configSections>`` add

在 ``<configSections>`` 节点下加入

.. literalinclude:: nano-server/applicationHost.config
  :language: xml
  :lines: 42-43
  :dedent: 4
  :emphasize-lines: 2

In the ``system.webServer`` section group, update the handlers section to allow the configured handlers to be overridden.

在 ``system.webServer`` 节点组(sectionGroup)，更新处理程序节点(handlers section) 来允许配置处理程序被重写。

.. literalinclude:: nano-server/applicationHost.config
  :language: xml
  :lines: 55,63
  :dedent: 8
  :emphasize-lines: 2

Add ``httpPlatformHandler`` to the ``globalModules`` section

添加 ``httpPlatformHandler`` 到 ``globalModules`` 节点下

.. literalinclude:: nano-server/applicationHost.config
  :language: xml
  :lines: 210,224
  :dedent: 8
  :emphasize-lines: 2

Additionally, add ``httpPlatformHandler`` to the ``modules`` section

除此之外，添加 ``httpPlatformHandler`` 到 ``modules`` 节点下

.. literalinclude:: nano-server/applicationHost.config
  :language: xml
  :lines: 275,286
  :dedent: 8
  :emphasize-lines: 2

Publishing the application
-----------------------------------

发布应用程序
-----------------------------------

Copy over the published output of your existing application to the Nano server.

将你发布好的现有应用程序复制到 Nano Server 。

.. code:: ps1

  $ip = "10.83.181.14" # replace with the correct IP address
  $s = New-PSSession -ComputerName $ip -Credential ~\Administrator
  Copy-Item -ToSession $s -Path <path-to-src>\bin\output\ -Destination C:\HelloAspNet5 -Recurse

Use the following PowerShell snippet to create a new site in IIS for our published app. This script uses the ``DefaultAppPool`` for simplicity. For more considerations on running under an application pool, see :ref:`apppool`.

使用以下 PowerShell 片段来创建一个用于我们发布应用程序的 IIS 站点。为了简单，这段脚本使用 ``默认应用程序池(DefaultAppPool)`` 。关于在应用程序池中运行的更多注意事项，查看 :ref:`apppool` 。

.. code:: powershell

  Import-module IISAdministration
  New-IISSite -Name "AspNet5" -PhysicalPath c:\HelloAspNet5\wwwroot -BindingInformation "*:8000:"

Manually Editing *applicationHost.config*
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

手动编辑 *applicationHost.config*
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can also create the site by manually editing the *applicationHost.config* file.

你也可以通过手动编辑 *applicationHost.config* 文件来创建站点。

.. literalinclude:: nano-server/applicationHost.config
  :language: xml
  :lines: 152,161-168,175
  :dedent: 8
  :emphasize-lines: 2-9

Open a Port in the Firewall
---------------------------

在防火墙中打开端口
---------------------------

Since we have IIS listening on port **8000** and forwarding request to our application, we will need open up the port to TCP traffic.

因为我们使用 IIS 在 **8000** 端口监听和转发请求到我们的应用程序，我们需要开放这个端口的 TCP 通信。

.. code:: ps1

  New-NetFirewallRule -Name "AspNet5" -DisplayName "HTTP on TCP/8000" -Protocol TCP -LocalPort 8000 -Action Allow -Enabled True

Running the Application
-----------------------

运行应用程序
-----------------------

At this point your published web application, should be accessible in browser by visiting ``http://<ip-address>:8000``.
If you have set up logging as described in :ref:`log-redirection`, you should be able to view your logs at *C:\\HelloAspNet5\\logs*.

此时你已经发布了你的 Web 应用程序，通过浏览器访问 ``http://<ip-address>:8000`` 应该能访问到(你的应用程序)。如果你设置了 :ref:`log-redirection` 中介绍的日志，你应该能够在 *C:\\HelloAspNet5\\logs* 查看你的日志。




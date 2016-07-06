.. _nano-server:

在 Nano Server 上运行ASP.NET Core
=================================

ASP.NET Core on Nano Server
===========================

原文 `ASP.NET Core on Nano Server <https://docs.asp.net/en/latest/tutorials/nano-server.html>`_

作者 `Sourabh Shirhatti`_

翻译 `娄宇(Lyrics) <http://github.com/xbuilder>`_

校对 `刘怡(AlexLEWIS) <https://github.com/alexinea>`_、`许登洋(Seay) <https://github.com/SeayXu>`_、`谢炀(kiler) <https://github.com/kiler398>`_

.. attention:: This tutorial uses a pre-release version of the Nano Server installation option of Windows Server Technical Preview 5. You may use the software in the virtual hard disk image only to internally demonstrate and evaluate it. You may not use the software in a live operating environment. Please see https://go.microsoft.com/fwlink/?LinkId=624232 for specific information about the end date for the preview.

.. attention:: 本教程使用 Windows Server Technical Preview 5 中的预发行版本的 Nano Server 安装选项。 你可以使用虚拟硬盘镜只用来内部演示和评估它。不能将该软件在生产环境中使用。请看 https://go.microsoft.com/fwlink/?LinkId=624232 查看具体的预览截止日期信息。

In this tutorial, you'll take an existing ASP.NET Core app and deploy it to a Nano Server instance running IIS.

在本教程中，你将使用一个现有的 ASP.NET Core 应用程序并将其部署在一个 Nano Server 实例的 IIS 上。

.. contents:: Sections:
  :local:
  :depth: 1

Introduction
------------

介绍
------------

Nano Server is an installation option in Windows Server 2016, offering a tiny footprint, better security and better servicing than Server Core or full Server. Please consult the official `Nano Server documentation <https://technet.microsoft.com/en-us/library/mt126167.aspx>`__ for more details.  There are 3 ways for you try out Nano Server for yourself:
1.	You can download the Windows Server 2016 Technical Preview 5 ISO file, and build a Nano Server image
2.	Download the Nano Server developer VHD
3.	Create a VM in Azure using the Nano Server image in the Azure Gallery. If you don’t have an Azure account, you can get a free 30-day trial

Nano Server 是 Windows Server 2016 附属的一个安装选项，比 Server Core 或者 full Server 提供更小的安装体积，更好的安全性以及更好的服务性能。 请参考官方 `Nano Server 文档 <https://technet.microsoft.com/en-us/library/mt126167.aspx>`__ 获取更多内容。  有以下三种方法来试用 Nano Server：
1.	你可以下载 Windows Server 2016 Technical Preview 5 ISO 文件，并且生成 Nano Server 镜像。
2.	下载 Nano Server 开发者 VHD （虚拟磁盘文件）
3.	在 Azure 中从 Azure Gallery 使用 Nano Server 镜像创建虚拟机。如果没有 Azure 账户，你可以申请一个 30天免费试用账户。

In this tutorial, we will be using the pre-built `Nano Server Developer VHD <https://msdn.microsoft.com/en-us/virtualization/windowscontainers/nano_eula>`_  from Windows Server Technical Preview 5.

在本教程中，我们将使用在 Windows Server Technical Preview 5 中预创建的 `Nano Server Developer VHD <https://msdn.microsoft.com/en-us/virtualization/windowscontainers/nano_eula>`_ 。

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

打开一个提升过权限的 PowerShell 窗口来添加你的远程 Nano Server 实例到你的 ``受信任的主机（TrustedHosts）`` 列表。

.. code:: ps1

  $nanoServerIpAddress = "10.83.181.14"
  Set-Item WSMan:\localhost\Client\TrustedHosts "$nanoServerIpAddress" -Concatenate -Force

``NOTE:`` Replace the variable ``$nanoServerIpAddress`` with the correct IP address.

``NOTE:`` 把 ``$nanoServerIpAddress`` 替换为对应的 IP 地址。

Once you have added your Nano Server instance to your ``TrustedHosts``, you can connect to it using PowerShell remoting

一旦你添加了 Nano Server 实例到你的 ``受信任的主机（TrustedHosts）`` 列表，你就可以用 PowerShell Remoting 连接它了。

.. code:: ps1

  $nanoServerSession = New-PSSession -ComputerName $nanoServerIpAddress -Credential ~\Administrator
  Enter-PSSession $nanoServerSession

A successful connection results in a prompt with a format looking like: ``[10.83.181.14]: PS C:\Users\Administrator\Documents>``

成功连接的命令提示符将看起来像这样 ``[10.83.181.14]: PS C:\Users\Administrator\Documents>``

Creating a file share

创建文件共享
---------------------
Create a file share on the Nano server so that the published application can be copied to it. Run the following commands in the remote session:

在 Nano server 上创建文件共享这样可以直接拷贝发布的程序，在远程服务器上运行下述命令：

.. code:: ps1

  mkdir C:\PublishedApps\AspNetCoreSampleForNano
  netsh advfirewall firewall set rule group="File and Printer Sharing" new enable=yes
  net share AspNetCoreSampleForNano=c:\PublishedApps\AspNetCoreSampleForNano /GRANT:EVERYONE`,FULL

After running the above commands you should be able to access this share by visiting ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano`` in the host machine's Windows Explorer.

上述命令运行完以后，你就可以在你本机使用 Windows 文件浏览器通过  ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano`` 地址访问共享文件了。

Open port in the Firewall
--------------------------
Run the following commands in the remote session to open up a port in the firewall to listen for TCP traffic.

打开防火墙端口
--------------------------
在远程会话中运行下面的命令在防火墙中打开一个端口来监听TCP流量。

.. code:: ps1

  New-NetFirewallRule -Name "AspNet5 IIS" -DisplayName "Allow HTTP on TCP/8000" -Protocol TCP -LocalPort 8000 -Action Allow -Enabled True

Installing IIS
--------------

安装 IIS
--------------

Add the ``NanoServerPackage`` provider from the PowerShell gallery. Once the provider is installed and imported, you can install Windows packages.

从 PowerShell 库平台（PowerShell gallery）中添加 ``NanoServerPackage`` provider，一旦 provider 被安装或者导入，你就可以安装 Window 包了。


Run the following commands in the PowerShell session that was created earlier:

在前面创建的 PowerShell 会话中运行以下代码：

.. code:: ps1

  Install-PackageProvider NanoServerPackage
  Import-PackageProvider NanoServerPackage
  Install-NanoServerPackage -Name Microsoft-NanoServer-IIS-Package

To quickly verify if IIS is setup correctly, you can visit the url ``http://<nanoserver-ip-address>/`` and should see a welcome page. When IIS is installed, by default a web site called ``Default Web Site`` listening on port 80 is created.

为了快速验证ISS是否正确安装，你可以访问 ``http://<nanoserver-ip-address>/`` 链接看看是否可以显示欢迎页面。当IIS被安装好以后，默认会创建一个名为 ``Default Web Site`` 的网站在80端口上侦听。

Installing the ASP.NET Core Module (ANCM)
-----------------------------------------

The ASP.NET Core Module is an IIS 7.5+ module which is responsible for process management of ASP.NET Core HTTP listeners and to proxy requests to processes that it manages. At the moment, the process to install the ASP.NET Core Module for IIS is manual. You will need to install the version of the `.NET Core Windows Server Hosting bundle <https://dot.net/>`__ on a regular (not Nano) machine. After installing the bundle on a regular machine, you will need to copy the following files to the file share that we created earlier.

The ASP.NET Core Module 是一个适用于 IIS 7.5 及以上版本的组件，它用来负责 ASP.NET Core HTTP 监听器的过程管理和代理请求的过程管理。 目前需要手动在 IIS 上安装 ASP.NET Core 组件。你需要在你的常规机（不是 Nano Server）上安装最新的 64 位版本的 `.NET Core Windows Server Hosting bundle <https://dot.net/>`__ 。安装之后你需要复制以下文件：

On a regular (not Nano) machine run the following copy commands:

在常规机（不是 Nano Server）上运行下述拷贝命令：

.. code:: ps1

  copy C:\windows\system32\inetsrv\aspnetcore.dll ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano``
  copy C:\windows\system32\inetsrv\config\schema\aspnetcore_schema.xml ``\\<nanoserver-ip-address>\AspNetCoreSampleForNano``

On a Nano machine, you will need to copy the following files from the file share that we created earlier to the valid locations.
So, run the following copy commands:

在 Nano 主机中你需要从前面创建的文件共享中复制这下面的文件到对应的位置。
运行下面拷贝脚本：

.. code:: ps1

  copy C:\PublishedApps\AspNetCoreSampleForNano\aspnetcore.dll C:\windows\system32\inetsrv\
  copy C:\PublishedApps\AspNetCoreSampleForNano\aspnetcore_schema.xml C:\windows\system32\inetsrv\config\schema\

Run the following script in the remote session:

在远程会话中运行下面的脚本：

.. literalinclude:: nano-server/enable-ancm.ps1

``NOTE:`` Delete the files ``aspnetcore.dll`` and ``aspnetcore_schema.xml`` from the share after the above step.

``NOTE:`` 从共享中删除 ``aspnetcore.dll`` 和 ``aspnetcore_schema.xml`` 文件在上述步骤之后。
 
 
 Installing .NET Core Framework
------------------------------
If you published a portable app, .NET Core must be installed on the target machine. Execute the following Powershell script in a remote Powershell session to install the .NET Framework on your Nano Server.

 安装 .NET Core Framework
------------------------------
如果你发布移动应用， .NET Core 必须安装在目标机器。 在远程 Powershell 会话中执行下述 Powershell 脚本来在你的 Nano Server 安装 .NET Framework。
 
.. literalinclude:: nano-server/Download-Dotnet.ps1
  :language: powershell
 


Enabling the HttpPlatformHandler
--------------------------------

启用 HttpPlatformHandler
--------------------------------

You can execute the following PowerShell script in a remote PowerShell session to enable the HttpPlatformHandler module on the Nano server.

你可以在 PowerShell 远程会话中执行以下 PowerShell 脚本来在 Nano Server 上启用 HttpPlatformHandler 组件。

.. note:: This script runs on a clean system, but is not meant to be idempotent. If you run this multiple times it will add multiple entries. If you end up in a bad state, you can find backups of the *applicationHost.config* file at *%systemdrive%\inetpub\history*.

.. note:: 这份脚本可以运行在纯净的系统里，但是不能运行多次。如果你运行这份脚本多次，它将会添加多个条目。如果你最终处于糟糕的状态，你可以在 *%systemdrive%\inetpub\history* 中找到 *applicationHost.config* 文件的备份。

.. literalinclude:: nano-server/enable-platformhandler.ps1
  :language: ps1

Publishing the application
--------------------------
Copy over the published output of your existing application to the file share. 

发布应用程序
-----------------------------------
将你发布好的现有应用程序复制到文件共享。

You may need to make changes to your *web.config* to point to where you extracted ``dotnet.exe``. Alternatively, you can add ``dotnet.exe`` to your path.

您可能需要修改你的  *web.config*  文件指向你解压缩 ``dotnet.exe`` 文件的路径。或者，你也可以把 ``dotnet.exe`` 添加到你的路径。

Example of how a web.config might look like if ``dotnet.exe`` was **not** on the path:

下面是一个修改 web.config 的例子，当 ``dotnet.exe`` **不在** 当前路径中：

.. code:: xml

  <?xml version="1.0" encoding="utf-8"?>
  <configuration>
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModule" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="C:\dotnet\dotnet.exe" arguments=".\AspNetCoreSampleForNano.dll" stdoutLogEnabled="false" stdoutLogFile=".\logs\stdout" forwardWindowsAuthToken="true" />
    </system.webServer>
  </configuration>


Run the following commands in the remote session to create a new site in IIS for the published app. This script uses the ``DefaultAppPool`` for simplicity. For more considerations on running under an application pool, see :ref:`apppool`.

在远程会话中运行以下命令在IIS中为已发布的应用创建一个新的站点。此脚本只是简单的使用了 ``DefaultAppPool`` 。更多关于应用程序池的信息，请参阅 :ref:`apppool` 。


.. code:: powershell

  Import-module IISAdministration
  New-IISSite -Name "AspNetCore" -PhysicalPath c:\PublishedApps\AspNetCoreSampleForNano -BindingInformation "*:8000:"



Running the Application
-----------------------

运行应用程序
-----------------------


The published web app should be accessible in browser at ``http://<nanoserver-ip-address>:8000``.
If you have set up logging as described in :ref:`log-redirection`, you should be able to view your logs at *C:\\PublishedApps\\AspNetCoreSampleForNano\\logs*.

发布好的 Web 应用程序可以通过浏览器打开 ``http://<nanoserver-ip-address>:8000`` 链接访问到。
如果你按照 :ref:`log-redirection` 介绍的方式设置好了日志。你应该能够在  *C:\\PublishedApps\\AspNetCoreSampleForNano\\logs* 目录中查看你的日志。
 
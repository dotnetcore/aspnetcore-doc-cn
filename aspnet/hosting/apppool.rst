.. _apppool:

Application Pools
=================

应用程序池
=================

By `Sourabh Shirhatti`_

作者： `Sourabh Shirhatti`_

翻译 `谢炀（Kiler） <https://github.com/kiler398/aspnetcore>`_ 

When hosting multiple web sites on a single server, you should consider isolating the applications from each other by running each application in its own application pool. This document provides an overview of how to set up Application Pools to securely host multiple web sites on a single server.

当在一台服务器上托管多个网站的时候，你应该考虑通过在各自的应用程序池中上运行应用程序来达到应用程序彼此隔离的效果。本文提供了如何设置应用程序池来在一台服务器上安全地承载的多个网站的概述。

Application Pool Identity Account
---------------------------------

应用程序池标识帐户
---------------------------------

An application pool identity account allows you to run an application under a unique account without having to create and manage domains or local accounts. On IIS 8.0+ the IIS Admin Worker Process (WAS) will create a virtual account with the name of the new application pool and run the application pool's worker processes under this account by default.

应用程序池标识帐户可以让你的应用程序在一个唯一的账户下运行而无需创建和管理域帐户或本地帐户。在IIS8.0及更高版本的IIS管理工作进程（WAS）将以应用程序池的名称创建一个新的虚拟帐户，应用程序池的工作进程默认情况下以该帐户下运行。

Configuring IIS Application Pool Identities
-------------------------------------------

配置 IIS 应用程序池标识帐户
-------------------------------------------

In the IIS Management Console, under **Advanced Settings** for your application pool ensure that `Identity` list item is set to use **ApplicationPoolIdentity** as shown in the image below.

在 IIS 管理控制台中，在 **Advanced Settings** 为您的应用程序池确保 `Identity` 列表项设置为使用 **ApplicationPoolIdentity** ，如下图所示。

.. image:: apppool/_static/apppool-identity.png

Securing Resources
------------------

安全资源
------------------

The IIS management process creates a secure identifier with the name of the application pool in the Windows Security System. Resources can be secured by using this identity, however this identity is not a real user account and will not show up in the Windows User Management Console.

IIS 管理进程在Windows安全系统中以应用程序池的名称创建一个安全标识符。资源可以通过使用这个标识符来被保护，然而，这标识符是不是一个真正的用户帐户，并且在Windows用户管理控制台中不会出现。

To grant the IIS worker process access to your application, you will need to modify the Access Control List (ACL) for the the directory containing your application.

为了授权 IIS 工作进程访问您的应用程序，您需要为包含应用程序的目录修改访问控制列表（ACL）。

1. Open Windows Explorer and navigate to the directory.
2. Right click on the directory and click properties.
3. Under the **Security** tab, click the **Edit** button and then the **Add** button
4. Click the **Locations** and make sure you select your server.

1. 打开 Windows 文件浏览器并跳转到目录。
2. 右击目录并点击属性菜单。
3. 在 **Security** 选项卡下， 点击 **Edit** 并点击 **Add** 按钮。
4. 点击 **Locations** 并确认选中了你的服务器。

.. image:: apppool/_static/apppool-adduser.jpg

5. Enter **IIS AppPool\\DefaultAppPool** in **Enter the object names to select** textbox.
6. Click the **Check Names** button and then click **OK**.

5. 在 **Enter the object names to select** 文本框中输入 **IIS AppPool\\DefaultAppPool** 。
6. 点击 **Check Names** 按钮并点击 **OK**。

You can also do this via the command-line by using **ICACLS** tool.

你也可以直接通过命令行使用 **ICACLS** 工具。

.. code:: bat

    ICACLS C:\sites\MyWebApp /grant "IIS AppPool\DefaultAppPool" :F


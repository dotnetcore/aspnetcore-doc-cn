.. _dataprotection:

.. _数据保护:

Data Protection
===============

数据保护
===============

By `Sourabh Shirhatti`_

作者： `Sourabh Shirhatti`_

The ASP.NET Core data protection stack provides a simple and easy to use cryptographic API a developer can use to protect data, including key management and rotation. This document provides an overview of how to configure Data Protection on your server to enable developers to use data protection.

ASP.NET Core 数据保护堆栈为开发人员提供了一种简单易用的密码API来保护数据，包括密钥管理与旋转。本文概述如何在你的服务器配置数据保护，以使开发人员使用数据保护。

Configuring Data Protection
---------------------------

配置数据保护
---------------------------

.. WARNING::

.. 注意::

  Data Protection is used by various ASP.NET middleware, including those used in authentication. The default behavior on IIS hosted web sites is to store keys in memory and discard them when the process restarts. This behavior will have side effects, for example, discarding keys invalidate any cookies written by the cookie authentication and users will have to login again.
  
  数据保护被用在大量的ASP.NET中间件，包括被用在权限验证的中间件。IIS宿主的Web站点默认是将密钥保存在内存中并在进程重启时丢弃。这种行为存在副作用，例如：丢弃的密钥造成使用Cookie的验证失效，用户便不得不重新登陆。
  
To automatically persist keys for an application hosted in IIS, you must create registry hives for each application pool. Use the `Provisioning PowerShell script <https://github.com/aspnet/DataProtection/blob/dev/Provision-AutoGenKeys.ps1>`_ for each application pool you will be hosting ASP.NET Core applications under. This script will create a special registry key in the HKLM registry that is ACLed only to the worker process account. Keys are encrypted at rest using DPAPI.

为使IIS宿主的应用能自动持久化密钥，你必须为每个应用程序池创建注册表配置单元。为每个应用程序池运行`Provisioning PowerShell 脚本 <https://github.com/aspnet/DataProtection/blob/dev/Provision-AutoGenKeys.ps1>`，即可完成ASP.NET Core应用程序的宿主部署。这个脚本将在HKLM注册表创建一个特殊的注册表配置单元，此配置单元只受活动进程账户访问控制。密钥被使用数据保护应用程序编程接口（DPAPI）加密静态存在。

.. note:: A developer can configure their applications Data Protection APIs to store data on the file system. Data Protection can be configured by the developer to use a UNC share to store keys, to enable load balancing. A hoster should ensure that the file permissions for such a share are limited to the Windows account the application runs as. In addition a developer may choose to protect keys at rest using an X509 certificate. A hoster may wish to consider a mechanism to allow users to upload certificates, place them into the user's trusted certificate store and ensure they are available on all machines the users application will run on.

.. 备注:: 开发人员可以配置自己的应用数据保护API，将数据保存在文件系统。数据保护可以被开发人员配置为使用UNC共享（UNC share）保存密钥，以启用负载均衡。主机所有者需要确保此类共享文件权限限制到运行的Windows账户。此外，开发人员应该选择采用X509认证凭证保护静态密钥。主机所有者也许希望考虑一种机制以允许用户上传凭证证书，将其放置在用户信任的证书存储区，并确保能被所有运行用户应用的计算机使用。

Machine Wide Policy
-------------------

计算机领域策略
-------------------

The data protection system has limited support for setting default :ref:`machine-wide policy <data-protection-configuration-machinewidepolicy>` for all applications that consume the data protection APIs. See the :doc:`data protection </security/data-protection/index>` documentation for more details.

默认设置数据保护系统对数据保护支持有限。:参考:计算机领域策略 <data-protection-configuration-machinewidepolicy> 适用所有使用数据保护API的应用程序。查阅:文档: 数据保护 </security/data-protection/index>` 文档了解更多详细内容.

使用Visual Studio发布一个Azure云Web应用程序 
===============================================

作者 `Erik Reitan`_
翻译 `谢炀（kiler）`_
校对 

本文描述了如何使用Visual Studio将一个Web应用程序发布到Azure云

**注意:** 为了完成这个教程, 你必须拥有一个微软Azure帐号. 如果没有账户, 你可以 `激活你的MSDN订阅用户权益`_ 或者 `注册免费试用版`_.

.. _`激活你的MSDN订阅用户权益`: http://azure.microsoft.com/pricing/member-offers/msdn-benefits-details/?WT.mc_id=A261C142F

.. _`注册免费试用版`: http://azure.microsoft.com/pricing/free-trial/?WT.mc_id=A261C142F


创建一个新的ASP.NET Web应用程序或者打开一个已存在的ASP.NETWeb应用程序. 

1. 在Visual Studio的**Solution Explorer**里面, 右击项目选择**Publish**.

.. image:: publish-to-azure-webapp-using-vs/_static/01-Publish.png

2. 在**Publish Web**弹出对话框里面, 点击**Microsoft Azure Web Apps**并且登入你的Azure订阅账户.

.. image:: publish-to-azure-webapp-using-vs/_static/02-PublishWebdb.png

3. 在**Select Existing Web App**对话框中点击**New**在Azure云中创建一个新的Web应用程序.

.. image:: publish-to-azure-webapp-using-vs/_static/03-SelectExistingWebAppdb.png

4. 输入站点名和区域.作为可选项你可以创建一个新的数据库服务器, 当然如果你以前创建过了,就可以直接使用. 当你设置完成以后, 点击**Create**.

.. image:: publish-to-azure-webapp-using-vs/_static/04-CreateWebAppOnMicrosoftAzuredb.png

数据库服务器是一种宝贵的资源。最好使用现有的服务器来测试和开发。这个阶段**不会**验证数据库密码, 如果你输入了一个错误的值, 在你的Web应用程序视图访问数据库之前你是不会收到任何错误消息的.

5. 在**Publish Web**对话框的**Connection**选项卡, 点击**Publish**.

.. image:: publish-to-azure-webapp-using-vs/_static/05-PublishWebdb.png

你可以在Visual Studio的**Web Publish Activity**的窗体查看发布进度.

.. image:: publish-to-azure-webapp-using-vs/_static/06-WebPublishActivityWindow.png

当发布到Azure的工作完成以后, 你的Web应用程序就可以挂载在Azure上通过浏览器直接访问了. 

.. image:: publish-to-azure-webapp-using-vs/_static/07-Browser.png



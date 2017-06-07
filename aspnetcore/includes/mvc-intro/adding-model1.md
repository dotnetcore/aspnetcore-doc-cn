# 添加模型

作者： [Rick Anderson](https://twitter.com/RickAndMSFT) 以及 [Tom Dykstra](https://github.com/tdykstra)

翻译： [娄宇(Lyrics)](http://github.com/xbuilder) 

校对： [许登洋(Seay)](https://github.com/SeayXu) 、[姚阿勇(Mr.Yao)](https://github.com/YaoaY) 、[夏申斌](https://github.com/xiashenbin) 、[孟帅洋(书缘)](https://github.com/mengshuaiyang)  

在这一节里，你将添加一些类来管理数据库中的电影数据。这些类将成为 **M**VC 应用程序中的 "**M**odel" 部分。

你将使用 .NET Framework 中名为 [Entity Framework Core](https://docs.microsoft.com/ef/core) (EF Core) 的数据库访问技术来使用这些数据模型类。EF Core 是一种可以用来简化数据访问代码的关系对象映射 (ORM) 框架。本教程中你会使用到 SQLite 数据库，但是[EF Core 还支持更多的数据库](https://docs.microsoft.com/ef/core/providers/)。

你创建的模型类也被称为 POCO 类 (源自 "plain-old CLR objects." )，这些类无需和 EF Core 有任何依赖。他们的作用仅仅是定义数据库属性并且能够存储到数据库。

本文中你将先编写一些数据模型类，然后 EF Core 根据你的类创建数据库。另外一种没有介绍的方式是从已经存在的数据库生成模型类，关于这个方式的信息，参考 [ASP.NET Core - 已存在数据库](https://docs.microsoft.com/ef/core/get-started/aspnetcore/existing-db)。

## 添加数据模型类

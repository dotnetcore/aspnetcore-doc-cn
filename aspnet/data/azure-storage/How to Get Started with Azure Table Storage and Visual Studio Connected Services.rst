如何开始 Azure Table storage 和 Visual Studio 连接服务
=====

作者：`Patrick Sheahan`、`Tom Archer`、`Robin Shahan`、`Kemp Brown`、`Peter Blazejewicz`、`Linda Lu Cannon`、`Chris Rummel`

翻译：`耿晓亮(Blue)`

概述
---------------------

本文描述了在 ASP.NET 5 项目中通过 Visual Studio **添加连接服务** 对话框中创建或者引用 Azure storage 账号后在 Visual Studio 中如何开始用 Azure Table storage。

Azure Table storage 服务可以让你存储大量的结构数据。NoSQL 数据存储接受来自内部和外部 Azure cloud 有效的调用。Azure tables 是理想的结构化，非关系型数据的存储。

在项目里 **添加连接服务** 安装适当的 NuGet 包访问 Azure storage 并添加 storage 账号的连接字符串到你的项目配置文件里。

更多关于 Azure Table storage 生成信息参见 `Get started with Azure Table storage using .NET <https://github.com/Azure/azure-content/blob/master/articles/storage/storage-dotnet-how-to-use-tables.md>`_。

开始前，你需要在存储账号里创建一张表。我们会展示给你如何用代码创建 Azure table。我们也会展示给你如何执行基础表和实体操作，例如添加，修改，读取和读取表实体。示例是用 C# 编写并用 .NET 的 Azure Storage Client Library。

**注意** -在 ASP.NET 5 中 Azure storage 的一些 API 的执行调用是异步的。更多信息参见 `Asynchronous Programming with Async and Await <https://msdn.microsoft.com/library/hh191443.aspx>`_。下边的代码假设用了 Async 编程。

用代码访问表
---------------------

在 ASP.NET 5 项目中访问表，你需要包括下面的条目到任一 C# 源文件用于访问 Azure table storage。

1. 确认 C# 文件的顶部命名空间声明包括这些 **using** 语句。

.. code-block:: c#

   using Microsoft.Framework.Configuration;
   using Microsoft.WindowsAzure.Storage;
   using Microsoft.WindowsAzure.Storage.Table;
   using System.Threading.Tasks;
   using LogLevel = Microsoft.Framework.Logging.LogLevel;

2. 获取 **CloudStorageAccount** 对象表示你的存储账号信息。使用下面的代码来获取你的存储账号连接字符串和 Azure 服务配置里的存储账号信息。

.. code-block:: c#

   CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
       CloudConfigurationManager.GetSetting("<storage-account-name>_AzureStorageConnectionString"));

**注意** -在下边的示例代码之前应用以上所有的代码。

3. 在你的存储账号里获取 **CloudTableClient** 引用表对象

.. code-block:: c#

   // Create the table client.
   CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

4. 获取 **CloudTable** 的引用对象来引用特定的表和实体。

.. code-block:: c#

   // Get a reference to a table named "peopleTable"
   CloudTable table = tableClient.GetTableReference("peopleTable");

用代码创建表
---------------------

创建 **Azure table**，只需添加 **CreateIfNotExistsAsync()** 调用。

.. code-block:: c#

   // Create the CloudTable if it does not exist
   await table.CreateIfNotExistsAsync();

向表中添加实体
---------------------

通过定义实体的属性来创建一个实体并添加到表中。下边的代码定义了叫作 **CustomerEntity** 的实体，用客户的 first name 作为 row key、last name 作为 partition key。

.. code-block:: c#

   public class CustomerEntity : TableEntity
   {
       public CustomerEntity(string lastName, string firstName)
       {
           this.PartitionKey = lastName;
           this.RowKey = firstName;
       }

       public CustomerEntity() { }

       public string Email { get; set; }

       public string PhoneNumber { get; set; }
   }

通过之前“用代码访问表”创建的 **CloudTable** 对象完成实体的表操作。**TableOperation** 对象代表要做的操作。下面的代码示例演示了如何创建 **CloudTable** 对象和 **CustomerEntity** 对象。准备操作，用创建的 **TableOperation** 插入客户实体到表里。最后，通过调用 **CloudTable.ExecuteAsync** 执行操作。

.. code-block:: c#

   // Create a new customer entity.
   CustomerEntity customer1 = new CustomerEntity("Harp", "Walter");
   customer1.Email = "Walter@contoso.com";
   customer1.PhoneNumber = "425-555-0101";

   // Create the TableOperation that inserts the customer entity.
   TableOperation insertOperation = TableOperation.Insert(customer1);

   // Execute the insert operation.
   await peopleTable.ExecuteAsync(insertOperation);

批量插入实体
---------------------

你可以通过一次写入操作插入多个实体到表里。下面的代码示例创建了两个实体对象（"Jeff Smith" 和 "Ben Smith"），通过插入方法添加到 **TableBatchOperation** 对象，然后通过调用 **CloudTable.ExecuteBatchAsync** 执行操作。

.. code-block:: c#

   // Create the batch operation.
   TableBatchOperation batchOperation = new TableBatchOperation();

   // Create a customer entity and add it to the table.
   CustomerEntity customer1 = new CustomerEntity("Smith", "Jeff");
   customer1.Email = "Jeff@contoso.com";
   customer1.PhoneNumber = "425-555-0104";

   // Create another customer entity and add it to the table.
   CustomerEntity customer2 = new CustomerEntity("Smith", "Ben");
   customer2.Email = "Ben@contoso.com";
   customer2.PhoneNumber = "425-555-0102";

   // Add both customer entities to the batch insert operation.
   batchOperation.Insert(customer1);
   batchOperation.Insert(customer2);

   // Execute the batch operation.
   await peopleTable.ExecuteBatchAsync(batchOperation);

获取分区里的所有实体
---------------------

通过 **TableQuery** 对象查询分区表的所有实体。下边的代码示例指定了一个实体的 partition key 为 'Smith' 的过滤器。这个示例将查询结果中的每一个实体打印到控制台。

.. code-block:: c#

   // Construct the query operation for all customer entities where PartitionKey="Smith".
   TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

   // Print the fields for each customer.
   TableContinuationToken token = null;
   do
   {
       TableQuerySegment<CustomerEntity> resultSegment = await peopleTable.ExecuteQuerySegmentedAsync(query, token);
       token = resultSegment.ContinuationToken;

       foreach (CustomerEntity entity in resultSegment.Results)
       {
           Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
           entity.Email, entity.PhoneNumber);
       }
   } while (token != null);

获取单个实体
---------------------

你可以写一个查询获取单个指定实体。下边的代码使用一个 **TableOperation** 对象指定一个叫 'Ben Smith' 的客户。此方法只返回一个实体而不是一个集合，并且 **TableResult.Result** 的返回值是一个 **CustomerEntity** 对象。在查询中指定 partition keys 和 row keys 是检索 **Table** 服务中单个实体的最快捷方式。

.. code-block:: c#

   // Create a retrieve operation that takes a customer entity.
   TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

   // Execute the retrieve operation.
   TableResult retrievedResult = await peopleTable.ExecuteAsync(retrieveOperation);

   // Print the phone number of the result.
   if (retrievedResult.Result != null)
      Console.WriteLine(((CustomerEntity)retrievedResult.Result).PhoneNumber);
   else
      Console.WriteLine("The phone number could not be retrieved.");

删除实体
---------------------

找到实体后你可以删除它。下边代码查询一个叫 "Ben Smith" 的客户实体并且如果找到了就删除它。

.. code-block:: c#

   // Create a retrieve operation that expects a customer entity.
   TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>("Smith", "Ben");

   // Execute the operation.
   TableResult retrievedResult = peopleTable.Execute(retrieveOperation);

   // Assign the result to a CustomerEntity object.
   CustomerEntity deleteEntity = (CustomerEntity)retrievedResult.Result;

   // Create the Delete TableOperation and then execute it.
   if (deleteEntity != null)
   {
      TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

      // Execute the operation.
      await peopleTable.ExecuteAsync(deleteOperation);

      Console.WriteLine("Entity deleted.");
   }

   else
      Console.WriteLine("Couldn't delete the entity.");

后续步骤
---------------------

[AZURE.INCLUDE `vs-storage-dotnet-tables-next-steps <https://github.com/Azure/azure-content/blob/master/includes/vs-storage-dotnet-tables-next-steps.md>`_]
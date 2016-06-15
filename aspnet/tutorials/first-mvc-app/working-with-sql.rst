Working with SQL Server LocalDB    
===============================
ʹ�� SQL Server LocalDB
================

���룺 `κ����(����) <http://github.com/ChujianA>`_

У�ԣ� `

 The ``ApplicationDbContext`` class handles the task of connecting to the database and mapping ``Movie`` objects to database records. The database context is registered with the :doc:`Dependency Injection  </fundamentals/dependency-injection>` container in the ``ConfigureServices`` method in the *Startup.cs* file:

``ApplicationDbContext`` �ฺ���������ݿⲢ�� ``Movie`` ��������ݼ�¼����ӳ�䡣 *Startup.cs* �ļ��У����ݿ����������� ``ConfigureServices`` �������� :doc:`Dependency Injection  </fundamentals/dependency-injection>`_ ��������ע��ġ�

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 39-45
  :dedent: 8
  :emphasize-lines: 5,6

The ASP.NET Core :doc:`Configuration </fundamentals/configuration>`_ system reads the ``ConnectionString``. For local development, it gets the connection string from the *appsettings.json* file:

ASP.NET Core  :doc:`Configuration </fundamentals/configuration>`_ ϵͳ��ȡ ``ConnectionString`` ��Ϊ�˱��ؿ������� *appsettings.json* �ļ��л�ȡ�����ַ�����

.. literalinclude:: start-mvc/sample/src/MvcMovie/appsettings.json
  :language: javascript
  :lines: 1-6
  :emphasize-lines: 3

When you deploy the app to a test or production server, you can use an environment variable or another approach to set the connection string to a real SQL Server. See :doc:`Configuration </fundamentals/configuration>`_ .

���㲿��Ӧ�ó��򵽲��Է�������������������ʱ�������ʹ�û�������������һ�ַ���������ʵ�����ݿ�������ַ������� :doc:`Configuration </fundamentals/configuration>`_��

SQL Server Express LocalDB
--------------------------------

SQL Server Express LocalDB
--------------------------

LocalDB is a lightweight version of the SQL Server Express Database Engine that is targeted for program development. LocalDB starts on demand and runs in user mode, so there is no complex configuration. By default, LocalDB database creates "\*.mdf" files in the *C:/Users/<user>* directory.

LocalDB��һ����Գ��򿪷����������汾��SQL Server Express���ݿ����棬������Գ��򿪷���LocalDB���������������û�ģʽ�����У�������û�и��ӵ����á�Ĭ������£�LocalDB���ݿ��� *C:/Users/<user>* Ŀ¼�´��� "\*.mdf" �ļ���

- From the **View** menu, open **SQL Server Object Explorer** (SSOX).

�� **View** �˵��У���SQL Server������Դ��������**SQL Server Object Explorer** ��(SSOX)��.

.. image:: working-with-sql/_static/ssox.png

- Right click on the ``Movie`` table **> View Designer**

�һ� ``Movie`` �� **> ��ͼ�������View Designer��**


.. image:: working-with-sql/_static/design.png

.. image:: working-with-sql/_static/dv.png

Note the key icon next to ``ID``. By default, EF will make a property named ``ID`` the primary key.

ע��Կ��ͼ������ ``ID``��Ĭ������£�EF������Ϊ ``ID`` ��������Ϊ������

.. comment: add this when we have it for MVC 6: For more information on EF and MVC, see Tom Dykstra's excellent tutorial on MVC and EF.

- Right click on the ``Movie`` table **> View Data**

-�һ� ``Movie`` ��>**> �鿴���ݣ�View Data��**

.. image:: working-with-sql/_static/ssox2.png

.. image:: working-with-sql/_static/vd22.png

Seed the database
--------------------------

������ݿ�
-------------

Create a new class named ``SeedData`` in the *Models* folder. Replace the generated code with the following:

�� *Models* �ļ����д���һ������ ``SeedData`` �����ࡣ�����´����滻���ɵĴ��롣

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :lines: 3-62

Notice if there are any movies in the DB, the seed initializer returns.

ע��������ݿ��������д���movies������ʼ�������ء�

.. literalinclude:: start-mvc/sample/src/MvcMovie/Models/SeedData.cs
  :language: c#
  :lines: 18-21
  :dedent: 12
  :emphasize-lines: 3

Add the seed initializer to the end of the ``Configure`` method in the *Startup.cs* file: 

*Startup.cs* �ļ��У��� ``Configure`` ����������������ʼ������

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-88
  :dedent: 8
  :emphasize-lines: 8

Test the app

����Ӧ�ó���

- Delete all the records in the DB. You can do this with the delete links in the browser or from SSOX.

- �����ݿ���ɾ�����еļ�¼����������������ʹ��ɾ�����ӻ����� SQL Server������Դ��������SSOX����������¡�

- Force the app to initialize (call the methods in the ``Startup`` class) so the seed method runs. To force initialization, IIS Express must be stopped and restarted. You can do this with any of the following approaches:

- ǿ��Ӧ�ó����ʼ������ ``Startup`` ���е��÷�����������䷽�����С�Ϊ�˳�ʼ����IIS Express����ֹͣ��Ȼ���������������������е��κ�һ��������ʵ�֣�

.. comment this no longer works  - ^<Shift>F5 (Hold down the control and Shift keys and tap F5)
  - Right click the IIS Express system tray icon in the notification area and tap **Exit** or **Stop* Site*
  - .. image:: working-with-sql/_static/iisExIcon.png
  - .. image:: working-with-sql/_static/stopIIS.png
  - If you were running VS in non-debug mode, press F5 to run in debug mode
  - If you were running VS in debug mode, stop the debugger and press ^F5

.. Note:: If the database doesn't initialize, put a break point on the line ``if (context.Movie.Any())`` and start debugging.

.. Note:: ��������ݿ�û�г�ʼ������ ``if (context.Movie.Any())`` �������öϵ㣬����ʼ����

.. image:: working-with-sql/_static/dbg.png

The app shows the seeded data.

Ӧ�ó�����ʾ�˱���������

.. image:: working-with-sql/_static/m55.png
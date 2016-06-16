
Examining the Details and Delete methods
======================================================
��ѯDetails��Delete����

By `Rick Anderson`_

���� `Rick Anderson`_

���룺 `κ����(����) <http://github.com/ChujianA>`_ 
У�ԣ�   

Open the Movie controller and examine the ``Details`` method:

��Movie���������Ҳ鿴 ``Details`` ������

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 29-44
 :dedent: 8

The MVC scaffolding engine that created this action method adds a comment showing a HTTP request that invokes the method. In this case it's a GET request with three URL segments, the ``Movies`` controller, the ``Details`` method and a ``id`` value. Recall these segments are defined in Startup.

MVC scaffolding���������һ��ע����ʾ���ڵ���һ��HTTP���󷽷��С���������£�GET������3��URL�Σ� ``Movies`` �������� ``Details`` ������һ�� ``id`` ֵ���ǵ������ζ�����Startup�ж���ġ�

.. literalinclude:: start-mvc/sample/src/MvcMovie/Startup.cs
  :language: c#
  :lines: 80-86
  :dedent: 8
  :emphasize-lines: 5

Code First makes it easy to search for data using the ``SingleOrDefaultAsync`` method. An important security feature built into the method is that the code verifies that the search method has found a movie before the code tries to do anything with it. For example, a hacker could introduce errors into the site by changing the URL created by the links from  *http://localhost:xxxx/Movies/Details/1* to something like  *http://localhost:xxxx/Movies/Details/12345* (or some other value that doesn't represent an actual movie). If you did not check for a null movie, the app would throw an exception.

Code Firstʹ�� ``SingleOrDefaultAsync`` �����������ݱȽ����ס�һ����Ҫ��ȫ�������õ��˷����С�������֤���������Ѿ��ҵ���movie��Ȼ����ִ���������롣���磬�ڿͿ�������վ��ͨ������ *http://localhost:xxxx/Movies/Details/1* �� *http://localhost:xxxx/Movies/Details/1* ������һЩ����ֵ����������ʵ�ʵ�movie���Ӷ�ʹ������URL���ִ��������û�м���Ƿ��ǿ�movie��Ӧ�ó���ͻ��׳�һ������

Examine the Delete and DeleteConfirmed methods.

�鿴Delete��DeleteConfirmed����

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-145
 :dedent: 8

Note that the ``HTTP GET Delete`` method doesn't delete the specified movie, it returns a view of the movie where you can submit (HttpPost) the deletion. Performing a delete operation in response to a GET request (or for that matter, performing an edit operation, create operation, or any other operation that changes data) opens up a security hole.

ע�� ``HTTP GET Delete`` ��������ɾ��ָ����movie,���������ύ (HttpPost) ɾ��movie��һ����ͼ��ʹ��GET����ִ��ɾ������������ִ�б༭�������������߻��߸������ݵ��κ���������������һ����ȫ©����

The ``[HttpPost]`` method that deletes the data is named ``DeleteConfirmed`` to give the HTTP POST method a unique signature or name. The two method signatures are shown below:

ɾ�����ݵ� ``[HttpPost]`` ����������Ϊ ``DeleteConfirmed`` ������Ϊ�˸�HTTP POST����һ��Ψһǩ�������ơ�������������ǩ������ͼ��ʾ��


.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 119-120,135-136,139
 :dedent: 8

The common language runtime (CLR) requires overloaded methods to have a unique parameter signature (same method name but different list of parameters). However, here you need two ``Delete`` methods -- one for GET and one for POST -- that both have the same parameter signature. (They both need to accept a single integer as a parameter.)


������������ʱҪ�����ط�����һ��Ψһ�Ĳ���ǩ������ͬ��������ͬ�����б������ǣ���������Ҫ���� ``Delete`` ���� -- һ��GET������һ��POST����--���Ƕ�������ͬ�Ĳ���ǩ����


There are two approaches to this problem, one is to give the methods different names. That's what the scaffolding mechanism did in the preceding example. However, this introduces a small problem: ASP.NET maps segments of a URL to action methods by name, and if you rename a method, routing normally wouldn't be able to find that method. The solution is what you see in the example, which is to add the ``ActionName("Delete")`` attribute to the ``DeleteConfirmed`` method. That attribute performs mapping for the routing system so that a URL that includes /Delete/ for a POST request will find the ``DeleteConfirmed`` method.

�����������취�����һ���⡣һ���Ǹ�������ͬ�����ơ����ǿ�ܴ�����ǰ��ʾ����ʹ�õķ�����Ȼ�����������һ��С���⣺ASP.NET��URL�Ĳ��ְ�������ӳ�䵽����������·��routingͨ�����ܹ��ҵ��Ǹ�������������������ʾ���п��Կ������� ``ActionName("Delete")`` ������ӵ� ``DeleteConfirmed``  �����С�����ִ��routing(·��)ϵͳ��ӳ�䣬������һ��URL���� /Delete/ ��POST�����ҵ� ``DeleteConfirmed`` ������

Another common work around for methods that have identical names and signatures is to artificially change the signature of the POST method to include an extra (unused) parameter. That's what we did in a previous post when we added the ``notUsed`` parameter. You could do the same thing here for the ``[HttpPost] Delete`` method:

��һ�������ķ�����������ͬ���ƺ�ǩ��������Ϊ�ĸ���POST������ǩ������һ�����⣨û���õģ���������֮ǰ�����е�������� ``notUsed`` �Ĳ���ʱ�������顣���������� ``[HttpPost] Delete`` ����������ͬ�����飺

.. literalinclude:: start-mvc/sample/src/MvcMovie/Controllers/MoviesController.cs
 :language: c#
 :lines: 312-322
 :dedent: 8

.. ToDo - Next steps, but it really needs to start with Tom's EF/MVC Core

.. ToDo - ��һ�ڣ���ʼTom's EF/MVC Core


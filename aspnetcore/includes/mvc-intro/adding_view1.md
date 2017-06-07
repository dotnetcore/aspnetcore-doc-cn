# 添加视图

作者： [Rick Anderson](https://twitter.com/RickAndMSFT)

翻译： [魏美娟(初见)](http://github.com/ChujianA)

校对： [赵亮(悲梦)](https://github.com/BeiMeng) 、[高嵩(Jack)](https://github.com/jack2gs) 、[娄宇(Lyrics)](https://github.com/xbuilder) 、[许登洋(Seay)](https://github.com/SeayXu)、[姚阿勇（Dr.Yao）](https://github.com/YaoaY) 

本节将修改 `HelloWorldController` 类，把使用 Razor 视图模板文件为客户端生成 HTML 响应的过程干净利落地封装起来。
 
您可以使用 Razor 视图引擎创建一个视图模板。基于 Razor 的视图模板的文件使用 *.cshtml* 作为其扩展名，并用 C# 优雅地输出 HTML。用 Razor 编写视图模板能减少字符的个数和敲击键盘的次数，并使工作流程快速灵活。

目前，控制器类中的 `Index` 方法返回的是一串硬编码的字符串。按下面的代码所示，修改 `Index` 方法使其返回视图对象：

[!code-csharp[Main](../../tutorials/first-mvc-app/start-mvc/sample/MvcMovie/Controllers/HelloWorldController.cs?name=snippet_4)]

上例中 `Index` 方法用一个视图模板生成 HTML 响应给浏览器。控制器方法 （也称为 Action 方法），比如上面的 ``Index`` 方法，通常返回 `IActionResult`（或者派生自 `ActionResult`的类），而不是字符串那样的基元类型。

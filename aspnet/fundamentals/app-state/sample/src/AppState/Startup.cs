using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AppState.Model;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace AppState
{
    public class Startup
    {
        // 关于如何配置应用程序的更多信息，请访问 http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });
        }

        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory)
        {
            
            loggerFactory.AddConsole(LogLevel.Debug);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async(context,next) => 
            {                
                context.Response.ContentType = "text/html;charset=utf-8";
                await next.Invoke();
            });

            // 不计入获取网站图标的请求
            app.Map("/favicon.ico", ignore => { });

            // 一个配置于 app.UseSession() 之前，完全不使用 session 的中间件的例子
            app.Map("/untracked", subApp =>
            {
                subApp.Run(async context =>
                {
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("请求时间: " + DateTime.Now.ToString() + "<br>");
                    await context.Response.WriteAsync("应用程序的这个目录没有使用 Session ...<br><a href=\"/\">返回</a>");
                    await context.Response.WriteAsync("</body></html>");
                });
            });

            app.UseSession();

            // 建立会话
            app.Map("/session", subApp =>
            {
                subApp.Run(async context =>
                {
                    // 把下面这行取消注释，并且清除 cookie ，在响应开始之后再存取会话时，就会产生错误
                    // await context.Response.WriteAsync("some content");
                    RequestEntryCollection collection = GetOrCreateEntries(context);
                    collection.RecordRequest(context.Request.PathBase + context.Request.Path);
                    SaveEntries(context, collection);
                    if (context.Session.GetString("StartTime") == null)
                    {
                        context.Session.SetString("StartTime", DateTime.Now.ToString());
                    }
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("统计: 你已经对本程序发起了"+ collection.TotalCount() +"次请求.<br><a href=\"/\">返回</a>");
                    await context.Response.WriteAsync("</body></html>");

                });
            });

            // 主要功能中间件
            app.Run(async context =>
            {
                RequestEntryCollection collection = GetOrCreateEntries(context);

                if (collection.TotalCount() == 0)
                {
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("你的会话尚未建立。<br>");
                    await context.Response.WriteAsync(DateTime.Now.ToString() + "<br>");
                    await context.Response.WriteAsync("<a href=\"/session\">建立会话</a>。<br>");
                }
                else
                {
                    collection.RecordRequest(context.Request.PathBase + context.Request.Path);
                    SaveEntries(context, collection);

                    // 注意：最好始终如一地在往响应流中写入内容之前执行完所有对会话的存取。
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("会话建立于： " + context.Session.GetString("StartTime") + "<br>");
                    foreach (var entry in collection.Entries)
                    {
                        await context.Response.WriteAsync("请求路径： " + entry.Path + " 被访问了 " + entry.Count + " 次。<br />");
                    }

                    await context.Response.WriteAsync("你的会话已找到, 你访问本站的次数是：" + collection.TotalCount() + "<br />");
                }
                await context.Response.WriteAsync("<a href=\"/untracked\">访问不计入统计的页面</a>.<br>");
                await context.Response.WriteAsync("</body></html>");
            });
        }

        private RequestEntryCollection GetOrCreateEntries(HttpContext context)
        {
            RequestEntryCollection collection = null;
            byte[] requestEntriesBytes = context.Session.Get("RequestEntries");

            if (requestEntriesBytes != null && requestEntriesBytes.Length > 0)
            {
                string json = System.Text.Encoding.UTF8.GetString(requestEntriesBytes);
                return JsonConvert.DeserializeObject<RequestEntryCollection>(json);
            }
            if (collection == null)
            {
                collection = new RequestEntryCollection();
            }
            return collection;
        }

        private void SaveEntries(HttpContext context, RequestEntryCollection collection)
        {
            string json = JsonConvert.SerializeObject(collection);
            byte[] serializedResult = System.Text.Encoding.UTF8.GetBytes(json);

            context.Session.Set("RequestEntries", serializedResult);
        }

    }
}

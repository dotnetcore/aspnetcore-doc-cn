﻿using System;
using AppState.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AppState
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Debug);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Use(async (context, next) =>
            {
                context.Items["entry_time"] = DateTime.Now;
                await next.Invoke();
            });

            // don't count favicon requests
            app.Map("/favicon.ico", ignore => { });

            // example middleware that does not reference session at all and is configured before app.UseSession()
            app.Map("/untracked", subApp =>
            {
                subApp.Run(async context =>
                {
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("Requested at: " + DateTime.Now.ToString("hh:mm:ss.ffff") + "<br>");
                    await context.Response.WriteAsync("This part of the application isn't referencing Session...<br><a href=\"/\">Return</a>");
                    await context.Response.WriteAsync("</body></html>");
                });
            });

            app.UseSession();

            // establish session
            app.Map("/session", subApp =>
            {
                subApp.Run(async context =>
                {
                    // uncomment the following line and delete session coookie to generate an error due to session access after response has begun
                    // await context.Response.WriteAsync("some content");
                    RequestEntryCollection collection = GetOrCreateEntries(context);
                    collection.RecordRequest(context.Request.PathBase + context.Request.Path);
                    SaveEntries(context, collection);
                    if (context.Session.GetString("StartTime") == null)
                    {
                        context.Session.SetString("StartTime", DateTime.Now.ToString());
                    }

                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync($"Counting: You have made {collection.TotalCount()} requests to this application.<br><a href=\"/\">Return</a>");
                    await context.Response.WriteAsync("</body></html>");
                });
            });

            // main catchall middleware
            app.Run(async context =>
            {
                RequestEntryCollection collection = GetOrCreateEntries(context);

                if (collection.TotalCount() == 0)
                {
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("Your session has not been established.<br>");
                    await context.Response.WriteAsync(DateTime.Now.ToString() + "<br>");
                    await context.Response.WriteAsync("<a href=\"/session\">Establish session</a>.<br>");
                }
                else
                {
                    collection.RecordRequest(context.Request.PathBase + context.Request.Path);
                    SaveEntries(context, collection);

                    // Note: it's best to consistently perform all session access before writing anything to response
                    await context.Response.WriteAsync("<html><body>");
                    await context.Response.WriteAsync("Session Established At: " + context.Session.GetString("StartTime") + "<br>");
                    foreach (var entry in collection.Entries)
                    {
                        await context.Response.WriteAsync("Request: " + entry.Path + " was requested " + entry.Count + " times.<br />");
                    }

                    await context.Response.WriteAsync("Your session was located, you've visited the site this many times: " + collection.TotalCount() + "<br />");
                }
                await context.Response.WriteAsync("<a href=\"/untracked\">Visit untracked part of application</a>.<br>");
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

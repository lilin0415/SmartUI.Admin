﻿using System.Text;
using System.IO;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Business.AutoJob;
using YiSha.Admin.WebApi.Controllers;
using YiSha.Service.WebSocketManager;
using YiSha.Service.SignalRManager;

namespace YiSha.Admin.WebApi
{
    public enum WebSocketTypeEnum
    {
        WebSocket,
        SigalR
    }
    public class Startup
    {
        public IConfiguration Configuration { get; }

        WebSocketTypeEnum webSocketTypeEnum = WebSocketTypeEnum.SigalR;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "YiSha Api", Version = "v1" });
                options.IncludeXmlComments(xmlPath, true);
            });

            services.AddOptions();
            services.AddCors();
            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddMemoryCache();

            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(GlobalContext.HostingEnvironment.ContentRootPath + Path.DirectorySeparatorChar + "DataProtection"));

            if (webSocketTypeEnum == WebSocketTypeEnum.SigalR)
            {
                //services.AddSignalRCore();
                services.AddSingleton<ClientHub>();
                services.AddSignalR();
            }
            else
            {
                services.AddWebSocketManager();
            }
            

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
            GlobalContext.Services = services;
            GlobalContext.Configuration = Configuration;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            FileHelper.CreateDirectory(resource);

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = GlobalContext.SetCacheControl
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                RequestPath = "/Resource",
                FileProvider = new PhysicalFileProvider(resource),
                OnPrepareResponse = GlobalContext.SetCacheControl
            });

            app.UseMiddleware(typeof(GlobalExceptionMiddleware));

            app.UseCors(builder =>
            {
                builder.WithOrigins(GlobalContext.SystemConfig.AllowCorsSite.Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            });
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "api-doc/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "api-doc";
                c.SwaggerEndpoint("v1/swagger.json", "YiSha Api v1");
            });
            app.UseRouting();

            if (webSocketTypeEnum == WebSocketTypeEnum.WebSocket)
            {
                var webSocketOptions = new WebSocketOptions()
                {
                    KeepAliveInterval = TimeSpan.FromSeconds(120),  //向客户端发送“ping”帧的频率，以确保代理保持连接处于打开状态
                    ReceiveBufferSize = 4 * 1024   //用于接收数据的缓冲区的大小。 只有高级用户才需要对其进行更改，以便根据数据大小调整性能。
                };
                app.UseWebSockets(webSocketOptions);
                //app.MapWebSocketManager("/zhajinhua", serviceProvider.GetService<ZjhGame>());
            }
            else
            {
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ClientHub>("/clienthub");
                });
                
            }
         

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=ApiHome}/{action=Index}/{id?}");
            });
            GlobalContext.ServiceProvider = app.ApplicationServices;
            if (!GlobalContext.SystemConfig.Debug)
            {
                new JobCenter().Start(); // 定时任务
            }
        }
    }
}
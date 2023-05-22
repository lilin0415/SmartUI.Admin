using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Util;
using YiSha.Util.Model;
using YiSha.Admin.Web.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using YiSha.Service.SignalRManager;
using YiSha.Service.WebSocketManager;
using YiSha.Business.AutoJob;

namespace YiSha.Admin.Web
{
    public enum WebSocketTypeEnum
    {
        WebSocket,
        SigalR
    }

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment WebHostEnvironment { get; }

        WebSocketTypeEnum webSocketTypeEnum = WebSocketTypeEnum.SigalR;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            WebHostEnvironment = env;
            GlobalContext.LogWhenStart(env);
            GlobalContext.HostingEnvironment = env;
            GlobalContext.Configuration = Configuration;
            GlobalContext.SystemConfig = Configuration.GetSection("SystemConfig").Get<SystemConfig>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            GlobalContext.Services = services;

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = SystemConfig.ProductName +" Api",
                    Version = "v1",
                    Description = "",
                    TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "John Walkner",
                    //    Email = "John.Walkner@gmail.com",
                    //    Url = new Uri("https://twitter.com/jwalkner"),
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Employee API LICX",
                    //    Url = new Uri("https://example.com/license"),
                    //}
                });
              
                //// Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //options.IncludeXmlComments(xmlPath);

                //var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                //options.IncludeXmlComments(xmlPath, true);
            });
            services.AddSwaggerGenNewtonsoftSupport();

            if (WebHostEnvironment.IsDevelopment())
            {
                services.AddRazorPages().AddRazorRuntimeCompilation();
            }
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();

            services.AddOptions();
            services.AddCors();


            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddControllers(options =>
            {
                options.ModelMetadataDetailsProviders.Add(new ModelBindingMetadataProvider());
            }).AddNewtonsoftJson(options =>
            {
                // 返回数据首字母不小写，CamelCasePropertyNamesContractResolver是小写
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            var dataProtectionDir = GlobalContext.GetAppData("DataProtection");
            if (!Directory.Exists(dataProtectionDir))
            {
                Directory.CreateDirectory(dataProtectionDir);
            }

			services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(dataProtectionDir));

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);  // 注册Encoding

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
           
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceProvider = app.ApplicationServices;

            if (!string.IsNullOrEmpty(GlobalContext.SystemConfig.VirtualDirectory))
            {
                app.UsePathBase(new PathString(GlobalContext.SystemConfig.VirtualDirectory)); // 让 Pathbase 中间件成为第一个处理请求的中间件， 才能正确的模拟虚拟路径
            }

            if (WebHostEnvironment.IsDevelopment())
            {
                GlobalContext.SystemConfig.Debug = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler($"/{SystemConfig.BackendRouter}/Error");
            }
            //GlobalContext.SystemConfig.Debug = false;

            string resource = Path.Combine(env.ContentRootPath, "Resource");
            BizFileHelper.CreateDirectory(resource);

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
                c.SwaggerEndpoint("/api-doc/v1/swagger.json", SystemConfig.ProductName+" v1");
            });

            app.UseSession();
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
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Member}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            GlobalContext.ServiceProvider = app.ApplicationServices;

            //if (!GlobalContext.SystemConfig.Debug)
            {
                new JobCenter().Start(); // 定时任务
            }
        }
    }
}
